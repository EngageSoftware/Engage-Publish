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
using System.Data;
using System.Globalization;
using System.Web;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Security.Roles;
using DotNetNuke.Services.Search;
using DotNetNuke.Entities.Users;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.Security
{
	/// <summary>
	/// Summary description for SecurityFilter.
	/// </summary>
	public abstract class SecurityFilter
	{
		//private static SecurityFilter instance;
		private const string SecuritySettingName = "EnablePermissionsForPortal";


		public abstract void FilterCategories(DataTable data);
		public abstract void FilterArticles(SearchResultsInfoCollection data);

        //TODO: should this still exist?
		private static bool IsAdmin
		{
			get 
			{
				if (HttpContext.Current.Request.IsAuthenticated)
				{
					return (UserController.GetCurrentUserInfo().IsSuperUser || IsUserInRole("Administrators"));
				}
				else
				{
					return false;
				}
			}
		}

		private static bool IsUserInRole(string roleName)
		{
			UserInfo ui = UserController.GetCurrentUserInfo();
			RoleController rc = new RoleController();
			string[] roles = rc.GetRolesByUser(ui.UserID, ui.PortalID);
			foreach (string role in roles)
			{
				if (roleName == role) return true;
			}
	
			return false;
		}

		private static bool IsSecurityEnabled()
		{
			//this is an issue with the dnn searching, may need to look at this again
			if (HttpContext.Current == null) return false;

			//check the portal setting
			int portalId = PortalController.GetCurrentPortalSettings().PortalId;
			string s = HostSettings.GetHostSetting(SecuritySettingName + portalId);
            if (Utility.HasValue(s))
            {
                //if security is off.....it's off
                bool enable = Convert.ToBoolean(s, CultureInfo.InvariantCulture);
                if (enable == false) return false;
            }
            else
            {
                //no setting exists. TURN OFF!!!!
                return false;
            }

			//check if user is Admin, if so, no security
			if (IsAdmin) return false;

			//otherwise it's on
			return true;
		}

		private static SecurityFilter CreateFilter()
		{
			return (IsSecurityEnabled() ? RoleBasedSecurityFilter.Instance : NullSecurityFilter.Instance);
		}

		public static SecurityFilter Instance
		{
			get
			{
				return CreateFilter();

//				if (instance == null)
//				{
//					lock (typeof(SecurityFilter))
//					{
//						if (instance == null)
//						{
//							instance = CreateFilter();
//						}
//					}
//				}
//
//				return instance;
			}
		}

		public static bool IsSecurityEnabled(int portalId)
		{
			string s = HostSettings.GetHostSetting(SecuritySettingName + portalId);
			//Hashtable ht = PortalSettings.GetSiteSettings(portalId);
			//string s = Convert.ToString(ht[SecuritySettingName]);

			return (Utility.HasValue(s) ? Convert.ToBoolean(s, CultureInfo.InvariantCulture) : false);
		}

		public static void EnableSecurity(bool enabled, int portalId)
		{
			HostSettingsController c = new HostSettingsController();
			c.UpdateHostSetting(SecuritySettingName + portalId, enabled.ToString());

			//PortalSettings.UpdatePortalSetting(portalId, SecuritySettingName, enabled.ToString());
		}
	}
}

