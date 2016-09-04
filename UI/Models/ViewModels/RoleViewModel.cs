using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using UI.Helpers;
using UI.Models.Paging;

namespace UI.Models.ViewModels
{
	public class RoleViewModel
	{
		private readonly short _page;
		private List<Role> _roleList;

		public Role Role { get; set; }
		//public Role SelectedRole { get; set; }

		public RolePagingInfo RolePagingInfo { get; set; }
		public int PageSize { get { return int.Parse(WebConfigurationManager.AppSettings.Get("RolePageSize")); } }
		public RoleViewModel() { }

		public RoleViewModel(short page = 1) : this()
		{
			_page = page;
			GetRoleList();
		}

		private void GetRoleList()
		{
			_roleList = AccountHelper.GetRoleList();
			SetPageInfo();
		}

		private void SetPageInfo()
		{
			RolePagingInfo pagingInfo = new RolePagingInfo();
			pagingInfo.CurrentPage = _page;
			pagingInfo.RolesPerPage = PageSize;

			if (_roleList != null && _roleList.Count > 0)
			{
				if (_page >= 1)
				{
					pagingInfo.TotalRoles = _roleList.Count();
					RolePagingInfo = pagingInfo;
					_roleList = _roleList.Skip((_page - 1) * PageSize).Take(PageSize).ToList();
				}
			}
		}
	}

	public class RoleEditModel
	{
		public Role Role { get; set; }

		public RoleEditModel() { }
		public RoleEditModel(int roleId) : this()
		{
			string name = AccountHelper.GetRoleNameById(roleId);
			Role = new Role { RoleId = roleId, RoleName = name };
		}
	}
}