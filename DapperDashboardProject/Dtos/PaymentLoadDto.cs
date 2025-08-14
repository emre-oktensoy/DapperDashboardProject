namespace DapperDashboardProject.Dtos
{
    public class PaymentLoadDto
    {
        public string PaymentType { get; set; } // Loan, Lease, Cash
        public double Percentage { get; set; }  // 0.0 formatında yüzde
    }
}
