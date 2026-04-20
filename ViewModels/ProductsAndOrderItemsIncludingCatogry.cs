namespace Simple_E_commers_App.ViewModels
{
    public class ProductsAndOrderItemsIncludingCatogry
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<OrderItem> OrderItem { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public Product NewProduct { get; set; }
    }
}
