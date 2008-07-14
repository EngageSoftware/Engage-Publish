<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.ArticleControls.ArticleEdit" CodeBehind="ArticleEdit.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="sectionhead" Src="~/controls/sectionheadcontrol.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<div id="ArticleEdit" class="Normal">
    <dnn:sectionhead ID="shPublishInstructions" CssClass="Head" runat="server" Text="Basic Options" Section="publishInstructions" ResourceKey="shPublishInstructions" IsExpanded="False" />
    <hr />
    <div id="publishInstructions" runat="server" class="instructions">
        <asp:Label ID="lblPublishInstructions" runat="server" resourcekey="lblPublishInstructions" CssClass="Normal"></asp:Label>
    </div>
    <br />
    <dnn:sectionhead ID="shArticleEdit" CssClass="Head" runat="server" Text="Basic Options" Section="tblArticleEdit" ResourceKey="shArticleEdit" IsExpanded="True" />
    <hr />
    <div id="tblArticleEdit" runat="server">
        <table class="PublishEditTable Normal">
            <tr id="trArticleId" runat="server">
                <td class="editTableLabelColumn nowrap">
                    <dnn:label ID="lblArticleId" ResourceKey="lblArticleId" runat="server" />
                </td>
                <td class="fullWidth">
                    <asp:Label ID="txtArticleId" runat="server" />
                </td>
            </tr>
        </table>
        
        <asp:PlaceHolder ID="phControls" runat="Server" />
        <table class="PublishEditTable Normal">
            <tr>
                <td class="editTableLabelColumn nowrap">
                    <dnn:label ID="lblArticleText" ResourceKey="ArticleText" runat="server" class="title" />
                </td>
                <td class="fullWidth">
                    <asp:PlaceHolder ID="phArticleText" runat="server" />
                </td>
            </tr>
        </table>
        <table class="PublishEditTable Normal">
            <tr>
                <td class="editTableLabelColumn nowrap">
                    <dnn:label ID="lblParentCategory" runat="server" ResourceKey="ParentCategory" />
                </td>
                <td class="fullWidth">
                    <asp:UpdatePanel ID="upnlParentCategory" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:PlaceHolder ID="phParentCategory" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
<ajaxToolkit:CollapsiblePanelExtender
     TargetControlID="pnlArticleEditExtended" 
     ExpandControlID="TitlePanel" 
     CollapseControlID="TitlePanel" 
     Collapsed="true" 
     ImageControlID="imgArticleEditExtendedHeader"
     TextLabelID="lblArticleEditExtendedHeader"
     ID="clpExtended" 
     runat="server" 
     SuppressPostBack="true"
     
     />
    <asp:Panel ID="TitlePanel" runat="server" CssClass="collapsePanelHeader"> 
           <table class="PublishEditTable Normal">
        <tr>
            <td class="editTableLabelColumn nowrap">
                <asp:Label ID="lblArticleEditExtendedHeader" CssClass="SubHead" resourcekey="lblArticleEditExtendedHeader" runat="server" />
            </td><td class="fullWidth">
                <asp:image id="imgArticleEditExtendedHeader" runat="server" />
                &nbsp;
            </td>
        </tr>
    </table>
           
           
    </asp:Panel>
    
