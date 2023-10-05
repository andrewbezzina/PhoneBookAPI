using Microsoft.EntityFrameworkCore;
using PhoneBookAPI.DataLayer.Models;

namespace PhoneBookAPI.DataLayer.Contexts
{
    public class PhoneBookDbContext : DbContext
    {

        public PhoneBookDbContext(DbContextOptions<PhoneBookDbContext> options ) : base( options )
        {
        }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Person> People { get; set; }

    }
}
