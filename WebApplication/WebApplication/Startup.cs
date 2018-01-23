using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Middleware;
using WebApplication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WebApplication
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) // Constructor for initialization of needed parameters
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services) // Addition of services
        {
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();

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

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});

            //var routeHandler = new RouteHandler(Handle);
            //var routeBuilder = new RouteBuilder(app, routeHandler);
            //routeBuilder.MapRoute("extended", "NewRoute/{controller}/{action}/{id?}");
            //app.UseRouter(routeBuilder.Build());
        }

        //public async Task Handle(HttpContext context)
        //{
        //    await context.Response.WriteAsync("ROUTE!");
        //}
    }
}
