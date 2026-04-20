namespace Simple_E_commers_App.Reprositrory
{
    public interface IOrderItemsRebrestory
    {
        void AddOrderItems(OrderItem orderItem);
        void UpdateOrderItem(OrderItem orderItem);
        void DeleteOrderItem(int id);
        OrderItem GetOrderItemById(int id);
        IEnumerable<OrderItem> GetAllOrderItems();
        int GetProductCount(int id);
        void Save();
    }
}
