using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MyWcModel;

namespace UI
{
	public partial class MyWcDemo : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			WordcollocationEntities db = new WordcollocationEntities();
			if (!IsPostBack)
			{
				List<pos> posList = db.poss.ToList();
				foreach (pos p in posList)
				{
					Output.Text += p.Entry + " " + p.EntryZht + "<br>";
				}
			}
		}
	}
}