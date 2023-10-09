using System.Collections;

namespace csca5028.lib
{
    public interface ISalesAnalyzer
    {
        public Task<Dictionary<string, Tuple<int, decimal>>> LoadSalesPerformance();
        public Task SetSalesPerformance(Dictionary<string, Tuple<int, decimal>> salesPerformance);

        public Task<decimal> GetTotalRevenueForTimeInterval();
        public Task<int> GetTransactionsPerMinuteAcrossAllStores();
        public Task<decimal> GetRevenueForStore(string storeName);
        public Task<int> GetTransactionsPerMinuteForStore(string storeName);
        
        public Task<Dictionary<string, Tuple<decimal, decimal>>> GetStoreLocations();
        public Task SetStoreLocations(Dictionary<string,Tuple<decimal, decimal>> storeLocations);
        
        public Task<Dictionary<string,Tuple<Store,List<POSTerminal>>>> GetStoresAndTerminals();
        public Task SetStoresAndTerminals (Dictionary<string, Tuple<Store, List<POSTerminal>>> storesAndTerminals);

        public Task<decimal> GetTotalSalesRevenue();
        public Task SetTotalSalesRevenue(decimal totalSalesRevenue);
    }
}
