<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
	CodeBehind="AddEdit.aspx.cs" Inherits="UI.Admin.WordCollocation.AddEdit" %>

<%@ Register Src="~/Controls/ErrorLabel.ascx" TagPrefix="uc2" TagName="ErrorLabel" %>

<%@ Register Src="~/Controls/BackButton.ascx" TagName="BackButton" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script>
		$(function ()
		{

			$('input#txtEntry').keyup(function (e)
			{
				$('#lblError').text = '';
				var s = $(this).val();
				checkIfDuplicatedEntry(s);
			});
		});

		function checkIfDuplicatedEntry(entry)
		{
			$.ajax({
				url: 'WcHandler.ashx',
				type: 'GET',
				dataType: 'json',
				contentType: "application/json; charset=utf-8",
				data: { method: 'CheckIfDuplicatedEntry', entry: entry },
				success: function (data)
				{
					$('#lblError').show();
					if (data) {
						$('#lblError').text('This entry: ' + entry + ' is already in the database!');
						$('input#txtEntry').focus();
						$('#btnCommand').attr("disabled", true).attr("style", "cursor:default");
					} else {
						$('#lblError').hide();
						$('#btnCommand').attr("disabled", false).attr("style", "cursor:hand");
					}
					
				},
				error: function (data)
				{
					$('#lblError').show();
					//var result = JSON.stringify(data.responseText);
					$('#lblError').text('Error: ' + data);
				}
			});
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div id="mainContent">
		<asp:Panel runat="server" ID="pnlAddEdit" CssClass="clear EntryBlock" DefaultButton="btnCommand">
			<fieldset>
				<legend>
					<asp:Label runat="server" ID="lblLegend"></asp:Label></legend>

				<div class="alert">
					<asp:ValidationSummary runat="server" ID="vsAddEdit" ValidationGroup="vgAddEdit" />
				</div>
				<asp:Panel runat="server" ID="pnlID">
					<div class="labelField">
						ID:
					</div>
					<div class="entryField">
						<asp:Label runat="server" ID="lblID"></asp:Label>
					</div>
				</asp:Panel>
				<asp:Panel runat="server" ID="pnlEntry" CssClass="EntryBlock">
					<div class="labelField">
						Entry:
					</div>
					<div class="entryField">
						<asp:TextBox runat="server" ID="txtEntry" Width="600"></asp:TextBox>
						<asp:RequiredFieldValidator runat="server" ID="rfEntry" ValidationGroup="vgAddEdit"
							ControlToValidate="txtEntry" Display="None" EnableClientScript="True" ErrorMessage="Entry is required!">*</asp:RequiredFieldValidator>
					</div>
					<div class="labelField">
						Entry Chinese:
					</div>
					<div class="entryField">
						<asp:TextBox runat="server" ID="txtEntryChi" TextMode="MultiLine" Rows="4"></asp:TextBox>
						<asp:RequiredFieldValidator runat="server" ID="rfEntryChi" ValidationGroup="vgAddEdit"
							ControlToValidate="txtEntryChi" Display="None" EnableClientScript="True" ErrorMessage="Entry Chinese is required!">*</asp:RequiredFieldValidator>
					</div>
					<div class="labelField">
						Entry Japanese:
					</div>
					<div class="entryField">
						<asp:TextBox runat="server" ID="txtEntryJap" TextMode="MultiLine" Rows="4"></asp:TextBox>
						<asp:RequiredFieldValidator runat="server" ID="rfEntryJap" ValidationGroup="vgAddEdit"
							ControlToValidate="txtEntryJap" Display="None" EnableClientScript="True" ErrorMessage="Entry Japanese is required!">*</asp:RequiredFieldValidator>
					</div>
				</asp:Panel>

				<asp:Panel runat="server" ID="pnlEntry_Ex" Visible="False">
					<div class="labelField">
						Entry:
					</div>
					<div class="entryField">
						<asp:TextBox runat="server" ID="txtEntry_Ex" TextMode="MultiLine"
							Rows="5" Width="720px"></asp:TextBox>
						<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="vgAddEdit"
							ControlToValidate="txtEntry_Ex" Display="None" EnableClientScript="True" ErrorMessage="Entry is required!">*</asp:RequiredFieldValidator>
					</div>
					<div class="labelField">
						Entry Chinese:
					</div>
					<div class="entryField">
						<asp:TextBox runat="server" ID="txtEntryChi_Ex" TextMode="MultiLine"
							Rows="5" Width="720px"></asp:TextBox>
						<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="vgAddEdit"
							ControlToValidate="txtEntryChi_Ex" Display="None" EnableClientScript="True" ErrorMessage="Entry Chinese is required!">*</asp:RequiredFieldValidator>
					</div>
				</asp:Panel>
				<asp:Button runat="server" ID="btnCommand" ValidationGroup="vgAddEdit" ClientIDMode="Static"
					OnClick="btnCommand_Click" />
			</fieldset>
		</asp:Panel>
		<uc1:BackButton ID="BackButton1" runat="server" />
		<uc2:ErrorLabel ID="ErrorLabel1" runat="server" />
	</div>


</asp:Content>
