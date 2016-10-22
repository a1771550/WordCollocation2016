<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CallJsonDemo.aspx.cs" Inherits="UI.CallJsonDemo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Call Json Demo</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
	<script src="/Scripts/jquery-2.1.4.js"></script>
	<script>
	
		$(document)
			.ready(function() {
				$.getJSON("http://www.translationhall.com/api/web/pos?callback=MyCallback", function (data)
				{
					var items = [];
					$.each(data, function (key, val)
					{
						items.push("<li id='" + key + "'>" + val + "</li>");
					});

					$("<ul/>", {
						"class": "my-new-list",
						html: items.join("")
					}).appendTo("body");
				});
			});
		
	</script>
</body>
</html>
