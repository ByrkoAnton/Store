using System.ComponentModel.DataAnnotations;

namespace Store.BusinessLogicLayer.Models.RequestModel
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
