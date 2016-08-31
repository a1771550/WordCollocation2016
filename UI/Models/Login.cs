using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
	public class Login
	{
		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "PasswordRequired")]
		[StringLength(100, ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "PasswordLengthError", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(ResourceType = typeof(THResources.Resources), Name = "Password")]
		public string Password { get; set; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "EmailRequired")]
		[Display(ResourceType = typeof(THResources.Resources), Name = "Email")]
		public string Email { get; set; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "RememberMe")]
		[Display(ResourceType = typeof(THResources.Resources), Name = "RememberMe")]
		public bool RememberMe { get; set; }
	}
}