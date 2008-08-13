<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.CommentDisplay" Codebehind="CommentDisplay.ascx.cs" %>

<div id="divComment" class="Normal">

<asp:Label ID="lblNoComments" runat="server" resourcekey="lblNoComments" CssClass="PublishNoComments"></asp:Label>
<asp:UpdatePanel ID="upnlCommentDisplay" runat="server" UpdateMode="Always">
    <ContentTemplate>
    <asp:DataList ID="dlCommentText" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
        <AlternatingItemStyle CssClass="PublishComment PublishCommentAlternate" />
        <ItemStyle CssClass="PublishComment" />
        <ItemTemplate>
           <div>
                <div id="CommentValue"><asp:label ID="lblCommentText" Runat="server"><%#Eval("CommentText") %></asp:label></div>
                <div id="CommentName">
                    <asp:Label id="lblName" runat="server" resourcekey="lblName"></asp:Label>
                    <asp:Label ID="lblFirstName" runat="server"><%#Eval("FirstName") %></asp:Label>
                    <asp:Label ID="lblLastName" runat="server"><%#Eval("LastName") %></asp:Label>
                </div>
                <div id="CommentCreatedDate"><asp:Label ID="lblCreatedDate" runat="server"><%#Eval("CreatedDate") %></asp:Label></div>
            </div>
        </ItemTemplate>
    </asp:DataList>
    <div id="divPager" runat="server" class="commentPager">
        <asp:LinkButton ID="btnPrevious" runat="server" ResourceKey="btnPrev" CssClass="commentPrev" CausesValidation="false" /><%--OnClick="btnPrevious_Click" --%>
        <asp:LinkButton ID="btnNext" runat="server" ResourceKey="btnNext" OnClick="btnNext_Click" CssClass="commentNext" CausesValidation="false"/>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</div>