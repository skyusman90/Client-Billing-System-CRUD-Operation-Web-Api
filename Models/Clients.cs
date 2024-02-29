using System.ComponentModel.DataAnnotations;

namespace InternetBillingSystem.Models
{
    public class Clients
    {
        [Key]
        public int client_id { get; set; }
        public string client_name { get; set; }
        public string client_username { get; set; }
        public string client_password { get; set; }
        public string client_mobilenum { get; set; }
        public char client_status { get; set; }
    }
}
