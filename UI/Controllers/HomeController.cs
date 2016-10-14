using System;
using System.Web;
using System.Web.Mvc;
using MyWcModel;
using CommonLib.Helpers;
using UI.Controllers.Abstract;
using UI.Models.Misc;
using UI.Models.ViewModels;

namespace UI.Controllers
{
	public class HomeController : WcControllerBase
	{
		public const string CollocationListSessionName = "CollocationList";
		//private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		//
		// GET: /Home/
		public ActionResult Index()
		{
			SearchViewModel model = new SearchViewModel(ViewMode.Home);
			return View(model);
		}

		/// <summary>
		/// for reference from setculturedropdown partial page
		/// </summary>
		/// <param name="culture"></param>
		/// <param name="returnUrl"></param>
		public void SetCulture(string culture, string returnUrl)
		{
			culture = CultureHelper.GetImplementedCulture(culture);
			HttpCookie cookie = Request.Cookies["_culture"];
			if (cookie != null) cookie.Value = culture;
			else
			{
				cookie = new HttpCookie("_culture");
				cookie.Value = culture;
				cookie.Expires = DateTime.Now.AddYears(1);
			}
			Response.Cookies.Add(cookie);

			//if (Request.IsAuthenticated)
			//{
			//	GreetingsHelper.SetGreetings(WebSecurity.CurrentUserName, "Greetings");
			//}

			Response.Redirect(returnUrl);
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult Search(string word, string colposid)
		{
			if (word != null && colposid != null)
			{
				//string word = model.Word;
				short colPosId = Convert.ToInt16(colposid);
				var repo = new CollocationRepository();
				var collocationList = repo.GetCollocationListByWordColPosId(word, colPosId);
				if (collocationList.Count > 0)
				{
					Session[CollocationListSessionName] = collocationList;
					int pageSize = SiteConfiguration.WcViewPageSize;
					int pageCount;
					int listSize = collocationList.Count;
					if (listSize > 10 && pageSize >= 1)
					{
						pageCount = (int)Math.Ceiling((double)(listSize / pageSize));
					}
					else pageCount = 1;
					return RedirectToAction("SearchResult", pageCount);
				}
				SearchViewModel model = new SearchViewModel();
				model.Word = word;
				model.ColPosId = colposid;
				Session["SearchViewModel"] = model;
			}
			return null;
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ViewResult SearchResult(int page = 1)
		{
			SearchViewModel model = new SearchViewModel(ViewMode.SearchResult, page);
			return View("SearchResult", model);
			//return null;
		}

		public ActionResult NoSearchResult()
		{
			SearchViewModel model = (SearchViewModel)Session["SearchViewModel"];
			return View("NoSearchResult", model);
		}

		public ViewResult UnderConstruction()
		{
			return View();
		}

		/// <summary>
		/// for debug only
		/// </summary>
		/// <returns></returns>
		public string Headers()
		{
			string host = System.Web.HttpContext.Current.Request.Headers["HOST"];
			return host;
		}
	}
}