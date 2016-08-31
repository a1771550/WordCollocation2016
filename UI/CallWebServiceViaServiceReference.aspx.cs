using System;
using System.Web.UI;
using UI.WebServices;

namespace UI
{
	public partial class CallWebServiceViaServiceReference : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Request.QueryString.Count > 0)
			{
				if (Request.QueryString.Get("name") != null)
				{
					var name = Request.QueryString.Get("name");
					var client = new WcServices();
					bool isOk = client.CheckIfDuplicatedUserName(name);
					Response.ContentType = "application/json; charset=utf-8";
					Response.Write(isOk);
				}
			}
		}

		

		protected void btnCall_OnClick(object sender, EventArgs e)
		{
			//var client = new WcServices();
			//var name = txtName.Text.Trim();
			//bool isOK=client.CheckIfDuplicatedUserName(name);
			//lblResult.Text = isOK.ToString();
		}
	}
}