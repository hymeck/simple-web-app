using System.Threading.Tasks;
using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.User;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IDateTimeService dateTimeService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IDateTimeService dateTimeService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dateTimeService = dateTimeService;
        }

        [HttpGet]
        public IActionResult Register() =>
            View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid input");
                return View(model);
            }
            
            if (await userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError(string.Empty, "Email is already taken");
                return View(model);
            }
            
            if (await userManager.FindByNameAsync(model.Username) != null)
            {
                ModelState.AddModelError("", "Username is already taken");
                return View(model);
            }
                
            var user = new ApplicationUser
            {
                UserName = model.Username, 
                Email = model.Email, 
                RegisteredAt = dateTimeService.Now
            };
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Login", "Account");
            }
            
            foreach (var error in result.Errors) 
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            var viewModel = new UserLoginViewModel {ReturnUrl = returnUrl};
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid input");
                return View(model);
            }
            
            var result =
                await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(model.Username);
                user.LastLoginAt = dateTimeService.Now;
                await userManager.UpdateAsync(user);
                return RedirectToAction("Index", "Home");
            }
            
            ModelState.AddModelError(string.Empty, "No user account with specified data");
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
