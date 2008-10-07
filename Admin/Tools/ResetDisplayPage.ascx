<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Admin.Tools.ResetDisplayPage" Codebehind="ResetDisplayPage.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<div class="Normal">
	<dnn:Label Runat="server" ResourceKey="ResetDisplayPageHeader" CssClass="Head" EnableViewState="false"/>
	<hr />
	
	<asp:UpdatePanel ID="CategoryDisplayPageUpdatePanel" runat="server" UpdateMode="Conditional">
	    <ContentTemplate>
	        <div>
	            <asp:Label runat="server" resourcekey="UpdateAllChildrenOf" EnableViewState="false"/> 
	            <asp:DropDownList ID="ParentCategoryDropDownList" runat="server" AutoPostBack="true" /> 
	            <asp:Label runat="server" resourcekey="ToTheFollowingDisplayPage" EnableViewState="false"/> 
	            <asp:Label ID="DisplayPageLabel" runat="server" CssClass="NormalBold" />
	        </div>
	        <div>
	            <asp:Label ID="SuccessMessage" runat="server" CssClass="NormalBold" EnableViewState="false" />
	        </div>
	    </ContentTemplate>
	</asp:UpdatePanel>
	<asp:LinkButton ID="ResetButton" runat="server" ResourceKey="ResetButton" EnableViewState="false" />
</div>

