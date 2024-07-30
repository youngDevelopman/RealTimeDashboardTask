using RealTimeDashboard.API.Models;

namespace RealTimeDashboard.API.Services.Interfaces
{
    public interface ISalesService
    {
        float GetTotalSales();

        List<SellingProduct> GetTopSellingProducts();
    }
}
