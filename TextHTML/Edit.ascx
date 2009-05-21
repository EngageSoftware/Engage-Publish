<%@ Control Language="c#" AutoEventWireup="False" Inherits="Engage.Dnn.Publish.TextHtml.Edit" Codebehind="Edit.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="texteditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="ep" TagName="approvals" Src="~/desktopmodules/engagepublish/controls/itemapproval.ascx" %>
<div class="ErrorBody mmText" runat="server" id="divPublishNotifications" visible="false">
    <asp:Label ID="lblPublishMessages" runat="server"/>
</div>

<div class="publishTextHtmlDisplay Normal" id="publishTextHTMLEntry" runat="server">
    <dnn:texteditor id="teArticleText" runat="server" Width="100%" Height="500" HtmlEncode="false" TextRenderMode="Raw" />
    <div>
        <asp:label ID="lblApproval" runat="server" />
        <ep:approvals ID="epApprovals" runat="server" />
    </div>
    <asp:LinkButton ID="btnSubmit" runat="server" onclick="btnSubmit_Click" />
</div>