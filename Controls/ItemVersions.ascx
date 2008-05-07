<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.ItemVersions" Codebehind="ItemVersions.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
<div id="divItemVersions" runat="server">
    <asp:DataGrid ID="dgVersions" runat="server" EnableViewState="true" AutoGenerateColumns="false" Width="100%"
        AlternatingItemStyle-CssClass="DataGrid_AlternatingItem" 
        ItemStyle-CssClass="DataGrid_Item"
        HeaderStyle-CssClass="DataGrid_Header"
        RowStyle-CssClass="DataGrid_Item"
        PagerStyle-CssClass="Normal"
        CssClass="Normal" 
        >       
        <Columns>            
            <asp:TemplateColumn HeaderText="Name">
                <ItemTemplate>
                   <asp:HyperLink ID="hlPreview" runat="server" CssClass="Normal" NavigateUrl='<%# GetPreviewUrl(Container.DataItem) %>'
                        Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Description">
                <ItemTemplate>
                    <asp:Label ID="lblDescription" runat="server" CssClass="Normal" Text='<%# GetItemDescription(Container.DataItem) %>'></asp:Label>
                    
                    <ajaxToolkit:HoverMenuExtender 
                    id="hme2" 
                    runat="Server" 
                    TargetControlID="lblDescription" 
                    PopupControlID="PopupMenu" 
                    PopupPosition="Bottom"
                    OffsetX="0" OffsetY="0" 
                    PopDelay="50" />
                         
                    <asp:Panel CssClass="publishVersionDescription" ID="PopupMenu" runat="server" style="display:none;">
                        <div class="Normal">
                        <%# GetItemDescriptionFull(Container.DataItem) %>
                        </div>
                    </asp:Panel>
                    
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:TemplateColumn HeaderText="Author">
                <ItemTemplate>
                    <asp:Label ID="lblAuthor" runat="server" CssClass="Normal" Text='<%# GetAuthorName(DataBinder.Eval(Container.DataItem,"AuthorUserId")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="VersionNumber" HeaderText="Version">
                <ItemStyle CssClass="Normal" />
            </asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Status">
                <ItemTemplate>
                    <asp:Label ID="lblStatus" runat="server" CssClass="Normal" Text='<%# GetStatus(DataBinder.Eval(Container.DataItem,"approvalStatusId")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>            
            <asp:TemplateColumn HeaderText="Start Date/End Date">
                <ItemTemplate>
                    <asp:Label ID="lblDates" runat="server" CssClass="Normal" Text='<%# GetDates(Container.DataItem) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>            
            <asp:BoundColumn DataField="ItemVersionDate" HeaderText="Version Date">
                <ItemStyle CssClass="Normal" />
            </asp:BoundColumn>
            <asp:TemplateColumn HeaderText="">
                <ItemTemplate>
                  <asp:HyperLink ID="hlEdit" runat="server" CssClass="Normal" NavigateUrl='<%# GetVersionEditUrl(Container.DataItem) %>'
                        Text='<%# GetLocalizedEditText() %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateColumn>
        </Columns>
    </asp:DataGrid></div>
<br />
<div style="text-align:center;">
    <asp:linkbutton class="CommandButton" id="cmdBack" resourcekey="cmdBack" runat="server" text="Back" causesvalidation="False" OnClick="cmdBack_Click"></asp:linkbutton>
</div>
