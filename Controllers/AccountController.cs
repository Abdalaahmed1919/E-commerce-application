using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Threading.Tasks;

namespace Simple_E_commers_App.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<AppUser> UserManager { get; }
        public SignInManager<AppUser> SignInManager { get; }

        public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager) {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View("Register");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveRegister (RegisterViewModel registerView)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser
                {
                    Name = registerView.FirstName + " " + registerView.LastName,
                    Email = registerView.Email,
                    UserName = registerView.Email,
                    PhoneNumber = registerView.PhoneNumber,
                    CreatedAt = DateTime.Now
                };
                IdentityResult result = await UserManager.CreateAsync(appUser, registerView.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                     await SignInManager.SignInAsync(appUser, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View("Register", registerView);
        }

        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null) {
                return Json(true);
            }
            return Json("The Email Is Already Taken");
        }
        public async Task<IActionResult> IsEmailFound (string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                return Json(true);
            }
            return Json("The Email Is Not Found");
        }
        public IActionResult Login ()
        {
            return View("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveLogin (LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await UserManager.FindByEmailAsync(loginViewModel.Email);
                if (user != null)
                {
                    bool found = await UserManager.CheckPasswordAsync(user, loginViewModel.Password);
                    if (found)
                    {
                        await SignInManager.SignInAsync(user, loginViewModel.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                }
                    ModelState.AddModelError("", "UserName Or Password Is Wrong");
                }
               return View("Login" , loginViewModel);
            }
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Login");
           
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    
    }
}
