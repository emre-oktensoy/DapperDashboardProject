using DapperDashboardProject.Dtos;

namespace DapperDashboardProject.Repositories.Abstract
{
    public interface IDashboardService
    {
        Task<TotalSalesDto> GetTotalSalesAsync();
        Task<TotalRevenueDto> GetTotalRevenueAsync();
        Task<TopModelDto> GetTopModelAsync();
        Task<LastSaleDto> GetLastSaleAsync();
        Task<List<CitySalesDto>> GetSalesByCityAsync();
        Task<GenderDistributionDto> GetGenderDistributionAsync();
        Task<List<MonthlySalesDto>> GetMonthlySalesAsync();
        Task<GenderProgressDto> GetGenderProgressAsync();
        Task<TopItemProgressDto> GetTopCarMakeProgressAsync();
        Task<TopItemProgressDto> GetTopCarModelProgressAsync();
        Task<TopItemProgressDto> GetTopRegionProgressAsync();
        Task<List<PaymentLoadDto>> GetPaymentLoadPercentagesAsync();
        Task<List<TopCustomerDto>> GetTopCustomersAsync();
        Task<List<CarMakeCountDto>> GetCarMakeCountAsync();
        Task<List<SalesPersonCountDto>> GetSalesPersonCountAsync();

    }
}
