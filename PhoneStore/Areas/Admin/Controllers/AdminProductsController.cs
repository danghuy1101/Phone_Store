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
    public class AdminProductsController : Controller
    {
        private PhoneStoreEntities db = new PhoneStoreEntities();
        // GET: Admin/AdminProducts
        public ActionResult Index(int? categoryId)
        {
            var phones = db.phones.Include(p => p.category);

            if (categoryId.HasValue)
            {
                phones = phones.Where(p => p.category_id == categoryId);
            }

            ViewBag.Categories = new SelectList(db.categories, "id", "name", categoryId);
            return View(phones.ToList());
        }

        // GET: Admin/AdminProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phone phone = db.phones.Find(id);
            if (phone == null)
            {
                return HttpNotFound();
            }
            return View(phone);
        }

        // GET: Admin/AdminProducts/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.categories, "id", "name");
            return View();
        }

        // POST: Admin/AdminProducts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(phone phone, HttpPostedFileBase Image1, HttpPostedFileBase Image2, HttpPostedFileBase Image3)
        {
            if (ModelState.IsValid)
            {
                if (Image1 != null)
                {
                    var fileName = Path.GetFileName(Image1.FileName);
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);
                    phone.image1 = fileName;
                    Image1.SaveAs(path);
                }

                if (Image2 != null)
                {
                    var fileName = Path.GetFileName(Image2.FileName);
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);
                    phone.image2 = fileName;
                    Image2.SaveAs(path);
                }

                if (Image3 != null)
                {
                    var fileName = Path.GetFileName(Image3.FileName);
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);
                    phone.image3 = fileName;
                    Image3.SaveAs(path);
                }

                db.phones.Add(phone);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.categories, "id", "name", phone.category_id);
            return View(phone);
        }

        // GET: Admin/AdminProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phone phone = db.phones.Find(id);
            if (phone == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.categories, "id", "name", phone.category_id);
            return View(phone);
        }

        // POST: Admin/AdminProducts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(phone phone, HttpPostedFileBase Image1, HttpPostedFileBase Image2, HttpPostedFileBase Image3)
        {
            if (ModelState.IsValid)
            {
                if (Image1 != null)
                {
                    var fileName = Path.GetFileName(Image1.FileName);
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);
                    phone.image1 = fileName;
                    Image1.SaveAs(path);
                }

                if (Image2 != null)
                {
                    var fileName = Path.GetFileName(Image2.FileName);
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);
                    phone.image2 = fileName;
                    Image2.SaveAs(path);
                }

                if (Image3 != null)
                {
                    var fileName = Path.GetFileName(Image3.FileName);
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);
                    phone.image3 = fileName;
                    Image3.SaveAs(path);
                }

                db.Entry(phone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.categories, "id", "name", phone.category_id);
            return View(phone);
        }

        // GET: Admin/AdminProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            phone phone = db.phones.Find(id);
            if (phone == null)
            {
                return HttpNotFound();
            }
            return View(phone);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            phone phone = db.phones.Find(id);
            db.phones.Remove(phone);
            db.SaveChanges();
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