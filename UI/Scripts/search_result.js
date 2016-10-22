var currLang = ($('#currLang').text()).toLowerCase();
var colwordTran = null;
var wordTran = null;
var posTran = null;
var colposTran = null;
var exTran = null;

// define an collocation class: word, pos, colword, colpos, colpattern
function collocation(pos, word, colpos, colword, colpattern)
{
	this.pos = pos;
	this.word = word;
	this.colpos = colpos;
	this.colword = colword;
	this.colpattern = colpattern;
}

// define an example class
function example(entry, entryTran, source, remark)
{
	this.entry = entry;
	this.entryTran = entryTran;
	this.source = source;
	this.remark = remark;
}

// define colpattern enum
var colpattern = {
	NOUN_VERB: 1,
	VERB_NOUN: 2,
	ADJECTIVE_NOUN: 3,
	VERB_PREPOSITION: 4,
	PREPOSITION_VERB: 5,
	ADVERB_VERB: 6,
	PHRASE_NOUN: 7,
	PREPOSITION_NOUN: 8,
	ADJECTIVE_PHRASE: 9
};

// define source enum
var Source = {
	OXFORD_COLLOCATIONS_DICTIONARY: 1,
	NEW_DICTIONARY_OF_ENGLISH_COLLOCATIONS: 2,
	NEWSPAPER: 3,
	WEB: 4,
	FICTION: 5,
	OTHERS: 6
};

if (Cookies.get('searchResult') !== undefined)
{
	//console.log('ready to read result cookie...');
	var data = Cookies.getJSON('searchResult');
	//var data = $.parseJSON(result);
	$.each(data,
		function ()
		{
			var obj = this;
			//document.write(obj.pos + " " + obj.colpos + " " + obj.word + " " + obj.wordTran + " " + obj.colword + " " + obj.colwordTran + " " + obj.ex + " " + obj.exTran + "<br>");
			document
				.write("<div class=\"paddingLeftDot3em\"><table id=\"ColWordList\" class=\"table table-striped\" style=\"width: 99%\"><tbody><tr><td><div class=\"colWordDiv indent-1em\"><h5><i class=\"halflings-icon share-alt\"></i>&nbsp;<span class=\"colWordIcon\">");
			switch (currLang)
			{
				case 'zh-hant':
					colwordTran = obj.colwordZht;
					break;
				case 'zh-hans':
					colwordTran = obj.colwordZhs;
					break;
				case 'ja-jp':
					colwordTran = obj.colwordJap;
					break;
			}
			document.write(obj.colword);
			document.write("</span>&nbsp;<span class=\"colWordTrans\">");
			document.write(colwordTran);
			document.write("</span></h5><div class=\"marginTop1em marginBottom05em\"><span class=\"exampleLabel\">");
			document.write($('#exampleText').text());
			document.write("</span></div>");

			switch (currLang)
			{
				case 'zh-hant':
					exTran = obj.exZht;
					break;
				case 'zh-hans':
					exTran = obj.exZhs;
					break;
				case 'ja-jp':
					exTran = obj.exJap;
					break;
			}
			// create an example object...
			//var example = { entry: obj.entry, entryTran: obj.entryTran, source: obj.source, remark: obj.remark };
			var ex = new example(obj.ex, exTran, obj.source, obj.remark);

			// create an collocation object: word, pos, colword, colpos, colpattern
			var col = new collocation(obj.pos, obj.word, obj.colpos, obj.colword, obj.colpattern);

			var sourceRemark = getSourceRemark(ex);

			/* TODO: format example */
			var formattedExample = getFormatedExample(ex, col);

			document
				.write("<table id=\"ExampleList\" class=\"table table-striped table-hover\"><tbody><tr><td><div class=\"exampleDiv\"><div class=\"indent-1em marginBottom05em exampleText\">" + obj.ex + "</div><div class=\"indent-1em marginBottom05em exampleTransText\">" + exTran + "</div><div class=\"indent-1em exampleSourceText\">" + sourceRemark + "</div></div></td></tr></tbody></table>");

			document.write("</div></td></tr></tbody></table></div>");

		});
	//Cookies.remove('searchResult'); don't remove that cookie here; otherwise such page reload as change language will generate errors...
}

function getSourceRemark(example)
{
	var ret;
	var sourceText = null;
	//console.log('source: ' + example.source);
	if (example.source !== '')
	{
		var source = parseInt(example.source);
		switch (source)
		{
			case Source.OXFORD_COLLOCATIONS_DICTIONARY:
				sourceText = String.format("<a href='{0}' target='_blank'>{1}</a>",
					$('#oxfordDictUrl').text(),
					$('#oxfordDictText').text());
				break;
			case Source.NEW_DICTIONARY_OF_ENGLISH_COLLOCATIONS:
				sourceText = String.format("<a href='{0}' target='_blank'>{1}</a>", $('#chDictUrl').text(), $('#chDictUrl').text());
				break;
			case Source.NEWSPAPER:
				sourceText = $('#npText').text();
				break;
			case Source.WEB:
				sourceText = $('#webText').text();
				break;
			case Source.FICTION:
				sourceText = $('#fictText').text();
				break;
			case Source.OTHERS:
				sourceText = $('#othersText').text();
				break;
		}
		//console.log('source text: ' + sourceText);
		if (example.remark === '')
			ret = String.format("({0}{1})", $('#sourceText').text(), sourceText);
		else ret = String.format("({0}{1};{2}:{3})", $('#sourceText').text(), sourceText, $('#remarkText').text(), example.remark);

	} else
	{
		sourceText = $('#webText').text(); //default as Web for the source, so as to be more efficient when creating or editing
		ret = String.format("({0}{1})", $('#sourceText').text(), sourceText);
	}
	return ret;
}

/* TODO: to be implemented... */
function getFormatedExample(example, collocation)
{
	//string word = collocation.Word.Entry;
	//string pos = collocation.Word.pos.Entry;
	//string colWord = collocation.colword.Entry;
	//string colpos = collocation.colword.pos.Entry;
	//return FormatExampleForView(example, word, pos, colWord, colpos, (CollocationPattern) collocation.CollocationPattern);

}