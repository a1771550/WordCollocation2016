$(document)
			.ready(function ()
			{
				//var lang = getCurrLang();
				//var url = 'http://www.translationhall.com/api/web/pos/list?lang=' + lang + '&callback=MyCallback';
				//var script = document.createElement('script');
				//script.type = 'text/javascript';
				//script.src = url;
				//$("body").append(script);
			});


function MyCallback(data)
{
	//console.log(data);
	var currLang = getCurrLang();
	var options = [];
	var selectText = '- ' + $('#selectColPosText').text() + ' -';
	if (Cookies.get('posList') === undefined)
		Cookies.set('posList', JSON.stringify(data), { expires: 365 });

	$.each(data,
		function ()
		{
			var obj = this;
			switch (currLang)
			{
				case 'tw':
					options.push({
						value: obj.Id,
						text: obj.Entry + ' ' + obj.EntryZht
					}
					);
					break;
				case 'cn':
					options.push({
						value: obj.Id,
						text: obj.Entry + ' ' + obj.EntryZhs
					}
					);
					break;
			}
		});

	var s = $("<select />", { 'class': 'text fLeft', 'id': 'ColPosId', 'name': 'ColPosId' });

	$('<option />', { 'value': '0', 'text': selectText, selected: true }).prependTo(s);

	for (var val in options)
	{
		if (options.hasOwnProperty(val))
		{
			//console.log('val: ' + val);
			//console.log('text: ' + options[val]);
			$('<option />', { value: options[val].value, text: options[val].text }).appendTo(s);

		}
	}
	s.appendTo('#lblColPos');
}