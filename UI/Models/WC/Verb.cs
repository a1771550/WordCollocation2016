using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using UI.Models.Misc;

namespace UI.Models.WC
{
	[Serializable]
	public class Verb
	{
		public string RegularVerb { get; set; }
		public string Infinitive { get; set; }
		public string SimplePast { get; set; }
		public string PastParticiple { get; set; }

		private string filepath;

		public List<string> GetRegularVerbList()
		{
			filepath = HttpContext.Current.Server.MapPath(SiteConfiguration.RegularVerbsXMLFile);
			XElement xelement = XElement.Load(filepath);
			IEnumerable<XElement> verbs = xelement.Elements();
			return verbs.Select(verb => verb.Value).ToList();
		}  

		public Dictionary<string, string> GetIrregluarVerbList()
		{
			Dictionary<string,string> verblist=new Dictionary<string, string>();
			filepath = HttpContext.Current.Server.MapPath(SiteConfiguration.IrregularVerbsXMLFile);
			XElement xelement = XElement.Load(filepath);
			IEnumerable<XElement> verbs = xelement.Elements();
			foreach (var verb in verbs)
			{
				/*
				 * <Verb>
		<Infinitive>abide</Infinitive>
		<SimplePast>abided / abode</SimplePast>
		<PastParticiple>abided</PastParticiple>
	</Verb>
				 */
				var xElement = verb.Element("Infinitive");
				if (xElement != null) Infinitive = xElement.Value.Trim();
				var element = verb.Element("SimplePast");
				if (element != null) SimplePast = element.Value.Trim();
				var o = verb.Element("PastParticiple");
				if (o != null) PastParticiple = o.Value.Trim();
				verblist.Add(Infinitive, string.Concat(SimplePast,"|",PastParticiple));
			}
			return verblist;
		} 
	}
}
