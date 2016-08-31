<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Example.aspx.cs" Inherits="UI.Admin.WordCollocation.Example" %>
<%@ Register TagPrefix="uc2" TagName="entitylist" Src="~/Controls/EntityList.ascx" %>
<%@ Register TagPrefix="uc1" TagName="searchbox" Src="~/Controls/SearchBox.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="mainContent">
		<uc1:searchbox id="SearchBox1" runat="server" />
		&nbsp;<hr />
		<uc2:entitylist id="EntityList1" runat="server" />
	</div>
</asp:Content>
