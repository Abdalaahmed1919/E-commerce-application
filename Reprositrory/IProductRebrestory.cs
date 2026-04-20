using Simple_E_commers_App.Models;

namespace Simple_E_commers_App.Reprositrory
{
    public interface IProductRebrestory
    {
        void AddProduct(Product categProductory);
        void UpdateProduct(Product Product);
        void DeleteProduct(int id);
        Product GetProductById(int id);
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetAllProductsIcludesCatogary();
        IEnumerable<Product> GetTheChepstProducts();
         void Save();
    }
}
