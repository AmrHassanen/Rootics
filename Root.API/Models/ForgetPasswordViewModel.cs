using System.ComponentModel.DataAnnotations;

namespace Root.API.Models
{
    public class ForgetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string ? Email { get; set; }
    }
}
