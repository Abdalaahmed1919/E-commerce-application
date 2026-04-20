
using Microsoft.EntityFrameworkCore;

namespace Simple_E_commers_App.Reprositrory
{
    public class OrderRebrestory : IOrderRebrestory
    {
        public AppDbContext context { get; }
        public OrderRebrestory (AppDbContext context) {
            this.context = context;
        }
        public void AddOrder(Order order)
        {
           context.Orders.Add(order);
        }

        public void DeleteOrder(int id)
        {
            var order = context.Orders.FirstOrDefault(o => o.Id == id);
            if (order != null) { 
                context.Orders.Remove(order);
            } 
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return context.Orders.ToList();
        }
        public IEnumerable<Order> GetAllOrdersIncudingUsersAndOrderItems()
        {
            return context.Orders.Include(o => o.User)
                .Include(o=> o.OrderItems)
                .ToList();
        }

        public Order GetOrderById(int id)
        {
            return context.Orders.FirstOrDefault(o => o.Id == id);
        }

        public void UpdateOrder(Order order)
        {
            context.Orders.Update(order);
            
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public int getPinndingOrderCount()
        {
            return context.Orders.Where(o => o.Status.ToString() == "Pending").Count();
        }

        public int getProcessingOrderCount()
        {
            return context.Orders.Where(o => o.Status.ToString() == "Processing").Count();
        }

        public int getShippedOrderCount()
        {
            return context.Orders.Where(o => o.Status.ToString() == "Shipped").Count();
        }

        public int getCompletedOrderCount()
        {
            return context.Orders.Where(o => o.Status.ToString() == "Completed").Count();
        }
    }
}
