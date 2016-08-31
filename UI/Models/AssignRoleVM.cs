using System.Collections.Generic;
using System.Web.Mvc;

namespace UI.Models
{
	public class AssignRoleVM
	{
		public string RoleName { get; set; }
		public string UserName { get; set; }

		public List<SelectListItem> UserDropDown { get; set; }
		
		public List<SelectListItem> RoleDropDown { get; set; }
	}

	public class AllRoleAndUser
	{
		public string RoleName { get; set; }

		public string Email { get; set; }

		public List<AllRoleAndUser> AllDetailsUserList { get; set; }
	}
}