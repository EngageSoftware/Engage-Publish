	<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.CategoryControls.CategoryDisplay" Codebehind="CategoryDisplay.ascx.cs" %>
	<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<div class="Normal">
    <asp:Literal ID="lblNoData" runat="server" Visible="false"/>
    <div class="divItemsListing">
	    <asp:DataList ID="dlCategories" Runat="server" Width="100%" RepeatLayout="Flow">
		    <ItemTemplate>
			    <div class="categoryThumbnail">
				    <asp:HyperLink ID="lnkThumbnail" Runat="server" CssClass="thumbnail" /></div>
			    <div class="categoryName">
				    <asp:HyperLink ID="lnkName" Runat="server" CssClass="link" /><%#Eval("Name") %></div>
			    <div class="categoryItem">
			    <asp:DataList ID="dlChildItems" runat="server">
			        <ItemTemplate>
    		            <div class="categoryItemList">
                            <div class="itemThumbnail">
                                <asp:HyperLink runat="server" ID="lnkThumbnail" NavigateUrl="" />
                            </div>
                            <div class="itemTitle">
                                <asp:HyperLink runat="server" ID="lnkTitle" NavigateUrl="">
                                    <%#Eval("Name") %>
                                </asp:HyperLink>
                            </div>
                            <div class="itemDescription">
                                <asp:Literal runat="server" ID="lblDescription" />
                            </div>
                        </div>
			        </ItemTemplate>
			    </asp:DataList>
			    </div>
		    </ItemTemplate>
	    </asp:DataList>
	    <asp:DataList ID="dlItems" Runat="server" Width="100%" RepeatLayout="Table" Visible="False">
		    <ItemTemplate>
		        <div class="categoryItemList">

		                <div class="itemThumbnail">
    	                    <asp:HyperLink runat="server" ID="lnkThumbnail" NavigateUrl="" />
    	                </div>
		                <div class="itemTitle">
		                    <asp:HyperLink runat="server" ID="lnkTitle" NavigateUrl="">
		                        <%#Eval("Name") %>
		                    </asp:HyperLink>
		                </div>
		                <div class="itemDescription">
		                    <asp:Literal runat="server" ID="lblDescription" />
                        </div>
                </div>
		    </ItemTemplate>
	    </asp:DataList>
    </div>
</div>

