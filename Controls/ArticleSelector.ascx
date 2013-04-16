<%@ Control Language="c#" AutoEventWireup="True" Inherits="Engage.Dnn.Publish.Controls.ArticleSelector" Codebehind="ArticleSelector.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelControl.ascx" %>
<asp:ScriptManagerProxy runat="server">
    <Services>
        <asp:ServiceReference path="~/DesktopModules/EngagePublish/Services/PublishServices.asmx" />
    </Services>
</asp:ScriptManagerProxy>

<table cellspacing="0" cellpadding="0" border="0" class="SettingsTable articleOptionDropDown">
    <tr valign="top">
        <td class="SubHead"><dnn:Label ResourceKey="CategoriesLabel" runat="server" ControlName="CategoriesDropDownList" /></td>
        <td><asp:DropDownList ID="CategoriesDropDownList" runat="server" CssClass="NormalTextBox" onchange="Engage_Publish_ArticleSelector_PopulateArticlesList()" /></td>
    </tr>
    <tr>
        <td class="SubHead"><dnn:Label resourcekey="lblArticleList" runat="server" ControlName="ArticlesDropDownList" /></td>
	    <td>
	        <%-- Use select instead of asp:DropDownList to opt-out of event validation, since this is dynamically filled --%>
	        <select id="ArticlesDropDownList" runat="server" class="NormalTextBox chooseArticleListDropDown" />
	    </td>
    </tr>
</table>