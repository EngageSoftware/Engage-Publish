<%@ Control Language="c#" AutoEventWireup="False" Inherits="Engage.Dnn.Publish.ArticleControls.ArticleDisplayOptions" Codebehind="ArticleDisplayOptions.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>


<style type="text/css">
    @import url(<%=Engage.Dnn.Publish.ModuleBase.ApplicationUrl %><%=Engage.Dnn.Publish.ModuleBase.DesktopModuleFolderName %>Module.css);
</style>

<asp:UpdatePanel ID="upnlArticleDisplayOptions" runat="server" UpdateMode="Conditional"><ContentTemplate>
</ContentTemplate></asp:UpdatePanel>
    
    <asp:PlaceHolder ID="phArticleSelection" runat="server" />
    <table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
	    <tr>
	        <td class="subhead"><dnn:label ID="lblLastUpdatedFormat" resourcekey="lblLastUpdatedFormat" runat="server" /></td>
		    <td><asp:TextBox ID="txtLastUpdatedFormat" runat="server" CssClass="NormalTextBox"></asp:TextBox></td>
	    </tr>
    </table>
    <hr />
    <asp:Label ID="lblEnablePhotoGallery" runat="server" CssClass="NormalRed" resourcekey="lblEnablePhotoGallery" />
    <asp:Panel ID="pnlPhotoGallerySettings" runat="server">
        <table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
            <tr>
                <td class="subhead"><dnn:label ID="lblDisplayPhotoGallery" runat="server" /></td>
                <td><asp:Checkbox ID="chkDisplayPhotoGallery" runat="server" CssClass="NormalTextBox" /></td><%-- AutoPostBack="true" CausesValidation="false" OnCheckedChanged="chkDisplayPhotoGallery_CheckedChanged"--%>
            </tr>
            <tr>
                <td class="subhead"><dnn:label ID="lblPhotoGalleryMaxCount" runat="server" /></td>
                <td>
                    <%--<asp:Checkbox ID="chkPhotoGalleryShowAll" runat="server" CssClass="NormalTextBox" AutoPostBack="true" CausesValidation="false" OnCheckedChanged="chkPhotoGalleryShowAll_CheckedChanged" resourcekey="ShowAll" />--%>
                    <asp:Textbox ID="txtPhotoGalleryMaxCount" runat="server" CssClass="NormalTextBox" />
                </td>
            </tr>
            <tr>
                <td class="subhead"><dnn:label ID="lblPhotoGalleryThumbnailHeight" runat="server" /></td>
                <td>
                    <asp:Textbox ID="txtPhotoGalleryThumbnailHeight" runat="server" CssClass="NormalTextBox" />
                </td>
            </tr><tr>
                <td class="subhead"><dnn:label ID="lblPhotoGalleryThumbnailWidth" runat="server" /></td>
                <td>
                    <asp:Textbox ID="txtPhotoGalleryThumbnailWidth" runat="server" CssClass="NormalTextBox" />
                </td>
            </tr>
            <tr>
                <td class="subhead"><dnn:label ID="lblPhotoGalleryHoverThumbnailHeight" runat="server" /></td>
                <td>
                    <asp:Textbox ID="txtPhotoGalleryHoverThumbnailHeight" runat="server" CssClass="NormalTextBox" />
                </td>
            </tr><tr>
                <td class="subhead"><dnn:label ID="lblPhotoGalleryHoverThumbnailWidth" runat="server" /></td>
                <td>
                    <asp:Textbox ID="txtPhotoGalleryHoverThumbnailWidth" runat="server" CssClass="NormalTextBox" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:CompareValidator ID="cvPhotoGalleryHoverThumbnailHeight" runat="server" ControlToValidate="txtPhotoGalleryHoverThumbnailHeight" Display="Dynamic" Operator="DataTypeCheck" Type="Integer" CssClass="NormalRed" resourcekey="cvPhotoGalleryHoverThumbnailHeight" />
                    <asp:CompareValidator ID="cvPhotoGalleryHoverThumbnailWidth" runat="server" ControlToValidate="txtPhotoGalleryHoverThumbnailWidth" Display="Dynamic" Operator="DataTypeCheck" Type="Integer" CssClass="NormalRed" resourcekey="cvPhotoGalleryHoverThumbnailWidth" />
                    <asp:CompareValidator ID="cvPhotoGalleryThumbnailHeight" runat="server" ControlToValidate="txtPhotoGalleryThumbnailHeight" Display="Dynamic" Operator="DataTypeCheck" Type="Integer" CssClass="NormalRed" resourcekey="cvPhotoGalleryThumbnailHeight" />
                    <asp:CompareValidator ID="cvPhotoGalleryThumbnailWidth" runat="server" ControlToValidate="txtPhotoGalleryThumbnailWidth" Display="Dynamic" Operator="DataTypeCheck" Type="Integer" CssClass="NormalRed" resourcekey="cvPhotoGalleryThumbnailWidth" />
                    <%--<asp:CompareValidator ID="cvPhotoGalleryMaxCount" runat="server" Display="Dynamic" ControlToValidate="txtPhotoGalleryMaxCount" Operator="GreaterThanEqual" ValueToCompare="1" Type="Integer" CssClass="NormalRed" resourcekey="cvPhotoGalleryMaxCount" />
                    <asp:RequiredFieldValidator ID="rfvPhotoGalleryMaxCount" runat="server" Display="Dynamic" ControlToValidate="txtPhotoGalleryMaxCount" CssClass="NormalRed" resourcekey="rfvPhotoGalleryMaxCount" />--%>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <hr />
    <asp:Label ID="lblEnableRatings" runat="server" CssClass="NormalRed" resourcekey="lblEnableRatings" />
    <asp:Panel ID="pnlRatingsSettings" runat="server">
        <table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
            <tr>
                <td class="subhead"><dnn:label ID="lblDisplayRatings" runat="server" /></td>
                <td><asp:DropDownList ID="ddlDisplayRatings" runat="server" CssClass="NormalTextBox" /></td>
            </tr>
        </table>
    </asp:Panel>
    <hr />
    <asp:Label ID="lblEnableComments" runat="server" CssClass="NormalRed" resourcekey="lblEnableComments"/>
    <asp:Panel ID="pnlCommentSettings" runat="server">
        <table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
            <tr>
                <td class="subhead"><dnn:label ID="lblCommentSubmit" runat="server" /></td>
		        <td><asp:CheckBox ID="chkCommentSubmit" runat="server" CssClass="NormalTextBox" /></td>
	        </tr>
	        <tr>
	            <td class="subhead"><dnn:label ID="lblFirstNameCollect" runat="server" /></td>
	            <td><asp:DropDownList ID="ddlFirstNameCollect" runat="server" CssClass="NormalTextBox" /></td>
	        </tr>
	        <tr>
	            <td class="subhead"><dnn:label ID="lblLastNameCollect" runat="server" /></td>
	            <td><asp:DropDownList ID="ddlLastNameCollect" runat="server" CssClass="NormalTextBox" /></td>
	        </tr>
	        <tr>
	            <td class="subhead"><dnn:label ID="lblEmailAddressCollect" runat="server" /></td>
	            <td><asp:CheckBox ID="chkEmailAddressCollect" runat="server" CssClass="NormalTextBox" /></td>
	        </tr>
	        <tr>
	            <td class="subhead"><dnn:label ID="lblUrlCollect" runat="server" /></td>
	            <td><asp:CheckBox ID="chkUrlCollect" runat="server" CssClass="NormalTextBox" /></td>
	        </tr>
	        <tr>
                <td class="subhead"><dnn:label ID="lblCommentDisplay" runat="server" /></td>
		        <td><asp:CheckBox ID="chkCommentDisplay" runat="server" CssClass="NormalTextBox" /></td>
	        </tr>
	        <tr>
                <td class="subhead"><dnn:label ID="lblDisplayComments" runat="server" /></td>
		        <td><asp:DropDownList ID="ddlDisplayComments" runat="server" CssClass="NormalTextBox" /></td>
	        </tr>
    	    
        <%--	<tr>
	            <td class="subhead"><dnn:label ID="lblCommentPaging" runat="server" /></td>
	            <td>
	                <asp:TextBox ID="txtCommentPaging" runat="server" CssClass="NormalTextBox" />
	                <asp:CustomValidator ID="cvCommentPaging" runat="server" ControlToValidate="txtCommentPaging" Display="Dynamic" resourceKey="cvCommentPaging" ValidateEmptyText="true" OnServerValidate="cvCommentPaging_ServerValidate" />
	            </td>
	        </tr>
	        <tr>
	            <td class="subhead"><dnn:label ID="lblCommentRandom" runat="server" /></td>
	            <td><asp:CheckBox ID="chkCommentRandom" runat="server" AutoPostBack="true" OnCheckedChanged="chkCommentRandom_CheckedChanged" CssClass="NormalTextBox" /></td>
	        </tr>
        --%>
        </table>
    </asp:Panel>
