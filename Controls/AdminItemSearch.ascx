<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.AdminItemSearch" Codebehind="AdminItemSearch.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

    <table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
        <tr valign="top">
            <td><dnn:Label ID="lblItemType" ResourceKey="lblItemType" Runat="server" CssClass="Normal" ControlName="cboCategories"></dnn:Label></td>
            <td><asp:DropDownList ID="cboCategories" Runat="server" AutoPostBack="True" CssClass="Normal"></asp:DropDownList></td>
        </tr>
        <tr>
            <td><dnn:Label ID="lblItemSearch" ResourceKey="lblItemSearch" Runat="server" cssClass="Normal" ControlName="txtItemSearch"></dnn:Label></td>
            <td><asp:TextBox ID="txtItemSearch" runat="server" CssClass="Normal"></asp:TextBox></td>
        </tr>
        <tr>
            <td></td>
            <td><asp:LinkButton ID="btnFilter" runat="server" resourcekey="btnFilter" 
                    onclick="btnFilter_Click"  CssClass="Normal" /></td>
        </tr>
        <tr class="articleOptionDropDown">
	        <td class="subhead"><dnn:label ID="lblArticleList" resourcekey="lblArticleList" runat="server" /></td>
		    <td><asp:dropdownlist id="ddlArticleList" AutoPostBack="true" DataTextField="Name" DataValueField="ItemId" Runat="server" CssClass="articleListDropDown NormalTextBox"></asp:dropdownlist></td>
	    </tr>
	    
	    <tr>
	        <td><asp:label id="lblSelectedItem" runat="server" /></td>
	        <td><asp:TextBox ID="txtSelectedId" runat="server" Visible="false" /></td>
	    </tr>
    </table>
