using System;
using UI.Models.Paging.Abstract;

namespace UI.Models.Paging
{
	public class UserPagingInfo:PagingInfoBase
	{
		public int TotalUsers { get; set; }
		public int UsersPerPage { get; set; }

		public override int TotalPages { get { return (int)Math.Ceiling((decimal)TotalUsers / UsersPerPage); } }
	}
}