using System.ComponentModel.DataAnnotations;

namespace InternetBillingSystem.Models
{
    public class Expenditures
    {
        [Key]
        public int expenditure_id { get; set; }
        public int client_id { get; set; }
        public string expenditure_title { get; set; }
        public string expenditure_details { get; set; }
        public string expenditure_type { get; set; }
        public decimal expenditure_amount { get; set; }
        public string expenditure_date { get; set; }
        public char expenditure_status { get; set; }
    }
}
