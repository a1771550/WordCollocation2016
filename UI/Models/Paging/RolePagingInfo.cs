using System;
using UI.Models.Paging.Abstract;

namespace UI.Models.Paging
{
	public class RolePagingInfo:PagingInfoBase
	{
		public int TotalRoles { get; set; }
		public int RolesPerPage { get; set; }

		public override int TotalPages { get { return (int)Math.Ceiling((decimal)TotalRoles / RolesPerPage); } }
	}
}