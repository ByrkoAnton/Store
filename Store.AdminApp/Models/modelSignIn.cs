
using System.ComponentModel.DataAnnotations;


namespace AdminApp.Models
{
    public class modelSignIn
    {
        [Required()]
        public string Email { get; set; }
        [Required()]
        public string Password { get; set; }
    }
}
