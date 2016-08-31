using System.Collections.Generic;
using System.Web.Mvc;
using BLL;
using BLL.Abstract;

namespace UI.Models.Abstract
{
	public abstract class CommonViewModelBase
	{
		//private readonly AccountRepository _accountDb = new AccountRepository();
		//readonly PosRepository posDb=new PosRepository();

		public virtual List<SelectListItem> CreateDropDownList(ModelType modelType, string selectedId = null)
		{
			var ddlEntity = new List<SelectListItem>();

			switch (modelType)
			{
				//case ModelType.Role:
				//	ddlEntity.Add(new SelectListItem { Selected = selectedId == null, Text = string.Format("- {0} -", THResources.Resources.RoleCode), Value = "0" });
				//	List<Role> roleList = _accountDb.GetRoleList();

				//	foreach (Role entity in roleList)
				//	{
				//		PopulateDropDownList_Role(selectedId, ref ddlEntity, entity);
				//	}
				//	break;
				//case ModelType.User:
				//	ddlEntity.Add(new SelectListItem { Selected = selectedId == null, Text = string.Format("- {0} -", THResources.Resources.Email), Value = "0" });
				//	List<User> uList = _accountDb.GetUserList();

				//	foreach (User entity in uList)
				//	{
				//		PopulateDropDownList_User(selectedId, ref ddlEntity, entity);
				//	}
				//	break;
				case ModelType.Pos:
					ddlEntity.Add(new SelectListItem { Selected = selectedId == null, Text = string.Format("- {0} -", THResources.Resources.Pos), Value = "0" });
					var prepo = new PosRepository();
					List<Pos> pList = prepo.GetList();

					foreach (Pos entity in pList)
					{
						PopulateDropDownList(entity.Entry, entity.Id.ToString(), entity.Id.ToString() == selectedId, ref ddlEntity);
					}
					break;
				case ModelType.ColPos:
					ddlEntity.Add(new SelectListItem { Selected = selectedId == null, Text = string.Format("- {0} -", THResources.Resources.ColPos), Value = "0" });
					var cprepo = new ColPosRepository();
					List<ColPos> cpList = cprepo.GetList();
					foreach (var entity in cpList)
					{
						PopulateDropDownList(entity.Entry, entity.Id.ToString(), entity.Id.ToString() == selectedId, ref ddlEntity);
					}
					break;
				case ModelType.Word:
					ddlEntity.Add(new SelectListItem { Selected = selectedId == null, Text = string.Format("- {0} -", THResources.Resources.Word), Value = "0" });
					var wrepo = new WordRepository();
					List<Word> wList = wrepo.GetList();
					foreach (var entity in wList)
					{
						PopulateDropDownList(entity.Entry, entity.Id.ToString(), entity.Id.ToString() == selectedId, ref ddlEntity);
					}
					break;
				case ModelType.ColWord:
					ddlEntity.Add(new SelectListItem { Selected = selectedId == null, Text = string.Format("- {0} -", THResources.Resources.ColWord), Value = "0" });
					var cwrepo = new ColWordRepository();
					List<ColWord> cwList = cwrepo.GetList();
					foreach (var entity in cwList)
					{
						PopulateDropDownList(entity.Entry, entity.Id.ToString(), entity.Id.ToString() == selectedId, ref ddlEntity);
					}
					break;
			}

			return ddlEntity;
		}

		//private void PopulateDropDownList_Role(string selectedCode, ref List<SelectListItem> ddlEntity, Role role)
		//{
		//	ddlEntity.Add(new SelectListItem
		//	{
		//		Text = role.Name,
		//		Value = role.Code,
		//		Selected = selectedCode == role.Code
		//	});
		//}

		//private static void PopulateDropDownList_User(string selectedEmail, ref List<SelectListItem> ddlEntity, User user)
		//{
		//	ddlEntity.Add(new SelectListItem
		//	{
		//		Text = user.UserName,
		//		Value = user.Email,
		//		Selected = selectedEmail == user.Email
		//	});
		//}
		

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