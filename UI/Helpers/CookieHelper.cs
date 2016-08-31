using System;
using System.Web;

namespace UI.Helpers
{
	public static class CookieHelper
	{
		public static void SetCookie(string cookieName, string value, DateTime expire)
		{
			HttpCookie cookie = new HttpCookie(cookieName);
			cookie.Value = HttpUtility.UrlEncode(value);
			cookie.Expires = expire;
			HttpContext.Current.Response.Cookies.Add(cookie);
		}

		public static string GetCookieValue(string cookieName)
		{
			HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
			return cookie != null ? HttpUtility.UrlDecode(cookie.Value) : null;
		}

		public static bool IsCookieExist(string cookieName)
		{
			return HttpContext.Current.Request.Cookies[cookieName] == null;
		}

		public static void DeleteCookie(string cookieName)
		{
			var httpCookie = HttpContext.Current.Response.Cookies[cookieName];
			if (httpCookie != null)
				httpCookie.Expires = DateTime.Now.AddDays(-1);
		}
	}
}