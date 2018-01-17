using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Middleware
{
    public class AuthenticationMiddleware
    {
        RequestDelegate Next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            Next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Query["token"];

            if (String.IsNullOrEmpty(token))
                context.Response.StatusCode = 403;
            else
                await Next(context);
        }
    }
}
