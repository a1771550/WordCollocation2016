using System;
using System.Globalization;
using System.Web.Configuration;

namespace UI.Demo
{
	public partial class TimezoneDemo : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				//ReadOnlyCollection<TimeZoneInfo> zones = TimeZoneInfo.GetSystemTimeZones();
				//lblResult.Text = string.Format("The local system has the following {0} time zones", zones.Count);
				//foreach (TimeZoneInfo zone in zones)
				//	lblResult.Text += zone.Id + "<br>";

				DateTime timeUtc = DateTime.UtcNow;
				try
				{
					//TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
					//DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
					//lblResult.Text += string.Format("The date and time are {0} {1}.",
					//				  cstTime,
					//				  cstZone.IsDaylightSavingTime(cstTime) ?
					//						  cstZone.DaylightName : cstZone.StandardName);

					string timezoneId = WebConfigurationManager.AppSettings.Get("TimezoneId");
					TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
					DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);

					//DateTime client = DateTime.Parse(cstTime.ToClientTime());
					string strHour = cstTime.ToString("HH", CultureInfo.CurrentCulture);
					int hour = int.Parse(strHour);

					lblResult.Text += "client time: " + cstTime;
					lblResult.Text += "<br>strHour: " + strHour;
					lblResult.Text += "<br>hour: " + hour;
				}
				catch (TimeZoneNotFoundException)
				{
					lblResult.Text += ("The registry does not define the Central Standard Time zone.");
				}
				catch (InvalidTimeZoneException)
				{
					lblResult.Text += ("Registry data on the Central Standard Time zone has been corrupted.");
				}
			}
		}
	}
}