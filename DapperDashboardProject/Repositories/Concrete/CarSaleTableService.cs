using DapperDashboardProject.Context;
using DapperDashboardProject.Dtos;
using DapperDashboardProject.Repositories.Abstract;
using Dapper;
namespace DapperDashboardProject.Repositories.Concrete;
using System.Data.SqlClient;

public class CarSaleTableService : ICarSaleTableService
{
    private readonly DapperContext _context;

    public CarSaleTableService(DapperContext context)
    {
        _context = context;
    }

    public async Task<List<CarSaleTableDto>> GetCarSaleTableAsync()
    {
        using (var connection = _context.CreateConnection())
        {
            var sql = @"
            SELECT TOP 200 
                Date,
                Salesperson,
                CustomerName,          
                CarMake,
                CarModel,
                CarYear,
                Quantity,
                SalePrice             
            FROM CarSales           
            ";
            var result = await connection.QueryAsync<CarSaleTableDto>(sql);
            return result.ToList();

        }
    }
    public async Task<(List<CarSaleTableDto> Data, int TotalRecords, int FilteredRecords)> GetCarSaleTableServerSideAsync(
    int start, int length, string searchValue, string sortColumn, string sortDirection)
    {
        using (var connection = _context.CreateConnection())
        {
            // Sorgu
            var sql = $@"
            SELECT *
            FROM CarSales
            WHERE (@Search IS NULL 
                OR CustomerName LIKE '%' + @Search + '%'
                OR CarMake LIKE '%' + @Search + '%'
                OR Salesperson LIKE '%' + @Search + '%')
            ORDER BY 
                CASE WHEN @SortColumn = 'Date' THEN Date END {(sortDirection == "asc" ? "ASC" : "DESC")},
                CASE WHEN @SortColumn = 'Salesperson' THEN Salesperson END {(sortDirection == "asc" ? "ASC" : "DESC")}
            OFFSET @Start ROWS FETCH NEXT @Length ROWS ONLY;

            SELECT COUNT(*) FROM CarSales;

            SELECT COUNT(*) 
            FROM CarSales
            WHERE (@Search IS NULL 
                OR CustomerName LIKE '%' + @Search + '%'
                OR CarMake LIKE '%' + @Search + '%'
                OR Salesperson LIKE '%' + @Search + '%');
        ";

            using (var multi = await connection.QueryMultipleAsync(sql, new
            {
                Start = start,
                Length = length,
                Search = string.IsNullOrWhiteSpace(searchValue) ? null : searchValue,
                SortColumn = sortColumn
            }))
            {
                var data = await multi.ReadAsync<CarSaleTableDto>();
                var totalRecords = await multi.ReadSingleAsync<int>();
                var filteredRecords = await multi.ReadSingleAsync<int>();

                return (data.ToList(), totalRecords, filteredRecords);
            }
        }
    }


}




