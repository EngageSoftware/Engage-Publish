<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.CategoryControls.CategoryDisplayOptions" Codebehind="CategoryDisplayOptions.ascx.cs" %>
<%@ Import Namespace="Engage.Dnn.Publish" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelControl.ascx" %>

<style type="text/css">
    @import url(<%=ModuleBase.ApplicationUrl%><%=ModuleBase.DesktopModuleFolderName%>Module.css);
</style>

<table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
    <tr>
	    <td class="SubHead"><dnn:Label id="lblChooseItemType" Runat="server" ResourceKey="lblChooseItemType"/></td>
	    <td class="NormalTextBox"><asp:dropdownlist id="ddlItemTypeList" Runat="server"/></td>
    </tr>
    <tr>
        <td class="SubHead"><dnn:Label id="lblChooseCategory" Runat="server" ResourceKey="lblChooseCategory"/></td>
        <td class="NormalTextBox"><asp:dropdownlist id="ddlCategoryList" Runat="server"/></td>
    </tr>
    <tr>
        <td class="SubHead"><dnn:Label id="lblChooseDisplayType" Runat="server" ResourceKey="lblChooseDisplayType"/></td>
        <td class="NormalTextBox"><asp:dropdownlist id="ddlViewOptions" Runat="server"/></td>
    </tr>
    <tr>
        <td class="SubHead"><dnn:Label id="lblChooseChildDisplay" Runat="server" ResourceKey="lblChooseChildDisplay"/></td>
        <td class="NormalTextBox"><asp:dropdownlist id="ddlChildDisplay" Runat="server"/></td>
    </tr>
    <tr>
        <td class="SubHead"><dnn:Label id="lblSortOption" Runat="server" ResourceKey="lblSortOption"/></td>
        <td class="NormalTextBox"><asp:dropdownlist id="ddlSortOption" Runat="server"/></td>
    </tr>
</table>