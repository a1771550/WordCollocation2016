using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UI.Admin.WordCollocation
{
	public partial class WcHandlerDemo : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				//Response.Redirect("WcHandler.ashx?method=CheckIfDuplicatedEntry&entry=look");
				var words = Models2013.Word.GetList();
				lblResult.Text = words.Any(w => w.Entry.ToLower() == "look").ToString();

			}
		}
	}
}