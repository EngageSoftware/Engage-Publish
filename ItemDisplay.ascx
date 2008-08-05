<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.ItemDisplay" Codebehind="ItemDisplay.ascx.cs" %>
<div id="Publish_ItemDisplay" class="Publish_ItemDisplay" runat="server">

	<asp:UpdatePanel ID="upnlPublish" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:PlaceHolder id="phAdminControls" runat="Server" />
        <asp:PlaceHolder id="phControls" runat="Server" />
    </ContentTemplate>
    </asp:UpdatePanel>

</div>
