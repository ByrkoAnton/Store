
using Store.Sharing.Constants;
using System.ComponentModel.DataAnnotations;

namespace Store.BusinessLogicLayer.Models.Users
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = Constants.Error.NO_PASSWORD)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = Constants.Error.NO_PASSWORD_CONFIRM)]
        public string NewPassword { get; set; }
    }
}
