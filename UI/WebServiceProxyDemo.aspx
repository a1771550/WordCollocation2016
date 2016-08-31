<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebServiceProxyDemo.aspx.cs" Inherits="UI.WebServiceProxyDemo" ClientIDMode="Static" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WebService Proxy Demo</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Name:
		<asp:TextBox runat="server" ID="txtName"></asp:TextBox>
		<br />
		<br />
		<asp:Button runat="server" ID="btnCheck" Text="Check Name" />
		<br />
		<br />
		<asp:Label runat="server" ID="lblResult"></asp:Label>
    </div>
    </form>
	<script src="https://code.jquery.com/jquery-2.1.1.js"></script>
	<script>
		(function ($)
		{
			$("#btnCheck").click(function (e)
			{
				e.preventDefault();
				checkIfDuplicatedUserName();
			});
		})(jQuery);

		function checkIfDuplicatedUserName()
		{
			var name = $("#txtName").val();
			//http://www.translationhall.com/webservices/wcservices.asmx?op=CheckIfDuplicatedUserName
			var url = "http://www.translationhall.com/webservices/wcservices.asmx/CheckIfDuplicatedUserName";

			$.ajax({
				type: 'GET',
				url: url,
				crossDomain: true,
				contentType: "application/json; charset=utf-8",
				data: { name: name },
				dataType: "jsonp",
				success: function (msg) {
					console.debug(msg);
					//$.each(msg, function (name, value)
					//{
					//	alert(value);
					//});
					alert(msg);
				},
				error: function (xhr, status, error) { alert('Servidor de error 404 !!'); },
				async: false,
				cache: false
			});
		}


		//if ((xmlhttprequest.readyState == 4) && (xmlhttprequest.status == 200)) {

		//	var myXml = xmlhttprequest.responseXML;
		//	var xmlobject = null;
		//	var XMLText = null;
		//	if (window.ActiveXObject) {
		//		XMLText = myXml.childNodes[1].firstChild.nodeValue;
		//	} else {

		//		XMLText = myXml.childNodes[0].firstChild.nodeValue;
		//	}
		//}
	</script>
</body>
</html>
