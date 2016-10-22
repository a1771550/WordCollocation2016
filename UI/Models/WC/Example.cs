using UI.Models.Abstract;

namespace UI.Models.WC
{
	public class Example:WcBase
	{
		public new long Id { get; set; }
		public short? Source { get; set; }
		public string Remark { get; set; }
	}

	public enum ExampleSource
	{
		OXFORD_COLLOCATIONS_DICTIONARY = 1,
		NEW_DICTIONARY_OF_ENGLISH_COLLOCATIONS = 2,
		NEWSPAPER = 3,
		WEB = 4,
		FICTION = 5,
		OTHERS = 6,
	}
}