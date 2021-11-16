using Microsoft.AspNetCore.Identity;
using System;

namespace AdminApp.Entities
{
    public class User : IdentityUser<long>
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string RefreshToken { get; set; }
        public User()
        {
            DateOfCreation = DateTime.Now;
        }
    }
}