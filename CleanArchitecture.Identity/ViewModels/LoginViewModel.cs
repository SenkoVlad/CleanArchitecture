using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Identity.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
