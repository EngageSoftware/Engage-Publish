//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using DotNetNuke.Common;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Framework;
    using DotNetNuke.Security;
    using Util;

    /// <summary>
    /// 
    /// </summary>
    public class ModuleBase : PortalModuleBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool allowTitleUpdate = true;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int externallySetItemId = -1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool logBreadcrumb = true;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool overrideable = true;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int pageId;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool useCache = true;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool useUrls;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Item versionInfoObject;

        public const string GlobalResourceFile = "~/DesktopModules/EngagePublish/App_GlobalResources/globalresources.resx";

        public bool UseUrls
        {
            [DebuggerStepThrough]
            get { return this.useUrls; }
            [DebuggerStepThrough]
            set { this.useUrls = value; }
        }

        public Item VersionInfoObject
        {
            [DebuggerStepThrough]
            get { return this.versionInfoObject; }
            [DebuggerStepThrough]
            set { this.versionInfoObject = value; }
        }

        public bool LogBreadcrumb
        {
            [DebuggerStepThrough]
            get { return this.logBreadcrumb; }
            [DebuggerStepThrough]
            set { this.logBreadcrumb = value; }
        }

        public bool Overrideable
        {
            [DebuggerStepThrough]
            get { return this.overrideable; }
            [DebuggerStepThrough]
            set { this.overrideable = value; }
        }

        public bool UseCache
        {
            get { return this.useCache && this.CacheTime > 0; }
            [DebuggerStepThrough]
            set { this.useCache = value; }
        }

        public bool AllowTitleUpdate
        {
            get
            {
                object o = this.Settings["AllowTitleUpdate"];
                if (o == null || !bool.TryParse(o.ToString(), out this.allowTitleUpdate))
                {
                    this.allowTitleUpdate = true;
                }
                return this.allowTitleUpdate;
            }
        }

        public int PageId
        {
            get
            {
                this.pageId = 1;
                if (this.versionInfoObject != null)
                {
                    object o = this.Request.QueryString["pageid"];
                    object c = this.Request.QueryString["catpageid"];

                    if (o != null && this.versionInfoObject.ItemTypeId == ItemType.Article.GetId())
                    {
                        this.pageId = Convert.ToInt32(o, CultureInfo.InvariantCulture);
                    }
                    else if (c != null
                             &&
                             (this.versionInfoObject.ItemTypeId == ItemType.Category.GetId()
                              || this.versionInfoObject.ItemTypeId == ItemType.TopLevelCategory.GetId()))
                    {
                        this.pageId = Convert.ToInt32(c, CultureInfo.InvariantCulture);
                    }
                }
                return this.pageId;
            }
        }

        //TODO: cache all the HostSetting values
        public bool IsSetup
        {
            get
            {
                string s = HostSettings.GetHostSetting(Utility.PublishSetup + this.PortalId);
                string d = HostSettings.GetHostSetting(Utility.PublishDefaultDisplayPage + this.PortalId);
                return !String.IsNullOrEmpty(s) && !String.IsNullOrEmpty(d);
            }
        }

        public static bool IsHostMailConfigured
        {
            get
            {
                string s = HostSettings.GetHostSetting("SMTPServer");
                return Utility.HasValue(s);
            }
        }

        public int ArticleEditWidth
        {
            get
            {
                string s = HostSettings.GetHostSetting(Utility.PublishArticleEditWidth + this.PortalId);
                if (Utility.HasValue(s))
                {
                    return Convert.ToInt32(s, CultureInfo.InvariantCulture);
                }
                return 500;
            }
        }

        public int ArticleEditHeight
        {
            get
            {
                string s = HostSettings.GetHostSetting(Utility.PublishArticleEditHeight + this.PortalId);
                if (Utility.HasValue(s))
                {
                    return Convert.ToInt32(s, CultureInfo.InvariantCulture);
                }
                return 400;
            }
        }

        public int ItemEditDescriptionHeight
        {
            get
            {
                string s = HostSettings.GetHostSetting(Utility.PublishDescriptionEditHeight + this.PortalId);
                if (Utility.HasValue(s))
                {
                    return Convert.ToInt32(s, CultureInfo.InvariantCulture);
                }
                return 300;
            }
        }

        public int ItemEditDescriptionWidth
        {
            get
            {
                string s = HostSettings.GetHostSetting(Utility.PublishDescriptionEditWidth + this.PortalId);
                if (Utility.HasValue(s))
                {
                    return Convert.ToInt32(s, CultureInfo.InvariantCulture);
                }
                return 500;
            }
        }

        public bool IsCommentsEnabled
        {
            get { return IsCommentsEnabledForPortal(this.PortalId); }
        }

        public bool IsCommentAuthorNotificationEnabled
        {
            get { return IsCommentAuthorNotificationEnabledForPortal(this.PortalId); }
        }

        public bool IsShortLinkEnabled
        {
            get { return IsShortLinkEnabledForPortal(this.PortalId); }
        }

        public bool AllowAnonymousComments
        {
            get { return AllowAnonymousCommentsForPortal(this.PortalId); }
        }

        public bool AreCommentsModerated
        {
            get { return AreCommentsModeratedForPortal(this.PortalId); }
        }

        public bool AutoApproveComments
        {
            get { return AutoApproveCommentsForPortal(this.PortalId); }
        }

        public bool AreRatingsEnabled
        {
            get { return AreRatingsEnabledForPortal(this.PortalId); }
        }

        public bool AllowAnonymousRatings
        {
            get { return AllowAnonymousRatingsForPortal(this.PortalId); }
        }

        public bool IsViewTrackingEnabled
        {
            get { return IsViewTrackingEnabledForPortal(this.PortalId); }
        }

        public bool EnablePublishFriendlyUrls
        {
            get { return EnablePublishFriendlyUrlsForPortal(this.PortalId); }
        }

        public bool AllowArticlePaging
        {
            get { return AllowArticlePagingForPortal(this.PortalId); }
        }

        public bool AllowVenexusSearch
        {
            get { return AllowVenexusSearchForPortal(this.PortalId); }
        }

        public bool AllowSimpleGalleryIntegration
        {
            get { return AllowSimpleGalleryIntegrationForPortal(this.PortalId); }
        }

        public bool AllowUltraMediaGalleryIntegration
        {
            get { return AllowUltraMediaGalleryIntegrationForPortal(this.PortalId); }
        }

        public bool EnableDisplayNameAsHyperlink
        {
            get { return EnableDisplayNameAsHyperlinkForPortal(this.PortalId); }
        }

        //TODO: implement settings on the Publish Admin Settings Control
        public bool AllowTags
        {
            get { return AllowTagsForPortal(this.PortalId); }
        }

        public int PopularTagCount
        {
            get { return PopularTagCountForPortal(this.PortalId); }
        }

        public int DefaultDisplayTabId
        {
            get { return DefaultDisplayTabIdForPortal(this.PortalId); }
        }

        public int DefaultTagDisplayTabId
        {
            get { return DefaultTagDisplayTabIdForPortal(this.PortalId); }
        }

        public int DefaultTextHtmlCategory
        {
            get { return DefaultTextHtmlCategoryForPortal(this.PortalId); }
        }



        public bool AllowRichTextDescriptions
        {
            get { return AllowRichTextDescriptionsForPortal(this.PortalId); }
        }

        public bool UseApprovals
        {
            get { return UseApprovalsForPortal(this.PortalId); }
        }

        public bool UseEmbeddedArticles
        {
            get { return UseEmbeddedArticlesForPortal(this.PortalId); }
        }

        public bool ShowItemIds
        {
            get { return ShowItemIdsForPortal(this.PortalId); }
        }

        public string ThumbnailSubdirectory
        {
            get { return ThumbnailSubdirectoryForPortal(this.PortalId); }
        }

        public string ThumbnailSelectionOption
        {
            get { return ThumbnailSelectionOptionForPortal(this.PortalId); }
        }

        public int MaximumRating
        {
            get { return MaximumRatingForPortal(this.PortalId); }
        }

        public bool IsAdmin
        {
            get 
            {
                return this.Request.IsAuthenticated
                       && (PortalSecurity.IsInRole(HostSettings.GetHostSetting(Utility.PublishAdminRole + this.PortalId)) || this.UserInfo.IsSuperUser);
            }
        }

        public bool IsConfigured
        {
            get { return this.Settings.Contains("DisplayType"); }
        }

        public bool IsAuthor
        {
            get { return this.Request.IsAuthenticated && PortalSecurity.IsInRole(HostSettings.GetHostSetting(Utility.PublishAuthorRole + this.PortalId)); }
        }

        public bool IsPingEnabled
        {
            get { return Utility.IsPingEnabledForPortal(this.PortalId); }
        }

        public bool IsPublishCommentType
        {
            get { return IsPublishCommentTypeForPortal(this.PortalId); }
        }

        public string ForumProviderType
        {
            get { return ForumProviderTypeForPortal(this.PortalId); }
        }

        public ItemType TypeOfItem
        {
            get
            {
                int typeId = Item.GetItemTypeId(this.ItemId, this.PortalId);

                return ItemType.GetFromId(typeId, typeof(ItemType));
            }
        }

        public int ItemId
        {
            get
            {
                //someone called the public method and set the ItemID (externally).
                if (this.externallySetItemId > 0)
                {
                    return this.externallySetItemId;
                }
                //ItemId has not been set externally now we need to look at settings.

                //if the querystring has the ItemId on it and the settings are to override				
                string i = this.Request.QueryString["itemId"];
                //we need to look if we're in admin mode, if so forget the reference about IsOverridable, it's always overridable.
                string ctl = this.Request.QueryString["ctl"];

                if (!String.IsNullOrEmpty(ctl) && ctl.Equals(Utility.AdminContainer))
                {
                    if (!String.IsNullOrEmpty(i))
                    {
                        return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                    }
                    if (this.ItemVersionId > 0)
                    {
                        return Item.GetItemIdFromVersion(this.ItemVersionId, this.PortalId);
                    }
                }

                ItemManager manager = new ItemManager(this);

                //Check if there's a moduleid

                object o = this.Request.Params["modid"];
                if (o != null)
                {
                    if (Convert.ToInt32(o, CultureInfo.InvariantCulture) == this.ModuleId)
                    {
                        //if we found the moduleid in the querystring we are trying to force the article here.                      
                        if (!String.IsNullOrEmpty(i))
                        {
                            return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                        }
                        //The local variable ItemVersionId is set so resolve the ItemVersionid to an Itemid
                        if (this.ItemVersionId > 0)
                        {
                            return Item.GetItemIdFromVersion(this.ItemVersionId, this.PortalId);
                        }
                        return manager.ResolveId();
                    }
                    //if we don't match the moduleid then we should get the module settings from DNN to use
                    return manager.ResolveId();
                }

                if (!String.IsNullOrEmpty(i) && manager.IsOverrideable)
                {
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }

                //The local variable ItemVersionId is set so resolve the ItemVersionid to an Itemid
                if (manager.IsOverrideable && this.ItemVersionId > 0)
                {
                    return Item.GetItemIdFromVersion(this.ItemVersionId, this.PortalId);
                }

                //none of the above have scenarios have been met, need to ask the Manager class to 
                //determine the itemid. The manager contains logic to check module settings and 
                //backward capatibility settings. hk
                return manager.ResolveId();
            }
        }

        //This is the cachetime used by Publish modules
        public int CacheTime
        {
            get
            {
                object o = this.Settings["CacheTime"];
                if (o != null)
                {
                    return Convert.ToInt32(o.ToString(), CultureInfo.InvariantCulture);
                }
                if (GetDefaultCacheSetting(this.PortalId) > 0)
                {
                    return GetDefaultCacheSetting(this.PortalId);
                }
                return 0;
            }
        }

        public int DefaultAdminPagingSize
        {
            get { return GetAdminDefaultPagingSize(this.PortalId); }
        }

        public static string ApplicationUrl
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Request.ApplicationPath == "/" ? string.Empty : HttpContext.Current.Request.ApplicationPath;
                }
                return string.Empty;
            }
        }

        public int ItemVersionId
        {
            get
            {
                string s = this.Request.QueryString["VersionId"];
                return (s == null ? -1 : Convert.ToInt32(s, CultureInfo.InvariantCulture));
            }
        }

        public int CommentId
        {
            get
            {
                string s = this.Request.QueryString["CommentId"];
                return (s == null ? -1 : Convert.ToInt32(s, CultureInfo.InvariantCulture));
            }
        }
        public static string DesktopModuleFolderName
        {
            get { return Utility.DesktopModuleFolderName; }
        }

        public static bool ApprovalEmailsEnabled(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishEmail + portalId);
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s);
            }
            return false;
        }

        public static bool IsCommentsEnabledForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishComment + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return false;
        }

        public static bool IsCommentAuthorNotificationEnabledForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishCommentEmailAuthor + portalId);
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s);
            }
            return false;
        }

        public static bool IsShortLinkEnabledForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishShortItemLink + portalId);
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s);
            }
            return false;
        }

        public static bool UseSessionForReturnToList(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishSessionReturnToList + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return false;
        }

        public static bool AllowAuthorEditCategory(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishAuthorCategoryEdit + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return true;
        }

        public static bool AllowAnonymousCommentsForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishCommentAnonymous + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return false;
        }

        public static bool AreCommentsModeratedForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishCommentApproval + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return true;
        }

        public static bool AutoApproveCommentsForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishCommentAutoApprove + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return false;
        }

        public static bool AreRatingsEnabledForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishRating + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return false;
        }

        public static bool AllowAnonymousRatingsForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishRatingAnonymous + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return false;
        }

        public static bool IsViewTrackingEnabledForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishEnableViewTracking + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return false;
        }

        public static bool EnablePublishFriendlyUrlsForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishEnablePublishFriendlyUrls + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return true;
        }

        public static bool AllowVenexusSearchForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishEnableVenexusSearch + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return false;
        }

        public static bool AllowSimpleGalleryIntegrationForPortal(int portalId)
        {
            if (Utility.IsSimpleGalleryInstalled)
            {
                string s = HostSettings.GetHostSetting(Utility.PublishEnableSimpleGalleryIntegration + portalId.ToString(CultureInfo.InvariantCulture));
                if (Utility.HasValue(s))
                {
                    return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
                }
            }
            return false;
        }

        public static bool AllowUltraMediaGalleryIntegrationForPortal(int portalId)
        {
            if (Utility.IsUltraMediaGalleryInstalled)
            {
                string s = HostSettings.GetHostSetting(Utility.PublishEnableUltraMediaGalleryIntegration + portalId.ToString(CultureInfo.InvariantCulture));
                if (Utility.HasValue(s))
                {
                    return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
                }
            }
            return false;
        }

        public static bool AllowArticlePagingForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishEnableArticlePaging + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return false;
        }

        public static bool EnableDisplayNameAsHyperlinkForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishEnableDisplayNameAsHyperlink + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return true;
        }

        public static bool AllowTagsForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishEnableTags + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return false;
        }

        public static int PopularTagCountForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishPopularTagCount + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToInt32(s, CultureInfo.InvariantCulture);
            }
            return -1;
        }

        public static int DefaultDisplayTabIdForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishDefaultDisplayPage + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToInt32(s, CultureInfo.InvariantCulture);
            }
            return -1;
        }

        public static int DefaultTagDisplayTabIdForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishDefaultTagPage + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToInt32(s, CultureInfo.InvariantCulture);
            }
            return -1;
        }


        public static int DefaultTextHtmlCategoryForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishDefaultTextHtmlCategory + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToInt32(s, CultureInfo.InvariantCulture);
            }
            return -1;
        }


        public static bool AllowRichTextDescriptionsForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishAllowRichTextDescriptions + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return true;
        }

        public static bool UseApprovalsForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishUseApprovals + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return true;
        }

        public static bool UseEmbeddedArticlesForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishUseEmbeddedArticles + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return false;
        }

        public static bool ShowItemIdsForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishShowItemId + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return Convert.ToBoolean(s, CultureInfo.InvariantCulture);
            }
            return true;
        }

        public static string ThumbnailSubdirectoryForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishThumbnailSubdirectory + portalId.ToString(CultureInfo.InvariantCulture));
            if (!Utility.HasValue(s))
            {
                s = "PublishThumbnails/";
            }
            else if (s.StartsWith("/", StringComparison.Ordinal))
            {
                s = s.Substring(1);
            }

            return s;
        }

        public static string ThumbnailSelectionOptionForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishThumbnailDisplayOption + portalId.ToString(CultureInfo.InvariantCulture));

            return s;
        }

        public static int MaximumRatingForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishRatingMaximum + portalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                return int.Parse(s, CultureInfo.InvariantCulture);
            }
            return UserFeedback.Rating.DefaultMaximumRating;
        }

        public static bool IsUserAdmin(int portalId)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.IsAuthenticated && PortalSecurity.IsInRole(HostSettings.GetHostSetting(Utility.PublishAdminRole + portalId));
            }
            return false;
        }

        public static bool IsPublishCommentTypeForPortal(int portalId)
        {
            return string.IsNullOrEmpty(HostSettings.GetHostSetting(Utility.PublishForumProviderType + portalId.ToString(CultureInfo.InvariantCulture)));
        }

        public static string ForumProviderTypeForPortal(int portalId)
        {
            return HostSettings.GetHostSetting(Utility.PublishForumProviderType + portalId.ToString(CultureInfo.InvariantCulture));
        }

        public static bool UseCachePortal(int portalId)
        {
            if (CacheTimePortal(portalId) > 0)
            {
                return true;
            }
            return false;
        }

        public static int CacheTimePortal(int portalId)
        {
            if (GetDefaultCacheSetting(portalId) > 0)
            {
                return GetDefaultCacheSetting(portalId);
            }
            return 0;
        }

        public static int GetDefaultCacheSetting(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishCacheTime + portalId);
            if (Utility.HasValue(s))
            {
                return Convert.ToInt32(s, CultureInfo.InvariantCulture);
            }
            return 0;
        }

        public static int GetAdminDefaultPagingSize(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishDefaultAdminPagingSize + portalId);
            if (Utility.HasValue(s))
            {
                return Convert.ToInt32(s, CultureInfo.InvariantCulture);
            }
            return 25;
        }

        public string BuildLinkUrl(string qsParameters)
        {
            return Globals.NavigateURL(this.TabId, string.Empty, qsParameters);
        }

        public string GetItemLinkUrl(object itemId)
        {
            if (itemId != null)
            {
                return Utility.GetItemLinkUrl(Convert.ToInt32(itemId, CultureInfo.InvariantCulture), this.PortalId, this.TabId, this.ModuleId, this.PageId, this.GetCultureName());
            }

            return string.Empty;
        }

        public void SetItemId(int value)
        {
            this.externallySetItemId = value;
        }

        public string GetItemLinkTarget(object itemId)
        {
            int curItemId;
            Int32.TryParse(itemId.ToString(), out curItemId);
            if (curItemId > 0)
            {
                Item i = this.BindItemData(curItemId);
                if (i.NewWindow)
                {
                    return "_blank";
                }
            }
            return string.Empty;
        }

        public string GetItemLinkUrl(object itemId, int portalId)
        {
            return Utility.IsDisabled(Convert.ToInt32(itemId, CultureInfo.InvariantCulture), portalId) ? string.Empty : this.GetItemLinkUrl((itemId));
        }

        public string GetItemLinkUrlExternal(object itemId)
        {
            //if (this.IsShortLinkEnabled)
            //{
            //    return "http://" + this.PortalAlias.HTTPAlias + "/itemlink.aspx?itemId=" + itemId;
            //}
            //return "http://" + this.PortalAlias.HTTPAlias + DesktopModuleFolderName + "itemlink.aspx?itemId=" + itemId;
            return Utility.GetItemLinkUrl(itemId, PortalId);
        }

        public static string GetRssLinkUrl(object itemId, int maxDisplayItems, int itemTypeId, int portalId, string displayType)
        {
            StringBuilder url = new StringBuilder(128);

            url.Append(ApplicationUrl);
            url.Append(DesktopModuleFolderName);
            url.Append("eprss.aspx?itemId=");
            url.Append(itemId);
            url.Append("&numberOfItems=");
            url.Append(maxDisplayItems);
            url.Append("&itemtypeid=");
            url.Append(itemTypeId);
            url.Append("&portalid=");
            url.Append(portalId);
            url.Append("&DisplayType=");
            url.Append(displayType);

            return url.ToString();
        }

        public static string GetRssLinkUrl(int portalId, string displayType, string tags)
        {
            StringBuilder url = new StringBuilder(128);

            url.Append(ApplicationUrl);
            url.Append(DesktopModuleFolderName);
            url.Append("eprss.aspx?");
            url.Append("portalid=");
            url.Append(portalId);
            url.Append("&DisplayType=");
            url.Append(displayType);
            url.Append("&Tags=");
            url.Append(HttpUtility.UrlEncode(tags));

            return url.ToString();
        }

        public void SetPageTitle()
        {
            //TODO: should we also allow for setting the module title here?
            if (this.AllowTitleUpdate)
            {
                CDefault tp = (CDefault)this.Page;
                tp.Title = Utility.HasValue(this.VersionInfoObject.MetaTitle) ? this.versionInfoObject.MetaTitle : this.versionInfoObject.Name;

                if (this.LogBreadcrumb)
                {
                    this.AddBreadcrumb(this.versionInfoObject.Name);
                }

                //do meta tag settings as well
                if (!String.IsNullOrEmpty(this.VersionInfoObject.MetaDescription))
                {
                    tp.Description = this.VersionInfoObject.MetaDescription;
                }

                if (!String.IsNullOrEmpty(this.VersionInfoObject.MetaKeywords))
                {
                    tp.KeyWords = this.VersionInfoObject.MetaKeywords;
                }

                //tp.SmartNavigation = true;
                this.Page.SetFocus(tp.ClientID);
            }
        }

        public void SetRssUrl(string rssUrl, string rssTitle)
        {
            if (rssUrl != null && rssTitle != null)
            {
                LiteralControl lc = new LiteralControl();
                StringBuilder sb = new StringBuilder(400);
                sb.Append("<link rel=\"alternate\" type=\"application/rss+xml\" href=\"");
                sb.Append(rssUrl);
                sb.Append("\" title=\"");
                sb.Append(rssTitle);
                sb.Append("\" />");
                lc.Text = sb.ToString();

                CDefault tp = (CDefault)this.Page;
                if (tp != null)
                {
                    tp.Header.Controls.Add(lc);
                }
            }
        }

        public void SetExternalRssUrl(string rssUrl, string rssTitle)
        {
            if (rssUrl != null && rssTitle != null)
            {
                LiteralControl lc = new LiteralControl();
                StringBuilder sb = new StringBuilder(400);
                sb.Append("<link rel=\"alternate\" type=\"application/rss+xml\" href=\"");
                sb.Append(rssUrl);
                sb.Append("\" title=\"");
                sb.Append(rssTitle);
                sb.Append("\" />");
                lc.Text = sb.ToString();

                CDefault tp = (CDefault)this.Page;
                if (tp != null)
                {
                    tp.Header.Controls.Add(lc);
                }
            }
        }


        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Compiler doesn't see validation")]
        public string GetThumbnailUrl(object objFileName)
        {
            string fileName = objFileName as string ?? string.Empty;
            if (!Utility.HasValue(fileName))
            {
                return ApplicationUrl + "/images/spacer.gif";
            }
            //check if we're storing another URL, if it doesn't start with a / then we have trouble brewing
            if (fileName.StartsWith(this.ThumbnailSubdirectory, StringComparison.OrdinalIgnoreCase)
                || (!fileName.StartsWith("/", StringComparison.Ordinal) && !fileName.StartsWith("http", StringComparison.OrdinalIgnoreCase)))
            {
                return this.GenerateLocalThumbnailUrl(fileName);
            }
            return fileName;
        }

        protected void AddBreadcrumb(string pageName)
        {
            Breadcrumb.Add(pageName, this.GetItemLinkUrl(this.ItemId));
        }

        protected Item BindItemData(int itemId)
        {
            Item i = null;
            string currentItemType = Item.GetItemType(itemId, this.PortalId);
            //bool getitem = true;
            if (itemId > 0)
            {
                if (currentItemType.Equals("ARTICLE", StringComparison.OrdinalIgnoreCase))
                {
                    i = Article.GetArticle(itemId, this.PortalId);
                    if (this.ItemVersionId > 0)
                    {
                        i = Article.GetArticleVersion(this.ItemVersionId, this.PortalId);
                    }
                    if (i != null)
                    {
                        if (this.AllowTags && i.Tags.Count < 1)
                        {
                            foreach (ItemTag it in ItemTag.GetItemTags(i.ItemVersionId, this.PortalId))
                            {
                                i.Tags.Add(it);
                            }
                        }
                    }

                    //If an Article can't be created based on the ItemID or the ItemVersionId then we'll create a new one.
                    if (i == null)
                    {
                        i = Article.Create(this.PortalId);
                    }
                }

                else if (currentItemType.Equals("CATEGORY", StringComparison.OrdinalIgnoreCase))
                {
                    i = Category.GetCategory(itemId, this.PortalId);
                    if (this.ItemVersionId > 0)
                    {
                        i = Category.GetCategoryVersion(this.ItemVersionId, this.PortalId);
                    }
                    //If a Category can't be created based on the ItemID or the ItemVersionId then we'll create a new one.
                    if (i == null)
                    {
                        i = Category.Create(this.PortalId);
                    }
                }

                else if (currentItemType.Equals("TOPLEVELCATEGORY", StringComparison.OrdinalIgnoreCase))
                {
                    i = Category.GetCategory(itemId, this.PortalId);
                    if (this.ItemVersionId > 0)
                    {
                        i = Category.GetCategoryVersion(this.ItemVersionId, this.PortalId);
                    }

                    //If a Category can't be created based on the ItemID or the ItemVersionId then we'll create a new one.
                    if (i == null)
                    {
                        i = Category.Create(this.PortalId);
                    }
                }

                else
                {
                    i = Article.GetArticle(itemId, this.PortalId);
                    if (this.ItemVersionId > 0)
                    {
                        i = Article.GetArticleVersion(this.ItemVersionId, this.PortalId);
                    }

                    //If an Article can't be created based on the ItemID or the ItemVersionId then we'll create a new one.
                    if (i == null)
                    {
                        i = Article.Create(this.PortalId);
                    }
                }
            }
            return i;
        }

        protected void BindItemData(bool createNew)
        {
            if (createNew || this.ItemId < 1)
            {
                this.BindNewItem();
            }
            else
            {
                this.BindCurrentItem();
            }
        }

        protected void BindItemData()
        {
            this.BindItemData(false);
        }

        /// <summary>
        /// Gets the name of the current culture under which the page and user is operating.
        /// </summary>
        /// <returns>The current culture name, or <see cref="string.Empty"/> if none is found</returns>
        protected string GetCultureName()
        {
            // add to this if necessary, currently we're only looking for language
            string languageValue = this.Request.QueryString["language"];
            if (languageValue != null)
            {
                // if languages are turned on we should pass the language querystring parameter
                if (this.UserId > -1 && this.UserInfo.Profile.PreferredLocale != CultureInfo.CurrentCulture.Name)
                {
                    return this.UserInfo.Profile.PreferredLocale;
                }

                return languageValue;
            }

            return string.Empty;
        }

        protected string GetEditUrl(string itemId)
        {
            return this.EditUrl("itemId", itemId, "Edit");
        }

        protected string BuildCategoryListUrl(ItemType type)
        {
            int parentCategoryId = -1;

            if (!this.VersionInfoObject.IsNew)
            {
                if (this.VersionInfoObject.ItemTypeId == ItemType.Category.GetId())
                {
                    parentCategoryId = this.VersionInfoObject.ItemId;
                }
                else
                {
                    //find the parent category ID from an item
                    parentCategoryId = Category.GetParentCategory(this.VersionInfoObject.ItemId, this.PortalId);
                }
            }

            return Globals.NavigateURL(
                this.TabId,
                string.Empty,
                "ctl=" + Utility.AdminContainer,
                "mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture),
                "adminType=" + type.Name + "list",
                "categoryId=" + parentCategoryId.ToString(CultureInfo.InvariantCulture));
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (AJAX.IsInstalled())
            {
                AJAX.RegisterScriptManager();
            }
        }

        private void BindNewItem()
        {
            string editControl = this.Request.QueryString["adminType"];
            if (editControl != null)
            {
                //If the querystring contains a admintype on it then we need to check to see if it's article edit or
                //categoryedit. The reason this is important, we can't use the ItemId property to determine the type of
                //new object to create (article/category) because it looks as settings for the module if needed. So, if you
                //click Add new Article on a Category Display you will get the wrong type of object created. hk
                this.BindNewItemForEdit();
            }
            else
            {
                this.BindNewItemByItemType();
            }
        }

        private void BindNewItemForEdit()
        {
            Item i; // = null;
            string editControl = this.Request.QueryString["adminType"];
            if (editControl.Equals("CATEGORYEDIT", StringComparison.OrdinalIgnoreCase))
            {
                i = Category.Create(this.PortalId);
            }
            else
            {
                i = Article.Create(this.PortalId);
            }

            this.versionInfoObject = i;
        }

        private void BindNewItemByItemType()
        {
            Item i; // = null;
            string currentItemType = Item.GetItemType(this.ItemId, this.PortalId);
            if (currentItemType.Equals("CATEGORY", StringComparison.OrdinalIgnoreCase))
            {
                i = Category.Create(this.PortalId);
            }
            else
            {
                i = Article.Create(this.PortalId);
            }
            this.versionInfoObject = i;
        }

        private void BindCurrentItem()
        {
            //check for version id
            int itemId = this.ItemId;
            this.versionInfoObject = this.BindItemData(itemId);
        }

        private string GenerateLocalThumbnailUrl(string fileName)
        {
            //DotNetNuke.Entities.Portals.PortalSettings ps = Utility.GetPortalSettings(portalId);
            return this.Request.Url.Scheme + "://" + this.Request.Url.Host + this.PortalSettings.HomeDirectory + fileName;
        }
    }
}