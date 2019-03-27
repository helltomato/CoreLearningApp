using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CoreLearningApplication.Controllers;
using CoreLearningApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace CoreLearningApplication.Test
{
    public interface IRepository
    {
        List<Order> Orders { get; set; }
        List<Tariff> Tariffs { get; set; }
        void SaveChanges();
    }
    public class HomeControllerTests
    {

        [Fact]
        public void IndexViewDataMessage()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.Orders).Returns(GetTestOpenOrders);
            mock.Setup(repo => repo.Tariffs).Returns(GetTestOpenTariffs);
            mock.Setup(repo => repo.SaveChanges());
            HomeController controller = new HomeController(mock.Object);

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);

        }

        private List<Order> GetTestOpenOrders()
        {
            return new List<Order>()
            {
                new Order()
                {
                    EnteringTime = DateTime.Now,
                    IsFinished = false,
                    OrderId = 1,
                    User = "VASA",
                    Tariff = new Tariff() {Id = 1, Name = "тест", Price = 10},
                    TariffId = 1}
                };
        }
        private List<Tariff> GetTestOpenTariffs()
        {
            return new List<Tariff>()
            {
                new Tariff() {  Id = 1, Name = "тест", Price = 10 },
                new Tariff() {  Id = 2, Name = "тест1", Price = 20 },
            };
        }

        private void TestSave()
        {
        }
        //private TariffContext GeTariffContext()
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<TariffContext>();
        //    optionsBuilder.UseSqlServer($@"Server=(localdb)\mssqllocaldb;Database=tariffesestoredb;Trusted_Connection=True;MultipleActiveResultSets=true");


        //    var context = new TariffContext(/*optionsBuilder.Options*/);
        //    context.Orders.Add(new Order() {EnteringTime = DateTime.Now, IsFinished = false, OrderId = 1, Tariff = new Tariff() {Id = 1, Name = "тест", Price = 10}, TariffId = 1});
        //    return context;

        //}
    }
}

