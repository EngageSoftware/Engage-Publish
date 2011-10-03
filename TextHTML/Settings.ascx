<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.TextHtml.Settings" Codebehind="Settings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<div>
    <table id="tblAdvanced" cellspacing="0" cellpadding="0" border="0" class="SettingsTable eng-publish-text-html-settings">
        <tr>
            <td class="SubHead"><dnn:label ResourceKey="lblTemplate" runat="server" /></td>
            <td class="NormalTextBox"><asp:TextBox ID="txtTemplate" runat="server" Text="[ArticleText]" TextMode="MultiLine" CssClass="TemplateTextBox" /></td>
        </tr>
    </table>
</div>