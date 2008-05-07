<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.CategoryControls.CategoryDisplayOptions" Codebehind="CategoryDisplayOptions.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<style type="text/css">
    @import url(<%=Engage.Dnn.Publish.ModuleBase.ApplicationUrl %><%=Engage.Dnn.Publish.ModuleBase.DesktopModuleFolderName %>Module.css);
</style>

<table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
    <tr>
	    <td class="SubHead"><dnn:label id="lblChooseItemType" Runat="server" ResourceKey="lblChooseItemType"/></td>
	    <td class="NormalTextBox"><asp:dropdownlist id="ddlItemTypeList" Runat="server"/></td>
    </tr>
    <tr>
        <td class="SubHead"><dnn:label id="lblChooseCategory" Runat="server" ResourceKey="lblChooseCategory"/></td>
        <td class="NormalTextBox"><asp:dropdownlist id="ddlCategoryList" Runat="server"/></td>
    </tr>
    <tr>
        <td class="SubHead"><dnn:label id="lblChooseDisplayType" Runat="server" ResourceKey="lblChooseDisplayType"/></td>
        <td class="NormalTextBox"><asp:dropdownlist id="ddlViewOptions" Runat="server"/></td>
    </tr>
    <tr>
        <td class="SubHead"><dnn:label id="lblChooseChildDisplay" Runat="server" ResourceKey="lblChooseChildDisplay"/></td>
        <td class="NormalTextBox"><asp:dropdownlist id="ddlChildDisplay" Runat="server"/></td>
    </tr>
    <tr>
        <td class="SubHead"><dnn:label id="lblSortOption" Runat="server" ResourceKey="lblSortOption"/></td>
        <td class="NormalTextBox"><asp:dropdownlist id="ddlSortOption" Runat="server"/></td>
    </tr>
</table>