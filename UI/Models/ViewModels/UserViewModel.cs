using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using UI.Helpers;
using UI.Models.Paging;

namespace UI.Models.ViewModels
{
	public class UserViewModel
	{
		private readonly short _page;
		private List<UserProfile> _userList;
		public UserPagingInfo UserPagingInfo { get; set; }
		public int PageSize = int.Parse(WebConfigurationManager.AppSettings.Get("UserPageSize")); 
		public List<UserProfile> UserList { get { return _userList;} }
		public UserViewModel() { }

		public UserViewModel(short page = 1) : this()
		{
			_page = page;
			GetUserList();
		}

		private void GetUserList()
		{
			_userList = AccountHelper.GetUserList();
			SetPageInfo();
		}

		private void SetPageInfo()
		{
			UserPagingInfo pagingInfo = new UserPagingInfo();
			pagingInfo.CurrentPage = _page;
			pagingInfo.UsersPerPage = PageSize;

			if (_userList != null && _userList.Count > 0)
			{
				if (_page >= 1)
				{
					pagingInfo.TotalUsers = _userList.Count();
					UserPagingInfo = pagingInfo;
					_userList = _userList.Skip((_page - 1) * PageSize).Take(PageSize).ToList();
				}
			}
		}
	}

	public class UserEditModel
	{
		public UserProfile User { get; set; }
		public List<Role> RoleList { get; set; }

		public string[] SelectedRoles { get; set; }

		public UserEditModel()
		{
			RoleList = AccountHelper.GetRoleList();
		}
		public UserEditModel(int userId) : this()
		{
			User = AccountHelper.GetUserFromId(userId);
		}
	}

	public class UserDeleteModel
	{
		public UserProfile User { get; set; }
		//public string[] RoleList { get; set; }
		public UserDeleteModel() { }

		public UserDeleteModel(int userId) : this()
		{
			User = AccountHelper.GetUserFromId(userId);
			//RoleList = Roles.GetRolesForUser(User.UserName);
		}
	}
}