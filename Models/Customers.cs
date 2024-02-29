using System.ComponentModel.DataAnnotations;

namespace InternetBillingSystem.Models
{
    public class Customers
    {
        [Key]
        public int customer_id { get; set; }
        public int customer_id_external { get; set; }
        public int client_id { get; set; }
        public string customer_name { get; set; }
        public string customer_user_name { get; set; }
        public string customer_cnic { get; set; }
        public string cusotmer_mobilenum { get; set; }
        public string customer_address { get; set; }
        public int package_id { get; set; }
        public int customer_due_day { get; set; }
        public string join_date { get; set; }
        public char customer_status { get; set; }
    }
}
