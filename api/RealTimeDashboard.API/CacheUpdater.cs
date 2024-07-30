
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Caching.Memory;

namespace RealTimeDashboard.API
{
    public class CacheUpdater : BackgroundService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<CacheUpdater> _logger;

        public CacheUpdater(IMemoryCache memoryCache, ILogger<CacheUpdater> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
            _memoryCache.Set(Constants.ACTIVE_USERS_AMOUNT_CACHE_KEY, 50);
            _memoryCache.Set(Constants.TOTAL_SALES_AMOUNT_CACHE_KEY, 120.0F);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                UpdateActiveUsers();
                UpdateTotalSales();
                await Task.Delay(Constants.CACHE_UPDATE_FREQUENCY, stoppingToken);
            }
        }

        private void UpdateActiveUsers()
        {
            Random random = new Random();
            int randomNumber = random.Next(-10, 11);

            var currentActiveUsers = _memoryCache.Get<int>(Constants.ACTIVE_USERS_AMOUNT_CACHE_KEY);
            currentActiveUsers += randomNumber;
            _memoryCache.Set(Constants.ACTIVE_USERS_AMOUNT_CACHE_KEY, currentActiveUsers);

            _logger.LogInformation($"Active user amount has been updated to {currentActiveUsers}");
        }

        private void UpdateTotalSales()
        {
            Random random = new Random();
            float lowerBound = 5;
            float upperBound = 10;
            float randomFloat = (float)random.NextDouble();
            float randomNumber = lowerBound + (randomFloat * (upperBound - lowerBound));

            var currentTotalSales = _memoryCache.Get<float>(Constants.TOTAL_SALES_AMOUNT_CACHE_KEY);
            currentTotalSales += randomNumber;
            _memoryCache.Set(Constants.TOTAL_SALES_AMOUNT_CACHE_KEY, currentTotalSales);

            _logger.LogInformation($"Total sales number has been updated to {currentTotalSales}");
        }
    }
}
