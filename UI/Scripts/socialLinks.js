(function ($)
{
	$("a.popup").click(function (e) {
		e.preventDefault();
		popupwindowInCentral($(this).attr("href"), "_blank", 500, 500);
	});
})(jQuery);

function popupwindowInCentral(url, title, w, h)
{
	var left = (screen.width / 2) - (w / 2);
	var top = (screen.height / 2) - (h / 2);
	var newWindow = window.open(url, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);
	if (window.focus)
	{
		newWindow.focus();
	}
	return false;
}