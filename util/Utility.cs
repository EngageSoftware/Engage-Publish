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
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Search;
using Engage.Dnn.Publish.Data;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;

namespace Engage.Dnn.Publish.Util
{
    public enum SearchSortOption
    {
        ItemId = 0,
        Name,
        UpdatedDate
    }

    public enum DisplayOption
    {
        /// <summary>
        /// Displays only the title of the article(s).
        /// </summary>
        Title = 0,
        /// <summary>
        /// Displays the title and abstract of the article(s).
        /// </summary>
        Abstract,
        /// <summary>
        /// Displays the title, abstract, and thumbnail (if available) of the article(s).
        /// </summary>
        Thumbnail,
        /// <summary>
        /// Displays the title and thumbnail (if available) of the article(s).
        /// </summary>
        TitleAndThumbnail,
        /// <summary>
        /// Displays the Date of the article
        /// </summary>
        Date,
        /// <summary>
        /// Displays the Read More link.
        /// </summary>
        ReadMore,
        /// <summary>
        /// Displays the Author name.
        /// </summary>
        Author

    }

    public enum ArticleViewOption
    {
        /// <summary>
        /// Displays only the title of the article(s).
        /// </summary>
        Title = 0,
        /// <summary>
        /// Displays the title and abstract of the article(s).
        /// </summary>
        Abstract,
        /// <summary>
        /// Displays the title, abstract, and thumbnail (if available) of the article(s).
        /// </summary>
        Thumbnail,
        /// <summary>
        /// Displays the title and thumbnail (if available) of the article(s).
        /// </summary>
        TitleAndThumbnail,
        /// <summary>
        /// Displays the Date of the article
        /// </summary>
        Date,
        /// <summary>
        /// Displays the Read More link.
        /// </summary>
        ReadMore
    }

    public enum ThumbnailDisplayOption
    {
        DotNetNuke,
        EngagePublish
    }

    /// <summary>
    /// Display options for the Rating control on the ArticleDisplay module
    /// </summary>
    public enum RatingDisplayOption
    {
        /// <summary>
        /// Do not display the Rating control
        /// </summary>
        Disable = 0,
        /// <summary>
        /// Display the current average rating in the Rating control, 
        /// but do not allow users to submit ratings
        /// </summary>
        ReadOnly,
        /// <summary>
        /// Display the current average rating in the Rating control,
        /// and also allow users to submit their own ratings.
        /// </summary>
        Enable
    }
    /// <summary>
    /// Display options for the CommentDisplay control on the ArticleDisplay module
    /// </summary>
    public enum CommentDisplayOption
    {
        /// <summary>
        /// Show only a certain number of comments at a time using paging
        /// </summary>
        Paging = 0,
        /// <summary>
        /// Show all comments
        /// </summary>
        ShowAll
    }

    /// <summary>
    /// Options used when displaying and collecting names
    /// </summary>
    public enum NameDisplayOption
    {
        /// <summary>
        /// Display or ask for the full name
        /// </summary>
        Full = 0,
        /// <summary>
        /// Display or ask for only the initial (first letter)
        /// </summary>
        Initial,
        /// <summary>
        /// Do not display or ask for the name
        /// </summary>
        None
    }

    //public enum PublishSyndicationModes
    //{
    //    Publisher = 0,
    //    Subscriber = 1
    //}

    public enum CategoryFeatureOption
    {
        RandomlyDisplayArticle = 0
    }

    /// <summary>
    /// Summary description for Utility.
    /// </summary>
    public static class Utility
    {
        public const string MediaFileTypes = "jpg,jpeg,jpe,gif,bmp,png,swf";
        public const string DnnFriendlyModuleName = "Engage: Publish";

        public const string DnnTagsFriendlyModuleName = "Engage: Publish Tag Cloud";

        public const string SimpleGalleryFriendlyName = "SimpleGallery";
        public const string SimpleGalleryDefinitionModuleName = "SimpleGallery";
        public const string UltraMediaGalleryFriendlyName = "BizModules - UltraPhotoGallery";
        public const string UltraMediaGalleryDefinitionModuleName = "BizModules - UltraPhotoGallery";
        public const string ActiveForumsDefinitionModuleName = "NTForums";
        public const int RedirectInSeconds = 10;
        public const string AdminContainer = "AdminContainer";
        public const string PublishEmailNotificationRole = "PublishEmailNotificationRole";
        public const string PublishAdminRole = "PublishAdminRole";
        public const string PublishAuthorRole = "PublishAuthorRole";
        public const string PublishEmail = "PublishEmail";
        public const string PublishSetup = "PublishSetup";
        public const string PublishRating = "PublishRating";

        private readonly static char[] tagSeparators = { ';', ',' };
        public static char[] GetTagSeparators()
        {
            return (char[])tagSeparators.Clone();
        }

        public const string PublishArticleEditWidth = "PublishArticleEditWidth";
        public const string PublishArticleEditHeight = "PublishArticleEditHeight";
        public const string PublishDescriptionEditHeight = "PublishDescriptionEditHeight";
        public const string PublishDescriptionEditWidth = "PublishDescriptionEditWidth";

        public const string PublishCacheKeys = "PublishCacheKeys";

        public const string PublishCacheTime = "PublishCacheTime";

        public const string PublishThumbnailDisplayOption = "PublishThumbnailDisplayOption";

