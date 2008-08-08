<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.CustomDisplay" Codebehind="CustomDisplay.ascx.cs" %>
<asp:label id="lblMessage" Font-Bold="True" Font-Size="Larger" runat="server" Visible="False"></asp:label>
<div class="Normal"> 
    <%--<div class="<%= DataDisplayFormat %>">--%>
    <asp:Panel ID="pnlCategory" runat="server" cssclass="itemCategory">
        <asp:Label runat="server" ID="lblCategory" />
    </asp:Panel>
    <div class="divItemsListing">
        <asp:repeater id="lstItems" runat="server" OnItemDataBound="lstItems_ItemDataBound">
            <headertemplate/>
            <AlternatingItemTemplate>
            
            <div class='categoryItemList altCategoryItemList <%# GetItemTypeCssClass(Container.DataItem) %>'>
		            <asp:Panel ID="pnlThumbnail" runat="server" cssclass='itemThumbnail'>
		                <a href='<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>' target='<%# GetItemLinkTarget(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>'>
		                    <asp:Image runat="server" ID="imgThumbnail" ImageUrl='<%# GetThumbnailUrl(DataBinder.Eval(Container.DataItem, "Thumbnail")) %>' CssClass="thumbnailImage" AlternateText='<%# DataBinder.Eval(Container.DataItem, "ChildName") %>'/>
		                </a>
		            </asp:Panel>
		            <asp:Panel ID="pnlTitle" runat="server" cssclass="itemTitle">
		                <asp:Panel ID="pnlEditLink" runat="server" cssclass="itemEditLink">
		                    <asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%# BuildEditUrl(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "ChildItemId")), TabId, ModuleId, PortalId) %>' Text='<%# editText.ToString() %>' Visible='<%# visibility %>'/>
		                </asp:Panel>
		                <h2>
		                    <asp:HyperLink runat="server" ID="lnkTitle" NavigateUrl='<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>' Text='<%# DataBinder.Eval(Container.DataItem, "ChildName") %>' target='<%# GetItemLinkTarget(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>'/>
		                </h2>
		            </asp:Panel>
		            <asp:Panel ID="pnlAuthorDate" runat="server" CssClass="itemAuthorDate">
		                <asp:Panel ID="pnlAuthor" runat="server" CssClass="itemAuthor">
		                    <asp:Label id="lblAuthor" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DisplayName") %>'></asp:Label>
		                </asp:Panel>
		                <asp:Panel ID="pnlDate" runat="server" CssClass="itemDate">
			                <asp:Label id="lblDate" runat="server" Text='<%# FormatDate(DataBinder.Eval(Container.DataItem, "StartDate")) %>'></asp:Label>
		                </asp:Panel>
		            </asp:Panel>
		            <asp:Panel ID="pnlDescription" runat="server" cssclass="itemDescription">
		                <asp:Label runat="server" ID="lblDescription" Text='<%# FormatText(DataBinder.Eval(Container.DataItem, "ChildDescription")) %>'/>
		            </asp:Panel>
		            <asp:Panel ID="pnlReadMore" runat="server" cssclass="itemReadmore">
		                <asp:HyperLink runat="server" ID="lnkReadMore" NavigateUrl='<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>' Text="Read More..." ResourceKey="lnkReadMore" target='<%# GetItemLinkTarget(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>'/>
		            </asp:Panel>
	            </div>
            </AlternatingItemTemplate>
            <itemtemplate>
	            <div class='categoryItemList <%# GetItemTypeCssClass(Container.DataItem) %>'>
		            <asp:Panel ID="pnlThumbnail" runat="server" cssclass='itemThumbnail'>
		                <a href='<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>' target='<%# GetItemLinkTarget(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>'>
		                    <asp:Image runat="server" ID="imgThumbnail" ImageUrl='<%# GetThumbnailUrl(DataBinder.Eval(Container.DataItem, "Thumbnail")) %>' CssClass="thumbnailImage" AlternateText='<%# DataBinder.Eval(Container.DataItem, "ChildName") %>'/>
		                </a>
		            </asp:Panel>
		            <asp:Panel ID="pnlTitle" runat="server" cssclass="itemTitle">
		                <asp:Panel ID="pnlEditLink" runat="server" cssclass="itemEditLink">
		                    <asp:HyperLink runat="server" ID="lnkEdit" NavigateUrl='<%# BuildEditUrl(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "ChildItemId")), TabId, ModuleId, PortalId) %>' Text='<%# editText.ToString() %>' Visible='<%# visibility %>'/>
		                </asp:Panel>
		                <h2>
		                    <asp:HyperLink runat="server" ID="lnkTitle" NavigateUrl='<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>' Text='<%# DataBinder.Eval(Container.DataItem, "ChildName") %>'  target='<%# GetItemLinkTarget(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>'/>
		                </h2>
		            </asp:Panel>
		            <asp:Panel ID="pnlAuthorDate" runat="server" CssClass="itemAuthorDate">
		                <asp:Panel ID="pnlAuthor" runat="server" CssClass="itemAuthor">
		                    <asp:Label id="lblAuthor" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DisplayName") %>'></asp:Label>
		                </asp:Panel>
		                <asp:Panel ID="pnlDate" runat="server" CssClass="itemDate">
			                <asp:Label id="lblDate" runat="server" Text='<%# FormatDate(DataBinder.Eval(Container.DataItem, "StartDate")) %>'></asp:Label>
		                </asp:Panel>
		            </asp:Panel>
		            <asp:Panel ID="pnlDescription" runat="server" cssclass="itemDescription">
		                <asp:Label runat="server" ID="lblDescription" Text='<%# FormatText(DataBinder.Eval(Container.DataItem, "ChildDescription")) %>'/>
		            </asp:Panel>
		            <asp:Panel ID="pnlReadMore" runat="server" cssclass="itemReadmore">
		                <asp:HyperLink runat="server" ID="lnkReadMore" NavigateUrl='<%# GetItemLinkUrl(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>' Text="Read More..." ResourceKey="lnkReadMore" target='<%# GetItemLinkTarget(DataBinder.Eval(Container.DataItem, "ChildItemId")) %>'/>
		            </asp:Panel>
	            </div>
            </itemtemplate>
            <footertemplate/>
        </asp:repeater>
    </div>
    <asp:Panel ID="pnlPaging" runat="server" CssClass="Publish_CustomDisplayPaging">
        <asp:HyperLink ID="lnkPrevious" runat="server" resourcekey="lnkPrevious" Visible="false"  CssClass="Publish_lnkPrevious"></asp:HyperLink>
        <asp:HyperLink ID="lnkNext" runat="server" resourcekey="lnkNext" Visible="false" CssClass="Publish_lnkNext"></asp:HyperLink>
        
    </asp:Panel>
        
    <asp:HyperLink Runat="server" ID="lnkRss" Visible="False"/>
</div>
