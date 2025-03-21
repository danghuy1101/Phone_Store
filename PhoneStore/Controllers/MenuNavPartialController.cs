using PhoneStore.Models;
using System.Linq;
using System.Web.Mvc;

namespace PhoneStore.Controllers
{
    public class MenuNavPartialController : Controller
    {
        PhoneStoreEntities db = new PhoneStoreEntities();
        // GET: MenuNavPartial
        public ActionResult MenuNav()
        {
            ViewBag.CategoryNavList = db.categories.ToList();
            return PartialView();
        }
    }
}