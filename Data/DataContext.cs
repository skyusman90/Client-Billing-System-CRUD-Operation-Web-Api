using InternetBillingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace InternetBillingSystem.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }
        public DbSet<Clients> clients { get; set; }
        public DbSet<Customers> client_customers { get; set; }
        public DbSet<Packages> client_packages { get; set; }
        public DbSet<Payments> customer_payments { get; set; }
        public DbSet<Expenditures> client_expenditures { get; set; }
    }
}
