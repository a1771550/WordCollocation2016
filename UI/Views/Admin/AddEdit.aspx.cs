using System;
using System.Globalization;
using System.Web.UI.WebControls;
using UI.Classes;

namespace UI.Admin.WordCollocation
{
	public partial class AddEdit : BasePage
	{
		private string entity;
		private string mode;
		private string entityID;
		private string returnUrl;

		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageTitle = UIHelper.SetPageTitle("Admin");
			if (IsPostBack) return;

			txtEntry.Focus();

			ViewState["entity"] = entity = Request.QueryString["entity"].ToLower();

			ViewState["mode"] = mode = Request.QueryString["mode"].ToLower();

			ViewState["ReturnUrl"] = returnUrl = BackButton1.ReturnUrl = Request.QueryString["ReturnUrl"].ToLower();

			lblLegend.Text = entity.ToUpper();
			//ErrorLabel1.Visible = false;

			switch (mode)
			{
				case "create":
					pnlID.Visible = false;
					btnCommand.Text = ModelAction.Create.ToString();
					break;
				case "update":
					ViewState["entityID"] = entityID = Request["entityID"];
					BindForm();
					pnlID.Visible = true;
					btnCommand.Text = ModelAction.Update.ToString();
					break;
				//case "delete":
				//    ViewState["entityID"] = entityID = Request["entityID"];
				//    DeleteEntity(entityID);
				//    break;
			}

			switch (entity)
			{
				case "pos":

					break;
				case "word":
					txtEntryJap.Width = txtEntryChi.Width = txtEntry.Width = Unit.Pixel(400);
					break;
				
				//case "collocation":

				//    break;
			}
		}


		private void BindForm()
		{
			switch (entity)
			{
				case "pos":
					var pid = Convert.ToInt16(entityID);
					var pos = Models2013.POS.GetById(pid);
					lblID.Text = pos.posId.ToString(CultureInfo.InvariantCulture);
					txtEntry.Text = pos.Entry;
					txtEntryChi.Text = pos.EntryChi;
					txtEntryJap.Text = pos.EntryJap;
					break;
				case "word":
					var wid = Convert.ToInt64(entityID);
					var word = Models2013.Word.GetById(wid);
					lblID.Text = word.wordId.ToString(CultureInfo.InvariantCulture);
					txtEntry.Text = word.Entry;
					txtEntryChi.Text = word.EntryChi;
					txtEntryJap.Text = word.EntryJap;
					break;
				
			}
		}


		protected void btnCommand_Click(object sender, EventArgs e)
		{
			if (!Page.IsValid) return;
			mode = (string)ViewState["mode"];
			entity = (string)ViewState["entity"];
			returnUrl = (string)ViewState["ReturnUrl"];

			switch (mode)
			{
				case "create":

					switch (entity)
					{
						case "pos":
							var pos = new Models2013.POS
										{
											Entry = txtEntry.Text.Trim(),
											EntryChi = txtEntryChi.Text.Trim(),
											EntryJap = txtEntryJap.Text.Trim()
										};
							try
							{
								if (Models2013.POS.Create(pos) > 0)
									Response.Redirect(returnUrl);

							}
							catch (Exception ex)
							{
								ErrorDisplay(ex);
							}
							break;
						case "word":
							var word = new Models2013.Word
										{
											Entry = txtEntry.Text.Trim(),
											EntryChi = txtEntryChi.Text.Trim(),
											EntryJap = txtEntryJap.Text.Trim()
										};
							try
							{
								if (Models2013.Word.Create(word) > 0)
									Response.Redirect(returnUrl);

							}
							catch (Exception ex)
							{
								ErrorDisplay(ex);
							}
							break;
					}

					break;
				case "update":
					entityID = (string)ViewState["entityID"];
					switch (entity)
					{
						case "pos":
							var pos = new Models2013.POS
										{
											posId = Convert.ToInt16(entityID),
											Entry = txtEntry.Text.Trim(),
											EntryChi = txtEntryChi.Text.Trim(),
											EntryJap = txtEntryJap.Text.Trim()
										};
							try
							{
								Models2013.POS.Update(pos);
								Response.Redirect(returnUrl);
							}
							catch (Exception ex)
							{
								ErrorDisplay(ex);
							}
							break;
						case "word":
							var Id = Convert.ToInt64(entityID);
							var word = Models2013.Word.GetById(Id);
							word.Entry = txtEntry.Text.Trim();
							word.EntryChi = txtEntryChi.Text.Trim();
							word.EntryJap = txtEntryJap.Text.Trim();
							try
							{
								Models2013.Word.Update(word);
								Response.Redirect(returnUrl);
							}
							catch (Exception ex)
							{
								ErrorDisplay(ex);
							}
							break;
					}
					break;
			}
		}

		private void ErrorDisplay(Exception ex)
		{
			ErrorLabel1.Visible = true;
			ErrorLabel1.ErrorText = string.Format("Error Message: {0}\nStackTrace: {1}", ex.Message, ex.StackTrace);
		}
	}
}