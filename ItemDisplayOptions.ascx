<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.ItemDisplayOptions" Codebehind="ItemDisplayOptions.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Sectionhead" Src="~/controls/Sectionheadcontrol.ascx" %>

<div>
    <table cellspacing="0" cellpadding="0" border="0" class="SettingsTable eng-publish-choose-display">
        <tr>
            <td class="SubHead"><dnn:Label ResourceKey="lblChooseDisplayType" runat="server" /></td>
            <td class="NormalTextBox"><asp:dropdownlist ID="ddlChooseDisplayType" runat="server" AutoPostBack="true" /></td>
        </tr>
    </table>

    <dnn:Sectionhead CssClass="Head" runat="server" Section="dvBasic" ResourceKey="shCurrentDisplay" IsExpanded="True"/>
    <div id="dvBasic" runat="server" class="eng-publish-basic-settings">
        <asp:PlaceHolder ID="phControls" runat="server" />
    </div>

    <dnn:Sectionhead CssClass="Head" runat="server" Section="dvAdvanced" ResourceKey="shAdvanced" IsExpanded="True"/>
    <div id="dvAdvanced" runat="server" class="eng-publish-advanced-settings">
        <table id="tblAdvanced" cellspacing="0" cellpadding="0" border="0" class="SettingsTable">
            <tr class="eng-publish-advanced-overrideable">
                <td class="SubHead"><dnn:Label ResourceKey="lblOverrideable" runat="server" /></td>
                <td class="NormalTextBox">
                    <asp:checkbox ID="chkOverrideable" runat="server" />
                    <asp:LinkButton ID="btnConfigure" runat="server" ResourceKey="btnConfigure" />
                </td>
            </tr>
            <tr class="eng-publish-advanced-title-update">
                <td class="SubHead"><dnn:Label ResourceKey="lblAllowTitleUpdate" runat="server" /></td>
                <td class="NormalTextBox"><asp:checkbox ID="chkAllowTitleUpdate" runat="server" /></td>
            </tr>
            <tr class="eng-publish-advanced-breadcrumb">
                <td class="SubHead"><dnn:Label ResourceKey="lblLogBreadCrumb" runat="server" /></td>
                <td class="NormalTextBox"><asp:checkbox ID="chkLogBreadcrumb" runat="server" /></td>
            </tr>
            <tr class="eng-publish-advanced-wlw-support">
                <td class="SubHead"><dnn:Label ResourceKey="lblEnableWLWSupport" runat="server" /></td>
                <td class="NormalTextBox"><asp:checkbox ID="chkEnableWLWSupport" runat="server" /></td>
            </tr>
            <tr class="eng-publish-advanced-cache-time">
                <td class="SubHead"><dnn:Label id="lblCacheTime" ResourceKey="lblCacheTime" runat="server" /></td>
                <td class="NormalTextBox">
                    <asp:TextBox ID="txtCacheTime" runat="server" Text="0" />
                    <asp:RangeValidator runat="server" ResourceKey="rvCacheTime" CssClass="error" ControlToValidate="txtCacheTime" MaximumValue="1000" MinimumValue="0" Type="Integer" />
                </td>
            </tr>
        </table>

        <asp:Panel ID="pnlConfigureOverrideable" runat="server" CssClass="configureOverrideable" Visible="false">
            <dnn:Sectionhead ID="shArticleDisplay" runat="server" CssClass="Head" Section="divArticleDisplay" ResourceKey="shArticleDisplay" IsExpanded="false"/>
            <div id="divArticleDisplay" runat="server" />
            
            <dnn:Sectionhead ID="shCategoryDisplay" runat="server" CssClass="Head" Section="divCategoryDisplay" ResourceKey="shCategoryDisplay" IsExpanded="false"/>
            <div id="divCategoryDisplay" runat="server" />
        </asp:Panel>
    </div>
</div>