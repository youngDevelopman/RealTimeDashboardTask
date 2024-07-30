using Microsoft.Extensions.Caching.Memory;
using RealTimeDashboard.API.Services.Interfaces;

namespace RealTimeDashboard.API.Services
{
    public class UserService : IUserService
    {
        private readonly IMemoryCache _memoryCache;
        public UserService(IMemoryCache cache)
        {
            _memoryCache = cache;            
        }

        public int GetAmountOfActiveUsers()
        {
            var activeUsers = _memoryCache.Get<int>(Constants.ACTIVE_USERS_AMOUNT_CACHE_KEY);
            return activeUsers;
        }
    }
}
