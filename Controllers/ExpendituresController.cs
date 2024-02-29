using InternetBillingSystem.Data;
using InternetBillingSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace InternetBillingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpendituresController : ControllerBase
    {
        private readonly DataContext _context;
        public ExpendituresController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<List<Expenditures>>> AddExpenditure(Expenditures request)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@client_id", Value = request.client_id, SqlDbType = SqlDbType.Int},
                new SqlParameter { ParameterName = "@expenditure_title", Value = request.expenditure_title, SqlDbType = SqlDbType.VarChar},
                new SqlParameter { ParameterName = "@expenditure_details", Value = request.expenditure_details, SqlDbType = SqlDbType.VarChar},
                new SqlParameter { ParameterName = "@expenditure_type", Value = request.expenditure_type, SqlDbType = SqlDbType.VarChar},
                new SqlParameter { ParameterName = "@expenditure_amount", Value = request.expenditure_amount, SqlDbType = SqlDbType.Decimal},
                new SqlParameter { ParameterName = "@expenditure_date", Value = request.expenditure_date, SqlDbType = SqlDbType.Date},
                new SqlParameter { ParameterName = "@expenditure_status", Value = request.expenditure_status, SqlDbType = SqlDbType.Char}
            };

            var dbexpenditures = await _context.client_expenditures.FromSqlRaw("InsertClientExpenditure @client_id, @expenditure_title" +
                                                                    ", @expenditure_details, @expenditure_type, @expenditure_amount, @expenditure_date," +
                                                                    "@expenditure_status", parameters.ToArray()).ToListAsync();

            return Ok(dbexpenditures);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<Expenditures>>> GetAllExpendituresDisplay(int client_id)
        {

            var dbexpenditures = await _context.client_expenditures.FromSqlRaw($"SelectAllExpendituresDisplay {client_id}").ToListAsync();

            return Ok(dbexpenditures);
        }

        [HttpGet]
        [Route("getallexpenitures")]
        [Authorize]
        public async Task<ActionResult<List<Expenditures>>> GetAllExpenditures(int client_id, string exp_title, char paidUnPaid, string orderby, string ascOrDesc, string StartDate, string EndDate)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@client_id", Value = client_id, SqlDbType = SqlDbType.Int},
                new SqlParameter { ParameterName = "@exp_title", Value = exp_title, SqlDbType = SqlDbType.VarChar},
                new SqlParameter { ParameterName = "@PaidUnPaid", Value = paidUnPaid, SqlDbType = SqlDbType.Char},
                new SqlParameter { ParameterName = "@orderBy", Value = orderby, SqlDbType = SqlDbType.VarChar},
                new SqlParameter { ParameterName = "@ascOrDesc", Value = ascOrDesc, SqlDbType = SqlDbType.VarChar},
                new SqlParameter { ParameterName = "@StartDate", Value = StartDate, SqlDbType = SqlDbType.Date},
                new SqlParameter { ParameterName = "@EndDate", Value = EndDate, SqlDbType = SqlDbType.Date}
            };

            var dbexpenditures = await _context.client_expenditures.FromSqlRaw("SelectAllClientExpenditures @client_id, @exp_title" +
                                                        ", @PaidUnPaid, @orderBy, @ascOrDesc, @StartDate," +
                                                        "@EndDate", parameters.ToArray()).ToListAsync();

            return Ok(dbexpenditures);
        }

        [HttpGet]
        [Route("getsingleexpeniture")]
        [Authorize]
        public async Task<ActionResult<List<Expenditures>>> GetSingleExpenditure(int expenditure_id, int client_id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@expenditure_id", Value = expenditure_id, SqlDbType = SqlDbType.Int},
                new SqlParameter { ParameterName = "@client_id", Value = client_id, SqlDbType = SqlDbType.Int}
            };

            var dbexpenditures = await _context.client_expenditures.FromSqlRaw("SelectClientExpenditureById @expenditure_id, @client_id", parameters.ToArray()).ToListAsync();

            return Ok(dbexpenditures);
        }

        [HttpPut]
        [Route("updateexpenditure")]
        [Authorize]
        public async Task<ActionResult<List<Expenditures>>> UpdateExpenditure(Expenditures request)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@expenditure_id", Value = request.expenditure_id, SqlDbType = SqlDbType.Int},
                new SqlParameter { ParameterName = "@client_id", Value = request.client_id, SqlDbType = SqlDbType.Int},
                new SqlParameter { ParameterName = "@expenditure_title", Value = request.expenditure_title, SqlDbType = SqlDbType.VarChar},
                new SqlParameter { ParameterName = "@expenditure_details", Value = request.expenditure_details, SqlDbType = SqlDbType.VarChar},
                new SqlParameter { ParameterName = "@expenditure_type", Value = request.expenditure_type, SqlDbType = SqlDbType.VarChar},
                new SqlParameter { ParameterName = "@expenditure_amount", Value = request.expenditure_amount, SqlDbType = SqlDbType.Decimal},
                new SqlParameter { ParameterName = "@expenditure_date", Value = request.expenditure_date, SqlDbType = SqlDbType.Date},
                new SqlParameter { ParameterName = "@expenditure_status", Value = request.expenditure_status, SqlDbType = SqlDbType.Char}
            };

            var dbexpenditures = await _context.client_expenditures.FromSqlRaw("UpdateClientExpenditure @expenditure_id, @client_id, @expenditure_title" +
                                                        ", @expenditure_details, @expenditure_type, @expenditure_amount, @expenditure_date," +
                                                        "@expenditure_status", parameters.ToArray()).ToListAsync();

            if (dbexpenditures == null)
            {
                return NotFound("Expenditure Not Found");
            }

            return Ok(dbexpenditures);
        }

        [HttpPut]
        [Route("updateexpenditurebyid")]
        [Authorize]
        public async Task<ActionResult<List<Expenditures>>> UpdateExpenditureById(string ListofIds, char expenditure_status, int client_id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@ListOfexpenditure_ids", Value = ListofIds, SqlDbType = SqlDbType.VarChar},
                new SqlParameter { ParameterName = "@expenditure_status", Value = expenditure_status, SqlDbType = SqlDbType.Char},
                new SqlParameter { ParameterName = "@client_id", Value = client_id, SqlDbType = SqlDbType.Int}
            };

            var dbexpenditures = await _context.client_expenditures.FromSqlRaw("UpdateExpenditureStatusById @ListOfexpenditure_ids, @expenditure_status, @client_id", parameters.ToArray()).ToListAsync();

            if (dbexpenditures == null)
            {
                return NotFound("Expenditure Not Found");
            }

            return Ok(dbexpenditures);
        }
    }
}
