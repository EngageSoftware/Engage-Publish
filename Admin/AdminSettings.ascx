<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Admin.AdminSettings" Codebehind="AdminSettings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<div class="Normal">
<asp:UpdatePanel ID="pnlSyndication" runat="server" UpdateMode="conditional">
<ContentTemplate>
	<asp:Label ID="lblMessage" CssClass="error" Runat="server"></asp:Label>
	<table id="PublishSettingsTable" border="0" class="Normal AdminSettingsTable SettingsTable">
		<tr>
		    <td colspan="2" class="Head"><asp:label ID="lblRolesAndPermissions" resourcekey="lblRolesAndPermissions" runat="server" />
		    </td>
		</tr>
		<tr>
			<td>
				<dnn:label id="plEmailNotificationRole" runat="server" controlname="ddlRoles" text="Email Notification Role:"></dnn:label>
			</td>
			<td>
				<asp:DropDownList ID="ddlEmailRoles" DataTextField="RoleName" DataValueField="RoleName" Runat="server"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				<dnn:label id="plAuthorRole" runat="server" controlname="ddlAuthor" text="Author Role:"></dnn:label>
			</td>
			<td>
				<asp:DropDownList ID="ddlAuthor" DataTextField="RoleName" DataValueField="RoleName" Runat="server"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				<dnn:label id="plAdministratorRole" runat="server" controlname="ddlRoles" text="Administrator Role:"></dnn:label>
			</td>
			<td>
				<asp:DropDownList ID="ddlRoles" DataTextField="RoleName" DataValueField="RoleName" Runat="server"></asp:DropDownList>
			</td>
		</tr>		
		<tr>
			<td>
				<dnn:label id="lblAuthorCategoryEdit" runat="server" controlname="chkAuthorCategoryEdit" text="Author Category Permissions:"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkAuthorCategoryEdit" runat="server" />
			</td>
		</tr>
		<tr>
		    <td colspan="2" class="Head"><asp:label ID="lblDefaultSettings" resourcekey="lblDefaultSettings" runat="server" />
		    </td>
		</tr>
		<tr>
			<td>
				<dnn:label id="plEmailNotification" runat="server" controlname="chkEmailNotification" text="Email Notification:"></dnn:label>
				<asp:Label ID="lblMailNotConfigured" runat="server" Visible="false" ResourceKey="lblMailNotConfigured" CssClass="error"></asp:Label>	
			</td>
			<td>
				<asp:CheckBox ID="chkEmailNotification" Runat="server"></asp:CheckBox>
				<asp:CustomValidator Text="You cannot enable email notifications if SMTP settings are not configured." resourcekey="cvEmailNotification" ID="cvEmailNotification" runat="server" OnServerValidate="cvEmailNotification_ServerValidate"></asp:CustomValidator>
				    
			</td>
		</tr>
		<tr>
			<td>
				<dnn:label id="plDefaultDisplayPage" runat="server" controlname="ddlDefaultDisplay" text="Default Display Page:"></dnn:label>
			</td>
			<td>
				<asp:DropDownList ID="ddlDefaultDisplay" runat="server"></asp:DropDownList><asp:RequiredFieldValidator ID="rfvDefaultDisplayPage" runat="server" ControlToValidate="ddlDefaultDisplay" resourcekey="rfvDefaultDisplayPage" InitialValue="-1" Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td>
				<dnn:label id="plThumbnailSelection" runat="server" controlname="plThumbnailSelection" text="Thumbnail Selection Option:"></dnn:label>
			</td>
			<td>
				<asp:RadioButtonList ID="radThumbnailSelection" runat="server"></asp:RadioButtonList>
			</td>
		</tr>		
		<tr>
			<td>
				<dnn:label id="plThumbnailSubdirectory" runat="server" controlname="chkThumbnailSubdirectory" text="Thumbnail Subdirectory Name:"></dnn:label>
			</td>
			<td>
				<asp:TextBox ID="txtThumbnailSubdirectory" runat="server"/>
			</td>
		</tr>		

		<tr>
			<td>
				<dnn:label id="plEnablePublishFriendlyUrls" runat="server" controlname="chkEnablePublishFriendlyUrls" text="Enable Publish Friendly Urls:"></dnn:label>
			    <asp:Label ID="lblFriendlyUrlsNotOn" runat="server" resourcekey="lblFriendlyUrlsNotOn" Visible="false" CssClass="error"/>
			</td>
			<td>
				<asp:CheckBox ID="chkEnablePublishFriendlyUrls" runat="server" />
			</td>
		</tr>
        <tr>
			<td>
				<dnn:label id="plEnablePaging" runat="server" controlname="chkEnablePaging" text="Enable Article Paging Functionality:"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkEnablePaging" runat="server" />
			</td>
		</tr>		
        <tr>
			<td>
				<dnn:label id="plDefaultCacheTime" runat="server" controlname="txtDefaultCacheTime" text="Default Publish Cache Time:"></dnn:label>
			</td>
			<td>
				<asp:TextBox ID="txtDefaultCacheTime" runat="server" /> <asp:RangeValidator ID="rvDefaultCacheTime" resourcekey="rvDefaultCacheTime" runat="server" ControlToValidate="txtDefaultCacheTime" Type="integer" MaximumValue="1000" MinimumValue="0"></asp:RangeValidator>
			</td>
		</tr>
        <tr>
			<td>
				<dnn:label id="plEnableTags" runat="server" controlname="chkEnableTags" text="Enable Tagging:"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkEnableTags" runat="server" />
			</td>
		</tr>		
        <tr>
			<td>
				<dnn:label id="plTagList" runat="server" controlname="ddlTagList" text="Default Tag Cloud:"></dnn:label>
			</td>
			<td>
                <asp:DropDownList ID="ddlTagList" runat="server"></asp:DropDownList>
			</td>
		</tr>		

        <tr>
			<td>
				<dnn:label id="plEnableSimpleGallery" runat="server" controlname="chkEnableSimpleGallery"/>
			</td>
			<td>
				<asp:CheckBox ID="chkEnableSimpleGallery" runat="server" />
			</td>
		</tr>		
        <tr>
			<td>
				<dnn:label id="plEnableUltraMediaGallery" runat="server" controlname="chkEnableUltraMediaGallery"/>
			</td>
			<td>
				<asp:CheckBox ID="chkEnableUltraMediaGallery" runat="server" />
			</td>
		</tr>		
        <tr>
			<td>
				<dnn:label id="plEnableVenexus" runat="server" controlname="chkEnableVenexus" text="Enable Venexus Search Integration:"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkEnableVenexus" runat="server" />
			</td>
		</tr>		
		
		<tr>
		    <td colspan="2" class="Head"><asp:label ID="lblDisplayFunctionality" resourcekey="lblDisplayFunctionality" runat="server" />
		    </td>
		    
		</tr>
		<tr>
			<td>
				<dnn:label id="plEnableViewTracking" runat="server" controlname="chkEnableViewTracking" text="Track Article/Category Views:"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkEnableViewTracking" runat="server" />
			</td>
		</tr>
        <tr>
			<td>
				<dnn:label id="plEnableRatings" runat="server" controlname="chkEnableRatings" text="Enable Ratings: "></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkEnableRatings" runat="server" OnCheckedChanged="chkEnableRatings_CheckedChanged" AutoPostBack="true" />
			</td>
		</tr>
		<tr id="rowMaximumRating" runat="server">
            <td>
                <dnn:label ID="plMaximumRating" runat="server" ControlName="txtMaximumRating" Text="Maximum Rating: "/>
            </td>
            <td>
                <asp:TextBox ID="txtMaximumRating" runat="server"></asp:TextBox><asp:CustomValidator ID="cvMaximumRating" runat="server" Display="Dynamic" ControlToValidate="txtMaximumRating" resourceKey="cvMaximumRating" ValidateEmptyText="true" OnServerValidate="cvMaximumRating_ServerValidate" />
            </td>
        </tr>
