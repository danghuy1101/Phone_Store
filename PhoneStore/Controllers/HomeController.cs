using PhoneStore.Models;
using System.Linq;
using System.Web.Mvc;

namespace PhoneStore.Controllers
{
    public class HomeController : Controller
    {
        private PhoneStoreEntities db = new PhoneStoreEntities();
        public ActionResult Index()
        {
            ViewBag.CategoriesList = db.categories.ToList();
            ViewBag.ProductsList = (from item in db.phones orderby item.id descending select item).ToList().Take(10);

            int firstCate = db.categories.First().id;
            int secondCate = db.categories.FirstOrDefault(c => c.id != firstCate).id;

            ViewBag.FirstCate = db.categories.FirstOrDefault(c => c.id == firstCate);
            ViewBag.SecondCate = db.categories.FirstOrDefault(c => c.id == secondCate);

            ViewBag.ProductsList_1 = db.phones.Where(p => p.category_id == firstCate).ToList().Take(10);
            ViewBag.ProductsList_2 = db.phones.Where(p => p.category_id == secondCate).ToList().Take(10);

            var products = db.phones.ToList();
            var productRatings = products.ToDictionary(product => product.id, product => db.reviews
                .Where(r => r.phone_id == product.id)
                .Average(r => (int?)r.rating) ?? 0);

            ViewBag.ProductsList = products;
            ViewBag.ProductRatings = productRatings;

            return View();
        }
    }
}