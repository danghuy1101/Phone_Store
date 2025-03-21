using PhoneStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PhoneStore.Controllers
{
    public class FavoriteProductController : Controller
    {
        PhoneStoreEntities db = new PhoneStoreEntities();
        // GET: FavoriteProduct

        public ActionResult FavoriteList(int id)
        {
            var favoriteProducts = db.favorites.Where(n => n.user_id == id).ToList();

            var productList = new List<phone>();
            foreach (var item in favoriteProducts)
            {
                var product = db.phones.FirstOrDefault(p => p.id == item.phone_id);
                if (product != null)
                {
                    productList.Add(product);
                }
            }
            ViewBag.ProductInfor = productList;
            return View(favoriteProducts);
        }

        [HttpPost]
        public ActionResult InsertListFavorite(favorite favoriteProd)
        {
            var userSession = Session["Account"] as PhoneStore.Models.user;

            favoriteProd.user_id = userSession.id;

            if (!db.phones.Any(p => p.id == favoriteProd.phone_id))
                return RedirectToAction("Index", "Home");

            if (!db.favorites.Any(p => p.phone_id == favoriteProd.phone_id && p.user_id == favoriteProd.user_id))
            {
                favoriteProd.added_at = DateTime.Now;
                db.favorites.Add(favoriteProd);
                db.SaveChanges();
            }

            return RedirectToAction("FavoriteList", "FavoriteProduct", new { id = favoriteProd.user_id });
        }

        [HttpPost]
        public ActionResult DeleteProduct(int phone_id)
        {
            var userSession = Session["Account"] as PhoneStore.Models.user;

            var prod = db.favorites.FirstOrDefault(p => p.phone_id == phone_id && p.user_id == userSession.id);
            if (prod != null)
            {
                db.favorites.Remove(prod);
                db.SaveChanges();
            }

            return RedirectToAction("FavoriteList", "FavoriteProduct", new { id = userSession.id });
        }
    }
}