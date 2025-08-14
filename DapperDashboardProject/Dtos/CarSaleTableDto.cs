namespace DapperDashboardProject.Dtos
{
    public class CarSaleTableDto
    {
        public DateTime Date { get; set; }
        public string Salesperson { get; set; }
        public string CustomerName { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public int CarYear { get; set; }
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }

    }

}
