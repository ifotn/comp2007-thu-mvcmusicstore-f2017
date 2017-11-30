using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore_Thu_F2017.Models
{
    public class ShoppingCart
    {
        // db 
        MusicStoreModel db = new MusicStoreModel();

        string ShoppingCartId { get; set; }

        // Get current Cart 
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            // get cart id from username if we have one, from guid if not
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        public string GetCartId(HttpContextBase context)
        {
            // is there an existing cart in the user's session?
            if (context.Session["CartId"] == null)
            {
                // do we know who the user is?
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session["CartId"] = context.User.Identity.Name;
                }
                else
                {
                    // user is unknown so generate random cart Id
                    Guid tempCartId = Guid.NewGuid();
                    context.Session["CartId"] = tempCartId.ToString();
                }
            }

            // send back CartId
            return context.Session["CartId"].ToString();
        }

        // add item
        public void AddToCart(Album album)
        {
            // is album already in cart?
            var cartItem = db.Carts.SingleOrDefault(c => c.CartId == ShoppingCartId
                && c.AlbumId == album.AlbumId);

            // new item
            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    AlbumId = album.AlbumId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                db.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }

            // commit changes
            db.SaveChanges();
        }

        // Remove Album from Cart
        public void RemoveFromCart(int AlbumId)
        {
            // get current item from Cart table
            var item = db.Carts.SingleOrDefault(c => c.CartId == ShoppingCartId
                && c.AlbumId == AlbumId);

            // check how many of this album are currently in the cart
            if (item.Count > 1)
            {
                item.Count--;
            }
            else
            {
                db.Carts.Remove(item);
            }
            db.SaveChanges();
        }

        // Get Cart Items
        public List<Cart> GetItems()
        {
            return db.Carts.Where(c => c.CartId == ShoppingCartId).ToList();
        }

        // Get Cart Total
        public decimal GetTotal()
        {
            // use linq to do 3 things at once:
            // get the items in the current cart
            // calculate the line item total for each
            // sum all the line item totals and return a single total value
            decimal? total = (from c in db.Carts
                              where c.CartId == ShoppingCartId
                              select (int?)c.Count * c.Album.Price).Sum();

            // return the total if there is one, if total is null, return zero
            return total ?? decimal.Zero;
        }
    }
}