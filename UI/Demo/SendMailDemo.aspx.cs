using System;
using System.Net;
using System.Net.Mail;
using System.Web.UI;
using UI.Models.Misc;

namespace UI.Demo
{
	public partial class SendMailDemo : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				try
				{
					SmtpClient SmtpServer = new SmtpClient
					{
						Credentials = new NetworkCredential("TongLing", "918273"),
						Port = 25,
						Host = "localhost"
					};
					SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
					MailMessage mail = new MailMessage { From = new MailAddress("admin@wordcollocation.net") };
					mail.To.Add("a1771550@gmail.com");
					mail.Subject = "Test";
					mail.Body = "This is testing";
					SmtpServer.Send(mail);
				}
				catch (SmtpException ex)
				{
					throw new SmtpException("Email Sending Error", ex.InnerException);
				}
				catch (Exception ex)
				{

					throw new Exception(ex.Message, ex.InnerException);

				}

			}
		}
	}
}