        public const string PublishEnableArticlePaging = "PublishEnableArticlePaging";
        public const string PublishEnableTags = "PublishEnableTags";

        public const string PublishEnableVenexusSearch = "PublishEnableVenexusSearch";
        public const string PublishEnableSimpleGalleryIntegration = "PublishEnableSimpleGalleryIntegration";
        public const string PublishEnableUltraMediaGalleryIntegration = "PublishEnableUltraMediaGalleryIntegration";
        public const string PublishEnableViewTracking = "PublishEnableViewTracking";
        public const string PublishEnablePublishFriendlyUrls = "PublishEnablePublishFriendlyUrls";
        public const string PublishEnableDisplayNameAsHyperlink = "PublishEnableDisplayNameAsHyperlink";
        public const string PublishAllowRichTextDescriptions = "PublishAllowRichTextDescriptions";
        public const string PublishUseApprovals = "PublishUseApprovals";
        public const string PublishUseEmbeddedArticles = "PublishUseEmbeddedArticles";
        public const string PublishShowItemId = "PublishShowItemId";
        public const string PublishThumbnailSubdirectory = "PublishThumbnailSubdirectory";
        public const string PublishDefaultDisplayPage = "PublishDefaultDisplayPage";

        public const string PublishDefaultTagPage = "PublishDefaultTagPage";
        
        public const string PublishRatingMaximum = "PublishRatingMaximum";
        public const string PublishRatingAnonymous = "PublishRatingAnonymous";
        public const string PublishComment = "PublishComment";
        public const string PublishDefaultEmailAFriend = "PublishDefaultEmailAFriend";
        public const string PublishDefaultPrinterFriendly = "PublishDefaultPrinterFriendly";
        public const string PublishDefaultRatings = "PublishDefaultRatings";
        public const string PublishDefaultComments = "PublishDefaultComments";
        public const string PublishDefaultReturnToList = "PublishDefaultReturnToList";
        public const string PublishSessionReturnToList = "PublishSessionReturnToList";
        public const string PublishDefaultShowAuthor = "PublishDefaultShowAuthor";
        public const string PublishDefaultShowTags = "PublishDefaultShowTags";

        //publisher/subscriber
        public const string PublishAutoArchiveContent = "PublishAutoArchiveContent";
        public const string PublishAutoApproveContent = "PublishAutoApproveContent";
        public const string PublishSubscriberKey = "PublishSubscriberKey";
        public const string PublishSubscriberUrl = "PublishSubscriberUrl";
        
        public const string PublishCommentApproval = "PublishCommentApproval";
        public const string PublishCommentAnonymous = "PublishCommentAnonymous";
        public const string PublishCommentAutoApprove = "PublishCommentAutoApprove";

        public const string PublishAuthorCategoryEdit = "PublishAuthorCategoryEdit";
        public const string PublishPopularTagCount = "PublishPopularTagCount";

        //ping settings
        public const string PublishEnablePing = "PublishEnablePing";
        public const string PublishPingServers = "PublishPingServers";
        public const string PublishPingChangedUrl = "PublishPingChangedUrl";

        //Forum settings
        public const string PublishForumProviderType = "PublishForumProviderType";
        
        /*******************Cache Key Constants********************/
        public const string CacheKeyPublishCategory = "PublishCacheKey_";

        //cache keys for Categories
        public const string CacheKeyPublishCustomDisplay = "PublishCacheKeyCustomDisplay_";
        public const string CacheKeyPublishCategoryDisplay = "PublishCacheKeyCategoryDisplay_";
        public const string CacheKeyPublishCategoryFeature = "PublishCacheKeyCategoryFeature_";
        public const string CacheKeyPublishCategoryNLevels = "PublishCacheKeyCategoryNLevels_";
        public const string CacheKeyPublishAdminCategorySort = "CacheKeyPublishAdminCategorySort_";

        //Cache keys for Articles
        public const string CacheKeyPublishArticle = "PublishCacheKeyArticleDisplay_";

        public const string CacheKeyPublishArticleComments = "CacheKeyPublishArticleComments_";

        //Cache keys for Tags
        public const string CacheKeyPublishTag = "PublishCacheKeyTag_";

        public static bool HasValue(string value)
        {
            return !String.IsNullOrEmpty(value) && value.Trim().Length > 0;
            //if (value == null)
            //{
            //    return false;	
            //}
            //else if (value.Trim().Length == 0)
            //{
            //    return false;	
            //}
            //else
            //{
            //    return true;
            //}
        }

        public static void AddCacheKey(string KeyName, int portalId)
        {
            string cacheKey = Utility.PublishCacheKeys + portalId.ToString(CultureInfo.InvariantCulture);
            ArrayList al = DataCache.GetCache(cacheKey) as ArrayList;
            if (al == null)
            {
                al = new ArrayList();
            }
            al.Add(KeyName);
            DataCache.SetCache(cacheKey, al);

        }

        public static void ClearPublishCache(int portalId)
        {
            string cacheKey = Utility.PublishCacheKeys + portalId.ToString(CultureInfo.InvariantCulture);
            ArrayList al = DataCache.GetCache(cacheKey) as ArrayList;
            if (al != null)
            {
                foreach (string s in al)
                {
                    DataCache.RemoveCache(s);
                }
            }
            DataCache.RemoveCache(cacheKey);
        }

