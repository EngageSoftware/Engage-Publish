<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.ArticleSelector" Codebehind="ArticleSelector.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<asp:ScriptManagerProxy runat="server">
    <Services>
        <asp:ServiceReference path="../Services/PublishServices.asmx" />
    </Services>
</asp:ScriptManagerProxy>

<script type="text/javascript">
    function PopulateArticlesList() {
        var categoryId = parseInt($get('<%=CategoriesDropDownList.ClientID %>').value, 10);
        Engage.Dnn.Publish.Services.PublishServices.GetArticlesByCategory(categoryId, GetArticlesSuccessFunction);
    }

    function GetArticlesSuccessFunction(articlesList) {
        var articlesDropDown = $get('<%=ArticlesDropDownList.ClientID %>');

        articlesDropDown.options.length = 0;
        for (var i = 0; i < articlesList.length; ++i) {
            articlesDropDown.options[i] = new Option(articlesList[i].First, articlesList[i].Second);
        }
    }
</script>

<table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
    <tr valign="top">
        <td class="subhead"><dnn:Label ResourceKey="CategoriesLabel" Runat="server" ControlName="CategoriesDropDownList" /></td>
        <td>
            <asp:DropDownList ID="CategoriesDropDownList" Runat="server" CssClass="NormalTextBox" onchange="PopulateArticlesList()" />
        </td>
    </tr>
    <tr class="articleOptionDropDown">
        <td class="subhead"><dnn:label resourcekey="lblArticleList" runat="server" ControlName="ArticlesDropDownList" /></td>
	    <td><asp:dropdownlist id="ArticlesDropDownList" DataTextField="Name" DataValueField="ItemId" Runat="server" CssClass="NormalTextBox chooseArticleListDropDown" EnableViewState="false" /></td>
    </tr>
</table>
