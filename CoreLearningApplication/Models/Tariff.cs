using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoreLearningApplication.Models
{
    public class Tariff
    {
        public int Id { get; set; }
        public string Name { get; set; } //название
        public int Price { get; set; }//цена
    }
    public class TariffContext : DbContext
    {
        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<Order> Orders { get; set; }

        public TariffContext(DbContextOptions<TariffContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