<%--        <tr id="rowAnonymousRatings" runat="server">
            <td>
                <dnn:label ID="plAnonymousRatings" runat="server" ControlName="chkAnonymousRatings" Text="Allow ratings from unauthenticated users: "/>
            </td>
            <td>
                <asp:CheckBox ID="chkAnonymousRatings" runat="server" />
            </td>
	    </tr>--%>
	    <tr>
			<td>
				<dnn:label id="plEnableComments" runat="server" controlname="chkEnableComments" text="Enable Comments: "></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkEnableComments" runat="server" /> <%--OnCheckedChanged="chkEnableComments_CheckedChanged" AutoPostBack="true" />--%>
			</td>
		</tr>
	    <tr id="rowCommentsType" runat="server">
			<td>
				<dnn:label id="lblCommentsType" runat="server" controlname="ddlCommentsType" text="Type of Comments: "></dnn:label>
			</td>
			<td>
				<asp:DropDownList ID="ddlCommentsType" runat="server" />
			</td>
		</tr>
        <tr>
			<td>
				<dnn:label id="plReturnToListSession" runat="server" controlname="chkReturnToListSession" text="Enable Session storage for the Return To List Link"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkReturnToListSession" runat="server" />
			</td>
		</tr>

		
<%--		<tr id="rowCommentApproval" runat="server">
            <td>
                <dnn:label ID="plCommentApproval" runat="server" ControlName="chkCommentApproval" Text="Require comments to be approved: "/>
            </td>
            <td>
                <asp:CheckBox ID="chkCommentApproval" runat="server" OnCheckedChanged="ShowCommentAutoApprove" AutoPostBack="true" />
            </td>
        </tr>
        <tr id="rowCommentAnonymous" runat="server">
            <td>
                <dnn:label ID="plCommentAnonymous" runat="server" ControlName="chkAnonymousComment" Text="Allow comments from unauthenticated users: "/>
            </td>
            <td>
                <asp:CheckBox ID="chkAnonymousComment" runat="server" OnCheckedChanged="ShowCommentAutoApprove" AutoPostBack="true" />
            </td>
        </tr>
        <tr id="rowCommentAutoApprove" runat="server">
            <td>
                <dnn:label ID="plCommentAutoApprove" runat="server" ControlName="chkCommentAutoApprove" Text="Automatically approve comments from authenticated users: "/>
            </td>
            <td>
                <asp:CheckBox ID="chkCommentAutoApprove" runat="server" />
            </td>
	    </tr>--%>

		<tr>
		    <td colspan="2" class="Head"><asp:label ID="lblAdminEdit" resourcekey="lblAdminEdit" runat="server" />
		    </td>
		</tr>
		<tr>
			<td>
				<dnn:label id="plUseApprovals" runat="server" controlname="chkUseApprovals" text="Use Approvals:"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkUseApprovals" runat="server" Checked="true" />
			</td>
		</tr>		
		<tr>
			<td>
				<dnn:label id="plShowItemId" runat="server" controlname="chkShowItemId" text="Show Item Id When Editing an Item:"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkShowItemId" runat="server" Checked="true" />
			</td>
		</tr>

		<tr>
			<td>
				<dnn:label id="plEnableDisplayNameAsHyperlink" runat="server" controlname="chkEnableDisplayNameAsHyperlink" text="Display option of whether to show an item title as a hyperlink in lists:"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkEnableDisplayNameAsHyperlink" runat="server" Checked="true" />
			</td>
		</tr>		
		<tr>
			<td>
				<dnn:label id="plAllowRichTextDescriptions" runat="server" controlname="chkAllowRichTextDescriptions" text="Allow Rich-Text Descriptions for Items:"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkAllowRichTextDescriptions" runat="server" Checked="true" />
			</td>
		</tr>
		<tr>
			<td>
				<dnn:label id="plItemDescriptionWidth" runat="server" controlname="lblItemDescriptionWidth" text="Item Description Width:"></dnn:label>
			</td>
			<td>
				<asp:TextBox ID="txtItemDescriptionWidth" runat="server" Text="500" />
				<asp:RangeValidator ID="rvItemDescriptionWidth" resourcekey="rvItemDescriptionWidth" runat="server" ControlToValidate="txtItemDescriptionWidth" Type="integer" MaximumValue="10000" MinimumValue="50"></asp:RangeValidator>
			</td>
		</tr>
		<tr>
			<td>
				<dnn:label id="plItemDescriptionHeight" runat="server" controlname="lblItemDescriptionHeight" text="Item Description Height:"></dnn:label>
			</td>
			<td>
				<asp:TextBox ID="txtItemDescriptionHeight" runat="server" Text="300" />
				<asp:RangeValidator ID="rvItemDescriptionHeight" resourcekey="rvItemDescriptionHeight" runat="server" ControlToValidate="txtItemDescriptionHeight" Type="integer" MaximumValue="10000" MinimumValue="50"></asp:RangeValidator>
			</td>
		</tr>

		<tr>
		    <td colspan="2" class="Head"><asp:label ID="lblArticleEditDefaults" resourcekey="lblArticleEditDefaults" runat="server" />
		    </td>
		</tr>
		<tr>
			<td>
				<dnn:label id="plUseEmbeddedArticles" runat="server" controlname="chkUseEmbeddedArticles" text="Use Embedded Articles:"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkUseEmbeddedArticles" runat="server" />
			</td>
		</tr>
		<tr>
			<td>
				<dnn:label id="plEnableEmailAFriend" runat="server" controlname="chkDefaultEmailAFriend" text="Default Setting for Email A Friend:"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkDefaultEmailAFriend" runat="server" Checked="true" />
			</td>
		</tr>
		<tr>
			<td>
				<dnn:label id="plPrinterFriendly" runat="server" controlname="chkDefaultPrinterFriendly" text="Default Setting for Printer Friendly:"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkDefaultPrinterFriendly" runat="server" Checked="true" />
			</td>
		</tr>
		<tr>
			<td>
				<dnn:label id="plArticleRatings" runat="server" controlname="chkDefaultArticleRatings" text="Default Setting for Ratings"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkDefaultArticleRatings" runat="server" Checked="true" />
			</td>
		</tr>

        <tr>
			<td>
				<dnn:label id="plArticleComments" runat="server" controlname="chkDefaultArticleComments" text="Default Setting for Comments"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkDefaultArticleComments" runat="server" Checked="true" />
			</td>
		</tr>
		
        <tr>
			<td>
				<dnn:label id="plReturnToList" runat="server" controlname="chkDefaultReturnToList" text="Default Setting for Return to List"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkDefaultReturnToList" runat="server" />
			</td>
		</tr>

        <tr>
			<td>
				<dnn:label id="plShowAuthor" runat="server" controlname="chkDefaultShowAuthor" text="Default Setting for Show Author"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkDefaultShowAuthor" runat="server" />
			</td>
		</tr>

        <tr>
			<td>
				<dnn:label id="plShowTags" runat="server" controlname="chkDefaultShowTags" text="Default Setting for Show Tags"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkDefaultShowTags" runat="server" />
			</td>
		</tr>


		<tr>
			<td>
				<dnn:label id="plArticleTextWidth" runat="server" controlname="txtArticleTextWidth" text="Article Text Width:"></dnn:label>
			</td>
			<td>
				<asp:TextBox ID="txtArticleTextWidth" runat="server" Text="500" />
				<asp:RangeValidator ID="rvArticleTextWidth" resourcekey="rvArticleTextWidth" runat="server" ControlToValidate="txtArticleTextWidth" Type="integer" MaximumValue="10000" MinimumValue="50"></asp:RangeValidator>
			</td>
		</tr>
		<tr>
			<td>
				<dnn:label id="plArticleTextHeight" runat="server" controlname="txtArticleTextHeight" text="Article Text Height:"></dnn:label>
			</td>
			<td>
				<asp:TextBox ID="txtArticleTextHeight" runat="server" Text="500" />
				<asp:RangeValidator ID="rvArticleTextHeight" resourcekey="rvArticleTextHeight" runat="server" ControlToValidate="txtArticleTextHeight" Type="integer" MaximumValue="10000" MinimumValue="50"></asp:RangeValidator>
			</td>
		</tr>
        <tr>
		    <td colspan="2" class="Head"><asp:label ID="lblCommunityServices" resourcekey="lblCommunityServices" runat="server" />
		    </td>
		</tr>
