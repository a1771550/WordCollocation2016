$(document)
			.ready(function ()
			{
				if ($('#btnLogOff').length) {
					$('#btnLogOff')
					.click(function (e)
					{
						e.preventDefault();
						Cookies.remove(".WcAuthentication");
						//Cookies.remove("timezoneoffset");
						console.log('auth cookie removed');
						//$('form#logoutForm').submit();
					});
				}
				
			});