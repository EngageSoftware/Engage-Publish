<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.CategoryControls.CategoryFeatureOptions" Codebehind="CategoryFeatureOptions.ascx.cs" %>
<%@ Import Namespace="Engage.Dnn.Publish" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelControl.ascx" %>

<style type="text/css">
    @import url(<%=ModuleBase.ApplicationUrl%><%=ModuleBase.DesktopModuleFolderName%>Module.css);
</style>

<table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
<%--    <tr>
        <td class="SubHead"><dnn:Label id="lblChooseItemType" Runat="server" ResourceKey="lblChooseItemType"></dnn:label></td>
        <td class="NormalTestBox"><asp:dropdownlist id="ddlItemTypeList" Runat="server"></asp:dropdownlist></td>
    </tr>
--%>    <tr>
        <td class="SubHead"><dnn:Label id="lblChooseCategory" Runat="server" ResourceKey="lblChooseCategory"></dnn:label></td>
        <td class="NormalTestBox"><asp:dropdownlist id="ddlCategoryList" Runat="server"></asp:dropdownlist></td>
    </tr>
    <tr>
        <td class="SubHead"><dnn:Label id="lblChooseDisplayType" Runat="server" ResourceKey="lblChooseDisplayType"></dnn:label></td>
        <td class="NormalTestBox"><asp:dropdownlist id="ddlViewOptions" Runat="server">
        <%--		<asp:ListItem Value="Title"></asp:ListItem>
		<asp:ListItem Value="Abstract"></asp:ListItem>
--%>	</asp:dropdownlist></td>
    </tr>
    <tr>
        <td class="SubHead"><dnn:Label id="lblEnableRss" Runat="server" ResourceKey="lblEnableRss" Text="Enable Rss:"></dnn:label></td>
        <td class="NormalTestBox"><asp:CheckBox ID="chkEnableRss" runat="server" /></td>
    </tr>
    <tr>
        <td class="SubHead"><dnn:Label id="lblRandomize" Runat="server" ResourceKey="lblRandomize" Text="Randomly Display an Article:"></dnn:label></td>
        <td class="NormalTestBox"><asp:CheckBox ID="chkRandomize" runat="server" /></td>
    </tr>
    
</table>