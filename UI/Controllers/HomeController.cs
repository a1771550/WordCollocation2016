using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.WebSockets;
using CommonLib.Helpers;
using UI.Classes;
using UI.Controllers.Abstract;
using UI.Helpers;
using UI.Models.Misc;
using UI.Models.ViewModels;
using UI.Models.WcRepo;
using UI.Models.WC;

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
		public ViewResult SearchResult(int page = 1)
		{
			SearchViewModel model = new SearchViewModel(ViewMode.SearchResult, page);
			return View("SearchResult", model);
			//return null;
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ViewResult SearchResult_bak()
		{
			//SearchViewModel tempModel = (SearchViewModel) TempData["SearchViewModel"];
			SearchViewModel model = new SearchViewModel();
			//model.Pos = tempModel.Pos;
			//model.PosTrans = tempModel.PosTrans;
			//model.Word = tempModel.Word;
			//model.WordTrans = tempModel.WordTrans;
			//model.ColPos = tempModel.ColPos;
			//model.ColPosTran = tempModel.ColPosTran;
			//model.Pattern = tempModel.Pattern;
			model.Pos = Request.Cookies.Get("pos")?.Value;
			model.PosZht = Request.Cookies.Get("posZht")?.Value;
			model.PosZht = HttpUtility.UrlDecode(model.PosZht);
			model.PosZhs = Request.Cookies.Get("posZhs")?.Value;
			model.PosZhs = HttpUtility.UrlDecode(model.PosZhs);
			model.PosJap = Request.Cookies.Get("posJap")?.Value;
			model.PosJap = HttpUtility.UrlDecode(model.PosJap);
			model.Word = Request.Cookies.Get("word")?.Value;
			model.WordZht = Request.Cookies.Get("wordZht")?.Value;
			model.WordZht = HttpUtility.UrlDecode(model.WordZht);
			model.WordZhs = Request.Cookies.Get("wordZhs")?.Value;
			model.WordZhs = HttpUtility.UrlDecode(model.WordZhs);
			model.WordJap = Request.Cookies.Get("wordJap")?.Value;
			model.WordJap = HttpUtility.UrlDecode(model.WordJap);
			model.ColPos = Request.Cookies.Get("colpos")?.Value;
			model.ColPosZht = Request.Cookies.Get("colposZht")?.Value;
			model.ColPosZht = HttpUtility.UrlDecode(model.ColPosZht);
			model.ColPosZhs = Request.Cookies.Get("colposZhs")?.Value;
			model.ColPosZhs = HttpUtility.UrlDecode(model.ColPosZhs);
			model.ColPosJap = Request.Cookies.Get("colposJap")?.Value;
			model.ColPosJap = HttpUtility.UrlDecode(model.ColPosJap);
			var httpCookie = Request.Cookies.Get("colpattern");
			if (httpCookie != null)
			{
				CollocationPattern pattern = (CollocationPattern)(int.Parse(httpCookie.Value));
				model.Pattern = WcHelper.GetPatternArray(pattern);
			}
			var cookie = Request.Cookies.Get("colcount");
			if (cookie != null)
				model.CollocationCount = int.Parse(cookie.Value);
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

		[HttpGet]
		public JsonpResult SearchCollocation(string word, string id)
		{
			//if (word != null && id != null)
			//{
			//	short wcpId = Convert.ToInt16(id);
			//	var colrepo = new CollocationRepository();
			//	var collocationList = colrepo.GetCollocationListByWordColPosId(word, wcpId);

			//	if (collocationList.Count > 0)
			//	{
			//		var data = from item in collocationList
			//				   select new { item.colword.Entry, item.colword.EntryZht };
			//		//JsonpResult result = new JsonpResult(data.FirstOrDefault());
			//		JsonpResult result = new JsonpResult(data);
			//		return result;
			//	}

			//}
			return null;
		}

		public ActionResult CallJsonpDemo()
		{
			return View();
		}

		public void JsonDemo()
		{
			StringBuilder sb = new StringBuilder();
			//byte[] buf = new byte[8192];
			var url = "http://www.translationhall.com/api/web/pos";
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Stream resstream = response.GetResponseStream();
			MemoryStream ms = new MemoryStream();
			
			if (resstream != null)
			{
				resstream.CopyTo(ms);
				byte[] buf = new byte[ms.Length];
				int count;
				ms.Position = 0;
				do
				{
					count = ms.Read(buf, 0, buf.Length);
					if (count != 0)
					{
						string tempString = Encoding.UTF8.GetString(buf, 0, count);
						sb.Append(tempString);
					}
				} while (count > 0);
			}

			//Response.Write(sb + "<br/><br/>");
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			List<PosRepository.Pos> posList = serializer.Deserialize<List<PosRepository.Pos>>(sb.ToString());
			foreach (PosRepository.Pos p in posList)
			{
				Response.Write("Id: " + p.Id + "&" + "Entry: " + p.Entry + "&EntryZht: " + p.EntryZht + "&EntryZhs: " + p.EntryZhs + "&EntryJap: " + p.EntryJap + "<br>");
			}
		}

	}

	//public struct Pos
	//{
	//	public short Id;
	//	public string Entry;
	//	public string EntryZht;
	//	public string EntryZhs;
	//	public string EntryJap;
	//}
}