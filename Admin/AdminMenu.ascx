<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Admin.AdminMenu" Codebehind="AdminMenu.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<div id="divAdminMenuWrapper" class="divAdminMenuWrapper Normal" runat="server">
        <div id="divAdminLinks">
            <ul>
                <asp:PlaceHolder ID="phLink" runat="server" Visible="false"></asp:PlaceHolder>
                <li>
                    <asp:LinkButton ID="lnkUpdateStatus" resourcekey="lnkUpdateStatus" runat="server" OnClick="lnkUpdateStatus_Click"></asp:LinkButton>
                </li>
            </ul>
        </div>
        <div id="PublishAdminStats">
                <div id="PublishStatsContent">
                    <asp:PlaceHolder ID="phStats" runat="server" Visible="false"></asp:PlaceHolder>
                </div>
        </div>
        
        <div id="divApprovalStatus" runat="server" visible="false" class="divItemApprovalSection">
            
            <div id="divVersionComments" class="Publish_VersionComments" runat="server">
            <dnn:label ID="lblVersionComments" runat="server" />
            <asp:Label ID="lblCurrentVersionComments" runat="server"  CssClass="Publish_CurrentVersionComments"/>
            </div>
            <dnn:label ID="lblApprovalComments" runat="server" />
            
            <asp:TextBox ID="txtApprovalComments" runat="server" CssClass="Publish_ApprovalComments" ></asp:TextBox><br />
            <asp:DropDownList ID="ApprovalStatusDropDownList" Runat="server" CssClass="Normal" />
            <br />
            <asp:LinkButton ID="lnkSaveApprovalStatus" runat="server" resourcekey="lnkSaveApprovalStatus" OnClick="lnkSaveApprovalStatus_Click"></asp:LinkButton>   
            <asp:LinkButton ID="lnkSaveApprovalStatusCancel" runat="server" resourcekey="lnkSaveApprovalStatusCancel" OnClick="lnkSaveApprovalStatusCancel_Click"></asp:LinkButton>   
        </div>
        
        <asp:Label ID="lblApprovalResults" runat="server" Visible="false" CssClass="lblApprovalResults"></asp:Label>
        
        <div id="divAdminMenu" runat="server" class="Normal">
            
        </div>
        
</div>

<br />  