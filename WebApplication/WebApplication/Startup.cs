using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Middleware;

namespace WebApplication
{
    public class Startup
    {
        public Startup() // Constructor for initialization of needed parameters
        {

        }

        public void ConfigureServices(IServiceCollection services) // Addition of services
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) // Processing of requests
        {
            int x = 0;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvc();
            app.UseStaticFiles();
            app.UseMiddleware<AuthenticationMiddleware>();
            app.UseMiddleware<RoutingMiddleware>();

            app.Map("/index", (appBuilder) =>
            {
                app.Run(async (context) =>
                {
                    await context.Response.WriteAsync("It's INDEX");
                });
            });

            app.Run(async (context) =>
            {
                x++;
                string host = context.Request.Host.Value;
                string path = context.Request.Path;
                string query = context.Request.QueryString.Value;
                await context.Response.WriteAsync($"<h1>{host}</h1><h2>{path}</h2><h3>{query}</h3>");
            });
        }
    }
}
