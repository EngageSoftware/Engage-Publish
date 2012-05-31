<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.CategoryControls.CategoryFeature" Codebehind="CategoryFeature.ascx.cs" %>
<div class="divItemsListing Normal">
    <asp:Literal ID="lblNoData" runat="server" Visible="false" />
	<asp:DataList ID="dlItems" runat="server" Width="100%" RepeatLayout="Table" OnItemDataBound="dlItems_ItemDataBound">
		<ItemTemplate>
			<div class="news_item_con">
				<div class="news_item_pad">
					<asp:Panel ID="pnlThumbnail" runat="server" cssclass="news_item_thumb">
					    <asp:Hyperlink ID="imgThumbnail" runat="server" ImageUrl='<%# GetThumbnailUrl(Eval("Thumbnail")) %>' alt='<%# Eval("Name") %>' NavigateUrl='<%# GetItemLinkUrl(Eval("ItemId")) %>' />
					</asp:Panel>
					<div class="news_item_copy">
						<div class="news_item_copy_pad">
							<div class="Normal">
							    <asp:HyperLink runat="server" ID="lnkName" CssClass="title" Text='<%# Eval("Name") %>' NavigateUrl='<%# GetItemLinkUrl(Eval("ItemId")) %>' />
								<br />
							    <asp:Literal runat="server" ID="lblDescription" Text='<%# Eval("Description") %>' />
							    <asp:Panel runat="server" ID="pnlReadMore" cssclass="item_listing_readmore">
							        <asp:HyperLink ID="lnkReadMore" runat="server" NavigateUrl='<%# GetItemLinkUrl(Eval("ItemId")) %>' resourceKey="lnkReadMore" />
							    </asp:Panel>
							</div>
						</div>
					</div>
				</div>
			</div>
		</ItemTemplate>
	</asp:DataList>
    <asp:HyperLink Runat="server" ID="lnkRss" Visible="False" />
</div>