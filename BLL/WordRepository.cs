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
	public class WordRepository : WcRepositoryBase<Word>
	{
		readonly WordCollocationEntities db = new WordCollocationEntities();
		internal override string GetCacheName { get { return "GetWordsCacheName"; } }
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public override List<Word> GetList()
		{
			try
			{
				List<Word> wordList;
				if (CacheHelper.Exists(GetCacheName))
				{
					CacheHelper.Get(GetCacheName, out wordList);
				}
				else
				{
					wordList = db.Words.ToList();
					CacheHelper.Add(wordList, GetCacheName, ModelAppSettings.CacheExpiration_Minutes);
				}
				return wordList;
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}

		}


		public string[] GetWord_TranByEntry(string entry)
		{
			var words = GetList();
			var word = words.FirstOrDefault(p => String.Equals(p.Entry, entry, StringComparison.OrdinalIgnoreCase));

			return word != null ? new[] { word.EntryZht, word.EntryZhs, word.EntryJap } : null;
		}

		public override bool[] Add(Word word)
		{
			bool[] bRet = new bool[2];
			bRet[0] = CheckIfDuplicatedEntry(word.Entry);

			if (bRet[0])
			{
				bRet[1] = false;
				return bRet;
			}


			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					var affectedRow = db.Words.Add(word);
					db.SaveChanges();
					scope.Complete();
					bRet[1] = affectedRow != null;
					CacheHelper.Clear(GetCacheName);
					return bRet;
				}
				catch (TransactionException ex)
				{
					Log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
				catch (Exception ex)
				{
					Log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
			}
		}

		public override bool Update(Word word)
		{

			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					db.Entry(word).State = EntityState.Modified;
					db.SaveChanges();
					scope.Complete();
					CacheHelper.Clear(GetCacheName);
					return true;
				}
				catch (TransactionException ex)
				{
					Log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
				catch (Exception ex)
				{
					Log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}

			}

		}

		public override Word GetById(string id)
		{
			try
			{
				return db.Words.Find(long.Parse(id));
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}

		}

		public override bool UpdateCanDel(string id, bool canDel = false)
		{

			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					var word = GetById(id);
					word.CanDel = canDel;
					db.Entry(word).State = EntityState.Modified;
					db.SaveChanges();
					scope.Complete();
					CacheHelper.Clear(GetCacheName);
					return true;
				}
				catch (TransactionException ex)
				{
					Log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
				catch (Exception ex)
				{
					Log.Error(ex.Message, ex);
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
					var word = db.Words.Find(_id);
					db.Words.Remove(word);
					db.SaveChanges();
					scope.Complete();
					CacheHelper.Clear(GetCacheName);
					return true;
				}
				catch (TransactionException ex)
				{
					Log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
				catch (Exception ex)
				{
					Log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
			}
		}

		public bool Delete(Word word)
		{
			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					var word_d = db.Words.Find(word.Id);
					db.Words.Remove(word_d);
					db.SaveChanges();
					scope.Complete();
					CacheHelper.Clear(GetCacheName);
					return true;
				}
				catch (TransactionException ex)
				{
					Log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
				catch (Exception ex)
				{
					Log.Error(ex.Message, ex);
					throw new Exception(ex.Message);
				}
			}
		}
	}
}
