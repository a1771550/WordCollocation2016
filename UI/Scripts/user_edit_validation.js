window.canSubmit = true;
window.isEmailTaken = false;

$(function ()
{
	$('.iOk').hide();

	$('#UserName')
		.bind('paste change',
			function ()
			{
				checkUserNameTaken('UserName');
			});

	$('#Email').bind('paste change', function ()
	{
		checkEmail('Email');
	});
});

function checkEmailFormat(emailFieldName)
{
	var email = document.getElementById(emailFieldName).value;
	var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
	return filter.test(email);
}

function checkUserNameTaken(usernameFieldName)
{
	var username = document.getElementById(usernameFieldName).value;
	//console.log('username: ' + username);

	if (username !== '')
	{
		$('#UserName-error').hide();

		if (username.toLowerCase() !== $('#OriginalUserName').val().toLowerCase())
		{
			var param = { username: username };
			var data = JSON.stringify(param);
			$.ajax({
				url: "/WebServices/WcServices.asmx/CheckUserNameTaken",
				type: "POST",
				data: data,
				dataType: "json",
				async: true,
				contentType: "application/json; charset=utf-8",
				success: function (msg)
				{
					console.log("msg: " + msg + ";type of msg: " + typeof (msg));
					var data = msg.hasOwnProperty("d") ? msg.d : msg;
					if (data === true)
					{
						msg = $('#UserNameTaken').val();
						console.log('username taken value: ' + msg);
						$('#UserName-error').text(msg);
						$('#iUserNameOk').hide();
						window.canSubmit = false;
					} else
					{
						$('#UserName-error').hide();
						$('#iUserNameOk').show();
					}
				},
				error: function (msg)
				{
					console.log('error: ' + msg);
					//window.isEmailTaken = true;
				}
			});
		}

	} else
	{
		var msg = $('#UserNameRequired').val();
		$('#UserName-error').text(msg);
		$('#iUserNameOk').hide();
		window.canSubmit = false;
	}


}

function checkEmail(emailFieldName)
{
	var email = document.getElementById(emailFieldName).value;
	//console.log('email: ' + email);
	//window.isEmailTaken = false;

	if (email !== '')
	{
		$('#Email-error').hide();

		if (email.toLowerCase() !== $('#OriginalEmail').val().toLowerCase())
		{
			var param = { email: email };
			var data = JSON.stringify(param);
			$.ajax({
				//url: 'http://localhost:50370/account/Checkemailtaken',
				//url: "/account/checkemailtaken",
				url: "/WebServices/WcServices.asmx/CheckEmailTaken",
				type: "POST",
				data: data,
				dataType: "json",
				//async: false, //Note: must be false so that it can return global value...
				async: true,
				contentType: "application/json; charset=utf-8",
				success: function (msg)
				{
					//console.log("msg: " + msg + ";type of msg: "+typeof (msg));
					var data = msg.hasOwnProperty("d") ? msg.d : msg;
					if (data === true)
					{
						msg = $('#EmailTaken').val();
						//console.log('email taken value: ' + msg);
						$('#Email-error').text(msg);
						$('#iEmailOk').hide();
						window.canSubmit = false;
					} else
					{
						//$('#iEmailOk').show();
						if (checkEmailFormat('Email'))
						{
							//console.log('email OK');
							$('#iEmailOk').show();
							//$('#Email-error').text('');
							$('#Email-error').hide();
						} else
						{
							$('#iEmailOk').hide();
							window.canSubmit = false;
							//console.log('email error!');
							msg = $('#EmailErrMsg').val();
							$('#Email-error').text(msg);
						}
					}
				},
				error: function (msg)
				{
					console.log('error: ' + msg);
					//window.isEmailTaken = true;
				}
			})
				.done(function ()
				{
					//console.log('return isTaken: ' + window.isEmailTaken);
					//return window.isEmailTaken;
					//$(emailFieldName).append('<span id=\"isEmailTaken\" style=\"display:none;\">' + window.isEmailTaken + '</span>');
				});
		}


	} else
	{
		var msg = $('#EmailRequired').val();
		$('#Email-error').text(msg);
		$('#iEmailOk').hide();
		window.canSubmit = false;
	}
}