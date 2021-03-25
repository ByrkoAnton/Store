using System.ComponentModel.DataAnnotations;
namespace Store.BusinessLogicLayer.Models
{
    public class UserSignInModel
    {
        [Required(ErrorMessage = Constants.Constants.Error.LOGIN_FAILD_NO_USER_WITH_THIS_EMAIL)]
        public string Email { get; set; }

        [Required(ErrorMessage = Constants.Constants.Error.LOGIN_FAILD_WRONG_PASSWORD)]
        public string Password { get; set; }
    }
}
