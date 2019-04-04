using System.Collections.Generic;
using CoreLearningApplication.Models;

namespace CoreLearningApplication
{
    public interface IRepository
    {
        void AddOrderAsync(Order order); 
        List<Order> Orders { get; set; }
        List<Tariff> Tariffs { get; set; }
        List<User> Users { get; set; }
        List<UserRole> UserRoles { get; set; }
        void SaveChanges();
    }
}