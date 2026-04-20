namespace Simple_E_commers_App.Reprositrory
{
    public interface IOrderRebrestory
    {
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int id);
        Order GetOrderById(int id);
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Order> GetAllOrdersIncudingUsersAndOrderItems();
        int getPinndingOrderCount();
        int getProcessingOrderCount();
        int getShippedOrderCount();
        int getCompletedOrderCount();
        void Save();
    }
}
