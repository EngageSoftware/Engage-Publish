<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.CategoryControls.CategoryFeature" Codebehind="CategoryFeature.ascx.cs" %>
<script type="text/javascript" src='<%= ApplicationUrl + DesktopModuleFolderName %>categorycontrols/main.js'></script>
<div class="divItemsListing Normal">
    <asp:Literal ID="lblNoData" runat="server" Visible="false"></asp:Literal>
	<asp:DataList ID="dlItems" Runat="server" Width="100%" RepeatLayout="Table" OnItemDataBound="dlItems_ItemDataBound">
		<ItemTemplate>
			<div class="news_item_con" onmouseover="cnSwap(this, 'news_item_con_over');" onmouseout="cnSwap(this, 'news_item_con');"
				onclick='window.location="<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ItemId")) %>";'>
				<div class="news_item_pad">
					<asp:Panel ID="pnlThumbnail" runat="server" cssclass="news_item_thumb">
					    <asp:Hyperlink ID="imgThumbnail" runat="server" ImageUrl='<%# GetThumbnailUrl(DataBinder.Eval(Container.DataItem, "Thumbnail")) %>' alt='<%# DataBinder.Eval(Container.DataItem, "Name") %>' NavigateUrl='<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ItemId")) %>' />
					</asp:Panel>
					<div class="news_item_copy">
						<div class="news_item_copy_pad">
							<asp:Panel ID="pnlName" runat="server" CssClass="Normal">
								<asp:HyperLink runat="server" ID="lnkName" CssClass="title" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' NavigateUrl='<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ItemId")) %>'></asp:HyperLink>
								<br />
								<asp:Literal runat="server" ID="lblDescription" Text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'></asp:Literal>
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

