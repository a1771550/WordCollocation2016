using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Transactions;
using CommonLib.Helpers;
using log4net;

namespace MyWcModel
{
	public class CollocationRepository
	{
		readonly WordcollocationEntities db = new WordcollocationEntities();
		internal string GetCacheName { get { return "GetCollocationsCacheName"; } }
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public List<Collocation> GetList()
		{
			try
			{
				var coList = db.collocations.ToList();
				return coList;
			}
			catch (Exception ex)
			{
				log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}

		}

		public Collocation GetByIds(params string[] ids)
		{
			return GetList().SingleOrDefault(x =>
				//x.posId == short.Parse(ids[0]) && x.colPosId == short.Parse(ids[1]) &&
				x.wordId == long.Parse(ids[0]) && x.colWordId == long.Parse(ids[1]) &&
				x.CollocationPattern == (int) Enum.Parse(typeof(CollocationPattern), ids[4], true));
		}

		public bool[] Add(Collocation collocation)
		{
			bool[] bRet = new bool[2];
			bRet[0] = CheckIfDuplicatedEntry(collocation);

			if (bRet[0])
			{
				bRet[1] = false;
				return bRet;
			}


			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					var affectedRow = db.collocations.Add(collocation);
					db.SaveChanges();

					//UpdateCanDel(collocation, false);

					scope.Complete();
					bRet[1] = affectedRow != null;
					CacheHelper.Clear(GetCacheName);
					return bRet;
				}
				catch (TransactionException ex)
				{
					log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
				catch (Exception ex)
				{
					log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
			}
		}

		//private static void UpdateCanDel(collocation collocation, bool del)
		//{
		//	WordRepository wDb = new WordRepository();
		//	canDel = wDb.GetById(collocation.wordId.ToString()).CanDel;
		//	bDel = canDel != null && canDel.Value;
		//	if (bDel && !del)
		//	{
		//		wDb.UpdateCanDel(collocation.wordId.ToString());
		//	}
		//	else if (!bDel && del)
		//	{
		//		wDb.UpdateCanDel(collocation.wordId.ToString(), true);
		//	}

		//	ColWordRepository cwDb = new ColWordRepository();
		//	canDel = cwDb.GetById(collocation.colWordId.ToString()).CanDel;
		//	bDel = canDel != null && canDel.Value;
		//	if (bDel && !del)
		//	{
		//		cwDb.UpdateCanDel(collocation.colWordId.ToString());
		//	}
		//	else if (!bDel && del)
		//	{
		//		cwDb.UpdateCanDel(collocation.colWordId.ToString(), true);
		//	}
		//}

		private bool CheckIfDuplicatedEntry(Collocation collocation)
		{
			return db.collocations.Any(
				c =>
					//c.posId == collocation.posId && c.colPosId == collocation.colPosId && 
					c.wordId == collocation.wordId &&
					c.colWordId == collocation.colWordId);
		}

		public bool Update(Collocation collocation)
		{

			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					db.Entry(collocation).State = EntityState.Modified;
					db.SaveChanges();
					scope.Complete();
					CacheHelper.Clear(GetCacheName);
					return true;
				}
				catch (TransactionException ex)
				{
					log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
				catch (Exception ex)
				{
					log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}

			}

		}

		public Collocation GetById(string id)
		{
			try
			{
				return db.collocations.Find(long.Parse(id));
			}
			catch (Exception ex)
			{
				log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}

		}

		public bool Delete(string id)
		{
			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					short _id = Int16.Parse(id);
					var collocation = db.collocations.Find(_id);
					db.collocations.Remove(collocation);

					//UpdateCanDel(collocation, true);

					db.SaveChanges();
					scope.Complete();
					CacheHelper.Clear(GetCacheName);
					return true;
				}
				catch (TransactionException ex)
				{
					log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
				catch (Exception ex)
				{
					log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
			}
		}

		public bool Delete(Collocation collocation)
		{
			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					var c_d = db.collocations.Find(collocation.Id);
					db.collocations.Remove(c_d);

					//UpdateCanDel(collocation, true);

					db.SaveChanges();
					scope.Complete();
					CacheHelper.Clear(GetCacheName);
					return true;
				}
				catch (TransactionException ex)
				{
					log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
				catch (Exception ex)
				{
					log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
			}
		}

