using System.Web.Mvc;
using UI.Models.ViewModels;

namespace UI.Controllers
{
    public class ValidateDemoController : Controller
    {
        // GET: ValidateDemo
        public ActionResult Index()
        {
			ValidateDemoViewModel model=new ValidateDemoViewModel();
            return View(model);
        }

	    [HttpPost]
	    [ValidateAntiForgeryToken]
	    public ActionResult Index(ValidateDemoViewModel model)
	    {
		    if (ModelState.IsValid)
		    {
			    return RedirectToAction("Index", "Home");
		    }
		    return View();
	    }
    }
}