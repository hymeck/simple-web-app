using System.Threading.Tasks;
using Application.Common;

namespace Application.Interfaces
{
    public interface IIdentityService
    {
        Task<(Result Result, string UserId)> CreateUserAsync(string username, string password);
        Task<string> GetUsernameAsync(string userId);
        Task<Result> DeleteUserAsync(string userId);
        Task<Result> BlockUserAsync(string userId);
        Task<Result> UnblockUserAsync(string userId);
    }
}
