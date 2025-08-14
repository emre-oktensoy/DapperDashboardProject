using DapperDashboardProject.Dtos;

namespace DapperDashboardProject.Repositories.Abstract
{
    public interface ICarSaleTableService
    {
        Task<List<CarSaleTableDto>> GetCarSaleTableAsync();
    }
}
