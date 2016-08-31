using System;
using System.Globalization;
using System.Web;
using System.Web.Configuration;
using UI.Models.Misc;
using WebMatrix.WebData;

namespace UI.Helpers
{
	public static class GreetingsHelper
	{
		public static string GetGreetings()
		{
			return SetupGreetings(WebSecurity.CurrentUserName);
			//CookieHelper.SetCookie(cookieName, greetings, DateTime.Now.AddMinutes(20));
			//HttpContext.Current.Session["Greetings"] = greetings;
		}

		private static string SetupGreetings(string userName)
		{
			DateTime timeUtc = DateTime.UtcNow;
			string timezoneId = WebConfigurationManager.AppSettings.Get("TimezoneId");
			TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
			DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
			string strHour = cstTime.ToString("HH", CultureInfo.CurrentCulture);
			int hour = int.Parse(strHour);

			string greetings = null;
			string culture = CookieHelper.GetCookieValue("_culture");
			switch (culture)
			{
				case null: // => set default value
				case "en-us": // => set default value
				case "zh-hans":
				case "zh-hant":
					if (hour <= 12)
						greetings = SiteConfiguration.GoodMorning;
					else if (hour > 12 && hour <= 17)
						greetings = SiteConfiguration.GoodAfternoon;
					else if (hour > 17) greetings = SiteConfiguration.GoodEvening;
					break;
				case "ja-jp":
					if (hour <= 12)
						greetings = SiteConfiguration.GoodMorningJap;
					else if (hour > 12 && hour <= 17)
						greetings = SiteConfiguration.GoodAfternoonJap;
					else if (hour > 17) greetings = SiteConfiguration.GoodEveningJap;
					break;
			}
			return $"{userName} {greetings}";
		}


		/// <summary>
		/// Convert the passed datetime into client timezone.
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static string ToClientTime(this DateTime dt)
		{
			var timeOffSet = HttpContext.Current.Session["timezoneoffset"];  // read the value from session

			if (timeOffSet != null)
			{
				var offset = int.Parse(timeOffSet.ToString());
				dt = dt.AddMinutes(-1 * offset);

				return dt.ToString(CultureInfo.CurrentCulture);
			}

			// if there is no offset in session return the datetime in server timezone
			return dt.ToLocalTime().ToString(CultureInfo.CurrentCulture);
		}
	}


}