using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace UI.Models.WcRepo
{
	public class PosRepository
	{
		public List<Pos> GetList()
		{
			StringBuilder sb = new StringBuilder();
			var url = "http://www.translationhall.com/api/web/pos";
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Stream resstream = response.GetResponseStream();

			if (resstream != null)
			{
				MemoryStream ms = new MemoryStream();
				resstream.CopyTo(ms);
				resstream.Close();
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
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			// don't modify below code!!
			// ReSharper disable RedundantNameQualifier
			List<PosRepository.Pos> posList = serializer.Deserialize<List<PosRepository.Pos>>(sb.ToString());
			// ReSharper restore RedundantNameQualifier
			return posList;
		}

		public string[] GetPos_TranByEntry(string entry)
		{
			var poss = GetList();
			var pos = poss.FirstOrDefault(p => String.Equals(p.Entry, entry, StringComparison.OrdinalIgnoreCase));

			return new[] { pos.EntryZht, pos.EntryZhs, pos.EntryJap };
		}

		public struct Pos
		{
			public string Entry;
			public string EntryZht;
			public string EntryZhs;
			public string EntryJap;
			public short Id;
		}
	}

	
}