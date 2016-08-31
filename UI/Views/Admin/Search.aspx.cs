using System;
using UI.Classes;

namespace UI.Admin.WordCollocation
{
	public partial class Search : System.Web.UI.Page
	{
		private string keyword;
		private string entity;
		private string returnUrl;
		
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				entity = Request.QueryString["entity"].ToLower();
				keyword = Request.QueryString["keyword"].ToLower();
				BackButton1.ReturnUrl = returnUrl = UIHelper.GetReturnUrlByReferer();

				if (!string.IsNullOrEmpty(entity))
				{
					switch (entity)
					{
						case "pos":
							var pos = Models.POS.Search(keyword) ?? Models.POS.SearchChi(keyword);
							if (pos != null)
							{
								HideShowSearchResult(true);
								lblEntry.Text = pos.Entry;
								lblEntryChi.Text = pos.EntryChi;
								lnkEdit.NavigateUrl = string.Format("AddEdit.aspx?entity=pos&entityID={0}&mode={1}&returnUrl={2}", pos.posId,
								                                    ModelAction.Update.ToString(), returnUrl);
							}else HideShowSearchResult(false);
							break;
						case "word":
							var word = Models.Word.Search(keyword) ?? Models.Word.SearchChi(keyword);
							if (word != null)
							{
								HideShowSearchResult(true);
								lblEntry.Text = word.Entry;
								lblEntryChi.Text = word.EntryChi;
								lnkEdit.NavigateUrl = string.Format("AddEdit.aspx?entity=word&entityID={0}&mode={1}&returnUrl={2}", word.wordId,
								                                    ModelAction.Update.ToString(), returnUrl);
							}else HideShowSearchResult(false);
							break;
						case "example":
							var example = Models.WcExample.Search(keyword) ?? Models.WcExample.SearchChi(keyword);
							if (example != null)
							{
								HideShowSearchResult(true);
								lblEntry.Text = example.Entry;
								lblEntryChi.Text = example.EntryChi;
								lnkEdit.NavigateUrl = string.Format("AddEdit.aspx?entity=example&entityID={0}&mode={1}&returnUrl={2}", example.WcExampleId,
								                                    ModelAction.Update.ToString(), returnUrl);
							}else HideShowSearchResult(false);
							break;
					}
				}
			}
		}

		private void HideShowSearchResult(bool show)
		{
			pnlSearch.Visible = show;
			lblNoResult.Visible = !show;
		}
	}
}