using System.Collections.Generic;
using System.Web.Mvc;
using CommonLib.Helpers;
using MyWcModel;
using MyWcModel.Abstract;

namespace UI.Models.Abstract
{
	public abstract class CommonViewModelBase
	{
		//private readonly AccountRepository _accountDb = new AccountRepository();
		//readonly PosRepository posDb=new PosRepository();

		public virtual List<SelectListItem> CreateDropDownList(ModelType modelType, string selectedId = null)
		{
			var ddlEntity = new List<SelectListItem>();
			string culturename = CultureHelper.GetCurrentCulture();
			string optiontext = null;
			switch (modelType)
			{
				case ModelType.Pos:
					ddlEntity.Add(new SelectListItem { Selected = selectedId == null, Text = string.Format("- {0} -", THResources.Resources.Pos), Value = "0" });
					var prepo = new PosRepository();
					List<pos> pList = prepo.GetList();
					foreach (pos entity in pList)
					{
						if (culturename.Contains("hans"))
						{
							optiontext = entity.Entry + " " + entity.EntryZhs;
						}
						else if (culturename.Contains("ja"))
						{
							optiontext = entity.Entry + " " + entity.EntryJap;
						}
						else 
						{
							optiontext = entity.Entry + " " + entity.EntryZht;
						}
						PopulateDropDownList(optiontext, entity.Id.ToString(), entity.Id.ToString() == selectedId, ref ddlEntity);
					}
					break;
				case ModelType.ColPos:
					ddlEntity.Add(new SelectListItem { Selected = selectedId == null, Text = string.Format("- {0} -", THResources.Resources.ColPos), Value = "0" });
					var cprepo = new PosRepository();
					List<pos> cpList = cprepo.GetList();
					foreach (pos entity in cpList)
					{
						if (culturename.Contains("hans"))
						{
							optiontext = entity.Entry + " " + entity.EntryZhs;
						}
						else if (culturename.Contains("ja"))
						{
							optiontext = entity.Entry + " " + entity.EntryJap;
						}
						else
						{
							optiontext = entity.Entry + " " + entity.EntryZht;
						}
						PopulateDropDownList(optiontext, entity.Id.ToString(), entity.Id.ToString() == selectedId, ref ddlEntity);
					}
					break;
				case ModelType.Word:
					ddlEntity.Add(new SelectListItem { Selected = selectedId == null, Text = string.Format("- {0} -", THResources.Resources.Word), Value = "0" });
					var wrepo = new WordRepository();
					List<word> wList = wrepo.GetList();
					foreach (word entity in wList)
					{
						if (culturename.Contains("hans"))
						{
							optiontext = entity.Entry + " " + entity.EntryZhs;
						}
						else if (culturename.Contains("ja"))
						{
							optiontext = entity.Entry + " " + entity.EntryJap;
						}
						else
						{
							optiontext = entity.Entry + " " + entity.EntryZht;
						}
						PopulateDropDownList(optiontext, entity.Id.ToString(), entity.Id.ToString() == selectedId, ref ddlEntity);
					}
					break;
				case ModelType.ColWord:
					ddlEntity.Add(new SelectListItem { Selected = selectedId == null, Text = string.Format("- {0} -", THResources.Resources.ColWord), Value = "0" });
					var cwrepo = new WordRepository();
					List<word> cwList = cwrepo.GetList();
					foreach (word entity in cwList)
					{
						if (culturename.Contains("hans"))
						{
							optiontext = entity.Entry + " " + entity.EntryZhs;
						}
						else if (culturename.Contains("ja"))
						{
							optiontext = entity.Entry + " " + entity.EntryJap;
						}
						else
						{
							optiontext = entity.Entry + " " + entity.EntryZht;
						}
						PopulateDropDownList(optiontext, entity.Id.ToString(), entity.Id.ToString() == selectedId, ref ddlEntity);
					}
					break;
			}

			return ddlEntity;
		}

		private static void PopulateDropDownList(string text, string value, bool selected, ref List<SelectListItem> ddlEntity)
		{
			ddlEntity.Add(new SelectListItem
			{
				Text = text,
				Value = value,
				Selected = selected
			});
		}
	}

	public enum CrudMode
	{
		Create,
		Update,
		Read,
		Delete
	}
}