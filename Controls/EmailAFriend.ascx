<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.EmailAFriend" Codebehind="EmailAFriend.ascx.cs" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>

<div id="divEmailAFriend">
    <asp:HyperLink ID="EmailAFriendPopupTriggerLink" runat="server" ResourceKey="btnEmailAFriend" CssClass="btnEmailAFriend engagePublishModalLink" NavigateUrl="#" CausesValidation="false" />	
</div>
<br />
<br />
<asp:Panel ID="pnlEmailAFriend" runat="server" CssClass="commentPopup dnnForm" style="display:none;">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional"><ContentTemplate>
    <asp:Panel runat="server" CssClass="divEmailAFriendForm" DefaultButton="SendButton">
        <div class="dnnFormItem">
            <asp:Label ID="lblTo" runat="server" ResourceKey="lblTo" AssociatedControlID="txtTo" />
            <asp:TextBox ID="txtTo" runat="server" type="email" />
            <asp:RequiredFieldValidator ID="ToRequiredValidator" runat="server" ControlToValidate="txtTo" ResourceKey="Please enter one or more recipients" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" />
        </div>
        <asp:Label ID="lblMultiple" runat="server" ResourceKey="lblMultiple" CssClass="dnnFormItem dnnFormHelp" />
        <div class="dnnFormItem">
            <asp:Label ID="lblFrom" runat="server" ResourceKey="lblFrom" AssociatedControlID="txtFrom" />
            <asp:TextBox ID="txtFrom" runat="server" type="email" />
            <asp:RequiredFieldValidator ID="FromRequiredValidator" runat="server" ControlToValidate="txtFrom" ResourceKey="Please enter your email address" CssClass="dnnFormMessage dnnFormError" Display="Dynamic" />
        </div>
        <asp:Label ID="lblPrivacy" runat="server" ResourceKey="lblPrivacy" CssClass="dnnFormItem dnnFormHelp" />
        <div class="dnnFormItem">
            <asp:Label ID="lblMessage" runat="server" ResourceKey="lblMessage" AssociatedControlID="txtMessage" />
            <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Columns="30" Rows="5" />            
        </div>
        
        <dnn:DnnCaptcha ID="InvisibleCaptcha" runat="server" ProtectionMode="InvisibleTextBox" Display="None" />
        <dnn:DnnCaptcha ID="TimeoutCaptcha" runat="server" ProtectionMode="MinimumTimeout" Display="None" />
        <dnn:DnnCaptcha ID="StandardCaptcha" runat="server" ProtectionMode="Captcha" Display="None" EnableRefreshImage="True" />
        
        <asp:ValidationSummary ID="ValidationSummary" runat="server" CssClass="dnnFormMessage dnnFormValidationSummary" />
        <ul class="dnnActions dnnClear">
            <li><asp:LinkButton ID="SendButton" runat="server" ResourceKey="btnSend" CssClass="dnnPrimaryAction" OnClick="btnSend_Click" /></li>
            <li><asp:LinkButton ResourceKey="btnCancel" runat="server" CssClass="dnnSecondaryAction simplemodal-close" CausesValidation="false" OnClick="btnCancel_Click1" /></li>
        </ul>
    </asp:Panel>
    </ContentTemplate></asp:UpdatePanel>
</asp:Panel>