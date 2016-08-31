using System;
using System.Linq;
using System.Web.UI.WebControls;
using UI.App_GlobalResources;
using UI.Classes;
using UI.Resources;

namespace UI.Admin.WordCollocation
{
	public partial class Collocation : BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageTitle = UIHelper.SetPageTitle(UICommon.Home);
			if (!IsPostBack)
			{
				Session["ReturnUrl"] = "collocation.aspx";
				lnkCreate.NavigateUrl = "AddEdit_Col.aspx?entity=collocation&mode=create&returnUrl=collocation.aspx";
				BindgvList();
			}

		}

		private void BindgvList()
		{
			gvList.DataSource = Models2013.Collocation.GetList().OrderBy(c=>c.Entry_Word).ToList();
			gvList.DataBind();
		}


		protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvList.PageIndex = e.NewPageIndex;
			BindgvList();
		}

		protected void gvList_Sorting(object sender, GridViewSortEventArgs e)
		{
			string direction = UIHelper.GetSortDirection(e.SortExpression, ref SortExpression, ref SortDirection);
			switch (e.SortExpression.ToLower())
			{
				case "word":
					switch (direction.ToLower())
					{
						case "asc":
							gvList.DataSource = Models2013.Collocation.GetList().OrderBy(c => c.Entry_Word).ToList();
							gvList.DataBind();
							break;
						case "desc":
							gvList.DataSource = Models2013.Collocation.GetList().OrderByDescending(c => c.Entry_Word).ToList();
							gvList.DataBind();
							break;
					}
					break;
				case "colword":
					switch (direction.ToLower())
					{
						case "asc":
							gvList.DataSource = Models2013.Collocation.GetList().OrderBy(c => c.Entry_colWord).ToList();
							gvList.DataBind();
							break;
						case "desc":
							gvList.DataSource = Models2013.Collocation.GetList().OrderByDescending(c => c.Entry_colWord).ToList();
							gvList.DataBind();
							break;
					}
					break;
				case "colpattern":
					switch (direction.ToLower())
					{
						case "asc":
							gvList.DataSource = Models2013.Collocation.GetList().OrderBy(c => c.CollocationPattern).ToList();
							gvList.DataBind();
							break;
						case "desc":
							gvList.DataSource = Models2013.Collocation.GetList().OrderByDescending(c => c.CollocationPattern).ToList();
							gvList.DataBind();
							break;
					}
					break;
			}
		}
	}
}