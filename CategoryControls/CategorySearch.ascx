<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.CategoryControls.CategorySearch" Codebehind="CategorySearch.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<div class="Normal"> 
    <div id="divSearchField" runat="server" class="divSearchField">
	    <div id="divSearchSpacer" ></div>
	    <div id="divSearchCategorySelection" runat="server" class="divSearchCategorySelection">
		    <asp:label ResourceKey="lblCategoryList" Text="Choose a Category" ID="lblCategoryList" runat="server" CssClass="searchCategorySelectLabel" />
		    <asp:DropDownList ID="ddlCategoryList" Runat="server" cssclass="searchCategorySelect"></asp:DropDownList>
	    </div>
	    <div id="divSearchInputs">
	        <div id="searchInputLabel" class="searchInputLabel"><dnn:label ResourceKey="lblCategorySearch" ID="lblCategorySearch" runat="server" /></div>
		    <asp:TextBox ID="txtCategorySearch" resourcekey="txtCategorySearch" Runat="server" OnClick="this.value='';"
			    CssClass="hi_searchbar_field"></asp:TextBox>
	    </div>
	    <div id="divSearchButton"><asp:LinkButton ID="btnCategorySearch" Runat="server" ResourceKey="btnCategorySearch" cssclass="btnCategorySearch"></asp:LinkButton></div>
    </div>
    <div id="divSearchResults" runat="server" visible="false" class="categorySearchResults">
        <asp:Label ID="lblNoResults" runat="server" resourcekey="lblNoResults" Visible="false" CssClass="subHead" />
	    <asp:Datagrid id="dgResults" runat="server" AutoGenerateColumns="False" AllowPaging="True" BorderStyle="None"
		    PagerStyle-CssClass="NormalBold" ShowHeader="False" CellPadding="4" GridLines="None">
		    <Columns>
			    <asp:TemplateColumn>
				    <ItemTemplate>
					    <asp:Label id="lblNo" runat="server" Text='<%# Convert.ToInt32(DataBinder.Eval(Container, "ItemIndex")) + 1 %>' CssClass="SubHead">
					    </asp:Label>
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn>
				    <ItemTemplate>
					    <asp:HyperLink id="lnkTitle" runat="server" CssClass="SubHead" NavigateUrl='<%# FormatUrl(Convert.ToInt32(DataBinder.Eval(Container.DataItem,"TabId")),DataBinder.Eval(Container.DataItem,"Guid").ToString()) %>' Text='<%# DataBinder.Eval(Container.DataItem, "Title").ToString() %>'>
					    </asp:HyperLink>&nbsp;
					    <asp:Label id="lblRelevance" runat="server" CssClass="Normal" Text='<%# FormatRelevance(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Relevance"))) %>' >
					    </asp:Label><br />
					    <asp:Label id="lblSummary" runat="server" CssClass="Normal" Text='<%# DataBinder.Eval(Container.DataItem, "Description").ToString() + "<br />" %>' Visible="<%# ShowDescription() %>">
					    </asp:Label>
					    <!--					<asp:HyperLink id="lnkLink" runat="server" CssClass="CommandButton" NavigateUrl='<%# FormatUrl(Convert.ToInt32(DataBinder.Eval(Container.DataItem,"TabId")),DataBinder.Eval(Container.DataItem,"Guid").ToString()) %>' Text='<%# FormatUrl(Convert.ToInt32(DataBinder.Eval(Container.DataItem,"TabId")),DataBinder.Eval(Container.DataItem,"Guid").ToString()) %>'>
					    </asp:HyperLink>&nbsp;- -->
					    <asp:Label id="lblPubDate" runat="server" CssClass="Normal" Text='<%# FormatDate(DataBinder.Eval(Container.DataItem, "PubDate").ToString()) %>'>
					    </asp:Label>
				    </ItemTemplate>
			    </asp:TemplateColumn>
		    </Columns>
		    <PagerStyle CssClass="NormalBold" Mode="NumericPages"></PagerStyle>
	    </asp:Datagrid>
    </div>
</div>