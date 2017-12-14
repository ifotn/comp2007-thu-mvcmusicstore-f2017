using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcMusicStore_Thu_F2017.Models;

namespace MvcMusicStore_Thu_F2017.Controllers
{

    [Authorize(Roles = "Administrator")]
    public class StoreManagerController : Controller
    {
        // old direct db connection - now in Models/EFStoreManagerRepository
        //private MusicStoreModel db = new MusicStoreModel();

        // repo link
        private IStoreManagerRepository db;

        // if no param passed to constructor, use EF Repository & DbContext
        public StoreManagerController()
        {
            this.db = new EFStoreManagerRepository();
        }

        // if mock repo object passed to constructor, use Mock interface for unit testing
        public StoreManagerController(IStoreManagerRepository smRepo)
        {
            this.db = smRepo;
        }

        // GET: StoreManager
        public ViewResult Index()
        {
            var albums = db.Albums.Include(a => a.Artist).Include(a => a.Genre);

            ViewBag.AlbumCount = albums.Count();
            return View(albums.OrderBy(a => a.Artist.Name).ThenBy(a => a.Title).ToList());
        }

        // POST: StoreManager - for Title Search on Index View
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string Title)
        {
            var albums = from a in db.Albums
                         where a.Title.Contains(Title)
                         orderby a.Artist.Name, a.Title
                         select a;

            ViewBag.AlbumCount = albums.Count();
            return View(albums);
        }

        [AllowAnonymous]
        // GET: StoreManager/Details/5
        public ViewResult Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            Album album = db.Albums.SingleOrDefault(a => a.AlbumId == id);
            if (album == null)
            {
                return View("Error");
            }
            return View(album);
        }

        // GET: StoreManager/Create
        public ActionResult Create()
        {
            ViewBag.ArtistId = new SelectList(db.Artists.OrderBy(a => a.Name), "ArtistId", "Name");
            ViewBag.GenreId = new SelectList(db.Genres.OrderBy(g => g.Name), "GenreId", "Name");
            return View("Create");
        }

        // POST: StoreManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album)
        {
            if (ModelState.IsValid)
            {
                // process album cover upload if any
                if (Request != null)
                {
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];

                        if (file.FileName != "" && file.ContentLength > 0)
                        {
                            string path = Server.MapPath("/Content/Images/") + file.FileName;
                            file.SaveAs(path);

                            album.AlbumArtUrl = "/Content/Images/" + file.FileName;
                        }
                    }
                }
                
                // scaffold code for inserting
                //db.Albums.Add(album);
                //db.SaveChanges();

                // new repository code for inserting
                db.Save(album);

                return RedirectToAction("Index");
            }

            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            return View("Create", album);
        }

        // GET: StoreManager/Edit/5
        public ViewResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            // scaffold code - now disabled
            //Album album = db.Albums.Find(id);

            // new repository code replacing line above
            Album album = db.Albums.SingleOrDefault(a => a.AlbumId == id);

            if (album == null)
            {
                return View("Error");
            }
            ViewBag.ArtistId = new SelectList(db.Artists.OrderBy(a => a.Name), "ArtistId", "Name", album.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres.OrderBy(g => g.Name), "GenreId", "Name", album.GenreId);
            return View(album);
        }

        // POST: StoreManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album)
        {
            if (ModelState.IsValid)
            {
                // process album cover upload if any
                if (Request != null)
                {
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];

                        if (file.FileName != "" && file.ContentLength > 0)
                        {
                            string path = Server.MapPath("/Content/Images/") + file.FileName;
                            file.SaveAs(path);

                            album.AlbumArtUrl = "/Content/Images/" + file.FileName;
                        }
                    }
                }

                // scaffold code - old
                //db.Entry(album).State = EntityState.Modified;
                //db.SaveChanges();

                // repository code - new
                db.Save(album);

                return RedirectToAction("Index");
            }
            ViewBag.ArtistId = new SelectList(db.Artists, "ArtistId", "Name", album.ArtistId);
            ViewBag.GenreId = new SelectList(db.Genres, "GenreId", "Name", album.GenreId);
            return View("Edit", album);
        }

        // GET: StoreManager/Delete/5
        public ViewResult Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }
            // scaffold code - now disabled
            //Album album = db.Albums.Find(id);

            // new repository code replacing line above
            Album album = db.Albums.SingleOrDefault(a => a.AlbumId == id);
            if (album == null)
            {
                return View("Error");
            }
            return View(album);
        }

        // POST: StoreManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            // scaffold code - now disabled
            //Album album = db.Albums.Find(id);
            //db.Albums.Remove(album);
            //db.SaveChanges();

            // new repository code replacing line above
            if (id == null)
            {
                return View("Error");
            }
            Album album = db.Albums.SingleOrDefault(a => a.AlbumId == id);
            if (album == null)
            {
                return View("Error");
            }

            db.Delete(album);

            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
