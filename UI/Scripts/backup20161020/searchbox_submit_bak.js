var progressTitle;

(function ($)
{
	// don't move this code...
	if ($("div#progress").length) $("div#progress").hide();
	$("a#btnSearch").click(function (e) {
		//console.log('btnSearch clicked...');
		e.preventDefault();
		var wordrequired = $("#wordRequired").val();
		var colposrequired = $("#colPosRequired").val();
		progressTitle = $("#progressTitle").val();
		var word = $("input[name='Word']").val();
		var submit = true;
		if (word === "")
		{
			alert(wordrequired);
			$("input[name='Word']").focus();
			submit = false;
		}
		var id = $("select#ColPosId").val();
		if (id === "0")
		{
			alert(colposrequired);
			$("select#ColPosId").focus();
			submit = false;
		}

		//console.log('submit: ' + submit);

		if (submit)
		{
			showProgress();
			//var lang = getCurrLang();
			var url = 'http://www.translationhall.com/api/web/collocation/search' + 'word='+ word + '&id='+ id + '&callback=SearchResult';
			var script = document.createElement('script');
			script.type = 'text/javascript';
			script.src = url;
			$("body").append(script);
		}
	});

	
})(jQuery);

function SearchResult(data) {
	hideProgress();
	if (data !== null)
	{
		//string pos, string posTran, string word, string wordTran, string colpos, string colposTran, int collocationPattern
		Cookies.set('pos', data[0].pos);
		Cookies.set('posZht', data[0].posZht);
		Cookies.set('posZhs', data[0].posZhs);
		Cookies.set('posJap', data[0].posJap);
		Cookies.set('word', data[0].word);
		Cookies.set('wordZht', data[0].wordZht);
		Cookies.set('wordZhs', data[0].wordZhs);
		Cookies.set('wordJap', data[0].wordJap);
		Cookies.set('colpos', data[0].colpos);
		Cookies.set('colposZht', data[0].colposZht);
		Cookies.set('colposZhs', data[0].colposZhs);
		Cookies.set('colposJap', data[0].colposJap);
		Cookies.set('colpattern', data[0].colpattern);
		Cookies.set('colcount', data.length);
		Cookies.set('searchResult', JSON.stringify(data));
		console.log('data saved in cookies...');
		window.location.assign($('#searchResultUrl').text());
	} else
	{
		console.log('nothing found');
		window.location.assign($('#noSearchResultUrl').text());
	}

	
}

function showProgress()
{
	$("div#progress").show();
	$("div#progress").dialog({
		closeOnEscape: false,
		open: function (event, ui) { $(".ui-dialog-titlebar-close", ui.dialog || ui).show(); },
		dialogClass: "modal-dialog",
		buttons: {},
		modal: true,
		width: 150,
		height: 'auto',
		title: progressTitle
	});
}

function hideProgress() {
	$("div#progress").hide();
	$('div#progress').dialog('close');
}