using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Middleware
{
    public class TokenMiddleware
    {
        RequestDelegate Next;

        public TokenMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Query["token"];

            if (token != "123")
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Token is invalid");
            }
            else
                await Next(context);
        }
    }

    public static class TokenExtension
    {
        public static void UseToken(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<TokenMiddleware>();
        }
    }
}
