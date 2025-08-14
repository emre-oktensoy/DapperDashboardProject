using DapperDashboardProject.Dtos;
using DapperDashboardProject.Models;
using DapperDashboardProject.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperDashboardProject.Controllers
{
    public class DefaultController : Controller
    {
        IDashboardService _dashboardService;     
        ICarSaleTableService _carSaleTableService;

        public DefaultController(IDashboardService dashboardService, ICarSaleTableService carSaleTableService)
        {
            _dashboardService = dashboardService;
            _carSaleTableService = carSaleTableService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalSales = await _dashboardService.GetTotalSalesAsync(),
                TotalRevenue = await _dashboardService.GetTotalRevenueAsync(),
                TopModel = await _dashboardService.GetTopModelAsync(),
                LastSale = await _dashboardService.GetLastSaleAsync(),
                SalesByCity = await _dashboardService.GetSalesByCityAsync(),
                MonthlySales = await _dashboardService.GetMonthlySalesAsync(),
                GenderProgress = await _dashboardService.GetGenderProgressAsync(),
                TopCarMake = await _dashboardService.GetTopCarMakeProgressAsync(),
                TopCarModel = await _dashboardService.GetTopCarModelProgressAsync(),
                TopRegion = await _dashboardService.GetTopRegionProgressAsync(),
                TopPaymentLoad = await _dashboardService.GetPaymentLoadPercentagesAsync(),
                TopCustomers = await _dashboardService.GetTopCustomersAsync(),
                CarMakeCount = await _dashboardService.GetCarMakeCountAsync(),
                SalesPersonCount = await _dashboardService.GetSalesPersonCountAsync()
            };

            return View(viewModel);
        }

        public async Task<IActionResult> TableDataIndex()
        {
            var viewModel = new TableViewModel 
            {
               CarSaleTable = await _carSaleTableService.GetCarSaleTableAsync()
            };
            return View(viewModel);
        }



    }
}