using InternetBillingSystem.Data;
using InternetBillingSystem.Models;
using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text;

namespace InternetBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        private readonly ILogger<AuthController> _logger;

        public AuthController(DataContext context, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("loginclient")]
        public async Task<ActionResult<string>> LoginClient(Login loginClient)
        {
            string token = "";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter{ParameterName = "@Username", Value = loginClient.Username, SqlDbType = SqlDbType.VarChar },
                new SqlParameter{ParameterName = "@Password", Value = loginClient.Password, SqlDbType = SqlDbType.VarChar }
            };
            var dbclient = await _context.clients.FromSqlRaw($"LoginClient @Username, @Password", parameters.ToArray()).ToListAsync();
            if (dbclient.Count == 0)
            {
                loginClient.Result = false;
                loginClient.Message = "Incorrect Username or Password";
                return loginClient.Message;
            }
            else
            {
                loginClient.Result = true;
                loginClient.Message = "Client is logged in.";
                token = GenerateToken(dbclient[0].client_id);

            }
            return Ok(token);
        }

        private string GenerateToken(int id)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim("client_id", id.ToString()),
                new Claim("generated_date", DateTime.Now.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentials
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }
    }
}
