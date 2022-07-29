//TODO extra lines
using System.ComponentModel.DataAnnotations;


namespace AdminApp.Models
{
    public class modelSignIn//TODO naming
    {
        [Required()]//TODO parentheses redundant
        public string Email { get; set; }
        [Required()]//TODO parentheses redundant
        public string Password { get; set; }
    }
}
