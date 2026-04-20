namespace Simple_E_commers_App.Reprositrory
{
    public interface ICartItemReprosetory
    {
        CartItem? GetById(int id);

        CartItem? GetCartItem(int cartId, int productId);

        void Add(CartItem cartItem);

        void Update(CartItem cartItem);

        void Delete(CartItem cartItem);

        void Save();
    }
}
