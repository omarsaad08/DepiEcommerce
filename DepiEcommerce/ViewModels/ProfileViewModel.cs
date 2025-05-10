using System.ComponentModel.DataAnnotations;

namespace DepiEcommerce.ViewModels
{
    public class ProfileViewModel
    {

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Display(Name = "Email")]
        public string Username { get; set; }
        public string PhoneNumber { get; set; }

    }
}
