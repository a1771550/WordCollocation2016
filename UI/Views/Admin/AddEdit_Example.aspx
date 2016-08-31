<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddEdit_Example.aspx.cs" Inherits="UI.Admin.WordCollocation.AddEdit_Example" %>
<%@ Register TagPrefix="uc1" TagName="backbutton" Src="~/Controls/BackButton.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="mainContent">
		
		<asp:Panel runat="server" ID="pnlWord" Visible="False">
			<h4>Word: <asp:Label runat="server" ID="lblWord"></asp:Label>&nbsp;(<asp:Label runat="server" ID="lblPOS"></asp:Label>)</h4>
		<h4>Collocated Word: <asp:Label runat="server" ID="lblColWord"></asp:Label>&nbsp;(<asp:Label runat="server" ID="lblColPOS"></asp:Label>)</h4>
		</asp:Panel>
		
		<div class="marginTop1em">&nbsp;</div>
		<asp:Panel runat="server" ID="WcExampleBox">
			<div class="labelField_Ex">
				Example:
			</div>
			<div class="entryField">
				<asp:TextBox runat="server" ID="txtExample" TextMode="MultiLine" Width="730px" Rows="5"></asp:TextBox>
				<asp:RequiredFieldValidator runat="server" ID="rfExample" ValidationGroup="vgAddEdit_Col"
				                            ControlToValidate="txtExample" Display="None" EnableClientScript="True" ErrorMessage="Example is required!">*</asp:RequiredFieldValidator>
			</div>
			<div class="labelField_Ex">
				Example Chinese:
			</div>
			<div class="entryField">
				<asp:TextBox runat="server" ID="txtExampleChi" TextMode="MultiLine" Width="730px"
				             Rows="5"></asp:TextBox>
				<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ValidationGroup="vgAddEdit_Col"
				                            ControlToValidate="txtExampleChi" Display="None" EnableClientScript="True" ErrorMessage="Example Chinese is required!">*</asp:RequiredFieldValidator>
			</div>
			<div class="labelField_Ex">
				Example Japanese:
			</div>
			<div class="entryField">
				<asp:TextBox runat="server" ID="txtExampleJap" TextMode="MultiLine" Width="730px"
					Rows="5"></asp:TextBox>
				<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ValidationGroup="vgAddEdit_Col"
					ControlToValidate="txtExampleJap" Display="None" EnableClientScript="True" ErrorMessage="Example Japanese is required!">*</asp:RequiredFieldValidator>
			</div>
			<div class="labelField_Ex">Source:</div>
			<div class="entryField">
				<asp:DropDownList runat="server" ID="ddlSources"></asp:DropDownList>
			</div>
			<div class="labelField_Ex">
				Remark: (Chinese|Japanese)
			</div>
			<div class="entryField">
				<asp:TextBox runat="server" ID="txtRemark"></asp:TextBox>
			</div>
			<asp:Button runat="server" ID="btnCommand" ValidationGroup="vgAddEdit_Col"
			            OnClick="btnCommand_Click" />
			<div class="labelField_Col"></div>
		</asp:Panel>
		<uc1:backbutton id="BackButton1" runat="server" />
	</div>
</asp:Content>
