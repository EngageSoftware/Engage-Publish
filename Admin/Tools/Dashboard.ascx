<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Admin.Tools.Dashboard" Codebehind="Dashboard.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<div class="Normal">
	<dnn:Label ID="lblControlTitle" ResourceKey="lblControlTitle" Runat="server" cssclass="Head"></dnn:Label>
	<hr />
    <asp:PlaceHolder ID="phAdminTools" runat="server"></asp:PlaceHolder>
</div>

