using MvcMusicStore_Thu_F2017.Models;
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
            // create a typed list of Genres
            var genres = new List<Genre>();

            // create 10 Genres in memory to pass to the main store view
            for (int i = 1; i <= 10; i++)
            {
                genres.Add(new Genre { Name = "Genre " + i.ToString() });
            }

           // pass the typed list to the view for display
           return View(genres);
        }

        // GET: Store/Browse
        public ActionResult Browse(string genre)
        {
            ViewBag.Genre = genre;
            return View();
        }
    }
}