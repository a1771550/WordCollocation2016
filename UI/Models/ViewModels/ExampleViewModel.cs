using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UI.Controllers.Abstract;
using UI.Models.Abstract;
using UI.Models.Misc;
using UI.Models.Paging;
using UI.Models.WC;

namespace UI.Models.ViewModels
{
	public class ExampleViewModel : WcManageViewModelBase
	{
		private readonly long collocationId;
		public long CollocationId { get { return collocationId; } }
		public bool NeedRemark { get; set; }
		//public override bool NeedRemark { get; set; }
		private readonly ExampleRepository repo = new ExampleRepository();
		private readonly Example _example;
		private List<IGrouping<long, Example>> _exampleListInGroup;
		private readonly int page;
		public ExamplePagingInfo ExamplePagingInfo { get; set; }
		public List<Example> ExampleList { get; }
		public List<IGrouping<long, Example>> ExampleListInGroup { get { return _exampleListInGroup; } }

		public int PageSize = SiteConfiguration.WcViewPageSize;
		public Example Example { get { return _example; } }

		//private readonly CreateMode createMode;
		//public CreateMode CreateMode { get { return createMode; } }

		//public ExampleViewModel()
		//{
		//	NeedRemark = false;
		//}

		public ExampleViewModel(long id, CreateMode mode)
		{
			switch (mode)
			{
				case CreateMode.Collocation:
					collocationId = id;
					break;
				case CreateMode.Example:
					_example = repo.GetById(id.ToString());
					break;
			}
			ExampleList = new List<Example> { _example };
		}

		public ExampleViewModel(int page = 1)
		{
			this.page = page;
			GetExampleListInGroup();
		}

		public ExampleViewModel(Example example)
		{
			ExampleList = new List<Example> { example };
		}
		private void GetExampleListInGroup()
		{
			_exampleListInGroup = repo.GetListInGroup();
			SetPageInfo();
		}
		private void SetPageInfo()
		{
			ExamplePagingInfo pagingInfo = new ExamplePagingInfo();
			pagingInfo.CurrentPage = page;
			pagingInfo.ExamplesPerPage = PageSize;

			if (_exampleListInGroup != null && _exampleListInGroup.Count > 0)
			{
				if (page >= 1)
				{
					pagingInfo.TotalExamples = _exampleListInGroup.Count();
					ExamplePagingInfo = pagingInfo;
					_exampleListInGroup = _exampleListInGroup.Skip((page - 1) * PageSize).Take(PageSize).ToList();
				}
			}
		}

		public List<SelectListItem> CollocationIdDropDownList
		{
			get
			{
				var ddlCollocation = new List<SelectListItem>();
				ddlCollocation.Add(new SelectListItem { Selected = collocationId == 0, Text = string.Format("- {0} -", THResources.Resources.WordCollocation), Value = "0" });
				var crepo = new CollocationRepository();
				List<Collocation> cList = crepo.GetList();

				ddlCollocation.AddRange(from entity in cList let isSelected = entity.Id.ToString().Equals(collocationId.ToString(), StringComparison.OrdinalIgnoreCase) select new SelectListItem { Text = entity.Id.ToString(), Value = entity.Id.ToString(), Selected = isSelected });

				return ddlCollocation;
			}
		}
	}
}