using InternetBillingSystem.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBillingSystem.Middleware
{
    public class AuthorizationMiddleware
    {
        private readonly ILogger<AuthorizationMiddleware> _logger;
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(ILogger<AuthorizationMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var result = "invalid token";
            var result2 = "null";
            string headers = context.Request.Headers["Authorization"];
            if (headers?.Length == 0 || headers == null)
            {
                context.Response.Headers.Add("AuthorizationToken", $"{result2}");
            }
            else
            {
                if(headers.Length < 200){
                    context.Response.Headers.Add("AuthorizationToken", $"{result}");
                }
                else
                {
                    context.Response.Headers.Add("AuthorizationToken", $"{headers}");
                }
            }

            await _next(context);
        }
    }
    public static class AuthorizationExtensions
    {
        public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<AuthorizationMiddleware>();
        }
    }
}
