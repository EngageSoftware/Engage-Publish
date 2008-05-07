//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.UI.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;

using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.Admin
{
    public partial class Syndication : ModuleBase, IActionable
	{
			

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);

		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		#region Event Handlers

		private void Page_Load(object sender, System.EventArgs e)
		{
			try 
			{
				//check VI for null then set information
				if (!Page.IsPostBack)
				{
                    LoadSettings();
				}
				
			} 
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		#endregion

        private void LoadSettings()
        {
            lblAuthorizationKey.Text = PortalController.GetCurrentPortalSettings().GUID.ToString();
            lblServiceUrl.Text = Utility.WebServiceUrl;

            //PublishSyndicationModes mode = (PublishSyndicationModes) Enum.Parse(typeof(PublishSyndicationModes), s);
            //if (mode == PublishSyndicationModes.Publisher)
            //{
            //    rbPublisher.Checked = true;
            //    rbSubscriber.Checked = false;
            //}
            //else
            //{
            //    rbSubscriber.Checked = true;
            //    rbPublisher.Checked = false;
            //}

            string s = HostSettings.GetHostSetting(Utility.PublishAutoArchiveContent + PortalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                chkArchiveContent.Checked = Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            s = HostSettings.GetHostSetting(Utility.PublishAutoApproveContent + PortalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                chkApproveContent.Checked = Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            s = HostSettings.GetHostSetting(Utility.PublishSubscriberKey + PortalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                txtAuthorizationKey.Text = s.ToString(CultureInfo.InvariantCulture);
            }
            s = HostSettings.GetHostSetting(Utility.PublishSubscriberUrl + PortalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                txtSubscriberServiceUrl.Text = s.ToString(CultureInfo.InvariantCulture);
            }           
        }
		#region Optional Interfaces

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions 
		{
			get 
			{
				DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
				Actions.Add(GetNextActionID(), Localization.GetString(DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", "", false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
				return Actions;
			}
		}

		#endregion

        protected void cmdUpdate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                HostSettingsController settingsController = new HostSettingsController();

                //subscriber settings
                //if (rbSubscriber.Checked)
                //{
                settingsController.UpdateHostSetting(Utility.PublishAutoArchiveContent + PortalId.ToString(CultureInfo.InvariantCulture), chkArchiveContent.Checked.ToString(CultureInfo.InvariantCulture));
                settingsController.UpdateHostSetting(Utility.PublishAutoApproveContent + PortalId.ToString(CultureInfo.InvariantCulture), chkApproveContent.Checked.ToString(CultureInfo.InvariantCulture));
                settingsController.UpdateHostSetting(Utility.PublishSubscriberKey + PortalId.ToString(CultureInfo.InvariantCulture), txtAuthorizationKey.Text.ToString(CultureInfo.InvariantCulture));
                settingsController.UpdateHostSetting(Utility.PublishSubscriberUrl + PortalId.ToString(CultureInfo.InvariantCulture), txtSubscriberServiceUrl.Text.ToString(CultureInfo.InvariantCulture));
                //settingsController.UpdateHostSetting(Utility.PublishSyndicationMode + PortalId.ToString(CultureInfo.InvariantCulture), PublishSyndicationModes.Subscriber.ToString(CultureInfo.InvariantCulture));
                //}
                //else
                //{
                //    settingsController.UpdateHostSetting(Utility.PublishSyndicationMode + PortalId.ToString(CultureInfo.InvariantCulture), PublishSyndicationModes.Publisher.ToString(CultureInfo.InvariantCulture));
                //}

                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(TabId, Utility.AdminContainer, "&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture)));
            }

        }

     
	}
}

