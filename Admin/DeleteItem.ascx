<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Admin.DeleteItem" Codebehind="DeleteItem.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<table border="0" class="Normal">
    <tr valign="top">
        <td><dnn:Label id="lblDeleteItem" ResourceKey="lblDeleteItem" Runat="server" CssClass="Normal"></dnn:Label></td>
        <td><asp:TextBox ID="txtItemId" Runat="server" CssClass="input_textbox"></asp:TextBox></td>
    </tr>
</table>
<asp:LinkButton ID="cmdDelete" Runat="server" ResourceKey="btnDelete" CssClass="CommandButton"></asp:LinkButton><br />
<asp:Label ID="lblResults" runat="server" Visible="false" />
	

