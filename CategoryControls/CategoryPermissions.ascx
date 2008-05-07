<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.CategoryControls.CategoryPermissions" Codebehind="CategoryPermissions.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<div id="divCategoryPermissions" runat="server">
	<table id="Table1" height="94" cellspacing="1" cellpadding="1" width="480" border="0">
		<tr>
			<td colspan="4"><asp:label id="lblMessage" runat="server" ForeColor="Red" EnableViewState="False"></asp:label></td>
		</tr>
		<tr>
			<td align="center" width="204"><dnn:label class="SubHead" id="lblAvailableRoles" ResourceKey="lblAvailableRoles" runat="server"></dnn:label><asp:listbox id="lstItems" SelectionMode="Multiple" Runat="server" Rows="6" Width="250px"></asp:listbox></td>
			<td width="19" colspan="1">
				<table id="Table2" height="42" cellspacing="1" cellpadding="1" width="24" align="right"
					border="0">
					<tr>
						<td valign="middle"><asp:imagebutton id="imgAdd" runat="server" ImageUrl="~/desktopmodules/Engage.Dnn.Publish/images/rt.gif" AlternateText="Add"></asp:imagebutton></td>
					</tr>
					<tr>
						<td valign="middle" height="29"><asp:imagebutton id="imgRemove" runat="server" ImageUrl="~/desktopmodules/Engage.Dnn.Publish/images/lt.gif" AlternateText="Remove"></asp:imagebutton></td>
					</tr>
				</table>
				<p>&nbsp;</p>
			</td>
			<td align="center" width="212"><dnn:label class="SubHead" id="lblSelectedRoles" ResourceKey="lblSelectedRoles" runat="server" Width="146px"></dnn:label><asp:listbox id="lstSelectedItems" SelectionMode="Multiple" Runat="server" Rows="6" Width="250px"></asp:listbox></td>
		</tr>
	</table>
	<hr/>
</div>
<br/>

