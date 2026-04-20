
namespace Simple_E_commers_App.Reprositrory
{
    public class OrderItemsRebrestory : IOrderItemsRebrestory
    {
        public AppDbContext context { get; }
        public OrderItemsRebrestory(AppDbContext context)
        {
            this.context = context;
        }


        public void AddOrderItems(OrderItem orderItem)
        {
            context.OrderItems.Add(orderItem);
        }

        public void DeleteOrderItem(int id)
        {
            var orderitem = context.OrderItems.FirstOrDefault(x => x.Id == id);
            if (orderitem != null) { 
             context.OrderItems.Remove(orderitem);
            }
        }

        public IEnumerable<OrderItem> GetAllOrderItems()
        {
            return context.OrderItems.ToList();
        }

        public OrderItem GetOrderItemById(int id)
        {
            return context.OrderItems.FirstOrDefault(o => o.Id == id);

        }

        public void UpdateOrderItem(OrderItem orderItem)
        {
            context.OrderItems.Update(orderItem);
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public int GetProductCount(int id)
        {
            return context.OrderItems.Where(oi => oi.ProductId == id).Sum(oi => oi.Quantity);
        }

    }
}
