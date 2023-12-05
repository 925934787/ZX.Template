using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Configuration;
using System.Reflection;
using ZX.Template.Core.Extensions;

namespace ZX.Template.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            var builder = new ConfigurationBuilder()
                .AddJsonFile("dbconfig.json");

            var configuration = builder.Build();

            configuration.GetSection("ConnectionConfigs").Bind(DbContext.ConnectionConfigs);


            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(LifetimeRegistrar.GetAssemblies().ToArray()));
        }


    }
}
