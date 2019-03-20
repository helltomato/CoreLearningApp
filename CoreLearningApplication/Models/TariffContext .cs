using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLearningApplication.Models
{

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
