namespace RealTimeDashboard.API.Configuration
{
    public class UpdateFrequenciesConfiguration
    {
        // Configuration is being passed in seconds, but we store it in milliseconds, because most of the time-related functions accept ms
        private int _activeUsers = 10 * 1000;
        private int _totalSales = 10 * 1000;
        private int _topSellingProducts = 30 * 1000;

        public int ActiveUsers 
        {
            get { return _activeUsers; }
            set
            {
                if (value < 10)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Update frequency for active users cannot be less that 10 seconds.");
                }
                _activeUsers = value * 1000;
            }
        }

        public int TotalSales
        {
            get { return _totalSales; }
            set
            {
                if (value < 10)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Update frequency for active top sales cannot be less that 10 seconds.");
                }
                _totalSales = value * 1000;
            }
        }

        public int TopSellingProducts
        {
            get { return _topSellingProducts; }
            set
            {
                if (value < 30)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Update frequency for active top selling products cannot be less that 30 seconds.");
                }
                _topSellingProducts = value * 1000;
            }
        }
    }
}
