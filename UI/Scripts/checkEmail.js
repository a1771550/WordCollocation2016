function checkEmailFormat(emailFieldName) {
	var email = document.getElementById(emailFieldName).value;
	var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
	return filter.test(email);
}

function checkEmailTaken(emailFieldName) {
	var email = document.getElementById(emailFieldName).value;
	//console.log('email: ' + email);
	//window.isEmailTaken = false;
	var param = { email: email };
	var data = JSON.stringify(param);
	$.ajax({
		//url: 'http://localhost:50370/account/Checkemailtaken',
		//url: "/account/checkemailtaken",
		url: "/WebServices/WcServices.asmx/CheckEmailTaken",
		type:"POST",
		data: data,
		dataType:"json",
		async:false, //Note: must be false so that it can return global value...
		//async: true,
		contentType: "application/json; charset=utf-8",
		success:function(msg) {
			//console.log("msg: " + msg + ";type of msg: "+typeof (msg));
			var data = msg.hasOwnProperty("d") ? msg.d : msg;
			if (data === true) {
				window.isEmailTaken = true;
			} else window.isEmailTaken = false;
		},
		error:function(msg) {
			console.log('error: ' + msg);
			window.isEmailTaken = true;
		}
	}).done(function() {
		//console.log('return isTaken: ' + window.isEmailTaken);
		//return window.isEmailTaken;
		//$(emailFieldName).append('<span id=\"isEmailTaken\" style=\"display:none;\">' + window.isEmailTaken + '</span>');
	});

	//console.log('return isTaken: ' + isTaken);
	//return isTaken;
}