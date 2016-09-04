using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using log4net;
using Microsoft.Web.WebPages.OAuth;
using UI.Helpers;
using UI.Models;
using WebMatrix.WebData;
using WcResources = THResources.Resources;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using UI.Models.ViewModels;

namespace UI.Controllers
{
	/* never put [Authorize(Roles="Admin")] above this controller; otherwise the logoff won't work!! */
	[Authorize]
	public class AccountController : Controller
	{
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		public ViewResult UserIndex(short page = 1)
		{
			UserViewModel model = new UserViewModel(page);
			return View(model);
		}

		public ActionResult UserEdit(int userId)
		{
			UserEditModel model = new UserEditModel(userId);
			model.SelectedRoles = AccountHelper.GetRoleListByUserId(userId);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UserEdit(UserEditModel model)
		{
			if (ModelState.IsValid)
			{
				if (AccountHelper.UpdateUser(model))
				{
					return RedirectToAction("UserIndex");
				}
				return View(model);
			}
			return View(model);
		}

		public ActionResult UserDelete(int userId)
		{
			AccountHelper.DeleteUser(userId);
			return RedirectToAction("UserIndex");
		}

		public ActionResult RoleIndex(short page = 1)
		{
			RoleViewModel model = new RoleViewModel(page);
			return View(model);
		}

		public ActionResult RoleCreate()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RoleCreate(RoleViewModel model)
		{
			if (ModelState.IsValid)
			{
				if (Roles.RoleExists(model.Role.RoleName))
				{
					ModelState.AddModelError(string.Empty, WcResources.RoleNameExisted);
					return View(model);
				}
				Roles.CreateRole(model.Role.RoleName);
				return RedirectToAction("RoleIndex", "Account");
			}
			ModelState.AddModelError("", WcResources.RoleNameRequired);
			return View(model);
		}

		public ViewResult RoleEdit(int roleId)
		{
			RoleEditModel model = new RoleEditModel(roleId);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RoleEdit(RoleEditModel model)
		{
			if (ModelState.IsValid)
			{
				if (Roles.RoleExists(model.Role.RoleName))
				{
					ModelState.AddModelError(string.Empty, WcResources.RoleNameExisted);
					return View(model);
				}
				if (AccountHelper.UpdateRole(model.Role))
					return RedirectToAction("RoleIndex", "Account");
			}
			return View(model);
		}

		public ActionResult RoleAddToUser()
		{
			AssignRoleVM objvm = new AssignRoleVM();
			AccountHelper.InitRoleDropDown(ref objvm);
			AccountHelper.InitUserDropDown(ref objvm);
			return View(objvm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RoleAddToUser(AssignRoleVM objvm)
		{
			if (objvm.RoleName == "0")
			{
				ModelState.AddModelError("", WcResources.RoleNameRequired);
			}
			if (objvm.UserName == "0")
			{
				ModelState.AddModelError("", WcResources.UserNameRequired);
			}

			if (ModelState.IsValid)
			{
				if (Roles.IsUserInRole(objvm.UserName, objvm.RoleName))
				{
					ModelState.AddModelError("", WcResources.UserRoleTaken);
				}
				else
				{
					Roles.AddUserToRole(objvm.UserName, objvm.RoleName);
					ViewBag.ResultMessage = string.Format(WcResources.RoleAddedToUser, objvm.UserName, objvm.RoleName);
				}
			}

			AccountHelper.InitRoleDropDown(ref objvm);
			AccountHelper.InitUserDropDown(ref objvm);
			return View(objvm);
		}
	

		[AllowAnonymous]
		public ActionResult Login(string returnUrl)
		{
			ViewBag.ReturnUrl = returnUrl;
			LoginModel model = new LoginModel()
			{
				// set default = true
				IsConfirmed = true
			};
			return View(model);
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginModel model, string returnUrl)
		{
			string errorMsg = null;
			if (ModelState.IsValid)
			{
				if (WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
				{
					if (returnUrl.ToLower().Contains("confirmationsuccess")) return RedirectToAction("Index", "Home");
					return RedirectToLocal(returnUrl);
				}
				
				errorMsg = WcResources.UserNamePasswordNotMatch;
			}

			// todo: check if user's email is confirmed...
			#region ToDo
			//if (model.IsConfirmed)
			//{
			//	if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
			//	{
			//		return RedirectToLocal(returnUrl);
			//	}
			//	else if (AccountHelper.GetUserFromName(model.UserName) != null && !WebSecurity.IsConfirmed(model.UserName))
			//	{
			//		model.IsConfirmed = false;
			//		errorMsg = WcResources.NotCompleteEmailConfirmParagraph;
			//	}
			//}
			//else //Need to resend confirmation email
			//{
			//	ResendConfirmationEmail(model.UserName);
			//	errorMsg = WcResources.NotCompleteEmailConfirmParagraph + " " + WcResources.ConfirmEmailSentParagraph;
			//	model.IsConfirmed = true;
			//}
			#endregion

			// If we got this far, something failed, redisplay form
			ModelState.AddModelError("", errorMsg);
			ViewBag.returnUrl = returnUrl;
			return View(model);
		}

		
		public ActionResult LogOff()
		{
			WebSecurity.Logout();
			return RedirectToAction("Index", "Home");
		}

		

		[AllowAnonymous]
		public ActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				// Attempt to register the user
				try
				{
					if (bool.Parse(WebConfigurationManager.AppSettings.Get("ReCaptchaEnabled")))
					{
						RecaptchaVerificationHelper recaptchaHelper = this.GetRecaptchaVerificationHelper();
						if (string.IsNullOrEmpty(recaptchaHelper.Response))
						{
							// donetodo: i18n
							ModelState.AddModelError("", WcResources.CaptchaRequired);
							return View(model);
						}
						RecaptchaVerificationResult recaptchaResult = await recaptchaHelper.VerifyRecaptchaResponseTaskAsync();
						if (recaptchaResult != RecaptchaVerificationResult.Success)
						{
							// donetodo: i18n
							ModelState.AddModelError("", WcResources.CaptchaError);
							return View(model);
						}
					}


					if (WebSecurity.UserExists(model.UserName))
					{
						ViewBag.RegisterResult = WcResources.UserNameTaken;
						return View(model);
					}
					string confirmationToken =
						WebSecurity.CreateUserAndAccount(model.UserName, model.Password, new { model.Email }, true);
					SendEmailConfirmation(model.Email, model.UserName, confirmationToken);
					return RedirectToAction("RegisterStepTwo", "Account");
				}
				catch (MembershipCreateUserException e)
				{
					ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
				}
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		private void SendEmailConfirmation(string to, string username, string confirmationToken)
		{
			try
			{
				dynamic email = new Postal.Email("RegEmail");
				email.To = to;
				email.UserName = username;
				email.ConfirmationToken = confirmationToken;
				email.Send();

				//	MailMessage mailMsg = new MailMessage();

				//	// To
				//	mailMsg.To.Add(new MailAddress(to, username));

				//	// From
				//	mailMsg.From = new MailAddress("admin@wordcollocation.net", "WordCollocation.net");

				//	// Subject and multipart/alternative Body
				//	mailMsg.Subject = "Send Email Test";
				//	string text = "text body";
				//	string html = @"<p>Test from 2016 Register</p><div>token: " + confirmationToken + "</div>";
				//	mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
				//	mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

				//	// Init SmtpClient and send
				//	SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
				//	System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("tongling", "a1b2c3d4");
				//	smtpClient.Credentials = credentials;
				//	smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
				//	smtpClient.Send(mailMsg);
			}
			catch (Exception ex)
			{
				log.Error(ex.Message, ex);
				Console.WriteLine(ex.Message);
			}
		}

		private void ResendConfirmationEmail(string username)
		{
			UserProfile user = AccountHelper.GetUserFromName(username);
			string token = AccountHelper.GetConfirmTokenFromUser(user.UserId);
			SendEmailConfirmation(user.Email, username, token);

		}

		//[HttpPost]
		//   public ActionResult Register(Register register)
		//   {
		//    if (ModelState.IsValid)
		//    {
		//	    if (!WebSecurity.UserExists(register.Email))
		//	    {
		//		    WebSecurity.CreateUserAndAccount(register.Email, register.Password,
		//			    new {register.FirstName, register.LastName, register.DisplayName});
		//		    Response.Redirect("~/Account/Login");
		//	    }
		//    }
		//    else
		//    {
		//	    ModelState.AddModelError("Error", ThResources.PleaseEnterAllData);
		//    }
		//    return View();
		//   }

		[AllowAnonymous]
		public ActionResult RegisterStepTwo()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult RegisterConfirmation(string Id)
		{
			if (WebSecurity.ConfirmAccount(Id))
			{
				return RedirectToAction("ConfirmationSuccess");
			}
			return RedirectToAction("ConfirmationFailure");
		}

		[AllowAnonymous]
		public ActionResult ConfirmationSuccess()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult ConfirmationFailure()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult ResetPassword()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ResetPassword(ResetPasswordModel model)
		{
			string username;
			// doneTODO: check if email confirmed
			if (AccountHelper.CheckIfUserEmailConfirmed(model.Email))
			{
				username = AccountHelper.GetUserNameFromEmail(model.Email);
				if (!string.IsNullOrEmpty(username))
				{
					string confirmationToken = WebSecurity.GeneratePasswordResetToken(username);
					dynamic email = new Postal.Email("ChngPasswordEmail");
					email.To = model.Email;
					email.UserName = username;
					email.ConfirmationToken = confirmationToken;
					email.Send();

					return RedirectToAction("ResetPwdStepTwo");
				}
				return RedirectToAction("InvalidUserName");
			}
			else
			{
				username = AccountHelper.GetUserNameFromEmail(model.Email);
				ResendConfirmationEmail(username);
				return RedirectToAction("ResetPwdEmailConfirmPending");
			}
		}

		[AllowAnonymous]
		public ActionResult ResetPwdEmailConfirmPending()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult InvalidUserName()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult ResetPwdStepTwo()
		{
			return View();
		}

		[AllowAnonymous]
		[HttpPost]
		public ActionResult ResetPasswordConfirmation(ResetPasswordConfirmModel model)
		{
			if (WebSecurity.ResetPassword(model.Token, model.NewPassword))
			{
				return RedirectToAction("PasswordResetSuccess");
			}
			return RedirectToAction("PasswordResetFailure");
		}

		[AllowAnonymous]
		public ActionResult ResetPasswordConfirmation(string Id)
		{
			ResetPasswordConfirmModel model = new ResetPasswordConfirmModel() { Token = Id };
			return View(model);
		}

	
		#region Helpers
		private ActionResult RedirectToLocal(string returnUrl)
		{
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("Index", "Home");
			}
		}

		public enum ManageMessageId
		{
			ChangePasswordSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
		}

		internal class ExternalLoginResult : ActionResult
		{
			public ExternalLoginResult(string provider, string returnUrl)
			{
				Provider = provider;
				ReturnUrl = returnUrl;
			}

			public string Provider { get; }
			public string ReturnUrl { get; }

			public override void ExecuteResult(ControllerContext context)
			{
				OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
			}
		}

		private static string ErrorCodeToString(MembershipCreateStatus createStatus)
		{
			// See http://go.microsoft.com/fwlink/?LinkID=177550 for
			// a full list of status codes.
			switch (createStatus)
			{
				case MembershipCreateStatus.DuplicateUserName:
					return "User name already exists. Please enter a different user name.";

				case MembershipCreateStatus.DuplicateEmail:
					return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

				case MembershipCreateStatus.InvalidPassword:
					return "The password provided is invalid. Please enter a valid password value.";

				case MembershipCreateStatus.InvalidEmail:
					return "The e-mail address provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidAnswer:
					return "The password retrieval answer provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidQuestion:
					return "The password retrieval question provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidUserName:
					return "The user name provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.ProviderError:
					return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				case MembershipCreateStatus.UserRejected:
					return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				default:
					return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
			}
		}
		#endregion

		public ActionResult PasswordResetSuccess()
		{
			return View();
		}

		public ActionResult PasswordResetFailure()
		{
			return View();
		}
	}
}