$(function ()
{
	var timezone_cookie = "timezoneoffset";

	if (!$.cookie(timezone_cookie))
	{ // if the timezone cookie not exists create one.

		// check if the browser supports cookie
		var test_cookie = 'test cookie';
		$.cookie(test_cookie, true);

		if ($.cookie(test_cookie))
		{ // browser supports cookie

			// delete the test cookie.
			$.cookie(test_cookie, null);

			// create a new cookie
			$.cookie(timezone_cookie, new Date().getTimezoneOffset());

			location.reload(); // re-load the page
		}
	}
	else
	{ // if the current timezone and the one stored in cookie are different then
		// store the new timezone in the cookie and refresh the page.

		var storedOffset = parseInt($.cookie(timezone_cookie));
		var currentOffset = new Date().getTimezoneOffset();

		if (storedOffset !== currentOffset)
		{ // user may have changed the timezone
			$.cookie(timezone_cookie, new Date().getTimezoneOffset());
			location.reload();
		}
	}
});