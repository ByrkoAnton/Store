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
using Store.DataAccessLayer.Repositories.Interfaces;
using Store.DataAccessLayer.Repositories;
using Store.Sharing.Constants;
using Stripe;
using System.Text;

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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = Configuration.GetValue<string>(Constants.JwtProviderConst.ISSUER),
                            ValidateAudience = true,
                            ValidAudience = Configuration.GetValue<string>(Constants.JwtProviderConst.AUDIENCE),
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey
                            (Encoding.ASCII.GetBytes(Configuration.GetValue<string>(Constants.JwtProviderConst.KEY)))
                        };
                    });
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<IPrintingEditionRepository, PrintingEditionRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderItemRepository, OrderItemRepository>();

            services.AddTransient<IUserAccountService, UserAccountService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IEmailProvider, EmailProvider>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITokenProvider, TokenProvider>();
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<IPrintingEditionService, PrintingEditionService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IOrderService, BusinessLogicLayer.Servises.OrderService>();
            services.AddTransient<IOrderItemService, OrderItemService>();

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Store.DataAccessLayer"))
                .UseLazyLoadingProxies());


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


            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AuthorMappingProfile());
                mc.AddProfile(new PrintingEditionMappingProfile());
                mc.AddProfile(new UserMappingProfile());
                mc.AddProfile(new PrintingEditionFiltrationMappingProfile());
                mc.AddProfile(new AuthorFiltrationMappingProfile());
                mc.AddProfile(new PaymentMappingProfile());
                mc.AddProfile(new OrderMappingProfile());
                mc.AddProfile(new OrderItemMappingProfile());
                mc.AddProfile(new OrderFiltrationMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddMvc();
            services.InitialazerAsync().Wait();

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];

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
