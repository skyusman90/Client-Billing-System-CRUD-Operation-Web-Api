using InternetBillingSystem.Data;
using InternetBillingSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace InternetBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly DataContext _context;
        public CustomersController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<List<Customers>>> AddCustomer(Customers request)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@customer_id_external", Value = request.customer_id_external, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@client_id", Value = request.client_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@customer_name", Value = request.customer_name, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@customer_user_name", Value = request.customer_user_name, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@customer_cnic", Value = request.customer_cnic, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@cusotmer_mobilenum", Value = request.cusotmer_mobilenum, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@customer_address", Value = request.customer_address, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@package_id", Value = request.package_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@customer_due_day", Value = request.customer_due_day, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@join_date", Value = request.join_date, SqlDbType = SqlDbType.Date },
                new SqlParameter { ParameterName = "@customer_status", Value = request.customer_status, SqlDbType = SqlDbType.Char },

            };

            var dbcustomers = await _context.client_customers.FromSqlRaw("InsertClientCustomer @customer_id_external, @client_id, @customer_name," +
                                                            "@customer_user_name, @customer_cnic, @cusotmer_mobilenum, @customer_address, @package_id, @customer_due_day," +
                                                            "@join_date, @customer_status", parameters.ToArray()).ToListAsync();

            await _context.SaveChangesAsync();

            return Ok(dbcustomers);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Customers>>> GetCustomersDisplay(int client_id)
        {
            var dbcustomers = await _context.client_customers.FromSqlRaw($"SelectAllCustomers {client_id}").ToListAsync();

            return Ok(dbcustomers);
        }

        [HttpGet]
        [Route("getcustomerbyid")]
        [Authorize]
        public async Task<ActionResult<List<Customers>>> GetCustomers(int customer_id, int client_id)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter { ParameterName = "@customer_id", Value = customer_id, SqlDbType = SqlDbType.Int},
                new SqlParameter { ParameterName = "@client_id", Value = client_id, SqlDbType = SqlDbType.Int}
            };

            var dbcustomers = await _context.client_customers.FromSqlRaw("SelectClientCustomer @customer_id, @client_id", parameters.ToArray()).ToListAsync();

            return Ok(dbcustomers);
        }

        [HttpGet]
        [Route("getallcustomers")]
        [Authorize]
        public async Task<ActionResult<List<Customers>>> GetAllCustomers(int client_id, string CustomerNameSearch, char activeInactive, string order_by, string asc_desc)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                new SqlParameter { ParameterName = "@client_id", Value = client_id, SqlDbType = SqlDbType.Int},
                new SqlParameter { ParameterName = "@CustomerNameSearch", Value = CustomerNameSearch, SqlDbType = SqlDbType.VarChar},
                new SqlParameter { ParameterName = "@activeInactive", Value = activeInactive, SqlDbType = SqlDbType.Char},
                new SqlParameter { ParameterName = "@orderBy", Value = order_by, SqlDbType = SqlDbType.VarChar},
                new SqlParameter { ParameterName = "@ascOrDesc", Value = asc_desc, SqlDbType = SqlDbType.VarChar}
            };

            var dbcustomers = await _context.client_customers.FromSqlRaw("SelectAllClientCustomer @client_id, @CustomerNameSearch, @activeInactive, @orderBy, @ascOrDesc", parameters.ToArray()).ToListAsync();

            return Ok(dbcustomers);
        }

        [HttpPut]
        [Route("customer")]
        [Authorize]
        public async Task<ActionResult<List<Customers>>> UpdateCustomer(Customers request)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@customer_id", Value = request.customer_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@customer_id_external", Value = request.customer_id_external, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@client_id", Value = request.client_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@customer_name", Value = request.customer_name, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@customer_user_name", Value = request.customer_user_name, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@customer_cnic", Value = request.customer_cnic, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@cusotmer_mobilenum", Value = request.cusotmer_mobilenum, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@customer_address", Value = request.customer_address, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@package_id", Value = request.package_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@customer_due_day", Value = request.customer_due_day, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@join_date", Value = request.join_date, SqlDbType = SqlDbType.Date },
                new SqlParameter { ParameterName = "@customer_status", Value = request.customer_status, SqlDbType = SqlDbType.Char },

            };

            var dbcustomers = await _context.client_customers.FromSqlRaw("UpdateClientCustomer @customer_id, @customer_id_external, @client_id, @customer_name," +
                                                "@customer_user_name, @customer_cnic, @cusotmer_mobilenum, @customer_address, @package_id, @customer_due_day," +
                                                "@join_date, @customer_status", parameters.ToArray()).ToListAsync();

            if(dbcustomers == null)
            {
                return NotFound("Customer Not Found");
            }

            return Ok(dbcustomers);
        }

        [HttpPut]
        [Route("customerstatus")]
        [Authorize]
        public async Task<ActionResult<List<Customers>>> UpdateCustomerStatus(string listOfCustomerIds, char customer_status, int client_id)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter { ParameterName = "@ListOfcustomer_ids", Value = listOfCustomerIds, SqlDbType = SqlDbType.VarChar},
                    new SqlParameter { ParameterName = "@customer_status", Value = customer_status, SqlDbType = SqlDbType.Char},
                    new SqlParameter { ParameterName = "@client_id", Value = client_id, SqlDbType = SqlDbType.Int}
            };

            var dbcustomers = await _context.client_customers.FromSqlRaw("UpdateCustomersStatus @ListOfcustomer_ids, @customer_status, @client_id", parameters.ToArray()).ToListAsync();

            if( dbcustomers == null)
            {
                return NotFound("Customer Not Found");
            }

            return Ok(dbcustomers);
        }
    }
}
