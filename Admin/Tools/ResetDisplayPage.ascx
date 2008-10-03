<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Admin.Tools.ResetDisplayPage" Codebehind="ResetDisplayPage.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<div class="Normal">
	<dnn:Label Runat="server" ResourceKey="lblControlTitle" CssClass="Head"/>
	<hr />
	<asp:Label runat="server" resourcekey="lblMessage"/>
</div>

