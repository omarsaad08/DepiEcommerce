using System.ComponentModel.DataAnnotations;

namespace DepiEcommerce.ViewModels
{
    public class LoginUserViewModel
    {

        [EmailAddress]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }


    }
}
