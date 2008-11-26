<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.ItemListing" Codebehind="ItemListing.ascx.cs" %>
<asp:label id="lblMessage" Font-Bold="True" Font-Size="Larger" runat="server" Visible="False"></asp:label>
<div class="Normal"> 
    <div class="<%= DataDisplayFormat %>">
	    <asp:repeater id="lstItems" runat="server" OnItemDataBound="lstItems_ItemDataBound">
		    <headertemplate>
		    </headertemplate>
		    <itemtemplate>
			    <div class="item_listing">
				    <asp:Panel ID="pnlThumbnail" runat="server" cssclass="item_listing_thumbnail"><asp:Image runat="server" ID="imgThumbnail" ImageUrl='<%# GetThumbnailUrl(DataBinder.Eval(Container.DataItem, "Thumbnail")) %>' CssClass="item_listing_thumbnail_image" AlternateText='<%# DataBinder.Eval(Container.DataItem, "ChildName") %>'></asp:Image></asp:Panel>
				    <asp:Panel ID="pnlCategory" runat="server" cssclass="item_listing_category"><asp:Literal runat="server" ID="lblCategory" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryName") %>'></asp:Literal></asp:Panel>
				    <asp:Panel ID="pnlTitle" runat="server" cssclass="item_listing_title">
				        <asp:HyperLink runat="server" ID="lnkTitle" NavigateUrl='<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>' Text='<%# DataBinder.Eval(Container.DataItem, "ChildName") %>' target='<%# GetItemLinkTarget(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>'></asp:HyperLink>
				    </asp:Panel>
				    <asp:Panel ID="pnlDescription" runat="server" cssclass="item_listing_abstract"><asp:Literal runat="server" ID="lblDescription" Text='<%# DataBinder.Eval(Container.DataItem, "ChildDescription") %>'></asp:Literal></asp:Panel>
				    <asp:Panel ID="pnlReadMore" runat="server" cssclass="item_listing_readmore"><asp:HyperLink runat="server" ID="lnkReadMore" NavigateUrl='<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>' Text="Read More..." ResourceKey="lnkReadMore" target='<%# GetItemLinkTarget(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>'></asp:HyperLink></asp:Panel>
			    </div>
		    </itemtemplate>
		    <footertemplate>
		    </footertemplate>
	    </asp:repeater>
	    <asp:HyperLink Runat="server" ID="lnkRss" Visible="False"></asp:HyperLink>
    </div>
</div>
