<%@ Control Language="c#" AutoEventWireup="False" Inherits="Engage.Dnn.Publish.TextHtml.Edit" Codebehind="Edit.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="texteditor" Src="~/controls/TextEditor.ascx" %>
    
<div id="publishTextHtmlDisplay">
    <dnn:texteditor id="teArticleText" runat="server" Width="100%" Height="500" HtmlEncode="false" TextRenderMode="Raw" />
    <asp:LinkButton ID="btnSubmit" runat="server" onclick="btnSubmit_Click" />
</div>