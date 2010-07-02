<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Tags.TagCloudOptions" Codebehind="TagCloudOptions.ascx.cs" %>
<%@ Import Namespace="Engage.Dnn.Publish" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<style type="text/css">
    @import url(<%=ModuleBase.ApplicationUrl%><%=ModuleBase.DesktopModuleFolderName%>Module.css);
</style>

<table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
    <tr>
        <td class="SubHead"><dnn:label id="lblPopularTagCount" runat="server" controlname="txtPopularTagCount" text="Limit display to 50:" ResourceKey="lblPopularTagCount"></dnn:label></td>
		<td class="NormalTextBox"><asp:CheckBox ID="chkLimitTagCount" runat="server" /></td>
	</tr>
</table>