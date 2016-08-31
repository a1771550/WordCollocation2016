using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using Models2013;
using UI.App_GlobalResources;
using UI.Classes;

namespace UI.Admin.WordCollocation
{
	public partial class AddEdit_Col : BasePage
	{
		private string mode;
		private string entityID;
		WcExample wcExample;
		private Models2013.Word Word, ColWord;
		private Models2013.Preposition ColPrep;
		private Models2013.Collocation Collocation;
		private List<WcExample> Examples;
		private long collocationId;
		private string word, colWord, pos, colpos;
		private CollocationPattern collocationPattern;
		//private const string SessionColWord = "ColWord";
		//private const string SessionColPrep = "ColPrep";
		private const string SessionCollocationText = "Collocation";

		protected void Page_Load(object sender, EventArgs e)
		{
			Master.PageTitle = UIHelper.SetPageTitle("Admin");
			mode = Request.QueryString["mode"].ToLower();
			//returnUrl = Request.QueryString["ReturnUrl"].ToLower();
			lblLegend_Col.Text = GlobalResources.Collocation;

			//BackButton1.ReturnUrl = UIHelper.GetReturnUrlByReferer(); //doesn't work as expected
			BackButton1.ReturnUrl = "collocation.aspx";

			switch (mode)
			{
				case "create":
					btnCommand.Text = ModelAction.Create.ToString();
					ShowHidePnlID(false);
					break;
				case "update":
					Collocation = GetCollocation();
					btnCommand.Text = ModelAction.Update.ToString();
					CreateExample.NavigateUrl = string.Format("AddEdit_Example.aspx?mode=create&collocationId={0}", collocationId);
					ShowHidePnlID(true);
					lblcolID.Text = entityID;
					Word = Collocation.Word;

					//if (Collocation.colWordId > 0)
					//{
						ColWord = Collocation.colWord;
						//Session[SessionColWord] = ColWord;
					//}
					//else
					//{
					//	ColPrep = Collocation.colPrep;
					//	//Session[SessionColPrep] = ColPrep;
					//}

					collocationPattern = Collocation.CollocationPattern;
					break;
			}

			if (!IsPostBack)
			{
				BindddlLetter();
				BindddlPOS();
				BindddlWord();
				BindExamples();
				BindddlSources();
				BindddlColPattern();
			}
		}

		private void BindddlColPattern()
		{
			ddlColPattern.Items.Clear();
			//ddlColPattern.Items.Add(new ListItem(CollocationPattern.noun_verb.ToString(),
			//									 ((int) CollocationPattern.noun_verb).ToString(CultureInfo.InvariantCulture)));
			//ddlColPattern.Items.Add(new ListItem(CollocationPattern.verb_noun.ToString(), ((int)CollocationPattern.verb_noun).ToString(CultureInfo.InvariantCulture)));
			//ddlColPattern.Items.Add(new ListItem(CollocationPattern.adj_noun.ToString(), ((int)CollocationPattern.adj_noun).ToString(CultureInfo.InvariantCulture)));
			//ddlColPattern.Items.Add(new ListItem(CollocationPattern.verb_prep.ToString(), ((int)CollocationPattern.verb_prep).ToString(CultureInfo.InvariantCulture)));
			//ddlColPattern.Items.Add(new ListItem(CollocationPattern.prep_verb.ToString(), ((int)CollocationPattern.prep_verb).ToString(CultureInfo.InvariantCulture)));
			//ddlColPattern.Items.Add(new ListItem(CollocationPattern.adv_verb.ToString(), ((int)CollocationPattern.adv_verb).ToString(CultureInfo.InvariantCulture)));

			var patterns = Models2013.Collocation.GetColPatternDictionary();
			foreach (var kv in patterns)
				ddlColPattern.Items.Add(new ListItem(kv.Key, kv.Value.ToString(CultureInfo.InvariantCulture)));

			switch (mode)
			{
				case "create":
					ddlColPattern.SelectedIndex = 0;
					break;
				case "update":
					Collocation = (Models2013.Collocation)Session[SessionCollocationText];
					var listItem = ddlColPattern.Items.FindByText(Collocation.CollocationPattern.ToString());
					if (listItem != null)
						ddlColPattern.SelectedValue = listItem.Value;
					break;
			}
		}

