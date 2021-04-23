using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Store.BusinessLogicLayer.Servises.Interfaces;
using Store.BusinessLogicLayer.Servises;
using Store.PresentationLayer.Middlewares;
using Microsoft.IdentityModel.Tokens;
using Store.BusinessLogicLayer.Providers.Interfaces;
using Store.BusinessLogicLayer.Providers;
using Store.BusinessLogicLayer.Mappings;
using AutoMapper;
using Store.DataAccessLayer.Repositories.Base;
using System.Collections.Generic;
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.DataAccessLayer.Repositories;

namespace Store.PresentationLayer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<IPrintingEditionRepository, PrintingEditionRepository>();

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IEmailProvider, EmailProvider>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IJwtProvider, JwtProvider>();
            services.AddTransient<IRandomPasswordGeneratorProvider, RandomPasswordGeneratorProvider>();
            services.AddTransient<IAuthorServise, AuthorService>();
            services.AddTransient<IPrintingEditionService, PrintingEditionService>();

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Store.DataAccessLayer")));

            services.AddIdentity<User, IdentityRole<long>>(
                opts =>
                {
                    opts.Password.RequireDigit = true;
                    opts.Password.RequireLowercase = true;
                    opts.Password.RequireUppercase = true;
                    opts.Password.RequireNonAlphanumeric = true;
                    opts.Password.RequiredLength = 8;
                })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.InitialazerAsync().Wait();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            ValidateAudience = true,
                            ValidAudience = Configuration["Jwt:AUDIENCE"],
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                        };
                    });

            services.AddControllersWithViews();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AuthorMappingProfile());
                mc.AddProfile(new PrintingEditionMappingProfile());
                mc.AddProfile(new UserMappingProfile());
                mc.AddProfile(new PrintingEditionFiltrationMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<ErrorHandingMiddleware>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
