using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;

namespace components
{
    public class StoreDBController
    {
        private string connectionString = "Data Source=localhost;User ID=sa;Password=YourStrong@Passw0rd;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public StoreDBController(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public SqlConnection Connect()
        {
            return new SqlConnection(connectionString);
        }

        public async Task Initialise(string dbName)
        {
            if (dbName.IsNullOrEmpty())
            {
                throw new ArgumentException("dbName cannot be null or empty");
            }
            await CreateDatabase(dbName);
            await CreateTables(dbName);
            await InsertInitialValues(dbName);
        }

        public async Task CreateDatabase(string dbName)
        {
            if(dbName.IsNullOrEmpty())
            { 
                throw new ArgumentException("dbName cannot be null or empty");
            }
            using (var connection = Connect())
            {
                await connection.OpenAsync();
                var sql = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{dbName}') CREATE DATABASE [{dbName}];";
                using(var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task CreateTables(string dbName)
        {
            if (dbName.IsNullOrEmpty())
            {
                throw new ArgumentException("dbName cannot be null or empty");
            }
            using (var connection = Connect())
            {
                
                await connection.OpenAsync();
                var sql = $@"USE {dbName} IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='StoreLocation' and xtype='U') 
                            CREATE TABLE [{dbName}].[dbo].[StoreLocation]
                            ([Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                            store_address VARCHAR(100) NOT NULL,
                            store_city VARCHAR(50) NOT NULL,
                            store_state VARCHAR(50) NOT NULL,
                            store_zip VARCHAR(10) NOT NULL,
                            store_country VARCHAR(50) NOT NULL,
                            lat DECIMAL(9,6) NOT NULL,
                            long DECIMAL(9,6) NOT NULL);";
                
                using(var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    await command.ExecuteNonQueryAsync();
                }

                sql = $@"USE {dbName} IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Stores' and xtype='U')
                            CREATE TABLE [{dbName}].[dbo].[Stores](
                            [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	                        store_name NVARCHAR(50) NOT NULL,
	                        healthcheckurl NVARCHAR(200) NOT NULL,
	                        healthcheckinterval INT NOT NULL,
	                        store_location_id INT NOT NULL, -- FK to StoreLocation.Id
	                        CONSTRAINT FK_Stores_StoreLocation FOREIGN KEY (store_location_id) REFERENCES StoreLocation(Id));";
                using(var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task InsertInitialValues(string dbName)
        {
            if (dbName.IsNullOrEmpty())
            {
                throw new ArgumentException("dbName cannot be null or empty");
            }
            using (var connection = Connect())
            {
                await connection.OpenAsync();
                var sql = $"select * from [{dbName}].[dbo].StoreLocation;";
                bool storeLocationHasValues = true;
                using(var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        storeLocationHasValues = reader.HasRows;
                        
                    }
                }

                bool storesHasValues = true;
                sql = $"select * from [{dbName}].[dbo].Stores;";
                using(var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        storesHasValues = reader.HasRows;
                    }
                }


                if (!storeLocationHasValues)
                {
                    sql = $@"INSERT INTO [{dbName}].[dbo].[StoreLocation] (store_address, store_city, store_state, store_zip, store_country, lat, long)
                            VALUES ('123 Main St', 'New York', 'NY', '10001', 'USA', 40.7128, -74.0060),
                            ('456 Main St', 'Los Angeles', 'CA', '90001', 'USA', 34.0522, -118.2437),
                            ('789 Main St', 'Chicago', 'IL', '60007', 'USA', 41.8781, -87.6298),
                            ('101 Main St', 'Houston', 'TX', '77001', 'USA', 29.7604, -95.3698),
                            ('112 Main St', 'Phoenix', 'AZ', '85001', 'USA', 33.4484, -112.0740),
                            ('131 Main St', 'Philadelphia', 'PA', '19019', 'USA', 39.9526, -75.1652),
                            ('415 Main St', 'San Antonio', 'TX', '78201', 'USA', 29.4241, -98.4936),
                            ('161 Main St', 'San Diego', 'CA', '92101', 'USA', 32.7157, -117.1611),
                            ('718 Main St', 'Dallas', 'TX', '75201', 'USA', 32.7767, -96.7970),
                            ('919 Main St', 'San Jose', 'CA', '95101', 'USA', 37.3382, -121.8863);";
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = sql;
                        await command.ExecuteNonQueryAsync();
                    }
                }

                if (!storesHasValues)
                {
                    sql = $@"INSERT INTO [{dbName}].[dbo].[Stores] (store_name, healthcheckurl, healthcheckinterval, store_location_id)
                        VALUES ('New York Store', 'new-york-pos', 5, 1),
                        ('Los Angeles Store', 'la-pos', 5, 2),
                        ('Chicago Store', 'chicago-pos', 5, 3),
                        ('Houston Store', 'houston-pos', 5, 4),
                        ('Phoenix Store', 'phoenix-pos', 5, 5),
                        ('Philadelphia Store', 'philly-pos', 5, 6),
                        ('San Antonio Store', 'sanantonio-pos', 5, 7),
                        ('San Diego Store', 'sandiego-pos', 5, 8),
                        ('Dallas Store', 'dallas-pos', 5, 9),
                        ('San Jose Store', 'sanjose-pos', 5, 10);";
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = sql;
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public async Task<IEnumerable<Store>> GetStoresAsync(string dbName)
        {
            using(var connection = Connect())
            {
                await connection.OpenAsync();
                //fetch all fields from store and store location tables
                var sql = $"select * from [{dbName}].[dbo].Stores s inner join [{dbName}].[dbo].StoreLocation sl on s.store_location_id = sl.Id;";
                using(SqlCommand  command = connection.CreateCommand()) 
                { 
                    command.CommandText = sql;
                    using(SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        var stores = new List<Store>();
                        while (reader.Read())
                        {
                            var store = new Store();
                            store.ID = Guid.Parse(reader["Id"].ToString());
                            store.Name = reader["store_name"].ToString();
                            store.HealthCheckURL = reader["healthcheckurl"].ToString();
                            store.HealthCheckInterval = int.Parse(reader["healthcheckinterval"].ToString());
                            store.StoreLocation = new StoreLocation();
                            store.StoreLocation.Id = int.Parse(reader["store_location_id"].ToString());
                            store.StoreLocation.Address = reader["store_address"].ToString();
                            store.StoreLocation.City = reader["store_city"].ToString();
                            store.StoreLocation.State = reader["store_state"].ToString();
                            store.StoreLocation.Zip = reader["store_zip"].ToString();
                            store.StoreLocation.Country = reader["store_country"].ToString();
                            store.StoreLocation.Lat = decimal.Parse(reader["lat"].ToString());
                            store.StoreLocation.Long = decimal.Parse(reader["long"].ToString());
                            stores.Add(store);
                        }
                        return stores;
                    }
                }
                
                
            }
        }
    }
}