		private Models2013.Collocation GetCollocation()
		{
			entityID = Request.QueryString["entityID"].ToLower();
			collocationId = Convert.ToInt64(entityID);
			Collocation = Models2013.Collocation.GetById(collocationId);
			Session[SessionCollocationText] = Collocation;
			return Collocation;
		}

		private void BindExamples()
		{
			switch (mode)
			{
				case "create":
					ExampleBlock.Visible = true;
					break;
				case "update":
					pnlEdit.Visible = true;
					Collocation = (Models2013.Collocation)Session[SessionCollocationText];
					Examples = Collocation.WcExamples;
					/* -- Formattion --*/
					foreach (var example in Examples)
					{
						word = Collocation.Word.Entry;
						pos = Collocation.POS.Entry;
						colpos = Collocation.colPOS.Entry;
						collocationPattern = Collocation.CollocationPattern;

						//if (Collocation.colWordId > 0)
						//{
							colWord = Collocation.colWord.Entry;
							example.Entry = UIHelper.FormatExampleForView(example.Entry, word, pos, colWord, colpos, collocationPattern);
						//}
						//else
						//{
						//	colPrep = Collocation.colPrep.Value;
						//	example.Entry = UIHelper.FormatExampleForView(example.Entry, word, pos, colPrep, colpos, collocationPattern);
						//}




					}
					/*-----------------*/
					ExampleList.DataSource = Examples;
					ExampleList.DataBind();
					break;
			}
		}

		private void ShowHidePnlID(bool show)
		{
			pnlID_Col.Visible = show;
		}

		private void BindddlWord()
		{
			ddlWord.Items.Clear();
			ddlWord.DataSource = Models2013.Word.GetList();
			ddlWord.DataTextField = "Entry";
			ddlWord.DataValueField = "wordId";
			ddlWord.DataBind();

			ddlColWord.Items.Clear();

			if (Session[SessionCollocationText] != null) //mode=update
			{
				Collocation = (Models2013.Collocation) Session[SessionCollocationText];

				if (Collocation.colWordId > 0)
				{
					ddlColWord.DataSource = Models2013.Word.GetList();
					ddlColWord.DataTextField = "Entry";
					ddlColWord.DataValueField = "wordId";
				}
				else
				{
					ddlColWord.DataSource = Models2013.Preposition.GetList();
					ddlColWord.DataTextField = "value";
					ddlColWord.DataValueField = "key";
				}
			}
			else //mode=create
			{
				ddlColWord.DataSource = Models2013.Word.GetList();
				ddlColWord.DataTextField = "Entry";
				ddlColWord.DataValueField = "wordId";
			}
			
			
			ddlColWord.DataBind();

			switch (mode)
			{
				case "create":
					
					break;
				case "update":
					pnlWord.Visible = true;
					pnlColWord.Visible = true;

					var listItem = ddlWord.Items.FindByValue(Word.wordId.ToString(CultureInfo.InvariantCulture));
					if (listItem != null)
						ddlWord.SelectedValue = listItem.Value;

					listItem = ddlColWord.Items.FindByValue(Collocation.colWordId.ToString(CultureInfo.InvariantCulture));

					if (listItem != null)
						ddlColWord.SelectedValue = listItem.Value;
					break;

			}
		}

