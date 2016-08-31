window.canSubmit = true;
var msg;

$(function ()
{
	$('#iEmailOk').hide();

	$('#Email').bind('paste change', function ()
	{
		checkEmailFormat('Email');
	});

	$('#btnSubmit').click(function (e)
	{
		e.preventDefault();

		if (checkEmailValue())
		{
			checkEmailFormat('Email');
		}

		if (canSubmit) {
			//console.log('cansubmit=true');
			$('#forget-form').submit();
		}
	});

});

function checkEmailValue()
{
	if ($('#Email').val() === '' || $('#Email').val() === null)
	{
		msg = $('#EmailRequired').val();
		$('#Email-error').text(msg);
		window.canSubmit = false;
		return false;
	}
	return true;
}

function checkEmailFormat(emailFieldName)
{
	var email = document.getElementById(emailFieldName).value;
	var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
	//console.log('checkEmailFormat called...');
	var ok = filter.test(email);
	if (ok)
	{
		//console.log('email OK');
		$('#iEmailOk').show();
		$('#Email-error').hide();
	} else
	{
		$('#iEmailOk').hide();
		window.canSubmit = false;
		//console.log('email error!')
		msg = $('#EmailErrMsg').val();
		$('#Email-error').text(msg);
	}
}