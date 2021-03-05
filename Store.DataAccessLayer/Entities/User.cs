using Microsoft.AspNetCore.Identity;

namespace Store.DataAccessLayer.Entities
{
    public class User : IdentityUser<long>
    {
        public string LastName { get; set; } 
        public string FirstName { get; set; }
        public bool IsBlocked { get; set; }
    }
}
