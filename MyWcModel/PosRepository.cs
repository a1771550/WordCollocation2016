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
	public class PosRepository:WcRepositoryBase<pos>
	{
		private readonly WordcollocationEntities db = new WordcollocationEntities();
		internal override string GetCacheName { get { return "GetPossCacheName"; } }

		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public override List<pos> GetList()
		{
			try
			{
				List<pos> posList;
				if (CacheHelper.Exists(GetCacheName))
				{
					CacheHelper.Get(GetCacheName, out posList);
				}
				else
				{
					posList = db.poss.ToList();
					CacheHelper.Add(posList, GetCacheName, ModelAppSettings.CacheExpiration_Minutes);
				}
				return posList;
			}
			catch (Exception ex)
			{
				log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}
			
		}


		public string[] GetPos_TranByEntry(string entry)
		{
			var poss = GetList();
			var pos = poss.FirstOrDefault(p => String.Equals(p.Entry, entry, StringComparison.OrdinalIgnoreCase));

			return pos != null ? new[] { pos.EntryZht, pos.EntryZhs, pos.EntryJap } : null;
		}

		public override bool[] Add(pos pos)
		{
			bool[] bRet = new bool[2];
			bRet[0] = CheckIfDuplicatedEntry(pos.Entry);

			if (bRet[0])
			{
				bRet[1] = false;
				return bRet;
			}


			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					var affectedRow = db.poss.Add(pos);
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

		public override bool Update(pos pos)
		{

			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					db.Entry(pos).State = EntityState.Modified;
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

		public override pos GetById(string id)
		{
			try
			{
				return db.poss.Find(short.Parse(id));
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
					var pos = GetById(id);
					pos.CanDel = canDel;
					db.Entry(pos).State = EntityState.Modified;
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
					var pos = db.poss.Find(_id);
					db.poss.Remove(pos);
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

		public bool Delete(pos pos)
		{
			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					var pos_d = db.poss.Find(pos.Id);
					db.poss.Remove(pos_d);
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
