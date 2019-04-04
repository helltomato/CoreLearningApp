using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLearningApplication.Models;
using CoreLearningApplication.Test;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CoreLearningApplication.Helpers
{
    public class Repository : IRepository
    {
        private ParkingDbContext _context;

        public Repository(ParkingDbContext context)
        {
            _context = context;
            Orders = context.Orders.ToList();
            Tariffs = context.Tariffs.ToList();
            Users = context.Users.ToList();
            UserRoles = context.UserRoles.ToList();
        }
 
        public void SaveChanges()
        {
            _context.Orders.UpdateRange(Orders);
            _context.Tariffs.UpdateRange(Tariffs);
            _context.Users.UpdateRange(Users);
            _context.UserRoles.UpdateRange(UserRoles);
            _context.SaveChanges();

        }
        public void SaveChanges(List<Order> orders)
        {
            _context.Orders.UpdateRange(orders);
        }
        public void SaveChanges(List<Tariff> tariffs)
        {
            _context.Tariffs.UpdateRange(tariffs);
        }

        public async void AddOrderAsync(Order order)
        {

            await Task.Run(() =>_context.Orders.AddAsync(order));
            await Task.Run(() => _context.SaveChangesAsync());
        }

        public List<Order> Orders { get; set; }
        public List<Tariff> Tariffs { get; set; }
        public List<User> Users { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}