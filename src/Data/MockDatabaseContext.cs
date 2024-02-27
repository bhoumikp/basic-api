using Microsoft.EntityFrameworkCore;
using basic_api.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace basic_api.Data
{
    public class MockDatabaseContext : DbContext
    {
        public MockDatabaseContext(DbContextOptions<MockDatabaseContext> options) : base(options)
        {
        }

        public DbSet<Entity> Entities { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Date> Dates { get; set; }
        public DbSet<Name> Names { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
