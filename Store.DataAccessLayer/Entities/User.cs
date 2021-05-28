using Microsoft.AspNetCore.Identity;
using System;

namespace Store.DataAccessLayer.Entities
{
    public class User : IdentityUser<long>
    {
        public string LastName { get; set; } 
        public string FirstName { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string RefreshToken { get; set; }
        public User()
        {
            DateOfCreation = DateTime.Now;
        }
    }
}
