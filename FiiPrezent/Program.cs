using System.Net;
using FiiPrezent.Db;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FiiPrezent
{
    class Program
    {
        public static void Main(string[] args)
        {
            IWebHost host = BuildWebHost(args);
            Migrate(host);
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
//                .UseKestrel(options =>
//                {
//                    options.Listen(IPAddress.Loopback, 5001, listenOptions =>
//                    {
//                        listenOptions.UseHttps("Properties/localhost.pfx", "1234");
//                    });
//                })
                .UseApplicationInsights()
                .Build();

        public static void Migrate(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var db = serviceProvider.GetService<EventsDbContext>();
                db.Database.Migrate();
            }
        }
    }
 }
