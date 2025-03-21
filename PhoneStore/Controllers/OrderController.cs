using PhoneStore.Models;
using System.Linq;
using System.Web.Mvc;

namespace PhoneStore.Controllers
{
    public class OrderController : Controller
    {
        PhoneStoreEntities db = new PhoneStoreEntities();
        // GET: Order

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetOrder(int id)
        {
            var orderList = db.orders
                              .Where(o => o.user_id == id)
                              .OrderByDescending(o => o.id)
                              .ToList();

            ViewBag.UserId = id;
            return View(orderList);
        }

        public ActionResult OrderDetail(int id)
        {
            var order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            var listProdOrder = db.order_details.Where(p => p.order_id == id).ToList();

            ViewBag.FinalPrice = listProdOrder.Sum(item => item.final_price);
            ViewBag.Address = order.address;
            ViewBag.Date = order.created_at;
            ViewBag.Id = order.id;
            ViewBag.Status = order.status;
            ViewBag.Customer = order;

            return View(listProdOrder);
        }

        public ActionResult CancelOrder(int id)
        {
            var order = db.orders.Find(id);
            if (order == null || order.status == "canceled")
            {
                return HttpNotFound();
            }

            order.status = "canceled";
            db.SaveChanges();

            return RedirectToAction("GetOrder", new { id = order.user_id });
        }
    }
}