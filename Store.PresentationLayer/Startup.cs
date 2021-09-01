using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.PresentationLayer.Middlewares;
using Store.Sharing.Constants;
using Stripe;
using Microsoft.OpenApi.Models;
using Store.BusinessLogicLayer;

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
            services.AddCors();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Constants.Swagger.VERSION, new OpenApiInfo
                {
                    Title = Constants.Swagger.VERSION,
                    Version = Constants.Swagger.VERSION
                });
            });

            services.InitBll(Configuration);

            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddMvc();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder => builder.AllowAnyOrigin()
                             .AllowAnyHeader()
                             .AllowAnyMethod());

            StripeConfiguration.ApiKey = Configuration.GetValue<string>(Constants.Stripe.SECRET_KEY);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(Constants.Swagger.ROUTE, Constants.Swagger.NAME);
            });

            app.UseHsts();
            app.UseMiddleware<ErrorHandingMiddleware>();
            app.UseHttpsRedirection();
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
