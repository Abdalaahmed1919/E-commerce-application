using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Simple_E_commers_App.Models;
using Simple_E_commers_App.Reprositrory;
using Simple_E_commers_App.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Simple_E_commers_App.Controllers
{
    public class ProductController : Controller
    {

        public IProductRebrestory ProductRebrestory { get; }
        public ICatograyRebrestory CatograyRebrestory { get; }
        public UserManager<AppUser> UserManager { get; }
        public ICartReprosetory CartReprosetory { get; }

        public ProductController(IProductRebrestory productRebrestory, ICatograyRebrestory catograyRebrestory, UserManager<AppUser> userManager
            , ICartReprosetory cartReprosetory)
        {
            ProductRebrestory = productRebrestory;
            CatograyRebrestory = catograyRebrestory;
            UserManager = userManager;
            CartReprosetory = cartReprosetory;
        }

        public IActionResult Index()
        {
            var viewModel = new ProductsAndCategoriesViewModel
            {
                Products = ProductRebrestory.GetAllProducts(),
                Categories = CatograyRebrestory.GetAllCategories(),
            };
            return View("Index", viewModel);
        }

        public IActionResult Pagination(int pageNumber = 1, int SortType = 1, string catogryName = "All", decimal? maxPrice = null)
        {
            var products = ProductRebrestory.GetAllProductsIcludesCatogary();

            if (catogryName != "All") {
                products = products.Where(p => p.Category.Name == catogryName);
            }
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            switch (SortType)
            {
                case 2:
                    products = products.OrderBy(p => p.Price);
                    break;

                case 3:
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case 4:
                    products = products.OrderByDescending(p => p.Id);
                    break;

                case 1:
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }
            products = products.Skip((pageNumber - 1) * 10).Take(10);
            return PartialView("_ProductsList", products);

        }

        //public IActionResult Sort (int SortType)
        //{
        //    var products = ProductRebrestory.GetAllProductsIcludesCatogary();
        //    switch (SortType)
        //    {
        //        case 2:
        //            products = products.OrderBy(p => p.Price);
        //            break;

        //        case 3:
        //            products = products.OrderByDescending(p => p.Price);
        //            break;

        //        case 1 :
        //        default:
        //            products = products.OrderBy(p => p.Name); 
        //            break;
        //    }
        //    return PartialView("_ProductsList" , products);
        //}
        [Authorize]
        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = int.Parse(userIdString!);
            var product = ProductRebrestory.GetProductById(productId);

            if (product == null ||  product.Stock == 0)
            {
                return Json(new { success = false, message = "نعتذر، هذا المنتج غير متوفر حالياً في المخزن!" });
            }

            var cart = CartReprosetory.GetCartByUserId(userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    AppUserId = userId,
                    CartItems = new List<CartItem>()
                };
                CartReprosetory.AddCart(cart);
                CartReprosetory.Save();
            }
            var exitingCartItmes = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (exitingCartItmes != null)
            {
                exitingCartItmes.Quantity++;
            }
            else
            {
                CartItem newItem = new CartItem
                {
                    ProductId = productId,
                    CartId = cart.Id,
                    Quantity = 1,
                    UnitPrice = product.Price
                };
                cart.CartItems.Add(newItem);
            }
            cart.TotalPrice = cart.CartItems.Sum(item => item.Quantity * item.UnitPrice);
            CartReprosetory.Save();
            return Json(new { success = true, message = "تمت الإضافة للسلة بنجاح! 🛒" });         
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var product = ProductRebrestory.GetProductById(id);

            if (product == null)
            {
                return RedirectToAction("Index"); 
            }

            return View(product);
        }
    }
}
