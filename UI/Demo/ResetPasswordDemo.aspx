<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPasswordDemo.aspx.cs" Inherits="UI.Demo.ResetPasswordDemo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reset Password Demo</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
	Email:	<asp:TextBox runat="server" id="txtEmail"></asp:TextBox>
		<br/>
		<asp:Button runat="server" id="btnEmail" text="Submit" OnClick="btnEmail_OnClick"/>
    </div>
    </form>
</body>
</html>
