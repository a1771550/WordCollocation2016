using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BLL.Abstract
{
	public enum ModelType
	{
		Pos=1,
		ColPos=2,
		Word=3,
		ColWord=4,
		Collocation=5,
		Example=6,
		User=7,
		Role=8
	}

	public abstract class WcRepositoryBase<T> where T : WcBase
	{
		internal abstract string GetCacheName { get; }

		[DataObjectMethod(DataObjectMethodType.Select, true)]
		public abstract List<T> GetList();

		public virtual List<IGrouping<long, T>> GetListInGroup()
		{
			return null;
		}

		[DataObjectMethod(DataObjectMethodType.Select, false)]
		public abstract T GetById(string id);


		public virtual bool CheckIfDuplicatedEntry(params string[] entities)
		{
			bool bRet = false;

			if (entities.Length == 1) // for pos,colpos,word,colword
			{
				bRet = GetList().Any(x => x.Entry.Equals(entities[0], StringComparison.OrdinalIgnoreCase));
			}

			return bRet;
		}

		public virtual WcBase GetObjectByEntries(params string[] entries)
		{
			return
				GetList()
					.SingleOrDefault(
						x => x.Entry == entries[0]);
		}

		public virtual bool[] Add(T obj)
		{
			return null;
		}

		public virtual bool Update(T obj)
		{
			return false;
		}

		public virtual bool Delete(string id)
		{
			return false;
		}
		public virtual bool UpdateCanDel(string id, bool canDel=false) { return false;}
	}
}
