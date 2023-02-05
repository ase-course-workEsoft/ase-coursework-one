using FuelIn.Models.Auth;
using FuelIn.Models.CustomerData;
using FuelIn.Models.FuelData;
using FuelIn.Models.StationData;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FuelIn.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> USER { get; set; }
        public DbSet<vehicleTypes> vehicleTypes { get; set; }
        public DbSet<stations> stations { get; set; }
        public DbSet<customers> customers { get; set; }
        public DbSet<fualDistributions> fualDistributions { get; set; }
    }
}
