using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Transactions;
using CommonLib.Helpers;
using log4net;
using MyWcModel.Abstract;

namespace MyWcModel
{
	public class WordRepository : WcRepositoryBase<word>
	{
		readonly WordcollocationEntities db = new WordcollocationEntities();
		internal override string GetCacheName { get { return "GetWordsCacheName"; } }
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public override List<word> GetList()
		{
			try
			{
				List<word> wordList;
				if (CacheHelper.Exists(GetCacheName))
				{
					CacheHelper.Get(GetCacheName, out wordList);
				}
				else
				{
					wordList = db.words.ToList();
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

		public override bool[] Add(word word)
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
					var affectedRow = db.words.Add(word);
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

		public override bool Update(word word)
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

		public override word GetById(string id)
		{
			try
			{
				return db.words.Find(long.Parse(id));
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
					var word = db.words.Find(_id);
					db.words.Remove(word);
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

		public bool Delete(word word)
		{
			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					var word_d = db.words.Find(word.Id);
					db.words.Remove(word_d);
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
