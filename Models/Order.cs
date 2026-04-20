using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Simple_E_commers_App.Models
{
   public class Order
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "عنوان الشحن مطلوب")]
        public string ShippingAddress { get; set; }

        [Required(ErrorMessage = "المدينة مطلوبة")]
        public string City { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        [Required]
        public string PaymentMethod { get; set; } 

        public bool IsPaid { get; set; } = false;
        public int AppUserId { get; set; }
        public AppUser User { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
    public enum OrderStatus
    {
        Pending = 0,
        Processing = 1,
        Shipped = 2,
        Completed = 3,
        Cancelled = 4
    }
}
