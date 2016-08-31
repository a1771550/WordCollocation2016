using System;
using System.Web.UI;
using UI.Helpers;

namespace UI.Demo
{
	public partial class GreetingsDemo : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				GreetingsHelper.SetGreetings("Kevin Lau", "Greetings");
			}
		}
	}
}