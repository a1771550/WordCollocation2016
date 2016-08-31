using System;
using System.Configuration;
using System.Web.UI.WebControls;

namespace UI.Models.Misc
{
	public static class SiteConfiguration
	{
		public static int MinPasswordLength { get
		{
			return Convert.ToInt32(ConfigurationManager.AppSettings["MinPasswordLength"]);
		} }
		public static int MaxPasswordLength
		{
			get
			{
				return Convert.ToInt32(ConfigurationManager.AppSettings["MaxPasswordLength"]);
			}
		}
		
		public static string CaptchaFont { get { return ConfigurationManager.AppSettings["CaptchaFont"]; } }
		public static string CaptchaHeight { get { return ConfigurationManager.AppSettings["CaptchaHeight"]; } }
		public static string CaptchaWidth { get { return ConfigurationManager.AppSettings["CaptchaWidth"]; } }
		public static string MailSenderName { get { return ConfigurationManager.AppSettings["MailSenderName"]; } }
		public static string Comments { get { return ConfigurationManager.AppSettings["Comments"]; } }
		public static string CommentsC { get { return ConfigurationManager.AppSettings["CommentsC"]; } }
		//public static string ReplyComments { get { return replyComments; } }
		//public static string ReplyCommentsC { get { return replyCommentsC; } }
		//public static string MemberZone { get { return memberZone; } }
		//public static string MemberZoneC { get { return memberZoneC; } }
		//public static string WordCollocationsC { get { return wordCollocationsC; } }
		//public static string WordCollocations { get { return wordCollocations; } }
		//public static string ContextualPhrasesC { get { return contextualPhrasesC; } }
		//public static string ContextualPhrases { get { return contextualPhrases; } }
		public static string HostRootPath { get { return ConfigurationManager.AppSettings["HostRootPath"]; } }
		public static string MailID { get { return ConfigurationManager.AppSettings["MailID"]; } }
		public static string MailServer { get { return ConfigurationManager.AppSettings["MailServer"]; } }
		public static string MailSender { get { return ConfigurationManager.AppSettings["MailSender"]; } }
		public static string MailReceiver { get { return ConfigurationManager.AppSettings["MailReceiver"]; } }
		public static string MailPassword { get { return ConfigurationManager.AppSettings["MailPassword"]; } }
		public static string ErrorLogEmail { get { return ConfigurationManager.AppSettings["ErrorLogEmail"]; } }
		public static bool EnableErrorLogEmail { get { return bool.Parse(ConfigurationManager.AppSettings["EnableErrorLogEmail"]); } }
		public static string ErrorMailSubject { get { return ConfigurationManager.AppSettings["ErrorMailSubject"]; } }
		public static int MailPort { get { return int.Parse(ConfigurationManager.AppSettings["MailPort"]); } }
		public static bool IsMailHtml { get { return bool.Parse(ConfigurationManager.AppSettings.Get("IsMailHtml")); } }
		//NeedCredentials
		public static bool NeedCredentials { get { return bool.Parse(ConfigurationManager.AppSettings.Get("NeedCredentials")); } }
		//UseDefaultCredentials
		public static bool UseDefaultCredentials { get { return bool.Parse(ConfigurationManager.AppSettings.Get("UseDefaultCredentials")); } }

		public static string DateTimeFormat { get { return ConfigurationManager.AppSettings.Get("DateTimeFormat"); } }
		public static string DateTimeFormatString { get { return ConfigurationManager.AppSettings.Get("DateTimeFormatString"); } }

		public static Unit AvatarWidth { get { return Unit.Parse(ConfigurationManager.AppSettings.Get("AvatarWidth")); } }
		public static Unit AvatarHeight { get { return Unit.Parse(ConfigurationManager.AppSettings.Get("AvatarHeight")); } }
		public static string AvatarDefaultImage { get { return ConfigurationManager.AppSettings.Get("AvatarDefaultImage"); } }
		public static string WebUrlRegEx { get { return ConfigurationManager.AppSettings.Get("WebUrlRegEx"); } }

