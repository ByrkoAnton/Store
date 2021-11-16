using AdminApp.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace AdminApp.Models
{
    public class ApplicationContext: IdentityDbContext<User, IdentityRole<long>, long>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
           : base(options)
        {
            Database.Migrate();
        }
    }
}
