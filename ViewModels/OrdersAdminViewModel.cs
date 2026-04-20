using Microsoft.Identity.Client;

namespace Simple_E_commers_App.ViewModels
{
    public class OrdersAdminViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalMoney { get; set; }
        public int totalUsers { get; set; }
        public int ActiveProducts { get; set; }
        public int OrderItemsInOrder { get; set; }
        public int PinndingOrdersCount { get; set; }
        public int ShippingOrdersCount { get; set; }
        public int ProcessingOrderCount { get; set; }
        public int CompletedOrderCount { get; set; }
    }
}
