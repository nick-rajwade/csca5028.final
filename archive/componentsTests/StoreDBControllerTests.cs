using Microsoft.VisualStudio.TestTools.UnitTesting;
using components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace components.Tests
{
    [TestClass()]
    public class StoreDBControllerTests
    {
        public static string connectionString = "Data Source=localhost;User ID=sa;Password=YourStrong@Passw0rd;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public static string dbName = "store_db_test";

        [TestMethod()]
        public async Task InitialiseTest()
        {
            StoreDBController storeDBController = new StoreDBController(connectionString);
            await storeDBController.Initialise(dbName);

            using (SqlConnection connection = storeDBController.Connect())
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

                sql = $"use {dbName} select name from sysobjects where name='StoreLocation' and xtype='U'";
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Assert.AreEqual(reader["name"], "StoreLocation");
                            }
                        }
                        else
                        {
                            Assert.Fail("StoreLocation table not created!");
                        }
                    }
                }

                sql = $"use {dbName} select name from sysobjects where name='Stores' and xtype='U'";
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Assert.AreEqual(reader["name"], "Stores");
                            }
                        }
                        else
                        {
                            Assert.Fail("Stores table not created!");
                        }
                    }
                }
            }


        }

        [TestMethod()]
        public async Task InsertInitialValuesTest()
        {
            StoreDBController storeDBController = new StoreDBController(connectionString);
            await storeDBController.InsertInitialValues(dbName);

            using (SqlConnection con = storeDBController.Connect())
            {
                con.Open();
                var sql = $"select count(*) from [{dbName}].[dbo].StoreLocation";
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = sql;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Assert.AreEqual(10, reader[0]);
                            }
                        }
                        else
                        {
                            Assert.Fail("StoreLocation table data incorrect.");
                        }
                    }
                }

                sql = $"select count(*) from [{dbName}].[dbo].Stores";
                using (SqlCommand command = con.CreateCommand())
                {
                    command.CommandText = sql;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Assert.AreEqual(10, reader[0]);
                            }
                        }
                        else
                        {
                            Assert.Fail("Stores table data incorrect.");
                        }
                    }
                }
            }
        }

        [TestMethod()]
        public void GetStoresAsyncTest()
        {
            StoreDBController storeDBController = new StoreDBController(connectionString);
            List<Store> stores = (List<Store>)storeDBController.GetStoresAsync(dbName).Result;
            Assert.AreEqual(10, stores.Count);
        }
    }
}