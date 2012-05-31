<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.EmailAFriend" Codebehind="EmailAFriend.ascx.cs" %>

<div id="divEmailAFriend">
    <asp:HyperLink ID="EmailAFriendPopupTriggerLink" Runat="server" ResourceKey="btnEmailAFriend" cssclass="btnEmailAFriend engagePublishModalLink" NavigateUrl="#" CausesValidation="false" />	
</div>
<br />
<br />
<asp:Panel ID="pnlEmailAFriend" runat="server" CssClass="commentPopup" style="display:none;">
    <div id="divEmailAFriendForm" class="divEmailAFriendForm" runat="server">
        <asp:Label ID="lblTo" ResourceKey="lblTo" Runat="server" />
        <br />
        <asp:TextBox id="txtTo" Runat="server" />
        <asp:RequiredFieldValidator ID="fvRecipients" runat="server" ControlToValidate="txtTo" ErrorMessage="Please enter one or more recipients" ValidationGroup="ValidateGroupSend" />
        <br />
        <asp:Label ID="lblMultiple" runat="server" ResourceKey="lblMultiple" />
        <br /><br />
        <asp:Label ID="lblFrom" ResourceKey="lblFrom" Runat="server" />
        <br />
        <asp:TextBox id="txtFrom" Runat="server" />
        <asp:RequiredFieldValidator ID="fvSender" runat="server" ControlToValidate="txtFrom" ErrorMessage="Please enter your email address." ValidationGroup="ValidateGroupSend" />
        <br />
        <asp:Label ID="lblMessage" ResourceKey="lblMessage" Runat="server" />
        <br />
        <asp:TextBox Runat="server" ID="txtMessage" TextMode="MultiLine" Columns="30" Rows="5" />
	    <br />
        <asp:Label ID="lblPrivacy" runat="server" ResourceKey="lblPrivacy" />
	    <br />
	    <br />
        <asp:LinkButton id="btnSend" ResourceKey="btnSend" Runat="server" ValidationGroup="ValidateGroupSend" OnClick="btnSend_Click" />&nbsp;
        <asp:LinkButton id="btnCancel" ResourceKey="btnCancel" Runat="server" OnClick="btnCancel_Click1" CssClass="simplemodal-close" CausesValidation="false" />
    </div>
</asp:Panel>