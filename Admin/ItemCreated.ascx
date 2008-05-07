<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Admin.ItemCreated" Codebehind="ItemCreated.ascx.cs" %>
<div id="divItemVersions" runat="server">
	<asp:Label id="lblItemId" ResourceKey="lblItemId" Runat="server" CssClass="Normal"/> 
	<asp:Label id="lblItemIdValue" Runat="server" CssClass="Normal"/> 
	<asp:Label id="lblItemCreated" ResourceKey="lblItemCreated" Runat="server" CssClass="Normal"/> 
	<br />
	<asp:HyperLink ID="lnkItemVersion" Runat="server">
	    <asp:Label id="lblItemVersion" ResourceKey="lblItemVersion" Runat="server" CssClass="Normal"/>
	</asp:HyperLink>
    <br />
	<asp:HyperLink ID="lnkCategoryList" Runat="server">
	    <asp:Label id="lblCategoryList" ResourceKey="lblCategoryList" Runat="server" CssClass="Normal"/>
	</asp:HyperLink>
</div>

