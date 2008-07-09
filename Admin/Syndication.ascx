<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Admin.Syndication"
    Codebehind="Syndication.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<table border="0" class="Normal">
    <tr>
        <td colspan="2" class="Head">
            <asp:Label ID="lblSyndicationSettings" resourcekey="lblSyndicationSettings" Text="Syndication Configuration"
                runat="server" /></td>
    </tr>
    <tr>
        <td colspan="2" class="Head">
            &nbsp;<asp:Label ID="lblPublisherSyndicationSettings" resourcekey="lblPublisherSyndicationSettings"
                runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            &nbsp;&nbsp;
            <asp:Label ID="lblPublisherSyndication" runat="server" resourceKey="lblPublisherSyndication"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;&nbsp;<dnn:label ID="lblAuthorizationKeyHelp" runat="server" Text="Authorization Key"
                ControlName="lblAuthorizationKey" Suffix=":" />
        </td>
        <td>
            <asp:Label ID="lblAuthorizationKey" CssClass="Normal" Width="390" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;&nbsp;<dnn:label ID="lblServiceUrlHelp" runat="server" ControlName="lblServiceUrl"
                Text="Service Url" Suffix=":" />
        </td>
        <td>
            <asp:Label ID="lblServiceUrl" CssClass="Normal" runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="2" class="Head">
            &nbsp;<asp:Label ID="lblSubscriberSyndicationSettings" resourcekey="lblSubscriberSyndicationSettings"
                runat="server" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            &nbsp;&nbsp;
            <asp:Label ID="lblSubscriberSyndication" runat="server" resourceKey="lblSubscriberSyndication"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;&nbsp;
            <dnn:label ID="lblSubscriberAuthorizationKey" runat="server" Text="Authorization Key"
                ControlName="lblSubscriberAuthorizationKey" Suffix=":" />
        </td>
        <td>
            <asp:TextBox ID="txtAuthorizationKey" CssClass="Normal" Width="200" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;&nbsp;
            <dnn:label ID="lblSubscriberServiceUrl" runat="server" ControlName="lblSubscriberServiceUrl"
                Text="Service Url" Suffix=":" />
        </td>
        <td>
            <asp:TextBox ID="txtSubscriberServiceUrl" CssClass="Normal" runat="server" Width="390" />
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;&nbsp;
            <dnn:label ID="lblArchiveContent" runat="server" ControlName="chkArchiveContent"
                Text="Archive my content locally if not found on publishing site."></dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkArchiveContent" runat="server" Checked="true" />
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;&nbsp;
            <dnn:label ID="lblApproveContent" runat="server" ControlName="chkApproveContent"
                Text="Disable approval emails when synchronizing content from the publishing site.">
            </dnn:label>
        </td>
        <td>
            <asp:CheckBox ID="chkApproveContent" runat="server" Checked="true" />
        </td>
    </tr>
</table>
<asp:LinkButton ID="cmdUpdate" runat="server" ResourceKey="btnDelete" CssClass="CommandButton" OnClick="cmdUpdate_Click">Update</asp:LinkButton><br />
<asp:Label ID="lblResults" runat="server" Visible="false" />
