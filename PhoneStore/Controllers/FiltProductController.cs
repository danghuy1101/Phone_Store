using PhoneStore.Models;
using System.Linq;
using System.Web.Mvc;

namespace PhoneStore.Controllers
{
    public class FiltProductController : Controller
    {
        PhoneStoreEntities db = new PhoneStoreEntities();
        // GET: FiltProduct
        public ActionResult Index()
        {
            return View();
        }

        //trên 4 củ
        public ActionResult Under4MilAllProduct(int id)
        {
            if (id == 0)
            {
                var products = (from item in db.phones orderby item.id descending where item.price <= 4000000 select item).ToList();

                var productRatings = products.ToDictionary(p => p.id, p => db.reviews
                    .Where(r => r.phone_id == p.id)
                    .Average(r => (int?)r.rating) ?? 0);

                ViewBag.ProductRatings = productRatings;

                ViewBag.CategoryProd = db.categories.FirstOrDefault(n => n.id == id);
                ViewBag.IdCategory = id;
                return View(products);
            }
            else
            {
                var products = (from item in db.phones orderby item.id descending where item.price <= 4000000 && item.category_id == id select item).ToList();

                var productRatings = products.ToDictionary(p => p.id, p => db.reviews
                    .Where(r => r.phone_id == p.id)
                    .Average(r => (int?)r.rating) ?? 0);

                ViewBag.ProductRatings = productRatings;

                ViewBag.CategoryProd = db.categories.FirstOrDefault(n => n.id == id);
                ViewBag.IdCategory = id;
                return View(products);
            }
        }

        //từ 4 củ tới 8 củ
        public ActionResult From4To8MilAllProduct(int id)
        {
            if (id == 0)
            {
                var products = (from item in db.phones orderby item.id descending where item.price >= 4000000 && item.price <= 8000000 select item).ToList();

                var productRatings = products.ToDictionary(p => p.id, p => db.reviews
                    .Where(r => r.phone_id == p.id)
                    .Average(r => (int?)r.rating) ?? 0);

                ViewBag.ProductRatings = productRatings;

                ViewBag.CategoryProd = db.categories.FirstOrDefault(n => n.id == id);
                ViewBag.IdCategory = id;
                return View(products);
            }
            else
            {
                var products = (from item in db.phones orderby item.id descending where item.price >= 4000000 && item.price <= 8000000 && item.category_id == id select item).ToList();

                var productRatings = products.ToDictionary(p => p.id, p => db.reviews
                    .Where(r => r.phone_id == p.id)
                    .Average(r => (int?)r.rating) ?? 0);

                ViewBag.ProductRatings = productRatings;

                ViewBag.CategoryProd = db.categories.FirstOrDefault(n => n.id == id);
                ViewBag.IdCategory = id;
                return View(products);
            }
        }

        //từ 8 củ tới 12 củ
        public ActionResult From8To12MilAllProduct(int id)
        {
            if (id == 0)
            {
                var products = (from item in db.phones orderby item.id descending where item.price >= 8000000 && item.price <= 12000000 select item).ToList();

                var productRatings = products.ToDictionary(p => p.id, p => db.reviews
                    .Where(r => r.phone_id == p.id)
                    .Average(r => (int?)r.rating) ?? 0);

                ViewBag.ProductRatings = productRatings;

                ViewBag.CategoryProd = db.categories.FirstOrDefault(n => n.id == id);
                ViewBag.IdCategory = id;
                return View(products);
            }
            else
            {
                var products = (from item in db.phones orderby item.id descending where item.price >= 8000000 && item.price <= 12000000 && item.category_id == id select item).ToList();

                var productRatings = products.ToDictionary(p => p.id, p => db.reviews
                    .Where(r => r.phone_id == p.id)
                    .Average(r => (int?)r.rating) ?? 0);

                ViewBag.ProductRatings = productRatings;

                ViewBag.CategoryProd = db.categories.FirstOrDefault(n => n.id == id);
                ViewBag.IdCategory = id;
                return View(products);
            }
        }

        //trên 12 củ
        public ActionResult Over12MilAllProduct(int id)
        {
            if (id == 0)
            {
                var products = (from item in db.phones orderby item.id descending where item.price >= 12000000 select item).ToList();

                var productRatings = products.ToDictionary(p => p.id, p => db.reviews
                    .Where(r => r.phone_id == p.id)
                    .Average(r => (int?)r.rating) ?? 0);

                ViewBag.ProductRatings = productRatings;

                ViewBag.CategoryProd = db.categories.FirstOrDefault(n => n.id == id);
                ViewBag.IdCategory = id;
                return View(products);
            }
            else
            {
                var products = (from item in db.phones orderby item.id descending where item.price >= 12000000 && item.category_id == id select item).ToList();

                var productRatings = products.ToDictionary(p => p.id, p => db.reviews
                    .Where(r => r.phone_id == p.id)
                    .Average(r => (int?)r.rating) ?? 0);

                ViewBag.ProductRatings = productRatings;

                ViewBag.CategoryProd = db.categories.FirstOrDefault(n => n.id == id);
                ViewBag.IdCategory = id;
                return View(products);
            }
        }

        //giá thấp -> cao
        public ActionResult IncreaseWithPrice(int id)
        {
            if (id == 0)
            {
                var products = (from item in db.phones orderby item.price ascending select item).ToList();

                var productRatings = products.ToDictionary(p => p.id, p => db.reviews
                    .Where(r => r.phone_id == p.id)
                    .Average(r => (int?)r.rating) ?? 0);

                ViewBag.ProductRatings = productRatings;

                ViewBag.CategoryProd = db.categories.FirstOrDefault(n => n.id == id);
                ViewBag.IdCategory = id;
                return View(products);
            }
            else
            {
                var products = (from item in db.phones orderby item.price ascending where item.category_id == id select item).ToList();

                var productRatings = products.ToDictionary(p => p.id, p => db.reviews
                    .Where(r => r.phone_id == p.id)
                    .Average(r => (int?)r.rating) ?? 0);

                ViewBag.ProductRatings = productRatings;

                ViewBag.CategoryProd = db.categories.FirstOrDefault(n => n.id == id);
                ViewBag.IdCategory = id;
                return View(products);
            }
        }

        //giá cao -> thấp
        public ActionResult DescreaseWithPrice(int id)
        {
            if (id == 0)
            {
                var products = (from item in db.phones orderby item.price descending select item).ToList();

                var productRatings = products.ToDictionary(p => p.id, p => db.reviews
                    .Where(r => r.phone_id == p.id)
                    .Average(r => (int?)r.rating) ?? 0);

                ViewBag.ProductRatings = productRatings;

                ViewBag.CategoryProd = db.categories.FirstOrDefault(n => n.id == id);
                ViewBag.IdCategory = id;
                return View(products);
            }
            else
            {
                var products = (from item in db.phones orderby item.price descending where item.category_id == id select item).ToList();

                var productRatings = products.ToDictionary(p => p.id, p => db.reviews
                    .Where(r => r.phone_id == p.id)
                    .Average(r => (int?)r.rating) ?? 0);

                ViewBag.ProductRatings = productRatings;

                ViewBag.CategoryProd = db.categories.FirstOrDefault(n => n.id == id);
                ViewBag.IdCategory = id;
                return View(products);
            }
        }
    }
}