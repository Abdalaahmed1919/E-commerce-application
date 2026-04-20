using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simple_E_commers_App.Models;
using Simple_E_commers_App.Reprositrory;
using System.Threading.Tasks;

namespace Simple_E_commers_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IOrderRebrestory OrderRebrestory { get; }
        public UserManager<AppUser> UserManager { get; }
        public IProductRebrestory ProductRebrestory { get; }
        public IOrderItemsRebrestory OrderItemsRebrestory { get; }
        public ICatograyRebrestory CatograyRebrestory { get; }

        public AdminController(IOrderRebrestory orderRebrestory, UserManager<AppUser> userManager, IProductRebrestory productRebrestory
            ,IOrderItemsRebrestory orderItemsRebrestory 
            ,ICatograyRebrestory catograyRebrestory)
        {
            OrderRebrestory = orderRebrestory;
            UserManager = userManager;
            ProductRebrestory = productRebrestory;
            OrderItemsRebrestory = orderItemsRebrestory;
            CatograyRebrestory = catograyRebrestory;
        }


        public IActionResult Index()
        {
            IEnumerable<Order> Order = OrderRebrestory.GetAllOrdersIncudingUsersAndOrderItems();

            OrdersAdminViewModel viewModel = new OrdersAdminViewModel
            {
                Orders = Order.OrderByDescending(o => o.CreatedAt).Take(5),
                TotalMoney = Order.Sum(o => o.TotalPrice),
                TotalOrders = Order.Count(),
                totalUsers = UserManager.Users.Count(),
                ActiveProducts = ProductRebrestory.GetAllProducts().Where(p => p.Stock > 0).Count(),
            };
            return View("Index", viewModel);
        }
        public IActionResult Orders()
        {
            var orders = OrderRebrestory.GetAllOrdersIncudingUsersAndOrderItems();
            OrdersAdminViewModel viewModel = new OrdersAdminViewModel
            {
                Orders = orders.OrderByDescending(o => o.CreatedAt),
                PinndingOrdersCount = OrderRebrestory.getPinndingOrderCount(),
                ShippingOrdersCount = OrderRebrestory.getShippedOrderCount(),
                CompletedOrderCount = OrderRebrestory.getCompletedOrderCount(),
                ProcessingOrderCount = OrderRebrestory.getProcessingOrderCount(),
            };
            return View("Orders", viewModel);
        }
        [HttpGet]
        public IActionResult OrderFilters(string status = "All", string search = "", DateTime? date = null)
        {
            var orders = OrderRebrestory.GetAllOrdersIncudingUsersAndOrderItems();

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                orders = orders.Where(o => o.Status.ToString().Trim().ToLower() == status.Trim().ToLower());
            }
            if (!string.IsNullOrEmpty(search))
            {
                orders = orders.Where(o => o.User.Name.Contains(search) || o.Id.ToString() == search);
            }
            if (date.HasValue)
            {
                orders = orders.Where(o => o.CreatedAt == date);
            }
            return PartialView("_orderdetails", orders.ToList());
        }
        [HttpGet]
        public IActionResult UpdateOrder(int id, string status)
        {
            try
            {
                var order = OrderRebrestory.GetOrderById(id);
                if (order == null) return Json(new { success = false, message = "الأوردر مش موجود!" });
                if (Enum.TryParse<OrderStatus>(status, out var parsedStatus))
                {
                    order.Status = parsedStatus;
                }
                OrderRebrestory.Save();

                return Json(new { success = true, message = "تم التحديث بنجاح!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حصلت مشكلة: " + ex.Message });
            }
        }

        public IActionResult Products()
        {
            var products = ProductRebrestory.GetAllProductsIcludesCatogary();
            var OrderItems = OrderItemsRebrestory.GetAllOrderItems();
            var catogries = CatograyRebrestory.GetAllCategories();

            ProductsAndOrderItemsIncludingCatogry vm = new ProductsAndOrderItemsIncludingCatogry
            {
                Products = products,
                OrderItem = OrderItems,
                Categories = catogries 
            };
            return View("Products", vm);
        }
        [HttpPost]
        public IActionResult AddProduct (ProductsAndOrderItemsIncludingCatogry product)
        {
            var producttpSave = product.NewProduct;
            ProductRebrestory.AddProduct(producttpSave);
            ProductRebrestory.Save();
            return RedirectToAction("Products", product);
            
        }
        public IActionResult DeletProduct (int id)
        {
            ProductRebrestory.DeleteProduct(id);
            ProductRebrestory.Save();
            return RedirectToAction("Products");
        }

        [HttpGet]
        public IActionResult GetProductForEdit(int id) {
         var product = ProductRebrestory.GetProductById(id);
            if (product == null) return NotFound();

            return Json(new
            {
                id = product.Id,
                name = product.Name,
                description = product.Description,
                price = product.Price,
                stock = product.Stock,
                categoryId = product.CategoryId,
                imageUrl = product.ImageUrl
            });
        }

        [HttpPost]
        public IActionResult EditProduct(Product model)
        {
            var productInDb = ProductRebrestory.GetProductById(model.Id);

            if (productInDb == null)
            {
                return NotFound();
            }

            productInDb.Name = model.Name;
            productInDb.Description = model.Description;
            productInDb.Price = model.Price;
            productInDb.Stock = model.Stock;
            productInDb.CategoryId = model.CategoryId;


            ProductRebrestory.UpdateProduct(productInDb); 
            ProductRebrestory.Save(); 

            
            return RedirectToAction("Products");
        }
        [HttpGet]
        public IActionResult FilterProducts (string search , int? categoryId, string Status) 
        {
            var products = ProductRebrestory.GetAllProductsIcludesCatogary();
            var OrderItems = OrderItemsRebrestory.GetAllOrderItems();
            if (!string.IsNullOrEmpty(search))
                products = products.Where(p => p.Name.Contains(search));
            
            if (categoryId.HasValue)
                products = products.Where(p => p.CategoryId == categoryId.Value);

            if (!string.IsNullOrEmpty(Status))
            {
                if (Status == "Active") products = products.Where(p => p.Stock > 0);
                else if (Status == "Out Of Stock") products = products.Where(p => p.Stock <= 0);
            }

            var vm = new ProductsAndOrderItemsIncludingCatogry()
            {
                Products = products,
                OrderItem = OrderItems,
            };
            return PartialView("_Product", vm);
        }
        public IActionResult customers ()
        {
            var users = UserManager.Users.Include(u => u.Orders).ToList();
            return View("Customers", users);
        }
        public async Task<IActionResult> DeleteCustomet (int id)
        {
            var user = await UserManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();

            var result = await UserManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("customers"); 
            }

            return BadRequest("حصلت مشكلة واحنا بنمسح العميل");
        }
        [HttpGet]
        public async Task<IActionResult> FilterCustomers(string search)
        {
            var query = UserManager.Users.Include(u => u.Orders).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(u => u.Name.Contains(search) || u.Email.Contains(search));
            }

            var users = await query.ToListAsync();

            return PartialView("_Customers", users);
        }
        public IActionResult Categories ()
        {
            var catogries = CatograyRebrestory.GetAllCategoriesIncludingProducts();
            return View("Categories" , catogries);
        }
        [HttpPost]
        public IActionResult AddCategory (string name)
        {
            if (ModelState.IsValid)
            {
                var catogry = new Category
                {
                    Name = name
                };
                CatograyRebrestory.AddCategory(catogry);
                CatograyRebrestory.Save();
                return RedirectToAction("Categories");
            }
            return NoContent();
        }
        public IActionResult DeleteCategory (int id)
        {
            CatograyRebrestory.DeleteCategory(id);
            CatograyRebrestory.Save();
            return NoContent();
        }
        [HttpPost]
        public IActionResult EditCategory(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return Json(new { success = false, message = "اسم القسم مطلوب" });
            }

            var category = CatograyRebrestory.GetCategoryById(id);

            if (category == null)
            {
                return Json(new { success = false, message = "القسم ده مش موجود" });
            }

            category.Name = name;

            CatograyRebrestory.Save();

            return Json(new { success = true });
        }
    }
}
