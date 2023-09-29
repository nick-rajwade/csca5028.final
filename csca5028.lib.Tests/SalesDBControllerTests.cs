﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using csca5028.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Xml.Linq;

namespace csca5028.lib.Tests
{
    [TestClass()]
    public class SalesDBControllerTests
    {
        [TestMethod()]
        public async Task CreateDatabaseTest()
        {
            SalesDBController controller = new SalesDBController("Server=localhost,1433;User ID=sa;Password=YourStrong@Passw0rd;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=True");
            var dbName = "sales_test_db";
            await controller.CreateDatabase(dbName);
            using (SqlConnection connection = controller.Connect())
            {
                connection.Open();
                var sql = $"select name from sys.databases where name = '{dbName}'";
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Assert.AreEqual(reader["name"], dbName);
                            }

                        }
                        else
                        {
                            Assert.Fail($"{dbName} not created!");
                        }
                    }
                }
            }
        }

        [TestMethod()]
        public async Task CreateTablesTest()
        {
            SalesDBController controller = new SalesDBController("Server=localhost,1433;User ID=sa;Password=YourStrong@Passw0rd;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=True");
            var dbName = "sales_test_db";
            await controller.CreateDatabase(dbName);
            await controller.CreateTables(dbName);
            using (SqlConnection connection = controller.Connect())
            {
                connection.Open();
                var sql = $"use {dbName} select name from sysobjects where name='Sales' and xtype='U'";
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Assert.AreEqual(reader["name"], "Sales");
                            }

                        }
                        else
                        {
                            Assert.Fail($"Sales table not created!");
                        }
                    }
                }
            }
        }

        [TestMethod()]
        public async Task InsertTest()
        {
            Store store = new Store("New York Store", new StoreLocation("123 Main St", "New York", "NY", "10001", "USA", decimal.Parse("40.7128"), decimal.Parse("-74.0060")),
                new Guid("00000000-0000-0000-0000-000000000001"), "new-york-pos");
            Sale sale = store.GenerateSale();
            SalesDBController controller = new SalesDBController("Server=localhost,1433;User ID=sa;Password=YourStrong@Passw0rd;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=True");
            var dbName = "sales_test_db";
            await controller.CreateDatabase(dbName);
            await controller.CreateTables(dbName);
            await controller.Insert(dbName, sale);
            using (SqlConnection connection = controller.Connect())
            {
                connection.Open();
                var sql = $"use {dbName} select * from Sales where saleID = '{sale.ID}'";
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Assert.AreEqual(reader["saleID"], sale.ID);
                                Assert.AreEqual(reader["store_id"], sale.StoreID);
                                Assert.AreEqual(reader["json_item_list"], sale.ItemsAsJson);
                                Assert.AreEqual(reader["total_items"], sale.TotalItems);
                                Assert.AreEqual(reader["total_price"], sale.TotalPrice);
                                Assert.AreEqual(reader["payment_type"], sale.paymentType.ToString());
                                Assert.AreEqual(reader["loyalty_card"], sale.loyaltyCard);
                            }
                        }
                        else
                        {
                            Assert.Fail($"Sale not inserted!");
                        }
                    }
                }
            }
        }
    }
}