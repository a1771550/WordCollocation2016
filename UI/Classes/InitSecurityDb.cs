using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using UI.Models;
using WebMatrix.WebData;

namespace UI.Classes
{
	public class InitSecurityDb : DropCreateDatabaseIfModelChanges<UsersContext>
	{
		protected override void Seed(UsersContext context)
		{

			WebSecurity.InitializeDatabaseConnection("WcUser",
			   "UserProfile", "UserId", "UserName", autoCreateTables: true);
			var roles = (SimpleRoleProvider)Roles.Provider;
			var membership = (SimpleMembershipProvider)Membership.Provider;

			if (!roles.RoleExists("Admin"))
			{
				roles.CreateRole("Admin");
			}
			if (membership.GetUser("test", false) == null)
			{
				Dictionary<string, object> dict = new Dictionary<string, object>();
				dict.Add("Email", "test@test.com");
				membership.CreateUserAndAccount("test", "111111", false, dict);
			}
			if (!roles.GetRolesForUser("test").Contains("Admin"))
			{
				roles.AddUsersToRoles(new[] { "test" }, new[] { "admin" });
			}

		}
	}
}