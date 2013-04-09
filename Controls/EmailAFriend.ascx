<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.EmailAFriend" Codebehind="EmailAFriend.ascx.cs" %>

<div id="divEmailAFriend">
    <asp:HyperLink ID="EmailAFriendPopupTriggerLink" runat="server" ResourceKey="btnEmailAFriend" CssClass="btnEmailAFriend engagePublishModalLink" NavigateUrl="#" CausesValidation="false" />	
</div>
<br />
<br />
<asp:Panel ID="pnlEmailAFriend" runat="server" CssClass="commentPopup dnnForm" style="display:none;">
    <div id="divEmailAFriendForm" class="divEmailAFriendForm" runat="server">
        <div class="dnnFormItem">
            <asp:Label ID="lblTo" runat="server" ResourceKey="lblTo" AssociatedControlID="txtTo" />
            <asp:TextBox ID="txtTo" runat="server" type="email" />
            <asp:RequiredFieldValidator ID="ToRequiredValidator" runat="server" ControlToValidate="txtTo" ResourceKey="Please enter one or more recipients" ValidationGroup="ValidateGroupSend" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" />
        </div>
        <asp:Label ID="lblMultiple" runat="server" ResourceKey="lblMultiple" CssClass="dnnFormItem dnnFormHelp" />
        <div class="dnnFormItem">
            <asp:Label ID="lblFrom" runat="server" ResourceKey="lblFrom" AssociatedControlID="txtFrom" />
            <asp:TextBox ID="txtFrom" runat="server" type="email" />
            <asp:RequiredFieldValidator ID="FromRequiredValidator" runat="server" ControlToValidate="txtFrom" ResourceKey="Please enter your email address" ValidationGroup="ValidateGroupSend" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" />
        </div>
        <asp:Label ID="lblPrivacy" runat="server" ResourceKey="lblPrivacy" CssClass="dnnFormItem dnnFormHelp" />
        <div class="dnnFormItem">
            <asp:Label ID="lblMessage" runat="server" ResourceKey="lblMessage" AssociatedControlID="txtMessage" />
            <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Columns="30" Rows="5" />            
        </div>

        <ul class="dnnActions dnnClear">
            <li><asp:LinkButton ID="SendButton" runat="server" ResourceKey="btnSend" CssClass="dnnPrimaryAction" ValidationGroup="ValidateGroupSend" OnClick="btnSend_Click" /></li>
            <li><asp:LinkButton ResourceKey="btnCancel" runat="server" CssClass="dnnSecondaryAction simplemodal-close" CausesValidation="false" OnClick="btnCancel_Click1" /></li>
        </ul>
    </div>
</asp:Panel>