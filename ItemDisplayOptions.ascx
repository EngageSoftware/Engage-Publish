<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.ItemDisplayOptions" Codebehind="ItemDisplayOptions.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="sectionhead" Src="~/controls/sectionheadcontrol.ascx" %>

<style type="text/css">
    @import url(<%=Engage.Dnn.Publish.ModuleBase.ApplicationUrl %><%=Engage.Dnn.Publish.ModuleBase.DesktopModuleFolderName %>Module.css);
    .dvUpdateBtns { DISPLAY: none }
</style>
<br />

<asp:UpdatePanel id="upnlSettings" runat="server" UpdateMode="Conditional"><ContentTemplate>

    <div style="text-align:left">
        <table cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
            <tr>
                <td class="SubHead"><dnn:label id="lblChooseDisplayType" resourcekey="lblChooseDisplayType" runat="server" /></td>
                <td class="NormalTextBox"><asp:dropdownlist id="ddlChooseDisplayType" Runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlChooseDisplayType_SelectedIndexChanged"></asp:dropdownlist></td>
            </tr>
        </table>
        <dnn:sectionhead id="shCurrentDisplay" cssclass="Head" runat="server" text="Basic Options" section="dvBasic" resourcekey="shCurrentDisplay" isexpanded="True"/><hr />
        <div id="dvBasic" runat="server">
            <asp:PlaceHolder ID="phControls" runat="server" ></asp:PlaceHolder>
        </div>
        <br />
        <dnn:sectionhead id="shAdvanced" cssclass="Head" runat="server" text="Advanced Options" section="dvAdvanced" resourcekey="shAdvanced" isexpanded="True"/>
        <hr />        
        <div id="dvAdvanced" runat="server">
            <table id="tblAdvanced" cellspacing="0" cellpadding="0" border="0" class="SettingsTable" >
                <tr>
                    <td class="SubHead"><dnn:label id="lblOverrideable" resourcekey="lblOverrideable" runat="server" /></td>
                    <td class="NormalTextBox"><asp:checkbox id="chkOverrideable" Runat="server"></asp:checkbox>&nbsp;&nbsp;&nbsp;<asp:LinkButton id="btnConfigure" runat="server" OnClick="btnConfigure_Click" resourcekey="btnConfigure" /></td>
                </tr>
                <tr>
                    <td class="SubHead"><dnn:label id="lblAllowTitleUpdate" resourcekey="lblAllowTitleUpdate" runat="server" /></td>
                    <td class="NormalTextBox"><asp:checkbox id="chkAllowTitleUpdate" Runat="server"></asp:checkbox></td>
                </tr>
                <tr>
                    <td class="SubHead"><dnn:label id="lblLogBreadcrumb" resourcekey="lblLogBreadCrumb" runat="server" /></td>
                    <td class="NormalTextBox"><asp:checkbox id="chkLogBreadcrumb" Runat="server"></asp:checkbox></td>
                </tr>
                <tr>
                    <td class="SubHead"><dnn:label id="lblEnableWLWSupport" resourcekey="lblEnableWLWSupport" runat="server" /></td>
                    <td class="NormalTextBox"><asp:checkbox id="chkEnableWLWSupport" Runat="server"></asp:checkbox></td>
                </tr>
                <tr>
                    <td class="SubHead"><dnn:label id="lblCacheTime" resourcekey="lblCacheTime" runat="server" /></td>
                    <td class="NormalTextBox"><asp:TextBox ID="txtCacheTime" runat="server" Text="0" /><asp:RangeValidator id="rvCacheTime"  resourcekey="rvCacheTime" CssClass="error" runat="server" ControlToValidate="txtCacheTime" MaximumValue="1000" MinimumValue="0" Type="Integer"></asp:RangeValidator></td>
                </tr>

            </table>
            <asp:Panel ID="pnlConfigureOverrideable" runat="server" CssClass="configureOverrideable" Visible="false">
                <br />
                <dnn:sectionhead id="shArticleDisplay" cssclass="Head" runat="server" text="Article Display Settings" section="divArticleDisplay" resourcekey="shArticleDisplay" isexpanded="false"/>
                <div id="divArticleDisplay" runat="server" />
                <br />
                <dnn:sectionhead id="shCategoryDisplay" cssclass="Head" runat="server" text="Category Display Settings" section="divCategoryDisplay" resourcekey="shCategoryDisplay" isexpanded="false"/>
                <div id="divCategoryDisplay" runat="server" />
            </asp:Panel>
        </div>
    </div>
</ContentTemplate></asp:UpdatePanel>
