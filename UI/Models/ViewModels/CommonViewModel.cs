using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UI.Models.Abstract;
using UI.Models.Misc;
using UI.Models.Paging;
using UI.Models.WcRepo;
using UI.Models.WC;

namespace UI.Models.ViewModels
{
	public class CommonViewModel : CommonViewModelBase
	{
		private readonly ModelType entity;
		public ModelType Entity { get { return entity; } }
		private readonly string selectedId;

		// don't change the following code!!
		private List<UI.Models.WcRepo.PosRepository.Pos> _PosList;

		//private List<ColPos> _ColPosList;
		private List<Word> _WordList;
		//private List<ColWord> _ColWordList;
		private readonly int page;
		public CommonPagingInfo PagingInfo { get; set; }

		// don't change the following code!!
		public List<UI.Models.WcRepo.PosRepository.Pos> PosList { get { return _PosList; } }

		public List<Word> WordList { get { return _WordList; } }
		//private List<Role> _roleList;
		//public List<Role> RoleList { get { return _roleList;} } 
		//public AssignRoleVM AssignRoleVM { get; set; }

		public int PageSize = SiteConfiguration.WcViewPageSize;

		//public CommonViewModel()
		//{
		//	AssignRoleVM = new AssignRoleVM();
		//}

		public CommonViewModel(int page = 1)
		{
			this.page = page;
		}

		public CommonViewModel(string selectedId = null)
		{
			this.selectedId = selectedId;
		}

		public CommonViewModel(ModelType entity, string selectedId = null)
			: this(selectedId)
		{
			this.entity = entity;
		}

		public CommonViewModel(ModelType entity, int page = 1)
			: this(page)
		{
			this.entity = entity;
			SetCommonList();
			if (entity != ModelType.Pos && entity != ModelType.ColPos && entity != ModelType.Role)
				SetPageInfo();
		}

		private void SetCommonList()
		{
			switch (entity)
			{
				//case ModelType.Role:

				//	AccountHelper.InitRoleList(ref AssignRoleVM);
				//	break;
				case ModelType.ColPos:
				case ModelType.Pos:
					PosRepository db = new PosRepository();
					_PosList = db.GetList();
					break;
				//case ModelType.ColPos:
				//	cprepo = new ColPosRepository();
				//	_ColPosList = cprepo.GetList();
				//	break;
				case ModelType.Word:
					//wrepo = new WordRepository();
					//_WordList = wrepo.GetList();
					break;
					//case ModelType.ColWord:
					//	cwrepo = new ColWordRepository();
					//	_ColWordList = cwrepo.GetList();
					//	break;

			}
		}
		private void SetPageInfo()
		{
			CommonPagingInfo pagingInfo = new CommonPagingInfo();
			pagingInfo.CurrentPage = page;
			pagingInfo.EntitiesPerPage = PageSize;

			switch (entity)
			{
				case ModelType.Word:
					//_ColWordList = null;
					setPagingDetails(ref _WordList, pagingInfo);
					break;
					//case ModelType.ColWord:
					//	_WordList = null;
					//	setPagingDetails(ref _WordList, ref _ColWordList, pagingInfo);
					//	break;
			}

		}

		private void setPagingDetails(ref List<Word> wlist, CommonPagingInfo pagingInfo)
		{
			if (wlist != null && wlist.Count > 0)
			{
				if (page < 1) return;
				pagingInfo.TotalWords = wlist.Count();
				PagingInfo = pagingInfo;
				wlist = wlist.Skip((page - 1) * PageSize).Take(PageSize).ToList();
			}
			//else if (cwlist != null && cwlist.Count > 0)
			//{
			//	if (page < 1) return;
			//	pagingInfo.TotalWords = cwlist.Count();
			//	PagingInfo = pagingInfo;
			//	cwlist = cwlist.Skip((page - 1) * PageSize).Take(PageSize).ToList();
			//}
		}

		public List<SelectListItem> CommonDropDownList
		{
			get
			{
				return CreateDropDownList(entity, selectedId);
			}
		}
	}
}