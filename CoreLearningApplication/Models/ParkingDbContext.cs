using Microsoft.EntityFrameworkCore;

namespace CoreLearningApplication.Models
{
    public class ParkingDbContext : DbContext
    {
        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public ParkingDbContext(DbContextOptions<ParkingDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}