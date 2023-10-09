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

		/*[TestMethod()]
		public async Task CreateDatabaseTest()
		{
			SalesDBController controller = new SalesDBController(connectionString);

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
		}*/

		/*[TestMethod()]
		public async Task CreateTablesTest()
		{
			StoreDBController storeDBController = new StoreDBController(connectionString);
			SalesDBController controller = new SalesDBController(connectionString);
			await storeDBController.Initialise(dbName);
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
		}*/

		/*[TestMethod()]
		public async Task InsertTest()
		{
			Store store = new Store("New York Store", new StoreLocation("123 Main St", "New York", "NY", "10001", "USA", decimal.Parse("40.7128"), decimal.Parse("-74.0060")),
				new Guid("00000000-0000-0000-0000-000000000001"), "new-york-pos");
			POSTerminal terminal = new POSTerminal(store, 1, 1, 1, 1, 1, 1, 1, 1, 1);
			Sale sale = store.GenerateSale();
			SalesDBController controller = new SalesDBController(connectionString);
			StoreDBController storeDBController = new StoreDBController(connectionString);
			await storeDBController.Initialise(dbName);

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
		}*/

		/*[TestMethod()]
		public async Task GetSalesRevenueByStoreIdTest()
		{
			Store store = new Store("New York Store", new StoreLocation("123 Main St", "New York", "NY", "10001", "USA", decimal.Parse("40.7128"), decimal.Parse("-74.0060")),
				new Guid("00000000-0000-0000-0000-000000000001"), "new-york-pos");
			StoreDBController storeDBController = new StoreDBController(connectionString);
				await storeDBController.Initialise(dbName);


			SalesDBController salesDBController = new SalesDBController(connectionString);
			await salesDBController.Initialise(dbName);
			decimal perf = 0.0M;
			for (int i = 0; i < 10; i++)
			{
				Sale sale = store.GenerateSale();
				await salesDBController.Insert(dbName, sale);
				perf += sale.TotalPrice;
			}

			var salesPerf = await salesDBController.GetSalesRevenueByStoreID(dbName, store.ID);
			Assert.IsTrue(salesPerf >= perf);

		}*/

		

		[TestMethod()]
		public async Task GetSalesPerformanceForTimeIntervalTest()
		{
			StoreDBController storeDBController = new StoreDBController(connectionString);
			await storeDBController.Initialise(dbName);
			SalesDBController salesDBController = new SalesDBController(connectionString);
			await salesDBController.Initialise(dbName);

            var salesPerf = await salesDBController.GetSalesPerformanceForTimeInterval(dbName,1);
			Assert.IsNotNull(salesPerf);

		}
	}
}