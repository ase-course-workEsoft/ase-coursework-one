using FuelIn.Models;
using FuelIn.Models.Auth;
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
        public DbSet<StationModel> Stations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerRequest> CustomerRequests { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
    }
}
