using System;
using System.Web.UI;

namespace UI
{
	public partial class _default : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Response.Redirect("~/Home/UnderConstruction");
		}
	}
}