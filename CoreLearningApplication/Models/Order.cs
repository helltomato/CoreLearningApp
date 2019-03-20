using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLearningApplication.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string User { get; set; } // имя покупателя //TODO: заменить на нормального юзверя
        public DateTime EnteringTime { get; set; }
        public DateTime LeavingTime { get; set; }
        public bool IsFinished { get; set; }
        public int Time { get; set; } // время //TODO: заменить на нормальное время после расчёта выезда
        public int TariffId { get; set; } // ссылка на связанную модель Tariff
        public Tariff Tariff { get; set; }
    }
}
