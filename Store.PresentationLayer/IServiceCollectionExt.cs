//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.DependencyInjection;
//using Store.DataAccessLayer;
//using Store.DataAccessLayer.Entities;
//using System.Threading.Tasks;

//namespace Store.PresentationLayer
//{
//    public static class IServiceCollectionExt
//    {
//        public static async Task InitialazerAsync(this IServiceCollection services)
//        {
//            var userManager = services.BuildServiceProvider().GetRequiredService<UserManager<User>>();
//            var rolesManager = services.BuildServiceProvider().GetRequiredService<RoleManager<IdentityRole<long>>>();

//            string email = "Byrko@gmail.com";
//            string password = "_Aa123456";
//            string firstName = "Anton";
//            string lastName = "Byrko";
//            string name = "AB";
        
//            try
//            {
//                var role = await rolesManager.FindByNameAsync("admin");
//                if (role is null)
//                {
//                    await rolesManager.CreateAsync(new IdentityRole<long>("admin"));
//                }
//            }

//            catch (System.Exception roleExeption )
//            {
//                var excec = roleExeption;
//            }

//            if (await userManager.FindByNameAsync(email) is null)
//            {
//                User admin = new User { Email = email, UserName = name, FirstName = firstName, LastName = lastName};
//                IdentityResult result = await userManager.CreateAsync(admin, password);
//                if (result.Succeeded)
//                {
//                    await userManager.AddToRoleAsync(admin, "admin");
//                }
//            }
//        }
//    }
//}
