using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nancy;
using Nancy.TinyIoc;

namespace LondonMovingSouth.CatalogService
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        public IConfigurationRoot Configuration;
        public static TinyIoCContainer Container;

        public Bootstrapper(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Bootstrapper>();
            }

            Configuration = builder.Build();
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            var connectionStrings = Configuration.GetSection("ConnectionStrings");
            var connectionString = connectionStrings.GetValue<string>("DefaultConnection");

            base.ConfigureApplicationContainer(container);

            var loggerFactory = new LoggerFactory();

            container.Register<ICatalogRepository, CatalogRepository>(new CatalogRepository(connectionString, loggerFactory));
            
            container.Register<IConfiguration>(Configuration);

            Container = container;
        }
    }
}