<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.CommentDisplay" Codebehind="CommentDisplay.ascx.cs" %>

<div id="divComment" class="Normal">

<asp:Label ID="lblNoComments" runat="server" resourcekey="lblNoComments" CssClass="PublishNoComments"></asp:Label>
    <asp:UpdatePanel ID="upnlCommentDisplay" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:repeater id="dlCommentText" runat="server">
                <headertemplate/>
                <ItemTemplate>
                   <div class="PublishComment PublishCommentWrapper">
                        <div class="CommentValue"><asp:label ID="lblCommentText" Runat="server"><%# Eval("CommentText") %></asp:label></div>
                        <div class="CommentNameDate"><asp:Label ID="lblCommentNameDate" runat="server"><%# BuildCommentNameDate(Eval("FirstName"), Eval("LastName"),Eval("Url"),Eval("CreatedDate")) %></asp:Label></div>
                    </div>
                </ItemTemplate>
                <AlternatingItemTemplate>
                   <div class="PublishComment PublishCommentAlternate">
                        <div class="CommentValue"><asp:label ID="lblCommentText" Runat="server"><%# Eval("CommentText") %></asp:label></div>
                        <div class="CommentNameDate"><asp:Label ID="lblCommentNameDate" runat="server"><%# BuildCommentNameDate(Eval("FirstName"), Eval("LastName"),Eval("Url"),Eval("CreatedDate")) %></asp:Label></div>
                    </div>
                </AlternatingItemTemplate>
            </asp:repeater>
            <div id="divPager" runat="server" class="commentPager">
                <asp:LinkButton ID="btnPrevious" runat="server" ResourceKey="btnPrev" CssClass="commentPrev" CausesValidation="false" /><%--OnClick="btnPrevious_Click" --%>
                <asp:LinkButton ID="btnNext" runat="server" ResourceKey="btnNext" OnClick="btnNext_Click" CssClass="commentNext" CausesValidation="false"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>