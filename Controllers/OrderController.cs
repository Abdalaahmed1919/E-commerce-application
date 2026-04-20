using Microsoft.AspNetCore.Mvc;
using Simple_E_commers_App.Reprositrory;
using System.Security.Claims;

namespace Simple_E_commers_App.Controllers
{
    public class OrderController : Controller
    {
        public OrderController(IOrderRebrestory orderRebrestory)
        {
            OrderRebrestory = orderRebrestory;
        }

        public IOrderRebrestory OrderRebrestory { get; }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userOrders = OrderRebrestory.GetAllOrders()
                                           .Where(o => o.AppUserId == int.Parse(userId))
                                           .OrderByDescending(o => o.CreatedAt)
                                           .ToList();

            return View(userOrders);
        }
    }
    }
