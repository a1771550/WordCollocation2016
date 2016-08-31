using System;

namespace UI.Models.Logging
{
	public class JavaScriptException:Exception
	{
		public JavaScriptException(string message)
			: base(message)
		{
		}
	}
}