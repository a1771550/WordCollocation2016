$(function () {
	//var originalPwd = $('#originalPwd').val();
	//var maskedPassword = new MaskedPassword(document.getElementById("Password"), '\u25CF', originalPwd);

	//maskedPassword.addListener(document.getElementById("Password"), 'change', pwdHandler);
	//maskedPassword.addListener(document.getElementById("Password"), 'paste', pwdHandler);
	//maskedPassword.addListener(document.getElementById("Password"), 'blur', pwdHandler);
	var msg = null;

	$('#btnSubmit').click(function (e)
	{
		e.preventDefault();

		if ($('Password').length) {
			if ($('#Password').val() === '' || $('#Password').val() === null)
			{
				msg = $('#PwdRequired').val();
				$('#Password-error').text(msg);

				window.canSubmit = false;
			} else
			{
				checkPwdLen(false);
			}
		}
		

		if ($('#UserName').val() === '' || $('#UserName').val() === null)
		{
			msg = $('#UserNameRequired').val();
			$('#UserName-error').text(msg);
			window.canSubmit = false;
		}

		if ($('#Email').val() === '' || $('#Email').val() === null)
		{
			msg = $('#EmailRequired').val();
			$('#Email-error').text(msg);
			window.canSubmit = false;
		}

		if (canSubmit)
		{
			console.log('SelectedRoles: ' + $('input[name=SelectedRoles]').val());
			$('#UserForm').submit();
		}
	});

});


function pwdHandler()
{
	checkPwdLen(false);
}
