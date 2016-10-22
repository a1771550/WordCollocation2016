using System.Web.Mvc;

namespace SimpleMembershipDemo.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
		[Authorize(Roles="Admin")]
        public ActionResult Index()
        {
            return View();
        }
    }
}