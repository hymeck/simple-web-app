using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.User
{
    public class UserRegisterViewModel
    {
        [Required]
        [Display(Name = "Username")]
        [MaxLength(30)]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
 
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords are not matched")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string PasswordConfirm { get; set; }
    }
}
