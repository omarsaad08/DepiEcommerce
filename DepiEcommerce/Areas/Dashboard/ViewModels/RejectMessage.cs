using System.ComponentModel.DataAnnotations;

namespace DepiEcommerce.Areas.Dashboard.ViewModels
{
    public class RejectMessage
    {
        [Required]
        [Display(Name = "Reject Message")]
        public string RejectMsg { get; set; }
        public string UserName { get; set; }
        public int? OrderId { get; set; }
    }
}
