using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Initialization;
using Microsoft.IdentityModel.Tokens;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.Sharing.Constants;
using System.Text;
using Microsoft.Extensions.Options;
using Store.BusinessLogicLayer.Configuration;

namespace Store.DataAccessLayer
{
    public static class Startup
    {
        public static void InitDal(this IServiceCollection services, IConfiguration config)
        {
            var tokenOptions = services.BuildServiceProvider().GetRequiredService<IOptions<TokenConfig>>().Value;

            services.Scan(scan => scan
            .FromAssemblyOf<IAuthorRepository>()
            .AddClasses()
            .AsMatchingInterface()
            .WithTransientLifetime());

            services.AddDbContext<ApplicationContext>(options =>
               options.UseSqlServer(config.GetConnectionString(Constants.Variables.CONNECTIONSTRING_NAME),
               b => b.MigrationsAssembly(Constants.Variables.MIGRATON_ASSMBLY_NAME))
              .UseLazyLoadingProxies());

            services.AddIdentity<User, IdentityRole<long>>(
                opts =>
                {
                    opts.Password.RequireDigit = true;
                    opts.Password.RequireLowercase = true;
                    opts.Password.RequireUppercase = true;
                    opts.Password.RequireNonAlphanumeric = true;
                    opts.Password.RequiredLength = Constants.User.PASSWORD_REQUIRED_LENGHT;
                })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                     .AddJwtBearer(options =>
                     {
                         options.RequireHttpsMetadata = false;
                         options.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuer = true,
                             ValidIssuer = tokenOptions.Issuer,
                             ValidateAudience = true,
                             ValidAudience = tokenOptions.Audience,
                             ValidateLifetime = true,
                             ValidateIssuerSigningKey = true,
                             IssuerSigningKey = new SymmetricSecurityKey
                             (Encoding.ASCII.GetBytes(tokenOptions.Key))
                         };

                     });
            services.InitialazerAsync().Wait();
        }
    }
}
