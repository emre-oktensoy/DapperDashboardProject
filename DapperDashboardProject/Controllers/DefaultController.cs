using DapperDashboardProject.Dtos;
using DapperDashboardProject.Models;
using DapperDashboardProject.Repositories.Abstract;
using DapperDashboardProject.Repositories.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace DapperDashboardProject.Controllers
{
    public class DefaultController : Controller
    {
        IDashboardService _dashboardService;     
        ICarSaleTableService _carSaleTableService;
        private readonly GoogleNewsService _newsService;

        public DefaultController(IDashboardService dashboardService, ICarSaleTableService carSaleTableService, GoogleNewsService newsService)
        {
            _dashboardService = dashboardService;
            _carSaleTableService = carSaleTableService; 
            _newsService = newsService;

        }      
            

        public async Task<IActionResult> Dashboard()
        {
            // API'den veriyi çek
            var newsList = await _newsService.GetNewsAsync("araba satışları");

            // ViewBag ile partial view'e göndereceğiz
            ViewBag.NewsList = newsList;

            return View();
        }

        public async Task<IActionResult> Index()
        {
            var newsList = await _newsService.GetNewsAsync("araba satışları");

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
                SalesPersonCount = await _dashboardService.GetSalesPersonCountAsync(),
                NewsList = newsList // <- Yeni property
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

        [HttpPost]
        public async Task<IActionResult> GetCarSaleData()
        {
            int start = Convert.ToInt32(Request.Form["start"]);
            int length = Convert.ToInt32(Request.Form["length"]);
            string searchValue = Request.Form["search[value]"];
            string sortColumn = Request.Form[$"columns[{Request.Form["order[0][column]"]}][data]"];
            string sortDirection = Request.Form["order[0][dir]"];

            var (data, totalRecords, filteredRecords) = await _carSaleTableService.GetCarSaleTableServerSideAsync(start, length, searchValue, sortColumn, sortDirection);

            return Json(new
            {
                draw = Request.Form["draw"],
                recordsTotal = totalRecords,
                recordsFiltered = filteredRecords,
                data = data
            });
        }

        public async Task<IActionResult> GetNewsPartial()
        {
            try
            {
                var newsList = await _newsService.GetNewsAsync("araba satışları");
                return PartialView("~/Views/Shared/__NewsListPartial.cshtml", newsList);
            }
            catch (Exception ex)
            {
                return Content($"Hata: {ex.Message}");
            }
        }
       

    }
}