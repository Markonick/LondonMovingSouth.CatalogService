using System.IO;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace LondonMovingSouth.CatalogService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Information("Starting web host");

            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseUrls("http://*:6666/")
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();

            host.Run();
        }
    }
}