		public Dictionary<string, int> GetColPatternDictionary()
		{
			Dictionary<string, int> ColPatternDictionary = new Dictionary<string, int>
				{
					{CollocationPattern.noun_verb.ToString(), (int) CollocationPattern.noun_verb},
					{CollocationPattern.verb_noun.ToString(), (int) CollocationPattern.verb_noun},
					{CollocationPattern.adjective_noun.ToString(), (int) CollocationPattern.adjective_noun},
					{CollocationPattern.adverb_verb.ToString(), (int) CollocationPattern.adverb_verb},
					{CollocationPattern.verb_preposition.ToString(), (int) CollocationPattern.verb_preposition},
					//{CollocationPattern.preposition_verb.ToString(), ((int) CollocationPattern.preposition_verb)},
					{CollocationPattern.phrase_noun.ToString(), (int) CollocationPattern.phrase_noun},
				{CollocationPattern.adjective_phrase.ToString(),(int)CollocationPattern.adjective_phrase },

				};
			return ColPatternDictionary;
		}

		private static IEnumerable<CollocationPattern> GetColPatternList()
		{
			List<CollocationPattern> ColPatternList = new List<CollocationPattern>
				{
					CollocationPattern.adjective_noun,
					CollocationPattern.adverb_verb,
					CollocationPattern.noun_verb,
					CollocationPattern.phrase_noun,
					//CollocationPattern.preposition_verb,
					CollocationPattern.verb_noun,
					CollocationPattern.verb_preposition,
					CollocationPattern.adjective_phrase,
				};

			return ColPatternList;
		}

		public CollocationPattern GetColPatternKeyByValue(int index)
		{
			return GetColPatternList().Single(p => (int)p == index);
		}

		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
		public List<Collocation> GetCollocationListInGroup(string letter = null)
		{
			//var collocations = Getcollocations();
			var collocationList = GetList();
			if (!string.IsNullOrEmpty(letter))
				collocationList =
					collocationList.Where(c => String.Equals(c.word.Entry, letter, StringComparison.CurrentCultureIgnoreCase)).ToList();

			var collist = collocationList.GroupBy(c => c.word + "|" + c.CollocationPattern).Select(g => new { g.Key, collocation = g });
			var cvList = new List<Collocation>();

			foreach (var l in collist)
			{
				Collocation cv = new Collocation();
				cv.word.Entry = l.Key.Split('|')[0];
					//CollocationPattern = (CollocationPattern)(Convert.ToInt32(l..Split('|')[1]))
				

				//just loop once, in fact just a transition from the grouped values to the properties of the new collocation class, not really a loop at all
				foreach (var c in l.collocation)
				{
					cv.CollocationPattern = c.CollocationPattern;
					cv.word = c.word;
					cv.colword = c.colword;
					cv.Id = c.Id;
					cv.examples = c.examples;
					//cvList.Add(cv);
				}
				cvList.Add(cv);
			}

			return cvList;
		}

		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
		public List<Collocation> GetCollocationListByWordColPosId(string word, long colposId)
		{
			//var collocations = Getcollocations();
			var collocationList = GetList();
			var collist = collocationList.Where(c => c.word.Entry.ToLower() == word.ToLower() && c.colword.posId==colposId).ToList();
			return collist;
		}

		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
		public List<Collocation> GetCollocationListByWordPattern(string word, CollocationPattern pattern)
		{
			//var collocations = Getcollocations();
			var colList = GetList();
			return colList.Where(c => string.Equals(c.word.Entry, word, StringComparison.CurrentCultureIgnoreCase) && c.CollocationPattern == (int) pattern).ToList();
		}

		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
		public List<Collocation> GetCollocationListByWord(string word)
		{
			//var collocations = Getcollocations();
			var collocationList = GetList();
			return collocationList.Where(c => String.Equals(c.word.Entry, word, StringComparison.CurrentCultureIgnoreCase)).ToList();
		}
	}
}
