using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Controllers.Abstract;
using UI.Helpers;
using UI.Models.ViewModels;

namespace UI.Controllers
{
    public class ContactController : WcControllerBase
    {
        // GET: Contact
		[HttpGet]
		public ActionResult Index()
		{
			return View(new ContactViewModel());
		}

		[HttpPost]
		public async Task<ActionResult> Index(ContactViewModel contact)
		{
			if (ModelState.IsValid)
			{
				await EmailHelper.SendMailAsnyc(contact);
				return View("Completed");
			}
			return View(contact);
		}
    }
}