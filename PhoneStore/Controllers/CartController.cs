using PhoneStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PhoneStore.Controllers
{
    public class CartController : Controller
    {
        PhoneStoreEntities db = new PhoneStoreEntities();
        // GET: Cart
        
        private List<CartItem> GetCart()
        {
            List<CartItem> myCart = Session["MyCart"] as List<CartItem>;
            if (myCart == null)
            {
                myCart = new List<CartItem>();
                Session["MyCart"] = myCart;
            }
            return myCart;
        }

        public ActionResult AddToCart(int phoneID, int quantity)
        {
            List<CartItem> myCart = GetCart();
            CartItem currentProduct = myCart.FirstOrDefault(p => p.PhoneID == phoneID);
            if (currentProduct == null)
            {
                currentProduct = new CartItem(phoneID);
                currentProduct.Quantity = quantity;
                myCart.Add(currentProduct);
            }
            else
            {
                currentProduct.Quantity += quantity;
            }

            return RedirectToAction("GetCartInfo", "Cart");
        }

        public ActionResult RemoveProduct(int phoneID)
        {
            List<CartItem> myCart = GetCart();
            CartItem currentProduct = myCart.FirstOrDefault(p => p.PhoneID == phoneID);
            if (currentProduct != null)
            {
                myCart.Remove(currentProduct);
            }

            return RedirectToAction("GetCartInfo", "Cart");
        }

        private int GetTotalQuantity()
        {
            return GetCart().Sum(sp => sp.Quantity);
        }

        private decimal GetTotalPrice()
        {
            return GetCart().Sum(sp => sp.FinalPrice());
        }

        public ActionResult GetCartInfo()
        {
            List<CartItem> myCart = GetCart();
            if (!myCart.Any())
            {
                return RedirectToAction("EmptyCart", "Cart");
            }

            ViewBag.TotalQuantity = GetTotalQuantity();
            ViewBag.TotalPrice = GetTotalPrice();
            return View(myCart);
        }

        public ActionResult UpdateQuantity(int phoneID, int quantity)
        {
            List<CartItem> myCart = GetCart();
            CartItem currentProduct = myCart.FirstOrDefault(p => p.PhoneID == phoneID);
            if (currentProduct != null)
            {
                currentProduct.Quantity = quantity;
            }

            return RedirectToAction("GetCartInfo", "Cart");
        }

        public ActionResult CartPartial()
        {
            ViewBag.TotalNumber = GetTotalQuantity();

            return PartialView();
        }

        public ActionResult EmptyCart()
        {
            ViewBag.EmptyNotification = "Chưa có sản phẩm nào trong giỏ hàng.";
            return View();
        }

        public ActionResult Payment()
        {
            List<CartItem> myCart = GetCart();
            ViewBag.TotalPrice = GetTotalPrice();
            return View(myCart);
        }

        public ActionResult Order(string address)
        {
            if (!(Session["Account"] is user cus))
            {
                return RedirectToAction("Login", "Users");
            }

            List<CartItem> myCart = GetCart();

            var order = new order
            {
                user_id = cus.id,
                total_price = GetTotalPrice(),
                address = address,
                phone_number = cus.phone_number,
                created_at = DateTime.Now,
                status = "pending"
            };

            db.orders.Add(order);
            db.SaveChanges();

            foreach (var item in myCart)
            {
                var orderDetail = new order_details
                {
                    order_id = order.id,
                    phone_id = item.PhoneID,
                    quantity = item.Quantity,
                    unit_price = item.Price,
                    final_price = item.FinalPrice()
                };
                db.order_details.Add(orderDetail);
            }

            db.SaveChanges();
            Session["MyCart"] = null;

            return RedirectToAction("GetOrder", "Order", new { id = cus.id });
        }
    }
}