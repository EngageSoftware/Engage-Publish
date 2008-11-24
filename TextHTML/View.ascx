<%@ Control Language="c#" AutoEventWireup="False" Inherits="Engage.Dnn.Publish.TextHtml.View" Codebehind="View.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<div id="divPublishApprovals" runat="server" visible="false">
        <div id="divApprovalStatus" runat="server" visible="false" class="divItemApprovalSection">
            <div id="divVersionComments" class="Publish_VersionComments" runat="server">
                <dnn:label ID="lblVersionComments" runat="server" />
                <asp:Label ID="lblCurrentVersionComments" runat="server"  CssClass="Publish_CurrentVersionComments"/>
            </div>

            <dnn:label ID="lblApprovalComments" runat="server" />          
            <asp:TextBox ID="txtApprovalComments" runat="server" CssClass="Publish_ApprovalComments" ></asp:TextBox><br />
            <asp:DropDownList ID="ddlApprovalStatus" Runat="server" CssClass="Normal" />
            <br />
            <asp:LinkButton ID="lnkSaveApprovalStatus" runat="server" resourcekey="lnkSaveApprovalStatus" OnClick="lnkSaveApprovalStatus_Click"></asp:LinkButton>   
            <asp:LinkButton ID="lnkSaveApprovalStatusCancel" runat="server" resourcekey="lnkSaveApprovalStatusCancel" OnClick="lnkSaveApprovalStatusCancel_Click"></asp:LinkButton>   
        </div>

</div>

<div id="publishTextHtmlDisplay" class="Normal">
    <asp:Label ID="lblArticleText" runat="server"></asp:Label>
</div>