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
    
}

      


