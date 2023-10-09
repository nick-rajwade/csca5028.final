using csca5028.lib;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;

namespace csca5028.blazor.webapp.Data.SalesAnalyser
{
    public class SalesAnalyzerWithCache : ISalesAnalyzer, IDisposable
    {
        private readonly IMemoryCache _cache;

        public SalesAnalyzerWithCache(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public async Task<decimal> GetRevenueForStore(string storeName)
        {
            if(_cache.TryGetValue("SalesPerformance", out Dictionary<string, Tuple<int, decimal>> salesPerformance))
            {
                var revenueForStore = salesPerformance[storeName].Item2;
                return revenueForStore;
            }
            await Task.CompletedTask;
            return 0M;
        }

        public async Task<decimal> GetTotalRevenueForTimeInterval()
        {
            if(_cache.TryGetValue("SalesPerformance", out Dictionary<string, Tuple<int, decimal>> salesPerformance))
            {
                var totalRevenue = salesPerformance.Sum(s => s.Value.Item2);
                return totalRevenue;
            }
            await Task.CompletedTask;
            return 0M;
        }

        public async Task<int> GetTransactionsPerMinuteAcrossAllStores()
        {
            if(_cache.TryGetValue("SalesPerformance", out Dictionary<string, Tuple<int, decimal>> salesPerformance))
            {
                var tpmAcrossAllStores = salesPerformance.Sum(s => s.Value.Item1);
                return tpmAcrossAllStores;
            }
            await Task.CompletedTask;
            return 0;
        }

        public async Task<int> GetTransactionsPerMinuteForStore(string storeName)
        {
            if(_cache.TryGetValue("SalesPerformance", out Dictionary<string, Tuple<int, decimal>> salesPerformance))
            {
                var tpmForStore = await Task.FromResult(salesPerformance[storeName].Item1);
                return tpmForStore;
            }
            await Task.CompletedTask;
            return 0;
        }

        public async Task<Dictionary<string, Tuple<int, decimal>>> LoadSalesPerformance()
        {
            if(_cache.TryGetValue("SalesPerformance", out Dictionary<string, Tuple<int, decimal>> salesPerformance))
            {
                return await Task.FromResult(salesPerformance);
            }
            return null;
        }

        public async Task SetSalesPerformance(Dictionary<string, Tuple<int, decimal>> salesPerformance)
        {
            _cache.Set("SalesPerformance", salesPerformance);
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _cache.Dispose();
        }

        public async Task<Dictionary<string, Tuple<decimal, decimal>>> GetStoreLocations()
        {
            if(_cache.TryGetValue("StoreLocations", out Dictionary<string, Tuple<decimal, decimal>> storeLocations))
            {
                return await Task.FromResult(storeLocations);
            }
            return null;
        }

        Task ISalesAnalyzer.SetStoreLocations(Dictionary<string, Tuple<decimal, decimal>> storeLocations)
        {
            _cache.Set("StoreLocations", storeLocations);
            return Task.CompletedTask;
        }

        public async Task<Dictionary<string, Tuple<Store, List<POSTerminal>>>> GetStoresAndTerminals()
        {
            if(_cache.TryGetValue("StoresAndTerminals", out Dictionary<string, Tuple<Store, List<POSTerminal>>> storesAndTerminals))
            {
                return await Task.FromResult(storesAndTerminals);
            }
            return null;
        }

        public Task SetStoresAndTerminals(Dictionary<string, Tuple<Store, List<POSTerminal>>> storesAndTerminals)
        {
            _cache.Set("StoresAndTerminals", storesAndTerminals);
            return Task.CompletedTask;
        }

        public async Task<decimal> GetTotalSalesRevenue()
        {
            if(_cache.TryGetValue("TotalSalesRevenue", out decimal totalSalesRevenue))
            {
                return await Task.FromResult(totalSalesRevenue);
            }
            return 0M;
        }

        public Task SetTotalSalesRevenue(decimal totalSalesRevenue)
        {
            _cache.Set("TotalSalesRevenue", totalSalesRevenue);
            return Task.CompletedTask;
        }
    }
}
