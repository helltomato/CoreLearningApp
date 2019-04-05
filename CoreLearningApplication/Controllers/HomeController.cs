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
            Order existingOrder = null;
            //изучаем наличие печенья
            if (HttpContext.Request.Cookies.TryGetValue("OrderId", out var orderString) 
                && HttpContext.Request.Cookies.TryGetValue("UserType", out var userTypeString)
                && HttpContext.Request.Cookies.TryGetValue("UserId", out var userIdString))
            {   //изучаем качество печенья
                if (int.TryParse(orderString, out var orderId) 
                    && Enum.TryParse<UserType>(userTypeString, out var userType)
                    && int.TryParse(userIdString, out var userId) )
                {
                    var expectedOrder = _repo.Orders.FirstOrDefault(x => x.OrderId == orderId);
                    //валидация
                    if(expectedOrder!=null)
                        if (expectedOrder.User.Id == userId && expectedOrder.User.UserType == userType && !expectedOrder.IsFinished)
                            existingOrder = expectedOrder;
                }
               
            }

            //если не нашёлся по кукам, проверяем по Имени
            var userName = User?.Identity?.Name;
            if(existingOrder == null)
               existingOrder = _repo.Orders.FirstOrDefault(x => x.User.Name == userName && !x.IsFinished);

            return View(existingOrder);
        }

        public IActionResult Tariffs()
        {
            //ищем юзверя в базе
            var userFromDb = _repo.Users.FirstOrDefault(y => y.Name == User.Identity.Name);
            //если такой есть - перебираем все роли для данного юзертайпа и собираем доступные тарифы из ролей
            var tariffs = (User.Identity.IsAuthenticated && userFromDb != null) 
                ? _repo.UserRoles.FindAll(x => x.UserType == userFromDb.UserType).Select(y => y.AvalibleTariff).ToList()
                //то-же самое но для Unregistered
                : _repo.UserRoles.FindAll(x => x.UserType == UserType.Unregistered).Select(y => y.AvalibleTariff).ToList();

            return View(tariffs);
        }

        #region Entering

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
            //устанавливаем юзера из базы, если его нет то ставим дефолтного незарегестрированного
            order.User = _repo.Users.FirstOrDefault(y => y.Name == UserName) ?? Models.User.GetDefaultUser(UserName);
            _repo.Orders.Add(order);
            _repo.SaveChanges();
            //отдаём куку с токеном
            HttpContext.Response.Cookies.Append("OrderId", order.OrderId.ToString());
            //информация о пользователе вообщем-то есть в ордере, но с куками не трудно махинировать на стороне браузера
            //другими словами можно выехать по чужому токену
            //так что немного информации о пользователе позволит проверить по базе валидность
            //и вычислить мамкиного хацкера
            HttpContext.Response.Cookies.Append("UserId", order.User.Id.ToString());
            //неуверен но пускай
            HttpContext.Response.Cookies.Append("UserType", order.User.UserType.ToString());

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
                HttpContext.Response.Cookies.Delete("OrderId");
                HttpContext.Response.Cookies.Delete("UserId");
                HttpContext.Response.Cookies.Delete("UserType");
                var spentTime = order.LeavingTime.Subtract(order.EnteringTime).Seconds /*.TotalHours*/; //для наглядности
                var bill = spentTime * _repo.Tariffs.First(x => x.Id == order.TariffId).Price;

                ViewBag.Title = $@"Досвиданье {order.User.Name}!";
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
