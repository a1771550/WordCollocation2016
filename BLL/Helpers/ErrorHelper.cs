using System;
using Models;

namespace BLL.Helpers
{
	public static class ErrorHelper
	{
		public static ErrorViewModel CreatErrorViewModel(Exception ex)
		{
			ErrorViewModel model = new ErrorViewModel();
			model.Message = ex.Message;
			model.StackTrace = ex.StackTrace;
			model.TargetSite = ex.TargetSite.Name;
			model.Source = ex.Source;
			return model;
		}
	}
}