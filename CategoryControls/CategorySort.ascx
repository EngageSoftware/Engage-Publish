<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.CategoryControls.CategorySort" CodeBehind="CategorySort.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="sectionhead" Src="~/controls/sectionheadcontrol.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>

<asp:Label ID="lblCategory" runat="server" CssClass="Head"></asp:Label>
<hr />
<dnn:sectionhead ID="shPublishInstructions" CssClass="Head" runat="server" Section="publishInstructions" ResourceKey="shPublishInstructions" IsExpanded="False" />
<hr />
<div id="publishInstructions" runat="server" class="instructions">
    <asp:Label ID="lblPublishInstructions" runat="server" resourcekey="lblPublishInstructions" CssClass="Normal"></asp:Label>
</div>
<asp:UpdatePanel ID="upnlPublish" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlSortList" runat="server">
            <div id="Publish_CategorySortInitialListDiv">
                <dnn:Label ID="lblCategoryItems" runat="server" />
                <asp:ListBox ID="lbCategoryItems" CssClass="Publish_CategorySortUnsortedList" runat="server" DataTextField="ChildName" DataValueField="ItemRelationshipId" SelectionMode="single"></asp:ListBox>
                <br />
                <asp:LinkButton ID="lbMoveToSort" runat="server" resourcekey="lbMoveToSort" OnClick="lbMoveToSort_Click"></asp:LinkButton>
            </div>
            <div id="Publish_CategorySortSortedListDiv">
                <dnn:Label ID="lblSortedItems" runat="server" />
                <ajaxToolkit:ReorderList ID="rlCategorySort" runat="server" DragHandleAlignment="Left" ItemInsertLocation="End" DataKeyField="ItemRelationshipId" SortOrderField="SortOrder" AllowReorder="true" OnItemReorder="rlCategorySort_Reorder" PostBackOnReorder="true" CssClass="Publish_CategorySortedList" OnDeleteCommand="DeleteItem">
                    <ItemTemplate>
                        <asp:Label ID="lblItemRelationshipId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container.DataItem,"itemRelationshipId") %>'></asp:Label>
                        <div class="Publish_CategorySortDelete">
                            <asp:ImageButton ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem,"itemRelationshipId") %>' ImageUrl="~/images/delete.gif" />
                        </div>
                    </ItemTemplate>
                    <DragHandleTemplate>
                        <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Name") %>' CssClass="Publish_SortItem"></asp:Label>
                    </DragHandleTemplate>
                    <ReorderTemplate>
                        <div class="Publish_CategorySortItemDrop">
                            <br />
                        </div>
                    </ReorderTemplate>
                    <EmptyListTemplate>
                        <asp:Label ID="lblEmptyList" runat="server" resourcekey="lblEmptyList"></asp:Label>
                    </EmptyListTemplate>
                </ajaxToolkit:ReorderList>
            </div>
        </asp:Panel>
        <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
    </ContentTemplate>
</asp:UpdatePanel>
<div id="publishSortSave">
    <asp:LinkButton ID="lbSaveSort" runat="server" resourcekey="lbSaveSort" OnClick="lbSaveSort_Click"></asp:LinkButton>
    <asp:LinkButton ID="lbCancel" runat="server" resourcekey="lbCancel" OnClick="lbCancel_Click"></asp:LinkButton>
</div>
