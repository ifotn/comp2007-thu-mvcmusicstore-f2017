using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MvcMusicStore_Thu_F2017.Models;

namespace MvcMusicStore_Thu_F2017.Controllers
{
    public class ShoppingCartController : Controller
    {
        // db
        MusicStoreModel db = new MusicStoreModel();

        // GET: ShoppingCart
        public ActionResult Index()
        {
            // get current cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // set up viewmodel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetItems(),
                CartTotal = cart.GetTotal()
            };

            // pass populated Cart viewmodel to the view
            return View(viewModel);
        }

        // GET: AddToCart/5
        public ActionResult AddToCart(int AlbumId)
        {
            // Get current cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the selected album
            var album = db.Albums.SingleOrDefault(a => a.AlbumId == AlbumId);

            // Add album to the cart
            cart.AddToCart(album);

            // Redirect to the cart page (/ShoppingCart/Index)
            return RedirectToAction("Index");
        }

        // GET: RemoveFromCart/5
        public ActionResult RemoveFromCart(int AlbumId)
        {
            // Get current cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // if quantity of selected album > 1, subtract 1 from quantity
            cart.RemoveFromCart(AlbumId);

            // reload updated cart page
            return RedirectToAction("Index");
        }

        // GET: AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }
    }
}