using Microsoft.EntityFrameworkCore;
using PhoneBookAPI.DataLayer.Models;

namespace PhoneBookAPI.DataLayer.Contexts
{
    public class PhoneBookDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public PhoneBookDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // in memory database used for simplicity, change to a real db for production applications
            //options.UseInMemoryDatabase("PhoneBookDb");
            // to connect to sql server with connection string from app settings use following instead.
            options.UseSqlServer(Configuration.GetConnectionString("PhonebookSqlDatabase"));
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Person> People { get; set; }

    }
}
