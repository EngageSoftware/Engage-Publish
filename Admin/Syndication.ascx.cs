//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
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
    using DotNetNuke.Entities.Host;
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
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            InitializeComponent();
            base.OnInit(e);
        }

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            if (this.Page.IsValid)
            {
                var settingsController = new HostSettingsController();

                // subscriber settings
                // if (rbSubscriber.Checked)
                // {
                settingsController.UpdateHostSetting(
                    Utility.PublishAutoArchiveContent + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkArchiveContent.Checked.ToString(CultureInfo.InvariantCulture));
                settingsController.UpdateHostSetting(
                    Utility.PublishAutoApproveContent + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.chkApproveContent.Checked.ToString(CultureInfo.InvariantCulture));
                settingsController.UpdateHostSetting(
                    Utility.PublishSubscriberKey + this.PortalId.ToString(CultureInfo.InvariantCulture), 
                    this.txtAuthorizationKey.Text.ToString(CultureInfo.InvariantCulture));
                settingsController.UpdateHostSetting(
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

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new EventHandler(this.Page_Load);
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
            string s = HostSettings.GetHostSetting(Utility.PublishAutoArchiveContent + this.PortalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                this.chkArchiveContent.Checked = Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }

            s = HostSettings.GetHostSetting(Utility.PublishAutoApproveContent + this.PortalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                this.chkApproveContent.Checked = Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }

            s = HostSettings.GetHostSetting(Utility.PublishSubscriberKey + this.PortalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                this.txtAuthorizationKey.Text = s.ToString(CultureInfo.InvariantCulture);
            }

            s = HostSettings.GetHostSetting(Utility.PublishSubscriberUrl + this.PortalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
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