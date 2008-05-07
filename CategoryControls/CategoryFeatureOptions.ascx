<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.CategoryControls.CategoryFeatureOptions" Codebehind="CategoryFeatureOptions.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<style type="text/css">
    @import url(<%=Engage.Dnn.Publish.ModuleBase.ApplicationUrl %><%=Engage.Dnn.Publish.ModuleBase.DesktopModuleFolderName %>Module.css);
</style>

<table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
<%--    <tr>
        <td class="SubHead"><dnn:label id="lblChooseItemType" Runat="server" ResourceKey="lblChooseItemType"></dnn:label></td>
        <td class="NormalTestBox"><asp:dropdownlist id="ddlItemTypeList" Runat="server"></asp:dropdownlist></td>
    </tr>
--%>    <tr>
        <td class="SubHead"><dnn:label id="lblChooseCategory" Runat="server" ResourceKey="lblChooseCategory"></dnn:label></td>
        <td class="NormalTestBox"><asp:dropdownlist id="ddlCategoryList" Runat="server"></asp:dropdownlist></td>
    </tr>
    <tr>
        <td class="SubHead"><dnn:label id="lblChooseDisplayType" Runat="server" ResourceKey="lblChooseDisplayType"></dnn:label></td>
        <td class="NormalTestBox"><asp:dropdownlist id="ddlViewOptions" Runat="server">
        <%--		<asp:ListItem Value="Title"></asp:ListItem>
		<asp:ListItem Value="Abstract"></asp:ListItem>
--%>	</asp:dropdownlist></td>
    </tr>
    <tr>
        <td class="SubHead"><dnn:label id="lblEnableRss" Runat="server" ResourceKey="lblEnableRss" Text="Enable Rss:"></dnn:label></td>
        <td class="NormalTestBox"><asp:CheckBox ID="chkEnableRss" runat="server" /></td>
    </tr>
    <tr>
        <td class="SubHead"><dnn:label id="lblRandomize" Runat="server" ResourceKey="lblRandomize" Text="Randomly Display an Article:"></dnn:label></td>
        <td class="NormalTestBox"><asp:CheckBox ID="chkRandomize" runat="server" /></td>
    </tr>
    
</table>