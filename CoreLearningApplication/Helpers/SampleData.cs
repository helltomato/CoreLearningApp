using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLearningApplication.Models;

namespace CoreLearningApplication.Helpers
{ 

    public static class SampleData
    {
        public static void Initialize(ParkingDbContext context) //наполняем базу тарифов
        {
            var guestTariff = new Tariff("Гостевой", 20);
            var userTariff = new Tariff("Пользовательский", 10);
            if (!context.Tariffs.Any())
            {
                context.Tariffs.AddRange(guestTariff,userTariff);
                context.SaveChanges();
            }
            if (!context.UserRoles.Any()) //наполняем базу ролей
            {
                context.UserRoles.AddRange
                (
                    new UserRole(UserType.Unregistered, guestTariff),
                    new UserRole(UserType.Registered, userTariff),
                    new UserRole(UserType.Registered, guestTariff)
                );
            
                context.SaveChanges();
            }

          
        }
    }
    
}
