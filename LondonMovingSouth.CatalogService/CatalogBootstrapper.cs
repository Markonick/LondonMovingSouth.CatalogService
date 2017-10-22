using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace LondonMovingSouth.CatalogService
{
    public class CatalogBootstrapper : DefaultNancyBootstrapper
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public static TinyIoCContainer Container;

        public CatalogBootstrapper(IConfiguration configuration, ILogger logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            container.Register(_logger);
        }
        
        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            var connectionStrings = _configuration.GetSection("ConnectionStrings");
            var connectionString = connectionStrings.GetValue<string>("DefaultConnection");

            container.Register<ICatalogRepository, CatalogRepository>(new CatalogRepository(connectionString, _logger));
            container.Register<NancyModule, CatalogModule>(new CatalogModule(new CatalogRepository(connectionString, _logger), _logger));
            container.Register<DbContext, CatalogDbContext>(new CatalogDbContext(connectionString));

            Container = container;
        }
    }
}