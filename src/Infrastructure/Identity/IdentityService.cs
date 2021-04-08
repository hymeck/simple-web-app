using System.Threading.Tasks;
using Application.Common;
using Application.Interfaces;
using Microsoft.AspNet.Identity;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> GetUsernameAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result> BlockUserAsync(string userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Result> UnblockUserAsync(string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
