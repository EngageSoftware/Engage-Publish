<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.CategoryControls.CategoryList" Codebehind="CategoryList.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<table border="0" class="Normal">
    <tr valign="top">
        <td><dnn:Label ID="Label1" ResourceKey="lblItemType" Runat="server" CssClass="Normal" ControlName="cboItemType"></dnn:Label></td>
        <td><asp:DropDownList ID="cboItemType" Runat="server" AutoPostBack="True" CssClass="Normal"></asp:DropDownList></td>
        <td><dnn:Label ID="lblWorkflow" ResourceKey="lblWorkFlow" Runat="server" CssClass="Normal" ControlName="cboWorkFlow"></dnn:Label></td>
        <td><asp:DropDownList ID="cboWorkflow" Runat="server" AutoPostBack="True" CssClass="Normal"></asp:DropDownList></td>
        <td><dnn:Label ID="lblArticleSearch" ResourceKey="lblArticleSearch" Runat="server" cssClass="Normal" ControlName="txtArticleSearch"></dnn:Label></td>   
        <td><asp:TextBox ID="txtArticleSearch" runat="server" CssClass="Normal"></asp:TextBox></td>
        <td><asp:LinkButton ID="btnFilter" runat="server" resourcekey="btnFilter" 
                onclick="btnFilter_Click"  CssClass="Normal" /></td>

    </tr>
</table>
<asp:label id="lblMessage" runat="server" CssClass="Subhead"></asp:label>
<br />
<div>
    <asp:placeholder id="phList" runat="server"></asp:placeholder>
</div>
<asp:hyperlink id="lnkAddNewCategory" Runat="server" ResourceKey="lnkAddNewCategory" CssClass="CommandButton"></asp:hyperlink>

