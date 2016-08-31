<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="UI.Admin.WordCollocation.Search" %>
<%@ Register src="~/Admin/BackButton.ascx" tagname="BackButton" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="mainContent">
		<asp:Panel runat="server" ID="pnlSearch">
		<table style="border: solid 1px #000000" cellspacing="4" cellpadding="4">
			<tr valign="top">
				<th>
					Entry
				</th>
				<th>
					Entry Chi
				</th>
				<th>
				</th>
			</tr>
			<tr valign="top">
				<td>
					<asp:Label runat="server" ID="lblEntry"></asp:Label>
				</td>
				<td>
					<asp:Label runat="server" ID="lblEntryChi"></asp:Label>
				</td>
				<td>
					<asp:HyperLink runat="server" ID="lnkEdit" Text="Edit"></asp:HyperLink>
					&nbsp;&nbsp;
					<asp:LinkButton runat="server" ID="btnDelete" Text="Delete" OnClientClick="javascript:return confirm('Are you sure you want to delete this item?');"></asp:LinkButton>
				</td>
			</tr>
		</table>
	</asp:Panel>
	
	<asp:Label runat="server" ID="lblNoResult" Text="No search result found."></asp:Label>
	

	<uc1:BackButton ID="BackButton1" runat="server" />
	</div>
	
	

</asp:Content>
