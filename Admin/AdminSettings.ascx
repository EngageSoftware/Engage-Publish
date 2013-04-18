<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Admin.AdminSettings" CodeBehind="AdminSettings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelControl.ascx" %>

<asp:Panel ID="AdminSettingsWrap" runat="server" CssClass="dnnForm publishAdminSettings">
    <div class="dnnFormExpandContent"><a href="#"><%=LocalizeString("ExpandAll") %></a></div>
    
    <asp:Panel runat="server" ID="FirstTimeMessagePanel" CssClass="dnnFormMessage dnnFormWarning" Visible="False">
        <%=Localize("FirstTime") %>
    </asp:Panel>
    
    <h2 class="dnnFormSectionHead"><a href="#"><%=LocalizeString("lblRolesAndPermissions") %></a></h2>
    <fieldset class="dnnClear">
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plEmailNotificationRole" runat="server" ControlName="ddlEmailRoles" />
            <asp:DropDownList ID="ddlEmailRoles" DataTextField="RoleName" DataValueField="RoleName" runat="server" />
        </div>
        <div class="dnnFormItem">                
            <dnn:Label ResourceKey="plAuthorRole" runat="server" ControlName="ddlAuthor" />
            <asp:DropDownList ID="ddlAuthor" DataTextField="RoleName" DataValueField="RoleName" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plAdministratorRole" runat="server" ControlName="ddlRoles" />
            <asp:DropDownList ID="ddlRoles" DataTextField="RoleName" DataValueField="RoleName" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="lblAuthorCategoryEdit" runat="server" ControlName="chkAuthorCategoryEdit" />
            <asp:CheckBox ID="chkAuthorCategoryEdit" runat="server" />
        </div>
    </fieldset>

    <h2 class="dnnFormSectionHead"><a href="#"><%=LocalizeString("lblDefaultSettings") %></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plUseApprovals" runat="server" ControlName="chkUseApprovals" />
            <asp:CheckBox ID="chkUseApprovals" runat="server" Checked="true" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plEmailNotification" runat="server" ControlName="chkEmailNotification" />
            <asp:CheckBox ID="chkEmailNotification" runat="server" />
            <asp:CustomValidator runat="server" ResourceKey="cvEmailNotification" OnServerValidate="cvEmailNotification_ServerValidate" CssClass="dnnFormMessage dnnFormError" />
        </div>
        <asp:Panel runat="server" ID="MailNotConfiguredPanel" Visible="False" CssClass="dnnFormMessage dnnFormWarning">
            <%=Localize("lblMailNotConfigured") %>
        </asp:Panel>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plDefaultDisplayPage" runat="server" ControlName="ddlDefaultDisplay" />
            <asp:DropDownList ID="ddlDefaultDisplay" runat="server" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlDefaultDisplay" ResourceKey="rfvDefaultDisplayPage" InitialValue="-1" Display="Dynamic" CssClass="dnnFormMessage dnnFormError" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="lblDefaultCategory" runat="server" ControlName="lblDefaultCategory" />
            <asp:DropDownList ID="ddlDefaultCategory" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plThumbnailSelection" runat="server" ControlName="plThumbnailSelection" />
            <asp:RadioButtonList ID="radThumbnailSelection" runat="server" CssClass="dnnFormRadioButtons" RepeatDirection="Horizontal" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plThumbnailSubdirectory" runat="server" ControlName="chkThumbnailSubdirectory" />
            <asp:TextBox ID="txtThumbnailSubdirectory" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plEnablePublishFriendlyUrls" runat="server" ControlName="chkEnablePublishFriendlyUrls" />
            <asp:CheckBox ID="chkEnablePublishFriendlyUrls" runat="server" />
        </div>
        <asp:Panel runat="server" ID="FriendlyUrlsNotOnPanel" Visible="False" CssClass="dnnFormMessage dnnFormWarning">
            <%=Localize("lblFriendlyUrlsNotOn") %>
        </asp:Panel>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plEnablePaging" runat="server" ControlName="chkEnablePaging" />
            <asp:CheckBox ID="chkEnablePaging" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plDefaultCacheTime" runat="server" ControlName="txtDefaultCacheTime" />
            <asp:TextBox ID="txtDefaultCacheTime" runat="server" />
            <asp:RangeValidator ResourceKey="rvDefaultCacheTime" runat="server" ControlToValidate="txtDefaultCacheTime" Type="integer" MaximumValue="1000" MinimumValue="0" CssClass="dnnFormMessage dnnFormError" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plAdminPagingSize" runat="server" ControlName="txtAdminPagingSize" />
            <asp:TextBox ID="txtAdminPagingSize" runat="server" />
            <asp:RangeValidator ResourceKey="rvAdminPagingSize" runat="server" ControlToValidate="txtAdminPagingSize" Type="integer" MaximumValue="1000" MinimumValue="1" CssClass="dnnFormMessage dnnFormError" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plDefaultTextHtmlCategory" runat="server" ControlName="ddlDefaultTextHtmlCategory" />
            <asp:DropDownList ID="ddlDefaultTextHtmlCategory" runat="server" />
        </div>
    </fieldset>
    
    <h2 class="dnnFormSectionHead"><a href="#"><%=Localize("lblDisplayFunctionality") %></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plEnableViewTracking" runat="server" ControlName="chkEnableViewTracking" />
            <asp:CheckBox ID="chkEnableViewTracking" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plEnableRatings" runat="server" ControlName="chkEnableRatings" />
            <asp:CheckBox ID="chkEnableRatings" runat="server" OnCheckedChanged="chkEnableRatings_CheckedChanged" AutoPostBack="true" />
        </div>
        <asp:Panel ID="MaximumRatingPanel" runat="server" CssClass="dnnFormItem">
            <dnn:Label ResourceKey="plMaximumRating" runat="server" ControlName="txtMaximumRating" />
            <asp:TextBox ID="txtMaximumRating" runat="server"></asp:TextBox>
            <asp:CustomValidator runat="server" Display="Dynamic" ControlToValidate="txtMaximumRating" ResourceKey="cvMaximumRating" ValidateEmptyText="true" OnServerValidate="cvMaximumRating_ServerValidate" CssClass="dnnFormMessage dnnFormError" />
        </asp:Panel>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plReturnToListSession" runat="server" ControlName="chkReturnToListSession" />
            <asp:CheckBox ID="chkReturnToListSession" runat="server" />
        </div>
    </fieldset>
    
    <h2 class="dnnFormSectionHead"><a href="#"><%=Localize("Admin Edit Settings") %></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plShowItemId" runat="server" ControlName="chkShowItemId" />
            <asp:CheckBox ID="chkShowItemId" runat="server" Checked="true" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plEnableDisplayNameAsHyperlink" runat="server" ControlName="chkEnableDisplayNameAsHyperlink" />
            <asp:CheckBox ID="chkEnableDisplayNameAsHyperlink" runat="server" Checked="true" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plAllowRichTextDescriptions" runat="server" ControlName="chkAllowRichTextDescriptions" />
            <asp:CheckBox ID="chkAllowRichTextDescriptions" runat="server" Checked="true" AutoPostBack="true" OnCheckedChanged="chkAllowRichTextDescriptions_CheckedChanged" />
        </div>
        <asp:Panel runat="server" ID="DefaultRichTextDescriptionsPanel" CssClass="dnnFormItem">
            <dnn:Label ResourceKey="plDefaultRichTextDescriptions" runat="server" ControlName="chkDefaultRichTextDescriptions" />
            <asp:CheckBox ID="chkDefaultRichTextDescriptions" runat="server" Checked="true" />
        </asp:Panel>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plItemDescriptionWidth" runat="server" ControlName="lblItemDescriptionWidth" />
            <asp:TextBox ID="txtItemDescriptionWidth" runat="server" />
            <asp:RangeValidator ResourceKey="rvItemDescriptionWidth" runat="server" ControlToValidate="txtItemDescriptionWidth" Type="integer" MaximumValue="10000" MinimumValue="50" CssClass="dnnFormMessage dnnFormError" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plItemDescriptionHeight" runat="server" ControlName="lblItemDescriptionHeight" />
            <asp:TextBox ID="txtItemDescriptionHeight" runat="server" />
            <asp:RangeValidator ResourceKey="rvItemDescriptionHeight" runat="server" ControlToValidate="txtItemDescriptionHeight" Type="integer" MaximumValue="10000" MinimumValue="50" CssClass="dnnFormMessage dnnFormError" />
        </div>
    </fieldset>
    
    <h2 class="dnnFormSectionHead"><a href="#"><%=Localize("Article Edit Default Settings") %></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plUseEmbeddedArticles" runat="server" ControlName="chkUseEmbeddedArticles" />
            <asp:CheckBox ID="chkUseEmbeddedArticles" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plEnableEmailAFriend" runat="server" ControlName="chkDefaultEmailAFriend" />
            <asp:CheckBox ID="chkDefaultEmailAFriend" runat="server" Checked="true" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plPrinterFriendly" runat="server" ControlName="chkDefaultPrinterFriendly" />
            <asp:CheckBox ID="chkDefaultPrinterFriendly" runat="server" Checked="true" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plArticleRatings" runat="server" ControlName="chkDefaultArticleRatings" />
            <asp:CheckBox ID="chkDefaultArticleRatings" runat="server" Checked="true" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plArticleComments" runat="server" ControlName="chkDefaultArticleComments" />
            <asp:CheckBox ID="chkDefaultArticleComments" runat="server" Checked="true" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plReturnToList" runat="server" ControlName="chkDefaultReturnToList" />
            <asp:CheckBox ID="chkDefaultReturnToList" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plShowAuthor" runat="server" ControlName="chkDefaultShowAuthor" />
            <asp:CheckBox ID="chkDefaultShowAuthor" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plShowTags" runat="server" ControlName="chkDefaultShowTags" />
            <asp:CheckBox ID="chkDefaultShowTags" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plArticleTextWidth" runat="server" ControlName="txtArticleTextWidth" />
            <asp:TextBox ID="txtArticleTextWidth" runat="server" />
            <asp:RangeValidator ResourceKey="rvArticleTextWidth" runat="server" ControlToValidate="txtArticleTextWidth" Type="Integer" MaximumValue="10000" MinimumValue="50" CssClass="dnnFormMessage dnnFormError" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plArticleTextHeight" runat="server" ControlName="txtArticleTextHeight" />
            <asp:TextBox ID="txtArticleTextHeight" runat="server" />
            <asp:RangeValidator ResourceKey="rvArticleTextHeight" runat="server" ControlToValidate="txtArticleTextHeight" Type="Integer" MaximumValue="10000" MinimumValue="50" CssClass="dnnFormMessage dnnFormError" />
        </div>
    </fieldset>
    
    <h2 class="dnnFormSectionHead"><a href="#"><%=Localize("Community Settings") %></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plEnableComments" runat="server" ControlName="chkEnableComments" />
            <asp:CheckBox ID="chkEnableComments" runat="server" />
        </div>
        <asp:Panel ID="CommentsTypePanel" runat="server" CssClass="dnnFormItem">
            <dnn:Label ResourceKey="lblCommentsType" runat="server" ControlName="ddlCommentsType" />
            <asp:DropDownList ID="ddlCommentsType" runat="server" />
        </asp:Panel>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plCommentNotification" runat="server" ControlName="chkCommentNotification" />
            <asp:CheckBox ID="chkCommentNotification" runat="server" />
            <asp:CustomValidator runat="server" ResourceKey="cvEmailNotification" OnServerValidate="cvEmailNotification_ServerValidate" CssClass="dnnFormMessage dnnFormError" />
        </div>
        <asp:Panel runat="server" ID="MailNotConfiguredCommentPanel" Visible="False" CssClass="dnnFormMessage dnnFormWarning">
            <%=Localize("lblMailNotConfigured") %>
        </asp:Panel>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="lblEnablePing" runat="server" ControlName="chkEnablePing" />
            <asp:CheckBox ID="chkEnablePing" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="lblPingServers" runat="server" ControlName="txtPingServers" />
            <asp:TextBox ID="txtPingServers" runat="server" TextMode="MultiLine" Columns="40" Rows="4" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="lblChangedPage" runat="server" ControlName="txtPingChangedUrl" />
            <asp:TextBox ID="txtPingChangedUrl" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="Enable Invisible CAPTCHA" runat="server" ControlName="chkEnableInvisibleCaptcha" />
            <asp:CheckBox ID="chkEnableInvisibleCaptcha" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="Enable Timed CAPTCHA" runat="server" ControlName="chkEnableTimedCaptcha" />
            <asp:CheckBox ID="chkEnableTimedCaptcha" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="Enable Standard CAPTCHA" runat="server" ControlName="chkEnableStandardCaptcha" />
            <asp:CheckBox ID="chkEnableStandardCaptcha" runat="server" />
        </div>
    </fieldset>
    
    <h2 class="dnnFormSectionHead"><a href="#"><%=Localize("Tag Settings") %></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plEnableTags" runat="server" ControlName="chkEnableTags" />
            <asp:CheckBox ID="chkEnableTags" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plTagList" runat="server" ControlName="ddlTagList" />
            <asp:DropDownList ID="ddlTagList" runat="server" />
        </div>
    </fieldset>
    
    <h2 class="dnnFormSectionHead"><a href="#"><%=Localize("Module AddOns") %></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plEnableSimpleGallery" runat="server" ControlName="chkEnableSimpleGallery" />
            <asp:CheckBox ID="chkEnableSimpleGallery" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plEnableUltraMediaGallery" runat="server" ControlName="chkEnableUltraMediaGallery" />
            <asp:CheckBox ID="chkEnableUltraMediaGallery" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ResourceKey="plEnableVenexus" runat="server" ControlName="chkEnableVenexus" />
            <asp:CheckBox ID="chkEnableVenexus" runat="server" />
        </div>
    </fieldset>

    <asp:ValidationSummary ID="vsSummary" runat="server" CssClass="dnnFormMessage dnnFormValidationSummary" />
    <ul class="dnnActions dnnClear">
        <li><asp:LinkButton runat="server" ResourceKey="lnkUpdate" CssClass="dnnPrimaryAction" OnClick="lnkUpdate_Click" /></li>
    </ul>
</asp:Panel>
<script>
    jQuery(function ($) {
        var $wrap = $('#<%=AdminSettingsWrap.ClientID%>');
        $wrap.dnnPanels();
        $wrap.find('.dnnFormExpandContent a').dnnExpandAll({
            targetArea: $wrap
        });
    });
</script>