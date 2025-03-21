using PhoneStore.Models;
using System.Linq;
using System.Web.Mvc;

namespace PhoneStore.Controllers
{
    public class CategoryController : Controller
    {
        private PhoneStoreEntities db = new PhoneStoreEntities();

        // GET: Category
        public ActionResult Index(int id)
        {
            var product = db.phones.Where(n => n.category_id == id).ToList();
            ViewBag.CategoryProd = db.categories.FirstOrDefault(n => n.id == id);
            ViewBag.IdCategory = id;

            var productRatings = product.ToDictionary(p => p.id, p => db.reviews
                .Where(r => r.phone_id == p.id)
                .Average(r => (int?)r.rating) ?? 0);

            ViewBag.ProductRatings = productRatings;

            return View(product);
        }

        public ActionResult GetAllProduct()
        {
            var product = (from item in db.phones orderby item.id descending select item).ToList();

            var productRatings = product.ToDictionary(p => p.id, p => db.reviews
                .Where(r => r.phone_id == p.id)
                .Average(r => (int?)r.rating) ?? 0);

            ViewBag.ProductRatings = productRatings;

            return View(product);
        }

        public ActionResult Search(string searchString)
        {
            var result = db.phones.Where(s => s.name.Contains(searchString)).ToList();

            var productRatings = result.ToDictionary(p => p.id, p => db.reviews
                .Where(r => r.phone_id == p.id)
                .Average(r => (int?)r.rating) ?? 0);

            ViewBag.ProductRatings = productRatings;

            return View(result);
        }
    }
}