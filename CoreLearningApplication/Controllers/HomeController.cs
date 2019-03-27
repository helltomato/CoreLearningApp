using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CoreLearningApplication.Models;
using CoreLearningApplication.Test;
using Microsoft.AspNetCore.Authorization;

namespace CoreLearningApplication.Controllers
{
    public class HomeController : Controller
    {
        //TariffContext db;

        private IRepository _repo;

        public HomeController(IRepository repo)
        {
            _repo = repo;
        }


        [Authorize]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            //если пользователь уже на парковке

            var userName = User?.Identity?.Name ?? "VASA";

            var existingOrder = _repo.Orders.FirstOrDefault(x => x.User == userName && !x.IsFinished);

            return View(existingOrder);
        }

        public IActionResult Tariffs()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(_repo.Tariffs);
            }
            else //оставить только первый тариф, который подороже
            {
                return View(new List<Tariff>() { _repo.Tariffs.FirstOrDefault() });
            }
        }

        #region Buy

        [HttpGet]
        public IActionResult EnterParking(int id)
        {
            ViewBag.TariffId = id;
            return View();
        }
        [HttpPost]
        public string EnterParking(Order order)
        {
            order.EnteringTime = DateTime.Now;
            order.Tariff = _repo.Tariffs.First(x => x.Id == order.TariffId);
            _repo.Orders.Add(order);
            _repo.SaveChanges();
            return $"Добро пожаловать, {order.User}";
        }
        #endregion

        [HttpGet]
        public IActionResult LeaveParking(int id)
        {
            var order = _repo.Orders.FirstOrDefault(x => x.OrderId == id);
            if (order != null)
            {
                order.IsFinished = true;
                order.LeavingTime = DateTime.Now;
                _repo.SaveChanges();
                var spentTime = order.LeavingTime.Subtract(order.EnteringTime).Seconds /*.TotalHours*/; //для наглядности
                var bill = spentTime * _repo.Tariffs.First(x => x.Id == order.TariffId).Price;

                ViewBag.Title = $@"Досвиданье {order.User}!";
                ViewBag.SpentTime = spentTime.ToString();
                ViewBag.Bill = bill.ToString();
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
