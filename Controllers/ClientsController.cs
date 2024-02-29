using InternetBillingSystem.Data;
using InternetBillingSystem.Middleware;
using InternetBillingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Data;

namespace InternetBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly DataContext _context;
        public ClientsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<List<Clients>>> AddClient(Clients client)
        {
            var dbclient = await _context.clients.FromSqlRaw($"InsertClient {client.client_name},{client.client_username},{client.client_password},{client.client_mobilenum},{client.client_status}").ToListAsync();
            await _context.SaveChangesAsync();
            return Ok(dbclient);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Clients>>> GetAllClients()
        {
            var dbclients = await _context.clients.ToListAsync();
            return Ok(dbclients);
        }

        [HttpGet]
        [Route("getclientbyid")]
        [Authorize]
        public async Task<ActionResult<List<Clients>>> GetClientByID(int id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter{ParameterName = "@client_id", Value = id, SqlDbType = SqlDbType.Int }
            };
            var dbclient = await _context.clients.FromSqlRaw($"SelectClient @client_id", parameters.ToArray()).ToListAsync();
            return Ok(dbclient);
        }

        [HttpGet]
        [Route("getclientbyusername")]
        public async Task<ActionResult<List<Clients>>> GetClientByUsername(string username)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter{ParameterName = "@Username", Value = username, SqlDbType = SqlDbType.NVarChar }
            };
            var dbclient = await _context.clients.FromSqlRaw($"GetClientByUsername @Username", parameters.ToArray()).ToListAsync();
            return Ok(dbclient);
        }

        [HttpPut]
        [Route("client")]
        [Authorize]
        public async Task<ActionResult<List<Clients>>> UpdateClient(Clients request)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {   
                new SqlParameter { ParameterName = "@client_id", Value = request.client_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@client_name", Value = request.client_name, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@client_username", Value = request.client_username, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@client_password", Value = request.client_password, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@client_mobilenum", Value = request.client_mobilenum, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@client_status", Value = request.client_status, SqlDbType = SqlDbType.Char }
            };

            var dbclients = await _context.clients.FromSqlRaw("UpdateClient @client_id, @client_name, @client_username, " +
                                                                "@client_password, @client_mobilenum, @client_status", parameters.ToArray()).ToListAsync();

            if(dbclients == null)
            {
                return NotFound("Client Not Found");
            }

            await _context.SaveChangesAsync();

            return Ok(dbclients);
        }

        [HttpPut]
        [Route("clientpassword")]
        [Authorize]
        public async Task<ActionResult<List<Clients>>> UpdateClientPassword(int client_id, string client_username, string client_new_password, string client_old_password)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter { ParameterName = "@client_id", Value = client_id, SqlDbType = SqlDbType.Int},
                    new SqlParameter { ParameterName = "@client_username", Value = client_username, SqlDbType = SqlDbType.VarChar},
                    new SqlParameter { ParameterName = "@client_new_password", Value = client_new_password, SqlDbType = SqlDbType.VarChar},
                    new SqlParameter { ParameterName = "@client_old_password", Value = client_old_password, SqlDbType = SqlDbType.VarChar}
            };

            var dbclients = await _context.clients.FromSqlRaw("UpdateClientPassword @client_id, @client_username, @client_new_password, @client_old_password", 
                                                                parameters.ToArray()).ToListAsync();

            if(dbclients == null)
            {
                return NotFound("Client Not Found");
            }

            await _context.SaveChangesAsync();

            return Ok(dbclients);
        }

        [HttpPut]
        [Route("clientstatus")]
        [Authorize]
        public async Task<ActionResult<List<Clients>>> UpdateClientStatus(string clientids, char client_status)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter { ParameterName = "@ListOfclient_ids", Value = clientids, SqlDbType = SqlDbType.VarChar },
                    new SqlParameter { ParameterName = "@client_status", Value = client_status, SqlDbType = SqlDbType.Char}
            };

            var dbclients = await _context.clients.FromSqlRaw("UpdateClientStatus @ListOfclient_ids, @client_status",
                                                                parameters.ToArray()).ToListAsync();

            if (dbclients == null)
            {
                return NotFound("Client Not Found");
            }

            await _context.SaveChangesAsync();

            return Ok(dbclients);
        }
    }
}
