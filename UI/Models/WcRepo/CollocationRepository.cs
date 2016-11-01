using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using UI.Helpers;
using UI.Models.WC;

namespace UI.Models.WcRepo
{
	public class CollocationRepository
	{
		public List<Collocation> GetCollocationListByWordColPosId(string word, short colPosId)
		{
			StringBuilder sb = new StringBuilder();
			var url = "http://www.translationhall.com/api/web/collocation/search?word="+word+"&id="+colPosId;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse responce = (HttpWebResponse)request.GetResponse();
			Stream resstream = responce.GetResponseStream();
			MemoryStream ms = new MemoryStream();
			if (resstream != null)
			{
				resstream.CopyTo(ms);
				resstream.Close();
				if (ms.Length == 0)
				{
					return null;
				}
				byte[] buf = new byte[ms.Length];
				ms.Position = 0;
				using (ms)
				{
					int count;
					do
					{
						count = ms.Read(buf, 0, buf.Length);
						if (count != 0)
						{
							string tempString = Encoding.UTF8.GetString(buf, 0, count);
							sb.Append(tempString);
						}
					} while (count > 0);
				}
				JavaScriptSerializer serializer = new JavaScriptSerializer();
				List<Collocation> colList = serializer.Deserialize<List<Collocation>>(sb.ToString());
				return colList;
			}
			return null;
		}
	}

	public struct Collocation
	{
		public long Id { get; set; }
		public long WordId { get; set; }
		public long ColWordId { get; set; }
		public CollocationPattern CollocationPattern { get; set; }
		public string pos { get; set; }
		public string posZht { get; set; }
		public string posZhs { get; set; }
		public string posJap { get; set; }
		public string colpos { get; set; }
		public string colposZht { get; set; }
		public string colposZhs { get; set; }
		public string colposJap { get; set; }
		public string word { get; set; }
		public string wordZht { get; set; }
		public string wordZhs { get; set; }
		public string wordJap { get; set; }
		public string colword { get; set; }
		public string colwordZht { get; set; }
		public string colwordZhs { get; set; }
		public string colwordJap { get; set; }
		public CollocationPattern colpattern { get; set; }

		public string ex { get; set; }
		public string exZht { get; set; }
		public string exZhs { get; set; }
		public string exJap { get; set; }
		public string source { get; set; }
		public string remark { get; set; }

	}

	public struct Example
	{
		public string entry { get; set; }
		public string entryZht { get; set; }
		public string entryZhs { get; set; }
		public string entryJap { get; set; }
		public short? source { get; set; }
		public string remark { get; set; }
		public string entryTran { get; set; }
		public string sourceRemark { get {return WcHelper.GetSourceRemark(this); } }
		public string formattedEntry { get; set; }
	}
}