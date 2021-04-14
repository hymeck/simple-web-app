using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels.User;

namespace WebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        
        
        public HomeController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            Actions = new Dictionary<string, Func<string[], Task>>
            {
                {"block", Block},
                {"unlock", Unlock},
                {"delete", Delete},
            };
        }

        public IActionResult Index() => View(new UserListViewModel(userManager.Users, null));

        private async Task Block(string[] users)
        {
            foreach (var u in users)
            {
                
            }
        }
        
        private async Task Unlock(string[] users)
        {
            
        }
        
        private async Task Delete(string[] users)
        {
            
        }

        private readonly Dictionary<string, Func<string[], Task>> Actions;
        
        [HttpPost]
#pragma warning disable 1998
        public async Task<IActionResult> Action(string block, string unlock, string delete, string[] checkboxes)
#pragma warning restore 1998
        {
            if (AreValidParameters(block, unlock, delete, checkboxes, out var actionName))
            {
                await Actions[actionName](checkboxes);
            }
            return RedirectToAction("Index", "Home");
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
