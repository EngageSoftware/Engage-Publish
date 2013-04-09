// <copyright file="UrlGenerator.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2010
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Util
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Web;

    using DotNetNuke.Common;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Entities.Tabs;
    using DotNetNuke.Services.Localization;

    using Engage.Util;

    /// <summary>
    /// Responsible for generating URLs to items
    /// </summary>
    public class UrlGenerator
    {
        public static string BuildEditUrl(int itemId, int tabId, int moduleId)
        {
            int id = Convert.ToInt32(itemId, CultureInfo.InvariantCulture);
            int typeId = Item.GetItemTypeId(id);
            ItemType type = ItemType.GetFromId(typeId, typeof(ItemType));
            Item i;
            var controller = new ModuleController();
            int portalId = controller.GetModule(moduleId, tabId).PortalID;
            if (type.Name == ItemType.Article.Name)
            {
                i = Article.GetArticle(id, portalId);
            }
            else
            {
                i = Category.GetCategory(id);
            }

            string returnUrl = string.Empty;
            if (HttpContext.Current != null)
            {
                returnUrl = "returnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl);
            }

            return Globals.NavigateURL(
                tabId, 
                string.Empty, 
                "ctl=" + Utility.AdminContainer, 
                "mid=" + moduleId.ToString(CultureInfo.InvariantCulture), 
                "adminType=" + type.Name + "Edit", 
                "versionId=" + i.ItemVersionId.ToString(CultureInfo.InvariantCulture), 
                returnUrl);
        }

        public static string BuildEditUrl(int itemId, int tabId, int moduleId, int portalId)
        {
            int id = Convert.ToInt32(itemId, CultureInfo.InvariantCulture);
            int typeId = Item.GetItemTypeId(id, portalId);
            ItemType type = ItemType.GetFromId(typeId, typeof(ItemType));
            Item i; // = null;
            if (type.Name == ItemType.Article.Name)
            {
                i = Article.GetArticle(id, portalId);
            }
            else
            {
                i = Category.GetCategory(id, portalId);
            }

            string returnUrl = string.Empty;
            if (HttpContext.Current != null)
            {
                returnUrl = "returnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl);
            }

            return Globals.NavigateURL(
                tabId, 
                string.Empty, 
                "ctl=" + Utility.AdminContainer, 
                "mid=" + moduleId.ToString(CultureInfo.InvariantCulture), 
                "adminType=" + type.Name + "Edit", 
                "versionId=" + i.ItemVersionId.ToString(CultureInfo.InvariantCulture), 
                returnUrl);
        }

        public static string GetItemLinkUrl(object itemId, PortalSettings portalSettings)
        {
            if (itemId != null)
            {
                int id = Convert.ToInt32(itemId, CultureInfo.InvariantCulture);
                int typeId = Item.GetItemTypeId(id, portalSettings.PortalId);
                ItemType type = ItemType.GetFromId(typeId, typeof(ItemType));

                Item i;
                if (type.Name == ItemType.Article.Name)
                {
                    i = Article.GetArticle(id, portalSettings.PortalId);
                }
                else
                {
                    i = Category.GetCategory(id, portalSettings.PortalId);
                }

                if (i != null)
                {
                    return GetItemLinkUrl(i, portalSettings);
                }

                // else there is no current version of this ITEM. Can't view it currently because ItemLink.aspx doesn't
                // support versions. hk
            }

            return string.Empty;
        }

        public static string GetItemLinkUrl(Item item, PortalSettings portalSettings)
        {
            if (item != null && item.IsLinkable() && item.ApprovalStatusId != ApprovalStatus.Approved.GetId())
            {
                return GetItemVersionLinkUrl(item);
                    
                // Globals.NavigateURL(displayTabId, "", "VersionId=" + itemVersionId.ToString(CultureInfo.InvariantCulture) + "&modid=" + version.ModuleId.ToString());
            }

            if (item != null)
            {
                return GetItemLinkUrl(item, portalSettings, -1, -1, -1, string.Empty);
            }

            return string.Empty;
        }

        public static string GetItemLinkUrl(int itemId, PortalSettings portalSettings, int tabId, int moduleId)
        {
            return GetItemLinkUrl(itemId, portalSettings, tabId, moduleId, 1, string.Empty);
        }

        public static string GetItemLinkUrl(int itemId, PortalSettings portalSettings, int tabId, int moduleId, int pageId, string cultureName)
        {
            int typeId = Item.GetItemTypeId(itemId, portalSettings.PortalId);
            ItemType type = ItemType.GetFromId(typeId, typeof(ItemType));
            Item item; // = null;
            if (type.Name == ItemType.Article.Name)
            {
                item = Article.GetArticle(itemId, portalSettings.PortalId);
            }
            else
            {
                item = Category.GetCategory(itemId, portalSettings.PortalId);
            }

            if (item != null)
            {
                return GetItemLinkUrl(item, portalSettings, tabId, moduleId, pageId, cultureName);
            }

            return string.Empty;
        }

        public static string GetItemLinkUrl(Item item, PortalSettings portalSettings, int tabId, int moduleId, int pageId, string cultureName)
        {
            string returnUrl = string.Empty;

            if (item != null)
            {
                if (!item.IsLinkable())
                {
                    tabId = ModuleBase.DefaultDisplayTabIdForPortal(portalSettings.PortalId);
                }

                returnUrl = GetItemLinkUrl(item, tabId, moduleId, pageId, portalSettings, cultureName, ModuleBase.EnablePublishFriendlyUrlsForPortal(item.PortalId));
            }

            return returnUrl;
        }

        public static string GetItemVersionLinkUrl(Item item)
        {
            string returnUrl = Globals.NavigateURL(
                item.DisplayTabId, string.Empty, "VersionId=" + item.ItemVersionId.ToString(CultureInfo.InvariantCulture) + "&modid=" + item.ModuleId);

            return returnUrl;
        }

        /// <summary>
        /// Gets a URL linking to the given item when friendly URLs are turned on
        /// </summary>
        /// <param name="item">The item to link to.</param>
        /// <param name="tabId">The tab ID.</param>
        /// <param name="moduleId">The module ID.</param>
        /// <param name="pageId">The page ID.</param>
        /// <param name="portalSettings">The portal settings.</param>
        /// <param name="cultureName">The name of the current culture of the page, or <see cref="string.Empty"/></param>
        /// <param name="createFriendlyUrl">Whether to generate a friendly URL where the page name is the name of the item</param>
        /// <returns>A URL linking to the given item</returns>
        private static string GetItemLinkUrl(Item item, int tabId, int moduleId, int pageId, PortalSettings portalSettings, string cultureName, bool createFriendlyUrl)
        {
            TabInfo tabInfo;
            var tabController = new TabController();
            int? queryStringModuleId = null;
            int defaultTabId = ModuleBase.DefaultDisplayTabIdForPortal(portalSettings.PortalId);

            // if the setting to "force display on this page" is set, be sure to send them there.
            if (!item.ForceDisplayOnPage() && tabId > 0 && item.DisplayOnCurrentPage())
            {
                if (!Utility.IsPageOverrideable(portalSettings.PortalId, tabId))
                {
                    // not overrideable, send them to default tab id
                    tabInfo = tabController.GetTab(defaultTabId, portalSettings.PortalId, false);
                }
                else
                {
                    tabInfo = tabController.GetTab(tabId, portalSettings.PortalId, false);

                    // check if there is a ModuleID passed in the querystring, if so then send it in the querystring as well
                    if (moduleId > 0)
                    {
                        queryStringModuleId = moduleId;
                    }
                }
            }
            else
            {
                tabInfo = tabController.GetTab(item.DisplayTabId, portalSettings.PortalId, false);
            }

            if (tabInfo == null || tabInfo.IsDeleted)
            {
                tabInfo = tabController.GetTab(defaultTabId, portalSettings.PortalId, false);
            }

            // if the tab doesn't have an overrideable module on it redirect them to the page without Publish querystring parameters, assuming it is force display on page
            if (item.ForceDisplayOnPage() && !Utility.IsPageOverrideable(portalSettings.PortalId, tabInfo.TabID))
            {
                return Globals.NavigateURL(tabInfo.TabID);
            }

            return NavigateURL(
                tabInfo.TabID,
                tabInfo.IsSuperTab,
                portalSettings,
                string.Empty,
                cultureName,
                createFriendlyUrl ? MakeUrlSafe(item.Name) + ".aspx" : null,
                "itemId=" + item.ItemId.ToString(CultureInfo.InvariantCulture),
                UsePageId(pageId, portalSettings.PortalId) ? "pageId=" + pageId.ToString(CultureInfo.InvariantCulture) : null,
                queryStringModuleId.HasValue ? "moduleId=" + queryStringModuleId.Value.ToString(CultureInfo.InvariantCulture) : null);
        }

        /// <summary>
        /// Cleans up a string to make a legal url part
        /// </summary>
        /// <param name="urlName">The URL part text.</param>
        /// <param name="punctuationReplacement">The text with which to replace invalid characters</param>
        /// <param name="maxLength">The maximum length of the URL part</param>
        /// <returns>The cleaned URL part name</returns>
        private static string MakeUrlSafe(string urlName, string punctuationReplacement = "-", int maxLength = 50)
        {
            const string IllegalCharacters = "#%&*{}\\:<>?/+'.";
            const string UnwantedCharacters = ";,\"+!'{}[]()^$*";

            if (urlName == null)
            {
                urlName = string.Empty;
            }

            urlName = urlName.Normalize(NormalizationForm.FormD);
            var outUrl = new StringBuilder(urlName.Length);

            var i = 0;
            foreach (char c in urlName)
            {
                if (!IllegalCharacters.Contains(c.ToString()))
                {
                    // can't have leading .. or trailing .
                    if (!((i <= 0 || i == urlName.Length) && c == '.'))
                    {
                        if (c == ' ' || UnwantedCharacters.Contains(c.ToString()))
                        {
                            // replace spaces, commas and semicolons
                            outUrl.Append(punctuationReplacement);
                        }
                        else if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                        {
                            outUrl.Append(c);
                        }
                    }
                }

                i++;
                if (i >= maxLength)
                {
                    break;
                }
            }
            
            string result = outUrl.ToString();
            
            // replace double replacements
            string doubleReplacement = punctuationReplacement + punctuationReplacement;
            if (result.Contains(doubleReplacement))
            {
                result = result.Replace(doubleReplacement, punctuationReplacement);
                
                // once more for triples
                result = result.Replace(doubleReplacement, punctuationReplacement);
            }

            // Global.asax Application_BeginRequest checks for these values and will try to redirect to the non-existent page
            if (result.EndsWith("install", StringComparison.CurrentCultureIgnoreCase) ||
                result.EndsWith("installwizard", StringComparison.CurrentCultureIgnoreCase))
            {
                result = result.Substring(0, result.Length - 1);
            }

            return result;
        }

        /// <summary>
        /// Returns a full internal url
        /// </summary>
        /// <param name="tabId">The tab id linking to</param>
        /// <param name="isSuperTab">Is the destination tab a host tab?</param>
        /// <param name="portalSettings">the Portalsettings</param>
        /// <param name="controlKey">an optional controlkey. If no controlkey is needed, pass "" </param>
        /// <param name="language">an optional language. If language is an empty string, the current active culture will be added in the url.</param>
        /// <param name="aspxPageName">an optional custom .aspx page name.  If aspxPageName is an empty string, the page name will be 'default.aspx', otherwise the returned url will contain the custom pagename in the place of default.aspx.  Only applies when 'FriendlyUrls' are switched on at the host level.</param>
        /// <param name="additionalParameters">Any aditional querystring parameters. Use this format: "param1=value1", "param2=value2", ... , "paramN=valueN"</param>
        /// <returns>A full internal url</returns>
        /// <remarks> from http://support.dotnetnuke.com/issue/ViewIssue.aspx?ID=7400&amp;PROJID=2 </remarks>
        /// <history>
        /// [bchapman] 18/04/08  Add new NavigateUrl overload to allow custom page names
        /// [bdukes]   23/11/10  Converted to C#
        /// </history>
        private static string NavigateURL(int tabId, bool isSuperTab, PortalSettings portalSettings, string controlKey, string language, string aspxPageName, params string[] additionalParameters)
        {
            string url = tabId == Null.NullInteger ? Globals.ApplicationURL() : Globals.ApplicationURL(tabId);

            if (!string.IsNullOrEmpty(controlKey))
            {
                url += "&ctl=" + controlKey;
            }

            if (additionalParameters != null)
            {
                url = additionalParameters
                    .Where(parameter => !string.IsNullOrEmpty(parameter))
                    .Aggregate(url, (current, parameter) => current + "&" + parameter);
            }

            if (isSuperTab)
            {
                url += "&portalid=" + portalSettings.PortalId.ToString(CultureInfo.InvariantCulture);
            }

            // only add language to url if more than one locale is enabled, and if admin did not turn it off
            if (LocaleController.Instance.GetLocales(portalSettings.PortalId).IsMany() && portalSettings.EnableUrlLanguage)
            {
                if (string.IsNullOrEmpty(language))
                {
                    url += "&language=" + Thread.CurrentThread.CurrentCulture.Name;
                }
                else
                {
                    url += "&language=" + language;
                }
            }

            if (Host.UseFriendlyUrls)
            {
                if (string.IsNullOrEmpty(aspxPageName))
                {
                    aspxPageName = Globals.glbDefaultPage;
                }

                var tab = new TabController().GetTab(tabId, portalSettings.PortalId, false);
                if (tab != null)
                {
                    return Globals.FriendlyUrl(tab, url, aspxPageName, portalSettings);
                }

                return Globals.FriendlyUrl(null, url, portalSettings);
            }

            return Globals.ResolveUrl(url);
        }

        /// <summary>
        /// Whether to use the current page ID on the <c>QueryString</c>.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="portalId">The portal id.</param>
        /// <returns><c>true</c> if the current page ID should appear on the <c>QueryString</c>; otherwise <c>false</c></returns>
        private static bool UsePageId(int pageId, int portalId)
        {
            return pageId > 1 && ModuleBase.AllowArticlePagingForPortal(portalId);
        }
    }
}