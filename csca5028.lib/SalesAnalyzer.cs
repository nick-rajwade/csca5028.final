using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csca5028.lib
{
    public class SalesAnalyzer : ISalesAnalyzer, IDisposable
    {
        private string connectionString = "Server=tcp:host.docker.internal,1433;User ID=sa;Password=YourStrong@Passw0rd;Connect Timeout=15;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=True";
        private string dbName = "sales_db";
        //calculate the total sales performance for all stores

        public SalesAnalyzer()
        {
        }

        public async Task<Dictionary<string, Tuple<int, decimal>>> LoadSalesPerformance()
        {
            SalesDBController salesDBController = new SalesDBController(connectionString);

            var storeSalesPerf = await salesDBController.GetSalesPerformanceForTimeInterval(dbName, 5); //look at last 5 minutes only
            return storeSalesPerf;
        }

        public async Task<decimal> GetTotalRevenueForTimeInterval()
        {
            decimal totalSalesPerformance = 0;
            var storeSalesPerf = await LoadSalesPerformance();

            for (int i = 0; i < storeSalesPerf.Count; i++)
            {
                totalSalesPerformance += storeSalesPerf.ElementAt(i).Value.Item2;
            }

            return totalSalesPerformance;
        }

        public async Task<int> GetTransactionsPerMinuteAcrossAllStores()
        {
            int transactionsPerMinute = 0;
            var storeSalesPerf = await LoadSalesPerformance();
            for (int i = 0; i < storeSalesPerf.Count; i++)
            {
                transactionsPerMinute += storeSalesPerf.ElementAt(i).Value.Item1;
            }

            return transactionsPerMinute;
        }

        public async Task<decimal> GetRevenueForStore(string storeName)
        {
            decimal revenueForStore = 0;
            var storeSalesPerf = await LoadSalesPerformance();
            revenueForStore = storeSalesPerf[storeName].Item2;
            return revenueForStore;
        }

        public async Task<int> GetTransactionsPerMinuteForStore(string storeName)
        {
            int transactionsPerMinuteForStore = 0;
            var storeSalesPerf = await LoadSalesPerformance();
            transactionsPerMinuteForStore = storeSalesPerf[storeName].Item1;
            return transactionsPerMinuteForStore;
        }

        public void Dispose()
        {

        }

        public Task SetSalesPerformance(Dictionary<string, Tuple<int, decimal>> salesPerformance)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, Tuple<decimal, decimal>>> GetStoreLocations()
        {
            StoreDBController storeDBController = new StoreDBController(connectionString);
            Hashtable storeAndTerminals = await storeDBController.GetStoresAndTerminalsAsync(dbName);
            Dictionary<string, Tuple<decimal, decimal>> storeLocations = new();
            foreach (Store store in storeAndTerminals.Keys)
            {
                storeLocations.Add(store.Name, new Tuple<decimal, decimal>(store.StoreLocation.Lat, store.StoreLocation.Long));
            }
            return storeLocations;
        }

        public Task SetStoreLocations(Dictionary<string, Tuple<decimal, decimal>> storeLocations)
        {
            throw new NotImplementedException();
        }

        public async Task<Dictionary<string, Tuple<Store, List<POSTerminal>>>> GetStoresAndTerminals()
        {
            StoreDBController storeDBController = new StoreDBController(connectionString);
            Hashtable _storeAndTerminals = await storeDBController.GetStoresAndTerminalsAsync(dbName);
            Dictionary<string, Tuple<Store, List<POSTerminal>>> storesAndTerminals = new();
            foreach (Store store in _storeAndTerminals.Keys)
            {
                storesAndTerminals.Add(store.Name, new Tuple<Store, List<POSTerminal>>(store, (List<POSTerminal>)_storeAndTerminals[store]));
            }
            return storesAndTerminals;
        }

        public async Task SetStoresAndTerminals(Dictionary<string, Tuple<Store, List<POSTerminal>>> storesAndTerminals)
        {
            throw new NotImplementedException();
        }

        public async Task<decimal> GetTotalSalesRevenue()
        {
            SalesDBController salesDBController = new SalesDBController(connectionString);
            return await salesDBController.GetTotalSalesRevenue(dbName);
        }

        public Task SetTotalSalesRevenue(decimal totalSalesRevenue)
        {
            throw new NotImplementedException();
        }
    }
}
