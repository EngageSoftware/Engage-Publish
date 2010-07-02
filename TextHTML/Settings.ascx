<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.TextHtml.Settings" Codebehind="Settings.ascx.cs" %>
<%@ Import Namespace="Engage.Dnn.Publish" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="sectionhead" Src="~/controls/sectionheadcontrol.ascx" %>

<style type="text/css">
    @import url(<%=ModuleBase.ApplicationUrl%><%=ModuleBase.DesktopModuleFolderName%>Module.css);
    .dvUpdateBtns { DISPLAY: none }
</style>
<br />

<asp:UpdatePanel id="upnlSettings" runat="server" UpdateMode="Conditional"><ContentTemplate>

    <div class="SettingsTable">
            <table id="tblAdvanced" cellspacing="0" cellpadding="0" border="0" class="SettingsTable" >
                <tr>
                    <td class="SubHead"><dnn:label id="lblTemplate" resourcekey="lblTemplate" runat="server" /></td>
                    <td class="NormalTextBox"><asp:TextBox ID="txtTemplate" runat="server" Text="[ArticleText]" TextMode="MultiLine" CssClass="TemplateTextBox" /></td>
                </tr>
            </table>
    </div>
</ContentTemplate></asp:UpdatePanel>
