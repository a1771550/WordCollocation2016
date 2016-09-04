using System.Web.Mvc;
using UI.Models.Logging;

namespace UI
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			//var handler = WebConfigurationManager.AppSettings["LoggingHandler"];
			//switch (handler.ToLower())
			//{
			//	case "l":
			//		filters.Add(new HandleErrorAttribute());
			//		//// Init Log4Net library
			//		//XmlConfigurator.Configure();
					
			//		return;
			//	case "e":
			//		filters.Add(new ElmahHandleErrorAttribute());
					
			//		return;
			//}

			//filters.Add(new ElmahHandleErrorAttribute());
			//filters.Add(new HandleErrorAttribute());
		}
	}
}
