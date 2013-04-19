// <copyright file="EmailAFriend.ascx.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Controls
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;
    using System.Web.UI;

    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Services.Mail;

    public partial class EmailAFriend : ModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            try
            {
                this.ClearCommentInput();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.Page.IsValid)
                {
                    return;
                }

                var message = new StringBuilder(this.Localize("EmailAFriend"))
                    .Replace("[Engage:Recipient]", this.txtTo.Text.Trim())
                    .Replace("[Engage:Url]", this.GetItemLinkUrlExternal(this.ItemId))
                    .Replace("[Engage:From]", this.txtFrom.Text.Trim())
                    .Replace("[Engage:Message]", this.txtMessage.Text.Trim())
                    .ToString();

                var subject = this.Localize("EmailAFriendSubject").Replace("[Engage:Portal]", this.PortalSettings.PortalName);

                Mail.SendEmail(this.PortalSettings.Email, this.txtTo.Text.Trim(), subject, message);
                this.ClearCommentInput();
                ScriptManager.RegisterStartupScript(this.Page, typeof(EmailAFriend), "Close SimpleModal", "jQuery.modal.close()", true);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void ClearCommentInput()
        {
            this.txtFrom.Text = string.Empty;
            this.txtMessage.Text = string.Empty;
            this.txtTo.Text = string.Empty;
            this.txtFrom.Text = this.UserInfo != null ? this.UserInfo.Email : string.Empty;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.AddJQueryReference();
                this.Page.ClientScript.RegisterClientScriptInclude("Engage_Publish_ModalPopup", this.ResolveUrl("../Scripts/ModalPopup.js"));
                this.EmailAFriendPopupTriggerLink.Attributes["data-modal-target-id"] = this.pnlEmailAFriend.ClientID;
                this.LocalizeValidators();
                this.SetValidationGroup();

                if (!this.IsPostBack)
                {
                    this.txtFrom.Text = this.UserInfo != null ? this.UserInfo.Email : string.Empty;
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void LocalizeValidators()
        {
            this.ValidationSummary.HeaderText = Localization.GetString("ValidationSummary.Header", this.LocalResourceFile);
            this.InvisibleCaptcha.ErrorMessage = Localization.GetString("InvisibleCaptchaFailed", this.LocalResourceFile);
            this.InvisibleCaptcha.InvisibleTextBoxLabel = Localization.GetString("InvisibleCaptchaLabel", this.LocalResourceFile);
            this.TimeoutCaptcha.ErrorMessage = Localization.GetString("TimeoutCaptchaFailed", this.LocalResourceFile);
            this.StandardCaptcha.ErrorMessage = Localization.GetString("StandardCaptchaFailed", this.LocalResourceFile);
            this.StandardCaptcha.CaptchaLinkButtonText = Localization.GetString("StandardCaptchaLink", this.LocalResourceFile);
            this.StandardCaptcha.CaptchaTextBoxLabel = Localization.GetString("StandardCaptchaLabel", this.LocalResourceFile);
        }

        private void SetValidationGroup()
        {
            var validationGroup = "ValidateGroupSend" + this.ModuleId.ToString(CultureInfo.InvariantCulture);
            this.ToRequiredValidator.ValidationGroup = validationGroup;
            this.FromRequiredValidator.ValidationGroup = validationGroup;
            this.InvisibleCaptcha.ValidationGroup = validationGroup;
            this.TimeoutCaptcha.ValidationGroup = validationGroup;
            this.StandardCaptcha.ValidationGroup = validationGroup;
            this.ValidationSummary.ValidationGroup = validationGroup;
            this.SendButton.ValidationGroup = validationGroup;
        }
    }
}