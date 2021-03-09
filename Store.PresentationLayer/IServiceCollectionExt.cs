using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Store.DataAccessLayer;
using Store.DataAccessLayer.Entities;
using System.Threading.Tasks;

namespace Store.PresentationLayer
{
    public static class IServiceCollectionExt
    {
        public static async Task InitialazerAsync(this IServiceCollection services)
        {
            var userManager = services.BuildServiceProvider().GetRequiredService<UserManager<User>>();
            var rolesManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole<long>>>();

            string adminEmail = "admin@gmail.com";
            string password = "_Aa123456";
        
            try
            {
                var a = await rolesManager.FindByNameAsync("admin");
                if (a == null)
                {
                    await rolesManager.CreateAsync(new IdentityRole<long>("admin"));
                }
            }
            catch (System.Exception ex)
            {
                var excec = ex;
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail};
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
