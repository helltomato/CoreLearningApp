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
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (optionsBuilder == null) { optionsBuilder = new DbContextOptionsBuilder();}
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=tariffesestoredb;Trusted_Connection=True;MultipleActiveResultSets=true");
        //    }
        //}

        //public TariffContext()
        //{
        //}

        public TariffContext(DbContextOptions<TariffContext> options)
            : base(options)
        {
            Database.EnsureCreated();

    }
    }
}
