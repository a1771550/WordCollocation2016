// JS error handler
window.onerror = function (msg, url, line)
{
	if (encodeURIComponent)
	{
		return loadXMLDoc("/Error/JSErrorHandler", "msg=" + encodeURIComponent(msg) + '&url=' + encodeURIComponent(url) + "&line=" + line);
	};
	return false;
};
// Ajax request
function loadXMLDoc(url, params)
{
	var req;
	if (window.XMLHttpRequest)
	{
		try
		{
			req = new XMLHttpRequest();
		} catch (e)
		{
			req = false;
		}
	} else
		if (window.ActiveXObject)
		{
			try
			{
				req = new ActiveXObject("Msxml2.XMLHTTP");
			} catch (e)
			{
				try
				{
					req = new ActiveXObject("Microsoft.XMLHTTP");
				} catch (e)
				{
					req = false;
				}
			}
		}

	if (req)
	{
		req.open("POST", url, true);
		req.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
		req.send(params);
		return true;
	}
	return false;
}