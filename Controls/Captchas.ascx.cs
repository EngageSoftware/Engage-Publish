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
    using System.Web.UI;

    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    /// <summary>The CAPTCHA protection for a Publish form</summary>
    public partial class Captchas : ModuleBase
    {
        /// <summary>Gets or sets the validation group.</summary>
        public string ValidationGroup
        {
            get { return this.StandardCaptcha.ValidationGroup; }
            set { this.InvisibleCaptcha.ValidationGroup = this.TimeoutCaptcha.ValidationGroup = this.StandardCaptcha.ValidationGroup = value; }
        }

        /// <summary>Raises the <see cref="Control.Init" /> event.</summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        /// <summary>Handles the <see cref="Control.Load" /> event of this control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.LocalizeValidators();
                this.EnableValidators();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>Localizes the validators.</summary>
        private void LocalizeValidators()
        {
            this.InvisibleCaptcha.ErrorMessage = Localization.GetString("InvisibleCaptchaFailed", this.LocalResourceFile);
            this.InvisibleCaptcha.InvisibleTextBoxLabel = Localization.GetString("InvisibleCaptchaLabel", this.LocalResourceFile);
            this.TimeoutCaptcha.ErrorMessage = Localization.GetString("TimeoutCaptchaFailed", this.LocalResourceFile);
            this.StandardCaptcha.ErrorMessage = Localization.GetString("StandardCaptchaFailed", this.LocalResourceFile);
            this.StandardCaptcha.CaptchaLinkButtonText = Localization.GetString("StandardCaptchaLink", this.LocalResourceFile);
            this.StandardCaptcha.CaptchaTextBoxLabel = Localization.GetString("StandardCaptchaLabel", this.LocalResourceFile);
        }

        /// <summary>Enables the validators, based on the Publish settings for this site.</summary>
        private void EnableValidators()
        {
            this.InvisibleCaptcha.Visible = this.InvisibleCaptcha.Enabled = Utility.GetBooleanPortalSetting(Utility.PublishEnableInvisibleCaptcha, this.PortalId, true);
            this.TimeoutCaptcha.Visible = this.TimeoutCaptcha.Enabled = Utility.GetBooleanPortalSetting(Utility.PublishEnableTimedCaptcha, this.PortalId, true);
            this.StandardCaptcha.Visible = this.StandardCaptcha.Enabled = Utility.GetBooleanPortalSetting(Utility.PublishEnableStandardCaptcha, this.PortalId, false);
            this.StandardCaptcha.CaptchaImage.EnableCaptchaAudio = SecurityPolicy.HasAspNetHostingPermission();
        }
    }
}