using PhoneStore.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PhoneStore.Controllers
{
    public class UsersController : Controller
    {
        private readonly PhoneStoreEntities db = new PhoneStoreEntities();
        // GET: Users

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ThongBao = "*Vui lòng nhập đầy đủ thông tin đăng nhập.";
                return View();
            }

            var user = db.users.FirstOrDefault(u => u.email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.password))
            {
                ViewBag.ThongBao = "*Email hoặc mật khẩu không đúng";
                return View();
            }

            Session["Account"] = user;
            return RedirectToAction("Index", user.role == "admin" ? "AdminHome" : "Home", new { area = user.role == "admin" ? "Admin" : "" });
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Users");
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            user customer = db.users.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Detail(user customer, HttpPostedFileBase ImageUser)
        {
            if (!ModelState.IsValid)
                return View(customer);

            var existingUser = db.users.Find(customer.id);
            if (existingUser == null)
                return HttpNotFound();

            if (ImageUser != null && ImageUser.ContentLength > 0)
            {
                string fileName = System.IO.Path.GetFileName(ImageUser.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/image"), fileName);
                ImageUser.SaveAs(path);
                existingUser.avatar_image = fileName;
            }

            existingUser.name = customer.name;
            existingUser.phone_number = customer.phone_number;

            db.Entry(existingUser).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Detail", new { id = customer.id });
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(string userEmail, string userPassword, string rePassword, string phoneNumber, string userName)
        {
            if (string.IsNullOrWhiteSpace(userEmail) || string.IsNullOrWhiteSpace(userPassword) || string.IsNullOrWhiteSpace(rePassword) || string.IsNullOrWhiteSpace(userName))
            {
                ViewBag.Notification = "Vui lòng điền đầy đủ thông tin!";
                return View();
            }

            if (!IsValidEmail(userEmail))
            {
                ViewBag.NotificationEmail = "Email không hợp lệ!";
                return View();
            }

            if (userPassword.Length < 6)
            {
                ViewBag.NotificationPassword = "Mật khẩu phải có ít nhất 6 ký tự!";
                return View();
            }

            if (userPassword != rePassword)
            {
                ViewBag.Notification = "Mật khẩu xác nhận không chính xác!";
                return View();
            }

            if (!IsValidPhoneNumber(phoneNumber))
            {
                ViewBag.NotificationPhone = "Số điện thoại không hợp lệ!";
                return View();
            }

            if (db.users.Any(k => k.email == userEmail))
            {
                ViewBag.NotificationEmail = "Email đã được đăng ký!";
                return View();
            }

            var newUser = new user
            {
                name = userName,
                email = userEmail,
                password = HashPassword(userPassword),
                role = "customer",
                phone_number = phoneNumber,
                avatar_image = "user.png",
                created_at = DateTime.Now
            };

            db.users.Add(newUser);
            db.SaveChanges();
            return RedirectToAction("Login");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                return new System.Net.Mail.MailAddress(email).Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Length == 10 && phoneNumber.All(char.IsDigit);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}