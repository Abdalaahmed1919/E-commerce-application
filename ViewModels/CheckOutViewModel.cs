using System.ComponentModel.DataAnnotations;

namespace Simple_E_commers_App.ViewModels
{
    public class CheckOutViewModel
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "رقم التليفون مطلوب")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "المدينة مطلوبة")]
        public string City { get; set; }

        [Required(ErrorMessage = "العنوان بالتفصيل مطلوب")]
        public string ShippingAddress { get; set; }

        [Required]
        public string PaymentMethod { get; set; } = "Visa";

        public List<CartItem>? CartItems { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
