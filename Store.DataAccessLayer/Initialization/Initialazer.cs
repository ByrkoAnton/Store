using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Store.DataAccessLayer.Entities;
using System.Threading.Tasks;
using static Store.DataAccessLayer.Enums.Enums;
using Store.Sharing.Constants;
using Microsoft.EntityFrameworkCore;
using static Store.DataAccessLayer.Enums.Enums.EditionEnums;

namespace Store.PresentationLayer
{
    public static class Initialazer
    {
        public static async Task InitialazerAsync(this IServiceCollection services)
        {
            var userManager = services.BuildServiceProvider().GetRequiredService<UserManager<User>>();
            var rolesManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole<long>>>();

            string email = Constants.UserConstants.EMAIL_INIT;
            string password = Constants.UserConstants.PASSWORD;
            string firstName = Constants.UserConstants.FIRST_NAME;
            string lastName = Constants.UserConstants.LAST_NAME;

            var roleAdmin = await rolesManager.FindByNameAsync(UserRole.Admin.ToString().ToLower());
            if (roleAdmin is null)
            {
                await rolesManager.CreateAsync(new IdentityRole<long>(UserRole.Admin.ToString().ToLower()));
            }

            var roleUser = await rolesManager.FindByNameAsync(UserRole.User.ToString().ToLower());
            if (roleUser is null)
            {
                await rolesManager.CreateAsync(new IdentityRole<long>(UserRole.User.ToString().ToLower()));
            }

            if (await userManager.FindByEmailAsync(email) is null)
            {
                User admin = new User { Email = email, UserName = email, FirstName = firstName, LastName = lastName };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Constants.UserConstants.ROLE_ADMIN);
                }
            }
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Author>()
            .HasData(
            new Author
            {
                Id = 1L,
                Name = "FirstAuthor"
            });

            modelBuilder.Entity<PrintingEdition>()
            .HasData(
            new PrintingEdition
            {
                Id = 1L,
                Title = "FirstEdition",
                Description = "FirstEdition",
                Currency = Currency.USD,
                Price = 5,
                Status = "Avalible",
                Type = PrintingEditionType.Book,
            });

            modelBuilder.Entity<Author>()
           .HasMany(author => author.PrintingEditions)
           .WithMany(edition => edition.Authors)
           .UsingEntity(join => join.HasData(new { AuthorsId = 1L, PrintingEditionsId = 1L }));
        }
    }
}
