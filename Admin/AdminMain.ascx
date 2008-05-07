<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Admin.AdminMain" Codebehind="AdminMain.ascx.cs" %>
<div class="admin_toolbar">
	<div class="admin_item">
		<div class="admin_item_icon">
		    <div id="admin_item_articles" class="admin_item_articles" runat="server">
		        <a href='<%= BuildLinkUrl("&amp;mid="+ ModuleId +"&amp;ctl=admincontainer&amp;adminType=articlelist")%>'><img src='<%=ApplicationUrl%>/images/1x1.gif' alt="Publish Admin Articles" border="0" height="45"/></a>
		    </div>
		</div>
		<div class="admin_item_text">
			<div class="Normal">
				<a href='<%= BuildLinkUrl("&amp;mid="+ ModuleId + "&amp;ctl=admincontainer&amp;adminType=articlelist")%>'>
					<asp:Label ID="lblArticles" ResourceKey="lblArticles" Runat="server"></asp:Label></a>
			</div>
		</div>
	</div>
    <div class="admin_item">
        <div class="admin_item_icon">
		    <div id="admin_item_categories" class="admin_item_categories" runat="server">
		        <a href='<%= BuildLinkUrl("&amp;mid="+ ModuleId +"&amp;ctl=admincontainer&amp;adminType=categorylist")%>'><img src='<%=ApplicationUrl%>/images/1x1.gif' alt="Publish Admin Categories" border="0" height="45"/></a>
		    </div>
		</div>
		<div class="admin_item_text">
			<div class="Normal">
				<a href='<%= BuildLinkUrl("&amp;mid="+ ModuleId +"&amp;ctl=admincontainer&amp;adminType=categorylist")%>'>
					<asp:Label ID="lblCategories" ResourceKey="lblCategories" Runat="server"></asp:Label></a>
			</div>
			
			
			
		</div>
	</div>


    <div class="admin_item" id="divComments" runat="server" visible="false">
        <div class="admin_item_icon">
            <div id="admin_item_comments" class="admin_item_comments" runat="server">
                <a href='<%= BuildLinkUrl("&amp;mid=" + ModuleId + "&amp;ctl=admincontainer&amp;adminType=commentList")%>'><img src='<%=ApplicationUrl %>/images/1x1.gif' alt="Publish Admin Comments" border="0" height="45"/></a>
            </div>
        </div>
        
        <div class="admin_item_text">
            <div class="Normal">
                <a href='<%= BuildLinkUrl("&amp;mid=" + ModuleId + "&amp;ctl=admincontainer&amp;adminType=commentList")%>'>
                    <asp:Label ID="lblComments" ResourceKey="lblComments" runat="server"></asp:Label></a>
            </div>
        </div>
    </div>


	<div class="admin_item" id="divSettings" runat="server" Visible="false">
		
        <div class="admin_item_icon">
		    <div id="admin_item_settings" class="admin_item_settings" runat="server">
		        <a href='<%= BuildLinkUrl("&amp;mid="+ ModuleId +"&amp;ctl=admincontainer&amp;adminType=amsSettings")%>'><img src='<%=ApplicationUrl%>/images/1x1.gif' alt="Publish Admin Settings" border="0" height="45"/></a>
		    </div>
		</div>
			
		<div class="admin_item_text">
			<div class="Normal">
				<a href='<%= BuildLinkUrl("&amp;mid="+ ModuleId +"&amp;ctl=admincontainer&amp;adminType=amsSettings")%>'>
					<asp:Label ID="lblAmsSettings" ResourceKey="lblAmsSettings" Runat="server"></asp:Label></a>
			</div>
		</div>
	</div>


	<div class="admin_item" id="divAdminTools" runat="server" Visible="false">
		
        <div class="admin_item_icon">
		    <div id="admin_item_admintools" class="admin_item_admintools" runat="server">
		        <a href='<%= BuildLinkUrl("&amp;mid="+ ModuleId +"&amp;ctl=admincontainer&amp;adminType=admintools")%>'><img src='<%=ApplicationUrl%>/images/1x1.gif' alt="Publish Admin Tools" border="0" height="45"/></a>
		    </div>
		</div>
			
		<div class="admin_item_text">
			<div class="Normal">
				<a href='<%= BuildLinkUrl("&amp;mid="+ ModuleId +"&amp;ctl=admincontainer&amp;adminType=admintools")%>'>
					<asp:Label ID="lblAdminTools" ResourceKey="lblAdminTools" Runat="server"></asp:Label></a>
			</div>
		</div>
	</div>




	<div class="admin_item" id="divDelete" runat="server" Visible="false">
        <div class="admin_item_icon">
		    <div id="admin_item_delete" class="admin_item_delete" runat="server">
		        <a href='<%= BuildLinkUrl("&amp;mid="+ ModuleId +"&amp;ctl=admincontainer&amp;adminType=deleteItem")%>'><img src='<%=ApplicationUrl%>/images/1x1.gif' alt="Publish Admin Delete Items" border="0" height="45"/></a>
		    </div>
		</div>

		<div class="admin_item_text">
			<div class="Normal">
				<a href='<%= BuildLinkUrl("&amp;mid="+ ModuleId +"&amp;ctl=admincontainer&amp;adminType=deleteItem")%>'>
					<asp:Label ID="lblDeleteItem" ResourceKey="lblDeleteItem" Runat="server"></asp:Label></a>
			</div>
		</div>
	</div>
	
	
	<div class="admin_item" id="divSyndication" runat="server" Visible="false">
        <div class="admin_item_icon">
		    <div id="admin_item_syndication" class="admin_item_syndication" runat="server">
		        <a href='<%= BuildLinkUrl("&amp;mid="+ ModuleId +"&amp;ctl=admincontainer&amp;adminType=syndication")%>'><img src='<%=ApplicationUrl%>/images/1x1.gif' alt="Syndication Configuration" border="0" height="45"/></a>
		    </div>
		</div>

		<div class="admin_item_text">
			<div class="Normal">
				<a href='<%= BuildLinkUrl("&amp;mid="+ ModuleId +"&amp;ctl=admincontainer&amp;adminType=syndication")%>'>
					<asp:Label ID="lblSyndication" ResourceKey="lblSyndication" Runat="server"></asp:Label></a>
			</div>
		</div>
	</div>
</div>
<br clear="all" />
<hr width="100%" />

