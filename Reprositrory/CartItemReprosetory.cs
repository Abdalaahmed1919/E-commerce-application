namespace Simple_E_commers_App.Reprositrory
{
    public class CartItemReprosetory : ICartItemReprosetory
    {
        private readonly AppDbContext _context;

        public CartItemReprosetory(AppDbContext context)
        {
            _context = context;
        }

        public CartItem? GetById(int id)
        {
            return _context.CartItems.FirstOrDefault(c => c.Id == id);
        }

        public CartItem? GetCartItem(int cartId, int productId)
        {
            return _context.CartItems.FirstOrDefault(c => c.CartId == cartId && c.ProductId == productId);
        }

        public void Add(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
        }

        public void Update(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
        }

        public void Delete(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
