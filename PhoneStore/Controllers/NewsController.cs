using PhoneStore.Models;
using System.Linq;
using System.Web.Mvc;

namespace PhoneStore.Controllers
{
    public class NewsController : Controller
    {
        PhoneStoreEntities db = new PhoneStoreEntities();
        // GET: News

        public ActionResult Index()
        {
            var newsList = db.news.OrderByDescending(n => n.created_at).ToList();
            return View(newsList);
        }

        public ActionResult Details(int id)
        {
            var news = db.news.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }
    }
}