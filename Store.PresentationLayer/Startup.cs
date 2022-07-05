using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.PresentationLayer.Middlewares;
using Store.Sharing.Constants;
using Stripe;
using Microsoft.OpenApi.Models;
using Store.BusinessLogicLayer;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Store.DataAccessLayer.Repositories.Base;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();

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

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, Constants.AreaConstants.AREAS_STYLES_PATH)),
                RequestPath = Constants.AreaConstants.AREAS_STYLES_SHORT_PATH
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, Constants.AreaConstants.AREAS_VIEVS_PATH)),
                RequestPath = Constants.AreaConstants.AREAS_VIEWS_SHORT_PATH
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: Constants.AreaConstants.AREA_NAME,
                pattern: Constants.AreaConstants.AREA_PATTERN);

                endpoints.MapControllerRoute(
                    name: Constants.MapControllerRoute.NAME,
                    pattern: Constants.MapControllerRoute.PATERN);
            });

        }
    }
}
