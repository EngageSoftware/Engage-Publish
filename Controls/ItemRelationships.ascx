<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.ItemRelationships" Codebehind="ItemRelationships.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<div id="divItemRelationships" runat="server">
    <table id="Table1" height="94" cellspacing="1" cellpadding="1" width="480" border="0" class="Normal">
        <tr>
            <td colspan="4">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="False" ForeColor="Red"/>
            </td>
        </tr>
        <tr valign="bottom">
            <td align="left" colspan="4">
                <asp:Panel id="pnlItemSearch" runat="server" Visible="false" CssClass="Normal" DefaultButton="btnItemSearch">
                    <dnn:label ID="lblItemSearch" ResourceKey="lblItemSearch" runat="server"/>
                    <asp:TextBox ID="txtItemSearch" runat="server"/>
                    <asp:LinkButton ID="btnItemSearch" resourcekey="btnItemSearch" runat="server"/>
                </asp:Panel>
            </td>
        </tr>
        <tr valign="top">
            <td width="204" align="center">
                <dnn:label class="title" ID="lblAvailableCategories" ResourceKey="lblAvailableCategories" runat="server"/>
                <asp:ListBox ID="lstItems" CssClass="Publish_ListItems Available" Rows="6" runat="server" SelectionMode="Multiple" />
            </td>
            <td width="19" colspan="1">
                <table cellspacing="1" cellpadding="1" width="24" align="right" border="0" style="margin-top:12px">
                    <tr>
                        <td>
                            <asp:ImageButton ID="imgAdd" runat="server" ImageUrl="~/desktopmodules/EngagePublish/images/rt.gif" AlternateText="Add"/>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="center" width="212">
                <dnn:label class="title" ID="lblSelectedCategories" runat="server" Width="146px"/>
                <asp:ListBox ID="lstSelectedItems" CssClass="Publish_ListItems Selected" Rows="6" runat="server" SelectionMode="Multiple"/>
            </td>
            <td >
                <table cellspacing="1" cellpadding="1" width="24" align="right" border="0" style="margin-top:12px">
                    <tr id="trUpImage" runat="server" visible="false">
                        <td >
                            <asp:ImageButton ID="imgUp" runat="server" ImageUrl="~/desktopmodules/EngagePublish/images/up.gif" AlternateText="Up"/>
                        </td>
                    </tr>
                    <tr id="trDownImage" runat="server" visible="false">
                        <td >
                            <asp:ImageButton ID="imgDown" runat="server" ImageUrl="~/desktopmodules/EngagePublish/images/dn.gif" AlternateText="Down"/>
                        </td>
                    </tr>
                    <tr>
                        <td valign="bottom">
                            <asp:ImageButton ID="imgRemove" runat="server" ImageUrl="~/desktopmodules/EngagePublish/images/trash.gif" AlternateText="Remove"/>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <div id="divDateControls" runat="server" visible="false">
                    <table cellpadding="0" border="0" cellspacing="0" class="Normal">
                        <tr>
                            <td class="nowrap">
                                <dnn:label ID="lblStartDate" runat="server"/>
                            </td>
                            <td>
                                <asp:TextBox ID="txtStartDate" runat="server"/>
                            </td>
                        </tr>
                        <tr>
                            <td class="nowrap">
                                <dnn:label ID="lblEndDate" runat="server"/>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEndDate" runat="server"/>
                            </td>
                        </tr>
                    </table>
                    <asp:LinkButton ID="btnStoreRelationshipDate" runat="server"/>
                </div>
            </td>
        </tr>
    </table>
    <hr />
</div>
