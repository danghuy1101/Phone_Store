using PhoneStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PhoneStore.Areas.Admin.Controllers
{
    public class CustomersAdminController : Controller
    {
        private PhoneStoreEntities db = new PhoneStoreEntities();
        // GET: Admin/CustomersAdmin

        public ActionResult Index()
        {
            var customers = db.users.Where(u => u.role == "customer").ToList();
            return View(customers);
        }

        // GET: Admin/CustomersAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = db.users.Find(id);
            if (customer == null || customer.role != "customer")
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Admin/CustomersAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/CustomersAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "name,email,password,phone_number,avatar_image")] user customer)
        {
            if (ModelState.IsValid)
            {
                customer.role = "customer";
                customer.created_at = DateTime.Now;
                db.users.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Admin/CustomersAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = db.users.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "admin", Text = "Admin" },
                new SelectListItem { Value = "customer", Text = "Customer" }
            };

            return View(customer);
        }

        // POST: Admin/CustomersAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string role, HttpPostedFileBase avatarFile)
        {
            var customer = db.users.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            if (role != "admin" && role != "customer")
            {
                ModelState.AddModelError("role", "Vai trò không hợp lệ.");
            }

            if (ModelState.IsValid)
            {
                if (avatarFile != null)
                {
                    string fileName = Path.GetFileName(avatarFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/image"), fileName);
                    customer.avatar_image = fileName;
                    avatarFile.SaveAs(path);
                }

                customer.role = role;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "admin", Text = "Admin" },
                new SelectListItem { Value = "customer", Text = "Customer" }
            };

            return View(customer);
        }


        // GET: Admin/CustomersAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = db.users.Find(id);
            if (customer == null || customer.role != "customer")
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Admin/CustomersAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var customer = db.users.Find(id);
            if (customer != null && customer.role == "customer")
            {
                db.users.Remove(customer);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}