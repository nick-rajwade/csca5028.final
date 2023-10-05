using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csca5028.lib
{
    public class SalesAnalyzer
    {
        //calculate the total sales performance for all stores
        public static double TotalSalesPerformance(List<Store> stores)
        {
            double totalSalesPerformance = 0;
            foreach (Store store in stores)
            {
                //totalSalesPerformance += store.SalesPerformance;
            }
            return totalSalesPerformance;
        }
    }
}
