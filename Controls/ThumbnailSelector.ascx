<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.ThumbnailSelector" Codebehind="ThumbnailSelector.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="URL" Src="~/controls/URLControl.ascx" %>

        <asp:Panel ID="pnlDnnThumb" runat="server">
        <dnn:URL ID="ctlMediaFile" runat="server" Width="325" ShowUrls="true" ShowTabs="False" ShowLog="False" ShowTrack="False" Required="False"/>
        </asp:Panel>



    <asp:Panel ID="pnlEngageThumb" runat="server">
<asp:UpdatePanel ID="upnlThumbnailImage" runat="server" UpdateMode="Conditional">
<Triggers><asp:PostBackTrigger ControlID="btnUploadThumbnail" /></Triggers>
<ContentTemplate>
    <asp:RadioButtonList ID="rblThumbnailImage" runat="server" CssClass="Normal" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblThumbnailImage_SelectedIndexChanged"/>
    <asp:MultiView ID="mvThumbnailImage" runat="server" ActiveViewIndex="0">
        <asp:View ID="vwUpload" runat="server">
            <asp:Label ID="lblImageName" runat="server" ResourceKey="lblImageName" /><br />
            <asp:FileUpload ID="fileThumbnail" runat="server" /><asp:LinkButton ID="btnUploadThumbnail" runat="server" ResourceKey="btnUploadThumbnail" OnClick="btnUploadThumbnail_Click" />
            <asp:Textbox id="txtMessage" runat="server" Visible="False" ReadOnly="True" ForeColor="Red" EnableViewState="False" Columns="75" TextMode="MultiLine" Rows="5"/>
        </asp:View>
        <asp:View ID="vwInternal" runat="server">
            <asp:Label ID="lblImageName2" runat="server" ResourceKey="lblImageName" /><br />
            <asp:DropDownList ID="ddlThumbnailLibrary" runat="server" />
        </asp:View>
        <asp:View ID="vwExternal" runat="server">
            <asp:Label ID="lblImageUrl" runat="server" ResourceKey="lblImageUrl" /><br />
            <asp:TextBox ID="txtThumbnailUrl" runat="server" Text="http://" Columns="60" />
        </asp:View>
    </asp:MultiView>
</ContentTemplate></asp:UpdatePanel>
   </asp:Panel>
