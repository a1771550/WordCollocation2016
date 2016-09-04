using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using log4net;
using UI.Controllers.Abstract;
using UI.Models.Misc;
using ErrorViewModel = UI.Models.ViewModels.ErrorViewModel;

namespace UI.Controllers
{
	public enum HttpStatusErrorCode
	{
		code404,
		code403,
		UrlErrors
	}

	public class ErrorController : WcControllerBase
	{
		//private readonly ILog log = ((LoggingHandler)System.Web.HttpContext.Current.Session[MvcApplication.LoggingHandlerSession] == LoggingHandler.log4net)
		//	? LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)
		//	: null;

		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		

		// GET: Error
		public ActionResult Index(ErrorViewModel model = null)
		{
			return View(model);
		}

		/// <summary>
		/// 404 error
		/// </summary>
		/// <returns></returns>
		public ActionResult NotFound()
		{
			ViewBag.ErrorCode = HttpStatusErrorCode.code404;
			return View("Error");
		}

		/// <summary>
		/// 403 error
		/// </summary>
		/// <returns></returns>
		public ActionResult AccessDenied()
		{
			ViewBag.ErrorCode = HttpStatusErrorCode.code403;
			return View("Error");
		}

		/// <summary>
		/// Catch all route errors
		/// </summary>
		/// <returns></returns>
		public ActionResult RouteErrors()
		{
			ViewBag.ErrorCode = HttpStatusErrorCode.UrlErrors;
			return View("Error");
		}


		/// <summary>
		/// for log4net
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		public EmptyResult JsErrorHandler()
		{
			if (!string.IsNullOrEmpty(Request["msg"]) && !string.IsNullOrEmpty(Request["url"]) && !string.IsNullOrEmpty(Request["line"]))
			{
				try
				{
					StringBuilder bodystr = new StringBuilder();
					bodystr.Append("<html><head>");

					bodystr.Append("<style>");
					bodystr.Append(" body {font-family:\"Verdana\";font-weight:normal;font-size: .7em;color:black;} ");
					bodystr.Append(" p {font-family:\"Verdana\";font-weight:normal;color:black;margin-top: -5px}");
					bodystr.Append(" b {font-family:\"Verdana\";font-weight:bold;color:black;margin-top: -5px}");
					bodystr.Append(" H1 { font-family:\"Verdana\";font-weight:normal;font-size:16pt;color:red }");
					bodystr.Append(" H2 { font-family:\"Verdana\";font-weight:normal;font-size:14pt;color:maroon }");
					bodystr.Append(" H4 { font-family:\"Verdana\";font-weight:bold;font-size:11pt;}");
					bodystr.Append(" pre {font-family:\"Lucida Console\";font-size: .9em}");
					bodystr.Append(" .marker {font-weight: bold; color: black;text-decoration: none;}");
					bodystr.Append(" .version {color: gray;}");
					bodystr.Append(" .error {margin-bottom: 10px;}");
					bodystr.Append(" .expandable { text-decoration:underline; font-weight:bold; color:navy; cursor:hand; }");
					bodystr.Append("</style>");
					bodystr.Append("</head><body bgcolor=\"white\">");

					bodystr.Append(String.Format("<b>Date:</b> {0}<br>", DateTime.Now));
					bodystr.Append(String.Format("<b>Page:</b> <a href=\"{0}\">{0}</a><br>", Request.ServerVariables["HTTP_REFERER"]));
					bodystr.Append(String.Format("<b>JS File: </b><a href=\"{0}\">{0}</a><br>", HttpUtility.HtmlEncode(Request["url"])));
					bodystr.Append(String.Format("<b>Error Message: </b>{0}<br>", HttpUtility.HtmlEncode(Request["msg"])));
					bodystr.Append(String.Format("<b>Line:</b>{0}<br><br>", HttpUtility.HtmlEncode(Request["line"])));

					int i;
					bodystr.Append("<h4>Server Variables:</h4>");
					for (i = 0; i <= Request.ServerVariables.Count - 1; i++)
					{
						bodystr.Append(String.Format("<b>{0}:</b> {1}<br>", Request.ServerVariables.Keys[i], Request.ServerVariables[i]));
					}

					// Add Session Values in email
					bodystr.Append("<h4>Session Variables:</h4>");

					if ((Session != null))
					{
						if (Session.Count > 0)
						{
							foreach (string item in Session.Keys)
							{
								bodystr.Append(String.Format("<b>{0}:</b> {1}<br>", item, (Session[item] == null ? "" : Session[item].ToString())));
							}
						}
						else
						{
							bodystr.Append("<b>No Session values: 0</b> <br>");
						}
					}
					else
					{
						bodystr.Append("<b> No Session values: nothing</b> <br>");
					}
					bodystr.Append("</body></html>");

					AlternateView bodyHtmlView = AlternateView.CreateAlternateViewFromString(bodystr.ToString(), new ContentType("text/html"));

					try
					{
						SmtpClient SmtpServer = new SmtpClient
						{
							Credentials = new NetworkCredential(SiteConfiguration.MailID, SiteConfiguration.MailPassword),
							Port = SiteConfiguration.MailPort,
							Host = SiteConfiguration.MailServer
						};
						MailMessage message = new MailMessage { From = new MailAddress(SiteConfiguration.MailSender) };
						message.To.Add(new MailAddress("a1771550@gmail.com"));
						message.Bcc.Add(new MailAddress("a1771550@hotmail.com"));
						message.Subject = String.Format("JS Error on the site - {0}", Request["msg"]);
						message.AlternateViews.Add(bodyHtmlView);
						SmtpServer.Send(message);
					}
					catch (SmtpException ex)
					{
						log.Error(ex.Message, ex);
						throw new SmtpException("Email Sending Error", ex.InnerException);
					}


					// Save error to file
					log.Error(bodystr.ToString());
				}
				catch (Exception ex)
				{
					log.Fatal("Error", ex);
				}
			}
			return null;
		}

		///// <summary>
		///// for Elmah
		///// </summary>
		///// <param name="message"></param>
		//public void LogJavaScriptError(string message)
		//{
		//	ErrorSignal.FromCurrentContext().Raise(new JavaScriptException(message));
		//}

		//public ViewResult ElmahJsDemo()
		//{
		//	return View();
		//}

		//public ViewResult ElmahDemo()
		//{
		//	//var error = new Error();
		//	return View();
		//}

		//public ViewResult Log4NetDemo()
		//{
		//	try
		//	{
		//		//string connString = null;
		//		//DataSet ds = DataAccess.CreateDataSet(null, "Select * from Categories");
		//		return View();

		//	}
		//	catch (Exception ex)
		//	{
		//		log.Error(ex.Message, ex);
		//	}
		//	return null;
		//}
	}
}