using System.Web.Mvc;
using UI.Controllers.Abstract;

namespace UI.Controllers
{
    public class AdminController : WcControllerBase
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}