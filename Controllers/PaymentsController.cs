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
    public class PaymentsController : ControllerBase
    {
        private readonly DataContext _context;
        public PaymentsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<List<Payments>>> AddPayment(Payments request)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@customer_id", Value = request.customer_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@client_id", Value = request.client_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@package_id", Value = request.package_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@payment_title", Value = request.payment_title, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@payment_due_date", Value = request.payment_due_date, SqlDbType = SqlDbType.Date },
                new SqlParameter { ParameterName = "@payment_amount", Value = request.payment_amount, SqlDbType = SqlDbType.Decimal },
                new SqlParameter { ParameterName = "@payment_status", Value = request.payment_status, SqlDbType = SqlDbType.Char },
                new SqlParameter { ParameterName = "@received_by", Value = request.received_by, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@generation_date", Value = request.generation_date, SqlDbType = SqlDbType.Date },
                new SqlParameter { ParameterName = "@sms_sent", Value = request.sms_sent, SqlDbType = SqlDbType.Char }
            };

            var dbpayments = await _context.customer_payments.FromSqlRaw("InsertCustomerPayment @customer_id, @client_id, @package_id," +
                                                                        "@payment_title, @payment_due_date, @payment_amount, @payment_status," +
                                                                        "@received_by, @generation_date, @sms_sent", parameters.ToArray()).ToListAsync();

            await _context.SaveChangesAsync();

            return Ok(dbpayments);
        }

        [HttpGet]
        [Route("getAllPaymentsDisplay")]
        [Authorize]
        public async Task<ActionResult<List<Payments>>> GetAllPaymentsDisplay(int client_id)
        {
            var dbpayments = await _context.customer_payments.FromSqlRaw($"SelectAllPaymentsDisplay {client_id}").ToListAsync();
            return Ok(dbpayments);
        }

        [HttpGet]
        [Route("getallpayments")]
        [Authorize]
        public async Task<ActionResult<List<Payments>>> GetAllPayments(int client_id, string Payment_title, char Paid_UnPaid, string orderby, string ascOrDesc, string StartDate, string EndDate, int customer_id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@client_id", Value = client_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@PaymentTitle", Value = Payment_title, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@PaidUnPaid", Value = Paid_UnPaid, SqlDbType = SqlDbType.Char },
                new SqlParameter { ParameterName = "@orderby", Value = orderby, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@ascOrDesc", Value = ascOrDesc, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@StartDate", Value = StartDate, SqlDbType = SqlDbType.Date },
                new SqlParameter { ParameterName = "@EndDate", Value = EndDate, SqlDbType = SqlDbType.Date },
                new SqlParameter { ParameterName = "@customer_id", Value = customer_id, SqlDbType = SqlDbType.Int }
            };

            var dbpayments = await _context.customer_payments.FromSqlRaw("SelectAllPayments @client_id, @PaymentTitle, @PaidUnPaid, @orderby, @ascOrDesc," +
                                                                          "@StartDate, @EndDate, @customer_id", parameters.ToArray()).ToListAsync();

            return Ok(dbpayments);
        }

        [HttpGet]
        [Route("getsinglepayment")]
        [Authorize]
        public async Task<ActionResult<List<Payments>>> GetSinglePayment(int customer_id, int client_id, int ordby)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@customer_id", Value = customer_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@client_id", Value = client_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@ordby", Value = ordby, SqlDbType = SqlDbType.Int }
            };

            var dbpayments = await _context.customer_payments.FromSqlRaw("SelectCustomerPayments @customer_id, @client_id, @ordby", parameters.ToArray()).ToListAsync();

            return Ok(dbpayments);
        }

        [HttpGet]
        [Route("paymentbyid")]
        [Authorize]
        public async Task<ActionResult<List<Payments>>> GetSinglePaymentbyID(int payment_id, int client_id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@payment_id", Value = payment_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@client_id", Value = client_id, SqlDbType = SqlDbType.Int },
            };

            var dbpayments = await _context.customer_payments.FromSqlRaw("SelectPaymentById @payment_id, @client_id", parameters.ToArray()).ToListAsync();

            return Ok(dbpayments);
        }

        [HttpPut]
        [Route("updatepayment")]
        [Authorize]
        public async Task<ActionResult<List<Payments>>> UpdatePayments(Payments request)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@payment_id", Value = request.payment_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@customer_id", Value = request.customer_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@client_id", Value = request.client_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@package_id", Value = request.package_id, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@payment_title", Value = request.payment_title, SqlDbType = SqlDbType.VarChar },
                new SqlParameter { ParameterName = "@payment_due_date", Value = request.payment_due_date, SqlDbType = SqlDbType.Date },
                new SqlParameter { ParameterName = "@payment_amount", Value = request.payment_amount, SqlDbType = SqlDbType.Decimal },
                new SqlParameter { ParameterName = "@payment_status", Value = request.payment_status, SqlDbType = SqlDbType.Char },
                new SqlParameter { ParameterName = "@received_by", Value = request.received_by, SqlDbType = SqlDbType.Int },
                new SqlParameter { ParameterName = "@generation_date", Value = request.generation_date, SqlDbType = SqlDbType.Date },
                new SqlParameter { ParameterName = "@sms_sent", Value = request.sms_sent, SqlDbType = SqlDbType.Char }
            };

            var dbpayments = await _context.customer_payments.FromSqlRaw("UpdateCustomerPayment @payment_id, @customer_id, @client_id, @package_id," +
                                                                        "@payment_title, @payment_due_date, @payment_amount, @payment_status," +
                                                                        "@received_by, @generation_date, @sms_sent", parameters.ToArray()).ToListAsync();

            if (dbpayments == null)
            {
                return NotFound("Payment Not Found");
            }

            await _context.SaveChangesAsync();

            return Ok(dbpayments);
        }

        [HttpPut]
        [Route("updatepaymentstatus")]
        [Authorize]
        public async Task<ActionResult<List<Payments>>> UpdatePaymentsStatus(string ListOfPayment_ids, char payment_status, int client_id)
        {
            List<SqlParameter> parameters = new List<SqlParameter> {
                    new SqlParameter { ParameterName = "@ListOfPayment_ids", Value = ListOfPayment_ids, SqlDbType = SqlDbType.VarChar},
                    new SqlParameter { ParameterName = "@payment_status", Value = payment_status, SqlDbType = SqlDbType.Char},
                    new SqlParameter { ParameterName = "@client_id", Value = client_id, SqlDbType = SqlDbType.Int}
            };

            var dbpayments = await _context.customer_payments.FromSqlRaw("UpdatePaymentStatusById @ListOfPayment_ids, @payment_status, @client_id",
                                                                parameters.ToArray()).ToListAsync();

            if (dbpayments == null)
            {
                return NotFound("Payment Not Found");
            }

            await _context.SaveChangesAsync();

            return Ok(dbpayments);
        }
    }

}
