using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Simple_E_commers_App.Models;
using Simple_E_commers_App.Reprositrory;

namespace Simple_E_commers_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ICatograyRebrestory categoryRebrestory { get; }
        public IProductRebrestory ProductRebrestory { get; }

        public HomeController(ILogger<HomeController> logger, ICatograyRebrestory categoryRebrestory 
            , IProductRebrestory productRebrestory)
        {
            _logger = logger;
            this.categoryRebrestory = categoryRebrestory;
            ProductRebrestory = productRebrestory;
        }
        public IActionResult Index()
        {
            ViewBag.Categories = categoryRebrestory.GetAllCategories();
            ViewBag.Products = ProductRebrestory.GetTheChepstProducts();
            return View("Index");
        }
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
    }
}
