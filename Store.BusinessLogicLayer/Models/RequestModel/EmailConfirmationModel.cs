using System.ComponentModel.DataAnnotations;//TODO new line
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
