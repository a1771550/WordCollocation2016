using System;
using UI.Models.Paging.Abstract;

namespace UI.Models.Paging
{
	public class ExamplePagingInfo:PagingInfoBase
	{
		public int TotalExamples { get; set; }
		public int ExamplesPerPage { get; set; }

		public override int TotalPages { get { return (int)Math.Ceiling((decimal)TotalExamples / ExamplesPerPage); } }
	}
}