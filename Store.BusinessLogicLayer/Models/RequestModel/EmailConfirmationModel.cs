using System.ComponentModel.DataAnnotations;

namespace Store.BusinessLogicLayer.Models.RequestModel
{
    public class EmailConfirmationModel
    {
        [Required]
        public string Email { set; get; }

        [Required]
        public string Code { set; get; }
    }
}
