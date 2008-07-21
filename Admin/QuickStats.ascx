<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Admin.QuickStats" Codebehind="QuickStats.ascx.cs" %>
<div class="EngagePublishQuickStats">

<div id="PublishStatsLabel"><asp:Label ID="lblStats" runat="server" resourcekey="lblStats"></asp:Label></div>

    <asp:HyperLink ID="lnkWaitingForApproval" runat="server" Visible="false"></asp:HyperLink>
    
    <asp:HyperLink ID="lnkCommentsForApproval" runat="server" Visible="false"></asp:HyperLink>
</div>