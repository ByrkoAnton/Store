using Store.BusinessLogicLayer.Models.Base;
using Store.Sharing.Constants;
using System.ComponentModel.DataAnnotations;

namespace Store.BusinessLogicLayer.Models.Users
{
    public class UserModel : BaseModel
    {
        [Required(ErrorMessage = Constants.Error.REGISRATION_FAILD_NO_IMAIL_IN_MODEL)]
        public string Email { get; set; }

        [Required(ErrorMessage = Constants.Error.REGISRATION_FAILD_NO_LAST_NAME_IN_MODEL)]
        public string LastName { get; set; }

        [Required(ErrorMessage = Constants.Error.REGISRATION_FAILD_NO_FIRST_NAME_IN_MODEL)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = Constants.Error.REGISRATION_FAILD_NO_PASSWORD_IN_MODEL)]
        public string Password { get; set; }
        public bool IsBlocked { get; set; } 
    }
}
