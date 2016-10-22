using UI.Models.Abstract;

namespace UI.Models.WC
{
	public class Word:WcBase
	{
		public new long Id { get; set; }
		public short PosId { get; set; }
	}
}