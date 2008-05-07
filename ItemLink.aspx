<%@ Page Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.ItemLink" Codebehind="ItemLink.aspx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>

<html>
<head>
<meta http-equiv="refresh"  runat="server" id="metaRefresh" />
</head>
<body>
    <asp:Label ID="lblTitle" ResourceKey="lblTitle" runat="server" Text="Module Incorrectly Configured" CssClass= "Head"></asp:Label>
    <br /><br />
    <div style="text-align=left;">
    <asp:Label ID="lblMessage" ResourceKey="lblMessage" runat="server" Text="There is a problem with the configuration for this module." CssClass="SubHead"></asp:Label>
    <br />
    <asp:Label ID="lblPossible" ResourceKey="lblPossible" runat="server" Text ="Possible Causes:" CssClass ="SubHead"></asp:Label>
    <br />
    <asp:Label ID="lblPossibleA" ResourceKey="lblPossibleA" runat="server" Text="1) The module does not have a Display Page defined." CssClass ="SubHead"></asp:Label>
    <br />
    <asp:Label ID="lblPossibleB" ResourceKey="lblPossibleB" runat="server" Text="2) The module is configured to point to an Article or Category that does not exist any more." CssClass ="SubHead"></asp:Label>
    <br />
    <br />
    <asp:Label ID="lblFooter" ResourceKey="lblFooter" runat="server" Text="Please contact your System Administrator with this problem." CssClass ="SubHead"></asp:Label>
    </div>
</body>
</html>
