using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.User
{
    public class UserLoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [Display(Name = "Stay logged on")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}