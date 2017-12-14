using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicStore.Data;
using MusicStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicStore.Web.Infrastructure.ShoppingCartIService
{
    public class ShoppingCart
    {
        private readonly MusicStoreDbContext db;

        public ShoppingCart(MusicStoreDbContext db)
        {
            this.db = db;
        }

        public string ShoppingCartId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
                .HttpContext.Session;

            var context = services.GetService<MusicStoreDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddToCart(int songId, int amount)
        {
            var shoppingCartItem =
                      db.ShoppingCartItems.SingleOrDefault(
                        s => s.SongId == songId && s.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    SongId = songId,
                    Amount = 1
                };

                db.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            db.SaveChanges();
        }

        public int RemoveFromCart(int songId)
        {
            var shoppingCartItem =
                    db.ShoppingCartItems.SingleOrDefault(
                        s => s.SongId == songId && s.ShoppingCartId == ShoppingCartId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    db.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            db.SaveChanges();

            return localAmount;
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??
                   (ShoppingCartItems =
                       db.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                           .Include(s => s.Song)
                           .ToList());
        }

        public void ClearCart()
        {
            var cartItems = db
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            db.ShoppingCartItems.RemoveRange(cartItems);

            db.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = db.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Song.Price * c.Amount).Sum();

            return total;
        }
    }
}
