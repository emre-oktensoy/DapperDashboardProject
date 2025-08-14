using Dapper;
using DapperDashboardProject.Context;
using DapperDashboardProject.Dtos;
using DapperDashboardProject.Repositories.Abstract;
using System;
using System.Globalization;

namespace DapperDashboardProject.Repositories.Concrete
{
    public class DashboardService : IDashboardService
    {
        private readonly DapperContext _context;

        public DashboardService(DapperContext context)
        {
            _context = context;
        }

        public async Task<TotalSalesDto> GetTotalSalesAsync()
        {
            var sql = "SELECT COUNT(*) FROM CarSales";
            var connection = _context.CreateConnection();
            var count = await connection.ExecuteScalarAsync<int>(sql);
            return new TotalSalesDto { TotalSalesCount = count };
        }

        public async Task<TotalRevenueDto> GetTotalRevenueAsync()
        {
            var sql = "SELECT SUM(SalePrice) AS TotalRevenue FROM CarSales";
            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<TotalRevenueDto>(sql);
            return result ?? new TotalRevenueDto { TotalRevenue = 0 };
        }

        public async Task<TopModelDto> GetTopModelAsync()
        {
            var sql = @"
                    SELECT TOP 1 CarMake, COUNT(*) AS SaleCount
                    FROM CarSales
                    GROUP BY CarMake
                    ORDER BY SaleCount DESC";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<TopModelDto>(sql);
            return result ?? new TopModelDto { CarMake = "Veri yok", SaleCount = 0 };
        }

        public async Task<LastSaleDto> GetLastSaleAsync()
        {
            var sql = @"
                    SELECT TOP 1 CarMake, SalePrice, Date
                    FROM CarSales
                    ORDER BY Date DESC";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<LastSaleDto>(sql);
            return result ?? new LastSaleDto();

        }

        public async Task<List<CitySalesDto>> GetSalesByCityAsync()
        {
            var sql = @"
                    SELECT SalesRegion, COUNT(*) AS SaleCount
                    FROM CarSales
                    GROUP BY SalesRegion";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<CitySalesDto>(sql);
            return result.ToList();

        }

        public async Task<GenderDistributionDto> GetGenderDistributionAsync()
        {
            var sql = @"
                    SELECT 
                        SUM(CASE WHEN CustomerGender = 'Male' THEN 1 ELSE 0 END) AS MaleCount,
                        SUM(CASE WHEN CustomerGender = 'Female' THEN 1 ELSE 0 END) AS FemaleCount                        
                     FROM CarSales";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryFirstOrDefaultAsync<GenderDistributionDto>(sql);
            return result;

        }

        public async Task<List<MonthlySalesDto>> GetMonthlySalesAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
            SELECT SaleMonth, SaleYear, COUNT(*) AS TotalCount
            FROM CarSales
            GROUP BY SaleMonth, SaleYear
            ORDER BY SaleYear, 
                     CASE 
                        WHEN SaleMonth = 'January' THEN 1
                        WHEN SaleMonth = 'February' THEN 2
                        WHEN SaleMonth = 'March' THEN 3
                        WHEN SaleMonth = 'April' THEN 4
                        WHEN SaleMonth = 'May' THEN 5
                        WHEN SaleMonth = 'June' THEN 6
                        WHEN SaleMonth = 'July' THEN 7
                        WHEN SaleMonth = 'August' THEN 8
                        WHEN SaleMonth = 'September' THEN 9
                        WHEN SaleMonth = 'October' THEN 10
                        WHEN SaleMonth = 'November' THEN 11
                        WHEN SaleMonth = 'December' THEN 12
                     END";

