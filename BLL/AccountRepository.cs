using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web;
using System.Web.Configuration;
using System.Web.Management;
using BLL.Helpers;
using CommonLib.Helpers;
using DAL;
using log4net;

namespace BLL
{
	public class AccountRepository
	{
		private readonly AccountEntities _db = new AccountEntities();
		private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		readonly string _connString = WebConfigurationManager.ConnectionStrings["WordCollocation"].ConnectionString;
		private string _sql;

		public User GetUserById(int id)
		{
			try
			{
				return _db.Users.Find(id);
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}
		}
		
		public User GetUserByNameEmail(string nameEmail)
		{
			try
			{
				Func<User, bool> func = u =>
					string.Equals(u.Email, nameEmail, StringComparison.CurrentCultureIgnoreCase) ||
					string.Equals(u.FirstName, nameEmail, StringComparison.CurrentCultureIgnoreCase) || string.Equals(u.LastName, nameEmail, StringComparison.CurrentCultureIgnoreCase) || string.Equals(u.FirstName + " " + u.LastName, nameEmail, StringComparison.CurrentCultureIgnoreCase) ||
					string.Equals(u.LastName + " " + u.FirstName, nameEmail, StringComparison.CurrentCultureIgnoreCase);
				if (_db.Users.Any(func))
				{
					User user = _db.Users.Single(func);
					GetRoleListFromUserInRoles(ref user);
					return user;
				}
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, ex);
				//throw new Exception(ex.Message);
				HttpContext.Current.Session["ErrorViewModel"] = ErrorHelper.CreatErrorViewModel(ex);
				HttpContext.Current.Response.RedirectToRoute("CustomError");
			}
			return null;
		}

		public void GetRoleListFromUserInRoles(ref User user)
		{
			var roleList = _db.GetRolesByEmail(user.Email);
			var roles = roleList.ToList();
			if (roles.Any())
			{
				foreach (var role in roles)
				{
					UserInRole ur = new UserInRole();
					Role r = new Role();
					r.Code = role.Code;
					r.Name = role.Name;
					ur.User = user;
					ur.Role = r;
					user.UserInRoles.Add(ur);
				}
			}
			
		}

		public User GetUserByName(string userName)
		{
			try
			{
				Func<User, bool> func = u =>

					string.Equals(u.FirstName, userName, StringComparison.CurrentCultureIgnoreCase) || string.Equals(u.LastName, userName, StringComparison.CurrentCultureIgnoreCase) || string.Equals(u.FirstName + " " + u.LastName, userName, StringComparison.CurrentCultureIgnoreCase) ||
					string.Equals(u.LastName + " " + u.FirstName, userName, StringComparison.CurrentCultureIgnoreCase);

				if (_db.Users.Any(func))
				{
					User user = _db.Users.Single(func);
					GetRoleListFromUserInRoles(ref user);
					return user;
				}
				return null;

			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}
		}

		public User GetUserByEmail(string email)
		{
			try
			{
				var result = _db.GetUserByEmail(email).ToList();
				if (result.Any())
				{
					GetUserByEmail_Result[] resArr = result.ToArray();
					User user = new User();
					user.Email = resArr[0].Email;
					user.Password = TextHelper.Decrypt(resArr[0].Password,
						WebConfigurationManager.AppSettings.Get("SecurityKey"), WebConfigurationManager.AppSettings.Get("KeyFileName"));
					user.FirstName = resArr[0].FirstName;
					user.LastName = resArr[0].LastName;
					user.RowVersion = resArr[0].RowVersion;
					GetRoleListFromUserInRoles(ref user);
					return user;
				}
				return null;
			}
			catch (SqlException ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}
			catch (SqlExecutionException ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}
		}

		public List<User> GetUserList()
		{
			try
			{
				return _db.Users.ToList();
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}
		}

		public bool CreateUser(User user, string[] selectedRoles)
		{
			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					_sql = "INSERT INTO [dbo].[User]([Email],[LastName],[FirstName],[Password])VALUES(" + "'" + user.Email + "'" + "," + "'" + user.LastName + "'" + "," + "'" + user.FirstName + "'" + "," + "'" + user.Password + "'" + ")";
					DataAccess.ExecuteNonQuery(_connString, _sql);
					if (selectedRoles!=null && selectedRoles.Length > 0)
					{
						UpdateUsersInRole(user.Email, selectedRoles);
					}
					
					scope.Complete();
					return true;
				}
				catch (TransactionException ex)
				{
					_log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
				catch (OptimisticConcurrencyException)
				{
					var ctx = ((IObjectContextAdapter)_db).ObjectContext;
					ctx.Refresh(RefreshMode.ClientWins, _db.Users);
					int iRet = ctx.SaveChanges();
					return (iRet>0);
				}
				catch (Exception ex)
				{
					_log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
			}
		}

