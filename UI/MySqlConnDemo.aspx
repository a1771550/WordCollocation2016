<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MySqlConnDemo.aspx.cs" Inherits="UI.MySqlConnDemo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MySql Conn Demo</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
		<asp:GridView runat="server" ID="gvPos" AllowPaging="True" AllowSorting="True" 
			DataSourceID="SqlDataSource1" pages="5"></asp:GridView>
    	<asp:SqlDataSource ID="SqlDataSource1" runat="server"
			ConnectionString="<%$ ConnectionStrings:MySqlPosConn %>"
			ProviderName="<%$ ConnectionStrings:MySqlPosConn.ProviderName %>"
			SelectCommand="SELECT Id, Entry, EntryZht, EntryZhs, EntryJap FROM pos">
		</asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
