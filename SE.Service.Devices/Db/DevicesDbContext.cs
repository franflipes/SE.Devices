using Microsoft.EntityFrameworkCore;

namespace SE.Service.Devices.Db
{
    public class DevicesDbContext : DbContext
    {
        public DevicesDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Not working as In-Memory is not relational
            //Add constraints for UNIQUENESS SNumber,model,brand for Gateways
            builder.Entity<Gateway>()
                .HasIndex(g => new { g.SerialNumber,g.Model,g.Brand })
                .IsUnique();

            

            //Add constraints for UNIQUENESS SNumber,Model,Brand and Type for Counters
            builder.Entity<Counter>()
                .HasIndex(c => new { c.SerialNumber, c.Model, c.Brand,c.Type })
                .IsUnique();
        }

        public DbSet<Gateway> Gateways { get; set; }
        public DbSet<Counter> Counters { get; set; }

    }
}
