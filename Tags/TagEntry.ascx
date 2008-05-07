<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Tags.TagEntry" Codebehind="TagEntry.ascx.cs" %>
<%@ Register Assembly="AjaxControlToolkit" TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" %>
    
<asp:TextBox ID="txtTags" runat="server" cssclass="publishTextBoxWidth" ></asp:TextBox>

<ajaxToolkit:AutoCompleteExtender runat="server" 
    ID="acTags" 
    TargetControlID="txtTags"
    ServiceMethod="GetTagsCompletionList"
    ServicePath="~/DesktopModules/EngagePublish/Services/PublishServices.asmx"
    MinimumPrefixLength = "1"
    DelimiterCharacters=";"
    
    >

</ajaxToolkit:AutoCompleteExtender>
