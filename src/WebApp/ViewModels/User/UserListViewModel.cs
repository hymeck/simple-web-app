using System.Linq;
using Infrastructure.Identity;

namespace WebApp.ViewModels.User
{
    public class UserListViewModel
    {
        public IQueryable<ApplicationUser> Users { get; }

        public UserListViewModel(IQueryable<ApplicationUser> users)
        {
            Users = users;
        }
    }
}
