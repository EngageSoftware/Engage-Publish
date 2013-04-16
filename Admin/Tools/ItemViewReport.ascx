<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Admin.Tools.ItemViewReport" Codebehind="ItemViewReport.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/labelControl.ascx" %>
<%@ Register TagPrefix="ajaxToolkit" Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" %>

<div class="Normal">
	<dnn:Label ID="lblControlTitle" ResourceKey="lblControlTitle" Runat="server" cssclass="Head"></dnn:Label>
	<hr />
	<asp:Label ID="lblMessage" runat="server" resourcekey="lblMessage"></asp:Label><br />
	
	<asp:Panel ID="pnlReportOptions" runat="server" CssClass="Publish_ItemViewReport">
	    <div class="Publish_ItemViewReportOptionLabel">
	        <dnn:Label ID="lblItemType" runat="server" />
	        <asp:Label ID="lblPortalId" runat="server" Visible="False"></asp:Label>
	    </div>
	    <div class="Publish_ItemViewReportOption">
	        <asp:DropDownList ID="ddlItemType" runat="server"></asp:DropDownList>
	    </div>  
	    <div class="Publish_ItemViewReportOptionLabel">
	        <dnn:Label ID="lblPageSize" runat="server" />
	    </div>
        <div class="Publish_ItemViewReportOption">
	        <asp:DropDownList ID="ddlPageNumbers" runat="server">
	            <asp:ListItem Value="10"></asp:ListItem>
	            <asp:ListItem Value="20"></asp:ListItem>
	            <asp:ListItem Value="30"></asp:ListItem>
	            <asp:ListItem Value="40"></asp:ListItem>
	            <asp:ListItem Value="50"></asp:ListItem>
	            <asp:ListItem Value="100"></asp:ListItem>
	        </asp:DropDownList>
	    </div>  
        <div class="Publish_ItemViewReportOptionLabel">
	        <dnn:Label ID="lblStartDate" runat="server" />
	    </div>
        <div class="Publish_ItemViewReportOption">
	        <asp:TextBox ID="txtStartDate" runat="server" CssClass="Publish_ItemViewReportCalendar"></asp:TextBox>
	        <asp:Image ID="imgStartCalendarIcon" runat="server" ImageUrl="~/desktopModules/EngagePublish/images/calendar.png" />
                <ajaxToolkit:CalendarExtender ID="ceStartDate" runat="server" TargetControlID="txtStartDate" PopupButtonID="imgStartCalendarIcon" />
	    </div>  
        <div class="Publish_ItemViewReportOptionLabel">
	        <dnn:Label ID="lblEndDate" runat="server" />
	    </div>
        <div class="Publish_ItemViewReportOption">
	        <asp:TextBox ID="txtEndDate" runat="server" CssClass="Publish_ItemViewReportCalendar"></asp:TextBox>
	        <asp:Image ID="imgEndCalendarIcon" runat="server" ImageUrl="~/desktopModules/EngagePublish/images/calendar.png" />
                <ajaxToolkit:CalendarExtender ID="ceEndDate" runat="server" TargetControlID="txtEndDate" PopupButtonID="imgEndCalendarIcon" />
	    </div>  
	    <div class="Publish_ItemViewReportOption">
	        <asp:LinkButton ID="lnkGenerate" runat="server" resourcekey="lnkGenerate" OnClick="lnkGenerate_Click"></asp:LinkButton>
	    </div>

	</asp:Panel>
	<asp:Panel ID="pnlReport" runat="server" CssClass="DataGrid_Container">
	    <asp:GridView runat="server" ID="gvReport" AutoGenerateColumns="false" >
	        <HeaderStyle />
	        <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
	        <RowStyle CssClass="DataGrid_Item" />
	        <AlternatingRowStyle CssClass="DataGrid_Item DataGrid_AlternateItem" />
	        <Columns>
	            <asp:BoundField DataField="ItemId" />
	            <asp:BoundField DataField="Name" />
	            <asp:BoundField DataField="Count" />
	        </Columns>
	    </asp:GridView>
    	
	    <asp:ObjectDataSource ID="odsItemViewReport"
	        SelectMethod="GetItemViewsPage" 
	        SelectCountMethod="GetItemViewsPageCount" 
	        TypeName="Engage.Dnn.Publish.Admin.Tools.ItemViewReportObject" 
	        runat="server" 
	        EnablePaging="True" 
	        MaximumRowsParameterName="PageSize" 
	        StartRowIndexParameterName="PageIndex">
	        <SelectParameters>
	            <asp:Parameter DefaultValue="0" Name="PageIndex" Type="int32" />
                <asp:ControlParameter 
                    ControlID="ddlItemType" 
                    DefaultValue="-1" 
                    Name="ItemTypeId"
                    PropertyName="SelectedValue" Type="int32" />
                <asp:ControlParameter 
                    ControlID="ddlPageNumbers" 
                    DefaultValue="10" 
                    Name="PageSize"
                    PropertyName="SelectedValue" 
                    Type="int32" />
                <asp:ControlParameter 
                    ControlID="txtStartDate" 
                    DefaultValue="1/1/2008" 
                    Name="StartDate" 
                    PropertyName="Text" 
                    Type="string" />
                <asp:ControlParameter 
                    ControlID="txtEndDate" 
                    DefaultValue="1/1/2008" 
                    Name="EndDate" 
                    PropertyName="Text" 
                    Type="string" />
                <asp:ControlParameter
                    ControlID="lblPortalId"
                    DefaultValue="0" 
                    Name="PortalId"
                    PropertyName="Text" 
                    Type="int32" />
	        </SelectParameters>
	    </asp:ObjectDataSource>
	</asp:Panel>	
</div>

