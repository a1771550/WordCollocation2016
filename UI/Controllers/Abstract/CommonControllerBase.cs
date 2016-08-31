using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using CommonLib.Helpers;
using log4net;

namespace UI.Controllers.Abstract
{
	public abstract class CommonControllerBase : Controller
	{
		//public const string UserCookie = "UserName";
		//public const string RolesCookie = "UserRoles";
		public const string GreetingsCookie = "Greetings";
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
		{
			try
			{
				string cultureName;

				// attempt to read the culture cookie from request
				HttpCookie cultureCookie = Request.Cookies["_culture"];
				if (cultureCookie != null) cultureName = cultureCookie.Value;
				else
					cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null;

				// validate culture name
				cultureName = CultureHelper.GetImplementedCulture(cultureName);

				// modify current thread's cultures
				Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
				Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

				return base.BeginExecuteCore(callback, state);
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, ex);
			}
			return null;
		}

		public virtual void RenderModelErrorList()
		{
			try
			{
				List<ModelError> errors = new List<ModelError>();
				foreach (var m in ModelState.Values)
				{
					errors.AddRange(m.Errors);
				}
				ViewBag.ErrorList = errors;
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, ex);
			}
		}

	}
}