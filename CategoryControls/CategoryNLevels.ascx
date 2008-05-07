	<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.CategoryControls.CategoryNLevels" Codebehind="CategoryNLevels.ascx.cs" %>
	<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<div class="Normal">
<asp:Label ID="lblNoData" runat="server" Visible="false"></asp:Label>
<div class="divNLevelsListing">
	<asp:PlaceHolder ID="phNLevels" runat="server"></asp:PlaceHolder>
</div>
</div>

