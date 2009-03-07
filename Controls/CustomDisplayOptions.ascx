<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.CustomDisplayOptions" Codebehind="CustomDisplayOptions.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<style type="text/css">
    @import url(<%=Engage.Dnn.Publish.ModuleBase.ApplicationUrl %><%=Engage.Dnn.Publish.ModuleBase.DesktopModuleFolderName %>Module.css);
</style>

<table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
    <tr>
        <td class="SubHead"><dnn:label id="plItemType" runat="server" controlname="ddlItemTypeList" text="Select an item type to Display:" ResourceKey="plItemType"/></td>
		<td class="NormalTextBox" style="width: 252px" colspan="3"><asp:dropdownlist id="ddlItemTypeList" Runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlItemTypeList_SelectedIndexChanged"/></td>
	</tr>
	<tr class="categoryOptionDropDown">
		<td class="SubHead"><dnn:label id="plCategory" runat="server" controlname="ddlCategory" ResourceKey="plCategory" text="Select the Category (only for Most Recent):"/></td>
		<td class="NormalTextBox" style="width: 252px" colspan="3"><asp:dropdownlist id="ddlCategory" Runat="server" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="true"/></td>
	</tr>
	<tr>
	    <td class="Head" colspan="2">
            <asp:Label ID="lblDisplaySetting" runat="server" resourcekey="lblDisplaySetting"></asp:Label>
	    </td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="plFormatType" runat="server" ResourceKey="plFormatType" controlname="ddlDisplayFormat" text="Select the format of the data to Display:"></dnn:label></td>
		<td class="NormalTextBox" style="width: 252px" colspan="3"><asp:CheckBoxList id="chkDisplayOptions" Runat="server" CssClass="Normal"/></td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="lblDateFormat" runat="server" controlname="txtDateFormat" ResourceKey="lblDateFormat" text="Date Format:"/></td>
		<td class="NormalTextBox" style="width: 252px"><asp:TextBox ID="txtDateFormat" Runat="server" /></td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="lblShowParent" runat="server" controlname="chkShowParent" ResourceKey="lblShowParent" text="Show Parent Category:"/></td>
		<td class="NormalTextBox" style="width: 252px"><asp:CheckBox ID="chkShowParent" 
                Runat="server" /></td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="lblShowParentDescription" runat="server" controlname="chkShowParentDescription" ResourceKey="lblShowParentDescription" text="Show Parent Category Description:"/></td>
		<td class="NormalTextBox" style="width: 252px"><asp:CheckBox ID="chkShowParentDescription" Runat="server"/></td>
	</tr>

	<tr>
		<td class="SubHead"><dnn:label id="lblLoadRelated" runat="server" controlname="chkRelatedItem" ResourceKey="lblLoadRelated" text="Dynamic Category:"/></td>
		<td class="NormalTextBox" style="width: 252px"><asp:CheckBox ID="chkRelatedItem" Runat="server"/></td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="lblLoadRelatedLevel" runat="server" controlname="chkRelatedItemLevel" ResourceKey="lblLoadRelatedLevel" text="Dynamic Category:"/></td>
		<td class="NormalTextBox" style="width: 252px"><asp:CheckBox ID="chkRelatedItemLevel" Runat="server"/></td>
	</tr>

	<tr>
		<td class="SubHead"><dnn:label id="lblAllowPaging" runat="server" controlname="chkAllowPaging" text="Allow Paging:"/></td>
		<td class="NormalTextBox" style="width: 252px"><asp:CheckBox ID="chkAllowPaging" Runat="server"/></td>
	</tr>
    <tr>
		<td class="SubHead"><dnn:label id="plMaxItems" runat="server" ResourceKey="plMaxItems" controlname="txtMaxItems" text="Max Items to Display (-1 to show all):"/></td>
		<td class="NormalTextBox" colspan="3">
		    <asp:textbox id="txtMaxItems" runat="server" Width="35px"></asp:textbox>&nbsp;<asp:Label id="lblEnterNumber" runat="server" Text="(enter number)"/> &nbsp;
		    <asp:CheckBox ID="chkShowAll" runat="server" Text ="Show All" OnCheckedChanged="chkShowAll_CheckedChanged" AutoPostBack="True"  />
		    <asp:CustomValidator ID="fvMaxItems" runat="server" ErrorMessage="Value must be an Integer" ControlToValidate="txtMaxItems" OnServerValidate="fvMaxItems_ServerValidate" ValidateEmptyText="True"/>
		</td>
	</tr>

	<tr>
	    <td class="Head" colspan="2">
            <asp:Label ID="lblSortHeading" runat="server" resourcekey="lblSortHeading"></asp:Label>
	    </td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="lblCustomSort" runat="server" controlname="chkUseCustomSort" text="Use Custom Sort:"/></td>
		<td class="NormalTextBox" style="width: 252px"><asp:CheckBox ID="chkUseCustomSort" Runat="server"/> <asp:HyperLink ID="lnkSortCategory" resourcekey="lnkSortCategory" runat="server"></asp:HyperLink></td>
	</tr>

    <tr>
        <td class="SubHead">
            <dnn:label id="lblSortOption" Runat="server" ResourceKey="lblSortOption"/>
        </td>
        <td class="NormalTextBox" style="width: 252px">
            <asp:dropdownlist id="ddlSortOption" Runat="server"/>
        </td>
        <td>
            <asp:RadioButtonList ID="rbSortDirection" runat="server" RepeatDirection="Horizontal" TextAlign="Right" CssClass="Normal">
                <asp:ListItem Text="Ascending" Value="0"/>
                <asp:ListItem Text="Descending" Value="1"/>
            </asp:RadioButtonList>
        </td>
    </tr>
	<tr>
	    <td class="Head" colspan="2">
            <asp:Label ID="lblOtherSettings" runat="server" resourcekey="lblOtherSettings"></asp:Label>
	    </td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="lblEnableRss" runat="server" controlname="chkEnableRss" ResourceKey="lblEnableRss" text="Enable RSS:"></dnn:label></td>
		<td class="NormalTextBox"><asp:CheckBox ID="chkEnableRss" Runat="server"></asp:CheckBox></td>
	</tr>
</table>