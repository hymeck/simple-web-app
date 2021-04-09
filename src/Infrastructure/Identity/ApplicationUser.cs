using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegisteredAt { get; set; }
        public DateTime? LastLoginAt { get; set; }

        public bool IsActive { get; set; } = true;

        public string GetStatus() => IsActive ? "Active" : "Blocked";
        public string GetLastLoginAtString() => LastLoginAt.HasValue ? LastLoginAt.Value.ToString() : "Not logged yet";
    }
}
