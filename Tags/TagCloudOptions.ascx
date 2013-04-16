<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Tags.TagCloudOptions" Codebehind="TagCloudOptions.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelControl.ascx" %>

<table cellspacing="0" cellpadding="0" border="0" class="SettingsTable eng-publish-tag-cloud-options">
    <tr>
        <td class="SubHead"><dnn:Label runat="server" ControlName="txtPopularTagCount" ResourceKey="lblPopularTagCount" /></td>
		<td class="NormalTextBox"><asp:CheckBox ID="chkLimitTagCount" runat="server" /></td>
	</tr>
</table>