$(function ()
{
	var maskedPassword = new MaskedPassword(document.getElementById("Password"), '\u25CF', '');
	var maskedConPassword = new MaskedPassword(document.getElementById("ConfirmPassword"), '\u25CF', '');

	maskedPassword.addListener(document.getElementById("Password"), 'change', pwdHandler);
	maskedPassword.addListener(document.getElementById("Password"), 'paste', pwdHandler);
	maskedPassword.addListener(document.getElementById("Password"), 'blur', pwdHandler);

	maskedConPassword.addListener(document.getElementById("ConfirmPassword"), 'change', conPwdHandler);
	maskedConPassword.addListener(document.getElementById("ConfirmPassword"), 'paste', conPwdHandler);
	maskedConPassword.addListener(document.getElementById("ConfirmPassword"), 'blur', conPwdHandler);

	$('#btnSubmit').click(function (e)
	{
		e.preventDefault();

		if ($('#Password').val() === '' || $('#Password').val() === null)
		{
			msg = $('#PwdRequired').val();
			$('#Password-error').text(msg);

			window.canSubmit = false;
		}

		if ($('#ConfirmPassword'))
		{
			if ($('#ConfirmPassword').val() === '' || $('#ConfirmPassword').val() === null)
			{
				msg = $('#ConfirmPwdRequired').val();
				$('#ConfirmPassword-error').text(msg);
				window.canSubmit = false;
			}
		}

		if ($('#Email').val() === '' || $('#Email').val() === null)
		{
			msg = $('#EmailRequired').val();
			$('#Email-error').text(msg);
			window.canSubmit = false;
		}

		if ($('#UserName').val() === '' || $('#UserName').val() === null)
		{
			msg = $('#UserNameRequired').val();
			$('#UserName-error').text(msg);
			window.canSubmit = false;
		}

		//if ($('#LastName').val() === '' || $('#LastName').val() === null)
		//{
		//	msg = $('#LastNameRequired').val();
		//	$('#LastName-error').text(msg);
		//	window.canSubmit = false;
		//}

		if (canSubmit)
		{
			$('#UserForm').submit();
		}
	});

});


function pwdHandler()
{
	checkPwdLen(false);
	if ($('#ConfirmPassword').val().length > 0)
	{
		checkIfPwdConPwdMatch();
	}
}

function conPwdHandler()
{
	checkPwdLen(true);
	checkIfPwdConPwdMatch();
}