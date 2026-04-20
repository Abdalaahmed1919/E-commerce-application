namespace Simple_E_commers_App.Models
{
    public class Category 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Product> Products { get; set; }
    }

}
