using Microsoft.Extensions.Caching.Memory;
using RealTimeDashboard.API.Services.Interfaces;

namespace RealTimeDashboard.API.Services
{
    public class SalesService : ISalesService
    {
        private readonly IMemoryCache _memoryCache;
        public SalesService(IMemoryCache cache)
        {
            _memoryCache = cache;
        }

        public float GetTotalSales()
        {
            var totalSales = _memoryCache.Get<float>(Constants.TOTAL_SALES_AMOUNT_CACHE_KEY);
            return totalSales;
        }
    }
}
