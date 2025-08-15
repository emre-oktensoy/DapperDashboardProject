using DapperDashboardProject.Dtos;

namespace DapperDashboardProject.Repositories.Abstract
{
    public interface ICarSaleTableService
    {
        Task<List<CarSaleTableDto>> GetCarSaleTableAsync();
        Task<(List<CarSaleTableDto> Data, int TotalRecords, int FilteredRecords)> GetCarSaleTableServerSideAsync(
            int start, int length, string searchValue, string sortColumn, string sortDirection);
    }
}
