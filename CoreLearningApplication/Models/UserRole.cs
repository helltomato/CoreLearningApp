using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLearningApplication.Models
{
    /// <summary>
    /// Класс реализующий связь между типом пользователя и доступными ему тарифами
    /// </summary>
    public class UserRole
    {
        public int Id { get; set; }
        public UserType UserType { get;  set; }
        public int AvalibleTariffId { get; set; }
        public Tariff AvalibleTariff { get;  set; }

        public UserRole()
        {
            
        }
        public UserRole(UserType userType,Tariff avalibleTariff)
        {
            UserType = userType;
            AvalibleTariff = avalibleTariff;
            AvalibleTariffId = avalibleTariff.Id;
        }
    }
}
