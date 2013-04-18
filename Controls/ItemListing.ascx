<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.ItemListing" Codebehind="ItemListing.ascx.cs" %>
<asp:Label ID="lblMessage" Font-Bold="True" Font-Size="Larger" runat="server" Visible="False" />
<div class="Normal"> 
    <div class="<%= DataDisplayFormat %>">
	    <asp:Repeater ID="lstItems" runat="server" OnItemDataBound="lstItems_ItemDataBound">
		    <ItemTemplate>
			    <div class="item_listing">
			        <asp:Panel ID="pnlThumbnail" runat="server" cssclass="item_listing_thumbnail"><asp:Image runat="server" ID="imgThumbnail" ImageUrl='<%# GetThumbnailUrl(DataBinder.Eval(Container.DataItem, "Thumbnail")) %>' CssClass="item_listing_thumbnail_image" AlternateText='<%# DataBinder.Eval(Container.DataItem, "ChildName") %>' /></asp:Panel>
			        <asp:Panel ID="pnlCategory" runat="server" cssclass="item_listing_category"><asp:Literal runat="server" ID="lblCategory" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryName") %>' /></asp:Panel>
				    <asp:Panel ID="pnlTitle" runat="server" cssclass="item_listing_title">
				        <asp:HyperLink runat="server" ID="lnkTitle" NavigateUrl='<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>' Text='<%# DataBinder.Eval(Container.DataItem, "ChildName") %>' target='<%# GetItemLinkTarget(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>' />
				    </asp:Panel>
			        <asp:Panel ID="pnlDescription" runat="server" cssclass="item_listing_abstract"><asp:Literal runat="server" ID="lblDescription" Text='<%# DataBinder.Eval(Container.DataItem, "ChildDescription") %>' /></asp:Panel>
			        <asp:Panel ID="pnlReadMore" runat="server" cssclass="item_listing_readmore"><asp:HyperLink runat="server" ID="lnkReadMore" NavigateUrl='<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>' Text="Read More..." ResourceKey="lnkReadMore" target='<%# GetItemLinkTarget(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>' /></asp:Panel>
			    </div>
		    </ItemTemplate>
	    </asp:Repeater>
        <asp:HyperLink Runat="server" ID="lnkRss" Visible="False" />
    </div>
</div>
