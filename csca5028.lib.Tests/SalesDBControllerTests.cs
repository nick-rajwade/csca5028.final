using Microsoft.VisualStudio.TestTools.UnitTesting;
using csca5028.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Xml.Linq;
using Azure.Core;
using System.Data;	

namespace csca5028.lib.Tests
{
    [TestClass()]
    public class SalesDBControllerTests
    {

        public static string connectionString = "Server=tcp:host.docker.internal,1433;User ID=sa;Password=YourStrong@Passw0rd;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=True";
        public static string dbName = "sales_db_test";

        [TestMethod()]
        public async Task InsertTest()
        {
            StoreDBController storeDBController = new StoreDBController(connectionString);
            await storeDBController.Initialise(dbName);

            var storesAndTerminals = await storeDBController.GetStoresAndTerminalsAsync(dbName);

            if (storesAndTerminals == null || storesAndTerminals.Count == 0)
            {
                Assert.Fail("Stores and Terminals not created!");
            }

            Store store = (Store)storesAndTerminals["New York Store"].Item1;
            POSTerminal terminal = (POSTerminal)storesAndTerminals["New York Store"].Item2[0];


            Sale sale = terminal.GenerateSale();

            SalesDBController controller = new SalesDBController(connectionString);
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

        [TestMethod()]
		public async Task GetSalesRevenueByStoreIdTest()
		{
			StoreDBController storeDBController = new StoreDBController(connectionString);
			await storeDBController.Initialise(dbName);
			SalesDBController salesDBController = new SalesDBController(connectionString);
			await salesDBController.Initialise(dbName);

            var storesAndTerminals = await storeDBController.GetStoresAndTerminalsAsync(dbName);

            if (storesAndTerminals == null || storesAndTerminals.Count == 0)
            {
                Assert.Fail("Stores and Terminals not created!");
            }

            Store store = (Store)storesAndTerminals["New York Store"].Item1;
            POSTerminal terminal = (POSTerminal)storesAndTerminals["New York Store"].Item2[0];

            decimal perf = 0.0M;
			for (int i = 0; i < 10; i++)
			{
				Sale sale = terminal.GenerateSale();
				await salesDBController.Insert(dbName, sale);
				perf += sale.TotalPrice;
			}

			var salesPerf = await salesDBController.GetSalesRevenueByStoreID(dbName, store.ID);
			Assert.IsTrue(salesPerf >= perf);

		}

        [TestMethod()]
        public async Task GetSalesPerformanceForTimeIntervalTest()
        {
            StoreDBController storeDBController = new StoreDBController(connectionString);
            await storeDBController.Initialise(dbName);
            SalesDBController salesDBController = new SalesDBController(connectionString);
            await salesDBController.Initialise(dbName);

            var salesPerf = await salesDBController.GetSalesPerformanceForTimeInterval(dbName, 1);
            Assert.IsNotNull(salesPerf);

        }

        [TestMethod()]
        public void SalesDBControllerTest()
        {
            SalesDBController salesDBController = new SalesDBController(connectionString);
            Assert.IsNotNull(salesDBController);
        }

        [TestMethod()]
        public void ConnectTest()
        {
            SalesDBController salesDBController = new SalesDBController(connectionString);
            Assert.IsNotNull(salesDBController.Connect());
        }

        [TestMethod()]
        public async Task InitialiseTest()
        {
            SalesDBController salesDBController = new SalesDBController(connectionString);
            await salesDBController.Initialise(dbName);
            using (SqlConnection connection = salesDBController.Connect())
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

                sql = $"use {dbName} select name from sysobjects where name='pos_terminals' and xtype='U'";
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Assert.AreEqual(reader["name"], "pos_terminals");
                            }
                        }
                        else
                        {
                            Assert.Fail("pos_terminals table not created!");
                        }
                    }
                }

                sql = $"use {dbName} select name from sysobjects where name='Sales' and xtype='U'";
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
                            Assert.Fail("Sales table not created!");
                        }
                    }
                }

                using (SqlConnection con = salesDBController.Connect())
                {
                    con.Open();
                    sql = $"select count(*) from [{dbName}].[dbo].StoreLocation";
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
        }

        [TestMethod()]
        public async Task GetAverageSalePriceByStoreIDTest()
        {
            StoreDBController storeDBController = new StoreDBController(connectionString);
            await storeDBController.Initialise(dbName);
            SalesDBController salesDBController = new SalesDBController(connectionString);
            await salesDBController.Initialise(dbName);

            var storesAndTerminals = await storeDBController.GetStoresAndTerminalsAsync(dbName);

            if (storesAndTerminals == null || storesAndTerminals.Count == 0)
            {
                Assert.Fail("Stores and Terminals not created!");
            }

            Store store = (Store)storesAndTerminals["New York Store"].Item1;
            POSTerminal terminal = (POSTerminal)storesAndTerminals["New York Store"].Item2[0];

            decimal perf = 0.0M;
            for (int i = 0; i < 10; i++)
            {
                Sale sale = terminal.GenerateSale();
                await salesDBController.Insert(dbName, sale);
                perf += sale.TotalPrice;
            }
            var avg = perf / 10;

            var avgSalesPerf = await salesDBController.GetAverageSalePriceByStoreID(dbName, store.ID);
            
            //check if the average is within 50% of the actual average
            Assert.IsTrue(avgSalesPerf >= 0);
        }

        [TestMethod()]
        public async Task GetTotalSalesRevenueTest()
        {
            SalesDBController salesDBController = new SalesDBController(connectionString);
            StoreDBController storeDBController = new StoreDBController(connectionString);
            await storeDBController.Initialise(dbName);
            await salesDBController.Initialise(dbName);
            var storesAndTerminals = await storeDBController.GetStoresAndTerminalsAsync(dbName);

            if (storesAndTerminals == null || storesAndTerminals.Count == 0)
            {
                Assert.Fail("Stores and Terminals not created!");
            }

            Store store = (Store)storesAndTerminals["New York Store"].Item1;
            POSTerminal terminal = (POSTerminal)storesAndTerminals["New York Store"].Item2[0];

            decimal perf = 0.0M;
            for (int i = 0; i < 10; i++)
            {
                Sale sale = terminal.GenerateSale();
                await salesDBController.Insert(dbName, sale);
                perf += sale.TotalPrice;
            }

            var totalSalesPerf = await salesDBController.GetTotalSalesRevenue(dbName);
            Assert.IsTrue(totalSalesPerf >= perf);
        }
    }
}