<asp:Panel ID="pnlArticleEditExtended" runat="server">

        <table class="PublishEditTable Normal">
            <tr>
                <td class="editTableLabelColumn nowrap">
                    <dnn:label ID="lblRelatedCategories" runat="server" ResourceKey="RelatedCategories" />
                </td>
                <td class="fullWidth">
                    <asp:UpdatePanel ID="upnlRelatedCategories" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:PlaceHolder ID="phRelatedCategories" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table class="PublishEditTable Normal">
            <tr>
                <td class="editTableLabelColumn nowrap">
                    <asp:Label ID="lblIncludeRelatedArticles" runat="server" ResourceKey="lblIncludeRelatedArticles" />
                </td>
                <td class="fullWidth">
                    <asp:CheckBox ID="chkIncludeRelatedArticles" runat="server" AutoPostBack="true" OnCheckedChanged="chkIncludeRelatedArticles_CheckedChanged" />
                    <hr />
                    <asp:UpdatePanel ID="upnlRelatedArticles" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="chkIncludeRelatedArticles" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:PlaceHolder ID="phRelatedArticles" runat="server" Visible="false">
                                <dnn:label ID="lblRelatedArticles" runat="server" ResourceKey="RelatedArticles" />
                                <asp:CheckBox ID="chkIncludeOtherArticlesFromSameList" runat="server" Text="Automatically include other articles from the same Article List(s), and/or" ResourceKey="chkIncludeOtherArticlesFromSameList" /><br />
                            </asp:PlaceHolder>
                            <asp:PlaceHolder ID="phEmbeddedArticle" runat="server" Visible="false">
                                <dnn:label ID="lblEmbeddedArticle" runat="server" ResourceKey="EmbeddedArticle" />
                            </asp:PlaceHolder>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table class="PublishEditTable Normal">
            <tr id="rowPhotoGallery" runat="server" visible="false">
                <td class="editTableLabelColumn nowrap">
                    <dnn:label ID="lblPhotoGalleryAlbum" runat="server" class="title" ControlName="ddlPhotoGalleryAlbum" />
                </td>
                <td class="fullWidth">
                    <asp:DropDownList ID="ddlPhotoGalleryAlbum" runat="server" CssClass="NormalTextBox" />
                    <hr />
                </td>
            </tr>
        </table>
        <table class="PublishEditTable Normal">
            <tr>
                <td class="editTableLabelColumn nowrap">
                    <dnn:label ID="lblDisplayOptions" ResourceKey="lblDisplayOptions" runat="server" class="title" />
                </td>
                <td class="fullWidth">
                    <asp:CheckBox ID="chkEmailAFriend" runat="server" ResourceKey="chkEmailAFriend" />
                    <asp:CheckBox ID="chkPrinterFriendly" runat="server" ResourceKey="chkPrinterFriendly" />
                    <asp:CheckBox ID="chkRatings" runat="server" ResourceKey="chkRatings" />
                    <asp:CheckBox ID="chkComments" runat="server" ResourceKey="chkComments" />
                    <asp:CheckBox ID="chkForumComments" runat="server" ResourceKey="chkForumComments" />
                    <asp:CheckBox ID="chkReturnList" runat="server" ResourceKey="chkReturnList" />
                    <asp:CheckBox ID="chkShowAuthor" runat="server" ResourceKey="chkShowAuthor" />
                    <asp:CheckBox ID="chkTags" runat="server" ResourceKey="chkTags" />
                    <hr />
                </td>
            </tr>
        </table>
        <table class="PublishEditTable Normal">
            <tr>
                <td class="editTableLabelColumn nowrap">
                    <dnn:label ID="lblVersionNumber" ResourceKey="lblVersionNumber" runat="server" class="title" />
                </td>
                <td class="fullWidth">
                    <asp:TextBox ID="txtVersionNumber" runat="server" TextMode="SingleLine" />
                </td>
            </tr>
        </table>
        <table class="PublishEditTable Normal">
            <tr>
                <td class="editTableLabelColumn nowrap">
                    <dnn:label ID="lblPreviousVersionDescription" ResourceKey="lblPreviousVersionDescription" runat="server" class="title" />
                </td>
                <td class="fullWidth">
                    <asp:TextBox ID="txtPreviousVersionDescription" runat="server" TextMode="MultiLine" Columns="50" Rows="3" ReadOnly="true" />
                </td>
            </tr>
        </table>
        <table class="PublishEditTable Normal">
            <tr>
                <td class="editTableLabelColumn nowrap">
                    <dnn:label ID="lblVersionDescription" ResourceKey="lblVersionDescription" runat="server" class="title" />
                </td>
                <td class="fullWidth">
                    <asp:TextBox ID="txtVersionDescription" runat="server" TextMode="MultiLine" Columns="50" Rows="3" />
                    <hr />
                </td>
            </tr>
        </table>
