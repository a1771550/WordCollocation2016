using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Transactions;
using System.Web.Configuration;
using CommonLib.Helpers;
using DAL;
using log4net;
using MyWcModel.Abstract;

namespace MyWcModel
{
	public class ExampleRepository : WcRepositoryBase<example>
	{
		readonly WordcollocationEntities db = new WordcollocationEntities();
		internal override string GetCacheName { get { return "GetExamplesCacheName"; } }
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		readonly string connString = WebConfigurationManager.ConnectionStrings["WordCollocation"].ConnectionString;
		private string sql;

		public override List<example> GetList()
		{
			try
			{
				List<example> exampleList;
				if (CacheHelper.Exists(GetCacheName))
				{
					CacheHelper.Get(GetCacheName, out exampleList);
				}
				else
				{
					exampleList = db.examples.ToList();
					CacheHelper.Add(exampleList, GetCacheName, ModelAppSettings.CacheExpiration_Minutes);
				}
				return exampleList;
			}
			catch (Exception ex)
			{
				log.Error(ex.Message, ex);
				throw new Exception(ex.Message);
			}

		}


		public string[] GetExample_TranByEntry(string entry)
		{
			var examples = GetList();
			var example = examples.FirstOrDefault(p => String.Equals(p.Entry, entry, StringComparison.OrdinalIgnoreCase));

			return example != null ? new[] { example.EntryZht, example.EntryZhs, example.EntryJap } : null;
		}

		public override bool[] Add(example example)
		{
			bool[] bRet = new bool[2];
			bRet[0] = CheckIfDuplicatedEntry(example.Entry);

			if (bRet[0])
			{
				bRet[1] = false;
				return bRet;
			}


			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					var affectedRow = db.examples.Add(example);
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

		public override bool Update(example example)
		{

			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					db.Entry(example).State = EntityState.Modified;
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

		public override example GetById(string id)
		{
			try
			{
				return db.examples.Find(short.Parse(id));
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
					var example = GetById(id);
					example.CanDel = canDel;
					db.Entry(example).State = EntityState.Modified;
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
					var example = db.examples.Find(_id);
					db.examples.Remove(example);
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

		public bool Delete(example example)
		{
			using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
			{
				try
				{
					var pos_d = db.examples.Find(example.Id);
					db.examples.Remove(pos_d);
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

		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
		public collocation GetCollocationObjectByWcExampleId(long exampleId)
		{
			var example = GetById(exampleId.ToString());
			var collocationId = example.collocationId;
			return new CollocationRepository().GetById(collocationId.ToString());
		}

		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
		public List<example> GetListByCollocationId(long collocationId)
		{
			return (from examples in GetListInGroup() from example in examples where example.collocationId == collocationId select example).ToList();
		}


		[System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Delete, false)]
		public void DeleteWcExamplesByCollocationId(long collocationId)
		{
			try
			{
				SqlParameter CollocationId = new SqlParameter("@CollocationId", SqlDbType.BigInt, Int32.MaxValue, "Id");
				CollocationId.Value = collocationId;
				SqlParameter[] parameters=new SqlParameter[1];
				parameters[0] = CollocationId;
				sql = "DeleteExampleByCollocationId";
                DataAccess.ExecuteNonQuery(connString, sql, CommandType.StoredProcedure, parameters);
				// return affectedRow >= 1; // no need to return affected rowcount, as some collocations maynot have any example yet => none of example rows contain that collocationId...
			}
			catch (Exception ex)
			{
				log.Error(ex.Message, ex);
			}
			
		}

		public override List<IGrouping<long, example>> GetListInGroup()
		{
			var exList = GetList();
			return exList.GroupBy(x => x.collocationId).OrderByDescending(x => x.Key).ToList();
		}
	}
}
