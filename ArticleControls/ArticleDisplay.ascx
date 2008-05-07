<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.ArticleControls.ArticleDisplay" Codebehind="ArticleDisplay.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>

<div id="articleDisplay" class="Normal">
    <div id="divLastUpdated" class="divLastUpdated" runat="server" visible="false" align="right">
		<asp:label ID="lblLastUpdated" Runat="server"></asp:label>
	</div>
	<asp:Panel ID="pnlPrinterFriendly" runat="server">
	    <asp:PlaceHolder ID="phPrinterFriendly" Runat="server"></asp:PlaceHolder>
	</asp:Panel>
	<asp:Panel ID="pnlEmailAFriend" runat="server">
	<asp:PlaceHolder ID="phEmailAFriend" Runat="server"></asp:PlaceHolder>
	</asp:Panel>

	<asp:Panel ID="pnlAuthor" runat="server" Visible="false">
	    <asp:label ID="lblAuthorInfo" runat="server" resourcekey="lblAuthorInfo"></asp:label>
	    <asp:label ID="lblAuthor" runat="server"></asp:label>
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
	
	<div id="divArticleTitle" class="Head" runat="server" visible="false">
		<div id="publishTitle"><asp:label ID="lblArticleTitle" Runat="server"></asp:label></div>
	</div>
	<div id="divRelatedArticle" runat="server" class="divRelatedArticle" visible="false">
	    <div id="divRelatedArticleHeader"></div>
	    <asp:PlaceHolder ID="phRelatedArticle" Runat="server"></asp:PlaceHolder>
	    </div>
	<div id="divArticleContent" class="Normal">
		<asp:label ID="lblArticleText" Runat="server"></asp:label>
	</div>
	
    <asp:Panel ID="pnlTags" runat="server" Visible="false" CssClass="Publish_ArticleTags">
        <asp:label ID="lblTag" runat="server" resourcekey="lblTag"></asp:label>
        <asp:PlaceHolder id="phTags" runat="server" EnableViewState="true"></asp:PlaceHolder>
    </asp:Panel>

	<div id="divArticlePage" class="Normal">
	   <asp:HyperLink rel="prev" ID="lnkPreviousPage" runat="server" />
	   <asp:HyperLink rel="next" ID="lnkNextPage" runat="server" />
	</div>
	
	<div id="div1" class="Normal">
		<asp:HyperLink ID="lnkConfigure" Runat="server" Visible="false"></asp:HyperLink>
	</div>
	
	<asp:Panel ID="pnlReturnToList" runat="server" CssClass="Publish_ReturnToList">
	    <asp:HyperLink ID="lnkReturnToList" runat="server"></asp:HyperLink>
	</asp:Panel>
	
	<div id="divRelatedArticlesControl">
		<asp:PlaceHolder ID="phRelatedArticles" Runat="server"></asp:PlaceHolder>
	</div>
	
	<asp:UpdatePanel ID="upnlRating" runat="server" UpdateMode="conditional" Visible="false">
	    <ContentTemplate>
            <div id="divRating" class="divRatingBefore">
                <asp:Label ID="lblRatingMessage" runat="server" resourcekey="lblRatingMessage" Visible="false"></asp:Label>
                <ajaxToolkit:Rating ID="ajaxRating" runat="server" MaxRating="5" StarCssClass="ratingStar" WaitingStarCssClass="savedRatingStar" FilledStarCssClass="filledRatingStar" EmptyStarCssClass="emptyRatingStar" OnChanged="ajaxRating_Changed" Visible="true"/>
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
                    <asp:LinkButton ID="btnComment" runat="server" resourcekey="btnComment" Visible="false"/>
                </div>
            </asp:View>
            <asp:View ID="vwForumComments" runat="server">
                <div id="divCommentLink">
                    <asp:LinkButton ID="btnForumComment" runat="server" resourcekey="btnForumComment" Visible="false"/>
                    <asp:HyperLink ID="lnkGoToForum" runat="server" resourcekey="lnkGoToForum" Visible="false" />
                </div>
            </asp:View>
        </asp:MultiView>
        <ajaxToolkit:ModalPopupExtender ID="mpeComment" runat="server" BackgroundCssClass="commentBackground" PopupControlID="pnlComment" TargetControlID="btnComment" BehaviorID="mpeComment" />
        <ajaxToolkit:ModalPopupExtender ID="mpeForumComment" runat="server" BackgroundCssClass="commentBackground" PopupControlID="pnlComment" TargetControlID="btnForumComment" BehaviorID="mpeForumComment" />
        <asp:Panel ID="pnlComment" runat="server" CssClass="commentPopup" style="display:none">
	        <asp:UpdatePanel ID="upnlComments" runat="server" UpdateMode="Conditional">
                <ContentTemplate>   
                    <asp:Panel ID="pnlCommentEntry" runat="server" CssClass="commentEntry">
                        <div id="commentEntryWrapper">
                            <div id="commentInstructions">
                                <asp:Label ID="lblInstructions" runat="server" ResourceKey="lblInstructions"></asp:Label></div>
                            <div id="commentText">
                                <asp:TextBox TextMode="MultiLine" ID="txtComment" runat="server" EnableViewState="false" CssClass="commentTextbox" /><asp:RequiredFieldValidator ID="rfvCommentText" resourcekey="rfvCommentText" runat="server" Display="None" ControlToValidate="txtComment" ValidationGroup="commentVal" />
                                
                            </div>
                            <br />
                            <asp:Panel ID="pnlNameComment" runat="server">
                                <div id="commentFirstName">
                                    <asp:Label ID="lblFirstNameComment" runat="server" />
                                    <asp:TextBox ID="txtFirstNameComment" runat="server" EnableViewState="false" CssClass="commentFirstNameTextbox" /><asp:RequiredFieldValidator ID="rfvFirstNameComment" resourcekey="rfvFirstNameComment" runat="server" Display="None" ControlToValidate="txtFirstNameComment" ValidationGroup="commentVal" />
                                    
                                </div>
                                <div id="commentLastName">
                                    <asp:Label ID="lblLastNameComment" runat="server" />
                                    <asp:TextBox ID="txtLastNameComment" runat="server" EnableViewState="false" CssClass="commentLastNameTextbox" /><asp:RequiredFieldValidator ID="rfvLastNameComment" resourcekey="rfvLastNameComment" runat="server" Display="None" ControlToValidate="txtLastNameComment" ValidationGroup="commentVal" />
                                    
                                </div>
                                <br />
                            </asp:Panel>
                            <asp:Panel ID="pnlEmailAddressComment" runat="server">
                                <div id="commentEmail">
                                    <asp:Label ID="lblEmailAddressComment" runat="server" resourcekey="lblEmailAddressComment" />
                                    <asp:TextBox ID="txtEmailAddressComment" runat="server" EnableViewState="false" CssClass="commentEmailTextbox" /><asp:RequiredFieldValidator ID="rfvEmailAddressComment" resourcekey="rfvEmailAddressComment" runat="server" Display="None" ControlToValidate="txtEmailAddressComment" ValidationGroup="commentVal" />
                                </div>
                                <br />
                            </asp:Panel>
                            <asp:Panel ID="pnlUrlComment" runat="server">
                                <div id="commentUrl">
                                    <asp:Label ID="lblUrlComment" runat="server" resourcekey="lblUrlComment" />
                                    <asp:TextBox ID="txtUrlComment" runat="server" EnableViewState="false" CssClass="commentUrlTextbox" /></div>
                                <br />
                            </asp:Panel>
                            <div id="commentSubmit">
                                <asp:LinkButton ID="btnSubmitComment" runat="server" resourcekey="btnSubmitComment" OnClick="btnSubmitComment_Click" CssClass="commentSubmitButton" ValidationGroup="commentVal"></asp:LinkButton></div>
                            <div id="commentCancel">
                                <asp:LinkButton ID="btnCancelComment" runat="server" resourcekey="btnCancelComment" OnClick="btnCancelComment_Click" CssClass="commentCancelButton" CausesValidation="false" ValidationGroup="commentVal"></asp:LinkButton></div>
                            <asp:ValidationSummary ID="valCommentSummary" runat="server" ShowMessageBox="true" ShowSummary="false" DisplayMode="List" ValidationGroup="commentVal" />
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlCommentConfirmation" runat="server" Visible="false">
                        <div id="commentConfirmationWrapper">
                            <div id="commentConfirmationClose">
                                <asp:LinkButton ID="btnConfirmationClose" CausesValidation="false" runat="server" resourcekey="btnConfirmationClose" OnClick="btnConfirmationClose_Click" CssClass="commentConfirmCloseButton"></asp:LinkButton></div>
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
    function changeCssClassMethod(eventElement) {
       Sys.UI.DomElement.removeCssClass($get('divRating'), 'divRatingBefore'); 
       Sys.UI.DomElement.addCssClass($get('divRating'), 'divRatingAfter'); 

       //Sys.UI.DomElement.toggleCssClass($get('divRating'), "divRatingAfter");
    }
    // Add handler using the getElementById method
    $addHandler(Sys.UI.DomElement.getElementById('<%= ajaxRating.ClientID %>'), 'click', changeCssClassMethod);
    <% } %>

    var Engage_ThumbnailHashtable = {};
    var Engage_HideTimeoutId = null;

    function ShowImage(behaviorId, elementId)
    {
//        clearTimeout(Engage_HideTimeoutId); //if the image hasn't been hidden yet, clear the call to hide it.  BD
//        var behaviorId = Engage_ThumbnailHashtable[eventElement.target.id || eventElement.target.parentNode.id][0];
//        var elementId = Engage_ThumbnailHashtable[eventElement.target.id || eventElement.target.parentNode.id][1];
        var pnlLargeImage = $get(elementId);
        if (pnlLargeImage)
        {
            pnlLargeImage.style.display = 'block';
        }
        
        var pceArticleThumbnail = $find(behaviorId);
        if (pceArticleThumbnail)
        {
            pceArticleThumbnail.showPopup();
        }
    }
    
    function HideImage(behaviorId, elementId, smallImageId)
    {
//        var elementId = Engage_ThumbnailHashtable[eventElement.target.id || eventElement.target.parentNode.id][1];
//        var smallImageId = Engage_ThumbnailHashtable[eventElement.target.id || eventElement.target.parentNode.id][2];
        
//        if (!mouseIsInElement(eventElement, eventElement.target.id || eventElement.target.parentNode.id))
//        {
//            var hideImage = function()
//            {
//                var behaviorId = Engage_ThumbnailHashtable[eventElement.target.id || eventElement.target.parentNode.id][0];
                var pceArticleThumbnail = $find(behaviorId);
                if (pceArticleThumbnail)
                {
                    pceArticleThumbnail.hidePopup();
                    var pnlLargeImage = $get(elementId);
                    if (pnlLargeImage)
                    {
                        var hideImage = function()
                        {
                            pnlLargeImage.style.visibility = 'hidden';
                            pnlLargeImage.style.display = 'none';
                        }
                        setTimeout(hideImage, 250); 
                    }
//                }
//            }
//            Engage_HideTimeoutId = setTimeout(hideImage, 100);
        }
    }
    
    function mouseIsInElement(eventElement, elementId)
    {
        var element = $get(elementId);
        var bounds = Sys.UI.DomElement.getBounds(element);
        
        return eventElement.offsetX >= 0 && eventElement.offsetX <= bounds.width && eventElement.offsetY >= 0 && eventElement.offsetY <= bounds.height;
    }
</script>
