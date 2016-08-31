using System.ComponentModel.DataAnnotations;

namespace BLL.Abstract
{
	public abstract class UserRoleBase
	{
		//[Key]
		//[ScaffoldColumn(false)]
		public int Id { get; set; }

		//[Required(ErrorMessageResourceType = typeof (Resources), ErrorMessageResourceName = "NameRequired")]
		public string Name { get; set; }
		public byte[] RowVersion { get; set; }

	}
}
