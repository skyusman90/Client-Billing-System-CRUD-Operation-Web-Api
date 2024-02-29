using System.ComponentModel.DataAnnotations;

namespace InternetBillingSystem.Models
{
    public class Payments
    {
        [Key]
        public int payment_id { get; set; }
        public int customer_id { get; set; }
        public int client_id { get; set; }
        public int package_id { get; set; }
        public string payment_title { get; set; }
        public string payment_due_date { get; set; }
        public decimal payment_amount { get; set; }
        public char payment_status { get; set; }
        public int received_by { get; set; }
        public string generation_date { get; set; }
        public char sms_sent { get; set; }
    }
}
