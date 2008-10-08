<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.CategoryControls.CategoryEdit" CodeBehind="CategoryEdit.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="sectionhead" Src="~/controls/sectionheadcontrol.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>

<dnn:sectionhead id="shPublishInstructions" cssclass="Head" runat="server" text="Basic Options" section="publishInstructions" resourcekey="shPublishInstructions" isexpanded="False" /><hr />
<div id="publishInstructions" runat="server" class="instructions">
    <asp:Label ID="lblPublishInstructions" runat="server" resourcekey="lblPublishInstructions" CssClass="Normal"></asp:Label>
</div>
<br />
<dnn:sectionhead id="shCategoryEdit" cssclass="Head" runat="server" text="Basic Options" section="CategoryEdit" resourcekey="shCategoryEdit" isexpanded="True" /><hr />
<div id="categoryEdit" class="Normal" runat="server">
    <table class="PublishEditTable Normal">
        <tr id="trCategoryId" runat="server">
            <td class="editTableLabelColumn nowrap">
                <dnn:label id="lblCategoryId" resourcekey="lblCategoryId" Runat="server" />
            </td>
            <td class="fullWidth">
                <asp:Label ID="txtCategoryId" runat="server" />
            </td>
        </tr>
        <tr style="display: none;">
            <td class="editTableLabelColumn nowrap">
                <dnn:label id="lblSortOrder" resourcekey="lblSortOrder" Runat="server" />
            </td>
            <td class="fullWidth">
                <asp:TextBox ID="txtSortOrder" resourcekey="txtSortOrder" runat="server" />
            </td>
        </tr>
    </table>

    <asp:PlaceHolder ID="phItemEdit" runat="Server" />
    
    <table class="PublishEditTable Normal">
        <tr id="trCategoryPermissions" runat="server" visible="false">
            <td class="editTableLabelColumn nowrap">
                <dnn:label ID="lblChooseRoles" runat="server" ResourceKey="ChooseRoles" />
            </td>
            <td class="fullWidth">
                <asp:UpdatePanel ID="upnlCategoryPermissions" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:PlaceHolder ID="phCategoryPermissions" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
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
     TargetControlID="pnlCategoryEditExtended" 
     ExpandControlID="TitlePanel" 
     CollapseControlID="TitlePanel" 
     Collapsed="true" 
     ImageControlID="imgCategoryEditExtendedHeader"
     TextLabelID="lblCategoryEditExtendedHeader"
     ID="clpExtended" 
     runat="server" 
     SuppressPostBack="true"
     
     />
    <asp:Panel ID="TitlePanel" runat="server" CssClass="collapsePanelHeader"> 
           <table class="PublishEditTable Normal">
        <tr>
            <td class="editTableLabelColumn nowrap">
                <asp:Label ID="lblCategoryEditExtendedHeader" CssClass="SubHead" resourcekey="lblCategoryEditExtendedHeader" runat="server" />
            </td><td class="fullWidth">
                <asp:image id="imgCategoryEditExtendedHeader" runat="server" />
                &nbsp;
            </td>
        </tr>
    </table>          
           
    </asp:Panel>
    
<asp:Panel ID="pnlCategoryEditExtended" runat="server">

    <table class="PublishEditTable Normal">
        <tr>
            <td class="editTableLabelColumn nowrap">
                <dnn:label ID="lblFeaturedArticles" runat="server" ResourceKey="ChooseFeatured" />
            </td>
            <td class="fullWidth">
                <asp:UpdatePanel ID="upnlFeaturedArticles" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:PlaceHolder ID="phFeaturedArticles" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <table class="PublishEditTable Normal">
        <tr id="rowCommentForum" runat="server">
            <td class="editTableLabelColumn nowrap">
                <dnn:Label ID="lblCommentForum" Runat="server" class="title" />
            </td>
            <td class="fullWidth">
                <asp:DropDownList ID="ddlCommentForum" runat="server" />
                <hr />
            </td>
        </tr>
    </table>
    
    <table class="PublishEditTable Normal">
        <tr id="Tr1" runat="server">
            <td class="editTableLabelColumn nowrap">
                <dnn:Label ID="lblRssUrl" Runat="server" class="title" />
            </td>
            <td class="fullWidth">
                <asp:TextBox ID="txtRssUrl" runat="server"></asp:TextBox>
                <hr />
            </td>
        </tr>
    </table>

</asp:Panel>
    <table class="PublishEditTable Normal">
        <tr>
            <td class="editTableLabelColumn nowrap">
                <dnn:Label ID="lblDisplayOnCurrentPage" ResourceKey="lblDisplayOnCurrentPage" Runat="server" class="title" />
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
                        <asp:CheckBox ID="chkForceDisplayTab" runat="server" OnCheckedChanged="chkForceDisplayTab_CheckedChanged" AutoPostBack="true" Visible="false" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <hr />
            </td>
        </tr>
    </table>
    <table class="PublishEditTable Normal">
        <tr>
            <td class="editTableLabelColumn nowrap">
                <dnn:Label ID="lblChildDisplayTabId" ResourceKey="lblChildDisplayTabId" Runat="server" class="title" />
            </td>
            <td class="fullWidth">
                <asp:DropDownList ID="ddlChildDisplayTabId" BorderWidth="0" DataValueField='TabID' DataTextField="TabName" runat="server" />
                <asp:CheckBox ID="chkResetChildDisplayTabs" runat="server" ResourceKey="chkResetChildDisplayTabs" />
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
    <asp:UpdateProgress ID="upPublishRelationshipsProgress" runat="server">
        <ProgressTemplate>
            <div class="progressWrap">
                <div class="progressUpdateMessage">
                    <asp:Label ID="lblProgressUpdate" runat="server" resourcekey="lblProgressUpdate"></asp:Label>
                    <img src="<%=ApplicationUrl%><%=DesktopModuleFolderName %>images/progressbar_green.gif" alt="Updating" id="imgProgressUpdate" />
                </div>
            </div>
            <div class="progressUpdate">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:TextBox ID="txtMessage" runat="server" Visible="False" ReadOnly="True" ForeColor="Red" EnableViewState="False" Columns="75" TextMode="MultiLine" Rows="5" />
    <br />
    <asp:LinkButton class="CommandButton" ID="cmdUpdate" resourcekey="cmdUpdate" runat="server" Text="Update" />&nbsp;&nbsp;
    <asp:LinkButton class="CommandButton" ID="cmdCancel" resourcekey="cmdCancel" runat="server" Text="Cancel" CausesValidation="False" />&nbsp;&nbsp;
    <asp:LinkButton class="CommandButton" ID="cmdDelete" resourcekey="cmdDelete" runat="server" Text="Delete" CausesValidation="false" OnClick="cmdDelete_Click" />&nbsp;&nbsp;
</div>
