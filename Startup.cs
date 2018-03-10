using System;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FiiPrezent
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                Uri uri = context.Request.GetUri();
                Console.WriteLine("before " + uri);
                await next();
                Console.WriteLine("after " + uri);
            });
            app.Run(async (context) =>
            {
                Console.WriteLine("Hello World!");
                await context.Response.WriteAsync("Hello World!");
            });

        }
    }
}
