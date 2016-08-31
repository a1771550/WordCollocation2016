<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckEmail.aspx.cs" Inherits="UI.Email" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Check Email</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
	    <asp:Button runat="server" ID="btnCheck" OnClick="btnCheck_OnClick"/>
		<asp:Label runat="server" ID="Output"></asp:Label>
    </div>
    </form>
</body>
</html>
