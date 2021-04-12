using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Store.DataAccessLayer;
using Store.DataAccessLayer.Entities;
using System;
using System.Threading.Tasks;

namespace Store.PresentationLayer
{
    public static class UserInitialazer
    {
        public static async Task InitialazerAsync(this IServiceCollection services)
        {
            var userManager = services.BuildServiceProvider().GetRequiredService<UserManager<User>>();
            var rolesManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole<long>>>();

            string email = "Byrko@gmail.com";
            string password = "1_Aa123456";
            string firstName = "Anton";
            string lastName = "Byrko";
        
            try
            {
                var role = await rolesManager.FindByNameAsync("admin");
                if (role is null)
                {
                    await rolesManager.CreateAsync(new IdentityRole<long>("admin"));
                }
            }

            catch (System.Exception roleExeption )
            {
                var ex = roleExeption;
            }

            if (await userManager.FindByEmailAsync(email) is null)
            {
                User admin = new User { Email = email, UserName = email, FirstName = firstName, LastName = lastName};
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
