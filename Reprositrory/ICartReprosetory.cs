namespace Simple_E_commers_App.Reprositrory
{
    public interface ICartReprosetory 
    {
        void AddCart(Cart cart);
        //void UpdateCart(Cart cart);
        void DeleteCart(int id);
        Cart GetCartById(int id);
        Cart? GetCartByUserId(int userId);
        IEnumerable<Cart> GetAllCarts();
        IEnumerable<Cart> GetAllCartsIncludingCartItems();
        void Save();
    }
}
