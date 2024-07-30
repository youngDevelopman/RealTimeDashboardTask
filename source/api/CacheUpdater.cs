
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.Extensions.Caching.Memory;
using RealTimeDashboard.API.Configuration;
using RealTimeDashboard.API.Models;
using System.Text.Json;

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

            string jsonString = File.ReadAllText(Constants.TOP_SELLING_PRODUCTS_SEED_FILE_PATH);
            var products = JsonSerializer.Deserialize<List<SellingProduct>>(jsonString);
            _memoryCache.Set(Constants.TOP_SELLING_PRODUCTS_CACHE_KEY, products);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                UpdateActiveUsers();
                UpdateTotalSales();
                UpdateTopSellingProducts();
                await Task.Delay(Constants.CACHE_UPDATE_FREQUENCY, stoppingToken);
            }
        }

        private void UpdateActiveUsers()
        {
            Random random = new Random();
            int randomNumber = random.Next(-10, 11);

            var currentActiveUsers = _memoryCache.Get<int>(Constants.ACTIVE_USERS_AMOUNT_CACHE_KEY);
            currentActiveUsers += randomNumber;
            if(currentActiveUsers < 0)
            {
                currentActiveUsers = 0;
            }
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

        private void UpdateTopSellingProducts()
        {
            var topSellingsProducts = _memoryCache.Get<List<SellingProduct>>(Constants.TOP_SELLING_PRODUCTS_CACHE_KEY);

            // Random class is not thread safe
            var threadLocalRandom = new ThreadLocal<Random>(() => new Random());
            Parallel.ForEach(topSellingsProducts, product =>
            {
                Random random = threadLocalRandom.Value;
                float randomFiguresToAdd = (float)(random.NextDouble() * 1000);
                product.Sales += randomFiguresToAdd;

                _logger.LogInformation($"{product.Name} total sales number has been updated to {product.Sales}");
            });
        }
    }
}
