using PhoneStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneStore.Areas.Admin.Controllers
{
    public class AdminNewsController : Controller
    {
        private PhoneStoreEntities db = new PhoneStoreEntities();

        public ActionResult Index()
        {
            var newsList = db.news.OrderByDescending(n => n.created_at).ToList();
            return View(newsList);
        }

        public ActionResult Details(int id)
        {
            var news = db.news.Find(id);
            if (news == null)
                return HttpNotFound();
            return View(news);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(news news, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    string fileExt = Path.GetExtension(imageFile.FileName).ToLower();
                    string fileName = Guid.NewGuid().ToString() + fileExt;
                    string path = Path.Combine(Server.MapPath("~/image"), fileName);

                    news.image_title = fileName;
                    imageFile.SaveAs(path);
                }

                news.created_at = DateTime.Now;
                db.news.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        public ActionResult Edit(int id)
        {
            var news = db.news.Find(id);
            if (news == null)
                return HttpNotFound();
            return View(news);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(news news, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                var existingNews = db.news.Find(news.id);
                if (existingNews == null)
                    return HttpNotFound();

                existingNews.title = news.title;
                existingNews.content = news.content;

                if (imageFile != null)
                {
                    string fileExt = Path.GetExtension(imageFile.FileName).ToLower();
                    string fileName = Guid.NewGuid().ToString() + fileExt;
                    string path = Path.Combine(Server.MapPath("~/image"), fileName);

                    existingNews.image_title = fileName;
                    imageFile.SaveAs(path);
                }

                db.Entry(existingNews).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        public ActionResult Delete(int id)
        {
            var news = db.news.Find(id);
            if (news == null)
                return HttpNotFound();
            return View(news);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            var news = db.news.Find(id);
            if (news != null)
            {
                db.news.Remove(news);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase upload)
        {
            if (upload == null || upload.ContentLength <= 0)
            {
                return Json(new { uploaded = 0, error = new { message = "Không có tệp nào được tải lên" } });
            }

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
            string fileExt = Path.GetExtension(upload.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExt))
            {
                return Json(new { uploaded = 0, error = new { message = "Chỉ chấp nhận tệp .jpg, .jpeg, .png, .gif" } });
            }

            string path = Server.MapPath("~/image/");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string fileName = Guid.NewGuid().ToString() + fileExt;
            string filePath = Path.Combine(path, fileName);

            upload.SaveAs(filePath);

            return Json(new
            {
                uploaded = 1,
                fileName = fileName,
                url = Url.Content("~/image/" + fileName)
            });
        }
    }
}