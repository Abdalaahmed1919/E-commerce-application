namespace Simple_E_commers_App.ViewModels
{
    public class CartViewModel
    {
        public int CartId { get; set; }
        public List<CartItem> cartItems { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }

    }
}
