using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using Handlers;
using System;

namespace UI.Admin.WordCollocation
{
	/// <summary>
	/// Summary description for WcHandler
	/// </summary>
	public class WcHandler : BaseHandler
	{
		public void DeleteEntity(string entity, string entityId)
		{
			switch (entity.ToLower())
			{
				case "pos":
					try
					{
						Models2013.POS.Delete(short.Parse(entityId));
					}
					catch (Exception ex)
					{
						UI.Classes.UIHelper.LogError(null, ex);
					}
					HttpContext.Current.Response.Redirect("POS.aspx");

					break;
				case "word":
					try
					{
						Models2013.Word.Delete(long.Parse(entityId));
					}
					catch (Exception ex)
					{
						UI.Classes.UIHelper.LogError(null, ex);
					}
					HttpContext.Current.Response.Redirect("Word.aspx");
					break;
			}
		}

		public void DeleteExample(string Id, string returnUrl)
		{
			Models2013.WcExample.Delete(Convert.ToInt64(Id));
			HttpContext.Current.Response.Redirect(returnUrl);
		}

		public void DeleteCollocation(string Id)
		{
			Models2013.Collocation.Delete(Convert.ToInt64(Id));
			HttpContext.Current.Response.Redirect("Collocation.aspx");
		}

		public void CheckIfDuplicatedEntry(string entry)
		{
			//context.Response.ContentType = "text/plain";
			HttpContext.Current.Response.ContentType = "application/json";
			HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;
			JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
			var isDuplicated = Models2013.Word.GetList().Any(w => w.Entry.ToLower() == entry.ToLower());
			if (isDuplicated)
			{
				var result = javaScriptSerializer.Serialize(true);
				HttpContext.Current.Response.Write(result);
			}
			else
			{
				HttpContext.Current.Response.Write(javaScriptSerializer.Serialize(null));
			}
		}
	}
}