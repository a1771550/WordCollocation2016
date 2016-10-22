using System;
using System.ComponentModel.DataAnnotations;
//using Resources = THResources.Resources;

namespace UI.Models.Abstract
{
	public abstract class WcBase
	{
		[Key]
		public virtual string Id { get; set; }
		[Required(ErrorMessage = "Please enter an entry")]
		public virtual string Entry { get; set; }

		[Required(ErrorMessageResourceType = typeof (THResources.Resources), ErrorMessageResourceName = "EntryZhtRequired")]
		[Display(Name = "條目")]
		public virtual string EntryZht { get; set; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "EntryZhsRequired")]
		[Display(Name = "条目")]
		public virtual string EntryZhs { get; set; }

		[Required(ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "EntryJapRequired")]
		[Display(Name = "エントリー")]
		public virtual string EntryJap { get; set; }

		public virtual DateTime RowVersion { get; set; }

		public virtual bool? CanDel { get; set; }
	}

	public enum ModelType
	{
		Pos = 1,
		ColPos = 2,
		Word = 3,
		ColWord = 4,
		Collocation = 5,
		Example = 6,
		User = 7,
		Role = 8
	}
}
