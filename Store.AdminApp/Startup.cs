using AdminApp.Config;
using AdminApp.Providers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.BusinessLogicLayer.Serviñes;
using Store.BusinessLogicLayer.Serviñes.Interfaces;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;

namespace AdminApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
           //services.AddTransient<IUserAccountService, UserAccountService>();
           services.AddTransient<ITokenProvider, TokenProvider>();

            string connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddRazorPages();

            services.AddIdentity<User, IdentityRole<long>>()
               .AddEntityFrameworkStores<ApplicationContext>();

            services.ConfigureApplicationCookie(option => option.LoginPath = "/Auth/Signin");
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);

            services.AddControllersWithViews();

            services.Configure<TokenConfig>(Configuration);
            services.Configure<TokenConfig>(Configuration.GetSection("Jwt"));

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Error");

            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "MyArea",
                  pattern: "{area:exists}/{controller=AdminAccount}/{action=SignIn}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=AdminAccount}/{action=SignIn}/{id?}");
            });

        }
    }
}
