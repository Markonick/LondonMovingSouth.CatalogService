using Microsoft.AspNetCore.Builder;
using Nancy.Owin;

namespace LondonMovingSouth.CatalogService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(buildFunc => buildFunc.UseNancy());
        }
    }
}
