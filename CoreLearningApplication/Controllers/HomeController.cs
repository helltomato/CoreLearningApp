using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

            var existingOrder = _repo.Orders.FirstOrDefault(x => x.User.Name == userName && !x.IsFinished);

            return View(existingOrder);
        }

        public IActionResult Tariffs()
        {
            //ищем юзверя в базе
            var userFromDb = _repo.Users.FirstOrDefault(y => y.Name == User.Identity.Name);
            //если такой есть - перебираем все роли для данного юзертайпа и собираем доступные тарифы из ролей
            var tariffs = (User.Identity.IsAuthenticated && userFromDb != null) 
                ? _repo.UserRoles.FindAll(x => x.UserType == userFromDb.UserType).Select(y => y.AvalibleTariff).ToList()
                //
                : _repo.UserRoles.FindAll(x => x.UserType == UserType.Unregistered).Select(y => y.AvalibleTariff).ToList();

            return View(tariffs);
        }

        #region Buy

        [HttpGet]
        public IActionResult EnterParking(int id)
        {
            ViewBag.TariffId = id;
            return View();
        }
        [HttpPost]
        public string EnterParking(Order order ,string UserName)
        {
            order.EnteringTime = DateTime.Now;
            order.Tariff = _repo.Tariffs.First(x => x.Id == order.TariffId);
            order.User = _repo.Users.FirstOrDefault(y => y.Name == UserName) ?? Models.User.GetDefaultUser(UserName);

                 _repo.Orders.Add(order);
                 _repo.SaveChanges();

            return $"Добро пожаловать, {order.User.Name}";
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
