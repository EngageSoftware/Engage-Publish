<%@ Control Language="c#" AutoEventWireup="false" Inherits="Engage.Dnn.Publish.Controls.Captchas" Codebehind="Captchas.ascx.cs" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>

<dnn:DnnCaptcha ID="InvisibleCaptcha" runat="server" ProtectionMode="InvisibleTextBox" Display="None" />
<dnn:DnnCaptcha ID="TimeoutCaptcha" runat="server" ProtectionMode="MinimumTimeout" Display="None" />
<dnn:DnnCaptcha ID="StandardCaptcha" runat="server" ProtectionMode="Captcha" Display="None" EnableRefreshImage="True" CssClass="publish-captcha" CaptchaImage-ImageCssClass="publish-captcha-image" CaptchaTextBoxCssClass="publish-catpcha-input" CaptchaTextBoxLabelCssClass="publish-captcha-label" />