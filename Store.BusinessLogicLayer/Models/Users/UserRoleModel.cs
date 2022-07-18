//TODO extra line
using System.ComponentModel.DataAnnotations;

namespace Store.BusinessLogicLayer.Models.Users
{
    public class UserRoleModel
    {
        [Required]
        public string RoleName { get; set; } 
    }
}
