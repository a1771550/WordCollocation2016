$(function ()
{
	var timezone_cookie = "timezoneoffset";

	if (Cookies.get(timezone_cookie)==='undefined')
	{ // if the timezone cookie not exists create one.

		// check if the browser supports cookie
		var test_cookie = 'test cookie';
		Cookies.set(test_cookie, true);
		//$.cookie(test_cookie, true);

		if (Cookies.get(test_cookie)!=='undefined')
		{ // browser supports cookie

			// delete the test cookie.
			Cookies.remove(test_cookie);

			// create a new cookie
			Cookies.set(timezone_cookie, new Date().getTimezoneOffset());

			location.reload(); // re-load the page
		}
	}
	else
	{ // if the current timezone and the one stored in cookie are different then
		// store the new timezone in the cookie and refresh the page.

		var storedOffset = parseInt(Cookies.get(timezone_cookie));
		var currentOffset = new Date().getTimezoneOffset();

		if (storedOffset !== currentOffset)
		{ // user may have changed the timezone
			Cookies.set(timezone_cookie, new Date().getTimezoneOffset());
			location.reload();
		}
	}
});