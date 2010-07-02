//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Security
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web;

    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Security.Roles;
    using DotNetNuke.Services.Search;

    using Engage.Dnn.Publish.Util;

    /// <summary>
    /// Summary description for SecurityFilter.
    /// </summary>
    public abstract class SecurityFilter
    {
        // private static SecurityFilter instance;
        private const string SecuritySettingName = "EnablePermissionsForPortal";

        public static SecurityFilter Instance
        {
            get
            {
                return CreateFilter();

                // 				if (instance == null)
                // 				{
                // 					lock (typeof(SecurityFilter))
                // 					{
                // 						if (instance == null)
                // 						{
                // 							instance = CreateFilter();
                // 						}
                // 					}
                // 				}
                // 				return instance;
            }
        }

        private static bool IsAdmin
        {
            get
            {
                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    return UserController.GetCurrentUserInfo().IsSuperUser || IsUserInRole("Administrators");
                }

                return false;
            }
        }

        public static void EnableSecurity(bool enabled, int portalId)
        {
            var c = new HostSettingsController();
            c.UpdateHostSetting(SecuritySettingName + portalId, enabled.ToString());

            // PortalSettings.UpdatePortalSetting(portalId, SecuritySettingName, enabled.ToString());
        }

        public static bool IsSecurityEnabled(int portalId)
        {
            string s = HostSettings.GetHostSetting(SecuritySettingName + portalId);

            // Hashtable ht = PortalSettings.GetSiteSettings(portalId);
            // string s = Convert.ToString(ht[SecuritySettingName]);
            return Utility.HasValue(s) ? Convert.ToBoolean(s, CultureInfo.InvariantCulture) : false;
        }

        public abstract void FilterArticles(SearchResultsInfoCollection data);

        public abstract void FilterCategories(DataTable data);

        private static SecurityFilter CreateFilter()
        {
            return IsSecurityEnabled() ? RoleBasedSecurityFilter.Instance : NullSecurityFilter.Instance;
        }

        private static bool IsSecurityEnabled()
        {
            // this is an issue with the dnn searching, may need to look at this again
            if (HttpContext.Current == null)
            {
                return false;
            }

            // check the portal setting
            int portalId = PortalController.GetCurrentPortalSettings().PortalId;
            string s = HostSettings.GetHostSetting(SecuritySettingName + portalId);
            if (Utility.HasValue(s))
            {
                // if security is off.....it's off
                bool enable = Convert.ToBoolean(s, CultureInfo.InvariantCulture);
                if (enable == false)
                {
                    return false;
                }
            }
            else
            {
                // no setting exists. TURN OFF!!!!
                return false;
            }

            // check if user is Admin, if so, no security
            if (IsAdmin)
            {
                return false;
            }

            // otherwise it's on
            return true;
        }

        private static bool IsUserInRole(string roleName)
        {
            UserInfo ui = UserController.GetCurrentUserInfo();
            var rc = new RoleController();
            string[] roles = rc.GetRolesByUser(ui.UserID, ui.PortalID);
            foreach (string role in roles)
            {
                if (roleName == role)
                {
                    return true;
                }
            }

            return false;
        }
    }
}