using System.Collections.Generic;
using System.Linq;
using Infrastructure.Identity;

namespace WebApp.ViewModels.User
{
    public sealed class Checkbox
    {
        public int Id { get; set; }
        public bool Value { get; set; }

        public Checkbox()
        {
        }

        public Checkbox(int id, bool value)
        {
            Id = id;
            Value = value;
        }
    }
    
    public class UserListViewModel
    {
        public IQueryable<ApplicationUser> Users { get; }
        public IList<Checkbox> Checkboxes { get; }

        public UserListViewModel(IQueryable<ApplicationUser> users, IList<Checkbox> checkboxes)
        {
            Users = users;
            Checkboxes = checkboxes;
        }
    }
}
