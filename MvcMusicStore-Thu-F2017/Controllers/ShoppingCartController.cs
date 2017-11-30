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

        [Authorize]
        // GET: AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        // POST: AddressAndPayment
        public ActionResult AddressAndPayment(FormCollection values)
        {
            // instantiate a new Order
            var order = new Order();

            // populate the Order properties with the matching form fields
            TryUpdateModel(order);

            // check the promotion code instead of payment
            if (string.Equals(values["PromoCode"], "FREE", StringComparison.OrdinalIgnoreCase) == false)
            {
                ViewBag.Message = "Invalid Promo Code.  Please use the code 'FREE'.";
                return View(order);
            }

            // save the Order & get the new OrderId
            order.Username = User.Identity.Name;
            order.OrderDate = DateTime.Now;
            order.Email = User.Identity.Name;

            var cart = ShoppingCart.GetCart(this.HttpContext);
            order.Total = cart.GetTotal();

            db.Orders.Add(order);
            db.SaveChanges();

            // save the Order Details
            var cartItems = cart.GetItems();

            foreach (Cart item in cartItems)
            {
                var orderDetail = new OrderDetail();
                orderDetail.AlbumId = item.AlbumId;
                orderDetail.OrderId = order.OrderId;
                orderDetail.Quantity = item.Count;
                orderDetail.UnitPrice = item.Album.Price;

                // save the detail
                db.OrderDetails.Add(orderDetail);
            }

            // commit order detail save
           db.SaveChanges();

            // empty the cart
            cart.EmptyCart();

            // display Order Confirmation page
            return RedirectToAction("OrderSummary", new { OrderId = order.OrderId });
        }

        [Authorize]
        // GET: OrderSummary/5
        public ActionResult OrderSummary(int OrderId)
        {
            // retrieve selected order if made by the current user
            var order = db.Orders.SingleOrDefault(o => o.OrderId == OrderId
            && o.Username == User.Identity.Name);
            
            if (order != null)
            {
                return View(order);
            }
            else
            {
                return View("Error");
            }
        }
    }
}