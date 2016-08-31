using System.ComponentModel.DataAnnotations;

namespace UI.Models.ViewModels
{
	public class ValidateDemoViewModel
	{
		//[Required(ErrorMessage = "Please enter your name")]
		//[Display(Name="User Name")]
		//public string Name { get; set; }
		[Required(ErrorMessageResourceName = "UserNameEmailRequired", ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessage = null)]
		[DataType(DataType.Text)]
		[Display(Name = "UserNameEmailForLogin", ResourceType = typeof(THResources.Resources))]
		public string UserNameEmail { get; set; }
		

		//[Required(ErrorMessage="Please enter password")]
		//[Display(Name="Password")]
		//[DataType(DataType.Password)]
		//public string Password { get; set; }
		[Required(ErrorMessageResourceName = "PasswordRequired", ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessage = null)]
		[DataType(DataType.Password)]
		[Display(Name = "Password", ResourceType = typeof(THResources.Resources))]
		public string Password { get; set; }
	}
}