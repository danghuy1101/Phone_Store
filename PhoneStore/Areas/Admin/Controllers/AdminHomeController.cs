using PhoneStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneStore.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        private PhoneStoreEntities db = new PhoneStoreEntities();
        // GET: Admin/AdminHome
        public ActionResult Index()
        {
            ViewBag.CountUser = db.users.Count();
            ViewBag.CountPhone = db.phones.Count();
            ViewBag.CountOrder = db.orders.Count();

            ViewBag.OrderList = db.orders.OrderByDescending(o => o.id).Take(10).ToList();

            var today = DateTime.Now.Date;
            ViewBag.OrderToday = db.orders.Count(o => o.created_at >= today);
            ViewBag.PendingOrder = db.orders.Count(o => o.status == "pending");
            ViewBag.CompletedOrder = db.orders.Count(o => o.status == "completed");
            ViewBag.CanceledOrder = db.orders.Count(o => o.status == "canceled");

            ViewBag.TotalRevenue = db.orders
                .Where(o => o.status == "completed")
                .Sum(o => (decimal?)o.total_price) ?? 0;

            return View();
        }

        [HttpPost]
        public JsonResult UpdateAvatar(HttpPostedFileBase avatar)
        {
            if (avatar != null && avatar.ContentLength > 0)
            {
                string path = Path.Combine(Server.MapPath("~/image/"), "admin.jpg");
                avatar.SaveAs(path);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Vui lòng chọn ảnh hợp lệ" });
        }
    }
}