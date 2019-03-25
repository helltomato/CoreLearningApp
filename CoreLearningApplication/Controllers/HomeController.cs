using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreLearningApplication.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreLearningApplication.Controllers
{
    public class HomeController : Controller
    {
        TariffContext db;
        UserContext userDB;

        public HomeController(TariffContext context) //TODO: понять откудаж ты вызвался и где вторая база
        {
            db = context;
        }


        [Authorize]
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            //если пользователь уже на парковке
           var existingOrder = db.Orders.FirstOrDefault(x => x.User == User.Identity.Name && !x.IsFinished);

                return View(existingOrder);
        }

        public IActionResult Tariffs()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View(db.Tariffs.ToList());
            }
            else //оставить только первый тариф, который подороже
            {
                return View(new List<Tariff>(){db.Tariffs.FirstOrDefault()});
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
            order.Tariff = db.Tariffs.First(x => x.Id == order.TariffId);
            db.Orders.Add(order);
            db.SaveChanges();
            return  "Добро пожаловать, " + order.User;
        }
        #endregion

        [HttpGet]
        public IActionResult LeaveParking(int id)
        {
            var order = db.Orders.FirstOrDefault(x => x.OrderId == id);
            order.IsFinished = true;
            order.LeavingTime = DateTime.Now;
            db.SaveChanges();
            var spentTime = order.LeavingTime.Subtract(order.EnteringTime).Seconds/*.TotalHours*/; //для наглядности
            var bill = spentTime * db.Tariffs.First(x=> x.Id == order.TariffId).Price;

            ViewBag.Title = $@"Досвиданье {order.User}!";
            ViewBag.SpentTime = spentTime.ToString();
            ViewBag.Bill = bill.ToString();
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
