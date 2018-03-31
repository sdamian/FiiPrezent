using System;
using FiiPrezent.Hubs;
using FiiPrezent.Services;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FiiPrezent
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddMvc();
            services.AddSingleton<EventsService>();
            services.AddSingleton<IParticipantsUpdatedNotifier, ParticipantsUpdatedNotifier>();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
