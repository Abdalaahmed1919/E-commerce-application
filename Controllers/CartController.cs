using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Simple_E_commers_App.Reprositrory;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Simple_E_commers_App.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        public CartController(ICartReprosetory cartReprosetory, ICartItemReprosetory cartItemReprosetory,
            UserManager<AppUser> userManager , IOrderRebrestory orderRebrestory ,IProductRebrestory productRebrestory)
        {
            CartReprosetory = cartReprosetory;
            CartItemReprosetory = cartItemReprosetory;
            UserManager = userManager;
            OrderRebrestory = orderRebrestory;
            ProductRebrestory = productRebrestory;
        }

        public ICartReprosetory CartReprosetory { get; }
        public ICartItemReprosetory CartItemReprosetory { get; }
        public UserManager<AppUser> UserManager { get; }
        public IOrderRebrestory OrderRebrestory { get; }
        public IProductRebrestory ProductRebrestory { get; }

        [HttpGet]
        public IActionResult Index()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString!);

            var cart = CartReprosetory.GetCartByUserId(userId);

            if (cart == null)
            {
                var empty = new CartViewModel
                {
                    cartItems = new List<CartItem>(),
                    Total = 0,
                };

                return View("Index", empty);

            }

            var cartViewModel = new CartViewModel
            {
                cartItems = cart.CartItems,
                Total = cart.TotalPrice,
            };

            return View("Index", cartViewModel);
        }

        [HttpPost]
        public IActionResult UpdateCartItemQuantity(int productId, int quantity)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString!);

            var cart = CartReprosetory.GetCartByUserId(userId);
            if (cart == null) return Json(new { success = false, message = "السلة غير موجودة" });

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (cartItem != null)
            {
                if (quantity <= 0)
                {
                    cart.CartItems.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity = quantity;
                }

                cart.TotalPrice = cart.CartItems.Sum(i => i.Quantity * i.UnitPrice);
                CartReprosetory.Save();

                var itemTotal = cartItem != null ? (cartItem.Quantity * cartItem.UnitPrice) : 0;
                var cartSubtotal = cart.TotalPrice;

                var grandTotal = cartSubtotal;

                return Json(new
                {
                    success = true,
                    itemTotal = itemTotal,
                    cartSubtotal = cartSubtotal,
                    grandTotal = grandTotal
                });
            }

            return Json(new { success = false, message = "المنتج غير موجود في السلة" });
        }
        [HttpPost]
        public IActionResult DeleteCartItem(int id)
        {
            var cartItem = CartItemReprosetory.GetById(id);

            if (cartItem != null)
            {
                CartItemReprosetory.Delete(cartItem);
                CartItemReprosetory.Save();

                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int userId = int.Parse(userIdString!);
                var cart = CartReprosetory.GetCartByUserId(userId);

                if (cart != null)
                {
                    cart.TotalPrice = cart.CartItems.Sum(i => i.Quantity * i.UnitPrice);
                    CartReprosetory.Save();

                    var carttotal = cart.TotalPrice;

                    return Json(new
                    {
                        success = true,
                        grandTotal = carttotal
                    });
                }
            }
            return Json(new { success = false, message = "المنتج غير موجود!" });
        }
        [HttpPost]
        public IActionResult ClearCart()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString!);

            var cart = CartReprosetory.GetCartByUserId(userId);

            if (cart != null)
            {
                CartReprosetory.DeleteCart(cart.Id);

                CartReprosetory.Save();

                return Json(new
                {
                    success = true,
                    grandTotal = 0,
                    message = "تم إفراغ السلة بنجاح"
                });
            }

            return Json(new { success = false, message = "السلة فارغة بالفعل" });
        }
        public async Task<IActionResult> Checkout()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userId = int.Parse(userIdString!);

            var cart = CartReprosetory.GetCartByUserId(userId);
            if (cart == null || cart.CartItems == null || !cart.CartItems.Any())
            {
                return RedirectToAction("Index", "Home");
            }
            var user = await UserManager.FindByIdAsync(userId.ToString());
            var model = new CheckOutViewModel
            {
                FullName = user.Name,
                PhoneNumber = user.PhoneNumber,
                CartItems = cart.CartItems,
                GrandTotal = cart.TotalPrice
            };
            return View("Checkout", model);
        }
        [Authorize]
        [HttpPost]
        public IActionResult PlaceOrder(CheckOutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                int usrId = int.Parse(userIdStr!);
                var cartData = CartReprosetory.GetCartByUserId(usrId);
                model.CartItems = cartData.CartItems.ToList();
                model.GrandTotal = cartData.TotalPrice;

                return View("Checkout", model);
            }
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString!);
            var cart = CartReprosetory.GetCartByUserId(userId);

            if (cart == null || !cart.CartItems.Any()) return RedirectToAction("Index", "Home");

            var order = new Order
            {
                AppUserId = userId,
                ShippingAddress = model.ShippingAddress,
                City = model.City,
                TotalPrice = cart.TotalPrice,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.Now,
                OrderItems = new List<OrderItem>() ,
                PaymentMethod = model.PaymentMethod,
                IsPaid = (model.PaymentMethod == "Visa") ? true : false,
            };
            foreach (var item in cart.CartItems)
            {
                var product = ProductRebrestory.GetProductById(item.ProductId);

                if (product != null)
                {
                    product.Stock -= item.Quantity;

                    ProductRebrestory.UpdateProduct(product);
                }
                order.OrderItems.Add(new OrderItem
                {
                   ProductId = item.ProductId,
                   Quantity = item.Quantity,
                   UnitPrice = item.UnitPrice,
                });

            }
                OrderRebrestory.AddOrder(order);
                OrderRebrestory.Save();


            if (cart != null)
            {
                CartReprosetory.DeleteCart(cart.Id);
                CartReprosetory.Save();
            }

            return RedirectToAction("OrderSuccess", new { orderId = order.Id });
        }
        [Authorize]
        [HttpGet]
        public IActionResult OrderSuccess(int orderId)
        {
            return View(orderId);
        }
    }
}
