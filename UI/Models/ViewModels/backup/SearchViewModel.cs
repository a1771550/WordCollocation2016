﻿using System.Collections.Generic;
using System.Web.Mvc;
using MyWcModel.Abstract;

namespace UI.Models.ViewModels
{
	//public enum ViewMode
	//{
	//	//Admin,
	//	Home,
	//	//PreSearchResult,
	//	SearchResult
	//}
	public class SearchViewModel
	{
		//private readonly WordcollocationEntities db = new WordcollocationEntities();
		public string ConnectionString { get; set; }

		//private const string letters = "A;B;C;D;E;F;G;H;I;J;K;L;M;N;O;P;Q;R;S;T;U;V;W;X;Y;Z";
		//public string[] Letters { get; set; }
		//public CollocationPagingInfo CollocationPagingInfo { get; set; }

		//public int PageSize = SiteConfiguration.WcColListPageSize;
		//private readonly int _page;
		//private readonly string _firstLetter;

		//private string _posJap, _posZht, _wordZht, _wordJap, _posZhs, _wordZhs;
		//private readonly ViewMode _mode;
		//private List<Collocation> _collocations;
		//private int _collocationCount;

		//public int Page;
		//public List<Collocation> CollocationList { get { return _collocations; } }

		public int CollocationCount { get; set; }
		public string PosZht { get; set; }
		public string PosZhs { get; set; }
		public string PosJap { get; set; }
		public string WordZht { get; set; }
		public string WordZhs { get; set; }
		public string WordJap { get; set; }
		public string Word { get; set; }
		public string Pos { get; set; }
		public string ColPos { get; set; }
		public string ColPosZht { get; set; }
		public string ColPosZhs { get; set; }
		public string ColPosJap { get; set; }
		public string ColPosId { get; set; }
		public string[] Pattern { get; set; }
		public string Action { get; set; }
		public string Controller { get; set; }
		//public ViewMode ViewMode { get { return _mode; } }
		public string ExampleLabelText { get { return THResources.Resources.ExampleText; } }
		public SearchViewModel()
		{
			Action = "SearchResult";
			Controller = "Home";
		}
		//public SearchViewModel(ViewMode mode)
		//	: this()
		//{
		//	_mode = mode;
		//}

		//public SearchViewModel(int collocationCount=1)
		//	: this()
		//{
		//	//_page = page;
		//	//CollocationCount = collocationCount;
		//	Action = "SearchResult";
		//	Controller = "Home";
		//	//GetCollocationList(false);
		//}

		//public SearchViewModel(ViewMode mode, string firstLetter = null, int page = 1)
		//	: this(mode, page)
		//{
		//	_firstLetter = !String.IsNullOrEmpty(firstLetter) ? firstLetter : "a";
		//	string[] lettterStrings = letters.Split(';');
		//	Letters = lettterStrings;
		//}

		//private void SetPageInfo()
		//{
		//	CollocationPagingInfo pagingInfo = new CollocationPagingInfo();
		//	pagingInfo.CurrentPage = _page;
		//	pagingInfo.CollocationsPerPage = PageSize;

		//	//if (_collocations != null && _collocations.Count > 0)
		//	//{
		//		if (_page >= 1)
		//		{
		//			pagingInfo.TotalCollocations = CollocationCount;
		//			CollocationPagingInfo = pagingInfo;
		//			_collocations = _collocations.Skip((_page - 1) * PageSize).Take(PageSize).ToList();
		//		}
		//	//}
		//}

		//private void GetCollocationList(bool setPageInfo = true)
		//{
		//	//var repo = new CollocationRepository();

		//	switch (_mode)
		//	{
		//		//case ViewMode.Admin:
		//		//	_collocations = repo.GetCollocationListInGroup(_firstLetter);
		//		//	Action = "Index";
		//		//	Controller = "Admin";
		//		//	break;
		//		case ViewMode.Home:
		//			//colList = repo.GetCollocationListByWordColPosId(word, Id);
		//			break;
		//		//case ViewMode.PreSearchResult:
		//		//	Action = "PreSearchResult";
		//		//	Controller = "Home";
		//		//	break;
		//		case ViewMode.SearchResult:
		//			//_collocations = (List<Collocation>)HttpContext.Current.Session[HomeController.CollocationListSessionName];
		//			//if (_collocations != null)
		//			//{
		//			//	var collocation = _collocations[0];
		//			//	Word = collocation.word.Entry;
		//			//	Pos = collocation.word.pos.Entry;
		//			//	ColPos = collocation.colword.pos.Entry;
		//			//	_posJap = collocation.word.pos.EntryJap;
		//			//	_posZht = collocation.word.pos.EntryZht;
		//			//	_posZhs = collocation.word.pos.EntryZhs;
		//			//	_wordZht = collocation.word.EntryZht;
		//			//	_wordZhs = collocation.word.EntryZhs;
		//			//	_wordJap = collocation.word.EntryJap;
		//			//Pattern = MyWcModelHelper.GetPatternArray((CollocationPattern)collocation.CollocationPattern);
		//			//	var culturename = CultureHelper.GetCurrentCulture();
		//			//	if (culturename.Contains("hans"))
		//			//	{
		//			//		PosTrans = _posZhs;
		//			//		WordTrans = _wordZhs;
		//			//	}
		//			//	else if (culturename.Contains("ja"))
		//			//	{
		//			//		PosTrans = _posJap;
		//			//		WordTrans = _wordJap;
		//			//	}
		//			//	else
		//			//	{
		//			//		PosTrans = _posZht;
		//			//		WordTrans = _wordZht;
		//			//	}
		//			//}
		//			Action = "SearchResult";
		//			Controller = "Home";
		//			break;
		//	}

		//	if (setPageInfo) SetPageInfo();
		//}

		public List<SelectListItem> ColPosDropDownList
		{
			get { return new CommonViewModel(ModelType.ColPos, ColPosId).CommonDropDownList; }
		}
	}
}