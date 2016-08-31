using System.ComponentModel.DataAnnotations;
using CommonLib.Helpers;

namespace UI.Models
{
	public class Register
	{
		public Register()
		{
			if (TextHelper.IsChinese(FirstName) && TextHelper.IsChinese(LastName))
				FullName = FirstName + LastName;
			else FullName = FirstName + " " + LastName;
		}

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "FirstNameRequired")]
		[Display(ResourceType = typeof(THResources.Resources), Name = "FirstName")]
		public string FirstName { get; set; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "LastNameRequired")]
		[Display(ResourceType = typeof(THResources.Resources), Name = "LastName")]
		public string LastName { get; set; }

		public string FullName { get; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "DisplayNameError")]
		[Display(ResourceType = typeof(THResources.Resources), Name="DisplayName")]
		public string DisplayName { get; set; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "EmailRequired")]
		[Display(ResourceType = typeof(THResources.Resources), Name="Email")]
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
}