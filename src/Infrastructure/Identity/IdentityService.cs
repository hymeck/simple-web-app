using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDateTimeService dateTimeService;

        public IdentityService(UserManager<ApplicationUser> userManager, IDateTimeService dateTimeService)
        {
            this.userManager = userManager;
            this.dateTimeService = dateTimeService;
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(string username, string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                RegisteredAt = dateTimeService.Now
            };

            var result = await userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<string> GetUsernameAsync(string userId)
        {
            var user = await userManager.Users.FirstAsync(u => u.Id == userId);
            return user.UserName;
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
                return await DeleteUserAsync(user);

            return Result.Success();
        }

        public async Task<Result> BlockUserAsync(string userId)
        {
            var user = userManager.Users.SingleOrDefault(u => u.Id == userId);
            return await SetUserStatusAsync(user, true);
        }

        public async Task<Result> UnblockUserAsync(string userId)
        {
            var user = userManager.Users.SingleOrDefault(u => u.Id == userId);
            return await SetUserStatusAsync(user, false);
        }
        
        private async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await userManager.DeleteAsync(user);
            return result.ToApplicationResult();
        }

        private async Task<Result> UpdateUserAsync(ApplicationUser user)
        {
            var result = await userManager.UpdateAsync(user);
            return result.ToApplicationResult();
        }

        private async Task<Result> SetUserStatusAsync(ApplicationUser user, bool lockoutEnabled)
        {
            if (user != null)
            {
                user.LockoutEnabled = lockoutEnabled;
                return await UpdateUserAsync(user);
            }
            
            return Result.Success();
        }
    }
}
