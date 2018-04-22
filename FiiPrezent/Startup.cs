using FiiPrezent.Db;
using FiiPrezent.Hubs;
using FiiPrezent.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FiiPrezent
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddMvc();
            services.AddScoped<IEventsRepository, DbEventsRepository>();
            services.AddScoped<EventsService>();
            services.AddScoped<IParticipantsUpdatedNotifier, ParticipantsUpdatedNotifier>();

            services.AddDbContext<EventsDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("FiiPrezent")));
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseSignalR(routes =>
            {
                routes.MapHub<UpdateParticipants>("/participants");
            });

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
