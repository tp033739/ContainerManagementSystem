using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using ContainerManagementSystem.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ContainerManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                DbInitializer.Initialize(scope.ServiceProvider);
            }
           
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
