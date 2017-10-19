using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Nancy.Owin;

namespace LondonMovingSouth.CatalogService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseOwin(buildFunc =>
            {
                buildFunc.UseNancy(opt => opt.Bootstrapper = new Bootstrapper(env));
            });
        }
    }
}
