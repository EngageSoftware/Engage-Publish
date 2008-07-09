<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Admin.Tools.DescriptionReplace" Codebehind="DescriptionReplace.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<div class="Normal">
	<dnn:Label ID="lblControlTitle" ResourceKey="lblControlTitle" Runat="server" cssclass="Head"></dnn:Label>
	<hr />
	<asp:Label ID="lblMessage" runat="server" resourcekey="lblMessage"></asp:Label><br />
	<asp:LinkButton ID="lbReplace" runat="server" resourcekey="lbReplace" OnClick="lbReplace_Click"></asp:LinkButton>
	<hr />
	<asp:Label ID="lblOutput" runat="server"></asp:Label>
</div>

