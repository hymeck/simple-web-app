using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.User;
// ReSharper disable ParameterTypeCanBeEnumerable.Local

namespace WebApp.Controllers
{
    [Authorize]
    public partial class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICurrentUserService currentUserService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly Dictionary<string, Func<string[], Task>> Actions; // if-else replacement
        
        public HomeController(UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.currentUserService = currentUserService;
            this.signInManager = signInManager;

            Actions = new Dictionary<string, Func<string[], Task>>
            {
                {"block", Block},
                {"unlock", Unlock},
                {"delete", Delete}
            };
        }

        public IActionResult Index() => View(new UserListViewModel(userManager.Users));
        

        [HttpPost]
        // checkboxes contains ids of checked users
        public async Task<IActionResult> Action(string block, string unlock, string delete, string[] checkboxes)
        {
            if (AreValidParameters(block, unlock, delete, checkboxes, out var action))
                await Actions[action].Invoke(checkboxes);
            
            return RedirectToAction("Index", "Home");
        }
    }

    // auxiliary code
    public partial class HomeController
    {
        private async Task SignOut(string id)
        {
            if (id == currentUserService.UserId)
                await signInManager.SignOutAsync();
        }

        private async Task BlockOrUnlock(string[] ids, bool isBlocked)
        {
            foreach (var id in ids)
            {
                var user = await userManager.FindByIdAsync(id);
                await userManager.SetLockoutEnabledAsync(user, isBlocked);
                user.IsActive = !isBlocked;
                await userManager.UpdateAsync(user);
                await SignOut(id);
            }
        }

        private async Task Block(string[] ids) => await BlockOrUnlock(ids, true);

        private async Task Unlock(string[] ids) => await BlockOrUnlock(ids, false);

        private async Task Delete(string[] ids)
        {
            foreach (var id in ids)
            {
                var user = await userManager.FindByIdAsync(id);
                await userManager.DeleteAsync(user);
                await SignOut(id);
            }
        }
        
        private bool AreValidParameters(string block, string unlock, string delete, string[] checkboxes, out string actionName)
        {
            var isBlock = !string.IsNullOrEmpty(block);
            var isUnlock = !string.IsNullOrEmpty(unlock);
            var isDelete = !string.IsNullOrEmpty(delete);
            var isNullOrEmpty = checkboxes == null || checkboxes.Length == 0;

            actionName = null;

            if (isBlock)
                actionName = block;
            else if (isUnlock)
                actionName = unlock;
            else if (isDelete)
                actionName = delete;
            
            return isBlock || isUnlock || isDelete || !isNullOrEmpty;
        }
    }
}