        public static string DesktopModuleFolderName
        {
            get
            {
                StringBuilder sb = new StringBuilder(128);
                sb.Append("/DesktopModules/");
                sb.Append(Globals.GetDesktopModuleByName(DnnFriendlyModuleName).FolderName);
                sb.Append("/");
                return sb.ToString();
            }
        }

        public static Uri GetThumbnailLibraryPath(int portalId)
        {
            return new Uri(Path.Combine(GetPortalSettings(portalId).HomeDirectory, ModuleBase.ThumbnailSubdirectoryForPortal(portalId)), UriKind.Relative);
        }

        public static Uri GetThumbnailLibraryMapPath(int portalId)
        {
            Uri path = new Uri(Path.Combine(GetPortalSettings(portalId).HomeDirectoryMapPath, ModuleBase.ThumbnailSubdirectoryForPortal(portalId)), UriKind.Absolute);

            //make sure it exists before we tell people about it.  BD
            if (!Directory.Exists(path.AbsolutePath))
            {
                Directory.CreateDirectory(path.AbsolutePath);
            }

            return path;
        }

        public static DataTable GetDisplayTabIds(string[] moduleNames)
        {
            int portalId = UserController.GetCurrentUserInfo().PortalID;

            ModuleController mc = new ModuleController();
            TabController tc = new TabController();
            ArrayList al;// = null;

            //only want a unique list of ItemIds
            IDictionary list = new Hashtable();

            foreach (string moduleName in moduleNames)
            {
                al = mc.GetModulesByDefinition(portalId, moduleName);
                foreach (ModuleInfo mi in al)
                {
                    TabInfo ti = tc.GetTab(mi.TabID, mi.PortalID, false);
                    if (ti != null)
                    {
                        if (ti.IsDeleted == false)
                        {
                            if (list.Contains(ti.TabID) == false)
                            {
                                //check if the Tab has an Overrideable instance of Publish

                                //ModuleSettingsBase msb = new ModuleSettingsBase();
                                IDictionary modSettings = PortalSettings.GetTabModuleSettings(mi.TabModuleID);

                                if (modSettings.Contains("Overrideable"))
                                {
                                    if (Convert.ToBoolean(modSettings["Overrideable"], CultureInfo.InvariantCulture))
                                    {
                                        list.Add(ti.TabID, ti);
                                    }
                                }
                                else
                                {
                                    //if the tabmodulesettings didn't contain overrideable then the module hasn't been configured and is overrideable by default.
                                    list.Add(ti.TabID, ti);
                                }
                            }
                        }
                    }
                }
            }

            //need to copy items to here in order to sort
            al = new ArrayList();
            foreach (TabInfo ti in list.Values)
            {
                al.Add(ti);
            }


            al.Sort(new TabInfoNameComparer());

            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            dt.Columns.Add("TabId", typeof(int));
            dt.Columns.Add("TabName", typeof(string));

            //finally create listitems from the sorted list
            foreach (TabInfo ti in al)
            {
                DataRow r = dt.NewRow();
                r["TabId"] = ti.TabID;
                r["TabName"] = ti.TabName;
                dt.Rows.Add(r);
            }

            return dt;
        }

        public static DataTable GetDisplayTabIdsAll(string[] moduleNames)
        {
            int portalId = UserController.GetCurrentUserInfo().PortalID;

            ModuleController mc = new ModuleController();
            TabController tc = new TabController();
            ArrayList al;// = null;

            //only want a unique list of ItemIds
            IDictionary list = new Hashtable();

            foreach (string moduleName in moduleNames)
            {
                al = mc.GetModulesByDefinition(portalId, moduleName);
                foreach (ModuleInfo mi in al)
                {
                    TabInfo ti = tc.GetTab(mi.TabID, mi.PortalID, false);
                    if (ti != null)
                    {
                        if (ti.IsDeleted == false)
                        {
                            if (list.Contains(ti.TabID) == false)
                            {
                                list.Add(ti.TabID, ti);
                            }
                        }
                    }
                }
            }

            //need to copy items to here in order to sort
            al = new ArrayList();
            foreach (TabInfo ti in list.Values)
            {
                al.Add(ti);
            }


            al.Sort(new TabInfoNameComparer());

            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            dt.Columns.Add("TabId", typeof(int));
            dt.Columns.Add("TabName", typeof(string));

            //finally create listitems from the sorted list
            foreach (TabInfo ti in al)
            {
                DataRow r = dt.NewRow();
                r["TabId"] = ti.TabID;
                r["TabName"] = ti.TabName;
                dt.Rows.Add(r);
            }

            return dt;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Code Analysis doesn't see validation")]
        public static int GetArticleId(SearchResultsInfo result)
        {
            if (result == null || result.SearchKey.IndexOf("Article", StringComparison.Ordinal) == -1) return 0;
            int id = 0;
            int index = result.SearchKey.IndexOf("-", StringComparison.Ordinal);
            if (index > -1)
            {
                string s = result.SearchKey.Substring(index + 1);
                if (Int32.TryParse(s.ToString(), out id))
                    return id;
                id = 0;
            }

            return id;
        }

        public static string OnlyAlphanumericCharacters(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }

            input = input.Trim();
            int length = input.Length;
            StringBuilder returnString = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                string sCurrent = input.Substring(i, 1);
                if (IsAlphaNumeric(sCurrent))
                {
                    returnString.Append(sCurrent);
                }
            }
            returnString = returnString.Replace(" ", "-");//Replace(sAns, " ", "-")

