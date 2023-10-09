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
        [TestMethod()]
        public async Task GetTotalRevenueForTimeIntervalTest()
        {
            SalesAnalyzer salesAnalyzer = new SalesAnalyzer();
            decimal totalSalesPerformance = await salesAnalyzer.GetTotalRevenueForTimeInterval();

            Assert.IsTrue(totalSalesPerformance > 0);
        }

        [TestMethod()]
        public async Task LoadSalesPerformanceTest()
        {
            SalesAnalyzer salesAnalyzer = new SalesAnalyzer();
            var storeSalesPerf = await salesAnalyzer.LoadSalesPerformance();
            Assert.IsNotNull(storeSalesPerf);
            Assert.IsTrue(storeSalesPerf.Count == 10);
        }

        [TestMethod()]
        public async Task GetTransactionsPerMinuteAcrossAllStoresTest()
        {
            SalesAnalyzer salesAnalyzer = new SalesAnalyzer();
            int transactionsPerMinute = await salesAnalyzer.GetTransactionsPerMinuteAcrossAllStores();
            Assert.IsTrue(transactionsPerMinute >= 0);
        }

        [TestMethod()]
        public async Task GetRevenueForStoreTest()
        {
            SalesAnalyzer salesAnalyzer = new SalesAnalyzer();
            decimal revenueForStore = await salesAnalyzer.GetRevenueForStore("Dallas Store");
            Assert.IsTrue(revenueForStore >= 0);
        }

        [TestMethod()]
        public async Task GetTransactionsPerMinuteForStoreTest()
        {
            SalesAnalyzer salesAnalyzer = new SalesAnalyzer();
            int transactionsPerMinute = await salesAnalyzer.GetTransactionsPerMinuteForStore("Dallas Store");
            Assert.IsTrue(transactionsPerMinute >= 0);
        }
    }
}