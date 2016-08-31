<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EncryptDecryptDemo.aspx.cs" Inherits="UI.Demo.EncryptDecryptDemo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Encrypt Decrypt Demo</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
	    Email: <asp:TextBox runat="server" id="txtEmail"></asp:TextBox>
	    <br />
        Password: <asp:TextBox runat="server" id="txtPassword"></asp:TextBox>
        <br/>
        <asp:Button runat="server" id="btnEncrpyt" OnClick="btnEncrpyt_OnClick" text="Encrypt"/>
        <br/>
		<asp:Label runat="server" id="lblEncrypted"></asp:Label>
		<br />
		<asp:Button runat="server" id="btnDecrpty" text="Decrpty" OnClick="btnDecrpty_OnClick"/>
        <asp:Label runat="server" id="lblResult"></asp:Label>
    </div>
    </form>
</body>
</html>
