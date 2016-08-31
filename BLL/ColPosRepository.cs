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
	public class ColPosRepository : WcRepositoryBase<ColPos>
	{
		readonly WordCollocationEntities db = new WordCollocationEntities();
		internal override string GetCacheName { get { return "GetColPossCacheName"; } }
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public override List<ColPos> GetList()
		{
			try
			{
				List<ColPos> colPosList;
				if (CacheHelper.Exists(GetCacheName))
				{
					CacheHelper.Get(GetCacheName, out colPosList);
				}
				else
				{
					colPosList = db.ColPos.ToList();
					CacheHelper.Add(colPosList, GetCacheName, ModelAppSettings.CacheExpiration_Minutes);
				}
				return colPosList;
			}
			catch (Exception ex)
			{
				log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}

		}


		public string[] GetColPos_TranByEntry(string entry)
		{
			var colPoss = GetList();
			var colPos = colPoss.FirstOrDefault(p => String.Equals(p.Entry, entry, StringComparison.OrdinalIgnoreCase));

			return colPos != null ? new[] { colPos.EntryZht, colPos.EntryZhs, colPos.EntryJap } : null;
		}

		public override bool[] Add(ColPos colPos)
		{
			bool[] bRet = new bool[2];
			bRet[0] = CheckIfDuplicatedEntry(colPos.Entry);

			if (bRet[0])
			{
				bRet[1] = false;
				return bRet;
			}


			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					var affectedRow = db.ColPos.Add(colPos);
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

		public override bool Update(ColPos colPos)
		{

			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					db.Entry(colPos).State = EntityState.Modified;
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

		public override ColPos GetById(string id)
		{
			try
			{
				return db.ColPos.Find(short.Parse(id));
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
					var colPos = GetById(id);
					colPos.CanDel = canDel;
					db.Entry(colPos).State = EntityState.Modified;
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
					short _id = Int16.Parse(id);
					var colPos = db.ColPos.Find(_id);
					db.ColPos.Remove(colPos);
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

		public bool Delete(ColPos colPos)
		{
			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					var pos_d = db.ColPos.Find(colPos.Id);
					db.ColPos.Remove(pos_d);
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
