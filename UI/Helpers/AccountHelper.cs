using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Dapper;
using log4net;
using UI.Models;
using UI.Models.ViewModels;

namespace UI.Helpers
{
	public static class AccountHelper
	{
		static readonly string dbPrefix = WebConfigurationManager.AppSettings.Get("DbPrefix");
		static readonly string conn = WebConfigurationManager.ConnectionStrings["WordCollocation"].ConnectionString;
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private static string GetQueryNameWithPrefix(string spname)
		{
			return string.Format("{0}{1}", dbPrefix, spname);
		}

		public static string GetRoleNameById(int roleId)
		{
			//string sql = string.Format("{0}GetRoleNameById", dbPrefix);
			string sql = GetQueryNameWithPrefix("GetRoleNameById");
			SqlParameter[] sqlParameters = new SqlParameter[1];
			SqlParameter roleIdParam = new SqlParameter();
			roleIdParam.ParameterName = "roleId";
			roleIdParam.Value = roleId;
			sqlParameters[0] = roleIdParam;
			return (string)DAL.DataAccess.ExecuteScalar(conn, sql, CommandType.StoredProcedure, sqlParameters);
		}

		public static bool UpdateRole(Role role)
		{
			//string conn = WebConfigurationManager.ConnectionStrings["WcUser"].ConnectionString;
			//string sql = string.Format("{0}UpdateRole", dbPrefix);
			string sql = GetQueryNameWithPrefix("UpdateRole");
			SqlParameter[] sqlParameters = new SqlParameter[2];
			SqlParameter roleIdParam = new SqlParameter();
			roleIdParam.ParameterName = "roleId";
			roleIdParam.Value = role.RoleId;
			sqlParameters[0] = roleIdParam;
			SqlParameter roleNameParam = new SqlParameter();
			roleNameParam.ParameterName = "roleName";
			roleNameParam.Value = role.RoleName;
			sqlParameters[1] = roleNameParam;
			return (DAL.DataAccess.ExecuteNonQuery(conn, sql, CommandType.StoredProcedure, sqlParameters) > 0);
		}

		public static bool UpdateUser(UserEditModel model)
		{
			UsersContext db = new UsersContext();
			//using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			//{
			try
			{
				db.Entry(model.User).State = EntityState.Modified;
				db.SaveChanges();

				string[] currentRoles = Roles.GetRolesForUser(model.User.UserName);
				if (currentRoles.Length > 0)
					Roles.RemoveUserFromRoles(model.User.UserName, currentRoles);

				foreach (string role in model.SelectedRoles)
					Roles.AddUserToRole(model.User.UserName, role);
				//scope.Complete();
				return true;
			}
			//catch (OptimisticConcurrencyException)
			//{
			//	var ctx = ((IObjectContextAdapter)db).ObjectContext;
			//	ctx.Refresh(RefreshMode.ClientWins, db.UserProfiles);
			//	ctx.SaveChanges();
			//	return true;
			//}
			//catch (TransactionException ex)
			//{
			//	log.Error(ex.Message, ex);
			//	throw new Exception(ex.Message);
			//}
			catch (Exception ex)
			{
				log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}
			//}
		}

		public static List<UserProfile> GetUserList()
		{
			UsersContext db = new UsersContext();
			return db.UserProfiles.ToList();
		}

		public static List<Role> GetRoleList()
		{
			string sql = GetQueryNameWithPrefix("GetRoles");
			DataSet ds = DAL.DataAccess.CreateDataSet(conn, sql, CommandType.StoredProcedure);
			DataRowCollection rows = ds.Tables[0].Rows;
			if (rows.Count > 0)
			{
				List<Role> roleList = new List<Role>();
				foreach (DataRow row in rows)
				{
					Role role = new Role();
					role.RoleId = (int)row["RoleId"];
					role.RoleName = (string)row["RoleName"];
					roleList.Add(role);
				}
				return roleList;
			}
			return null;
		}

		public static void DeleteUser(int userId)
		{
			try
			{
				UserProfile user = GetUserFromId(userId);
				string[] rolenames = Roles.GetRolesForUser(user.UserName);
				Roles.RemoveUserFromRoles(user.UserName, rolenames);
				UsersContext db = new UsersContext();
				db.Entry(user).State = EntityState.Deleted;
				db.SaveChanges();
			}
			catch (Exception ex)
			{
				log.Error(ex.Message);
				throw new Exception(ex.Message);
			}
		}

		public static UserProfile GetUserFromId(int userId)
		{
			UsersContext db = new UsersContext();
			return db.UserProfiles.FirstOrDefault(u => u.UserId == userId);
		}

		public static List<string> GetRoleListForUser(string username)
		{
			var userRoles = Roles.GetRolesForUser(username);
			return userRoles.ToList();
		}

		public static UserProfile GetUserFromName(string username)
		{
			UsersContext db = new UsersContext();
			return
				db.UserProfiles.FirstOrDefault(u => string.Equals(u.UserName, username, StringComparison.CurrentCultureIgnoreCase));
		}

		public static string GetEmailFromUser(string username)
		{
			UsersContext db = new UsersContext();
			UserProfile user =
				db.UserProfiles.FirstOrDefault(u => string.Equals(u.UserName, username, StringComparison.CurrentCultureIgnoreCase));
			if (user != null)
				return user.Email;
			return null;
		}

