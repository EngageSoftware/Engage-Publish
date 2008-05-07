<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.ArticleControls.ArticleCategoryListing" Codebehind="ArticleCategoryListing.ascx.cs" %>

	<asp:DataList ID="dlArticles" Runat="server" Width="100%">
		<AlternatingItemStyle BackColor="#FFFFFF" />
		<ItemStyle BackColor="#EBEBEB" />
		<ItemTemplate>
			<table border="0" cellspacing="10" cellpadding="0" width="100%">
				<tr>
					<td valign="top" align="center">
						<asp:HyperLink ID="lnkThumbnail" Runat="server" />
					</td>
					<td valign="top" align="left" width="100%">
						<asp:Label ID="lblName" Runat="server" />
						<asp:Label ID="lblDescription" Runat="server" />
					</td>
				</tr>
			</table>
		</ItemTemplate>
	</asp:DataList>