                var result = await connection.QueryAsync<MonthlySalesDto>(sql);
                return result.ToList();
            }
        }

        public async Task<GenderProgressDto> GetGenderProgressAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
            SELECT 
                SUM(CASE WHEN CustomerGender = 'Male' THEN 1 ELSE 0 END) AS MaleCount,
                SUM(CASE WHEN CustomerGender = 'Female' THEN 1 ELSE 0 END) AS FemaleCount
            FROM CarSales";

                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(sql);

                int maleCount = result.MaleCount;
                int femaleCount = result.FemaleCount;
                int total = maleCount + femaleCount;

                double malePercentage = total > 0 ? (maleCount * 100.0 / total) : 0;
                double femalePercentage = total > 0 ? (femaleCount * 100.0 / total) : 0;

                return new GenderProgressDto
                {
                    MaleCount = maleCount,
                    FemaleCount = femaleCount,
                    // Nokta ile formatlı string dönüyoruz
                    MalePercentage = malePercentage.ToString("0.0", CultureInfo.InvariantCulture),
                    FemalePercentage = femalePercentage.ToString("0.0", CultureInfo.InvariantCulture)
                };
            }


        }

        public async Task<TopItemProgressDto> GetTopCarMakeProgressAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
            SELECT TOP 1 CarMake, COUNT(*) AS MakeCount
            FROM CarSales
            GROUP BY CarMake
            ORDER BY MakeCount DESC";

                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(sql);

                if (result == null) return null;

                int total = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM CarSales");
                double percentage = total > 0 ? (result.MakeCount * 100.0 / total) : 0;

                return new TopItemProgressDto
                {
                    Name = result.CarMake,
                    Count = result.MakeCount,
                    Percentage = Math.Round(percentage, 1) // 1 basamak küsurat
                };
            }
        }

        public async Task<TopItemProgressDto> GetTopCarModelProgressAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
            SELECT TOP 1 CarModel, COUNT(*) AS ModelCount
            FROM CarSales
            GROUP BY CarModel
            ORDER BY ModelCount DESC";

                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(sql);

                if (result == null) return null;

                int total = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM CarSales");
                double percentage = total > 0 ? (result.ModelCount * 100.0 / total) : 0;

                return new TopItemProgressDto
                {
                    Name = result.CarModel,
                    Count = result.ModelCount,
                    Percentage = Math.Round(percentage, 1)
                };
            }
        }

        public async Task<TopItemProgressDto> GetTopRegionProgressAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
            SELECT TOP 1 SalesRegion, COUNT(*) AS RegionCount
            FROM CarSales
            GROUP BY SalesRegion
            ORDER BY RegionCount DESC";

                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(sql);

                if (result == null) return null;

                int total = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM CarSales");
                double percentage = total > 0 ? (result.RegionCount * 100.0 / total) : 0;

                return new TopItemProgressDto
                {
                    Name = result.SalesRegion,
                    Count = result.RegionCount,
                    Percentage = Math.Round(percentage, 1)
                };
            }
        }

        public async Task<List<PaymentLoadDto>> GetPaymentLoadPercentagesAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
            SELECT PaymentMethod, COUNT(*) AS Count
            FROM CarSales
            GROUP BY PaymentMethod";

                var result = await connection.QueryAsync<dynamic>(sql);

                int total = result.Sum(r => (int)r.Count);

                var list = new List<PaymentLoadDto>();

                foreach (var row in result)
                {
                    double percentage = total > 0
                        ? ((int)row.Count * 100.0 / total)
                        : 0;

                    list.Add(new PaymentLoadDto
                    {
                        PaymentType = row.PaymentMethod,
                        Percentage = Math.Round(percentage, 1) // 30.5 formatında
                    });
                }
                return list;
            }
        }

        public async Task<List<TopCustomerDto>> GetTopCustomersAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
            SELECT TOP 10 
                CustomerName,
                CustomerAge,
                CustomerGender,
                SalesRegion,
                COUNT(*) AS PurchaseCount
            FROM CarSales
            GROUP BY CustomerName, CustomerAge, CustomerGender, SalesRegion
            ORDER BY PurchaseCount DESC";

                var result = await connection.QueryAsync<TopCustomerDto>(sql);
                return result.ToList();
            }
        }

        public async Task<List<CarMakeCountDto>> GetCarMakeCountAsync()
        {
            var sql = @"
                    SELECT CarMake, COUNT(*) AS SaleCount
                    FROM CarSales
                    GROUP BY CarMake
                    ORDER BY SaleCount DESC";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<CarMakeCountDto>(sql);
            return result.ToList();
        }
        public async Task<List<SalesPersonCountDto>> GetSalesPersonCountAsync()
        {
            var sql = @"
        SELECT SalesPerson, COUNT(*) AS SaleCount
        FROM CarSales
        GROUP BY SalesPerson
        ORDER BY SaleCount DESC";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<SalesPersonCountDto>(sql);
            return result.ToList();
        }

    }
}
