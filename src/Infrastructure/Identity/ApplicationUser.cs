using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegisteredAt { get; set; }
        public DateTime LastLoginAt { get; set; }
        
        [NotMapped]
        public bool IsActive => !LockoutEnabled;
    }
}
