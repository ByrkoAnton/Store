using Store.Sharing.Constants;
using System.ComponentModel.DataAnnotations;

namespace Store.BusinessLogicLayer.Models//TODO wrong namespace
{
    public class UserSignInModel
    {
        [Required(ErrorMessage = Constants.Error.NO_EMAIL_MSG)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = Constants.Error.NO_PASSWORD_MSG)]
        public string Password { get; set; }
    }
}
