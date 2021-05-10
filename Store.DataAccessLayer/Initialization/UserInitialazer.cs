using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Store.DataAccessLayer.Entities;
using System;
using System.Threading.Tasks;
using static Store.DataAccessLayer.Enums.Enums;
using Store.Sharing.Constants;


namespace Store.PresentationLayer
{
    public static class UserInitialazer
    {
        public static async Task InitialazerAsync(this IServiceCollection services)
        {
            var userManager = services.BuildServiceProvider().GetRequiredService<UserManager<User>>();
            var rolesManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole<long>>>();

            string email = Constants.UserConstants.EMAIL_INIT;
            string password = Constants.UserConstants.PASSWORD;
            string firstName = Constants.UserConstants.FIRST_NAME;
            string lastName = Constants.UserConstants.LAST_NAME;

            try
            {
                var role = await rolesManager.FindByNameAsync(UserRole.Admin.ToString().ToLower());
                if (role is null)
                {
                    await rolesManager.CreateAsync(new IdentityRole<long>(UserRole.Admin.ToString().ToLower()));
                }
            }

            catch (Exception roleExeption)
            {
                var ex = roleExeption;
            }

            if (await userManager.FindByEmailAsync(email) is null)
            {
                User admin = new User {Email = email, UserName = email, FirstName = firstName, LastName = lastName};
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Constants.UserConstants.ROLE_ADMIN);
                }
            }
        }
    }
}
