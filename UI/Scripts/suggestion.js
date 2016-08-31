var wordSuggestion, ok, cancel;
(function ($)
{
	wordSuggestion = $("input#wordSuggestion").val();
	ok = $("input#ok").val();
	cancel = $("input#cancel").val();
	setWord("");

	$("div#WordSuggestion").hide();

	$("a.lnkWord").click(function (e)
	{
		e.preventDefault();
		var word = $(this).text();
		//console.debug("word: " + word);
		setWord(word);
		setDropDownList(word);
	});

	$("input#Word").click(function (e)
	{
		$(this).val("");
		showSuggestion();
	});
})(jQuery);

var showSuggestion = function ()
{
	$("div#WordSuggestion").show();

	$("div#WordSuggestion").dialog({
		closeOnEscape: true,
		open: function (event, ui) { $(".ui-dialog-titlebar-close", ui.dialog || ui).show(); },
		dialogClass: "modal-dialog-suggestion",
		buttons: [
			{
				text: ok,
				"class":'Add',
				click: function ()
				{
					// set default value
				if ($("input#Word").val() == "") {
					$("input#Word").val("abandon");
					setDropDownList("abandon");
				}
				$(this).dialog("close");
			} },
			{
				text: cancel,
				"class":'Add',
				click: function ()
				{
					setWord(""); $(this).dialog("close");
				}
			}
		],
		modal: true,
		width: 300,
		height: 'auto',
		title: wordSuggestion,
	});

	setTitleSize();
};

function setTitleSize()
{
	//.modal-dialog-suggestion .ui-dialog-title
	var ts = $("#titleSize").val();
	$(".modal-dialog-suggestion").find(".ui-dialog-title").css("font-size", ts);
};

function setWord(w)
{
	$("input#Word").val(w);
};

function setDropDownList(w)
{
	var wl = w.toLowerCase();
	/*
	<li><a href="#" class="lnkWord" tabindex="-1">abandon</a></li>
	<li><a href="#" class="lnkWord" tabindex="-1">ability</a></li>
	<li><a href="#" class="lnkWord" tabindex="-1">absence</a></li>
	<li><a href="#" class="lnkWord" tabindex="-1">look</a></li>
	*/
	switch (wl)
	{
		case "abandon":
			$("#ColPosId").val("4");
			break;
		case "ability":
			$("#ColPosId").val("2");
			break;
		case "absence":
			$("#ColPosId").val("3");
			break;
		case "look":
			$("#ColPosId").val("2");
			break;
	}
}