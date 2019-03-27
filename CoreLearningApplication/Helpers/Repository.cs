using System.Collections.Generic;
using System.Linq;
using CoreLearningApplication.Models;
using CoreLearningApplication.Test;

namespace CoreLearningApplication.Helpers
{
    public class Repository : IRepository
    {
        private TariffContext _context;

        public Repository(TariffContext context)
        {
            _context = context;
            Orders = context.Orders.ToList();
            Tariffs = context.Tariffs.ToList();
        }
 
        public void SaveChanges()
        {
            _context.Orders.UpdateRange(Orders);
            _context.Tariffs.UpdateRange(Tariffs);
        }
        public void SaveChanges(List<Order> orders)
        {
            _context.Orders.UpdateRange(orders);
        }
        public void SaveChanges(List<Tariff> tariffs)
        {
            _context.Tariffs.UpdateRange(tariffs);
        }

        public List<Order> Orders { get; set; }

        public List<Tariff> Tariffs { get; set; }
    }
}