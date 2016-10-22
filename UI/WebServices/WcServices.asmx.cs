using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using UI.Helpers;
using UI.Models.Abstract;
using UI.Models.Misc;
using UI.Models.WcRepo;
using WebMatrix.WebData;

namespace UI.WebServices
{
	/// <summary>
	/// Summary description for WcServices
	/// </summary>
	[WebService(Namespace = "http://www.wordcollocation.net/WebServices")]
	[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	[ToolboxItem(false)]
	// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
	[ScriptService]
	public class WcServices : WebService
	{
		//private readonly AccountRepository _repo = new AccountRepository();

		private IEnumerable<WcBase> GetWcList(ModelType entity)
		{
			switch (entity)
			{
				case ModelType.Pos:


				case ModelType.ColPos:


				case ModelType.Word:


				case ModelType.ColWord:

					return null;
			}
			return null;
		}
		const string CollocationListSessionName = "CollocationList";
		/*
				private IEnumerable<UserRoleBase> GetUrList(ModelType entity)
				{
					switch (entity)
					{
						case ModelType.User:
							var urepo = new WcUserRepository();
							return urepo.GetList();
						case ModelType.Role:
							var rrepo = new RoleRepository();
							return rrepo.GetList();
					}
					return null;
				}
		*/

		//[WebMethod]
		//public bool CheckIfDuplicatedUserName(string name)
		//{
		//	return _repo.GetUserByName(name) != null;
		//}

		[WebMethod]
		public bool CheckUserNameTaken(string username)
		{
			return WebSecurity.UserExists(username);
		}

		[WebMethod]
		public bool CheckEmailTaken(string email)
		{
			return AccountHelper.CheckIfDuplicatedEmail(email);
		}

		[WebMethod(EnableSession = true)]
		public string SearchCollocation(string word, string id)
		{
			if (word != null && id != null)
			{
				short wcpId = Convert.ToInt16(id);
				var colrepo = new CollocationRepository();
				var collocationList = colrepo.GetCollocationListByWordColPosId(word, wcpId);
				string host = HttpContext.Current.Request.Headers["HOST"];
				//string url;
				string protocol = SiteConfiguration.Protocol + @"://";
				if (collocationList.Count > 0)
				{
					HttpContext.Current.Session[CollocationListSessionName] = collocationList;
					return string.Format("{0}{1}{2}", protocol, host, @"/Home/SearchResult");

				}
				return string.Format("{0}{1}{2}", protocol, host, @"/Home/NoSearchResult");
				//return url;
			}
			return null;
		}

		[WebMethod]
		public bool CheckIfDuplicatedEntry(ModelType entity, params string[] entities)
		{
			bool bRet = false;

			if (entities.Length == 1) // for pos,colpos,word,colword, wcuser, wcrole
			{
				bRet = GetWcList(entity).Any(x => x.Entry.Equals(entities[0], StringComparison.OrdinalIgnoreCase));
			}

			return bRet;
		}

		//[WebMethod]
		//public bool CheckIfDuplicatedEmail(string email)
		//{
		//	return AccountHelper.CheckIfDuplicatedEmail(email);
		//}
		//[WebMethod]
		//public bool CheckPasswordLength(string package)
		//{
		//	var min = SiteConfiguration.MinPasswordLength;
		//	var max = SiteConfiguration.MaxPasswordLength;
		//	return package.Length >= min && package.Length <= max;
		//}
	}
}