		public Role GetRoleById(int id)
		{
			try
			{
				return _db.Roles.Find(id);
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}
		}

		public List<Role> GetRoleList()
		{
			return _db.Roles.ToList();
		}
		
		public bool CreateRole(Role role)
		{
			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					_sql = "AddRole";
					SqlParameter name = new SqlParameter("@Name", SqlDbType.NVarChar, 30, "Name");
					name.Value = role.Name;
					SqlParameter code = new SqlParameter("@Code", SqlDbType.NVarChar, 2, "Code");
					code.Value = role.Code;
					SqlParameter[] parameters = new SqlParameter[2];
					parameters[0] = code;
					parameters[1] = name;
					DataAccess.ExecuteNonQuery(_connString, _sql, CommandType.StoredProcedure, parameters);
					_db.SaveChanges();
					scope.Complete();
					return true;
				}
				catch (TransactionException ex)
				{
					_log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
				catch (OptimisticConcurrencyException)
				{
					var ctx = ((IObjectContextAdapter)_db).ObjectContext;
					ctx.Refresh(RefreshMode.ClientWins, _db.Roles);
					int iRet = ctx.SaveChanges();
					return iRet >= 1;
				}
				catch (Exception ex)
				{
					_log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
			}
		}


		public bool UpdateUsersInRole(string email, string[] roleCodes)
		{
			try
			{
				_sql = "Delete from UserInRoles where Email='" + email + "'";
				int iRet = DataAccess.ExecuteNonQuery(_connString, _sql);

				if (roleCodes.Length > 0)
				{
					const string insertSql = "Insert into UserInRoles(Email,RoleCode) ";

					/*
					 * foreach (string roleCode in roleCodes)
					{
						unionSql += "Select " + "'" + email + "'" + ", " + "'" + roleCode + "'" + " Union All ";
					}
					 */

					string unionSql = roleCodes.Aggregate<string, string>(null, (current, roleCode) => current + ("Select " + "'" + email + "'" + ", " + "'" + roleCode + "'" + " Union All "));


					// remove the last 'Union All'
					if (unionSql != null)
					{
						int lastIndex = unionSql.LastIndexOf("Union All", StringComparison.Ordinal);
						unionSql = unionSql.Remove(lastIndex);
					}

					_sql = insertSql + unionSql;
					iRet = DataAccess.ExecuteNonQuery(_connString, _sql);
				}

				return (iRet >= 1);
			}
			catch (SqlException ex)
			{
				_log.Error(ex.Message, ex);
				throw;
			}
			catch (SqlExecutionException ex)
			{
				_log.Error(ex.Message, ex);
				throw;
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, ex);
				throw;
			}
		}

		public bool UpdateUser(User user)
		{
			try
			{
				using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
				{
					#region "Important Note"
					/* Don't use the following code, as it will incur error!!! */
					//_db.Entry(user).State = EntityState.Modified;
					//var bRet = (_db.SaveChanges() >= 1);
					/* ------------------------------------------------------- */
					#endregion	

					int iRet = _db.UpdateUser(user.Email, user.LastName, user.FirstName, user.Password, user.RegisterToken, user.RowVersion);
					scope.Complete();
					return (iRet>0);
				}

			}
			catch (TransactionException ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}
			catch (OptimisticConcurrencyException)
			{
				var ctx = ((IObjectContextAdapter)_db).ObjectContext;
				ctx.Refresh(RefreshMode.ClientWins, _db.Users);
				int iRet = ctx.SaveChanges();
				return (iRet >= 1);
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}
		}

		public bool UpdateRole(Role role)
		{
			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					_db.Entry(role).State = EntityState.Modified;
					int iRet = _db.SaveChanges();
					scope.Complete();
					return iRet >= 1;
				}
				catch (TransactionException ex)
				{
					_log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
				catch (OptimisticConcurrencyException)
				{
					var ctx = ((IObjectContextAdapter)_db).ObjectContext;
					ctx.Refresh(RefreshMode.ClientWins, _db.Roles);
					int iRet = ctx.SaveChanges();
					return iRet >= 1;
				}
				catch (Exception ex)
				{
					_log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
			}
		}

		public bool DeleteUser(int id)
		{
			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				int iRet;
				try
				{
					User user = _db.Users.Find(id);
					_db.Users.Remove(user);
					iRet = _db.SaveChanges();

					scope.Complete();
					return iRet >= 1;
				}
				catch (TransactionException ex)
				{
					_log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
				catch (Exception ex)
				{
					_log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
			}
		}

		public bool DeleteRole(int id)
		{
			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					Role role = _db.Roles.Find(id);
					_db.Roles.Remove(role);
					int iRet = _db.SaveChanges();
					scope.Complete();
					return iRet >= 1;
				}
				catch (TransactionException ex)
				{
					_log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
				catch (OptimisticConcurrencyException)
				{
					var ctx = ((IObjectContextAdapter)_db).ObjectContext;
					ctx.Refresh(RefreshMode.ClientWins, _db.Roles);
					int iRet = ctx.SaveChanges();
					return iRet >= 1;
				}
				catch (Exception ex)
				{
					_log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
			}
		}

		public bool CheckIfDuplicatedEmail(string email)
		{
			try
			{
				_sql = "CheckIfDuplicatedEmail";
				SqlParameter emailParam = new SqlParameter("@Email", SqlDbType.NVarChar, 255, "Email");
				emailParam.Value = email;
				SqlParameter count = new SqlParameter("@Count", SqlDbType.Int, Int32.MaxValue);
				count.Direction = ParameterDirection.Output;
				SqlParameter[] parameters = new SqlParameter[2];
				parameters[0] = emailParam;
				parameters[1] = count;
				DataAccess.ExecuteNonQuery(_connString, _sql, CommandType.StoredProcedure, parameters);
				return ((int)count.Value == 1);
			}
			catch (SqlException ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message, ex);
			}
			catch (SqlExecutionException ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message, ex);
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message, ex);
			}
		}



		public void ResetPassword(string email, string pwd)
		{
			try
			{
				_sql = "ResetPassword";
				SqlParameter emailParam = new SqlParameter("@Email", SqlDbType.NVarChar, 255, "Email");
				emailParam.Value = email;
				SqlParameter password = new SqlParameter("@Password", SqlDbType.NVarChar, 50, "Password");
				password.Value = pwd;
				SqlParameter[] parameters = new SqlParameter[2];
				parameters[0] = emailParam;
				parameters[1] = password;
				DataAccess.ExecuteNonQuery(_connString, _sql, CommandType.StoredProcedure, parameters);
			}
			catch (SqlException ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message, ex);
			}
			catch (SqlExecutionException ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message, ex);
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message, ex);
			}
		}



		public List<Role> GetRoleListByUserId(int userId)
		{
			_sql = "GetRoleListByUserId";
			SqlParameter idParam = new SqlParameter("@Id", SqlDbType.Int, Int32.MaxValue, "Id");
			idParam.Value = userId;
			SqlParameter[] parameters = new SqlParameter[1];
			parameters[0] = idParam;
			try
			{
				DataSet ds = DataAccess.CreateDataSet(_connString, _sql, CommandType.StoredProcedure, null, parameters);
				DataRowCollection rows = ds.Tables[0].Rows;

				List<Role> roles = new List<Role>();
				if (rows.Count > 0)
				{
					foreach (DataRow row in rows)
					{
						Role role = new Role();
						role.Code = (string)row["Code"];
						role.Name = (string)row["Name"];
						role.RowVersion = (byte[])row["RowVersion"];
						roles.Add(role);
					}
				}
				return roles;
			}
			catch (SqlException ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message, ex);
			}
			catch (SqlExecutionException ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message, ex);
			}
			catch (Exception ex)
			{
				_log.Error(ex.Message, ex);
				throw new Exception(ex.Message, ex);
			}
		}

		public Role GetRoleByCode(string roleCode)
		{
			var obj = _db.GetRoleByCode(roleCode).ToArray()[0];
			Role role = new Role();
			role.Code = roleCode;
			role.Name = obj.Name;
			role.RowVersion = obj.RowVersion;
			return role;
		}
	}

	public enum RoleType
	{
		Admin = 1,
		GeneralUser = 2,
	}
}
