using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UI.App_GlobalResources;
using UI.Classes;
using UI.Resources;

namespace UI.Admin.WordCollocation
{
	public partial class Example : BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageTitle = UIHelper.SetPageTitle(UICommon.Home);
			if (!IsPostBack)
			{
				Session["ReturnUrl"] = "example.aspx";
				//lnkCreate.NavigateUrl = "AddEdit.aspx?entity=example&mode=create&returnUrl=example.aspx";
			}
			EntityList1.Entity = "example";
		}
	}
}