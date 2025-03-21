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
    public class AdminCategoriesController : Controller
    {
        private PhoneStoreEntities db = new PhoneStoreEntities();

        // GET: Admin/AdminCategories
        public ActionResult Index()
        {
            return View(db.categories.ToList());
        }

        // GET: Admin/AdminCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Admin/AdminCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(category category, HttpPostedFileBase ImgCate)
        {
            if (ModelState.IsValid)
            {
                if (ImgCate != null)
                {
                    var fileName = Path.GetFileName(ImgCate.FileName);
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);
                    category.category_image = fileName;
                    ImgCate.SaveAs(path);
                }
                db.categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/AdminCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/AdminCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(category category, HttpPostedFileBase ImgCate)
        {
            if (ModelState.IsValid)
            {
                if (ImgCate != null)
                {
                    var fileName = Path.GetFileName(ImgCate.FileName);
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);
                    category.category_image = fileName;
                    ImgCate.SaveAs(path);
                }
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Admin/AdminCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = db.categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Admin/AdminCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            category category = db.categories.Find(id);
            db.categories.Remove(category);
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