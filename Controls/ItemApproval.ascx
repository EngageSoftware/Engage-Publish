<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.ItemApproval" Codebehind="ItemApproval.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>
<div>
    <%--<dnn:sectionhead id="dshApprovalStatus" cssclass="Head" runat="server" text="Approval Status" section="divApprovalStatus" resourcekey="ApprovalStatus" isexpanded="True" />--%>
    <div id="divApprovalStatus" runat="server" class="Normal" visible="false">
	    <asp:RadioButtonList Runat="server" ID="radApprovalStatus" RepeatDirection="Vertical" CssClass="Normal"/>
    </div>
    <div id="divSubmitForApproval" runat="server" visible="false">
	    <asp:CheckBox ID="chkSubmitForApproval" Runat="server"/>
    </div>
</div>

