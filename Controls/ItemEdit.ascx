<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.ItemEdit" Codebehind="ItemEdit.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelControl.ascx" %>

<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="engage" TagName="Thumbnail" Src="~/DesktopModules/EngagePublish/Controls/ThumbnailSelector.ascx" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>

<table class="PublishEditTable Normal">
    <tr>
        <td class="editTableLabelColumn nowrap">
            <dnn:Label ID="lblName" ResourceKey="lblName" runat="server" cssclass="title"/>
        </td><td class="fullWidth">
            <asp:TextBox ID="txtName" runat="server" CssClass="txtName" />
            <asp:CheckBox ID="chkDisplayAsHyperlink" runat="server" />
            
            <dnn:Label ID="lblDisplayAsHyperlink" ResourceKey="lblDisplayAsHyperlink" runat="server" cssclass="title" />
            
        </td>
    </tr>
</table>
<table class="PublishEditTable Normal">
    <tr>
        <td class="editTableLabelColumn nowrap">
            <dnn:Label ID="lblDescription" ResourceKey="lblDescription" runat="server" cssclass="title" />
        </td><td class="publishDescriptionEditor fullWidth">
            <div  class="rightAlign">
                <asp:LinkButton ID="btnChangeDescriptionEditorMode" runat="server" OnClick="btnChangeDescriptionEditorMode_Click" />
            </div>
            <dnn:TextEditor ID="teDescription" runat="server" HtmlEncode="false"  ChooseRender="false" TextRenderMode="Text" Visible="false" />
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" cssclass="publishDescriptionTxt" />
            <hr />
        </td>
    </tr>
</table>
<ajaxToolkit:CollapsiblePanelExtender
     TargetControlID="pnlItemEditAdvanced" 
     ExpandControlID="TitlePanel" 
     CollapseControlID="TitlePanel" 
     Collapsed="true" 
     ImageControlID="imgItemEditAdvancedHeader"
     TextLabelID="lblItemEditAdvancedHeader"
     ID="clpAdvanced" 
     runat="server" 
     SuppressPostBack="true"
     
     />
    <asp:Panel ID="TitlePanel" runat="server" CssClass="collapsePanelHeader"> 
           <table class="PublishEditTable Normal">
        <tr>
            <td class="editTableLabelColumn nowrap">
                <asp:Label ID="lblItemEditAdvancedHeader" CssClass="SubHead" resourcekey="lblItemEditAdvancedHeader" runat="server" />
            </td><td class="fullWidth">
                <asp:image id="imgItemEditAdvancedHeader" runat="server" />
                &nbsp;
            </td>
        </tr>
    </table>
           
           
    </asp:Panel>
    
