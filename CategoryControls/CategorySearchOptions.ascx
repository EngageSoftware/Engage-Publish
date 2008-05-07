<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.CategoryControls.CategorySearchOptions" Codebehind="CategorySearchOptions.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>

<style type="text/css">
    @import url(<%=Engage.Dnn.Publish.ModuleBase.ApplicationUrl %><%=Engage.Dnn.Publish.ModuleBase.DesktopModuleFolderName %>Module.css);
</style>

<table cellspacing="0" cellpadding="2" summary="Edit Search Design Table" border="0" class="SettingsTable">
	<tr>
		<td class="SubHead"><dnn:label id="lblAllowCategorySelection" runat="server" controlname="chkAllowCategorySelection"
				text="Allow Category Selection:"></dnn:label></td>
		<td class="NormalTextBox"><asp:checkbox id="chkAllowCategorySelection" runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="plResults" runat="server" controlname="txtresults" text="Maximum Search Results:"></dnn:label></td>
		<td class="NormalTextBox"><asp:textbox id="txtResults" runat="server" MaxLength="5" CssClass="NormalTextBox">200</asp:textbox></td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="plPage" runat="server" controlname="txtPage" text="Results per Page:"></dnn:label></td>
		<td class="NormalTextBox"><asp:textbox id="txtPage" runat="server" MaxLength="5" CssClass="NormalTextBox">15</asp:textbox></td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="plTitle" runat="server" controlname="txtTitle" text="Maximum Title Length:"></dnn:label></td>
		<td class="NormalTextBox"><asp:textbox id="txtTitle" runat="server" MaxLength="5" CssClass="NormalTextBox">65</asp:textbox></td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="plDescription" runat="server" controlname="txtdescription" text="Maximum Description Length:"></dnn:label></td>
		<td class="NormalTextBox"><asp:textbox id="txtDescription" runat="server" MaxLength="5" CssClass="NormalTextBox">200</asp:textbox></td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="lblChooseCategorySearch" runat="server" controlname="ddlCategorySearchList" text="Choose a Category to Search Under:"></dnn:label></td>
		<td><asp:dropdownlist id="ddlCategorySearchList" Runat="server"></asp:dropdownlist><br/>
		</td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="plShowDescription" runat="server" controlname="chkDescription" text="Show Description?"></dnn:label></td>
		<td class="NormalTextBox"><asp:checkbox id="chkDescription" runat="server" CssClass="NormalTextBox"></asp:checkbox></td>
	</tr>
	<tr>
		<td class="SubHead"><dnn:label id="lblSearchUrl" runat="server" controlname="txtSearchUrl" text="Redirect URL if Search String Empty:"></dnn:label></td>
		<td class="NormalTextBox"><asp:textbox id="txtSearchUrl" runat="server" CssClass="NormalTextBox"></asp:textbox></td>
	</tr>
</table>