		public static string GetUserNameFromEmail(string email)
		{
			UsersContext db = new UsersContext();
			UserProfile user = db.UserProfiles.FirstOrDefault(u => u.Email.ToLower().Equals(email.ToLower()));
			if (user != null)
				return user.UserName;
			return null;
		}

		public static bool CheckIfUserEmailConfirmed(string email)
		{
			UsersContext db = new UsersContext();
			UserProfile user = db.UserProfiles.FirstOrDefault(u => u.Email.ToLower().Equals(email.ToLower()));
			if (user != null)
			{
				string sql = GetQueryNameWithPrefix("CheckIfUserEmailConfirmed");
				SqlParameter[] sqlParameters = new SqlParameter[1];
				SqlParameter userIdParam = new SqlParameter();
				userIdParam.ParameterName = "UserId";
				userIdParam.Value = user.UserId;
				sqlParameters[0] = userIdParam;
				return (bool)DAL.DataAccess.ExecuteScalar(conn, sql, CommandType.StoredProcedure, sqlParameters);
			}
			return false;
		}

		public static string[] GetRoleListByUserId(int userId)
		{
			string sql = GetQueryNameWithPrefix("GetRoleListByUserId");
			SqlParameter[] sqlParameters = new SqlParameter[1];
			SqlParameter userIdParam = new SqlParameter();
			userIdParam.ParameterName = "UserId";
			userIdParam.Value = userId;
			sqlParameters[0] = userIdParam;
			DataSet ds = DAL.DataAccess.CreateDataSet(conn, sql, CommandType.StoredProcedure, null, sqlParameters);
			DataRowCollection rows = ds.Tables[0].Rows;
			string[] roleList = new string[rows.Count];
			if (rows.Count > 0)
			{
				int index = 0;
				foreach (DataRow row in rows)
				{
					string rolename = (string)row["RoleName"];
					roleList[index] = rolename;
					index++;
				}
			}
			return roleList;
		}

		public static string GetConfirmTokenFromUser(int userId)
		{
			string cmd = "select ConfirmationToken from webpages_Membership where UserId = " + userId;
			UsersContext db = new UsersContext();
			return db.Database.SqlQuery<string>(cmd).FirstOrDefault();
		}

		public static void InitUserErrMsgs(out UserErrMsg msg)
		{
			msg = new UserErrMsg();
			msg.PwdRequired = THResources.Resources.PasswordRequired;
			msg.ConfirmPwdRequired = THResources.Resources.ConfirmPasswordRequired;
			msg.PwdLenErr = THResources.Resources.PasswordLengthError;
			msg.ConfirmPwdErr = THResources.Resources.ConfirmPasswordError;
			msg.EmailRequired = THResources.Resources.EmailRequired;
			msg.EmailErr = THResources.Resources.EmailFormatError;
			msg.EmailTaken = THResources.Resources.EmailTaken;
			msg.FirstNameRequired = THResources.Resources.FirstNameRequired;
			msg.LastNameRequired = THResources.Resources.LastNameRequired;
			msg.UserNameTaken = THResources.Resources.UserNameTaken;
			msg.UserNameRequired = THResources.Resources.UserNameRequired;
		}

		public static void InitRoleErrMsgs(out RoleErrMsg msg)
		{
			msg = new RoleErrMsg();
			msg.RoleNameRequired = THResources.Resources.RoleNameRequired;
		}

		public static string GetEmailFromAuthCookie()
		{
			var httpCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
			if (httpCookie != null)
			{
				var formsAuthenticationTicket = FormsAuthentication.Decrypt(httpCookie.Value);
				return formsAuthenticationTicket?.Name;
			}
			return null;
		}

		public static void InitUserDropDown(ref AssignRoleVM objvm)
		{
			using (
				SqlConnection con = new SqlConnection(conn))
			{
				string sql = string.Format("Select * From {0}UserProfile", dbPrefix);
				var userList = con.Query(sql).ToList();
				List<SelectListItem> users = new List<SelectListItem>();
				users.Add(new SelectListItem { Text = "Select", Value = "0" });
				foreach (var item in userList)
				{
					users.Add(new SelectListItem { Text = item.UserName, Value = item.UserName });
				}
				objvm.UserDropDown = users;
			}
		}

		public static void InitRoleDropDown(ref AssignRoleVM objvm)
		{
			List<SelectListItem> rolelist = new List<SelectListItem>();
			rolelist.Add(new SelectListItem { Text = "Select", Value = "0" });
			foreach (var item in Roles.GetAllRoles())
			{
				rolelist.Add(new SelectListItem { Text = item, Value = item });
			}
			objvm.RoleDropDown = rolelist;
		}

		public static bool CheckIfDuplicatedEmail(string email)
		{
			UsersContext db = new UsersContext();
			return db.UserProfiles.Any(u => u.Email.ToLower().Equals(email.ToLower()));
		}

		public static string GetUserNameById(int userId)
		{
			UsersContext db = new UsersContext();
			var firstOrDefault = db.UserProfiles.FirstOrDefault(u => u.UserId == userId);
			if (firstOrDefault != null)
				return firstOrDefault.UserName;
			return null;
		}
	}
}