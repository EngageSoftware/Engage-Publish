<%@ Control Language="c#" AutoEventWireup="True" Inherits="Engage.Dnn.Publish.Controls.ArticleSelector" Codebehind="ArticleSelector.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<asp:ScriptManagerProxy runat="server">
    <Services>
        <asp:ServiceReference path="~/DesktopModules/EngagePublish/Services/PublishServices.asmx" />
    </Services>
</asp:ScriptManagerProxy>

<table cellspacing="0" cellpadding="0" border="0" class="SettingsTable articleOptionDropDown">
    <tr valign="top">
        <td class="subhead"><dnn:Label ResourceKey="CategoriesLabel" Runat="server" ControlName="CategoriesDropDownList" /></td>
        <td>
            <asp:DropDownList ID="CategoriesDropDownList" Runat="server" CssClass="NormalTextBox" onchange="PopulateArticlesList()" />
        </td>
    </tr>
    <tr>
        <td class="subhead"><dnn:label resourcekey="lblArticleList" runat="server" ControlName="ArticlesDropDownList" /></td>
	    <td><asp:dropdownlist id="ArticlesDropDownList" DataTextField="Name" DataValueField="ItemId" Runat="server" CssClass="NormalTextBox chooseArticleListDropDown" EnableViewState="false" /></td>
    </tr>
</table>
