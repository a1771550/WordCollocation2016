using System;
using Models2013;
using UI.Classes;

namespace UI.Admin.WordCollocation
{
	public partial class AddEdit_Example : BasePage
	{
		private long collocationId, Id;
		private string mode;
		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageTitle = UIHelper.SetPageTitle("Admin");
			mode = Request.QueryString.Get("mode").ToLower();
			if (!IsPostBack)
			{
				BackButton1.ReturnUrl = Request.QueryString.Get("ReturnUrl");
				switch (mode)
				{
					case "create":
						var collocation = GetCollocation();
						pnlWord.Visible = true;
						lblWord.Text = collocation.Word.Entry;
						lblColWord.Text = collocation.colWord.Entry;//.Entry_colWord;
						lblPOS.Text = collocation.POS.Entry;//.Entry_POS;
						lblColPOS.Text = collocation.colPOS.Entry;//.Entry_colPOS;
						btnCommand.Text = ModelAction.Create.ToString();
						break;
					case "update":
						var example = GetExample();
						txtExample.Text = example.Entry;
						txtExampleChi.Text = example.EntryChi;
						txtExampleJap.Text = example.EntryJap;
						ViewState["source"] = example.Source;
						txtRemark.Text = string.IsNullOrEmpty(example.Remark) ? null : example.Remark;
						btnCommand.Text = ModelAction.Update.ToString();
						break;
				}
				BindddlSources();
			}

		}

		private void BindddlSources()
		{
			ddlSources.Items.Clear();
			var sources = SiteConfiguration.WcExampleSources;
			var sourceList = sources.Split(',');
			foreach (string t in sourceList)
				ddlSources.Items.Add(t);

			ddlSources.SelectedValue = ViewState["source"] == null ? sourceList[0] : ViewState["source"].ToString();
		}

		private Models2013.Collocation GetCollocation()
		{
			collocationId = Convert.ToInt64(Request.QueryString.Get("collocationId"));
			var collocation = Models2013.Collocation.GetById(collocationId);
			return collocation;
		}

		private WcExample GetExample()
		{
			Id = Convert.ToInt64(Request.QueryString.Get("Id"));
			var example = Models2013.WcExample.GetById(Id);
			return example;
		}

		protected void btnCommand_Click(object sender, EventArgs e)
		{
			if (IsValid)
			{
				string entry = txtExample.Text.Trim();
				string entryChi = txtExampleChi.Text.Trim();
				string entryJap = txtExampleJap.Text.Trim();
				WcExample example;

				switch (mode)
				{
					case "create":
						//var collocation = GetCollocation();
						example = new WcExample { Entry = entry, EntryChi = entryChi, EntryJap = entryJap, Source = ddlSources.SelectedValue, Remark = string.IsNullOrEmpty(txtRemark.Text) ? null : txtRemark.Text.Trim() };
						collocationId = Convert.ToInt64(Request.QueryString.Get("collocationId"));
						WcExample.Create(example, collocationId);
						Response.Redirect("Collocation.aspx");
						Id = example.WcExampleId;
						break;
					case "update":
						example = GetExample();
						example.Entry = entry;
						example.EntryChi = entryChi;
						example.EntryJap = entryJap;
						example.Source = ddlSources.SelectedValue;
						example.Remark = string.IsNullOrEmpty(txtRemark.Text) ? null : txtRemark.Text.Trim();
						WcExample.Update(example);
						Response.Redirect("Collocation.aspx");
						break;
				}
			}
		}
	}
}