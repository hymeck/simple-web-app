using Microsoft.AspNet.Identity.EntityFramework;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        
        public UserStatus UserStatus { get; set; }
    }
}
