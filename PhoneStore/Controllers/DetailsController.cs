using PhoneStore.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace PhoneStore.Controllers
{
    public class DetailsController : Controller
    {
        // GET: Details
        PhoneStoreEntities db = new PhoneStoreEntities();

        public ActionResult Index(int id)
        {
            ViewBag.ProdDetails = db.phones.FirstOrDefault(n => n.id == id);
            
            int thisProdCategories = db.phones.FirstOrDefault(n => n.id == id).category_id ?? 0;
            ViewBag.ThisProdCategories = db.categories.FirstOrDefault(n => n.id == thisProdCategories);

            ViewBag.ProductList = (from p in db.phones where p.category_id == thisProdCategories && p.id != id select p).ToList().Take(10);

            ViewBag.CommentList = (from c in db.reviews orderby c.id descending where c.phone_id == id select c).ToList();

            var productRatings = db.phones.ToDictionary(p => p.id, p => db.reviews
                .Where(r => r.phone_id == p.id)
                .Average(r => (int?)r.rating) ?? 0);

            ViewBag.ProductRatings = productRatings;

            return View();
        }

        [HttpPost]
        public ActionResult AddComment(review cmt)
        {
            int userId = cmt.user_id.GetValueOrDefault();
            int productId = cmt.phone_id.GetValueOrDefault();

            // Kiểm tra xem người dùng đã mua sản phẩm chưa
            bool hasPurchased = db.order_details
                .Join(db.orders, od => od.order_id, o => o.id, (od, o) => new { od, o })
                .Any(x => x.od.phone_id == productId && x.o.user_id == userId && x.o.status == "completed");

            if (!hasPurchased)
            {
                TempData["ErrorMessage"] = "Bạn phải mua sản phẩm trước mới có thể đánh giá.";
                return RedirectToAction("Index/" + productId, "Details");
            }

            // Kiểm tra xem người dùng đã đánh giá sản phẩm này chưa
            bool hasReviewed = db.reviews.Any(r => r.user_id == userId && r.phone_id == productId);
            if (hasReviewed)
            {
                TempData["ErrorMessage"] = "Bạn chỉ có thể đánh giá sản phẩm một lần.";
                return RedirectToAction("Index/" + productId, "Details");
            }

            if (ModelState.IsValid)
            {
                cmt.created_at = DateTime.Now;
                db.reviews.Add(cmt);
                db.SaveChanges();
            }
            //int productId = cmt.phone_id.GetValueOrDefault();
            return RedirectToAction("Index/" + productId, "Details");
        }

        public ActionResult DeleteComment(int id)
        {
            var cmt = db.reviews.Where(c => c.id == id).FirstOrDefault();
            int idProduct = cmt.phone_id.GetValueOrDefault();
            if (ModelState.IsValid)
            {
                db.reviews.Remove(cmt);
                db.SaveChanges();
            }
            return RedirectToAction("Index/" + idProduct, "Details");
        }
    }
}