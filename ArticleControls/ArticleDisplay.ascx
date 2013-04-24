<%@ Control Language="c#" AutoEventWireup="True" Inherits="Engage.Dnn.Publish.ArticleControls.ArticleDisplay" Codebehind="ArticleDisplay.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>

<div id="articleDisplay" class="Normal">
	<div id="divArticleTitle" runat="server" visible="false">
	    <h1 class="Head" id="publishTitle"><asp:literal ID="lblArticleTitle" Runat="server" /></h1>
	</div>

    <div id="divLastUpdated" class="divLastUpdated" runat="server" visible="false" align="right">
        <asp:Literal ID="lblLastUpdated" runat="server" />
	</div>
    <div class="ArticleUtilities">
		<asp:Panel ID="pnlPrinterFriendly" runat="server">
	        <asp:PlaceHolder ID="phPrinterFriendly" runat="server" />
	    </asp:Panel>
	    <asp:Panel ID="pnlEmailAFriend" runat="server">
	        <asp:PlaceHolder ID="phEmailAFriend" runat="server" />
	    </asp:Panel>
    </div>
	<asp:Panel ID="pnlAuthor" runat="server" Visible="false">
	    <asp:label ID="lblAuthorInfo" runat="server" resourcekey="lblAuthorInfo" />
	    <asp:literal ID="lblAuthor" runat="server" />
	</asp:Panel>
	
	<div id="articleThumbnails">
        <asp:Repeater ID="rpThumbnails" runat="server" OnItemDataBound="rpThumbnails_ItemDataBound">
            <ItemTemplate>
                <ajaxToolkit:PopupControlExtender ID="pceArticleThumbnail" runat="server" PopupControlID="pnlLargeImage" TargetControlID="pnlSmallImage" BehaviorID="pceArticleThumbnail" Position="Right">
                    <Animations>
                        <OnShow>
                            <Sequence>
                                <HideAction Visible="true" />
                                <FadeIn Duration="0.2" />
                            </Sequence>
                        </OnShow>
                        <OnHide>
                            <Sequence>
                                <FadeOut Duration="0.2" />
                                <HideAction Visible="false" />
                            </Sequence>
                        </OnHide>
                    </Animations>
                </ajaxToolkit:PopupControlExtender>

                <a class="mouseOverImage" href="javascript:void(0);">
                    <asp:Panel ID="pnlSmallImage" runat="server">
                        <img id="imgThumbnail" runat="server" src='<%#GetPhotoPath(Container.DataItem) %>' class='publishArticleThumbnail' title='<%#PhotoMouseOverText %>' alt='<%#GetPhotoAltText(Container.DataItem) %>' />
                    </asp:Panel>
                </a>
                <asp:Panel ID="pnlLargeImage" runat="server" style="visibility:hidden;position:absolute;" CssClass="publishArticleThumbnailWrapper">
                    <img src='<%#GetPhotoPath(Container.DataItem) %>' class='publishArticleThumbnail mouseOverPopup' <% if (HoverThumbnailHeight.HasValue) { %> height='<%=HoverThumbnailHeight %>' <% } %> <% if (HoverThumbnailWidth.HasValue) { %> width='<%=HoverThumbnailWidth %>' <% } %> alt='<%#GetPhotoAltText(Container.DataItem) %>' />
                </asp:Panel>
            </ItemTemplate>
        </asp:Repeater>
    </div>
	
	<div id="divRelatedArticle" runat="server" class="divRelatedArticle" visible="false">
	    <div id="divRelatedArticleHeader"></div>
	    <asp:PlaceHolder ID="phRelatedArticle" Runat="server" />
	</div>
	<div id="divArticleContent" class="Normal">
	    <asp:literal ID="lblArticleText" Runat="server" />
	</div>
	
    <asp:Panel ID="pnlTags" runat="server" Visible="false" CssClass="Publish_ArticleTags">
        <asp:label ID="lblTag" runat="server" resourcekey="lblTag" />
        <asp:PlaceHolder id="phTags" runat="server" EnableViewState="true" />
    </asp:Panel>

	<div id="divArticlePage" class="Normal">
	   <asp:HyperLink rel="prev" ID="lnkPreviousPage" CssClass="Publish_lnkPrevious" runat="server" />
	   <asp:HyperLink rel="next" ID="lnkNextPage" CssClass="Publish_lnkNext" runat="server" />
	</div>
	
	<div id="div1" class="Normal">
	    <asp:HyperLink ID="lnkConfigure" Runat="server" Visible="false" />
	</div>
	
	<asp:Panel ID="pnlReturnToList" runat="server" CssClass="Publish_ReturnToList">
	    <asp:HyperLink ID="lnkReturnToList" runat="server" />
	</asp:Panel>
	
	<div id="divRelatedArticlesControl">
	    <asp:PlaceHolder ID="phRelatedArticles" Runat="server" />
	</div>
	
	<asp:UpdatePanel ID="upnlRating" runat="server" UpdateMode="conditional" Visible="false">
	    <ContentTemplate>
            <div id="divRating" class="divRatingBefore">
                <asp:Label ID="lblRatingMessage" runat="server" resourcekey="lblRatingMessage" />
                <ajaxToolkit:Rating ID="ajaxRating" 
                    runat="server" MaxRating="5" 
                    StarCssClass="ratingStar" 
                    WaitingStarCssClass="savedRatingStar" 
                    FilledStarCssClass="filledRatingStar" 
                    EmptyStarCssClass="emptyRatingStar" 
                    OnChanged="ajaxRating_Changed" 
                    Visible="true"
                    AutoPostBack="true"
                    />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
        
    <asp:Panel ID="pnlComments" runat="server" visible="false">
        <asp:MultiView ID="mvCommentDisplay" runat="server" ActiveViewIndex="0">
            <asp:View ID="vwPublishComments" runat="server">
                <asp:Panel ID="pnlCommentDisplay" runat="server" Visible="false">
                    <div id="divCommentsDisplay">
                        <div class="Publish_CommentHeading">
                            <asp:Label ID="lblCommentHeading" resourcekey="lblCommentHeading" runat="server" CssClass="Head"/>
                        </div>
                        <asp:PlaceHolder ID="phCommentsDisplay" runat="server" />
                    </div>
                </asp:Panel>
                <div id="divCommentLink">
                    <asp:HyperLink ID="CommentPopupTriggerLink" runat="server" resourcekey="btnComment" Visible="false" CssClass="engagePublishModalLink" NavigateUrl="#" />
                </div>
            </asp:View>
            <asp:View ID="vwForumComments" runat="server">
                <div id="divCommentLink">
                    <asp:HyperLink ID="ForumCommentPopupTriggerLink" runat="server" resourcekey="btnForumComment" Visible="false" CssClass="engagePublishModalLink" NavigateUrl="#" />
                    <asp:HyperLink ID="lnkGoToForum" runat="server" resourcekey="lnkGoToForum" Visible="false" />
                </div>
            </asp:View>
        </asp:MultiView>
        <asp:Panel ID="pnlComment" runat="server" CssClass="commentPopup" style="display:none">
	        <asp:UpdatePanel ID="upnlComments" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlCommentEntry" runat="server" CssClass="commentEntry" DefaultButton="btnSubmitComment">
                        <div id="commentEntryWrapper" class="dnnForm">
                            <div id="commentText" class="dnnFormItem">
                                <asp:Label ID="lblInstructions" runat="server" ResourceKey="lblInstructions" AssociatedControlID="txtComment" />
                                <asp:TextBox TextMode="MultiLine" ID="txtComment" runat="server" EnableViewState="false" CssClass="commentTextbox" />
                                <asp:RequiredFieldValidator ID="rfvCommentText" resourcekey="rfvCommentText" runat="server" Display="None" ControlToValidate="txtComment" ValidationGroup="commentVal" />
                            </div>
                            <asp:Placeholder ID="pnlNameComment" runat="server">
                                <div id="commentFirstName" class="dnnFormItem">
                                    <asp:Label ID="lblFirstNameComment" runat="server" AssociatedControlID="txtFirstNameComment" />
                                    <asp:TextBox ID="txtFirstNameComment" runat="server" EnableViewState="false" CssClass="commentFirstNameTextbox" />
                                    <asp:RequiredFieldValidator ID="rfvFirstNameComment" resourcekey="rfvFirstNameComment" runat="server" Display="None" ControlToValidate="txtFirstNameComment" ValidationGroup="commentVal" />
                                </div>
                                <div id="commentLastName" class="dnnFormItem">
                                    <asp:Label ID="lblLastNameComment" runat="server" AssociatedControlID="txtLastNameComment" />
                                    <asp:TextBox ID="txtLastNameComment" runat="server" EnableViewState="false" CssClass="commentLastNameTextbox" />
                                    <asp:RequiredFieldValidator ID="rfvLastNameComment" resourcekey="rfvLastNameComment" runat="server" Display="None" ControlToValidate="txtLastNameComment" ValidationGroup="commentVal" />
                                </div>
                            </asp:Placeholder>
                            <asp:Placeholder ID="pnlEmailAddressComment" runat="server">
                                <div id="commentEmail" class="dnnFormItem">
                                    <asp:Label ID="lblEmailAddressComment" runat="server" resourcekey="lblEmailAddressComment" AssociatedControlID="txtEmailAddressComment" />
                                    <asp:TextBox ID="txtEmailAddressComment" runat="server" EnableViewState="false" CssClass="commentEmailTextbox" />
                                    <asp:RequiredFieldValidator ID="rfvEmailAddressComment" resourcekey="rfvEmailAddressComment" runat="server" Display="None" ControlToValidate="txtEmailAddressComment" ValidationGroup="commentVal" />
                                </div>
                            </asp:Placeholder>
                            <asp:Placeholder ID="pnlUrlComment" runat="server">
                                <div id="commentUrl" class="dnnFormItem">
                                    <asp:Label ID="lblUrlComment" runat="server" resourcekey="lblUrlComment" AssociatedControlID="txtUrlComment" />
                                    <asp:TextBox ID="txtUrlComment" runat="server" EnableViewState="false" CssClass="commentUrlTextbox" />
                                </div>
                            </asp:Placeholder>
                            
                            <dnn:DnnCaptcha ID="InvisibleCaptcha" runat="server" ProtectionMode="InvisibleTextBox" Display="None" ValidationGroup="commentVal" />
                            <dnn:DnnCaptcha ID="TimeoutCaptcha" runat="server" ProtectionMode="MinimumTimeout" Display="None" ValidationGroup="commentVal" />
                            <dnn:DnnCaptcha ID="StandardCaptcha" runat="server" ProtectionMode="Captcha" Display="None" EnableRefreshImage="True" ValidationGroup="commentVal" />
                            
                            <asp:ValidationSummary ID="valCommentSummary" runat="server" ShowSummary="true" DisplayMode="BulletList" ValidationGroup="commentVal" CssClass="dnnFormMessage dnnFormValidationSummary" />

                            <ul class="dnnActions dnnClear">
                                <li><asp:LinkButton ID="btnSubmitComment" runat="server" resourcekey="btnSubmitComment" OnClick="btnSubmitComment_Click" CssClass="commentSubmitButton dnnPrimaryAction" ValidationGroup="commentVal" /></li>
                                <li><asp:LinkButton ID="btnCancelComment" runat="server" resourcekey="btnCancelComment" OnClick="btnCancelComment_Click" CssClass="commentCancelButton dnnSecondayAction simplemodal-close" CausesValidation="false" ValidationGroup="commentVal" /></li>
                            </ul>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlCommentConfirmation" runat="server" Visible="false">
                        <div id="commentConfirmationWrapper">
                            <div id="commentConfirmationClose">
                                <asp:LinkButton ID="btnConfirmationClose" CausesValidation="false" runat="server" resourcekey="btnConfirmationClose" OnClick="btnConfirmationClose_Click" CssClass="commentConfirmCloseButton simplemodal-close" />
                            </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </asp:Panel>
