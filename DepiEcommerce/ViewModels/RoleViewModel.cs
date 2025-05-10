namespace DepiEcommerce.ViewModels
{
    using System.ComponentModel.DataAnnotations;


    public class RoleViewModel
    {
        [Display(Name = "Role Name")]
        [Required]
        public string RoleName { get; set; }
    }

}
