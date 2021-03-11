using System.ComponentModel.DataAnnotations;
namespace Store.BusinessLogicLayer.Models
{
    public class SignInModel
    {
        //[Required]
        public string Email { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
