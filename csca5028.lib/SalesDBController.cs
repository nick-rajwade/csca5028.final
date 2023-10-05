using csca5028.web;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csca5028.lib
{
    public class SalesDBController
    {
        private string connectionString = "Server=tcp:host.docker.internal,1433;Database=store_db;User ID=sa;Password=YourStrong@Passw0rd;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=True";

        public SalesDBController(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public SqlConnection Connect()
        {
            return new SqlConnection(connectionString);
        }

        public async Task Initialise(string dbName)
        {
            if(dbName.IsNullOrEmpty())
            {
                throw new ArgumentException("dbName cannot be null or empty");
            }
            await CreateDatabase(dbName);
            await CreateTables(dbName);
        }

        public async Task CreateDatabase(string dbName)
        {
            if (dbName.IsNullOrEmpty())
            {
                throw new ArgumentException("dbName cannot be null or empty");
            }

            using (var conn = Connect())
            {
                await conn.OpenAsync();
                var sql = $" IF NOT EXISTS (select * from sys.databases where name = '{dbName}') CREATE DATABASE [{dbName}]";
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task CreateTables(string dbName)
        {
            if (dbName.IsNullOrEmpty())
            {
                throw new ArgumentException("dbName cannot be null or empty");
            }

            using (var conn = Connect())
            {
                await conn.OpenAsync();

                var sql = @$"USE {dbName} 
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Sales' and xtype='U')
                    CREATE TABLE [dbo].[Sales](
                                	[saleId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	                                [store_id] UNIQUEIDENTIFIER NOT NULL,
	                                [loyalty_card] bit NOT NULL,
	                                [payment_type] varchar(10) NOT NULL CHECK (payment_type IN ('Cash', 'Creditcard', 'Check')),
	                                [total_items] INT NOT NULL,
	                                [total_price] DECIMAL(10,2) NOT NULL,
	                                [json_item_list] varchar(max) NOT NULL,
	                                [created_at] DATETIME NOT NULL,
	                                [updated_at] DATETIME NOT NULL DEFAULT GETDATE(),
	                                [CC_AUTH] varchar(7) NULL CHECK (CC_AUTH IN ('AUTH','DECLINE')),
	                                [CC_AUTH_CODE] varchar(10) NULL);";

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Insert(string dbName, Sale sale)
        {
            using (var conn = Connect()) 
            { 
                await conn.OpenAsync();
                var sql = @$"INSERT INTO [{dbName}].[dbo].[Sales] ([saleId], [store_id], [loyalty_card], [payment_type], [total_items], [total_price], [json_item_list], [created_at], [CC_AUTH], [CC_AUTH_CODE])
                    VALUES (@saleId, @store_id, @loyalty_card, @payment_type, @total_items, @total_price, @json_item_list, @created_at, @CC_AUTH, @CC_AUTH_CODE);";
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@saleId", sale.ID);
                    cmd.Parameters.AddWithValue("@store_id", sale.StoreID);
                    cmd.Parameters.AddWithValue("@loyalty_card", sale.loyaltyCard);
                    cmd.Parameters.AddWithValue("@payment_type", sale.paymentType.ToString());
                    cmd.Parameters.AddWithValue("@total_items", sale.TotalItems);
                    cmd.Parameters.AddWithValue("@total_price", sale.TotalPrice);
                    cmd.Parameters.AddWithValue("@json_item_list", sale.ItemsAsJson);
                    cmd.Parameters.AddWithValue("@created_at", sale.CreatedAt);
                    if(sale.CreditCardResponse != null)
                    {
                        if(sale.CreditCardResponse.ResponseType == CreditCardResponseTypes._0)
                        {
                            cmd.Parameters.AddWithValue("@CC_AUTH", "AUTH");
                            cmd.Parameters.AddWithValue("@CC_AUTH_CODE", sale.CreditCardResponse?.AuthCode);

                        }
                        else if(sale.CreditCardResponse.ResponseType == CreditCardResponseTypes._1)
                        {
                            cmd.Parameters.AddWithValue("@CC_AUTH", "DECLINE");
                            cmd.Parameters.AddWithValue("@CC_AUTH_CODE", sale.CreditCardResponse?.AuthCode);
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@CC_AUTH", DBNull.Value);
                        cmd.Parameters.AddWithValue("@CC_AUTH_CODE", DBNull.Value);
                    }

                    await cmd.ExecuteNonQueryAsync();
                }


            }
        }

        //from sales_db get all sales for a given store and calculate the sales performance
        public async Task<decimal> GetSalesRevenueByStoreID(string dbName, Guid storeId)
        {
            decimal salesPerformance = 0;
            using (var conn = Connect())
            {
                await conn.OpenAsync();
                var sql = @$"SELECT [total_price] FROM [{dbName}].[dbo].[Sales] WHERE [store_id] = @storeId AND (CC_AUTH IS NULL OR CC_AUTH <> 'DECLINE');";
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@storeId", storeId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            salesPerformance += reader.GetDecimal(0);
                        }
                    }
                }
            }
            return salesPerformance;
        }

        //Get average sale price for a given store
        public async Task<decimal> GetAverageSalePriceByStoreID(string dbName, Guid storeId)
        {
            decimal averageSalePrice = 0;
            using (var conn = Connect())
            {
                await conn.OpenAsync();
                var sql = @$"SELECT AVG([total_price]) FROM [{dbName}].[dbo].[Sales] WHERE [store_id] = @storeId AND (CC_AUTH IS NULL OR CC_AUTH <> 'DECLINE');";
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@storeId", storeId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            averageSalePrice = reader.GetDecimal(0);
                        }
                    }
                }
            }
            return averageSalePrice;
        }

        

    }
}
