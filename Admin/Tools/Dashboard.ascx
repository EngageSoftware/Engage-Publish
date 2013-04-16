<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Admin.Tools.Dashboard" Codebehind="Dashboard.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelControl.ascx" %>
<div class="Publish_AdminTools_Dashboard Normal">
	<dnn:Label ID="lblControlTitle" ResourceKey="lblControlTitle" Runat="server" cssclass="Head"></dnn:Label>
	<hr />
    <asp:PlaceHolder ID="phAdminTools" runat="server"></asp:PlaceHolder>
</div>

