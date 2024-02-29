using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using InternetBillingSystem.Models;
using InternetBillingSystem.Controllers;
using System.IdentityModel.Tokens.Jwt;
using InternetBillingSystem.Data;

namespace InternetBillingSystem.Middleware
{
    public class CheckJwtExpirationMiddleware
    {
        private readonly ILogger<CheckJwtExpirationMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        

        public CheckJwtExpirationMiddleware(ILogger<CheckJwtExpirationMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var result = "no";
            var result2 = "yes";
            DateTime today = DateTime.Now;

            string token = context.Request.Headers["Authorization"];

            if (token != null)
            {

                var jwt = token.Replace("bearer ", string.Empty);

                DateTime expirationTime = GetJwtExpirationTime(jwt);

                _logger.LogInformation($"Expiration Time in Middleware: {expirationTime}");
                _logger.LogInformation($"Minutes in Middleware: {today}, {expirationTime}");

                if (today.TimeOfDay > expirationTime.TimeOfDay)
                {
                    context.Response.Headers.Add("isExpired", $"{result2}");
                    throw new SecurityTokenException("Expired Token");
                }
                else
                {
                    context.Response.Headers.Add("isExpired", $"{result}");
                }
                await _next(context);
            }
            else
            {
                await _next(context);
            }
            
        }

        private DateTime GetJwtExpirationTime(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                throw new SecurityTokenException("Invalid JWT token.");
            }

            return jwtToken.ValidTo.ToLocalTime();
        }
    }

    public static class CheckJwtExpirationExtensions
    {
        public static IApplicationBuilder UseCheckJwtExpirationMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CheckJwtExpirationMiddleware>();
        }
    }
}
