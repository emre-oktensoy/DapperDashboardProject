using DapperDashboardProject.Models;
using DapperDashboardProject.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperDashboardProject.Controllers
{
    public class TestController : Controller
    {
        IDashboardService _dashboardService;

        public TestController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalSales = await _dashboardService.GetTotalSalesAsync(),
                TotalRevenue = await _dashboardService.GetTotalRevenueAsync(),
                TopModel = await _dashboardService.GetTopModelAsync(),
                LastSale = await _dashboardService.GetLastSaleAsync(),
                SalesByCity = await _dashboardService.GetSalesByCityAsync()
            };

            return View(viewModel);

        }
    }
}
