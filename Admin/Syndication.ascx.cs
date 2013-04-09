//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Admin
{
    using System;
    using System.Globalization;

    using DotNetNuke.Common;
    using DotNetNuke.Entities.Controllers;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    public partial class Syndication : ModuleBase, IActionable
    {
        public ModuleActionCollection ModuleActions
        {
            get
            {
                return new ModuleActionCollection
                    {
                        {
                            this.GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile), 
                            ModuleActionType.AddContent, string.Empty, string.Empty, string.Empty, false, SecurityAccessLevel.Edit, true, false
                            }
                    };
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            if (this.Page.IsValid)
            {
                var settingsController = HostController.Instance;

                // subscriber settings
                // if (rbSubscriber.Checked)
                // {
                settingsController.Update(
                    Utility.PublishAutoArchiveContent + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkArchiveContent.Checked.ToString(CultureInfo.InvariantCulture));
                settingsController.Update(
                    Utility.PublishAutoApproveContent + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkApproveContent.Checked.ToString(CultureInfo.InvariantCulture));
                settingsController.Update(
                    Utility.PublishSubscriberKey + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.txtAuthorizationKey.Text.ToString(CultureInfo.InvariantCulture));
                settingsController.Update(
                    Utility.PublishSubscriberUrl + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.txtSubscriberServiceUrl.Text.ToString(CultureInfo.InvariantCulture));

                // settingsController.UpdateHostSetting(Utility.PublishSyndicationMode + PortalId.ToString(CultureInfo.InvariantCulture), PublishSyndicationModes.Subscriber.ToString(CultureInfo.InvariantCulture));
                // }
                // else
                // {
                // settingsController.UpdateHostSetting(Utility.PublishSyndicationMode + PortalId.ToString(CultureInfo.InvariantCulture), PublishSyndicationModes.Publisher.ToString(CultureInfo.InvariantCulture));
                // }
                this.Response.Redirect(
                    Globals.NavigateURL(this.TabId, Utility.AdminContainer, "&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture)));
            }
        }

        private void LoadSettings()
        {
            this.lblAuthorizationKey.Text = PortalController.GetCurrentPortalSettings().GUID.ToString();
            this.lblServiceUrl.Text = Utility.WebServiceUrl;

            // PublishSyndicationModes mode = (PublishSyndicationModes) Enum.Parse(typeof(PublishSyndicationModes), s);
            // if (mode == PublishSyndicationModes.Publisher)
            // {
            // rbPublisher.Checked = true;
            // rbSubscriber.Checked = false;
            // }
            // else
            // {
            // rbSubscriber.Checked = true;
            // rbPublisher.Checked = false;
            // }
            var hostController = HostController.Instance;
            string s = hostController.GetString(Utility.PublishAutoArchiveContent + this.PortalId.ToString(CultureInfo.InvariantCulture));
            if (Engage.Utility.HasValue(s))
            {
                this.chkArchiveContent.Checked = Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }

            s = hostController.GetString(Utility.PublishAutoApproveContent + this.PortalId.ToString(CultureInfo.InvariantCulture));
            if (Engage.Utility.HasValue(s))
            {
                this.chkApproveContent.Checked = Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }

            s = hostController.GetString(Utility.PublishSubscriberKey + this.PortalId.ToString(CultureInfo.InvariantCulture));
            if (Engage.Utility.HasValue(s))
            {
                this.txtAuthorizationKey.Text = s.ToString(CultureInfo.InvariantCulture);
            }

            s = hostController.GetString(Utility.PublishSubscriberUrl + this.PortalId.ToString(CultureInfo.InvariantCulture));
            if (Engage.Utility.HasValue(s))
            {
                this.txtSubscriberServiceUrl.Text = s.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // check VI for null then set information
                if (!this.Page.IsPostBack)
                {
                    this.LoadSettings();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}