            return returnString.ToString();
        }

        private static bool IsAlphaNumeric(string testString)
        {
            Regex objAlphaPattern = new Regex("[^0-9a-zA-Z ]");

            return !objAlphaPattern.IsMatch(testString);
        }

        public static string StripTags(string html)
        {
            return Regex.Replace(html, "<[^>]*>", "");
        }

        public static bool IsDisabled(int itemId, int portalId)
        {
            //check if an item is disabled
            int typeId = Item.GetItemTypeId(itemId);
            Item item = Item.GetItem(itemId, portalId, typeId, true);
            return item.Disabled;
        }

        private static PortalAliasInfo GetPortalAliasInfo(int portalId)
        {
            ArrayList portalAliases = (new PortalAliasController()).GetPortalAliasArrayByPortalID(portalId);
            return portalAliases != null && portalAliases.Count > 0 ? (PortalAliasInfo)portalAliases[0] : null;
        }

        public static PortalSettings GetPortalSettings(int portalId)
        {
            return new PortalSettings(-1, GetPortalAliasInfo(portalId));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Code Analysis doesn't see validation")]
        public static IList GetRandomItem(IList view)
        {
            if (view == null || view.Count <= 1)
            {
                return view;
            }
            int seed = GetRandomSeed();
            Random random = new Random(seed);

            while (view.Count > 1)
            {
                int i = random.Next(0, view.Count);
                //System.Diagnostics.Debug.WriteLine(i);
                //System.Diagnostics.Debug.WriteLine(view[i]["ItemID"]);
                view.RemoveAt(i);
            }

            //System.Diagnostics.Debug.WriteLine(view[0]["ItemID"]);

            return view;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Does not return class state information")]
        public static int GetRandomSeed()
        {
            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            return (randomBytes[0] & 0x7f) << 24 | randomBytes[1] << 16 | randomBytes[2] << 8 | randomBytes[3];
        }

        //This will take a datetime string and convert it to the InvariantCulture datetime string.
        public static string GetInvariantDateTime(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            try
            {
                return Convert.ToDateTime(value, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture);
            }
            catch
            {
                return value;
            }
        }

        //This will take a datetime string and convert it to the InvariantCulture datetime string.
        public static string GetCurrentCultureDateTime(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            try
            {
                return Convert.ToDateTime(value, CultureInfo.InvariantCulture).ToString(CultureInfo.CurrentCulture);
            }
            catch
            {
                return value;
            }


        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Code Analysis doesn't see validation")]
        public static DataTable SortDataTable(DataTable dt, string sortOrder)
        {
            if (dt != null && !String.IsNullOrEmpty(sortOrder))
            {
                DataTable dtreturn = dt.Clone();

                sortOrder = sortOrder.Remove(sortOrder.Length - 1, 1);

                //parse through the sortorder list first

                string[] orderlist = sortOrder.Split(',');
                foreach (string itemid in orderlist)
                {
                    object o = Convert.ToInt32(itemid, CultureInfo.InvariantCulture);
                    DataRow dr = dt.Rows.Find(o);
                    if (dr != null)
                    {
                        dtreturn.ImportRow(dr);
                        dt.Rows.Remove(dr);
                    }

                }
                //for any rows that weren't in the sort list add them to this list.
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dtreturn.ImportRow(dt.Rows[i]);
                }

                return dtreturn;
            }
            return dt;
        }

        public static string BuildEditUrl(int itemId, int tabId, int moduleId )
        {
            int id = Convert.ToInt32(itemId, CultureInfo.InvariantCulture);
            int typeId = Item.GetItemTypeId(id);
            ItemType type = ItemType.GetFromId(typeId, typeof(ItemType));
            Item i;// = null;
            if (type.Name == ItemType.Article.Name)
            {
                i = Article.GetArticle(id);
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

            return Globals.NavigateURL(tabId, "", "ctl=" + AdminContainer, "mid=" + moduleId.ToString(CultureInfo.InvariantCulture),
                    "adminType=" + type.Name + "Edit", "versionId=" + i.ItemVersionId.ToString(CultureInfo.InvariantCulture), returnUrl);
        }

        public static bool IsPageOverrideable(int portalId, int displayTabId)
        {
            //int portalId = UserController.GetCurrentUserInfo().PortalID;
            try
            {
                ModuleController mc = new ModuleController();
                DotNetNuke.Entities.Tabs.TabController tc = new TabController();
                TabInfo tab = tc.GetTab(displayTabId, portalId, false);
                if (tab.IsDeleted) //The tab is in the recycle bin.
                {
                    return false;
                }
                Dictionary<int, ModuleInfo> modules = mc.GetTabModules(displayTabId);
                foreach (ModuleInfo mi in modules.Values)
                {
                    if (mi.FriendlyName == DnnFriendlyModuleName)
                    {
                        //found one
                        object o = mc.GetTabModuleSettings(mi.TabModuleID)["Overrideable"];

                        if (o != null)
                        {
                            if (Convert.ToBoolean(o, CultureInfo.InvariantCulture))
                            {
                                return true;
                            } //get out there is a module on the page that is configured overrideable.
                        }
                        else
                        {
                            //if there isn't a setting for overrideable, it is overrideable.
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                
                //if we get an error here then return false as it's not linkable
                return false;
            }
        }

        public static ModuleInfo GetPublishModuleInfo(int moduleId)
        {

            //4.6.2 and later this would work, but for backwards compatibility we can't use.
            //ModuleController mc = new ModuleController();
            //foreach (ModuleInfo info in mc.GetModuleTabs(moduleId))
            //{
            //    if (info.FriendlyName == Utility.DnnFriendlyModuleName)
            //    {
            //        return info;
            //    }
            //}

            ModuleController mc = new ModuleController();
            ModuleInfo info = null;
            using (IDataReader dr = DataProvider.Instance().GetModuleInfo(moduleId))
            {
                if (dr.Read())
                {
                    int tabId = (int)dr["Tabid"];
                    info = mc.GetModule(moduleId, tabId);
                }
            }
            return info;
        }

        public static string GetItemLinkUrl(object itemId, int portalId, int tabId, int moduleId, int pageId, string otherParameters)
        {
            if (itemId != null)
            {
                int id = Convert.ToInt32(itemId, CultureInfo.InvariantCulture);
                int typeId = Item.GetItemTypeId(id);
                ItemType type = ItemType.GetFromId(typeId, typeof(ItemType));
                Item i;// = null;
                if (type.Name == ItemType.Article.Name)
                {
                    i = Article.GetArticle(id);
                }
                else
                {
                    i = Category.GetCategory(id);
                }
                if (i == null)
                {
                    //there is no current version of this ITEM. Can't view it currently because ItemLink.aspx doesn't
                    //support versions. hk
                    return string.Empty;
                }
                else
                {
                    //see DisplayOnCurrentPage is true for this item.
                    if (i.DisplayOnCurrentPage())
                    {
                        //check if there is an overrideable module on this page, if not be sure to pass the moduleid
                        if (IsPageOverrideable(portalId, tabId))
                        {
                            return ApplicationUrl + DesktopModuleFolderName + "itemlink.aspx?itemId=" + id.ToString(CultureInfo.InvariantCulture) + "&tabid=" + tabId.ToString(CultureInfo.InvariantCulture) + UsePageId(pageId, portalId) + otherParameters;
                        }
                        else
                        {
                            return ApplicationUrl + DesktopModuleFolderName + "itemlink.aspx?itemId=" + id.ToString(CultureInfo.InvariantCulture) + "&tabid=" + tabId.ToString(CultureInfo.InvariantCulture) + "&modid=" + moduleId + UsePageId(pageId, portalId) + otherParameters;
                        }
                    }
                    else
                    {
                        return ApplicationUrl + DesktopModuleFolderName + "itemlink.aspx?itemId=" + id.ToString(CultureInfo.InvariantCulture) + UsePageId(pageId, portalId) + otherParameters;
                    }
                }
            }
            else
            {
                return string.Empty;
            }
        }

        private static string UsePageId(int pageid, int portalid)
        {
            if (pageid > 1 && ModuleBase.AllowArticlePagingForPortal(portalid))
            {
                    return "&pageid=" + pageid.ToString(CultureInfo.InvariantCulture);
            }
            else return string.Empty;
        }

        public static bool IsSimpleGalleryInstalled
        {
            get
            {
                DesktopModuleController desktopModules = new DesktopModuleController();
                //return desktopModules.GetDesktopModuleByFriendlyName(SimpleGalleryFriendlyName) != null;
                return desktopModules.GetDesktopModuleByModuleName(SimpleGalleryDefinitionModuleName) != null;
            }
        }

        public static bool IsUltraMediaGalleryInstalled
        {
            get
            {
                DesktopModuleController desktopModules = new DesktopModuleController();
                //return desktopModules.GetDesktopModuleByFriendlyName(UltraMediaGalleryDefinitionFriendlyName) != null;
                return desktopModules.GetDesktopModuleByModuleName(UltraMediaGalleryDefinitionModuleName) != null;
            }
        }

        public static string ApplicationUrl
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Request.ApplicationPath == "/" ? "" : HttpContext.Current.Request.ApplicationPath;
                }
            }
        }

        public static string WebServiceUrl
        {
            get
            {
                StringBuilder url = new StringBuilder();
                if (HttpContext.Current != null)
                {
                    url.Append(HttpContext.Current.Request.Url.Scheme);
                    url.Append("://");
                    url.Append(HttpContext.Current.Request.Url.Authority);
                    url.Append(ApplicationUrl);
                    url.Append(DesktopModuleFolderName);
                    url.Append("services/publishservices.asmx");
                }

                return url.ToString();
            }
        }

        //		Public Function OnlyAlphaNumericChars(ByVal OrigString As String) As String
        //            '***********************************************************
        //            'INPUT:  Any String
        //            'OUTPUT: The Input String with all non-alphanumeric characters 
        //            '        removed
        //            'EXAMPLE Debug.Print OnlyAlphaNumericChars("Hello World!")
        //            'output = "HelloWorld")
        //            'NOTES:  Not optimized for speed and will run slow on long
        //            '        strings.  If you plan on using long strings, consider 
        //            '        using alternative method of appending to output string,
        //            '        such as the method at
        //            '        http://www.freevbcode.com/ShowCode.Asp?ID=154
        //            '***********************************************************
        //            Dim lLen As Integer
        //            Dim sAns As String
        //            Dim lCtr As Integer
        //            Dim sChar As String
        //
        //            OrigString = Trim(OrigString)
        //            lLen = Len(OrigString)
        //            For lCtr = 1 To lLen
        //                sChar = Mid(OrigString, lCtr, 1)
        //                If IsAlphaNumeric(Mid(OrigString, lCtr, 1)) Then
        //                    sAns = sAns & sChar
        //                End If
        //            Next
        //
        //            OnlyAlphaNumericChars = sAns
        //
        //        End Function
        //
        //        Private Function IsAlphaNumeric(ByVal sChr As String) As Boolean
        //            IsAlphaNumeric = sChr Like "[0-9A-Za-z]"
        //        End Function

        public static IDataReader GetModuleByModuleId(int moduleId)
        {
            return DataProvider.Instance().GetModuleByModuleId(moduleId);
        }

        public static string GetTabModuleSettingAsString(int moduleId, string settingName, string defaultValue)
        {
            using (IDataReader dr = GetModuleByModuleId(moduleId))
            {
                if (dr.Read())
                {
                    int tabModuleId = (int)dr["TabModuleId"];
                    object value = (new ModuleController()).GetTabModuleSettings(tabModuleId)[settingName];
                    if (value != null)
                    {
                        return value.ToString();
                    }
                }
            }
            return defaultValue;
        }

        public static int GetTabModuleSettingAsInt(int moduleId, string settingName, int defaultValue)
        {
            using (IDataReader dr = GetModuleByModuleId(moduleId))
            {
                if (dr.Read())
                {
                    int value;
                    int tabModuleId = (int)dr["TabModuleId"];
                    object valueObj = (new ModuleController()).GetTabModuleSettings(tabModuleId)[settingName];
                    if (valueObj != null && int.TryParse(valueObj.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                    {
                        return value;
                    }
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Parses a <see cref="string"/> array into a <see cref="List{T}"/> of <see cref="int"/>.
        /// </summary>
        /// <param name="splitIntegers">A <see cref="string"/> array of integer values.</param>
        /// <exception cref="FormatException">If any value is not an integer</exception>>
        /// <returns>A <see cref="List{T}"/> of the values in <paramref name="splitIntegers"/> parsed as <see cref="int"/>s.</returns>
        public static List<int> ParseIntegerList(string[] splitIntegers)
        {
            List<int> integers = new List<int>(splitIntegers.Length);
            foreach (string integer in splitIntegers)
            {
                if (HasValue(integer))
                {
                    integers.Add(int.Parse(integer, CultureInfo.InvariantCulture));
                }
            }
            return integers;
        }

        /// <summary>
        /// Determines whether the <paramref name="leftHandSide"/> is greater than or equal to the <paramref name="rightHandSide"/>.
        /// </summary>
        /// <param name="leftHandSide">The left hand side version.</param>
        /// <param name="rightHandSide">The right hand side version.</param>
        /// <returns>
        /// 	<c>true</c> if the <paramref name="leftHandSide"/> if greater or equal to the <paramref name="rightHandSide"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsVersionGreaterOrEqual(List<int> leftHandSide, List<int> rightHandSide)
        {
            if (leftHandSide != null && rightHandSide != null)
            {
                if (leftHandSide.Count != rightHandSide.Count)
                {
                    int sizeDifference = leftHandSide.Count - rightHandSide.Count;
                    while (sizeDifference > 0) //leftHandSide has more
                    {
                        rightHandSide.Add(0);
                        sizeDifference--;
                    }
                    while (sizeDifference < 0) //rightHandSide has more
                    {
                        leftHandSide.Add(0);
                        sizeDifference++;
                    }
                }

                for (int i = 0; i < leftHandSide.Count; i++)
                {
                    if (leftHandSide[i] > rightHandSide[i])
                    {
                        return true;
                    }
                    if (leftHandSide[i] < rightHandSide[i])
                    {
                        return false;
                    }
                }
                return true; //equal
            }
            if (leftHandSide == null)
            {
                throw new ArgumentNullException("leftHandSide");
            }
            else
            {
                throw new ArgumentNullException("rightHandSide");
            }
        }

        public static void SortDataTableSingleParam(DataTable dt, string sort)
        {
            DataTable newDT = dt.Clone();
            int rowCount = dt.Rows.Count;

            DataRow[] foundRows = dt.Select(null, sort); // Sort with Column name 
            for (int i = 0; i < rowCount; i++)
            {
                object[] arr = new object[dt.Columns.Count];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    arr[j] = foundRows[i][j];
                }
                DataRow data_row = newDT.NewRow();
                data_row.ItemArray = arr;
                newDT.Rows.Add(data_row);
            }

            //clear the incoming dt 
            dt.Rows.Clear();

            for (int i = 0; i < newDT.Rows.Count; i++)
            {
                object[] arr = new object[dt.Columns.Count];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    arr[j] = newDT.Rows[i][j];
                }

                DataRow data_row = dt.NewRow();
                data_row.ItemArray = arr;
                dt.Rows.Add(data_row);
            }

        }

        public static void LocalizeGridView(GridView gridView, string resourceFile)
        {
            if (gridView == null)
            {
                throw new ArgumentNullException("gridView");
            }
            foreach (DataControlField column in gridView.Columns)
            {
                if (!string.IsNullOrEmpty(column.HeaderText))
                {
                    string localizedText = Localization.GetString(column.HeaderText + ".Header", resourceFile);
                    if (!string.IsNullOrEmpty(localizedText))
                    {
                        column.HeaderText = localizedText;
                        column.AccessibleHeaderText = localizedText;
                    }
                }
                if (!string.IsNullOrEmpty(column.FooterText))
                {
                    column.FooterText = Localization.GetString(column.FooterText + ".Header", resourceFile);
                }
                if (!string.IsNullOrEmpty(column.AccessibleHeaderText))
                {
                    column.AccessibleHeaderText = Localization.GetString(column.AccessibleHeaderText + ".Header", resourceFile);
                }

                ButtonField buttonField = column as ButtonField;
                if (buttonField != null)
                {
                    if (string.IsNullOrEmpty(buttonField.Text))
                    {
                        buttonField.Text = Localization.GetString(buttonField.Text, resourceFile);
                    }
                }
                else
                {
                    CheckBoxField checkboxField = column as CheckBoxField;
                    if (checkboxField != null)
                    {
                        if (!string.IsNullOrEmpty(checkboxField.Text))
                        {
                            checkboxField.Text = Localization.GetString(checkboxField.Text, resourceFile);
                        }
                    }
                    else
                    {
                        CommandField commands = column as CommandField;
                        if (commands != null)
                        {
                            if (!string.IsNullOrEmpty(commands.CancelText))
                            {
                                commands.CancelText = Localization.GetString(commands.CancelText, resourceFile);
                            }
                            if (!string.IsNullOrEmpty(commands.DeleteText))
                            {
                                commands.DeleteText = Localization.GetString(commands.DeleteText, resourceFile);
                            }
                            if (!string.IsNullOrEmpty(commands.EditText))
                            {
                                commands.EditText = Localization.GetString(commands.EditText, resourceFile);
                            }
                            if (!string.IsNullOrEmpty(commands.InsertText))
                            {
                                commands.InsertText = Localization.GetString(commands.InsertText, resourceFile);
                            }
                            if (!string.IsNullOrEmpty(commands.NewText))
                            {
                                commands.NewText = Localization.GetString(commands.NewText, resourceFile);
                            }
                            if (!string.IsNullOrEmpty(commands.SelectText))
                            {
                                commands.SelectText = Localization.GetString(commands.SelectText, resourceFile);
                            }
                            if (!string.IsNullOrEmpty(commands.UpdateText))
                            {
                                commands.UpdateText = Localization.GetString(commands.UpdateText, resourceFile);
                            }
                        }
                        else
                        {
                            HyperLinkField hyperLinkfield = column as HyperLinkField;
                            if (hyperLinkfield != null)
                            {
                                if (!string.IsNullOrEmpty(hyperLinkfield.Text))
                                {
                                    hyperLinkfield.Text = Localization.GetString(hyperLinkfield.Text, resourceFile);
                                }
                            }
                            else
                            {
                                ImageField imageField = column as ImageField;
                                if (imageField != null)
                                {
                                    if (!string.IsNullOrEmpty(imageField.AlternateText))
                                    {
                                        imageField.AlternateText = Localization.GetString(imageField.AlternateText, resourceFile);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #region Portal Settings

        //public static bool? GetBooleanPortalSetting(string settingName, int portalId)
        //{
        //    return _GetBooleanPortalSetting(settingName, portalId, null);
        //}

        public static bool GetBooleanPortalSetting(string settingName, int portalId, bool defaultValue)
        {
            return _GetBooleanPortalSetting(settingName, portalId, defaultValue).Value;
        }

        private static bool? _GetBooleanPortalSetting(string settingName, int portalId, bool? defaultValue)
        {
            bool settingValue;
            string value = HostSettings.GetHostSetting(settingName + portalId.ToString(CultureInfo.InvariantCulture));
            if (bool.TryParse(value, out settingValue))
            {
                return settingValue;
            }
            return defaultValue;
        }

        public static string GetStringPortalSetting(string settingName, int portalId)
        {
            return GetStringPortalSetting(settingName, portalId, null);
        }

        public static string GetStringPortalSetting(string settingName, int portalId, string defaultValue)
        {
            string setting = HostSettings.GetHostSetting(settingName + portalId.ToString(CultureInfo.InvariantCulture));
            return HasValue(setting) ? setting : defaultValue;
        }

        public static void SetSettingListValue(string settingName, int portalId, ListControl list)
        {
            string setting = GetStringPortalSetting(settingName, portalId);
            if (setting != null)
            {
                ListItem li = list.Items.FindByValue(setting);
                if (li != null)
                {
                    list.ClearSelection();
                    li.Selected = true;
                }
            }
        }



        public static bool IsPingEnabledForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishEnablePing + portalId.ToString(CultureInfo.InvariantCulture));
            return Utility.HasValue(s) && Convert.ToBoolean(s, CultureInfo.InvariantCulture);
        }


        #endregion

        #region SQL Parameters

        /// <summary>
        /// Creates a String (varchar) SQL param, setting and checking the bounds of the value within the parameter.
        /// Sets the value to DBNull if the string is <see cref="Null.NullString"/> or <c>null</c>.
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="size">The size of the field in the database.</param>
        /// <returns>A SqlParameter with the correct value, type, and capacity.</returns>
        public static SqlParameter CreateVarcharParam(string parameterName, string value, int size)
        {
            SqlParameter param = new SqlParameter(parameterName, SqlDbType.VarChar, size);
            if (value == null)
            {
                param.Value = DBNull.Value;
            }
            else
            {
                if (value.Length > size)
                {
                    value = value.Substring(0, size);
                }
                param.Value = value;
            }
            return param;
        }

        /// <summary>
        /// Creates a String (nvarchar) SQL param, setting and checking the bounds of the value within the parameter.
        /// Sets the value to DBNull if the string is <see cref="Null.NullString"/> or <c>null</c>.
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="size">The size of the field in the database.</param>
        /// <returns>A SqlParameter with the correct value, type, and capacity.</returns>
        public static SqlParameter CreateNvarcharParam(string parameterName, string value, int size)
        {
            SqlParameter param = new SqlParameter(parameterName, SqlDbType.NVarChar, size);
            if (value == null)
            {
                param.Value = DBNull.Value;
            }
            else
            {
                if (value.Length > size)
                {
                    value = value.Substring(0, size);
                }
                param.Value = value;
            }
            return param;
        }

        public static SqlParameter CreateNtextParam(string parameterName, string value)
        {
            SqlParameter param = new SqlParameter(parameterName, SqlDbType.NText);
            param.Value = value ?? (object)DBNull.Value;
            return param;
        }

        /// <summary>
        /// Creates a GUID (uniqueidentifier) SQL param, setting the value to DBNull if the Guid is <see cref="Guid.Empty"/>, 
        /// <see cref="Null.NullGuid"/>, or <see cref="Nullable{T}"/> without a value.
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A SqlParameter with the correct value and type.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Private API, could be used in the future")]
        public static SqlParameter CreateGuidParam(string parameterName, Guid? value)
        {
            SqlParameter param = new SqlParameter(parameterName, SqlDbType.UniqueIdentifier);
            if (!value.HasValue || value.Equals(Null.NullGuid) || value.Equals(Guid.Empty))
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = value;
            }
            return param;
        }

        /// <summary>
        /// Creates a DateTime SQL param, setting the value to DBNull if the DateTime is its initial value, <see cref="Null.NullDate"/>,
        /// <see cref="DateTime.MinValue"/>, <see cref="DateTime.MaxValue"/>, or <see cref="Nullable{T}"/> without a value.
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A SqlParameter with the correct value and type.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Private API, could be used in the future")]
        public static SqlParameter CreateDateTimeParam(string parameterName, DateTime? value)
        {
            SqlParameter param = new SqlParameter(parameterName, SqlDbType.DateTime);
            if (!value.HasValue || value.Equals(Null.NullDate) || value.Equals(new DateTime()) || value.Equals(DateTime.MaxValue) || value.Equals(DateTime.MinValue))
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = value;
            }
            return param;
        }

        /// <summary>
        /// Creates a DateTime SQL param, setting the value to DBNull if the DateTime is its initial value, <see cref="Null.NullDate"/>,
        /// <see cref="DateTime.MinValue"/>, <see cref="DateTime.MaxValue"/>, or <see cref="Nullable{T}"/> without a value.
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A SqlParameter with the correct value and type.</returns>
        public static SqlParameter CreateDateTimeParam(string parameterName, string value)
        {
            SqlParameter param = new SqlParameter(parameterName, SqlDbType.DateTime);
            if (!HasValue(value))
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
            }
            return param;
        }

        /// <summary>
        /// Creates an Integer SQL param, setting the value to DBNull if the value is <see cref="Null.NullInteger"/>, 
        /// <see cref="int.MaxValue"/>, <see cref="int.MinValue"/>, or <see cref="Nullable{T}"/> without a value.
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A SqlParameter with the correct value and type.</returns>
        public static SqlParameter CreateIntegerParam(string parameterName, int? value)
        {
            SqlParameter param = new SqlParameter(parameterName, SqlDbType.Int);
            if (!value.HasValue || value.Equals(int.MaxValue) || value.Equals(int.MinValue))
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = value;
            }
            return param;
        }

        /// <summary>
        /// Creates an Bit(bool) SQL param
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A SqlParameter with the correct value and type.</returns>
        public static SqlParameter CreateBitParam(string parameterName, bool? value)
        {
            SqlParameter param = new SqlParameter(parameterName, SqlDbType.Bit);
            param.Value = value;
            return param;
        }

        /// <summary>
        /// Creates an Float(double) SQL param, setting the value to DBNull if the value is <see cref="Null.NullDouble"/>, 
        /// <see cref="double.MaxValue"/>, <see cref="double.MinValue"/>, or <see cref="Nullable{T}"/> without a value.
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A <see cref="SqlParameter" /> with the correct value and type.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Private API, could be used in the future")]
        public static SqlParameter CreateDoubleParam(string parameterName, double? value)
        {
            SqlParameter param = new SqlParameter(parameterName, SqlDbType.Float);
            if (!value.HasValue || value.Equals(Null.NullDouble) || value.Equals(double.MaxValue) || value.Equals(double.MinValue))
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = value;
            }
            return param;
        }

        #endregion
    }
}

