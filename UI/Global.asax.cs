using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using UI.Classes;
using UI.Models;
using WebMatrix.WebData;

namespace UI
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
			// Initialize log4net.
			log4net.Config.XmlConfigurator.Configure();

			//Database.SetInitializer(new InitSecurityDb());

			/* To turn off database initializer: */
			Database.SetInitializer<UsersContext>(null);
			
			UsersContext context = new UsersContext();
			context.Database.Initialize(true);
			if (!WebSecurity.Initialized)
				WebSecurity.InitializeDatabaseConnection("WordCollocation",
					"UserProfile", "UserId", "UserName", autoCreateTables: true);
		}
    }
}
