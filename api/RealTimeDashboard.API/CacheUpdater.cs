
using Microsoft.Extensions.Caching.Memory;

namespace RealTimeDashboard.API
{
    public class CacheUpdater : BackgroundService
    {
        private readonly IMemoryCache _memoryCache;

        public CacheUpdater(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _memoryCache.Set("active-users", 50);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Random random = new Random();
                int randomNumber = random.Next(-10, 11);
                var currentActiveUsers = _memoryCache.Get<int>("active-users");
                currentActiveUsers += randomNumber;
                _memoryCache.Set("active-users", currentActiveUsers);

                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
