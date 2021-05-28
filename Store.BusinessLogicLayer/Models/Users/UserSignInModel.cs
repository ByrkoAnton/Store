using Store.Sharing.Constants;
using System.ComponentModel.DataAnnotations;
namespace Store.BusinessLogicLayer.Models
{
    public class UserSignInModel
    {
        [Required(ErrorMessage = Constants.Error.LOGIN_FAILD_EMAIL)]
        public string Email { get; set; }

        [Required(ErrorMessage = Constants.Error.LOGIN_FAILD_WRONG_PASSWORD)]
        public string Password { get; set; }
    }
}
