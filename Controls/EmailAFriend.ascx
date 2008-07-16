<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.EmailAFriend" Codebehind="EmailAFriend.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>

<div id="divEmailAFriend">
	<asp:LinkButton ID="btnEmailAFriend" Runat="server" ResourceKey="btnEmailAFriend" cssclass="btnEmailAFriend" CausesValidation="false"></asp:LinkButton>	
</div>
<br />
<br />
    <ajaxToolkit:ModalPopupExtender ID="mpeEmailAFriend" runat="server" BackgroundCssClass="commentBackground" PopupControlID="pnlEmailAFriend" TargetControlID="btnEmailAFriend" />
        <asp:Panel ID="pnlEmailAFriend" runat="server" CssClass="commentPopup" style="display:none;">
            <div id="divEmailAFriendForm" class="divEmailAFriendForm" runat="server">
	            <asp:Label ID="lblTo" ResourceKey="lblTo" Runat="server"></asp:Label><br />
	            <asp:TextBox id="txtTo" Runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="fvRecipients" runat="server" ControlToValidate="txtTo" ErrorMessage="Please enter one or more recipients" ValidationGroup="ValidateGroupSend"></asp:RequiredFieldValidator><br />
	            <asp:Label ID="lblMultiple" runat="server" ResourceKey="lblMultiple"></asp:Label><br /><br />
	            <asp:Label ID="lblFrom" ResourceKey="lblFrom" Runat="server"></asp:Label><br />
	            <asp:TextBox id="txtFrom" Runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="fvSender" runat="server" ControlToValidate="txtFrom"
                    ErrorMessage="Please enter your email address." ValidationGroup="ValidateGroupSend"></asp:RequiredFieldValidator><br />
	            <asp:Label ID="lblMessage" ResourceKey="lblMessage" Runat="server"></asp:Label><br />
	            <asp:TextBox Runat="server" ID="txtMessage" TextMode="MultiLine" Columns="30" Rows="5"></asp:TextBox>
	            <br />
	            <asp:Label ID="lblPrivacy" runat="server" ResourceKey="lblPrivacy"></asp:Label>
	            <br />
	            <br />
	            <asp:LinkButton id="btnSend" ResourceKey="btnSend" Runat="server" ValidationGroup="ValidateGroupSend" OnClick="btnSend_Click"></asp:LinkButton>&nbsp;
	            <asp:LinkButton id="btnCancel" ResourceKey="btnCancel" Runat="server" OnClick="btnCancel_Click1" CausesValidation="false"></asp:LinkButton>
            </div>
    </asp:Panel>


