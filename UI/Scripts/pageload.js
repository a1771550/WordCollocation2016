function getCurrLang()
{
	var currLang = $('#currLang').text().toLowerCase();
	var lang=null;
	switch (currLang)
	{
		case 'zh-hant':
			lang = 'tw'; break;
		case 'zh-hans':
			lang = 'cn'; break;
		case 'ja-jp':
			lang = 'ja'; break;
		/*default:
			lang = 'tw';break;*/
	}
	return lang;
}

String.format = function ()
{
	var s = arguments[0];
	for (var i = 0; i < arguments.length - 1; i++)
	{
		var reg = new RegExp("\\{" + i + "\\}", "gm");
		s = s.replace(reg, arguments[i + 1]);
	}

	return s;
}

String.prototype.endsWith = function (suffix)
{
	return (this.substr(this.length - suffix.length) === suffix);
}

String.prototype.startsWith = function (prefix)
{
	return (this.substr(0, prefix.length) === prefix);
}