<%@ Page Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.PrinterFriendly" Codebehind="PrinterFriendly.aspx.cs" %>
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>	    
	    <title><asp:Literal id="pageTitle" runat="server" /></title>
		<link rel="stylesheet" type="text/css" href="<%= GetCssStyle() %>" />
		<style type="text/css">
		    @media print
		    {
		        #articlePrint
		        {
		            display: none;
		        }
		    }
		</style>
	</head>
	<body id="printerFriendlyBody">
	    <div id="articlePrint">
	        <input type="button" onclick="javascript:print();return false;" value='<%=DotNetNuke.Services.Localization.Localization.GetString("Print", LocalResourceFile) %>' />
	    </div>
	    <div id="ArticleDisplay">
		    <div id="divPortalLogo">
		        <asp:Hyperlink ID="lnkPortalLogo" Runat="server"/>
		    </div>
		    <div id="divArticleTitle" class="Head">
			    <asp:label ID="lblArticleTitle" Runat="server"/>
		    </div>
		    <div id="divArticleContent" class="Normal">
			    <asp:label ID="lblArticleText" Runat="server"/>
		    </div>
	    </div>
	</body>
</html>

