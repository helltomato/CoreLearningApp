using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLearningApplication.Models;

namespace CoreLearningApplication.Helpers
{ 

    public static class SampleData
    {
        public static void Initialize(TariffContext context)
        {
            if (!context.Tariffs.Any())
            {
                context.Tariffs.AddRange(
                    new Tariff
                    {
                        Name = "Гостевой",
                        Price = 20
                    },
                    new Tariff
                    {
                        Name = "Пользовательский",
                        Price = 10
                    }
                );
                context.SaveChanges();
            }
        }
    }
    
}
