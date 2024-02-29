using System.ComponentModel.DataAnnotations;

namespace InternetBillingSystem.Models
{
    public class Packages
    {
        [Key]
        public int package_id { get; set; }
        public string package_name { get; set; }
        public decimal package_price { get; set; }
        public char package_status { get; set; }
        public int client_id { get; set; }
    }
}
