namespace Simple_E_commers_App.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public List<OrderItem> OrderItems { get; set; }
         public int CategoryId { get; set; }
         public Category Category { get; set; }
        // image 
         public string ImageUrl { get; set; }
        public bool IsDeleted { get; set; } = false;

    }

}
