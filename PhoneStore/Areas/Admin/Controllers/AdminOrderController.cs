using PhoneStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneStore.Areas.Admin.Controllers
{
    public class AdminOrderController : Controller
    {
        private PhoneStoreEntities db = new PhoneStoreEntities();
        // GET: Admin/AdminOrder

        public ActionResult Index(string statusFilter)
        {
            var orderList = db.orders.AsQueryable();

            if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "all")
            {
                orderList = orderList.Where(o => o.status == statusFilter);
            }

            ViewBag.StatusFilter = statusFilter;
            return View(orderList.OrderByDescending(o => o.id).ToList());
        }

        public ActionResult Details(int id)
        {
            var listProdOrder = db.order_details.Where(od => od.order_id == id).ToList();
            decimal finalPrice = listProdOrder.Sum(item => item.final_price);

            var order = db.orders.FirstOrDefault(o => o.id == id);
            if (order == null)
            {
                return HttpNotFound();
            }

            ViewBag.FinalPrice = finalPrice;
            ViewBag.Address = order.address;
            ViewBag.Date = order.created_at;
            ViewBag.Id = order.id;
            ViewBag.Status = order.status;
            ViewBag.CusInfor = order;

            return View(listProdOrder);
        }

        public ActionResult Confirm(int id)
        {
            var prodListOrder = db.order_details.Where(od => od.order_id == id).ToList();

            foreach (var item in prodListOrder)
            {
                var phone = db.phones.FirstOrDefault(p => p.id == item.phone_id);
                if (phone != null)
                {
                    phone.stock -= item.quantity;
                }
            }

            var order = db.orders.FirstOrDefault(o => o.id == id);
            if (order != null)
            {
                order.status = "completed";
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}