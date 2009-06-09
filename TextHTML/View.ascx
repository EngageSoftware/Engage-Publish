<%@ Control Language="c#" AutoEventWireup="False" Inherits="Engage.Dnn.Publish.TextHtml.View" Codebehind="View.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<div id="divAdminMenuWrapper" class="divAdminMenuWrapper Normal" runat="server" visible="false">
    <div id="divPublishApprovals" runat="server" visible="false">
            <div id="divApprovalStatus" runat="server" visible="false" class="divItemApprovalSection">
                <dnn:label ID="lblApprovalComments" runat="server" />          
                <asp:TextBox ID="txtApprovalComments" runat="server" CssClass="Publish_ApprovalComments" ></asp:TextBox><br />
                <asp:DropDownList ID="ddlApprovalStatus" Runat="server" CssClass="Normal" />
                <br />
                <asp:LinkButton ID="lnkSaveApprovalStatus" runat="server" resourcekey="lnkSaveApprovalStatus" OnClick="lnkSaveApprovalStatus_Click"></asp:LinkButton>   
                <asp:LinkButton ID="lnkSaveApprovalStatusCancel" runat="server" resourcekey="lnkSaveApprovalStatusCancel" OnClick="lnkSaveApprovalStatusCancel_Click"></asp:LinkButton>   
            </div>
    </div>
</div>

<div class="publishTextHtmlDisplay Normal">
    <asp:Literal ID="lblArticleText" runat="server"></asp:Literal>
</div>