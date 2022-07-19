using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Store.BusinessLogicLayer.Serviсes.Interfaces;
using AutoMapper;
using Store.Sharing.Constants;
using System;
using Store.DataAccessLayer;
using Store.Sharing.Configuration;

namespace Store.BusinessLogicLayer
{
    public static class Startup
    {
        public static void InitBll(this IServiceCollection services, IConfiguration config)
        { 
            services.Scan(scan => scan
           .FromAssemblyOf<IUserAccountService>()
            .AddClasses()
            .AsMatchingInterface()
            .WithTransientLifetime());

            var configuration = new MapperConfiguration(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
            IMapper mapper = configuration.CreateMapper();
            services.AddSingleton(mapper);

            services.Configure<EmailConfig>(config.GetSection(Constants.EmailProvider.EMAIL_SECTION));
            services.Configure<TokenConfig>(config.GetSection(Constants.JwtProvider.JWT_SECTION));
            services.Configure<ConnectionStringConfig>(config.GetSection(Constants.ConnectionsConstants.CONNECTION_SECTION));

            services.InitDal(config);
        }
    }
}
