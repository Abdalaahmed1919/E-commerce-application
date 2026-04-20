using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace Simple_E_commers_App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole<int>> roleManager;

        public RoleController(RoleManager<IdentityRole<int>> roleManager)
        {
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View("Index");
        }
        // Add Role 
        [HttpPost] // مهم جداً تحط دي
        [ValidateAntiForgeryToken] // حماية إضافية
        public async Task<IActionResult> AddRole(RoleViewModel roleView)
        {
            if (ModelState.IsValid)
            {
                bool roleexist = await roleManager.RoleExistsAsync(roleView.RoleName);

                if (!roleexist)
                {
                    IdentityRole<int> identityRole = new IdentityRole<int>(roleView.RoleName);
                    IdentityResult result = await roleManager.CreateAsync(identityRole);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError("RoleName", "The Role Is Aleardy Exist");
                }
            }

            return View("Index",roleView);
        }
    }
}
