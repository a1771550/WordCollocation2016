<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Collocation.aspx.cs" Inherits="UI.Admin.WordCollocation.Collocation" %>
<%@ Register TagPrefix="uc1" TagName="searchbox" Src="~/Controls/SearchBox.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="mainContent">
		<asp:HyperLink runat="server" Text="Create" ID="lnkCreate"></asp:HyperLink>
		<hr />
		<uc1:searchbox id="SearchBox1" runat="server" />
		<hr />
		<asp:HiddenField runat="server" ID="SortExpression" />
		<asp:HiddenField runat="server" ID="SortDirection" />
		<asp:GridView runat="server" ID="gvList" ClientIDMode="Static" AutoGenerateColumns="False"
		              AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvList_PageIndexChanging" OnSorting="gvList_Sorting" DataKeyNames="CollocationId" Width="50%">
			<EmptyDataTemplate>
				<p>
					No Data Available</p>
			</EmptyDataTemplate>
			<Columns>
				<asp:TemplateField HeaderText="Word" SortExpression="Word">
					<ItemTemplate>
						<asp:HiddenField runat="server" ID="HiColId" Value='<%#Eval("CollocationId") %>' />
						<div class="padd1em">
							<asp:Label runat="server" ID="lblWord" CssClass="lowercase" Text='<%#Eval("Entry_Word") %>'></asp:Label>&nbsp;<asp:Label
								runat="server" ID="lblPOS" CssClass="font10px">(<%#Eval("Entry_POS") %>)</asp:Label>
						</div>
						<div class="paddLeft1em paddBottom1em">
							<asp:Label runat="server" ID="lblWordChi"><%#Eval("Entry_WordChi") %></asp:Label>
						</div>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Collocating Word" SortExpression="ColWord">
					<ItemTemplate>
						<div class="padd1em">
							<asp:Label runat="server" ID="lblColWord" CssClass="lowercase"><%#Eval("Entry_ColWord") %></asp:Label>&nbsp;<asp:Label
								runat="server" ID="lblColPOS" CssClass="font10px">(<%#Eval("Entry_ColPOS") %>)</asp:Label>
						</div>
						<div class="paddLeft1em paddBottom1em">
							<asp:Label runat="server" ID="lblColWordChi"><%#Eval("Entry_ColWordChi") %></asp:Label>
						</div>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Collocation Pattern" SortExpression="ColPattern">
					<ItemTemplate>
						<div class="padd1em">
							<asp:Label ID="lblCollocationPattern" runat="server" CssClass="lowercase"><%#Eval("CollocationPattern") %></asp:Label>
						</div>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField>
					<ItemStyle Width="8%"></ItemStyle>
					<ItemTemplate>
						<div id="commandField">
							<asp:HyperLink runat="server" ID="lnkEdit" Text="Edit" NavigateUrl='<%# string.Format("AddEdit_Col.aspx?entityID={0}&mode=update",Eval("CollocationId")) %>'></asp:HyperLink>
							&nbsp;&nbsp;
							<asp:LinkButton runat="server" ID="btnDelete" Text="Delete" OnClientClick="javascript:return confirm('Are you sure you want to delete this item?');" PostBackUrl='<%# string.Format("WcHandler.ashx?method=DeleteCollocation&Id={0}",Eval("CollocationId")) %>'></asp:LinkButton>
						</div>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
	</div>
</asp:Content>
