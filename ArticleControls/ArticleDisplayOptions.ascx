<%@ Control Language="c#" AutoEventWireup="False" Inherits="Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions" Codebehind="ArticleDisplayOptions.ascx.cs" %>
<%@ Import Namespace="Engage.Dnn.Publish" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="engage" TagName="ArticleSelector" Src="../controls/ArticleSelector.ascx" %>
<style type="text/css">
    @import url(<%=ModuleBase.ApplicationUrl%><%=ModuleBase.DesktopModuleFolderName%>Module.css);
</style>

<Engage:ArticleSelector ID="ArticleSelectorControl" runat="server" />
<table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
    <tr>
        <td class="subhead"><dnn:Label ID="lblLastUpdatedFormat" resourcekey="lblLastUpdatedFormat" runat="server" /></td>
	    <td><asp:TextBox ID="txtLastUpdatedFormat" runat="server" CssClass="NormalTextBox"/></td>
    </tr>
</table>
<hr />
<asp:Label ID="lblEnablePhotoGallery" runat="server" CssClass="NormalRed" resourcekey="lblEnablePhotoGallery" />
<asp:Panel ID="pnlPhotoGallerySettings" runat="server">
    <table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
        <tr>
            <td class="subhead"><dnn:Label ID="lblDisplayPhotoGallery" runat="server" /></td>
            <td><asp:Checkbox ID="chkDisplayPhotoGallery" runat="server" CssClass="NormalTextBox" /></td>
        </tr>
        <tr>
            <td class="subhead"><dnn:Label ID="lblPhotoGalleryMaxCount" runat="server" /></td>
            <td><asp:Textbox ID="txtPhotoGalleryMaxCount" runat="server" CssClass="NormalTextBox" /></td>
        </tr>
        <tr>
            <td class="subhead"><dnn:Label ID="lblPhotoGalleryThumbnailHeight" runat="server" /></td>
            <td><asp:Textbox ID="txtPhotoGalleryThumbnailHeight" runat="server" CssClass="NormalTextBox" /></td>
        </tr><tr>
            <td class="subhead"><dnn:Label ID="lblPhotoGalleryThumbnailWidth" runat="server" /></td>
            <td><asp:Textbox ID="txtPhotoGalleryThumbnailWidth" runat="server" CssClass="NormalTextBox" /></td>
        </tr>
        <tr>
            <td class="subhead"><dnn:Label ID="lblPhotoGalleryHoverThumbnailHeight" runat="server" /></td>
            <td><asp:Textbox ID="txtPhotoGalleryHoverThumbnailHeight" runat="server" CssClass="NormalTextBox" /></td>
        </tr><tr>
            <td class="subhead"><dnn:Label ID="lblPhotoGalleryHoverThumbnailWidth" runat="server" /></td>
            <td><asp:Textbox ID="txtPhotoGalleryHoverThumbnailWidth" runat="server" CssClass="NormalTextBox" /></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:CompareValidator ID="cvPhotoGalleryHoverThumbnailHeight" runat="server" ControlToValidate="txtPhotoGalleryHoverThumbnailHeight" Display="Dynamic" Operator="DataTypeCheck" Type="Integer" CssClass="NormalRed" resourcekey="cvPhotoGalleryHoverThumbnailHeight" />
                <asp:CompareValidator ID="cvPhotoGalleryHoverThumbnailWidth" runat="server" ControlToValidate="txtPhotoGalleryHoverThumbnailWidth" Display="Dynamic" Operator="DataTypeCheck" Type="Integer" CssClass="NormalRed" resourcekey="cvPhotoGalleryHoverThumbnailWidth" />
                <asp:CompareValidator ID="cvPhotoGalleryThumbnailHeight" runat="server" ControlToValidate="txtPhotoGalleryThumbnailHeight" Display="Dynamic" Operator="DataTypeCheck" Type="Integer" CssClass="NormalRed" resourcekey="cvPhotoGalleryThumbnailHeight" />
                <asp:CompareValidator ID="cvPhotoGalleryThumbnailWidth" runat="server" ControlToValidate="txtPhotoGalleryThumbnailWidth" Display="Dynamic" Operator="DataTypeCheck" Type="Integer" CssClass="NormalRed" resourcekey="cvPhotoGalleryThumbnailWidth" />
            </td>
        </tr>
    </table>
</asp:Panel>
<hr />
<asp:Label ID="lblEnableRatings" runat="server" CssClass="NormalRed" resourcekey="lblEnableRatings" />
<asp:Panel ID="pnlRatingsSettings" runat="server">
    <table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
        <tr>
            <td class="subhead"><dnn:Label ID="lblDisplayRatings" runat="server" /></td>
            <td><asp:DropDownList ID="ddlDisplayRatings" runat="server" CssClass="NormalTextBox" /></td>
        </tr>
    </table>
</asp:Panel>
<hr />
<asp:Label ID="lblEnableComments" runat="server" CssClass="NormalRed" resourcekey="lblEnableComments"/>
<asp:Panel ID="pnlCommentSettings" runat="server">
    <table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
        <tr>
            <td class="subhead"><dnn:Label ID="lblCommentSubmit" runat="server" /></td>
	        <td><asp:CheckBox ID="chkCommentSubmit" runat="server" CssClass="NormalTextBox" /></td>
        </tr>
        <tr>
            <td class="subhead"><dnn:Label ID="lblFirstNameCollect" runat="server" /></td>
            <td><asp:DropDownList ID="ddlFirstNameCollect" runat="server" CssClass="NormalTextBox" /></td>
        </tr>
        <tr>
            <td class="subhead"><dnn:Label ID="lblLastNameCollect" runat="server" /></td>
            <td><asp:DropDownList ID="ddlLastNameCollect" runat="server" CssClass="NormalTextBox" /></td>
        </tr>
        <tr>
            <td class="subhead"><dnn:Label ID="lblEmailAddressCollect" runat="server" /></td>
            <td><asp:CheckBox ID="chkEmailAddressCollect" runat="server" CssClass="NormalTextBox" /></td>
        </tr>
        <tr>
            <td class="subhead"><dnn:Label ID="lblUrlCollect" runat="server" /></td>
            <td><asp:CheckBox ID="chkUrlCollect" runat="server" CssClass="NormalTextBox" /></td>
        </tr>
        <tr>
            <td class="subhead"><dnn:Label ID="lblCommentDisplay" runat="server" /></td>
	        <td><asp:CheckBox ID="chkCommentDisplay" runat="server" CssClass="NormalTextBox" /></td>
        </tr>
        <tr>
            <td class="subhead"><dnn:Label ID="lblDisplayComments" runat="server" /></td>
	        <td><asp:DropDownList ID="ddlDisplayComments" runat="server" CssClass="NormalTextBox" /></td>
        </tr>
        
    </table>
</asp:Panel>
