using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Transactions;
using CommonLib.Helpers;
using log4net;

namespace BLL
{
	public class CollocationRepository
	{
		readonly WordCollocationEntities db = new WordCollocationEntities();
		internal string GetCacheName { get { return "GetCollocationsCacheName"; } }
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public List<Collocation> GetList()
		{
			try
			{
				List<Collocation> coList;
				//if (CacheHelper.Exists(GetCacheName))
				//{
				//	CacheHelper.Get(GetCacheName, out coList);
				//}
				//else
				//{
				//	coList = db.Collocations.ToList();
				//	CacheHelper.Add(coList, GetCacheName, ModelAppSettings.CacheExpiration_Minutes);
				//}
				coList = db.Collocations.ToList();
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
				x.posId == short.Parse(ids[0]) && x.colPosId == short.Parse(ids[1]) &&
				x.wordId == long.Parse(ids[2]) && x.colWordId == long.Parse(ids[3]) &&
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
					var affectedRow = db.Collocations.Add(collocation);
					db.SaveChanges();

					UpdateCanDel(collocation, false);

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

		//public bool[] Add(Collocation collocation, out long collocationId)
		//{
		//	bool[] bRet = new bool[3];
		//	collocationId = 0;

		//	bRet[0] = CheckIfDuplicatedEntry(collocation);

		//	if (bRet[0])
		//	{
		//		bRet[1] = false;
		//		return bRet;
		//	}

		//	try
		//	{
		//		using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
		//		{
		//			collocationId = Convert.ToInt64(Adapter.InsertQuery(collocation.posId, collocation.colPosId, collocation.wordId, collocation.colWordId,
		//			(int)collocation.CollocationPattern));
		//			scope.Complete();
		//			bRet[1] = collocationId > 0;
		//		}
		//	}
		//	catch (TransactionException ex)
		//	{
		//		throw new Exception(ex.Message, ex.InnerException);
		//	}
		//	CacheHelper.Clear(GetCacheName);

		//	bRet[2] = UpdateOtherIDTables(collocation);

		//	return bRet;
		//}

		private static void UpdateCanDel(Collocation collocation, bool del)
		{
			PosRepository posDb = new PosRepository();
			var canDel = posDb.GetById(collocation.posId.ToString()).CanDel;
			bool bDel = canDel != null && canDel.Value;
			if (bDel && !del)
			{
				posDb.UpdateCanDel(collocation.posId.ToString());
			}
			else if (!bDel && del)
			{
				posDb.UpdateCanDel(collocation.posId.ToString(), true);
			}

			ColPosRepository colPdb = new ColPosRepository();
			canDel = colPdb.GetById(collocation.colPosId.ToString()).CanDel;
			bDel = canDel != null && canDel.Value;
			if (bDel && !del)
			{
				colPdb.UpdateCanDel(collocation.colPosId.ToString());
			}
			else if (!bDel && del)
			{
				colPdb.UpdateCanDel(collocation.colPosId.ToString(), true);
			}

			WordRepository wDb = new WordRepository();
			canDel = wDb.GetById(collocation.wordId.ToString()).CanDel;
			bDel = canDel != null && canDel.Value;
			if (bDel && !del)
			{
				wDb.UpdateCanDel(collocation.wordId.ToString());
			}
			else if (!bDel && del)
			{
				wDb.UpdateCanDel(collocation.wordId.ToString(), true);
			}

			ColWordRepository cwDb = new ColWordRepository();
			canDel = cwDb.GetById(collocation.colWordId.ToString()).CanDel;
			bDel = canDel != null && canDel.Value;
			if (bDel && !del)
			{
				cwDb.UpdateCanDel(collocation.colWordId.ToString());
			}
			else if (!bDel && del)
			{
				cwDb.UpdateCanDel(collocation.colWordId.ToString(), true);
			}
		}

		// doneTODO:...
		private bool CheckIfDuplicatedEntry(Collocation collocation)
		{
			return db.Collocations.Any(
				c =>
					c.posId == collocation.posId && c.colPosId == collocation.colPosId && c.wordId == collocation.wordId &&
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
				return db.Collocations.Find(long.Parse(id));
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
					var collocation = db.Collocations.Find(_id);
					db.Collocations.Remove(collocation);

					UpdateCanDel(collocation, true);

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
					var c_d = db.Collocations.Find(collocation.Id);
					db.Collocations.Remove(c_d);

					UpdateCanDel(collocation, true);

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
					{CollocationPattern.noun_verb.ToString(), ((int) CollocationPattern.noun_verb)},
					{CollocationPattern.verb_noun.ToString(), ((int) CollocationPattern.verb_noun)},
					{CollocationPattern.adjective_noun.ToString(), ((int) CollocationPattern.adjective_noun)},
					{CollocationPattern.adverb_verb.ToString(), ((int) CollocationPattern.adverb_verb)},
					{CollocationPattern.verb_preposition.ToString(), ((int) CollocationPattern.verb_preposition)},
					//{CollocationPattern.preposition_verb.ToString(), ((int) CollocationPattern.preposition_verb)},
					{CollocationPattern.phrase_noun.ToString(), ((int) CollocationPattern.phrase_noun)}
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
					CollocationPattern.verb_preposition
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
			//var collocations = GetCollocations();
			var collocationList = GetList();
			if (!string.IsNullOrEmpty(letter))
				collocationList =
					collocationList.Where(c => String.Equals(c.Word.Entry, letter, StringComparison.CurrentCultureIgnoreCase)).ToList();

			var collist = collocationList.GroupBy(c => c.Word + "|" + c.CollocationPattern).Select(g => new { g.Key, Collocation = g });
			var cvList = new List<Collocation>();

			foreach (var l in collist)
			{
				Collocation cv = new Collocation();
				cv.Word.Entry = l.Key.Split('|')[0];
					//CollocationPattern = (CollocationPattern)(Convert.ToInt32(l..Split('|')[1]))
				

				//just loop once, in fact just a transition from the grouped values to the properties of the new Collocation class, not really a loop at all
				foreach (var c in l.Collocation)
				{
					cv.CollocationPattern = c.CollocationPattern;
					cv.Pos = c.Pos;
					//cv.PosZht = c.PosZht;
					//cv.PosZhs = c.PosZhs;
					//cv.PosJap = c.PosJap;
					cv.ColPos = c.ColPos;
					//cv.ColPosZht = c.ColPosZht;
					//cv.ColPosZhs = c.ColPosZhs;
					//cv.ColPosJap = c.ColPosJap;
					cv.Word = c.Word;
					//cv.WordZht = c.WordZht;
					//cv.WordZhs = c.WordZhs;
					//cv.WordJap = c.WordJap;
					cv.ColWord = c.ColWord;
					//cv.ColWordZht = c.ColWordZht;
					//cv.ColWordZhs = c.ColWordZhs;
					//cv.ColWordJap = c.ColWordJap;
					cv.Id = c.Id;
					cv.Examples = c.Examples;
					//cvList.Add(cv);
				}
				cvList.Add(cv);
			}

			return cvList;
		}

		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
		public List<Collocation> GetCollocationListByWordColPosId(string word, short colPosId)
		{
			//var collocations = GetCollocations();
			var collocationList = GetList();
			var collist = collocationList.Where(c => c.Word.Entry.ToLower() == word.ToLower() && c.colPosId == colPosId).ToList();
			return collist;
		}

		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
		public List<Collocation> GetCollocationListByWordPattern(string word, CollocationPattern pattern)
		{
			//var collocations = GetCollocations();
			var colList = GetList();
			return colList.Where(c => string.Equals(c.Word.Entry, word, StringComparison.CurrentCultureIgnoreCase) && c.CollocationPattern == (int) pattern).ToList();
		}

		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
		public List<Collocation> GetCollocationListByWord(string word)
		{
			//var collocations = GetCollocations();
			var collocationList = GetList();
			return collocationList.Where(c => String.Equals(c.Word.Entry, word, StringComparison.CurrentCultureIgnoreCase)).ToList();
		}
	}
}
