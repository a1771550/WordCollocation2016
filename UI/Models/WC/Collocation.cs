using System.Security.Policy;

namespace UI.Models.WC
{
	public class Collocation
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

	public enum CollocationPattern
	{
		noun_verb = 1,
		verb_noun = 2,
		adjective_noun = 3,
		verb_preposition = 4,
		preposition_verb = 5, //not used, but must NOT be deleted; otherwise the current examples in the db cannot be shown...
		adverb_verb = 6,
		phrase_noun = 7,
		preposition_noun = 8,
		adjective_phrase = 9,
	}
}
 