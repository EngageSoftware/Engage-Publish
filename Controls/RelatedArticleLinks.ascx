<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.RelatedArticleLinks" Codebehind="RelatedArticleLinks.ascx.cs" %>
<div id="divRelatedArticles" class="Normal">
	<asp:LinkButton ID="btnShowRelatedItem" Runat="server" resourcekey="btnShowRelatedItem"></asp:LinkButton>
	<div id="divRelatedLinks" runat="server" visible="False">
		<div id="ra_top"></div>
			<div id="ra_back">
			<div id="ra_back_pad">
				<asp:repeater id="lstItems" runat="server" OnItemDataBound="lstItems_DataBound">
					<headertemplate>
					</headertemplate>
					<itemtemplate>
						<div class="relatedArticle"><asp:HyperLink ID="lnkRelatedArticle" NavigateUrl='<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ItemId"), PortalId) %>' 
						    target='<%# GetItemLinkTarget(DataBinder.Eval(Container.DataItem, "ItemId")) %>' Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' runat="server"></asp:HyperLink></div>
					</itemtemplate>
					<footertemplate>
					</footertemplate>
				</asp:repeater>			
		</div>
		<div id="ra_bottom"></div>
		</div>
	</div>
</div>

