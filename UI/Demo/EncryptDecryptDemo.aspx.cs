using System;
using System.Web.UI;
using CommonLib.Helpers;
using UI.Helpers;

namespace UI.Demo
{
	public partial class EncryptDecryptDemo : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		

		protected void btnEncrpyt_OnClick(object sender, EventArgs e)
		{
			string password = txtPassword.Text.Trim();
			string enPwd = TextHelper.Encrypt(password, txtEmail.Text.Trim(), true);
			lblEncrypted.Text = "Encrypted: " + enPwd + "<br>Length: " + enPwd.Length;
			Session["Password"] = password;
			Session["Encrypted"] = enPwd;
		}

		protected void btnDecrpty_OnClick(object sender, EventArgs e)
		{
			string dePwd = TextHelper.Decrypt(Session["Encrypted"].ToString(),txtEmail.Text.Trim(), true);
			if (Session["Password"].ToString() == dePwd) lblResult.Text = "OK";
			else lblResult.Text = "NG";
			Session.Abandon();
		}
	}
}