<% 
/*
        <tr>
		    <td colspan="2" class="Head"><asp:label ID="lblPingServices" resourcekey="lblPingServices" runat="server" />
		    </td>
		</tr>
		*/
		
 %>		
		<tr>
			<td>
				<dnn:label id="lblEnablePing" runat="server" controlname="chkEnablePing" text="Enable Pinging Services:"></dnn:label>
			</td>
			<td>
				<asp:CheckBox ID="chkEnablePing" runat="server" />
			</td>
		</tr>
		<tr>
			<td>
				<dnn:label id="lblPingServers" runat="server" controlname="txtPingServers" text="Ping Servers"></dnn:label>
			</td>
			<td>
				<asp:TextBox ID="txtPingServers" runat="server" TextMode="MultiLine" Columns="50" Rows="10"></asp:TextBox>
			</td>
		</tr>
		<tr>
			<td>
				<dnn:label id="lblChangedPage" runat="server" controlname="txtPingChangedUrl" text="Changed Page"></dnn:label>
			</td>
			<td>
				<asp:TextBox ID="txtPingChangedUrl" runat="server" TextMode="SingleLine" Columns="50"></asp:TextBox>
			</td>
		</tr>
	</table>
</ContentTemplate>
</asp:UpdatePanel>
    <br />
    <br />
    <asp:ValidationSummary ID="vsSummary" runat="server" />
    <asp:linkbutton id="lnkUpdate" Runat="server" ResourceKey="lnkUpdate" CssClass="CommandButton" OnClick="lnkUpdate_Click"></asp:linkbutton>
</div>

