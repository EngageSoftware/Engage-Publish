<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.CategoryControls.CategoryFeature" Codebehind="CategoryFeature.ascx.cs" %>
<script type="text/javascript" src='<%= ApplicationUrl + DesktopModuleFolderName %>categorycontrols/main.js'></script>
<div class="divItemsListing">
    <asp:Label ID="lblNoData" runat="server" Visible="false" CssClass="Normal"></asp:Label>
	<asp:DataList ID="dlItems" Runat="server" Width="100%" RepeatLayout="Table" OnItemDataBound="dlItems_ItemDataBound">
		<ItemTemplate>
			<div class="news_item_con" onmouseover="cnSwap(this, 'news_item_con_over');" onmouseout="cnSwap(this, 'news_item_con');"
				onclick='window.location="<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ItemId")) %>";'>
				<div class="news_item_pad">
					<asp:Panel ID="pnlThumbnail" runat="server" cssclass="news_item_thumb">
					    <asp:Hyperlink ID="imgThumbnail" runat="server" ImageUrl='<%# GetThumbnailUrl(DataBinder.Eval(Container.DataItem, "Thumbnail")) %>' alt='<%# DataBinder.Eval(Container.DataItem, "Name") %>' NavigateUrl='<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ItemId")) %>' />
					</asp:Panel>
					<div class="news_item_copy" id="	">
						<div class="news_item_copy_pad">
							<asp:Panel ID="pnlName" runat="server" CssClass="Normal">
								<asp:HyperLink runat="server" ID="lnkName" CssClass="title" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' NavigateUrl='<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ItemId")) %>'></asp:HyperLink>
								<br />
								<asp:Label runat="server" ID="lblDescription" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'></asp:Label>
								<asp:Panel runat="server" ID="pnlReadMore" cssclass="item_listing_readmore"><asp:HyperLink ID="lnkReadMore" runat="server" NavigateUrl='<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ItemId")) %>' Text="Read More..." resourceKey="lnkReadMore"></asp:HyperLink></asp:Panel>
							</asp:Panel>
						</div>
					</div>
				</div>
			</div>
		</ItemTemplate>
	</asp:DataList>
	<asp:HyperLink Runat="server" ID="lnkRss" Visible="False"></asp:HyperLink>
</div>