</div>

<script type="text/javascript">
<% if (ajaxRating.Visible) { %>

    // Method called when the Rating is changed
    function changeCssClassMethod() {
       Sys.UI.DomElement.removeCssClass($get('divRating'), 'divRatingBefore'); 
       Sys.UI.DomElement.addCssClass($get('divRating'), 'divRatingAfter'); 
    }

    $addHandler(Sys.UI.DomElement.getElementById('<%= ajaxRating.ClientID %>'), 'click', changeCssClassMethod);

<% } %>
<% if (AllowPhotoGalleryIntegration) { %>

    var Engage_ThumbnailHashtable = {};
    var Engage_HideTimeoutId = null;

    function ShowImage(behaviorId, elementId)
    {
        var pnlLargeImage = $get(elementId);
        if (pnlLargeImage) {
            pnlLargeImage.style.display = 'block';
        }
        
        var pceArticleThumbnail = $find(behaviorId);
        if (pceArticleThumbnail) {
            pceArticleThumbnail.showPopup();
        }
    }
    
    function HideImage(behaviorId, elementId)
    {
        var pceArticleThumbnail = $find(behaviorId);
        if (pceArticleThumbnail) {
            pceArticleThumbnail.hidePopup();
            var pnlLargeImage = $get(elementId);
            if (pnlLargeImage) {
                var hideImage = function () {
                    pnlLargeImage.style.visibility = 'hidden';
                    pnlLargeImage.style.display = 'none';
                };
                setTimeout(hideImage, 250); 
            }
        }
    }
    
    function mouseIsInElement(eventElement, elementId) {
        var element = $get(elementId);
        var bounds = Sys.UI.DomElement.getBounds(element);
        
        return eventElement.offsetX >= 0 && eventElement.offsetX <= bounds.width && eventElement.offsetY >= 0 && eventElement.offsetY <= bounds.height;
    }
    
<% } %>
</script>