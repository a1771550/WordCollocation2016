using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using MyWcModel;
using UI.Models.Abstract;
using UI.Models.Misc;
using UI.Models.Paging;
using UI.Models.WC;

namespace UI.Models.ViewModels
{
	public class CollocationViewModel : WcManageViewModelBase
	{
		private CollocationRepository repo;
		private List<Collocation> _CollocationList;
		private readonly int page;
		private readonly Collocation collocation;
		public CollocationPagingInfo CollocationPagingInfo { get; set; }

		public List<Collocation> CollocationList
		{
			get { return _CollocationList; }
		}

		public Collocation Collocation { get { return collocation; } }

		public int PageSize = SiteConfiguration.WcViewPageSize;

		public CollocationViewModel()
		{
		}

		public CollocationViewModel(Collocation collocation)
		{
			this.collocation = collocation;
		}

		//public CollocationViewModel(int page = 1)
		//	: this()
		//{
		//	this.page = page;
		//	GetCollocationList();
		//}

		//public CollocationViewModel(long id)
		//{
		//	repo = new CollocationRepository();
		//	collocation = repo.GetById(id.ToString());
		//}

		//private void GetCollocationList()
		//{
		//	repo = new CollocationRepository();
		//	_CollocationList = repo.GetList();
		//	SetPageInfo();
		//}

		//private void SetPageInfo()
		//{
		//	CollocationPagingInfo pagingInfo = new CollocationPagingInfo();
		//	pagingInfo.CurrentPage = page;
		//	pagingInfo.CollocationsPerPage = PageSize;

		//	if (_CollocationList != null && _CollocationList.Count > 0)
		//	{
		//		if (page >= 1)
		//		{
		//			pagingInfo.TotalCollocations = _CollocationList.Count();
		//			CollocationPagingInfo = pagingInfo;
		//			_CollocationList = _CollocationList.Skip((page - 1) * PageSize).Take(PageSize).ToList();
		//		}
		//	}
		//}

		public List<SelectListItem> PosDropDownList
		{
			get
			{
				var id = collocation == null ? null : collocation.Word.pos.Id.ToString();
				return new CommonViewModel(ModelType.Pos, id).CommonDropDownList;
			}
		}

		public List<SelectListItem> ColPosDropDownList
		{
			get { 
				var id = collocation == null ? null : collocation.colword.pos.Id.ToString();
				return new CommonViewModel(ModelType.ColPos, id).CommonDropDownList; 
			}
		}

		public List<SelectListItem> WordDropDownList { get { var id = collocation == null ? null : collocation.wordId.ToString(); return new CommonViewModel(ModelType.Word, id).CommonDropDownList; } }
		public List<SelectListItem> ColWordDropDownList { get { var id = collocation == null ? null : collocation.colWordId.ToString(); return new CommonViewModel(ModelType.ColWord, id).CommonDropDownList; } }

		public List<SelectListItem> CollocationPatternDropDownList
		{
			get
			{
				var ddlEntity = new List<SelectListItem>();
				ddlEntity.Add(new SelectListItem { Selected = collocation == null, Text = string.Format("- {0} -", THResources.Resources.CollocationPattern), Value = "0" });
				ddlEntity.Add(new SelectListItem { Text = CollocationPattern.adjective_noun.ToString(), Value = CollocationPattern.adjective_noun.ToString(), Selected = collocation != null && collocation.CollocationPattern == (int) CollocationPattern.adjective_noun });
				ddlEntity.Add(new SelectListItem { Text = CollocationPattern.adverb_verb.ToString(), Value = CollocationPattern.adverb_verb.ToString(), Selected = collocation != null && collocation.CollocationPattern == (int) CollocationPattern.adverb_verb });
				ddlEntity.Add(new SelectListItem { Text = CollocationPattern.noun_verb.ToString(), Value = CollocationPattern.noun_verb.ToString(), Selected = collocation != null && collocation.CollocationPattern == (int) CollocationPattern.noun_verb });
				ddlEntity.Add(new SelectListItem { Text = CollocationPattern.phrase_noun.ToString(), Value = CollocationPattern.phrase_noun.ToString(), Selected = collocation != null && collocation.CollocationPattern == (int) CollocationPattern.phrase_noun });
				ddlEntity.Add(new SelectListItem { Text = CollocationPattern.preposition_noun.ToString(), Value = CollocationPattern.preposition_noun.ToString(), Selected = collocation != null && collocation.CollocationPattern == (int) CollocationPattern.preposition_noun });
				//ddlEntity.Add(new SelectListItem { Text = CollocationPattern.preposition_verb.ToString(), Value = CollocationPattern.preposition_verb.ToString(), Selected = collocation.CollocationPattern == CollocationPattern.preposition_verb });
				ddlEntity.Add(new SelectListItem { Text = CollocationPattern.verb_noun.ToString(), Value = CollocationPattern.verb_noun.ToString(), Selected = collocation != null && collocation.CollocationPattern == (int) CollocationPattern.verb_noun });
				ddlEntity.Add(new SelectListItem { Text = CollocationPattern.verb_preposition.ToString(), Value = CollocationPattern.verb_preposition.ToString(), Selected = collocation != null && collocation.CollocationPattern == (int) CollocationPattern.verb_preposition });
				ddlEntity.Add(new SelectListItem { Text = CollocationPattern.phrase_noun.ToString(), Value = CollocationPattern.phrase_noun.ToString(), Selected = collocation != null && collocation.CollocationPattern == (int)CollocationPattern.phrase_noun });
				return ddlEntity;
			}
		}

		#region ICollocation Members

		public string Word { get; set; }

		public string ColWord { get; set; }


		public string ColWordZhs { get; set; }


		public string WordZht { get; set; }


		public string WordZhs { get; set; }


		public string ColWordZht { get; set; }


		public string WordJap { get; set; }


		public string ColWordJap { get; set; }


		public string Pos { get; set; }


		public string ColPos { get; set; }

		public string PosZht { get; set; }


		public string ColPosZht { get; set; }

		public string PosJap { get; set; }


		public string PosZhs { get; set; }


		public string ColPosJap { get; set; }


		public string ColPosZhs { get; set; }


		public List<Example> ExampleList { get; set; }

		// dont's put this notaion into bll, otherwise resulting in THResources.Resources ambigious reference...
		[Required, Range(0, 20, ErrorMessageResourceType = typeof(THResources.Resources), ErrorMessageResourceName = "CollocationPatternRequired")]
		public CollocationPattern CollocationPattern { get; set; }

		public long Id { get; set; }

		#endregion
	}
}