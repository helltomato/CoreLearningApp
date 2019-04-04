using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreLearningApplication.Models
{
    public class Tariff
    {
        public int Id { get; set; }
        public string Name { get; private set; } //название
        public int Price { get; private set; }//цена

        public Tariff(string name, int price)
        {
            Name = name;
            Price = price;
        }
    }
}
