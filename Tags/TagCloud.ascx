<%@ Control Language="c#" AutoEventWireup="False" Inherits="Engage.Dnn.Publish.Tags.TagCloud" Codebehind="TagCloud.ascx.cs" %>
    
<div id="publishTagCloud">

    <div id="publishTagFilter" runat="server" class="publishTagFilters Normal">
        <asp:HyperLink ID="lnkTagFilters" runat="server" resourcekey="lnkTagFilters"></asp:HyperLink>
        <asp:PlaceHolder ID="phTagFilters" runat="server"></asp:PlaceHolder>
    </div>

    <ol>
        <asp:PlaceHolder ID="phTagCloud" runat="server"></asp:PlaceHolder>
    </ol>
</div>