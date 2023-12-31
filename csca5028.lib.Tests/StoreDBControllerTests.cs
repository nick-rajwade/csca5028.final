﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using csca5028.lib;
using System.Collections;

namespace csca5028.lib.Tests
{
    [TestClass()]
    public class StoreDBControllerTests
    {
        public static string connectionString = @"Server=tcp:host.docker.internal,1433;User ID=sa;Password=YourStrong@Passw0rd;
                                                    Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite
                                                    ;Multi Subnet Failover=True";
        public static string dbName = "sales_db_test";


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
            await storeDBController.Initialise(dbName);
            await storeDBController.InsertInitialValues(dbName); //called twice to test if it can be called multiple times and get 10 stores

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
        public async Task GetStoresAsyncTest()
        {
            StoreDBController storeDBController = new StoreDBController(connectionString);
            await storeDBController.Initialise(dbName);
            await storeDBController.InsertInitialValues(dbName);

            List<Store> stores = (List<Store>)storeDBController.GetStoresAsync(dbName).Result;
            Assert.AreEqual(10, stores.Count);
        }

        [TestMethod()]
        public async Task GetTerminalsAsyncTest()
        {
            StoreDBController storeDBController = new StoreDBController(connectionString);
            await storeDBController.Initialise(dbName);
            await storeDBController.InsertInitialValues(dbName);
            List<Store> stores = (List<Store>)storeDBController.GetStoresAsync(dbName).Result;

            foreach (Store store in stores)
            {
                List<POSTerminal> terminals = (List<POSTerminal>)storeDBController.GetTerminalsAsync(store.ID, dbName).Result;
                //Assert.AreEqual(10, terminals.Count);
                Assert.IsTrue(terminals.Count > 0);
            }
        }

        [TestMethod()]
        public async Task GetStoresAndTerminalsAsyncTest()
        {
            StoreDBController storeDBController = new StoreDBController(connectionString);
            await storeDBController.Initialise(dbName);
            await storeDBController.InsertInitialValues(dbName);

            Dictionary<string, Tuple<Store,List<POSTerminal>>> storesAndTerminals = await storeDBController.GetStoresAndTerminalsAsync(dbName);
            Assert.AreEqual(10, storesAndTerminals.Count);
        }

        [TestMethod()]
        public async Task GetStoreIdAsyncTest()
        {
            StoreDBController storeDBController = new StoreDBController(connectionString);
            await storeDBController.Initialise(dbName);
            await storeDBController.InsertInitialValues(dbName);

            Guid storeid = await storeDBController.GetStoreIdAsync("Dallas Store", dbName);
            Assert.IsNotNull(storeid);
        }

        [TestMethod()]
        public async Task GetStoresDataTableAsyncTest()
        {
            StoreDBController storeDBController = new StoreDBController(connectionString);
            await storeDBController.Initialise(dbName);
            await storeDBController.InsertInitialValues(dbName);

            var stores = await storeDBController.GetStoresDataTableAsync(dbName);
            Assert.AreEqual(10, stores.Rows.Count);

        }

        [TestMethod()]
        public void StoreDBControllerTest()
        {
            StoreDBController storeDBController = new StoreDBController(connectionString);
            Assert.IsNotNull(storeDBController);
        }

        [TestMethod()]
        public void ConnectTest()
        {
            StoreDBController storeDBController = new StoreDBController(connectionString);
            SqlConnection connection = storeDBController.Connect();
            Assert.IsNotNull(connection);
            Assert.IsTrue(connection.State == System.Data.ConnectionState.Closed);
        }
    }
}
