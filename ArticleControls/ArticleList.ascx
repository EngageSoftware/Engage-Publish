<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.ArticleControls.ArticleList" Codebehind="ArticleList.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<asp:UpdateProgress ID="upArticleListProgress" runat="server" AssociatedUpdatePanelID="upnlArticleList">
    <ProgressTemplate>
        <div class="progressWrap">
            <div class="progressUpdateMessage">
                <asp:Label ID="lblProgressUpdate" runat="server" resourcekey="lblProgressUpdate"></asp:Label>
                <img src="<%=ApplicationUrl%><%=DesktopModuleFolderName %>images/progressbar_green.gif" alt="Updating" id="imgProgressUpdate" />
            </div>
        </div>
        <div class="progressUpdate">
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
    	
<asp:UpdatePanel ID="upnlArticleList" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <table border="0" class="Normal">
            <tr valign="top">
                <td><dnn:Label ID="lblItemType" ResourceKey="lblItemType" Runat="server" CssClass="Normal" ControlName="cboCategories"></dnn:Label></td>
                <td><asp:DropDownList ID="cboCategories" Runat="server" AutoPostBack="True" CssClass="Normal"></asp:DropDownList></td>
                <td><dnn:Label ID="lblWorkflow" ResourceKey="lblWorkFlow" Runat="server" CssClass="Normal" ControlName="cboWorkFlow"></dnn:Label></td>
                <td><asp:DropDownList ID="cboWorkflow" Runat="server" AutoPostBack="True" CssClass="Normal"></asp:DropDownList></td>
                <td><dnn:Label ID="lblArticleSearch" ResourceKey="lblArticleSearch" Runat="server" cssClass="Normal" ControlName="txtArticleSearch"></dnn:Label></td>   
                <td><asp:TextBox ID="txtArticleSearch" runat="server" CssClass="Normal"></asp:TextBox></td>
                <td><asp:LinkButton ID="btnFilter" runat="server" resourcekey="btnFilter" 
                        onclick="btnFilter_Click"  CssClass="Normal" /></td>

            </tr>
        </table>
        <div id="divArticleRepeater">
            <asp:GridView ID="dgItems" 
                Visible="false" 
                runat="server" 
                EnableViewState="true" 
                AlternatingRowStyle-CssClass="DataGrid_AlternatingItem Normal"
                HeaderStyle-CssClass="DataGrid_Header"
                RowStyle-CssClass="DataGrid_Item Normal"
                PagerStyle-CssClass="Normal"
                CssClass="Normal" 
                AutoGenerateColumns="false" 
                width="100%"
                AllowPaging="true"
                PagerSettings-Visible="true" 
                PageSize='<%# DefaultAdminPagingSize %>'
                OnPageIndexChanging="dgItems_PageIndexChanging"
                AllowSorting="true"
                OnSorting="dgItems_Sorting"
                >
                <Columns>
                
                    <asp:TemplateField ShowHeader="true"  HeaderText="SelectText" ItemStyle-CssClass="Publish_CheckBoxColumn">
                        <ItemTemplate>
                               <asp:CheckBox ID="chkSelect" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField ShowHeader="true"  HeaderText="ID" SortExpression="ItemId">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlId" runat="server" CssClass="Normal" NavigateUrl='<%# GetItemVersionLinkUrl(DataBinder.Eval(Container.DataItem,"ItemVersionId")) %>'  Text='<%# DataBinder.Eval(Container.DataItem,"ItemId") %>'/>
                            <asp:Label ID="lblItemVersionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"ItemVersionId") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField ShowHeader="true"  HeaderText="Name" SortExpression="Name">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlPreview" runat="server" CssClass="Normal" NavigateUrl='<%# GetItemVersionLinkUrl(DataBinder.Eval(Container.DataItem,"ItemVersionId")) %>'
                                Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField ShowHeader="true"  HeaderText="Description" SortExpression="Name">
                        <ItemTemplate>
                            <asp:Label ID="lblDescription" runat="server" CssClass="Normal" Text='<%# GetDescription(DataBinder.Eval(Container.DataItem,"Description")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Author" HeaderText="DisplayName" SortExpression="Author" ItemStyle-CssClass="Normal" />
                    <asp:BoundField DataField="CreatedDate" HeaderText="CreatedDate" SortExpression="CreatedDate" ItemStyle-CssClass="Normal" />
                    <asp:BoundField DataField="LastUpdated" HeaderText="LastUpdated" SortExpression="LastUpdated" ItemStyle-CssClass="Normal" />
                     <asp:TemplateField ShowHeader="true"  HeaderText="">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlVersions" runat="server" CssClass="Normal" NavigateUrl='<%# GetVersionsUrl(DataBinder.Eval(Container.DataItem,"ItemId")) %>'
                                    Text='<%# GetLocalizedVersionText() %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField ShowHeader="true"  HeaderText="">
                        <ItemTemplate>
                               <asp:HyperLink ID="hlEdit" runat="server" CssClass="Normal" NavigateUrl='<%# GetArticleEditUrl(DataBinder.Eval(Container.DataItem,"ItemVersionId")) %>'
                                    Text='<%# GetLocalizedEditText() %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:label id="lblMessage" runat="server" CssClass="Subhead"></asp:label>
        <br />
        <asp:hyperlink id="lnkAddNewArticle" Runat="server" ResourceKey="lnkAddNewArticle" CssClass="CommandButton"></asp:hyperlink>
        <div style="text-align:center;">
            <asp:linkbutton cssclass="CommandButton" id="cmdApprove" resourcekey="cmdApprove" runat="server" text="Approve Articles" causesvalidation="False" OnClick="cmdApprove_Click"></asp:linkbutton>
            <asp:linkbutton cssclass="CommandButton" id="cmdArchive" resourcekey="cmdArchive" runat="server" text="Archive Articles" causesvalidation="False" OnClick="cmdArchive_Click"></asp:linkbutton>
            <asp:linkbutton cssclass="CommandButton" id="cmdDelete" resourcekey="cmdDelete" runat="server" text="Delete Articles" causesvalidation="False" OnClick="cmdDelete_Click"></asp:linkbutton>
            <asp:linkbutton cssclass="CommandButton" id="cmdBack" resourcekey="cmdBack" runat="server" text="Back" causesvalidation="False" OnClick="cmdBack_Click"></asp:linkbutton>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
