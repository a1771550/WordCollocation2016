<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
	CodeBehind="AddEdit_Col.aspx.cs" Inherits="UI.Admin.WordCollocation.AddEdit_Col" %>

<%@ Register TagPrefix="uc1" TagName="backbutton" Src="~/Controls/BackButton.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<asp:ScriptManager runat="server" ID="sm1" />
	<div id="mainContent">
		<asp:Panel runat="server" ID="pnlAddEdit_Col" CssClass="clear EntryBlock" DefaultButton="btnCommand">
			<fieldset>
				<legend>
					<asp:Label runat="server" ID="lblLegend_Col"></asp:Label></legend>
				<div class="alert">
					<asp:ValidationSummary runat="server" ID="vsAddEdit_Col" ValidationGroup="vgAddEdit_Col" />
				</div>
				<asp:Panel runat="server" ID="pnlID_Col">
					<div class="labelField_Col">
						ID:
					</div>
					<div class="entryField">
						<asp:Label runat="server" ID="lblcolID"></asp:Label>
					</div>
				</asp:Panel>
				<div class="clear" />
				<div class="labelField_Col">
					POS:
				</div>
				<div class="entryField">
					<asp:DropDownList runat="server" ID="ddlPOS" AutoPostBack="True" OnSelectedIndexChanged="ddlPOS_SelectedIndexChanged">
					</asp:DropDownList>
					<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ValidationGroup="vgAddEdit_Col"
						ControlToValidate="ddlPOS" Display="None" EnableClientScript="True" ErrorMessage="POS is required!" InitialValue="---">*</asp:RequiredFieldValidator>
				</div>
				<div class="labelField_Col">
					Word:
				</div>
				<asp:UpdatePanel runat="server" ID="upLetter" UpdateMode="Conditional">
					<ContentTemplate>
						<div id="divLetter" runat="server" class="entryField">
							<asp:DropDownList runat="server" ID="ddlLetter" AutoPostBack="True" OnSelectedIndexChanged="ddlLetter_SelectedIndexChanged">
							</asp:DropDownList>
							<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="vgAddEdit_Col"
								ControlToValidate="ddlLetter" Display="None" EnableClientScript="True" ErrorMessage="Letter is required!" InitialValue="---">*</asp:RequiredFieldValidator>
						</div>
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="ddlPOS" EventName="SelectedIndexChanged" />
					</Triggers>
				</asp:UpdatePanel>
				<div class="entryField">
					<asp:UpdatePanel runat="server" ID="upWord" UpdateMode="Conditional">
						<ContentTemplate>
							<asp:Panel runat="server" ID="pnlWord" Visible="False" CssClass="displayInLine">
								<asp:DropDownList runat="server" ID="ddlWord">
								</asp:DropDownList>
								<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ValidationGroup="vgAddEdit_Col"
									ControlToValidate="ddlWord" Display="None" EnableClientScript="True" ErrorMessage="Word is required!">*</asp:RequiredFieldValidator>
							</asp:Panel>
						</ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="ddlLetter" EventName="SelectedIndexChanged" />
							<asp:AsyncPostBackTrigger ControlID="ddlPOS" EventName="SelectedIndexChanged" />
						</Triggers>
					</asp:UpdatePanel>
				</div>
				<div class="labelField_Col">
					Collocating POS:
				</div>
				<div class="entryField">
					<asp:DropDownList runat="server" ID="ddlColPOS" AutoPostBack="True" OnSelectedIndexChanged="ddlColPOS_SelectedIndexChanged">
					</asp:DropDownList>
					<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ValidationGroup="vgAddEdit_Col"
						ControlToValidate="ddlColPOS" Display="None" EnableClientScript="True" ErrorMessage="Collocated POS is required!" InitialValue="---">*</asp:RequiredFieldValidator>
				</div>
				<div class="labelField_Col">
					Collocating Word:
				</div>
				<asp:UpdatePanel runat="server" ID="upColLetter" UpdateMode="Conditional">
					<ContentTemplate>
						<div id="divColLetter" runat="server" class="entryField">
							<asp:DropDownList runat="server" ID="ddlColLetter" AutoPostBack="True" OnSelectedIndexChanged="ddlColLetter_SelectedIndexChanged">
							</asp:DropDownList>
							<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="vgAddEdit_Col"
								ControlToValidate="ddlColLetter" Display="None" EnableClientScript="True" ErrorMessage="Collocated Letter is required!" InitialValue="---">*</asp:RequiredFieldValidator>
						</div>
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="ddlColPOS" EventName="SelectedIndexChanged" />
					</Triggers>
				</asp:UpdatePanel>
				<div class="entryField">
					<asp:UpdatePanel runat="server" ID="upColWord" UpdateMode="Conditional">
						<ContentTemplate>
							<asp:Panel runat="server" ID="pnlColWord" Visible="False" CssClass="displayInLine">
								<asp:DropDownList runat="server" ID="ddlColWord">
								</asp:DropDownList>
								<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ValidationGroup="vgAddEdit_Col"
								                            ControlToValidate="ddlColWord" Display="None" EnableClientScript="True" ErrorMessage="Word is required!">*</asp:RequiredFieldValidator>
							</asp:Panel>
						</ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="ddlColLetter" EventName="SelectedIndexChanged" />
							<asp:AsyncPostBackTrigger ControlID="ddlColPOS" EventName="SelectedIndexChanged" />
						</Triggers>
					</asp:UpdatePanel>
				</div>
				<div class="labelField_Col">
					Collocation Pattern:
				</div>
				<div class="entryField">
					<asp:DropDownList runat="server" ID="ddlColPattern"/>
					<asp:RequiredFieldValidator runat="server" ID="refColPattern" ValidationGroup="vgAddEdit_Col" ControlToValidate="ddlColPattern" Display="None" EnableClientScript="True" ErrorMessage="Collocation Pattern is required!">*</asp:RequiredFieldValidator>
				</div>
				<asp:Panel runat="server" ID="ExampleBlock" Visible="False">
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
						<%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ValidationGroup="vgAddEdit_Col"
							ControlToValidate="txtExampleChi" Display="None" EnableClientScript="True" ErrorMessage="Example Chinese is required!">*</asp:RequiredFieldValidator>--%>
					</div>
					<div class="labelField_Ex">
						Example Japanese:
					</div>
					<div class="entryField">
						<asp:TextBox runat="server" ID="txtExampleJap" TextMode="MultiLine" Width="730px"
							Rows="5"></asp:TextBox>
						<%--<asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ValidationGroup="vgAddEdit_Col"
							ControlToValidate="txtExampleJap" Display="None" EnableClientScript="True" ErrorMessage="Example Japanese is required!">*</asp:RequiredFieldValidator>--%>
					</div>
					<div class="labelField">
						Source:</div>
					<div class="entryField">
						<asp:DropDownList runat="server" ID="ddlSources">
						</asp:DropDownList>
					</div>
					<div class="labelField">
						Remark: (Chinese|Japanese)</div>
					<div class="entryField">
						<asp:TextBox runat="server" ID="txtRemark" Width="300"></asp:TextBox>
					</div>
				</asp:Panel>
				<asp:Panel runat="server" ID="pnlEdit" Visible="False">
					<asp:HyperLink runat="server" Text="Add Example" ID="CreateExample"></asp:HyperLink>
					<br />
					<asp:GridView runat="server" ID="ExampleList" DataKeyNames="WcExampleId" AutoGenerateColumns="False"
						Width="95%">
						<EmptyDataTemplate>
							<p>
								No Data Available</p>
						</EmptyDataTemplate>
						<Columns>
							<asp:TemplateField HeaderText="Examples">
								<ItemTemplate>
									<div class="padd1em">
										<asp:Label ID="lblEntry" runat="server" Text='<%#Eval("Entry") %>'></asp:Label>
									</div>
									<div class="paddLeft1em paddBottom1em">
										&nbsp;
									</div>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Examples (Chi)">
								<ItemTemplate>
									<div class="padd1em">
										<asp:Label ID="lblEntryChi" runat="server" Text='<%#Eval("EntryChi")%>'></asp:Label>
									</div>
									<div class="paddLeft1em paddBottom1em">
										&nbsp;
									</div>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Examples (Jap)">
								<ItemTemplate>
									<div class="padd1em">
										<asp:Label ID="lblEntryJap" runat="server" Text='<%#Eval("EntryJap")%>'></asp:Label>
									</div>
									<div class="paddLeft1em paddBottom1em">
										&nbsp;
									</div>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:BoundField DataField="Source" HeaderText="Source" />
							<asp:BoundField DataField="Remark" HeaderText="Remark" />
							<asp:TemplateField>
								<ItemTemplate>
									<div id="commandField">
										<asp:HyperLink runat="server" ID="lnkEdit" Text="Edit" NavigateUrl='<%#string.Format("AddEdit_Example.aspx?mode=update&Id={0}&returnUrl=collocation.aspx",Eval("WcExampleId")) %>'></asp:HyperLink>
										&nbsp;&nbsp;
										<asp:LinkButton runat="server" ID="btnDelete" Text="Delete" OnClientClick="javascript:return confirm('Are you sure you want to delete this item?');"
											PostBackUrl='<%#string.Format("WcHandler.ashx?method=DeleteExample&Id={0}&returnUrl=collocation.aspx",Eval("WcExampleId")) %>'></asp:LinkButton>
									</div>
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>
					</asp:GridView>
				</asp:Panel>
				<div style="margin-top: 2em;">
					<asp:Button runat="server" ID="btnCommand" ValidationGroup="vgAddEdit_Col" OnClick="btnCommand_Click" />
				</div>
			</fieldset>
		</asp:Panel>
		<uc1:backbutton ID="BackButton1" runat="server" />
	</div>
</asp:Content>
