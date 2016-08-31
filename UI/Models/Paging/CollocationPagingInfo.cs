using System;
using UI.Models.Paging.Abstract;

namespace UI.Models.Paging
{
	public class CollocationPagingInfo:PagingInfoBase
	{
		public int TotalCollocations { get; set; }
		public int CollocationsPerPage { get; set; }

		public override int TotalPages { get { return (int) Math.Ceiling((decimal) TotalCollocations/CollocationsPerPage); } }
	}
}