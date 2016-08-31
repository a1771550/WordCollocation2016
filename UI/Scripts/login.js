//$(".fakefield").show();
// some DOM manipulation/ajax here
$(document)
	.ready(function ()
	{
		if ($('.fakefield').length) {
			window.setTimeout(function ()
			{
				$(".fakefield").remove();
			}, 1000);
		}
		
	});
