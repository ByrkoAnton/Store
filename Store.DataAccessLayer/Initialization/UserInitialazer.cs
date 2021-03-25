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

            string email = "Byrko2@gmail.com";
            string password = "1_Aa123456";
            string firstName = "Anton2";
            string lastName = "Byrko2";
            string name = "AB2";
        
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
        public static async Task<ActionResult> LockOut(this IServiceCollection services, string userName)
        {
            var userManager = services.BuildServiceProvider().GetRequiredService<UserManager<User>>();

            User user = await userManager.FindByNameAsync(userName);
            if (user is null)
            {
                return new ObjectResult("user not found"); 
            }

            user.LockoutEnabled = true;
            user.LockoutEnd = DateTime.UtcNow.AddYears(100);
            await userManager.UpdateAsync(user);

            return new ObjectResult("user locked out");
        }

        public static async Task<ActionResult> ChangeUserName(this IServiceCollection services, string userName, string newUserName)
        {
            var userManager = services.BuildServiceProvider().GetRequiredService<UserManager<User>>();

            User user = await userManager.FindByNameAsync(userName);
            if (user is null)
            {
                return new ObjectResult("user not found");
            }

            user.UserName = newUserName;
            await userManager.UpdateAsync(user);

            return new ObjectResult("user name changed");
        }
    }
}
