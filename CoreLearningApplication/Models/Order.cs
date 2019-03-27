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
        //[Required]
        public string User { get; set; } // имя покупателя //TODO: заменить на нормального юзверя
        public DateTime EnteringTime { get; set; } //время въезда
        public DateTime LeavingTime { get; set; }//время выезда
        public bool IsFinished { get; set; }//
        //[Required]
        public int TariffId { get; set; } // ссылка на связанную модель Tariff
        public Tariff Tariff { get; set; }
    }
}