		private void BindddlLetter()
		{
			ddlLetter.Items.Clear();

			string[] letters = new[] { "---", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
			ddlColLetter.DataSource = ddlLetter.DataSource = letters;
			ddlLetter.DataBind();
			ddlColLetter.DataBind();

			switch (mode)
			{
				case "create":

					break;
				case "update":
					var letter = Word.Entry.Substring(0, 1).ToUpper();

					ddlLetter.SelectedValue = ddlLetter.Items.FindByText(letter).Value;

					Collocation = (Models2013.Collocation)Session[SessionCollocationText];

					//letter = Collocation.colWordId > 0 ? Collocation.colWord.Entry.Substring(0, 1).ToUpper() : Collocation.colPrep.Value.Substring(0, 1).ToUpper();
					letter = Collocation.colWord.Entry.Substring(0, 1).ToUpper();

					ddlColLetter.SelectedValue = ddlColLetter.Items.FindByText(letter).Value;
					break;
			}
		}

		private void BindddlPOS()
		{
			ddlPOS.Items.Clear();
			ddlPOS.DataSource = Models2013.POS.GetList();
			ddlPOS.DataTextField = "Entry";
			ddlPOS.DataValueField = "posId";
			ddlPOS.DataBind();

			ddlColPOS.Items.Clear();
			ddlColPOS.DataSource = Models2013.POS.GetList();
			ddlColPOS.DataTextField = "Entry";
			ddlColPOS.DataValueField = "posId";
			ddlColPOS.DataBind();

			switch (mode)
			{
				case "create":
					ddlPOS.Items.Insert(0, "---");
					ddlPOS.SelectedIndex = 0;
					ddlColPOS.Items.Insert(0, "---");
					ddlColPOS.SelectedIndex = 0;
					break;
				case "update":
					var collocation = Models2013.Collocation.GetById(Convert.ToInt64(entityID));
					Models2013.POS pos1 = collocation.POS;
					var listItem = ddlPOS.Items.FindByValue(pos1.posId.ToString(CultureInfo.InvariantCulture));
					if (listItem != null)
						ddlPOS.SelectedValue = listItem.Value;
					var colPos = collocation.colPOS;
					listItem = ddlColPOS.Items.FindByValue(colPos.posId.ToString(CultureInfo.InvariantCulture));
					if (listItem != null)
						ddlColPOS.SelectedValue = listItem.Value;
					break;

			}

		}

		protected void ddlLetter_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ddlLetter.SelectedItem.Value != "---")
			{
				pnlWord.Visible = true;

				ddlWord.DataSource = Models2013.Word.GetListByLetter(ddlLetter.SelectedItem.Value.ToLower());
				ddlWord.DataBind();
				ddlWord.SelectedIndex = 0;
			}
		}

		protected void ddlColLetter_SelectedIndexChanged(object sender, EventArgs e)
		{
			//ViewState["SelectedColLetterIndex"] = ddlColLetter.SelectedIndex;
			if (ddlColLetter.SelectedItem.Value != "---")
			{
				pnlColWord.Visible = true;
				ddlColWord.DataSource = Models2013.Word.GetListByLetter(ddlColLetter.SelectedItem.Value);
				ddlColWord.DataBind();
				ddlColWord.SelectedIndex = 0;
			}
		}

		private void BindddlSources()
		{
			ddlSources.Items.Clear();
			var sources = SiteConfiguration.WcExampleSources;
			var sourceList = sources.Split(',');
			foreach (string t in sourceList)
				ddlSources.Items.Add(t);
			ddlSources.SelectedIndex = 1;
		}

