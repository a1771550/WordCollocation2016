using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Web.Configuration;

namespace UI.Models
{
	public class UsersContext : DbContext
	{
		public UsersContext()
			: base("WordCollocation")
		{
		}

		public DbSet<UserProfile> UserProfiles { get; set; }
	}

	public class UsersRepository
	{
		public bool ChangeUserName(int userId)
		{
			return false;
		}
	}
	public class Role
	{
		[Key]
		public int RoleId { get; set; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "RoleNameRequired")]
		[Display(ResourceType = typeof(THResources.Resources), Name = "RoleName")]
		public string RoleName { get; set; }
	}

	[Table("UserProfile")]
	public class UserProfile
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserId { get; set; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "UserNameRequired")]
		[Display(ResourceType = typeof(THResources.Resources), Name = "UserName")]
		public string UserName { get; set; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "EmailRequired")]
		[Display(ResourceType = typeof(THResources.Resources), Name = "Email")]
		[DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "EmailFormatError")]
		public string Email { get; set; }
	}

	public class RegisterExternalLoginModel
	{
		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }

		public string ExternalLoginData { get; set; }
	}

	public class LocalPasswordModel
	{
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Current password")]
		public string OldPassword { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "New password")]
		public string NewPassword { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm new password")]
		[Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }
	}

	public class LoginModel
	{
		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "UserNameRequired")]
		[Display(ResourceType = typeof(THResources.Resources), Name = "UserName")]
		public string UserName { get; set; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "PasswordRequired")]
		[StringLength(100, ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "PasswordLengthError", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(ResourceType = typeof(THResources.Resources), Name = "Password")]
		public string Password { get; set; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "RememberMe")]
		[Display(ResourceType = typeof(THResources.Resources), Name = "RememberMe")]
		public bool RememberMe { get; set; }

		public bool IsConfirmed { get; set; }
	}

	public class RegisterModel
	{
		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "UserNameRequired")]
		[Display(ResourceType = typeof(THResources.Resources), Name = "UserName")]
		public string UserName { get; set; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "EmailRequired")]
		[Display(ResourceType = typeof(THResources.Resources), Name = "Email")]
		[DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "EmailFormatError")]
		public string Email { get; set; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "PasswordRequired")]
		[StringLength(100, ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "PasswordLengthError", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(ResourceType = typeof(THResources.Resources), Name = "Password")]
		public string Password { get; set; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "ConfirmPasswordRequired")]
		[DataType(DataType.Password)]
		[Display(ResourceType = typeof(THResources.Resources), Name = "ConfirmPassword")]
		[Compare("Password", ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "ConfirmPasswordError")]
		public string ConfirmPassword { get; set; }
	}

	public class ResetPasswordModel
	{
		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "EmailRequired")]
		[Display(ResourceType = typeof(THResources.Resources), Name = "Email")]
		[DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "EmailFormatError")]
		public string Email { get; set; }

	}

	public class ResetPasswordConfirmModel
	{

		public string Token { get; set; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "NewPasswordRequired")]
		[StringLength(100, ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "PasswordLengthError", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(ResourceType = typeof(THResources.Resources), Name = "NewPassword")]
		public string NewPassword { get; set; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "ConfirmNewPasswordRequired")]
		[DataType(DataType.Password)]
		[Display(ResourceType = typeof(THResources.Resources), Name = "ConfirmNewPassword")]
		[Compare("NewPassword", ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "ConfirmNewPasswordError")]
		public string ConfirmNewPassword { get; set; }
	}
	public class ExternalLogin
	{
		public string Provider { get; set; }
		public string ProviderDisplayName { get; set; }
		public string ProviderUserId { get; set; }
	}

	public struct UserErrMsg
	{
		public string PwdRequired;
		public string ConfirmPwdRequired;
		public string PwdLenErr;
		public string ConfirmPwdErr;
		public string EmailRequired;
		public string EmailErr;
		public string EmailTaken;
		public string UserNameRequired;
		public string UserNameTaken;
		public string FirstNameRequired;
		public string LastNameRequired;
	}

	public struct RoleErrMsg
	{
		public string RoleNameRequired;
	}
}
