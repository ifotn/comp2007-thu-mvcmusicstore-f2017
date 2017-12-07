using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore_Thu_F2017.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public int sum(int x, int y)
        {
            return x + y;
        }

        public string getWeather(int Temp)
        {
            if (Temp < 0)
            {
                return "I need a coffee";
            }
            else if (Temp < 25)
            {
                return "I need a tea";
            }
            else
            {
                return "I need a beer";
            }
        }
    }
}