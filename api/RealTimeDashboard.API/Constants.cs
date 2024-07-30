namespace RealTimeDashboard.API
{
    public static class Constants
    {
        public const string ACTIVE_USERS_AMOUNT_CACHE_KEY = "active-users";
        public const string TOTAL_SALES_AMOUNT_CACHE_KEY = "total-sales";
        public const string TOP_SELLING_PRODUCTS_CACHE_KEY = "top-selling-product";

        public const int CACHE_UPDATE_FREQUENCY = 2000; // specified in ms
        public const string TOP_SELLING_PRODUCTS_SEED_FILE_PATH = "data.json";
    }
}