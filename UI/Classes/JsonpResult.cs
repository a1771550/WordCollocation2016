using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace UI.Classes
{
	public class JsonpResult:JsonResult
	{
		object data = null;
		public JsonpResult() { }
		public JsonpResult(object data)
		{
			this.data = data;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (context != null)
			{
				HttpResponseBase response = context.HttpContext.Response;
				HttpRequestBase request = context.HttpContext.Request;
				string callbackfunction = request["callback"];
				if (string.IsNullOrEmpty(callbackfunction))
				{
					throw new Exception("callback function name must be provided in the request!");
				}
				response.ContentType = "application/x-javascript";
				response.ContentEncoding = Encoding.UTF8;
				if (data != null)
				{
					JavaScriptSerializer serializer = new JavaScriptSerializer();
					//response.Write(string.Format("{0}({1});", callbackfunction, serializer.Serialize(data)));
					StringBuilder sb = new StringBuilder();
					sb.Append(callbackfunction + "(");
					sb.Append(serializer.Serialize(data));
					sb.Append(");");
					response.Write(sb.ToString());
				}
			}
		}
	}
}