using System.ComponentModel.DataAnnotations;
using UI.Models.Abstract;

namespace UI.Models.ViewModels
{
	public class ContactViewModel
	{
		[Display(ResourceType = typeof (THResources.Resources), Name = "Name")]
		[Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(THResources.Resources))]
		public string Name { get; set; }

		[Display(ResourceType = typeof(THResources.Resources), Name = "Email")]
		[Required(ErrorMessageResourceName = "EmailRequired", ErrorMessageResourceType = typeof(THResources.Resources))]
		public string Email { get; set; }

		[Display(ResourceType = typeof(THResources.Resources), Name = "Opinion")]
		[Required(ErrorMessageResourceName = "OpinionRequired", ErrorMessageResourceType = typeof(THResources.Resources))]
		public string MessageText { get; set; }
	}
}