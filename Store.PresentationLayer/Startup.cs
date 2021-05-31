using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Store.DataAccessLayer.AppContext;
using Store.DataAccessLayer.Entities;
using Store.DataAccessLayer.Initialization;
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
using Microsoft.OpenApi.Models;

namespace Store.PresentationLayer
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Constants.Swagger.VERSION, new OpenApiInfo { Title = Constants.Swagger.VERSION,
                    Version = Constants.Swagger.VERSION });
            });
            services.AddSwaggerGen();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = Configuration.GetValue<string>(Constants.JwtProvider.ISSUER),
                            ValidateAudience = true,
                            ValidAudience = Configuration.GetValue<string>(Constants.JwtProvider.AUDIENCE),
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey
                            (Encoding.ASCII.GetBytes(Configuration.GetValue<string>(Constants.JwtProvider.KEY)))
                        };
                    });
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<IPrintingEditionRepository, PrintingEditionRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderItemRepository, OrderItemRepository>();
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddTransient<IUserAccountService, UserAccountService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IEmailProvider, EmailProvider>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITokenProvider, TokenProvider>();
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<IPrintingEditionService, PrintingEditionService>();
            services.AddTransient<IPaymentService, PaymentService>();
            services.AddTransient<IOrderService, BusinessLogicLayer.Servises.OrderService>();        

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(Constants.Variables.CONNECTIONSTRING_NAME),
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StripeConfiguration.ApiKey = Configuration.GetValue<string>(Constants.Stripe.SECRET_KEY);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(Constants.Swagger.ROUTE, Constants.Swagger.NAME);
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(Constants.Error.ERROR);
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
                    name: Constants.MapControllerRoute.NAME,
                    pattern: Constants.MapControllerRoute.PATERN);
            });
        }
    }
}
