
using Microsoft.EntityFrameworkCore;
using Simple_E_commers_App.Models;

namespace Simple_E_commers_App.Reprositrory
{
    public class CartReprosetory : ICartReprosetory
    {

        private readonly AppDbContext context;

        public CartReprosetory(AppDbContext context)
        {
            this.context = context;
        }

        public void AddCart(Cart cart)
        {
            context.Carts.Add(cart);
        }

        public void DeleteCart(int id)
        {
            var cart = context.Carts.FirstOrDefault(c => c.Id == id);
            if (cart != null)
            {
                context.Carts.Remove(cart);
            }
        }

        public IEnumerable<Cart> GetAllCarts()
        {
            return context.Carts.ToList();
        }

        public IEnumerable<Cart> GetAllCartsIncludingCartItems()
        {
            return context.Carts.Include(c => c.CartItems).ToList();
        }

        public Cart GetCartById(int id)
        {
            return context.Carts.FirstOrDefault(c => c.Id == id);
        }

        public Cart? GetCartByUserId(int userId)
        {
            return context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product) 
                       .FirstOrDefault(c => c.AppUserId == userId);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}