<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.ItemListingOptions" Codebehind="ItemListingOptions.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<style type="text/css">
    @import url(<%=Engage.Dnn.Publish.ModuleBase.ApplicationUrl %><%=Engage.Dnn.Publish.ModuleBase.DesktopModuleFolderName %>Module.css);
</style>

<table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
    <tr>
        <td class="SubHead"><dnn:label id="plItemType" runat="server" controlname="ddlItemTypeList" text="Select an item type to Display:" ResourceKey="plItemType"></dnn:label></td>
		<td class="NormalTextBox"><asp:dropdownlist id="ddlItemTypeList" Runat="server"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="plDataType" runat="server" ResourceKey="plDataType" controlname="ddlDataType" text="Select the type of data to Display:"></dnn:label></td>
		<td class="NormalTextBox"><asp:dropdownlist id="ddlDataType" Runat="server" OnSelectedIndexChanged="ddlDataType_SelectedIndexChanged" AutoPostBack="true">
<%--		    <asp:ListItem Value="Item Listing">Item Listing</asp:ListItem>
				<asp:ListItem Value="Most Popular">Most Popular</asp:ListItem>
				<asp:ListItem Value="Most Recent">Most Recent</asp:ListItem>
--%>		</asp:dropdownlist>
		</td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="plCategory" runat="server" controlname="ddlCategory" ResourceKey="ddlCategory" text="Select the Category (only for Most Recent):"></dnn:label></td>
		<td class="NormalTextBox"><asp:dropdownlist id="ddlCategory" Runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true"></asp:dropdownlist></td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="plFormatType" runat="server" ResourceKey="plFormatType" controlname="ddlDisplayFormat" text="Select the format of the data to Display:"></dnn:label></td>
		<td class="NormalTextBox"><asp:dropdownlist id="ddlDisplayFormat" Runat="server">
<%--			<asp:ListItem Value="List">List</asp:ListItem>
				<asp:ListItem Value="Abstract">Abstract</asp:ListItem>
--%>		</asp:dropdownlist>
		</td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="lblShowParent" runat="server" controlname="chkShowParent" ResourceKey="lblShowParent" text="Show Parent Category:"></dnn:label></td>
		<td class="NormalTextBox"><asp:CheckBox ID="chkShowParent" Runat="server"></asp:CheckBox></td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="plMaxItems" runat="server" ResourceKey="plMaxItems" controlname="txtMaxItems" text="Max Items to Display (-1 to show all):"></dnn:label></td>
		<td class="NormalTextBox"><asp:textbox id="txtMaxItems" runat="server"></asp:textbox>&nbsp;
            <asp:CustomValidator ID="fvMaxItems" runat="server" ErrorMessage="Value must be an Integer" resourcekey="fvMaxItems"
            ControlToValidate="txtMaxItems" OnServerValidate="fvMaxItems_ServerValidate" ValidateEmptyText="True"></asp:CustomValidator></td>
	</tr>
<%--	<tr>
		<td class="SubHead"><dnn:label id="plMaxSubItems" runat="server" controlname="txtMaxSubItems" text="Max Sub Items To Display:" Visible="False"></dnn:label></td>
		<td class="NormalTextBox"><asp:textbox id="txtMaxSubItems" runat="server" Visible="False"></asp:textbox>
			<asp:comparevalidator id="CompareValidator2" runat="server" ControlToValidate="txtMaxSubItems" Operator="DataTypeCheck"	Type="Integer" ErrorMessage="Value must be an Integer" Visible="False" Display="Dynamic"></asp:comparevalidator>
		</td>
	</tr>
--%>	<tr>
		<td class="SubHead"><dnn:label id="lblEnableRss" runat="server" controlname="chkEnableRss" ResourceKey="lblEnableRss" text="Enable RSS:"></dnn:label></td>
		<td class="NormalTextBox"><asp:CheckBox ID="chkEnableRss" Runat="server"></asp:CheckBox></td>
	</tr>
</table>