var seconds = 7;
var i = 0;
(function ($)
{
	ShowCurrentTime();
})(jQuery);

function ShowCurrentTime()
{
	document.getElementById("timeout").innerHTML = seconds - i;
	i++;
	if (i == seconds)
	{
		setTimeout("location.href='/'", 0);
	}
	window.setTimeout("ShowCurrentTime()", 1000);
}