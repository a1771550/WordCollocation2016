using System;
using System.IO;
using System.Net;
using System.Text;

namespace UI.Demo
{
	public partial class MemoryStreamDemo : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
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
				lblOutput.Text = sb.ToString();
			}
		}
	}
}