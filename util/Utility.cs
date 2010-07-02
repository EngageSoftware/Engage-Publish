//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Util
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Entities.Tabs;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Services.Search;

    using Engage.Dnn.Publish.Data;

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
        Author, 

        /// <summary>
        /// Displays the Stats name.
        /// </summary>
        Stats
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

    // public enum PublishSyndicationModes
    // {
    // Publisher = 0,
    // Subscriber = 1
    // }
    public enum CategoryFeatureOption
    {
        RandomlyDisplayArticle = 0
    }

    /// <summary>
    /// Summary description for Utility.
    /// </summary>
    public static class Utility
    {
        public const string ActiveForumsDefinitionModuleName = "Active Forums";

        public const string AdminContainer = "AdminContainer";

        public const string CacheKeyPublishAdminCategorySort = "CacheKeyPublishAdminCategorySort_";

        public const string CacheKeyPublishAllChildCategories = "CacheKeyPublishAllChildCategories_";

        public const string CacheKeyPublishArticle = "PublishCacheKeyArticleDisplay_";

        public const string CacheKeyPublishArticleComments = "CacheKeyPublishArticleComments_";

        public const string CacheKeyPublishArticleTags = "CacheKeyPublishArticleTags_";

        public const string CacheKeyPublishArticleVersion = "CacheKeyPublishArticleVersion_";

        public const string CacheKeyPublishAuthorCommentCount = "CacheKeyPublishAuthorCommentCount_";

        public const string CacheKeyPublishCategory = "CacheKeyPublishCategory_";

        public const string CacheKeyPublishCategoryDisplay = "PublishCacheKeyCategoryDisplay_";

        public const string CacheKeyPublishCategoryFeature = "PublishCacheKeyCategoryFeature_";

        public const string CacheKeyPublishCategoryNLevels = "PublishCacheKeyCategoryNLevels_";

        public const string CacheKeyPublishCategoryVersion = "CacheKeyPublishCategoryVersion_";

        public const string CacheKeyPublishChildCategories = "CacheKeyPublishChildCategories_";

        public const string CacheKeyPublishChildCategoriesItemType = "CacheKeyPublishChildCategoriesItemType_";

        public const string CacheKeyPublishChildItemRelationships = "CacheKeyPublishChildItemRelationships_";

        public const string CacheKeyPublishCustomDisplay = "PublishCacheKeyCustomDisplay_";

        public const string CacheKeyPublishDisplayOnCurrentPage = "CacheKeyPublishDisplayOnCurrentPage_";

        public const string CacheKeyPublishForceDisplayOn = "CacheKeyPublishForceDisplayOn_";

        public const string CacheKeyPublishGetTagsByPortal = "CacheKeyPublishGetTagsByPortal_";

        public const string CacheKeyPublishGetTagsByString = "CacheKeyPublishGetTagsByString_";

        public const string CacheKeyPublishItem = "CacheKeyPublishItem_";

        public const string CacheKeyPublishItemParentCategoryId = "CacheKeyPublishItemParentCategoryId_";

        public const string CacheKeyPublishItemRelationships = "CacheKeyPublishItemRelationships_";

        public const string CacheKeyPublishItemTypeId = "CacheKeyPublishItemTypeId_";

        public const string CacheKeyPublishItemTypeIntForItemId = "CacheKeyPublishItemTypeIntForItemId_";

        public const string CacheKeyPublishItemTypeName = "CacheKeyPublishItemTypeName_";

        public const string CacheKeyPublishItemTypeNameItemId = "CacheKeyPublishItemTypeNameItemId_";

        public const string CacheKeyPublishItemTypeStringForItemVersionId = "CacheKeyPublishItemTypeStringForItemVersionId_";

        public const string CacheKeyPublishItemTypesDT = "CacheKeyPublishItemTypesDT_";

        public const string CacheKeyPublishItemVersionSetting = "CacheKeyPublishItemVersionSetting_";

        public const string CacheKeyPublishItemVersionSettings = "CacheKeyPublishItemVersionSettings_";

        public const string CacheKeyPublishItemVersionSettingsByModuleId = "CacheKeyPublishItemVersionSettingsByModuleId_";

        public const string CacheKeyPublishItemVersionSettingsByPortalId = "CacheKeyPublishItemVersionSettingsByPortalId_";

        public const string CacheKeyPublishItemsFromTags = "CacheKeyPublishItemsFromTags_";

        public const string CacheKeyPublishItemsFromTagsPage = "CacheKeyPublishItemsFromTagsPage_";

        public const string CacheKeyPublishPopularTags = "CacheKeyPublishPopularTags_";

        public const string CacheKeyPublishPopularTagsCount = "CacheKeyPublishPopularTagsCount_";

        public const string CacheKeyPublishTag = "PublishCacheKeyTag_";

        public const string CacheKeyPublishTagById = "PublishCacheKeyTagById_";

        public const string CacheKeyPublishTopLevelCategories = "CacheKeyPublishTopLevelCategories_";

        public const string DnnFriendlyModuleName = "Engage: Publish";

        public const string DnnFriendlyModuleNameTextHTML = "Engage: Text/HTML";

        public const string DnnTagsFriendlyModuleName = "Engage: Publish Tag Cloud";

        public const string MediaFileTypes = "jpg,jpeg,jpe,gif,bmp,png,swf";

        public const string PublishAdminRole = "PublishAdminRole";

        public const string PublishAllowRichTextDescriptions = "PublishAllowRichTextDescriptions";

        public const string PublishArticleEditHeight = "PublishArticleEditHeight";

        public const string PublishArticleEditWidth = "PublishArticleEditWidth";

        public const string PublishAuthorCategoryEdit = "PublishAuthorCategoryEdit";

        public const string PublishAuthorRole = "PublishAuthorRole";

        public const string PublishAutoApproveContent = "PublishAutoApproveContent";

        public const string PublishAutoArchiveContent = "PublishAutoArchiveContent";

        public const string PublishCacheKeys = "PublishCacheKeys";

        public const string PublishCacheTime = "PublishCacheTime";

        public const string PublishComment = "PublishComment";

        public const string PublishCommentAnonymous = "PublishCommentAnonymous";

        public const string PublishCommentApproval = "PublishCommentApproval";

        public const string PublishCommentAutoApprove = "PublishCommentAutoApprove";

        public const string PublishCommentEmailAuthor = "PublishCommentEmailAuthor";

        public const string PublishDefaultAdminPagingSize = "PublishDefaultAdminPagingSize";

        public const string PublishDefaultCategory = "PublishDefaultCategory";

        public const string PublishDefaultComments = "PublishDefaultComments";

        public const string PublishDefaultDisplayPage = "PublishDefaultDisplayPage";

        public const string PublishDefaultEmailAFriend = "PublishDefaultEmailAFriend";

        public const string PublishDefaultPrinterFriendly = "PublishDefaultPrinterFriendly";

        public const string PublishDefaultRatings = "PublishDefaultRatings";

        public const string PublishDefaultReturnToList = "PublishDefaultReturnToList";

        public const string PublishDefaultRichTextDescriptions = "PublishDefaultRichTextDescriptions";

        public const string PublishDefaultShowAuthor = "PublishDefaultShowAuthor";

        public const string PublishDefaultShowTags = "PublishDefaultShowTags";

        public const string PublishDefaultTagPage = "PublishDefaultTagPage";

        public const string PublishDefaultTextHtmlCategory = "PublishDefaultTextHtmlCategory";

        public const string PublishDescriptionEditHeight = "PublishDescriptionEditHeight";

        public const string PublishDescriptionEditWidth = "PublishDescriptionEditWidth";

        public const string PublishEmail = "PublishEmail";

        public const string PublishEmailNotificationRole = "PublishEmailNotificationRole";

        public const string PublishEnableArticlePaging = "PublishEnableArticlePaging";

        // publisher/subscriber

        // Community Settings
        public const string PublishEnableCommunityCredit = "PublishEnableCommunityCredit";

        public const string PublishEnableDisplayNameAsHyperlink = "PublishEnableDisplayNameAsHyperlink";

        public const string PublishEnablePing = "PublishEnablePing";

        public const string PublishEnablePublishFriendlyUrls = "PublishEnablePublishFriendlyUrls";

        public const string PublishEnableSimpleGalleryIntegration = "PublishEnableSimpleGalleryIntegration";

        public const string PublishEnableTags = "PublishEnableTags";

        public const string PublishEnableUltraMediaGalleryIntegration = "PublishEnableUltraMediaGalleryIntegration";

        public const string PublishEnableVenexusSearch = "PublishEnableVenexusSearch";

        public const string PublishEnableViewTracking = "PublishEnableViewTracking";

        public const string PublishEnableWlwSupport = "PublishEnableWLWSupport";

        // Forum settings
        public const string PublishForumProviderType = "PublishForumProviderType";

        public const string PublishPingChangedUrl = "PublishPingChangedUrl";

        public const string PublishPingServers = "PublishPingServers";

        public const string PublishPopularTagCount = "PublishPopularTagCount";

        public const string PublishRating = "PublishRating";

        public const string PublishRatingAnonymous = "PublishRatingAnonymous";

        public const string PublishRatingMaximum = "PublishRatingMaximum";

        public const string PublishSessionReturnToList = "PublishSessionReturnToList";

        public const string PublishSetup = "PublishSetup";

        public const string PublishShortItemLink = "PublishShortItemLink";

        public const string PublishShowItemId = "PublishShowItemId";

        public const string PublishSubscriberKey = "PublishSubscriberKey";

        public const string PublishSubscriberUrl = "PublishSubscriberUrl";

        public const string PublishThumbnailDisplayOption = "PublishThumbnailDisplayOption";

        public const string PublishThumbnailSubdirectory = "PublishThumbnailSubdirectory";

        public const string PublishUseApprovals = "PublishUseApprovals";

        public const string PublishUseEmbeddedArticles = "PublishUseEmbeddedArticles";

        public const int RedirectInSeconds = 10;

        public const string SimpleGalleryDefinitionModuleName = "SimpleGallery";

        public const string SimpleGalleryFriendlyName = "SimpleGallery";

        public const string UltraMediaGalleryDefinitionModuleName = "BizModules - UltraPhotoGallery";

        public const string UltraMediaGalleryFriendlyName = "BizModules - UltraPhotoGallery";

        /*******************Cache Key Constants********************/

        // cache keys for Categories
        private static readonly object CacheLock = new object();

        private static readonly char[] TagSeparators = {
                                                           ';', ','
                                                       };

        public static string ApplicationUrl
        {
            get
            {
                // todo: should application URL use PortalId?
                if (HttpContext.Current == null)
                {
                    return String.Empty;
                }

                return HttpContext.Current.Request.ApplicationPath == "/" ? String.Empty : HttpContext.Current.Request.ApplicationPath;
            }
        }

        public static string DesktopModuleFolderName
        {
            get
            {
                var sb = new StringBuilder(128);
                sb.Append("/DesktopModules/");
                sb.Append(Globals.GetDesktopModuleByName(DnnFriendlyModuleName).FolderName);
                sb.Append("/");
                return sb.ToString();
            }
        }

        public static bool IsSimpleGalleryInstalled
        {
            get
            {
                var desktopModules = new DesktopModuleController();

                // return desktopModules.GetDesktopModuleByFriendlyName(SimpleGalleryFriendlyName) != null;
                return desktopModules.GetDesktopModuleByModuleName(SimpleGalleryDefinitionModuleName) != null;
            }
        }

        public static bool IsUltraMediaGalleryInstalled
        {
            get
            {
                var desktopModules = new DesktopModuleController();

                // return desktopModules.GetDesktopModuleByFriendlyName(UltraMediaGalleryDefinitionFriendlyName) != null;
                return desktopModules.GetDesktopModuleByModuleName(UltraMediaGalleryDefinitionModuleName) != null;
            }
        }

        public static string LocalSharedResourceFile
        {
            get { return "~" + DesktopModuleFolderName + Localization.LocalResourceDirectory + "/" + Localization.LocalSharedResourceFile; }
        }

        public static string WebServiceUrl
        {
            get
            {
                var url = new StringBuilder();
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

        [Obsolete("This should only be used by Utility.GetValueFromCache, use that method for caching instead")]
        public static void AddCacheKey(string keyName, int portalId)
        {
            string cacheKey = PublishCacheKeys + portalId.ToString(CultureInfo.InvariantCulture);

            lock (CacheLock)
            {
                List<string> cacheList = DataCache.GetCache(cacheKey) as List<string> ?? new List<string>();
                if (!cacheList.Contains(keyName))
                {
                    cacheList.Add(keyName);
                    DataCache.SetCache(cacheKey, cacheList);
                }
            }
        }

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

            string returnUrl = String.Empty;
            if (HttpContext.Current != null)
            {
                returnUrl = "returnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl);
            }

            return Globals.NavigateURL(
                tabId, 
                String.Empty, 
                "ctl=" + AdminContainer, 
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

            string returnUrl = String.Empty;
            if (HttpContext.Current != null)
            {
                returnUrl = "returnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl);
            }

            return Globals.NavigateURL(
                tabId, 
                String.Empty, 
                "ctl=" + AdminContainer, 
                "mid=" + moduleId.ToString(CultureInfo.InvariantCulture), 
                "adminType=" + type.Name + "Edit", 
                "versionId=" + i.ItemVersionId.ToString(CultureInfo.InvariantCulture), 
                returnUrl);
        }

        public static void ClearPublishCache(int portalId)
        {
            string cacheKey = PublishCacheKeys + portalId.ToString(CultureInfo.InvariantCulture);

            lock (CacheLock)
            {
                // ArrayList al = DataCache.GetCache(cacheKey) as ArrayList;
                var cacheList = DataCache.GetCache(cacheKey) as List<string>;
                if (cacheList != null)
                {
                    foreach (string s in cacheList)
                    {
                        DataCache.RemoveCache(s);
                    }
                }

                DataCache.RemoveCache(cacheKey);
            }
        }

        /// <summary>
        /// Creates an Bit(bool) SQL param
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A SqlParameter with the correct value and type.</returns>
        public static SqlParameter CreateBitParam(string parameterName, bool? value)
        {
            var param = new SqlParameter(parameterName, SqlDbType.Bit)
                {
                    Value = value
                };
            return param;
        }

        /// <summary>
        /// Creates a DateTime SQL param, setting the value to DBNull if the DateTime is its initial value, <see cref="Null.NullDate"/>,
        /// <see cref="DateTime.MinValue"/>, <see cref="DateTime.MaxValue"/>, or <see cref="Nullable{T}"/> without a value.
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A SqlParameter with the correct value and type.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Private API, could be used in the future")]
        public static SqlParameter CreateDateTimeParam(string parameterName, DateTime? value)
        {
            var param = new SqlParameter(parameterName, SqlDbType.DateTime);
            if (!value.HasValue || value.Equals(Null.NullDate) || value.Equals(new DateTime()) || value.Equals(DateTime.MaxValue) ||
                value.Equals(DateTime.MinValue))
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
            var param = new SqlParameter(parameterName, SqlDbType.DateTime);
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
        /// Creates an Float(double) SQL param, setting the value to DBNull if the value is <see cref="Null.NullDouble"/>, 
        /// <see cref="double.MaxValue"/>, <see cref="double.MinValue"/>, or <see cref="Nullable{T}"/> without a value.
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A <see cref="SqlParameter" /> with the correct value and type.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Private API, could be used in the future")]
        public static SqlParameter CreateDoubleParam(string parameterName, double? value)
        {
            var param = new SqlParameter(parameterName, SqlDbType.Float);
            if (!value.HasValue || value.Equals(Null.NullDouble) || value.Equals(Double.MaxValue) || value.Equals(Double.MinValue))
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
        /// Creates a GUID (uniqueidentifier) SQL param, setting the value to DBNull if the Guid is <see cref="Guid.Empty"/>, 
        /// <see cref="Null.NullGuid"/>, or <see cref="Nullable{T}"/> without a value.
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A SqlParameter with the correct value and type.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Private API, could be used in the future")]
        public static SqlParameter CreateGuidParam(string parameterName, Guid? value)
        {
            var param = new SqlParameter(parameterName, SqlDbType.UniqueIdentifier);
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
        /// Creates an Integer SQL param, setting the value to DBNull if the value is <see cref="Null.NullInteger"/>, 
        /// <see cref="int.MaxValue"/>, <see cref="int.MinValue"/>, or <see cref="Nullable{T}"/> without a value.
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A SqlParameter with the correct value and type.</returns>
        public static SqlParameter CreateIntegerParam(string parameterName, int? value)
        {
            var param = new SqlParameter(parameterName, SqlDbType.Int);
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

        public static SqlParameter CreateNtextParam(string parameterName, string value)
        {
            var param = new SqlParameter(parameterName, SqlDbType.NText)
                {
                    Value = value ?? (object)DBNull.Value
                };
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
            var param = new SqlParameter(parameterName, SqlDbType.NVarChar, size);
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
        /// Creates a String (varchar) SQL param, setting and checking the bounds of the value within the parameter.
        /// Sets the value to DBNull if the string is <see cref="Null.NullString"/> or <c>null</c>.
        /// </summary>
        /// <param name="parameterName">Name of the parameter in the SQL Stored Procedure.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="size">The size of the field in the database.</param>
        /// <returns>A SqlParameter with the correct value, type, and capacity.</returns>
        public static SqlParameter CreateVarcharParam(string parameterName, string value, int size)
        {
            var param = new SqlParameter(parameterName, SqlDbType.VarChar, size);
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

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Code Analysis doesn't see validation")]
        public static int GetArticleId(SearchResultsInfo result)
        {
            if (result == null || result.SearchKey.IndexOf("Article", StringComparison.Ordinal) == -1)
            {
                return 0;
            }

            int id = 0;
            int index = result.SearchKey.IndexOf("-", StringComparison.Ordinal);
            if (index > -1)
            {
                string s = result.SearchKey.Substring(index + 1);
                if (Int32.TryParse(s, out id))
                {
                    return id;
                }

                id = 0;
            }

            return id;
        }

        public static bool GetBooleanPortalSetting(string settingName, int portalId, bool defaultValue)
        {
            return _GetBooleanPortalSetting(settingName, portalId, defaultValue).Value;
        }

        public static string GetCurrentCultureDateTime(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
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

        public static DataTable GetDisplayTabIds(string[] moduleNames)
        {
            // TODO: we should cache this info as it gets used on GetItemLink requests now.
            int portalId = UserController.GetCurrentUserInfo().PortalID;

            var mc = new ModuleController();
            var tc = new TabController();
            ArrayList al; // = null;

            // only want a unique list of ItemIds
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
                                // check if the Tab has an Overrideable instance of Publish

                                // ModuleSettingsBase msb = new ModuleSettingsBase();
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
                                    // if the tabmodulesettings didn't contain overrideable then the module hasn't been configured and is overrideable by default.
                                    list.Add(ti.TabID, ti);
                                }
                            }
                        }
                    }
                }
            }

            // need to copy items to here in order to sort
            al = new ArrayList();
            foreach (TabInfo ti in list.Values)
            {
                al.Add(ti);
            }

            al.Sort(new TabInfoNameComparer(true));

            var dt = new DataTable
                {
                    Locale = CultureInfo.InvariantCulture
                };
            dt.Columns.Add("TabId", typeof(int));
            dt.Columns.Add("TabName", typeof(string));

            // finally create listitems from the sorted list
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
            // TODO: this should be cached.
            int portalId = UserController.GetCurrentUserInfo().PortalID;

            var mc = new ModuleController();
            var tc = new TabController();
            ArrayList al; // = null;

            // only want a unique list of ItemIds
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

            // need to copy items to here in order to sort
            al = new ArrayList();
            foreach (TabInfo ti in list.Values)
            {
                al.Add(ti);
            }

            al.Sort(new TabInfoNameComparer(true));

            var dt = new DataTable
                {
                    Locale = CultureInfo.InvariantCulture
                };
            dt.Columns.Add("TabId", typeof(int));
            dt.Columns.Add("TabName", typeof(string));

            // finally create listitems from the sorted list
            foreach (TabInfo ti in al)
            {
                DataRow r = dt.NewRow();
                r["TabId"] = ti.TabID;
                r["TabName"] = ti.TabName;
                dt.Rows.Add(r);
            }

            return dt;
        }

        // This will take a datetime string and convert it to the InvariantCulture datetime string.
        public static string GetInvariantDateTime(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
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

        // This will take a datetime string and convert it to the InvariantCulture datetime string.

        public static string GetItemLinkUrl(object itemId, int portalId)
        {
            if (itemId != null)
            {
                int id = Convert.ToInt32(itemId, CultureInfo.InvariantCulture);
                int typeId = Item.GetItemTypeId(id, portalId);
                ItemType type = ItemType.GetFromId(typeId, typeof(ItemType));

                Item i;
                if (type.Name == ItemType.Article.Name)
                {
                    i = Article.GetArticle(id, portalId);
                }
                else
                {
                    i = Category.GetCategory(id, portalId);
                }

                if (i != null)
                {
                    return GetItemLinkUrl(i);
                }

                // else there is no current version of this ITEM. Can't view it currently because ItemLink.aspx doesn't
                // support versions. hk
            }

            return String.Empty;
        }

        public static string GetItemLinkUrl(Item item)
        {
            if (item != null && item.IsLinkable() && item.ApprovalStatusId != ApprovalStatus.Approved.GetId())
            {
                return GetItemVersionLinkUrl(item);
                    
                    // Globals.NavigateURL(displayTabId, "", "VersionId=" + itemVersionId.ToString(CultureInfo.InvariantCulture) + "&modid=" + version.ModuleId.ToString());
            }

            if (item != null)
            {
                return GetItemLinkUrl(item, item.PortalId, -1, -1, -1, String.Empty);
            }

            return string.Empty;
        }

        public static string GetItemLinkUrl(int itemId, int portalId, int tabId, int moduleId)
        {
            return GetItemLinkUrl(itemId, portalId, tabId, moduleId, 1, String.Empty);
        }

        public static string GetItemLinkUrl(int itemId, int portalId, int tabId, int moduleId, int pageId, string cultureName)
        {
            int typeId = Item.GetItemTypeId(itemId, portalId);
            ItemType type = ItemType.GetFromId(typeId, typeof(ItemType));
            Item item; // = null;
            if (type.Name == ItemType.Article.Name)
            {
                item = Article.GetArticle(itemId, portalId);
            }
            else
            {
                item = Category.GetCategory(itemId, portalId);
            }

            if (item != null)
            {
                return GetItemLinkUrl(item, portalId, tabId, moduleId, pageId, cultureName);
            }

            return String.Empty;
        }

        public static string GetItemLinkUrl(Item item, int portalId, int tabId, int moduleId, int pageId, string cultureName)
        {
            string returnUrl = String.Empty;

            if (item != null)
            {
                if (!item.IsLinkable())
                {
                    tabId = ModuleBase.DefaultDisplayTabIdForPortal(portalId);
                }

                if (HostSettings.GetHostSetting("UseFriendlyUrls") == "Y" && ModuleBase.EnablePublishFriendlyUrlsForPortal(item.PortalId))
                {
                    returnUrl = GetFriendlyItemLinkUrl(item, tabId, moduleId, pageId, portalId, cultureName);
                }
                else
                {
                    returnUrl = GetNonFriendlyLinkUrl(item, tabId, moduleId, pageId, portalId);
                }
            }

            return returnUrl;
        }

        public static string GetItemVersionLinkUrl(Item item)
        {
            string returnUrl = Globals.NavigateURL(
                item.DisplayTabId, string.Empty, "VersionId=" + item.ItemVersionId.ToString(CultureInfo.InvariantCulture) + "&modid=" + item.ModuleId);

            return returnUrl;
        }

        public static IDataReader GetModuleByModuleId(int moduleId)
        {
            return DataProvider.Instance().GetModulesByModuleId(moduleId);
        }

        public static int GetModuleIdFromDisplayTabId(int displayTabId, int portalId, string modules)
        {
            var mc = new ModuleController();
            int modid = -1;
            ArrayList al = mc.GetModulesByDefinition(portalId, modules);
            foreach (ModuleInfo mi in al)
            {
                var tc = new TabController();
                TabInfo ti = tc.GetTab(mi.TabID, mi.PortalID, false);
                if (ti != null && ti.TabID == displayTabId)
                {
                    // TODO: check if the module is overrideable, if so then set the moduleid
                    object o = mc.GetTabModuleSettings(mi.TabModuleID)["Overrideable"];

                    if (o != null)
                    {
                        if (Convert.ToBoolean(o, CultureInfo.InvariantCulture))
                        {
                            modid = mi.ModuleID;
                            return modid;
                        }
 // get out there is a module on the page that is configured overrideable.
                    }

                    // set the moduleid to the last one, in case we get to the end and haven't found a match
                    modid = mi.ModuleID;

                    // else we just move on                   
                }
            }

            return modid;
        }

        public static PortalSettings GetPortalSettings(int portalId)
        {
            return new PortalSettings(-1, GetPortalAliasInfo(portalId));
        }

        public static string GetPortalUrl(int portalId)
        {
            PortalAliasInfo pai = GetPortalAliasInfo(portalId);
            if (pai != null)
            {
                return "http://" + pai.HTTPAlias;
            }

            return ApplicationUrl;
        }

        public static ModuleInfo GetPublishModuleInfo(int moduleId)
        {
            // 4.6.2 and later this would work, but for backwards compatibility we can't use.
            // ModuleController mc = new ModuleController();
            // foreach (ModuleInfo info in mc.GetModuleTabs(moduleId))
            // {
            // if (info.FriendlyName == Utility.DnnFriendlyModuleName)
            // {
            // return info;
            // }
            // }
            var mc = new ModuleController();
            ModuleInfo info = null;
            using (IDataReader dr = DataProvider.Instance().GetModuleInfo(moduleId))
            {
                if (dr.Read())
                {
                    var tabId = (int)dr["Tabid"];
                    info = mc.GetModule(moduleId, tabId);
                }
            }

            return info;
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Code Analysis doesn't see validation")]
        public static IList GetRandomItem(IList view)
        {
            if (view == null || view.Count <= 1)
            {
                return view;
            }

            int seed = GetRandomSeed();
            var random = new Random(seed);

            while (view.Count > 1)
            {
                int i = random.Next(0, view.Count);

                // System.Diagnostics.Debug.WriteLine(i);
                // System.Diagnostics.Debug.WriteLine(view[i]["ItemID"]);
                view.RemoveAt(i);
            }

            // System.Diagnostics.Debug.WriteLine(view[0]["ItemID"]);
            return view;
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Does not return class state information")]
        public static int GetRandomSeed()
        {
            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            var randomBytes = new byte[4];

            // Generate 4 random bytes.
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            return (randomBytes[0] & 0x7f) << 24 | randomBytes[1] << 16 | randomBytes[2] << 8 | randomBytes[3];
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

        public static int GetTabModuleSettingAsInt(int moduleId, string settingName, int defaultValue)
        {
            using (IDataReader dr = GetModuleByModuleId(moduleId))
            {
                if (dr.Read())
                {
                    int value;
                    var tabModuleId = (int)dr["TabModuleId"];
                    object valueObj = (new ModuleController()).GetTabModuleSettings(tabModuleId)[settingName];
                    if (valueObj != null && int.TryParse(valueObj.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                    {
                        return value;
                    }
                }
            }

            return defaultValue;
        }

        public static string GetTabModuleSettingAsString(int moduleId, string settingName, string defaultValue)
        {
            using (IDataReader dr = GetModuleByModuleId(moduleId))
            {
                if (dr.Read())
                {
                    var tabModuleId = (int)dr["TabModuleId"];
                    object value = (new ModuleController()).GetTabModuleSettings(tabModuleId)[settingName];
                    if (value != null)
                    {
                        return value.ToString();
                    }
                }
            }

            return defaultValue;
        }

        public static char[] GetTagSeparators()
        {
            return (char[])TagSeparators.Clone();
        }

        public static Uri GetThumbnailLibraryMapPath(int portalId)
        {
            var path = new Uri(
                Path.Combine(GetPortalSettings(portalId).HomeDirectoryMapPath, ModuleBase.ThumbnailSubdirectoryForPortal(portalId)), UriKind.Absolute);

            // make sure it exists before we tell people about it.  BD
            if (!Directory.Exists(path.LocalPath))
            {
                Directory.CreateDirectory(path.LocalPath);
            }

            return path;
        }

        public static Uri GetThumbnailLibraryPath(int portalId)
        {
            return new Uri(
                Path.Combine(GetPortalSettings(portalId).HomeDirectory, ModuleBase.ThumbnailSubdirectoryForPortal(portalId)), UriKind.Relative);
        }

        /// <summary>
        /// Gets the value for the given <paramref name="cacheKey"/> from cache, or calls <paramref name="getValue"/> if the key is not present in the cache.  
        /// Stores the result in the cache for future retrievals, and adds the <paramref name="cacheKey"/> to the list of Publish keys.
        /// </summary>
        /// <typeparam name="T">The type of the value being retrieved from the cache</typeparam>
        /// <param name="portalId">The portal id.</param>
        /// <param name="cacheKey">The cache key with which the value should be stored and retrieved.</param>
        /// <param name="getValue">A function to get the value if it is not present in the cache.</param>
        /// <returns>The requested value calculated by <paramref name="getValue"/>, retrieved from the cache if possible </returns>
        public static T GetValueFromCache<T>(int portalId, string cacheKey, Func<T> getValue)
        {
            T value;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object cachedValue = DataCache.GetCache(cacheKey);
                value = cachedValue != null ? (T)cachedValue : getValue();

                DataCache.SetCache(cacheKey, value, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                AddCacheKey(cacheKey, portalId);
            }
            else
            {
                value = getValue();
            }

            return value;
        }

        public static bool HasValue(string value)
        {
            return !String.IsNullOrEmpty(value) && value.Trim().Length > 0;
        }

        public static bool IsDisabled(int itemId, int portalId)
        {
            // check if an item is disabled
            int typeId = Item.GetItemTypeId(itemId, portalId);
            Item item = Item.GetItem(itemId, portalId, typeId, true);
            return item.Disabled;
        }

        public static bool IsPageOverrideable(int portalId, int displayTabId)
        {
            // int portalId = UserController.GetCurrentUserInfo().PortalID;
            try
            {
                var mc = new ModuleController();
                var tc = new TabController();
                TabInfo tab = tc.GetTab(displayTabId, portalId, false);
                if (tab.IsDeleted)
                {
                    // The tab is in the recycle bin.
                    return false;
                }

                Dictionary<int, ModuleInfo> modules = mc.GetTabModules(displayTabId);
                foreach (ModuleInfo mi in modules.Values)
                {
                    if (mi.FriendlyName == DnnFriendlyModuleName)
                    {
                        // found one
                        object o = mc.GetTabModuleSettings(mi.TabModuleID)["Overrideable"];

                        if (o != null)
                        {
                            if (Convert.ToBoolean(o, CultureInfo.InvariantCulture))
                            {
                                return true;
                            }
 // get out there is a module on the page that is configured overrideable.
                        }
                        else
                        {
                            // if there isn't a setting for overrideable, it is overrideable.
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception)
            {
                // if we get an error here then return false as it's not linkable
                return false;
            }
        }

        public static bool IsPingEnabledForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(PublishEnablePing + portalId.ToString(CultureInfo.InvariantCulture));
            return HasValue(s) && Convert.ToBoolean(s, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Determines whether the <paramref name="leftHandSide"/> is greater than or equal to the <paramref name="rightHandSide"/>.
        /// </summary>
        /// <param name="leftHandSide">The left hand side version.</param>
        /// <param name="rightHandSide">The right hand side version.</param>
        /// <returns>
        /// 	<c>true</c> if the <paramref name="leftHandSide"/> if greater or equal to the <paramref name="rightHandSide"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref cref="leftHandSide"/> or <paramref cref="rightHandSide"/> is null.</exception>
        public static bool IsVersionGreaterOrEqual(List<int> leftHandSide, List<int> rightHandSide)
        {
            if (leftHandSide != null && rightHandSide != null)
            {
                if (leftHandSide.Count != rightHandSide.Count)
                {
                    int sizeDifference = leftHandSide.Count - rightHandSide.Count;
                    while (sizeDifference > 0)
                    {
                        // leftHandSide has more
                        rightHandSide.Add(0);
                        sizeDifference--;
                    }

                    while (sizeDifference < 0)
                    {
                        // rightHandSide has more
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

                return true; // equal
            }

            if (leftHandSide == null)
            {
                throw new ArgumentNullException("leftHandSide");
            }

            throw new ArgumentNullException("rightHandSide");
        }

        /// <exception cref="ArgumentNullException"><paramref name="gridView"/> is null.</exception>
        public static void LocalizeGridView(GridView gridView, string resourceFile)
        {
            if (gridView == null)
            {
                throw new ArgumentNullException("gridView");
            }

            foreach (DataControlField column in gridView.Columns)
            {
                if (!String.IsNullOrEmpty(column.HeaderText))
                {
                    string localizedText = Localization.GetString(column.HeaderText + ".Header", resourceFile);
                    if (!String.IsNullOrEmpty(localizedText))
                    {
                        column.HeaderText = localizedText;
                        column.AccessibleHeaderText = localizedText;
                    }
                }

                if (!String.IsNullOrEmpty(column.FooterText))
                {
                    column.FooterText = Localization.GetString(column.FooterText + ".Header", resourceFile);
                }

                if (!String.IsNullOrEmpty(column.AccessibleHeaderText))
                {
                    column.AccessibleHeaderText = Localization.GetString(column.AccessibleHeaderText + ".Header", resourceFile);
                }

                var buttonField = column as ButtonField;
                if (buttonField != null)
                {
                    if (String.IsNullOrEmpty(buttonField.Text))
                    {
                        buttonField.Text = Localization.GetString(buttonField.Text, resourceFile);
                    }
                }
                else
                {
                    var checkboxField = column as CheckBoxField;
                    if (checkboxField != null)
                    {
                        if (!String.IsNullOrEmpty(checkboxField.Text))
                        {
                            checkboxField.Text = Localization.GetString(checkboxField.Text, resourceFile);
                        }
                    }
                    else
                    {
                        var commands = column as CommandField;
                        if (commands != null)
                        {
                            if (!String.IsNullOrEmpty(commands.CancelText))
                            {
                                commands.CancelText = Localization.GetString(commands.CancelText, resourceFile);
                            }

                            if (!String.IsNullOrEmpty(commands.DeleteText))
                            {
                                commands.DeleteText = Localization.GetString(commands.DeleteText, resourceFile);
                            }

                            if (!String.IsNullOrEmpty(commands.EditText))
                            {
                                commands.EditText = Localization.GetString(commands.EditText, resourceFile);
                            }

                            if (!String.IsNullOrEmpty(commands.InsertText))
                            {
                                commands.InsertText = Localization.GetString(commands.InsertText, resourceFile);
                            }

                            if (!String.IsNullOrEmpty(commands.NewText))
                            {
                                commands.NewText = Localization.GetString(commands.NewText, resourceFile);
                            }

                            if (!String.IsNullOrEmpty(commands.SelectText))
                            {
                                commands.SelectText = Localization.GetString(commands.SelectText, resourceFile);
                            }

                            if (!String.IsNullOrEmpty(commands.UpdateText))
                            {
                                commands.UpdateText = Localization.GetString(commands.UpdateText, resourceFile);
                            }
                        }
                        else
                        {
                            var hyperLinkfield = column as HyperLinkField;
                            if (hyperLinkfield != null)
                            {
                                if (!String.IsNullOrEmpty(hyperLinkfield.Text))
                                {
                                    hyperLinkfield.Text = Localization.GetString(hyperLinkfield.Text, resourceFile);
                                }
                            }
                            else
                            {
                                var imageField = column as ImageField;
                                if (imageField != null)
                                {
                                    if (!String.IsNullOrEmpty(imageField.AlternateText))
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

        public static string OnlyAlphanumericCharacters(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                return input;
            }

            input = input.Trim();
            int length = input.Length;
            var returnString = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                string sCurrent = input.Substring(i, 1);
                if (IsAlphaNumeric(sCurrent))
                {
                    returnString.Append(sCurrent);
                }
            }

            returnString = returnString.Replace(" ", "-"); // Replace(sAns, " ", "-")

            return returnString.ToString();
        }

        /// <summary>
        /// Parses a <see cref="string"/> array into a <see cref="List{T}"/> of <see cref="int"/>.
        /// </summary>
        /// <param name="splitIntegers">A <see cref="string"/> array of integer values.</param>
        /// <exception cref="FormatException">If any value is not an integer</exception>>
        /// <returns>A <see cref="List{T}"/> of the values in <paramref name="splitIntegers"/> parsed as <see cref="int"/>s.</returns>
        public static List<int> ParseIntegerList(string[] splitIntegers)
        {
            var integers = new List<int>(splitIntegers.Length);
            foreach (string integer in splitIntegers)
            {
                if (HasValue(integer))
                {
                    integers.Add(int.Parse(integer, CultureInfo.InvariantCulture));
                }
            }

            return integers;
        }

        // Utility method for replacing YouTube and Flickr tokens in text
        public static string ReplaceTokens(string replaceText)
        {
            // TODO: change : to | (pipe) sometime
            int youTubeLocation = replaceText.ToUpperInvariant().IndexOf("[YOUTUBE:", StringComparison.Ordinal);
            if (youTubeLocation >= 0)
            {
                string afterToken = replaceText.Substring(youTubeLocation + 9);
                int youTubeFinishLocation = afterToken.IndexOf("]", StringComparison.Ordinal);
                string youTubeId = afterToken.Substring(0, youTubeFinishLocation);

                // string youTubeEmbed = "<span class=\"Publish_Video\"><object width=\"425\" height=\"355\"><param name=\"movie\" value=\"http://www.youtube.com/v/" + youTubeId + "\"></param><param name=\"wmode\" value=\"transparent\"></param><embed src=\"http://www.youtube.com/v/" + youTubeId + "\" type=\"application/x-shockwave-flash\" wmode=\"transparent\" width=\"425\" height=\"355\"></embed></object></span>";
                string youTubeEmbed =
                    "<span class=\"Publish_Video\"><object type=\"application/x-shockwave-flash\" width=\"560\" height=\"340\" data=\"http://www.youtube.com/v/" +
                    youTubeId + "&hd=1\"><param name=\"movie\" value=\"http://www.youtube.com/v/" + youTubeId + "&hd=1\" />" +
                    Localization.GetString("NoFlash", LocalSharedResourceFile) + "</object></span>";

                // <object type=\"application/x-shockwave-flash\" data=\"CONTENTHERE\" width=\"300\" height=\"120\"><param name=\"movie\" value=\"CONTENTHERE\" /><p>flash disabled</p></object>
                string fullArticleText = replaceText.Substring(0, youTubeLocation);
                fullArticleText += youTubeEmbed;
                fullArticleText += replaceText.Substring(youTubeLocation + youTubeId.Length + youTubeFinishLocation - 1);
                replaceText = fullArticleText;
                replaceText = ReplaceTokens(replaceText);
            }

            // flickr
            int flickrLocation = replaceText.ToUpperInvariant().IndexOf("[FLICKR|", StringComparison.Ordinal);
            if (flickrLocation >= 0)
            {
                string afterToken = replaceText.Substring(flickrLocation + 8);
                int flickrFinishLocation = afterToken.IndexOf("]", StringComparison.Ordinal);
                string flickrId = afterToken.Substring(0, flickrFinishLocation);

                string flickrEmbed = string.Format(
                    CultureInfo.InvariantCulture, 
                    "<object type=\"application/x-shockwave-flash\" width=\"499\" height=\"333\" data=\"http://www.flickr.com/apps/video/stewart.swf?v=1.173\" classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\"> <param name=\"flashvars\" value=\"intl_lang=en-us&amp;photo_secret=d70f929acb&amp;photo_id={0}&amp;show_info_box=true\"></param> <param name=\"movie\" value=\"http://www.flickr.com/apps/video/stewart.swf?v=1.173\"></param> <param name=\"bgcolor\" value=\"#000000\"></param> <param name=\"allowFullScreen\" value=\"true\"></param><embed type=\"application/x-shockwave-flash\" src=\"http://www.flickr.com/apps/video/stewart.swf?v=1.173\" bgcolor=\"#000000\" allowfullscreen=\"true\" flashvars=\"intl_lang=en-us&amp;photo_secret=d70f929acb&amp;photo_id={0}&amp;flickr_show_info_box=true\" height=\"333\" width=\"499\"></embed></object>", 
                    flickrId);
                string fullArticleText = replaceText.Substring(0, flickrLocation);
                fullArticleText += flickrEmbed;
                fullArticleText += replaceText.Substring(flickrLocation + flickrId.Length + flickrFinishLocation - 1);
                replaceText = fullArticleText;
                replaceText = ReplaceTokens(replaceText);
            }

            return replaceText;
        }

        // public static bool? GetBooleanPortalSetting(string settingName, int portalId)
        // {
        // return _GetBooleanPortalSetting(settingName, portalId, null);
        // }
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

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Code Analysis doesn't see validation")]
        public static DataTable SortDataTable(DataTable dt, string sortOrder)
        {
            if (dt != null && !String.IsNullOrEmpty(sortOrder))
            {
                DataTable dtreturn = dt.Clone();

                sortOrder = sortOrder.Remove(sortOrder.Length - 1, 1);

                // parse through the sortorder list first
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

                // for any rows that weren't in the sort list add them to this list.
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dtreturn.ImportRow(dt.Rows[i]);
                }

                return dtreturn;
            }

            return dt;
        }

        public static void SortDataTableSingleParam(DataTable dt, string sort)
        {
            DataTable newDT = dt.Clone();
            int rowCount = dt.Rows.Count;

            DataRow[] foundRows = dt.Select(null, sort); // Sort with Column name 
            for (int i = 0; i < rowCount; i++)
            {
                var arr = new object[dt.Columns.Count];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    arr[j] = foundRows[i][j];
                }

                DataRow dataRow = newDT.NewRow();
                dataRow.ItemArray = arr;
                newDT.Rows.Add(dataRow);
            }

            // clear the incoming dt 
            dt.Rows.Clear();

            for (int i = 0; i < newDT.Rows.Count; i++)
            {
                var arr = new object[dt.Columns.Count];
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    arr[j] = newDT.Rows[i][j];
                }

                DataRow dataRow = dt.NewRow();
                dataRow.ItemArray = arr;
                dt.Rows.Add(dataRow);
            }
        }

        public static string StripTags(string html)
        {
            return Regex.Replace(html, "<[^>]*>", String.Empty);
        }

        public static string TrimDescription(int length, string description)
        {
            if (description.Length > length)
            {
                description = description.Substring(0, length);
            }

            if (description.Length > 0)
            {
                int lastSpace = description.LastIndexOf(' ');

                if (lastSpace != description.Length - 1 && lastSpace > 0)
                {
                    description = description.Substring(0, lastSpace);
                }
            }

            return description;
        }

        /// <summary>
        /// Converts the given <c>QueryString</c> parameters to the friendly ("/key/value/") format.
        /// </summary>
        /// <param name="queryStringParameters">The query string parameters.</param>
        /// <returns></returns>
        private static string ConvertParametersToFriendly(NameValueCollection queryStringParameters)
        {
            var friendlyParameters = new StringBuilder();

            for (int i = 0; i < queryStringParameters.Count; i++)
            {
                // Friendly URLs start reading parameters with tabId, it must be first
                string key = queryStringParameters.Keys[i];
                if (key.Equals("TABID", StringComparison.OrdinalIgnoreCase))
                {
                    friendlyParameters.Insert(0, "/" + key + "/" + queryStringParameters[i]);
                }
                else
                {
                    friendlyParameters.Append("/");
                    friendlyParameters.Append(key);
                    friendlyParameters.Append("/");
                    friendlyParameters.Append(queryStringParameters[i]);
                }
            }

            return friendlyParameters.ToString();
        }

        /// <summary>
        /// Converts the given <c>QueryString</c> parameters to the non-friendly (<see cref="Globals.NavigateURL"/>) format.
        /// </summary>
        /// <param name="queryStringParameters">The query string parameters.</param>
        /// <returns></returns>
        private static string[] ConvertParametersToNonFriendly(NameValueCollection queryStringParameters)
        {
            string[] results = queryStringParameters.AllKeys;

            for (int i = 0; i < results.Length; i++)
            {
                // add value to key
                results[i] += "=" + queryStringParameters[results[i]];
            }

            return results;
        }

        /// <summary>
        /// Creates a collection of the possible parameters to be added to the <c>QueryString</c> of a URL linking to an item.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <param name="tabId">The tab id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="pageId">The page id.</param>
        /// <param name="portalId">The portal id.</param>
        /// <param name="cultureName">The name of the current culture of the page, or <see cref="string.Empty"/></param>
        /// <returns>
        /// A collection of the possible <c>QueryString</c> parameters to use when linking to an item
        /// </returns>
        private static NameValueCollection CreateParametersForQueryString(
            int itemId, int? tabId, int? moduleId, int pageId, int portalId, string cultureName)
        {
            NameValueCollection queryStringParameters = new NameValueCollection(5);
            queryStringParameters.Add("itemId", itemId.ToString(CultureInfo.InvariantCulture));

            if (tabId.HasValue)
            {
                queryStringParameters.Add("tabId", tabId.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (moduleId.HasValue)
            {
                queryStringParameters.Add("moduleId", moduleId.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (UsePageId(pageId, portalId))
            {
                queryStringParameters.Add("pageId", pageId.ToString(CultureInfo.InvariantCulture));
            }

            if (HasValue(cultureName))
            {
                queryStringParameters.Add("language", cultureName);
            }

            return queryStringParameters;
        }

        /// <summary>
        /// Gets a URL linking to the given item when friendly URLs are turned on
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tabId">The tab id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="pageId">The page id.</param>
        /// <param name="portalId">The portal id.</param>
        /// <param name="cultureName">The name of the current culture of the page, or <see cref="string.Empty"/></param>
        /// <returns>A URL linking to the given item</returns>
        private static string GetFriendlyItemLinkUrl(Item item, int tabId, int moduleId, int pageId, int portalId, string cultureName)
        {
            TabInfo tabInfo;
            var tabController = new TabController();
            int? queryStringModuleId = null;
            int defaultTabId = ModuleBase.DefaultDisplayTabIdForPortal(item.PortalId);
            string pageName = GetFriendlyPageName(item.Name);

            // if the setting to "force display on this page" is set, be sure to send them there.
            if (!item.ForceDisplayOnPage() && tabId > 0 && item.DisplayOnCurrentPage())
            {
                if (!IsPageOverrideable(item.PortalId, tabId))
                {
                    // not overrideable, send them to default tab id
                    tabInfo = tabController.GetTab(defaultTabId, item.PortalId, false);
                }
                else
                {
                    tabInfo = tabController.GetTab(tabId, item.PortalId, false);

                    // check if there is a ModuleID passed in the querystring, if so then send it in the querystring as well
                    if (moduleId > 0)
                    {
                        queryStringModuleId = moduleId;
                    }
                }
            }
            else
            {
                tabInfo = tabController.GetTab(item.DisplayTabId, item.PortalId, false);
            }

            if (tabInfo == null || tabInfo.IsDeleted)
            {
                tabInfo = tabController.GetTab(defaultTabId, item.PortalId, false);
            }

            // if the tab doesn't have an overrideable module on it redirect them to the page without Publish querystring parameters, assuming it is force display on page
            if (item.ForceDisplayOnPage() && !IsPageOverrideable(item.PortalId, tabInfo.TabID))
            {
                return Globals.NavigateURL(tabInfo.TabID);
            }

            return Globals.FriendlyUrl(
                tabInfo, 
                ConvertParametersToFriendly(
                    CreateParametersForQueryString(item.ItemId, tabInfo.TabID, queryStringModuleId, pageId, portalId, cultureName)), 
                pageName, 
                GetPortalSettings(item.PortalId));
        }

        /// <summary>
        /// Gets the item's name as a page name for a friendly URL.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <returns>The item's name as a page name for a friendly URL</returns>
        private static string GetFriendlyPageName(string itemName)
        {
            string pageName = itemName.Trim();
            if (pageName.Length > 50)
            {
                pageName = pageName.Substring(0, 50);
            }

            pageName = OnlyAlphanumericCharacters(pageName);

            // Global.asax Application_BeginRequest checks for these values and will try to redirect to the non-existent page
            if (pageName.EndsWith("install", StringComparison.CurrentCultureIgnoreCase) ||
                pageName.EndsWith("installwizard", StringComparison.CurrentCultureIgnoreCase))
            {
                pageName = pageName.Substring(0, pageName.Length - 1);
            }

            return pageName + ".aspx";
        }

        /// <summary>
        /// Gets a URL linking to the given item when friendly URLs are not turned on.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tabId">The tab id.</param>
        /// <param name="moduleId">The module id.</param>
        /// <param name="pageId">The page id.</param>
        /// <param name="portalId">The portal id.</param>
        /// <returns>A URL linking to the given item</returns>
        private static string GetNonFriendlyLinkUrl(Item item, int tabId, int moduleId, int pageId, int portalId)
        {
            PortalSettings portalSettings = GetPortalSettings(item.PortalId);

            // check to see if the Page has an overrideable module on it, if not do a normal DNN link to the page.
            TabInfo tabInfo;
            var tabController = new TabController();
            int? queryStringModuleId = null;
            int defaultTabId = ModuleBase.DefaultDisplayTabIdForPortal(item.PortalId);

            // string pageName = GetFriendlyPageName(item.Name);

            // if the setting to "force display on this page" is set, be sure to send them there.
            if (!item.ForceDisplayOnPage() && tabId > 0 && item.DisplayOnCurrentPage())
            {
                if (!IsPageOverrideable(item.PortalId, tabId))
                {
                    // not overrideable, send them to default tab id
                    tabInfo = tabController.GetTab(defaultTabId, item.PortalId, false);
                }
                else
                {
                    tabInfo = tabController.GetTab(tabId, item.PortalId, false);

                    // check if there is a ModuleID passed in the querystring, if so then send it in the querystring as well
                    if (moduleId > 0)
                    {
                        queryStringModuleId = moduleId;
                    }
                }
            }
            else
            {
                tabInfo = tabController.GetTab(item.DisplayTabId, item.PortalId, false);
            }

            if (tabInfo == null || tabInfo.IsDeleted)
            {
                tabInfo = tabController.GetTab(defaultTabId, item.PortalId, false);
            }

            // if the tab doesn't have an overrideable module on it redirect them to the page without Publish querystring parameters, assuming it is force display on page
            if (item.ForceDisplayOnPage() && !IsPageOverrideable(item.PortalId, tabInfo.TabID))
            {
                return Globals.NavigateURL(tabInfo.TabID);
            }

            string[] queryStringParameters =
                ConvertParametersToNonFriendly(CreateParametersForQueryString(item.ItemId, null, queryStringModuleId, pageId, portalId, String.Empty));
            return Globals.NavigateURL(tabInfo.TabID, portalSettings, String.Empty, queryStringParameters);

            ////below is the original version of this
            ////int displayTabId = item.DisplayTabId;
            ////int? queryStringModuleId = null;
            ////if (!item.ForceDisplayOnPage() && tabId > 0 && item.DisplayOnCurrentPage())
            ////{
            ////    displayTabId = tabId;
            ////    if (moduleId > 0)
            ////    {
            ////        queryStringModuleId = moduleId;
            ////    }
            ////}

            ////string[] queryStringParameters = ConvertParametersToNonFriendly(CreateParametersForQueryString(item.ItemId, null, queryStringModuleId, pageId, portalId, String.Empty));
            ////return Globals.NavigateURL(displayTabId, portalSettings, String.Empty, queryStringParameters);
        }

        private static PortalAliasInfo GetPortalAliasInfo(int portalId)
        {
            ArrayList portalAliases = (new PortalAliasController()).GetPortalAliasArrayByPortalID(portalId);
            return portalAliases != null && portalAliases.Count > 0 ? (PortalAliasInfo)portalAliases[0] : null;
        }

        private static bool IsAlphaNumeric(string testString)
        {
            var objAlphaPattern = new Regex("[^0-9a-zA-Z ]");

            return !objAlphaPattern.IsMatch(testString);
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
    }

    /// <summary>
    /// Encapsulates a method that has no parameters and returns a value of the type specified by the <typeparamref name="TResult"/> parameter.  
    /// </summary>
    /// <typeparam name="TResult">
    /// The type of the return value of the method that this delegate encapsulates. 
    /// </typeparam>
    /// <remarks>
    /// This is a copy of the type introduced in .NET 3.5 (http://msdn.microsoft.com/en-us/library/bb534960.aspx), and should be replaced by that type once possible.
    /// </remarks>
    /// <returns>
    /// The return value of the method that this delegate encapsulates. 
    /// </returns>
    public delegate TResult Func<TResult>();
}