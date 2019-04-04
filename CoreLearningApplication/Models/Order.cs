using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLearningApplication.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; } // имя покупателя 
        public DateTime EnteringTime { get; set; } //время въезда
        public DateTime LeavingTime { get; set; }//время выезда
        public bool IsFinished { get; set; }//
        [Required]
        public int TariffId { get; set; } // ссылка на связанную модель Tariff
        public Tariff Tariff { get; set; }
    }
}
