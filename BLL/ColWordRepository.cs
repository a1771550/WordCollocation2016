using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Transactions;
using BLL.Abstract;
using CommonLib.Helpers;
using log4net;

namespace BLL
{
	public class ColWordRepository : WcRepositoryBase<ColWord>
	{
		readonly WordCollocationEntities db = new WordCollocationEntities();
		internal override string GetCacheName { get { return "GetColWordsCacheName"; } }
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public override List<ColWord> GetList()
		{
			try
			{
				List<ColWord> colWordList;
				if (CacheHelper.Exists(GetCacheName))
				{
					CacheHelper.Get(GetCacheName, out colWordList);
				}
				else
				{
					colWordList = db.ColWords.ToList();
					CacheHelper.Add(colWordList, GetCacheName, ModelAppSettings.CacheExpiration_Minutes);
				}
				return colWordList;
			}
			catch (Exception ex)
			{
				log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}

		}


		public string[] GetColWord_TranByEntry(string entry)
		{
			var colWords = GetList();
			var colWord = colWords.FirstOrDefault(p => String.Equals(p.Entry, entry, StringComparison.OrdinalIgnoreCase));

			return colWord != null ? new[] { colWord.EntryZht, colWord.EntryZhs, colWord.EntryJap } : null;
		}

		public override bool[] Add(ColWord colWord)
		{
			bool[] bRet = new bool[2];
			bRet[0] = CheckIfDuplicatedEntry(colWord.Entry);

			if (bRet[0])
			{
				bRet[1] = false;
				return bRet;
			}


			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					var affectedRow = db.ColWords.Add(colWord);
					db.SaveChanges();
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

		public override bool Update(ColWord colWord)
		{

			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					db.Entry(colWord).State = EntityState.Modified;
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

		public override ColWord GetById(string id)
		{
			try
			{
				return db.ColWords.Find(long.Parse(id));
			}
			catch (Exception ex)
			{
				log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}

		}

		public override bool UpdateCanDel(string id, bool canDel = false)
		{

			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					var colWord = GetById(id);
					colWord.CanDel = canDel;
					db.Entry(colWord).State = EntityState.Modified;
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

		public override bool Delete(string id)
		{
			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					long _id = Int64.Parse(id);
					var colWord = db.ColWords.Find(_id);
					db.ColWords.Remove(colWord);
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

		public bool Delete(ColWord colWord)
		{
			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					var word_d = db.ColWords.Find(colWord.Id);
					db.ColWords.Remove(word_d);
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
	}
}
