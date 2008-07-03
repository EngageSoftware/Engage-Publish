<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.CategoryControls.CategoryNLevelsOptions"
    Codebehind="CategoryNLevelsOptions.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<style type="text/css">
    @import url(<%=Engage.Dnn.Publish.ModuleBase.ApplicationUrl %><%=Engage.Dnn.Publish.ModuleBase.DesktopModuleFolderName %>Module.css);
</style>
        <table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
            <tr>
                <td class="SubHead">
                    <dnn:label ID="lblChooseCategory" runat="server" ResourceKey="lblChooseCategory"></dnn:label>
                </td>
                <td class="NormalTextBox">
                    <asp:DropDownList ID="ddlCategoryList" runat="server" >
                   <%-- AutoPostBack="true"  OnSelectedIndexChanged="ddlCategoryList_SelectedIndexChanged"--%>
                    
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="SubHead">
                    <dnn:label ID="lblChooseNLevels" runat="server" ResourceKey="lblChooseNLevels"></dnn:label>
                </td>
                <td class="NormalTextBox">
                    <asp:TextBox ID="txtNLevels" runat="server"></asp:TextBox></td>
            </tr>
<%--
    <tr>
        <td class="SubHead"><dnn:label id="lblChooseMItems" Runat="server" ResourceKey="lblChooseMItems"></dnn:label></td>
        <td class="NormalTextBox"><asp:TextBox id="txtMItems" Runat="server"></asp:TextBox></td>
    </tr>
--%>
            <tr>
                <td class="SubHead">
                    <dnn:label ID="lblHighlightCurrentItem" runat="server" ResourceKey="lblHighlightCurrentItem" />
                </td>
                <td class="NormalTextBox">
                    <asp:CheckBox runat="server" ID="chkHighlightCurrentItem" /></td>
            </tr>
            <tr>
                <td class="SubHead">
                    <dnn:label ID="lblShowParentItem" runat="server" ResourceKey="lblShowParentItem" />
                </td>
                <td class="NormalTextBox">
                    <asp:CheckBox runat="server" ID="chkShowParentItem" Checked="true" /></td>
            </tr>
<%--
            <tr>
                <td class="SubHead">
                    <dnn:label ID="lblSortItems" runat="server" ResourceKey="lblSortItems" EnableViewState="true"></dnn:label>
                </td>
                <td>
                    <asp:ListBox ID="lstItems" runat="server" DataValueField="ItemId" DataTextField="Name"
                        Rows="10" Width="150px"></asp:ListBox>
                    <asp:ImageButton ID="imgUp" runat="server" ImageUrl="~/desktopmodules/EngagePublish/images/up.gif"
                        AlternateText="Up" OnClick="imgUp_Click"></asp:ImageButton>
                    <asp:ImageButton ID="imgDown" runat="server" ImageUrl="~/desktopmodules/EngagePublish/images/dn.gif"
                        AlternateText="Down" OnClick="imgDown_Click"></asp:ImageButton></td>
            </tr>
--%>
        </table>
