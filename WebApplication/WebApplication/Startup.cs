using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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
            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) // Processing of requests
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvcWithDefaultRoute();

            var routeHandler = new RouteHandler(Handle);
            var routeBuilder = new RouteBuilder(app, routeHandler);
            routeBuilder.MapRoute("extended", "NewRoute/{controller}/{action}/{id?}");
            app.UseRouter(routeBuilder.Build());

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
        }

        public async Task Handle(HttpContext context)
        {
            await context.Response.WriteAsync("ROUTE!");
        }
    }
}
