<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.AdminItemSearch" Codebehind="AdminItemSearch.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

    <table cellspacing="0" cellpadding="0" border="0" class="SettingsTable" >
        <tr valign="top">
            <td class="subhead"><dnn:Label ID="lblItemType" ResourceKey="lblItemType" Runat="server" ControlName="ddlCategories"></dnn:Label></td>
            <td><asp:DropDownList ID="ddlCategories" Runat="server" AutoPostBack="True" CssClass="Normal"></asp:DropDownList></td>
        </tr>
        <tr class="articleOptionDropDown">
	        <td class="subhead"><dnn:label ID="lblArticleList" resourcekey="lblArticleList" runat="server" /></td>
		    <td><asp:dropdownlist id="ddlArticleList"  DataTextField="Name" DataValueField="ItemId" Runat="server" CssClass="articleListDropDown NormalTextBox"></asp:dropdownlist></td>
	    </tr>
    </table>
