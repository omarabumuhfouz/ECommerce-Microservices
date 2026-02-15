namespace FrontEnd_Ecommerce.Models
{
    public class AdminStatsViewModel
    {
        public int TotalOrders { get; set; }
        public decimal Revenue { get; set; }
        public int PendingOrders { get; set; }
        public int Cancellations { get; set; }
    }
}
