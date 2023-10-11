using Microsoft.VisualStudio.TestTools.UnitTesting;
using csca5028.lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csca5028.lib.Tests
{
    [TestClass()]
    public class SalesAnalyzerTests
    {
        private static string dbName = "sales_db_test";
        private static string connectionString = "Server=tcp:host.docker.internal,1433;User ID=sa;Password=YourStrong@Passw0rd;Connect Timeout=15;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=True";

        [TestMethod()]
        public async Task GetTotalRevenueForTimeIntervalTest()
        {
            SalesAnalyzer salesAnalyzer = new SalesAnalyzer(dbName, connectionString);
            decimal totalSalesPerformance = await salesAnalyzer.GetTotalRevenueForTimeInterval();

            Assert.IsTrue(totalSalesPerformance >= 0);
        }

        [TestMethod()]
        public async Task LoadSalesPerformanceTest()
        {
            SalesAnalyzer salesAnalyzer = new SalesAnalyzer(dbName, connectionString);
            var storeSalesPerf = await salesAnalyzer.LoadSalesPerformance();
            Assert.IsNotNull(storeSalesPerf);
            Assert.IsTrue(storeSalesPerf.Count <= 10);
        }

        [TestMethod()]
        public async Task GetTransactionsPerMinuteAcrossAllStoresTest()
        {
            SalesAnalyzer salesAnalyzer = new SalesAnalyzer(dbName, connectionString);
            int transactionsPerMinute = await salesAnalyzer.GetTransactionsPerMinuteAcrossAllStores();
            Assert.IsTrue(transactionsPerMinute >= 0);
        }

        [TestMethod()]
        public async Task GetRevenueForStoreTest()
        {
            StoreDBController storeDBController = new StoreDBController(connectionString);
            SalesDBController salesDBController = new SalesDBController(connectionString);
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

            SalesAnalyzer salesAnalyzer = new SalesAnalyzer(dbName, connectionString);
            decimal revenueForStore = await salesAnalyzer.GetRevenueForStore("New York Store");
            Assert.IsTrue(revenueForStore >= 0);
        }

        [TestMethod()]
        public async Task GetTransactionsPerMinuteForStoreTest()
        {
            SalesAnalyzer salesAnalyzer = new SalesAnalyzer(dbName, connectionString);
            int transactionsPerMinute = await salesAnalyzer.GetTransactionsPerMinuteForStore("Dallas Store");
            Assert.IsTrue(transactionsPerMinute >= 0);
        }

        [TestMethod()]
        public async Task SetSalesPerformanceTest()
        {
            SalesAnalyzer salesAnalyzer = new SalesAnalyzer(dbName, connectionString);
            Dictionary<string, Tuple<int, decimal>> salesPerformance = new Dictionary<string, Tuple<int, decimal>>();
            salesPerformance.Add("New York Store", new Tuple<int, decimal>(10, 100.0M));
            salesPerformance.Add("Dallas Store", new Tuple<int, decimal>(20, 200.0M));
            salesPerformance.Add("Chicago Store", new Tuple<int, decimal>(30, 300.0M));
            await salesAnalyzer.SetSalesPerformance(salesPerformance);
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public async Task GetStoreLocationsTest()
        {
            SalesAnalyzer salesAnalyzer = new SalesAnalyzer(dbName, connectionString);
            var storeLocations = await salesAnalyzer.GetStoreLocations();
            Assert.IsNotNull(storeLocations);
            Assert.IsTrue(storeLocations.Count == 10);
        }

        [TestMethod()]
        public async Task SetStoreLocationsTest()
        {
            SalesAnalyzer salesAnalyzer = new SalesAnalyzer(dbName, connectionString);
            Dictionary<string, Tuple<decimal, decimal>> storeLocations = new Dictionary<string, Tuple<decimal, decimal>>();
            storeLocations.Add("New York Store", new Tuple<decimal, decimal>(40.0M, 40.0M));
            storeLocations.Add("Dallas Store", new Tuple<decimal, decimal>(50.0M, 50.0M));
            storeLocations.Add("Chicago Store", new Tuple<decimal, decimal>(60.0M, 60.0M));
            await salesAnalyzer.SetStoreLocations(storeLocations);
            Assert.IsTrue(true);

        }

        [TestMethod()]
        public async Task GetStoresAndTerminalsTest()
        {
            SalesAnalyzer salesAnalyzer = new SalesAnalyzer(dbName, connectionString);
            var storesAndTerminals = await salesAnalyzer.GetStoresAndTerminals();
            Assert.IsNotNull(storesAndTerminals);
            Assert.IsTrue(storesAndTerminals.Count == 10);
        }

        [TestMethod()]
        public async Task SetStoresAndTerminalsTest()
        {
            SalesAnalyzer salesAnalyzer = new SalesAnalyzer(dbName, connectionString);
            StoreDBController storeDBController = new StoreDBController(connectionString);
            await storeDBController.Initialise(dbName);
            Dictionary<string, Tuple<Store, List<POSTerminal>>> storesAndTerminals = await storeDBController.GetStoresAndTerminalsAsync(dbName);
            await salesAnalyzer.SetStoresAndTerminals(storesAndTerminals);
            Assert.IsTrue(true);
                

        }

        [TestMethod()]
        public async Task GetTotalSalesRevenueTest()
        {
            SalesAnalyzer salesAnalyzer = new SalesAnalyzer(dbName, connectionString);
            decimal totalSalesRevenue = await salesAnalyzer.GetTotalSalesRevenue();
            Assert.IsTrue(totalSalesRevenue >= 0);
        }

        [TestMethod()]
        public async Task SetTotalSalesRevenueTest()
        {
            SalesAnalyzer salesAnalyzer = new SalesAnalyzer(dbName, connectionString);
            await salesAnalyzer.SetTotalSalesRevenue(1000.0M);
            Assert.IsTrue(true);
        }
    }
}