		public static int GravatarSize { get { return Convert.ToInt32(ConfigurationManager.AppSettings.Get("GravatarSize")); } }
		//public static GravatarRating GravatarRating
		//{
		//	get
		//	{
		//		string rating = ConfigurationManager.AppSettings.Get("GravatarRating");
		//		GravatarRating grating = GravatarRating.Default;
		//		switch(rating)
		//		{
		//			case "G":
		//				grating = GravatarRating.G;
		//				break;
		//			case "R":
		//				grating = GravatarRating.R;
		//				break;
		//			case "X":
		//				grating = GravatarRating.X;
		//				break;
		//			case "PG":
		//				grating = GravatarRating.PG;
		//				break;
		//			case "Default":
		//				grating = GravatarRating.Default;
		//				break;
		//		}
		//		return grating;
		//	}
		//}
		//public static GravatarDefaultImageBehavior GravatarDefaultImageBehavior
		//{
		//	get 
		//	{ 
		//		string behavior = ConfigurationManager.AppSettings.Get("GravatarDefaultBehavior");
		//		GravatarDefaultImageBehavior gbehavior = GravatarDefaultImageBehavior.Default;
		//		switch(behavior)
		//		{
		//			case "Retro":
		//				gbehavior = GravatarDefaultImageBehavior.Retro;
		//				break;
		//			case "MonsterId":
		//				gbehavior= GravatarDefaultImageBehavior.MonsterId;
		//				break;
		//			case "Identicon":gbehavior = GravatarDefaultImageBehavior.Identicon;
		//				break;
		//			case "MysteryMan":gbehavior = GravatarDefaultImageBehavior.MysteryMan;
		//				break;
		//			case "Default":
		//				gbehavior = GravatarDefaultImageBehavior.Default;
		//				break;
		//			case "Wavatar":
		//				gbehavior = GravatarDefaultImageBehavior.Wavatar;
		//				break;
		//		}
		//		return gbehavior;
		//	}
		//}
		public static string GravatarDefaultImage { get { return ConfigurationManager.AppSettings.Get("GravatarDefaultImage"); } }
		public static string GravatarUrl { get { return ConfigurationManager.AppSettings.Get("GravatarUrl"); } }
		public static string DomainName { get { return ConfigurationManager.AppSettings.Get("DomainName"); } }
		public static string ExampleSources { get { return ConfigurationManager.AppSettings["ExampleSources"]; } }
		public static string LogsPath { get { return ConfigurationManager.AppSettings["LogsPath"]; } }
		public static double CacheExpiration_Minutes { get { return double.Parse(ConfigurationManager.AppSettings.Get("CacheExpiration_Minutes")); } }
		public static string IrregularVerbsXMLFile { get { return ConfigurationManager.AppSettings.Get("IrregularVerbsXMLFile"); } }
		public static string RegularVerbsXMLFile { get { return ConfigurationManager.AppSettings.Get("RegularVerbsXMLFile"); } }
		public static string RegularVerbsXMLFile1 { get { return ConfigurationManager.AppSettings.Get("RegularVerbsXMLFile1"); } }
		
		public static string SecurityKey { get { return ConfigurationManager.AppSettings.Get("SecurityKey"); } }
		public static string KeyPath { get { return ConfigurationManager.AppSettings.Get("KeyPath"); } }
		public static string KeyFileName { get { return ConfigurationManager.AppSettings.Get("KeyFileName"); } }
		public static string OxfordDictUrl { get { return ConfigurationManager.AppSettings.Get("OxfordDictUrl"); } }
		public static string ChDictUrl { get { return ConfigurationManager.AppSettings.Get("ChDictUrl"); } }
		public static string TitleSeperator { get { return ConfigurationManager.AppSettings.Get("TitleSeperator"); } }
		public static bool EnableOAuth { get { return bool.Parse(ConfigurationManager.AppSettings.Get("EnableOAuth")); } }

		public static string ContactMailSubject { get
		{
			return ConfigurationManager.AppSettings.Get("ContactMailSubject");
		} }

		public static int WcColListPageSize { get { return Convert.ToInt32(ConfigurationManager.AppSettings.Get("WcColListPageSize")); } }

		public static int WcViewPageSize { get { return Convert.ToInt32(ConfigurationManager.AppSettings.Get("WcViewPageSize")); } }

		public static string DictionaryLinkZht { get
		{
			return ConfigurationManager.AppSettings.Get("DictionaryLinkZht");
		} }
		public static string DictionaryLinkZhs { get
		{
			return ConfigurationManager.AppSettings.Get("DictionaryLinkZhs");
		} }
		public static string DictionaryLinkJap
		{
			get
			{
				return ConfigurationManager.AppSettings.Get("DictionaryLinkJap");
			}
		}

		public static string SiteUrl { get { return ConfigurationManager.AppSettings.Get("SiteUrl"); } }

		public static string EmailPattern { get { return ConfigurationManager.AppSettings.Get("EmailPattern"); } }

		public static string GoodMorning { get { return ConfigurationManager.AppSettings.Get("GoodMorning"); } }
		public static string GoodMorningJap { get { return ConfigurationManager.AppSettings.Get("GoodMorningJap"); } }

		public static string GoodAfternoon { get { return ConfigurationManager.AppSettings.Get("GoodAfternoon"); } }
		public static string GoodAfternoonJap { get { return ConfigurationManager.AppSettings.Get("GoodAfternoonJap"); } }

		public static string GoodEvening { get { return ConfigurationManager.AppSettings.Get("GoodEvening"); } }
		public static string GoodEveningJap { get { return ConfigurationManager.AppSettings.Get("GoodEveningJap"); } }
		public static string Protocol { get { return ConfigurationManager.AppSettings.Get("Protocol"); } }
		public static string KeyFileUserId => ConfigurationManager.AppSettings.Get("KeyFileUserId");
	}
}