<asp:Panel ID="pnlItemEditAdvanced" runat="server">
    <table class="PublishEditTable Normal">
        <tr>
            <td class="editTableLabelColumn nowrap">
                <dnn:Label ID="lblAuthor" runat="server" ResourceKey="lblAuthor" />
            </td><td class="fullWidth">
                <asp:DropDownList ID="ddlAuthor" runat="server" 
                    onselectedindexchanged="ddlAuthor_SelectedIndexChanged" AutoPostBack="true" />
                <hr />
            </td>
        </tr>
        <tr>
            <td class="editTableLabelColumn nowrap">
                <dnn:Label ID="lblAuthorName" runat="server" ResourceKey="lblAuthorName" />
            </td><td class="fullWidth">
                <asp:TextBox ID="txtAuthorName" runat="server" CssClass="NormalTextBox"></asp:TextBox>
                <hr />
            </td>
        </tr>

    </table>

    <table class="PublishEditTable Normal">
        <tr>
            <td class="editTableLabelColumn nowrap">
                <dnn:Label ID="lblUploadFile" runat="server" ResourceKey="lblUploadFile"/>
            </td><td class="fullWidth">
                <engage:Thumbnail ID="thumbnailSelector" runat="server" />
                <hr />
            </td>
        </tr>
    </table>
    <table class="PublishEditTable Normal">
        <tr>
            <td class="editTableLabelColumn nowrap">
                <dnn:Label ID="lblPostingDates" runat="server" ResourceKey="lblPostingDates" />
            </td><td class="fullWidth">
                <table class="Normal fullWidth"><tr>
                    <td>
                        <dnn:Label ID="lblStartDate" ResourceKey="lblStartDate" runat="server" CssClass="title"/>
                        <asp:TextBox ID="txtStartDate" runat="server"/>&nbsp;<asp:Image ID="imgStartCalendarIcon" runat="server" ImageUrl="~/desktopModules/EngagePublish/images/calendar.png" />
                        <ajaxToolkit:CalendarExtender ID="calendarStart" runat="server" TargetControlID="txtStartDate" PopupButtonID="imgStartCalendarIcon" />

                    </td><td>
                        <dnn:Label ID="lblEndDate" ResourceKey="lblEndDate" runat="server" cssclass="title"/>
                        <asp:TextBox ID="txtEndDate" runat="server"/>&nbsp;<asp:Image ID="imgEndCalendarIcon" runat="server" ImageUrl="~/desktopModules/EngagePublish/images/calendar.png" />
                        <ajaxToolkit:CalendarExtender ID="calendarEnd" runat="server" TargetControlID="txtEndDate" PopupButtonID="imgEndCalendarIcon" />
                    </td>
                </tr></table>
                <hr />
            </td>
        </tr>
    </table>
    <table class="PublishEditTable Normal">
        <tr>
            <td class="editTableLabelColumn nowrap">
                <dnn:Label ID="lblSearchEngine" runat="server" Text="Optimize for Search Engines: " ResourceKey="lblSearchEngine" />
            </td><td class="fullWidth">
                <asp:CheckBox ID="chkSearchEngine" runat="server" AutoPostBack="true" OnCheckedChanged="chkSearchEngine_CheckedChanged" />
                
                <asp:UpdatePanel ID="upnlSearchEngine" runat="server" UpdateMode="Conditional">
                <Triggers><asp:AsyncPostBackTrigger ControlID="chkSearchEngine" /></Triggers>
                <ContentTemplate>
                    <asp:Panel ID="pnlSearchEngine" runat="server" visible="false">
                        <div id="divSearchEngine"><%-- runat="server"--%>
                            <dnn:Label ID="lblMetaTitle" ResourceKey="lblMetaTitle" runat="server" cssclass="title"/>
                            <asp:TextBox ID="txtMetaTitle" runat="server" TextMode="SingleLine" Columns="50" Rows="1"/>
                            <br />
                            <br />
                            <dnn:Label ID="lblMetaKeywords" ResourceKey="lblMetaKeywords" runat="server" cssclass="title"/>
                            <asp:TextBox ID="txtMetaKeywords" runat="server" TextMode="MultiLine" Columns="50" Rows="3"/>
                            <br />
                            <br />
                            <dnn:Label ID="lblMetaDescription" ResourceKey="lblMetaDescription" runat="server" cssclass="title"/>
                            <asp:TextBox ID="txtMetaDescription" runat="server" TextMode="MultiLine" Columns="50" Rows="3"/>
                        </div>
                    </asp:Panel>
                </ContentTemplate></asp:UpdatePanel>
                <hr />
            </td>
        </tr>
    </table>
    <table class="PublishEditTable Normal">
        <tr>
            <td class="editTableLabelColumn nowrap">
                <dnn:Label ID="lblChooseUrl" runat="server" ResourceKey="lblChooseUrl"/>
            </td>
            <td class="fullWidth">
                <asp:CheckBox ID="chkUrlSelection" runat="server" AutoPostBack="true" OnCheckedChanged="chkUrlSelection_CheckedChanged" />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <Triggers><asp:AsyncPostBackTrigger ControlID="chkUrlSelection" /></Triggers>
                <ContentTemplate>
                    <asp:Panel ID="pnlUrlSelection" runat="server" visible="false">
                        <div id="divUrlSelection"><%-- runat="server"--%>
                            <dnn:URL ID="ctlUrlSelection" runat="server" Width="325"
                             ShowFiles="true"
                             ShowUrls="true" 
                             ShowTabs="true" 
                             ShowLog="false" 
                             ShowTrack="false" 
                             Required="False" 
                             ShowNewWindow="False"/>
                             <asp:CheckBox ID="chkNewWindow" resourcekey="chkNewWindow" runat="server" />
                        </div>
                    </asp:Panel>
                </ContentTemplate></asp:UpdatePanel>
                <hr />
            </td>
        </tr>
    </table>

</asp:Panel>

