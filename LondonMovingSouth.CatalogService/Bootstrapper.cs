using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Nancy;
using Nancy.TinyIoc;
using Serilog.Events;

namespace LondonMovingSouth.CatalogService
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private readonly ILogger _logger;
        public IConfigurationRoot Configuration;
        public static TinyIoCContainer Container;

        public Bootstrapper(IHostingEnvironment env)
        {
            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                //builder.AddUserSecrets<Bootstrapper>();
            }

            Configuration = builder.Build();
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            var connectionStrings = Configuration.GetSection("ConnectionStrings");
            var connectionString = connectionStrings.GetValue<string>("DefaultConnection");
            base.ConfigureRequestContainer(container, context);

            container.Register(_logger);

            container.Register<ICatalogRepository, CatalogRepository>(new CatalogRepository(connectionString, _logger));
            container.Register<NancyModule, CatalogModule>(new CatalogModule(new CatalogRepository(connectionString, _logger), _logger));
            
            container.Register<ICatalogDbContext, CatalogDbContext>(new CatalogDbContext(connectionString));

            Container = container;
        }
    }
}