using InternetBillingSystem.Data;
using InternetBillingSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace InternetBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController : ControllerBase
    {
        private readonly DataContext _context;
        public PackagesController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<List<Packages>>> AddPackage(Packages request)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@package_name", Value = request.package_name, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@package_price", Value = request.package_price, SqlDbType = SqlDbType.Decimal },
                new SqlParameter { ParameterName = "@package_status", Value = request.package_status, SqlDbType = SqlDbType.Char },
                new SqlParameter { ParameterName = "@client_id", Value = request.client_id, SqlDbType = SqlDbType.Int },
            };

            var dbpackage = await _context.client_packages.FromSqlRaw("InsertClientPackages @package_name, @package_price, @package_status," +
                                                                        "@client_id", parameters.ToArray()).ToListAsync();

            await _context.SaveChangesAsync();

            return Ok(dbpackage);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Packages>>> GetAllPackagesDisplay(int client_id)
        {

            var dbpackages = await _context.client_packages.FromSqlRaw($"SelectAllPackagesDisplay {client_id}").ToListAsync();

            return Ok(dbpackages);
        }

        [HttpGet]
        [Route("allpackages")]
        [Authorize]
        public async Task<ActionResult<List<Packages>>> GetAllPackages(int client_id, string customerNameSearch, string packageNameSearch, char activeInactive, string orderby, string ascOrDesc)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@client_id", Value = client_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@CustomerNameSearch", Value = customerNameSearch,SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@PackageNameSearch", Value = packageNameSearch, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@activeInactive", Value = activeInactive, SqlDbType = SqlDbType.Char },
                new SqlParameter { ParameterName = "@orderBy", Value = orderby, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@ascOrDesc", Value = ascOrDesc, SqlDbType = SqlDbType.VarChar }
            };

            var dbpackages = await _context.client_packages.FromSqlRaw("SelectAllClientPackages @client_id, @CustomerNameSearch, @PackageNameSearch," +
                                                                        "@activeInactive, @orderBy, @ascOrDesc", parameters.ToArray()).ToListAsync();

            return Ok(dbpackages);
        }

        [HttpGet]
        [Route("singlepackage")]
        [Authorize]
        public async Task<ActionResult<List<Packages>>> GetSinglePackage(int package_id, int client_id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@package_id", Value = package_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@client_id", Value = client_id, SqlDbType = SqlDbType.Int }
            };

            var dbpackages = await _context.client_packages.FromSqlRaw("SelectClientPackage @package_id, @client_id", parameters.ToArray()).ToListAsync();

            return Ok(dbpackages);
        }

        [HttpPut]
        [Route("updatepackage")]
        [Authorize]
        public async Task<ActionResult<List<Packages>>> UpdatePackage(Packages request)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@package_id", Value = request.package_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@package_name", Value = request.package_name, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@package_price", Value = request.package_price, SqlDbType = SqlDbType.Decimal },
                new SqlParameter { ParameterName = "@package_status", Value = request.package_status, SqlDbType = SqlDbType.Char },
                new SqlParameter { ParameterName = "@client_id", Value = request.client_id, SqlDbType = SqlDbType.Int },
            };

            var dbpackage = await _context.client_packages.FromSqlRaw("UpdateClientPackage @package_id, @package_name, @package_price," +
                                                                        "@package_status, @client_id", parameters.ToArray()).ToListAsync();
            await _context.SaveChangesAsync();

            return Ok(dbpackage);
        }

        [HttpPut]
        [Route("packagestatus")]
        [Authorize]
        public async Task<ActionResult<List<Clients>>> UpdatePackageStatus(string packageIds, char package_status, int client_id)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter { ParameterName = "@ListOfPackage_ids", Value = packageIds, SqlDbType = SqlDbType.VarChar},
                    new SqlParameter { ParameterName = "@package_status", Value = package_status, SqlDbType = SqlDbType.Char},
                    new SqlParameter { ParameterName = "@client_id", Value = client_id, SqlDbType = SqlDbType.Int}
            };

            var dbclients = await _context.client_packages.FromSqlRaw("UpdatePackageStatus @ListOfPackage_ids, @package_status, @client_id",
                                                                parameters.ToArray()).ToListAsync();

            if (dbclients == null)
            {
                return NotFound("Package Not Found");
            }

            await _context.SaveChangesAsync();

            return Ok(dbclients);
        }
    }
}
