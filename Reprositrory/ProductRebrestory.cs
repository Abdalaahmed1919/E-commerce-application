using Microsoft.EntityFrameworkCore;
using Simple_E_commers_App.Models;

namespace Simple_E_commers_App.Reprositrory
{
    public class ProductRebrestory : IProductRebrestory
    {
        public AppDbContext context { get; }
        public ProductRebrestory(AppDbContext context)
        {
            this.context = context;
        }


        public void AddProduct(Product Product)
        {
            context.Products.Add(Product);
        }

        public void DeleteProduct(int id)
        {
            var Product = context.Products.FirstOrDefault(c => c.Id == id);
            if (Product != null)
            {

                Product.IsDeleted = true;
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return context.Products
                    .Where(p => !p.IsDeleted) 
                    .ToList();
        }

        public Product GetProductById(int id)
        {
            return context.Products.FirstOrDefault(c => c.Id == id);
        }

        public void UpdateProduct(Product Product)
        {
            var ProductToUpdate = context.Products.FirstOrDefault(c => c.Id == Product.Id);

            if (ProductToUpdate != null)
            {
                ProductToUpdate.Name = Product.Name;
            }
        }
        public IEnumerable<Product> GetAllProductsIcludesCatogary()
        {
            return context.Products.Where(p => !p.IsDeleted).Include(p => p.Category).ToList();
        }
        public IEnumerable<Product> GetTheChepstProducts()
        {
            return context.Products.Where(p => !p.IsDeleted).OrderBy(p => p.Price).Take(5).ToList();
        }
        public void Save()
        {
            context.SaveChanges();
        }
    }
}