</asp:Panel>
        <table class="PublishEditTable Normal">
            <tr>
                <td class="editTableLabelColumn nowrap">
                    <dnn:label ID="lblDisplayOnCurrentPage" ResourceKey="lblDisplayOnCurrentPage" runat="server" class="title" />
                </td>
                <td class="fullWidth">
                    <asp:UpdatePanel ID="upnlDisplayLocationOptions" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:RadioButtonList ID="rblDisplayOnCurrentPage" runat="server" CssClass="Normal" Style="display: inline" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblDisplayOnCurrentPage_SelectedIndexChanged" />
                            <asp:DropDownList ID="ddlDisplayTabId" BorderWidth="0" DataValueField="TabID" DataTextField="TabName" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table class="PublishEditTable Normal">
            <tr>
                <td class="editTableLabelColumn nowrap">
                    <asp:UpdatePanel ID="upnlForceDisplayTabLabel" runat="server" RenderMode="Inline" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="rblDisplayOnCurrentPage" />
                        </Triggers>
                        <ContentTemplate>
                            <dnn:label ID="lblForceDisplayTab" runat="server" class="title" Visible="false" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td class="fullWidth">
                    <asp:UpdatePanel ID="upnlForceDisplayTab" runat="server" RenderMode="Inline" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="rblDisplayOnCurrentPage" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:CheckBox ID="chkForceDisplayTab" runat="server" AutoPostBack="true" Visible="false" /><%--OnCheckedChanged="chkForceDisplayTab_CheckedChanged"--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <hr />
                </td>
            </tr>
        </table>
        <table class="PublishEditTable Normal">
            <tr>
                <td class="editTableLabelColumn nowrap">
                    <dnn:label ID="lblApproval" runat="server" ResourceKey="ApprovalStatus" />
                </td>
                <td class="fullWidth">
                    <asp:UpdatePanel ID="upnlApproval" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:CheckBox ID="chkUseApprovals" runat="server" Text="Use Approvals" ResourceKey="chkUseApprovals" AutoPostBack="true" OnCheckedChanged="chkUseApprovals_CheckedChanged" Visible="false" Checked="true" />
                            <asp:PlaceHolder ID="phApproval" runat="Server" />
                            <asp:Label ID="lblNotUsingApprovals" runat="server" Text="Approvals are turned off.  Any changes you make will appear immediately on your website." ResourceKey="lblNotUsingApprovals" Visible="false" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
        <table class="PublishEditTable Normal">
            <tr runat="server" id="rowTagEntry">
                <td class="editTableLabelColumn nowrap">
                    <dnn:label ID="lblTagEntry" runat="server" ResourceKey="TagEntry" />
                </td>
                <td class="fullWidth">
                    <asp:PlaceHolder ID="phTagEntry" runat="server"></asp:PlaceHolder>
                </td>
            </tr>
        </table>
    </div>
    <asp:UpdateProgress ID="upPublishRelationshipsProgress" runat="server">
        <ProgressTemplate>
            <div class="progressWrap">
                <div class="progressUpdateMessage">
                    <asp:Label ID="lblProgressUpdate" runat="server" resourcekey="lblProgressUpdate" />
                    <img src="<%=ApplicationUrl%><%=DesktopModuleFolderName %>images/progressbar_green.gif" alt="Updating" id="imgProgressUpdate" />
                </div>
            </div>
            <div class="progressUpdate">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:TextBox ID="txtMessage" runat="server" Visible="False" ReadOnly="True" ForeColor="Red" EnableViewState="False" Columns="75" TextMode="MultiLine" Rows="5" /><br />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
    <br />
    <asp:LinkButton ID="cmdUpdate" runat="server" ResourceKey="cmdUpdate"></asp:LinkButton>&nbsp;&nbsp;
    <asp:LinkButton ID="cmdCancel" runat="server" ResourceKey="cmdCancel" CausesValidation="False"></asp:LinkButton>&nbsp;&nbsp;
    <asp:LinkButton ID="cmdDelete" runat="server" resourceKey="cmdDelete" CausesValidation="False" Text="Delete" OnClick="cmdDelete_Click"></asp:LinkButton>
</div>
