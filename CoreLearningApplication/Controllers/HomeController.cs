using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AuthApp.Models;
using Microsoft.AspNetCore.Mvc;
using CoreLearningApplication.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoreLearningApplication.Controllers
{
    public class HomeController : Controller
    {
        TariffContext db;
        UserContext userDB;

        #region Buy

        

         
        [HttpGet]
        public IActionResult Buy(int id)
        {
            ViewBag.TariffId = id;
            return View();
        }
        [HttpPost]
        public string Buy(Order order)
        {
            order.EnteringTime = DateTime.Now;
            db.Orders.Add(order);
           var selectedTariff = db.Tariffs.First(x => x.Id == order.TariffId);
            // сохраняем в бд все изменения
            db.SaveChanges();
            return "Добро пожаловать, " + order.User + ", с вас примерно " + order.Time* selectedTariff.Price+ " рублей";
        }
        #endregion

        public HomeController(TariffContext context) //TODO: понять откудаж ты вызвался и где вторая база
        {
            db = context;
        }


        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegisteredTariffs()
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
