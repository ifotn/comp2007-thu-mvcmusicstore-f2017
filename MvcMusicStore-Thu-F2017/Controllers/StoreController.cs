using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore_Thu_F2017.Controllers
{
    public class StoreController : Controller
    {
        // GET: Store
        public ActionResult Index()
        {
            return View();
        }

        // GET: Store/Browse
        public ActionResult Browse(string genre)
        {
            ViewBag.Genre = genre;
            return View();
        }
    }
}