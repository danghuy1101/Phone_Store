using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneStore.Models
{
	public class CartItem
	{
        private readonly PhoneStoreEntities db = new PhoneStoreEntities();

        public int PhoneID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public decimal FinalPrice()
        {
            return Quantity * Price;
        }

        public CartItem(int phoneID)
        {
            this.PhoneID = phoneID;
            var phoneDB = db.phones.SingleOrDefault(p => p.id == this.PhoneID);
            if (phoneDB != null)
            {
                this.Name = phoneDB.name;
                this.Image = phoneDB.image1;
                this.Price = phoneDB.price;
                this.Quantity = 1;
            }
            else
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }
        }
    }
}