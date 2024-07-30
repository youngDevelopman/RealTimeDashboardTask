
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
            _memoryCache.Set("active-users", 50);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Random random = new Random();
                int randomNumber = random.Next(-10, 11);

                var currentActiveUsers = _memoryCache.Get<int>(Constants.ACTIVE_USERS_AMOUNT_CACHE_KEY);
                currentActiveUsers += randomNumber;
                _memoryCache.Set(Constants.ACTIVE_USERS_AMOUNT_CACHE_KEY, currentActiveUsers);

                _logger.LogInformation($"Active user amount has been updated to {currentActiveUsers}");

                await Task.Delay(Constants.ACTIVE_USERS_AMOUNT_CACHE_UPDATE_FREQUENCY, stoppingToken);
            }
        }
    }
}
