using DapperDashboardProject.Dtos;

namespace DapperDashboardProject.Models
{
    public class DashboardViewModel
    {
        public TotalSalesDto TotalSales { get; set; }
        public TotalRevenueDto TotalRevenue { get; set; }
        public TopModelDto TopModel { get; set; }
        public LastSaleDto LastSale { get; set; }
        public List<CitySalesDto> SalesByCity { get; set; }                    
        public List<MonthlySalesDto> MonthlySales { get; set; }
        public GenderProgressDto GenderProgress { get; set; }
        public TopItemProgressDto TopCarMake { get; set; }
        public TopItemProgressDto TopCarModel { get; set; }
        public TopItemProgressDto TopRegion { get; set; }
        public List<PaymentLoadDto> TopPaymentLoad { get; set; }
        public List<TopCustomerDto> TopCustomers { get; set; }
        public List<CarMakeCountDto> CarMakeCount { get; set; }
        public List<SalesPersonCountDto> SalesPersonCount { get; set; }

        public List<GoogleNewsItem> NewsList { get; set; }

    }
}
