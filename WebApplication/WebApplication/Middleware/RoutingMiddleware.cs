using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Middleware
{
    public class RoutingMiddleware
    {
        RequestDelegate Next;

        public RoutingMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value.ToLower();

            if (path == "/" || path == "/index")
                await context.Response.WriteAsync("Home Page");
            else if (path == "/about")
                await context.Response.WriteAsync("About Page");
            else
                context.Response.StatusCode = 404;
        }
    }
}
