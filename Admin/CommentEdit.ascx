<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Admin.CommentEdit" Codebehind="CommentEdit.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<div id="CommentEdit" class="Normal">
	<div id="divCommentId" runat="server">
		<dnn:label id="lblCommentId" resourcekey="lblCommentId" Runat="server"></dnn:label>
		<asp:label id="txtCommentId" Runat="server"></asp:label>
	</div>
	<hr />
	<div id="divUser" runat="server">
	    <dnn:label ID="lblUserId" runat="server" /><asp:Label ID="txtUserId" runat="server" /><br />
	    <dnn:label ID="lblFirstName" runat="server" /><asp:TextBox ID="txtFirstName" runat="server" /><br />
	    <dnn:label ID="lblLastName" runat="server" /><asp:TextBox ID="txtLastName" runat="server" /><br />
	    <dnn:label ID="lblEmailAddress" runat="server" /><asp:TextBox ID="txtEmailAddress" runat="server" /><br />
	    <dnn:label ID="lblUrl" runat="server" /><asp:TextBox ID="txtUrl" runat="server" /><br />
	</div>
	<hr />
	<div id="divCommentText" runat="server"><asp:placeholder id="phCommentText" Runat="server"></asp:placeholder></div>
	<asp:textbox id="txtMessage" runat="server" Visible="False" ReadOnly="True" ForeColor="Red" EnableViewState="False"
		Columns="75" TextMode="MultiLine" Rows="5"></asp:textbox><br />
	<asp:PlaceHolder id="phApproval" runat="Server" />
	<hr />
	<asp:ValidationSummary id="ValidationSummary1" runat="server"></asp:ValidationSummary>
	<br />
    <asp:LinkButton ID="cmdUpdate" Runat="server" ResourceKey="cmdUpdate"></asp:LinkButton>&nbsp;&nbsp;
    <asp:LinkButton ID="cmdCancel" Runat="server" ResourceKey="cmdCancel" CausesValidation="False"></asp:LinkButton>&nbsp;&nbsp;
    <asp:LinkButton ID="cmdDelete" runat="server" resourceKey="cmdDelete" CausesValidation="False" Text="Delete" OnClick="cmdDelete_Click"></asp:LinkButton>
</div>