		protected void btnCommand_Click(object sender, EventArgs e)
		{
			if (IsValid)
			{
				long wordId, colWordId;
				short posId = Convert.ToInt16(ddlPOS.SelectedItem.Value);
				short colPosId = Convert.ToInt16(ddlColPOS.SelectedItem.Value);
				CollocationPattern collocationPattern = CollocationPattern.noun_verb; //initialize the variable

				if (posId == 5) //word = preposition
				{
					var prep = Preposition.GetByValue(ddlWord.SelectedItem.Text);
					wordId = long.Parse(prep.Key);
				}
				else wordId = Convert.ToInt64(ddlWord.SelectedItem.Value);

				if (colPosId == 5) // colword = preposition
				{
					ColPrep = Preposition.GetByValue(ddlColWord.SelectedItem.Text);
					colWordId = long.Parse(ColPrep.Key);
				}
				else colWordId = Convert.ToInt64(ddlColWord.SelectedItem.Value);

				//switch (ddlColPattern.SelectedValue)
				//{
				//	case "0":
				//		collocationPattern = CollocationPattern.noun_verb;
				//		break;
				//	case "1":
				//		collocationPattern = CollocationPattern.verb_noun;
				//		break;
				//	case "2":
				//		collocationPattern = CollocationPattern.adj_noun;
				//		break;
				//	case "3":
				//		collocationPattern = CollocationPattern.verb_prep;
				//		break;
				//	case "4":
				//		collocationPattern = CollocationPattern.prep_verb;
				//		break;
				//	case "5":
				//		collocationPattern = CollocationPattern.adv_verb;
				//		break;
				//}

				collocationPattern = Models2013.Collocation.GetColPatternKeyByValue(int.Parse(ddlColPattern.SelectedValue));

				switch (mode)
				{
					case "create":
						Models2013.Collocation collocation = new Models2013.Collocation
						{
							posId = posId,
							colPosId = colPosId,
							wordId = wordId,
							colWordId = colWordId,
							CollocationPattern = collocationPattern
						};

						collocationId = Models2013.Collocation.Create(collocation);

						string entry = txtExample.Text.Trim();
						string entryChi = txtExampleChi.Text.Trim();
						string entryJap = txtExampleJap.Text.Trim();
						string remark = txtRemark.Text.Trim();

						if (!string.IsNullOrEmpty(entry))
						{
							wcExample = new WcExample
											{
												Entry = entry,
												EntryChi = entryChi,
												EntryJap = entryJap,
												Source = ddlSources.SelectedValue,
												Remark = remark
											};
							WcExample.Create(wcExample, collocationId);
						}

						if (collocationId > 0)
							Response.Redirect("Collocation.aspx");
						break;
					case "update":
						collocation = Models2013.Collocation.GetById(collocationId);
						collocation.posId = posId;
						collocation.colPosId = colPosId;
						collocation.wordId = wordId;
						collocation.colWordId = colWordId;
						collocation.CollocationPattern = collocationPattern;
						Models2013.Collocation.Update(collocation);

						Response.Redirect("Collocation.aspx");
						break;
				}

			}
		}

		protected void ddlColPOS_SelectedIndexChanged(object sender, EventArgs e)
		{
			pos = ddlColPOS.SelectedItem.Value.ToLower();
			if (pos == "5") //preposition
			{
				//ViewState["preposition"] = PrepositionFlag.ColWordIsPrep;
				divColLetter.Visible = false;
				pnlColWord.Visible = true;
				ddlColWord.Items.Clear();
				ddlColWord.DataSource = Preposition.GetList();
				ddlColWord.DataTextField = "Value";
				ddlColWord.DataValueField = "Key";
				ddlColWord.DataBind();
				ddlColWord.SelectedIndex = 0;
			}
			//else ViewState["preposition"] = null;
		}

		protected void ddlPOS_SelectedIndexChanged(object sender, EventArgs e)
		{
			pos = ddlPOS.SelectedItem.Value.ToLower();
			if (pos == "4") //preposition
			{
				//ViewState["preposition"] = PrepositionFlag.WordIsPrep;
				divLetter.Visible = false;
				pnlWord.Visible = true;
				ddlWord.Items.Clear();
				ddlWord.DataSource = Preposition.GetList();
				ddlWord.DataTextField = "Value";
				ddlWord.DataValueField = "Key";
				ddlWord.DataBind();
				ddlWord.SelectedIndex = 0;
			}
			//else ViewState["preposition"] = null;
		}
	}
}