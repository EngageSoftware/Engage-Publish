//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Security;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish
{
    /// <summary>
    /// Summary description for ModuleBase.
    /// </summary>
    public class ModuleBase : PortalModuleBase
    {

        public string GlobalResourceFile = "~/DesktopModules/EngagePublish/App_GlobalResources/globalresources.resx";

        private int externallySetItemId = -1;
        //private Breadcrumb breadCrumb;
        private bool allowTitleUpdate = true;
        private Item versionInfoObject;
        private bool _logBreadcrumb = true;
        private bool overrideable = true;
        private int pageId;
        private bool useCache = true;

        private bool useUrls = false;
        public bool UseUrls
        {
            get
            {
                return useUrls;
            }
            set
            {
                useUrls = value;
            }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (DotNetNuke.Framework.AJAX.IsInstalled())
            {
                DotNetNuke.Framework.AJAX.RegisterScriptManager();
            }            
        }

        //TODO: cache all the HostSetting values
        public bool IsSetup
        {
            get
            {
                string s = HostSettings.GetHostSetting(Utility.PublishSetup + PortalId);
                string d = HostSettings.GetHostSetting(Utility.PublishDefaultDisplayPage + PortalId);
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
                string s = HostSettings.GetHostSetting(Utility.PublishArticleEditWidth + PortalId);
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
                string s = HostSettings.GetHostSetting(Utility.PublishArticleEditHeight + PortalId);
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
                string s = HostSettings.GetHostSetting(Utility.PublishDescriptionEditHeight + PortalId);
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
                string s = HostSettings.GetHostSetting(Utility.PublishDescriptionEditWidth + PortalId);
                if (Utility.HasValue(s))
                {
                    return Convert.ToInt32(s, CultureInfo.InvariantCulture);
                }
                return 500;
            }
        }

        public static bool ApprovalEmailsEnabled(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishEmail + portalId);
            if(Utility.HasValue(s))
                return Convert.ToBoolean(s);
            return false;
        }


        public bool IsCommentsEnabled
        {
            get
            {
                return IsCommentsEnabledForPortal(PortalId);
            }
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


        public bool IsCommentAuthorNotificationEnabled
        {
            get
                {
                    return IsCommentAuthorNotificationEnabledForPortal(PortalId);
                }
        }


        public static bool IsCommentAuthorNotificationEnabledForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishCommentEmailAuthor + portalId);
            if (Utility.HasValue(s))
                return Convert.ToBoolean(s);
            return false;
        }


        public bool IsShortLinkEnabled
        {
            get
            {
                return IsShortLinkEnabledForPortal(PortalId);
            }
        }


        public static bool IsShortLinkEnabledForPortal(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishShortItemLink + portalId);
            if (Utility.HasValue(s))
                return Convert.ToBoolean(s);
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

        public bool AllowAnonymousComments
        {
            get
            {
                return AllowAnonymousCommentsForPortal(PortalId);
            }
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

        public bool AreCommentsModerated
        {
            get
            {
                return AreCommentsModeratedForPortal(PortalId);
            }
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

        public bool AutoApproveComments
        {
            get
            {
                return AutoApproveCommentsForPortal(PortalId);
            }
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

        public bool AreRatingsEnabled
        {
            get
            {
                return AreRatingsEnabledForPortal(PortalId);
            }
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

        public bool AllowAnonymousRatings
        {
            get
            {
                return AllowAnonymousRatingsForPortal(PortalId);
            }
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

        public bool IsViewTrackingEnabled
        {
            get
            {
                return IsViewTrackingEnabledForPortal(PortalId);
            }
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

        public bool EnablePublishFriendlyUrls
        {
            get
            {
                return EnablePublishFriendlyUrlsForPortal(PortalId);
            }
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

        public bool AllowArticlePaging
        {
            get
            {
                return AllowArticlePagingForPortal(PortalId);
            }
        }
        
        public bool AllowVenexusSearch
        {
            get
            {
                return AllowVenexusSearchForPortal(PortalId);
            }
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

        public bool AllowSimpleGalleryIntegration
        {
            get
            {
                return AllowSimpleGalleryIntegrationForPortal(PortalId);
            }
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

        public bool AllowUltraMediaGalleryIntegration
        {
            get
            {
                return AllowUltraMediaGalleryIntegrationForPortal(PortalId);
            }
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

        public bool EnableDisplayNameAsHyperlink
        {
            get
            {
                return EnableDisplayNameAsHyperlinkForPortal(PortalId);
            }
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

        //TODO: implement settings on the Publish Admin Settings Control
        public bool AllowTags
        {
            get
            {
                return AllowTagsForPortal(PortalId);
            }
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

        public int PopularTagCount
        {
            get
            {
                return PopularTagCountForPortal(PortalId);
            }
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

        public int DefaultDisplayTabId
        {
            get
            {
                return DefaultDisplayTabIdForPortal(PortalId);
            }
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

        public int DefaultTagDisplayTabId
        {
            get
            {
                return DefaultTagDisplayTabIdForPortal(PortalId);
            }
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

        public bool AllowRichTextDescriptions
        {
            get
            {
                return AllowRichTextDescriptionsForPortal(PortalId);
            }
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

        public bool UseApprovals
        {
            get
            {
                return UseApprovalsForPortal(PortalId);
            }
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

        public bool UseEmbeddedArticles
        {
            get
            {
                return UseEmbeddedArticlesForPortal(PortalId);
            }
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

        public bool ShowItemIds
        {
            get
            {
                return ShowItemIdsForPortal(PortalId);
            }
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

        public string ThumbnailSubdirectory
        {
            get
            {
                return ThumbnailSubdirectoryForPortal(PortalId);
            }
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

        public string ThumbnailSelectionOption
        {
            get
            {
                return ThumbnailSelectionOptionForPortal(PortalId);
            }
        }

        public static string ThumbnailSelectionOptionForPortal(int portalId)
        {

            string s = HostSettings.GetHostSetting(Utility.PublishThumbnailDisplayOption + portalId.ToString(CultureInfo.InvariantCulture));

            return s;
        }

        public int MaximumRating
        {
            get
            {
                return MaximumRatingForPortal(PortalId);
            }
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

        public bool IsAdmin
        {
            get
            {
                if (Request.IsAuthenticated == false)
                {
                    return false;
                }
                else
                {
                    return (PortalSecurity.IsInRole(HostSettings.GetHostSetting(Utility.PublishAdminRole + PortalId))|| UserInfo.IsSuperUser);
                }
            }
        }

        public bool IsConfigured
        {
            get
            {
                return this.Settings.Contains("DisplayType");
            }
        }

        public static bool IsUserAdmin(int portalId)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.IsAuthenticated && PortalSecurity.IsInRole(HostSettings.GetHostSetting(Utility.PublishAdminRole + portalId));
            }
            return false;
        }

        public bool IsAuthor
        {
            get
            {
                return Request.IsAuthenticated && PortalSecurity.IsInRole(HostSettings.GetHostSetting(Utility.PublishAuthorRole + PortalId));
            }
        }

        public bool IsPingEnabled
        {
            get
            {
                return Utility.IsPingEnabledForPortal(PortalId);
            }
        }

       

        public bool IsPublishCommentType
        {
            get
            {
                return IsPublishCommentTypeForPortal(PortalId);
            }
        }

        public static bool IsPublishCommentTypeForPortal(int portalId)
        {
            return string.IsNullOrEmpty(HostSettings.GetHostSetting(Utility.PublishForumProviderType + portalId.ToString(CultureInfo.InvariantCulture)));
        }

        public string ForumProviderType
        {
            get
            {
                return ForumProviderTypeForPortal(PortalId);
            }
        }

        public static string ForumProviderTypeForPortal(int portalId)
        {
            return HostSettings.GetHostSetting(Utility.PublishForumProviderType + portalId.ToString(CultureInfo.InvariantCulture));
        }

        protected void AddBreadcrumb(string pageName)
        {
            Breadcrumb.Add(pageName, GetItemLinkUrl(ItemId));
        }

        public ItemType TypeOfItem
        {
            get
            {
                int typeId = Item.GetItemTypeId(ItemId, PortalId);

                return ItemType.GetFromId(typeId, typeof(ItemType));
            }
        }

        public int ItemId
        {
            get
            {
                //someone called the public method and set the ItemID (externally).
                if (externallySetItemId > 0) return externallySetItemId;
                //ItemId has not been set externally now we need to look at settings.

                //if the querystring has the ItemId on it and the settings are to override				
                string i = Request.QueryString["itemId"];
                //we need to look if we're in admin mode, if so forget the reference about IsOverridable, it's always overridable.
                string ctl = Request.QueryString["ctl"];

                if (!String.IsNullOrEmpty(ctl) && ctl.Equals(Utility.AdminContainer))
                {
                    if (!String.IsNullOrEmpty(i))
                    {
                        return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                    }
                    else if (ItemVersionId > 0)
                    {
                        return Item.GetItemIdFromVersion(ItemVersionId, PortalId);
                    }
                }

                ItemManager manager = new ItemManager(this);


                //Check if there's a moduleid

                object o = Request.Params["modid"];
                if (o != null)
                {
                    if (Convert.ToInt32(o, CultureInfo.InvariantCulture) == ModuleId)
                    {
                        //if we found the moduleid in the querystring we are trying to force the article here.                      
                        if (!String.IsNullOrEmpty(i))
                        {
                            return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                        }
                        //The local variable ItemVersionId is set so resolve the ItemVersionid to an Itemid
                        if (ItemVersionId > 0)
                        {
                            return Item.GetItemIdFromVersion(ItemVersionId, PortalId);
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
                if (manager.IsOverrideable && ItemVersionId > 0)
                {
                    return Item.GetItemIdFromVersion(ItemVersionId, PortalId);
                }



                //none of the above have scenarios have been met, need to ask the Manager class to 
                //determine the itemid. The manager contains logic to check module settings and 
                //backward capatibility settings. hk
                return manager.ResolveId();
            }
        }

        public int PageId
        {
            get
            {
                pageId = 1;
                if (versionInfoObject != null)
                {
                    object o = Request.QueryString["pageid"];
                    object c = Request.QueryString["catpageid"];

                    if (o != null && versionInfoObject.ItemTypeId == Util.ItemType.Article.GetId())
                    {
                        pageId = Convert.ToInt32(o, CultureInfo.InvariantCulture);
                    }
                    else if (c != null && (versionInfoObject.ItemTypeId == Util.ItemType.Category.GetId() || versionInfoObject.ItemTypeId == Util.ItemType.TopLevelCategory.GetId()))
                    {
                        pageId = Convert.ToInt32(c, CultureInfo.InvariantCulture);
                    }
                }
                return pageId;
            }
            set
            {
                pageId = value;
            }
        }

        public bool Overrideable
        {
            get { return (this.overrideable); }
            set { this.overrideable = value; }
        }

        public bool UseCache
        {
            get {
                return this.useCache && CacheTime>0;
            }
            set { this.useCache = value; }
        }

        public static bool UseCachePortal(int portalId)
        {
            if (CacheTimePortal(portalId) > 0)

                return true;
            else return false;
        }

        public static int CacheTimePortal(int portalId)
        {
            if (GetDefaultCacheSetting(portalId) > 0)
            {
                return GetDefaultCacheSetting(portalId);
            }
            return 0;
        }

        public bool AllowTitleUpdate
        {
            get
            {
                object o = Settings["AllowTitleUpdate"];
                if (o == null || !bool.TryParse(o.ToString(), out this.allowTitleUpdate))
                {
                    this.allowTitleUpdate = true;
                }
                return this.allowTitleUpdate;
            }
            set
            {
                this.allowTitleUpdate = value;
            }
        }

        //This is the cachetime used by Publish modules
        public int CacheTime
        {
            get
            {
                object o = Settings["CacheTime"];
                if (o != null)
                {
                    return Convert.ToInt32(o.ToString(), CultureInfo.InvariantCulture);
                }
                else if (GetDefaultCacheSetting(PortalId) > 0)
                {
                    return GetDefaultCacheSetting(PortalId);
                }
                return 0;
            }
        }


        public static int GetDefaultCacheSetting(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishCacheTime + portalId);
            if (Utility.HasValue(s))
            {
                return Convert.ToInt32(s, CultureInfo.InvariantCulture);
            }
            else
            {
                return 0;
            }
        }

        public int DefaultAdminPagingSize
        {
            get
            {
                return GetAdminDefaultPagingSize(PortalId);
            }
        }


        public static int GetAdminDefaultPagingSize(int portalId)
        {
            string s = HostSettings.GetHostSetting(Utility.PublishDefaultAdminPagingSize + portalId);
            if (Utility.HasValue(s))
            {
                return Convert.ToInt32(s, CultureInfo.InvariantCulture);
            }
            else
            {
                return 25;
            }
        }


        public int SetItemId
        {
            get { return (this.externallySetItemId); }
            set { this.externallySetItemId = value; }
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
                string s = Request.QueryString["VersionId"];
                return (s == null ? -1 : Convert.ToInt32(s, CultureInfo.InvariantCulture));
            }
        }

        public int CommentId
        {
            get
            {
                string s = Request.QueryString["CommentId"];
                return (s == null ? -1 : Convert.ToInt32(s, CultureInfo.InvariantCulture));
            }
        }

        public Item VersionInfoObject
        {
            get { return this.versionInfoObject; }
            set { this.versionInfoObject = value; }
        }

        private void BindNewItem()
        {
            string editControl = Request.QueryString["adminType"];
            if (editControl != null)
            {
                //If the querystring contains a admintype on it then we need to check to see if it's article edit or
                //categoryedit. The reason this is important, we can't use the ItemId property to determine the type of
                //new object to create (article/category) because it looks as settings for the module if needed. So, if you
                //click Add new Article on a Category Display you will get the wrong type of object created. hk
                BindNewItemForEdit();
            }
            else
            {
                BindNewItemByItemType();
            }
        }

        private void BindNewItemForEdit()
        {
            Item i;// = null;
            string editControl = Request.QueryString["adminType"];
            if (editControl.Equals("CATEGORYEDIT", StringComparison.OrdinalIgnoreCase))
            {
                i = Category.Create(PortalId);
            }
            else
            {
                i = Article.Create(PortalId);
            }

            versionInfoObject = i;

        }

        private void BindNewItemByItemType()
        {
            Item i;// = null;
            string currentItemType = Item.GetItemType(ItemId,PortalId);
            if (currentItemType.Equals("CATEGORY", StringComparison.OrdinalIgnoreCase))
            {
                i = Category.Create(PortalId);
            }
            else
            {
                i = Article.Create(PortalId);
            }
            versionInfoObject = i;
        }

        protected Item BindItemData(int itemId)
        {
            Item i = null;
            string currentItemType = Item.GetItemType(itemId,PortalId);
            //bool getitem = true;
            if (itemId > 0)
            {
                if (currentItemType.Equals("ARTICLE", StringComparison.OrdinalIgnoreCase))
                {
                    //TODO: remove this caching

                        i = Article.GetArticle(itemId, PortalId);
                        if (ItemVersionId > 0)
                        {
                            i = Article.GetArticleVersion(ItemVersionId, PortalId);
                        }
                        if (i != null)
                        {
                            if (AllowTags)
                            {
                                foreach (ItemTag it in ItemTag.GetItemTags(i.ItemVersionId, PortalId))
                                {
                                    i.Tags.Add(it);
                                }
                            }
                        }

                        //If an Article can't be created based on the ItemID or the ItemVersionId then we'll create a new one.
                        if (i == null)
                        {
                            i = Article.Create(PortalId);
                        }
                    
                }

                else if (currentItemType.Equals("CATEGORY", StringComparison.OrdinalIgnoreCase))
                {
                    //TODO: remove this caching
                        i = Category.GetCategory(itemId, PortalId);
                        if (ItemVersionId > 0)
                        {
                            i = Category.GetCategoryVersion(ItemVersionId, PortalId);
                        }
                        //If a Category can't be created based on the ItemID or the ItemVersionId then we'll create a new one.
                        if (i == null)
                        {
                            i = Category.Create(PortalId);
                        }

                    
                }

                else if (currentItemType.Equals("TOPLEVELCATEGORY", StringComparison.OrdinalIgnoreCase))
                {
                        i = Category.GetCategory(itemId, PortalId);
                        if (ItemVersionId > 0)
                        {
                            i = Category.GetCategoryVersion(ItemVersionId, PortalId);

                        }

                        //If a Category can't be created based on the ItemID or the ItemVersionId then we'll create a new one.
                        if (i == null)
                        {
                            i = Category.Create(PortalId);
                        }                   
                }

                else
                {
                        i = Article.GetArticle(itemId, PortalId);
                        if (ItemVersionId > 0)
                        {
                            i = Article.GetArticleVersion(ItemVersionId, PortalId);
                        }

                        //If an Article can't be created based on the ItemID or the ItemVersionId then we'll create a new one.
                        if (i == null)
                        {
                            i = Article.Create(PortalId);
                        }
                }
                
            }
            return i;
        }

        private void BindCurrentItem()
        {
            Item i = null;
            //check for version id
            int itemId = ItemId;
            versionInfoObject = BindItemData(itemId);

        }

        protected void BindItemData(bool createNew)
        {
            if (createNew || ItemId < 1)
            {
                BindNewItem();
            }
            else
            {
                BindCurrentItem();
            }
        }

        protected void BindItemData()
        {
            BindItemData(false);
        }

        public bool LogBreadcrumb
        {
            get { return this._logBreadcrumb; }
            set { this._logBreadcrumb = value; }
        }

        public string BuildLinkUrl(string qsParameters)
        {
            return DotNetNuke.Common.Globals.NavigateURL(TabId, "", qsParameters);
        }

        public static string DesktopModuleFolderName
        {
            get
            {
                return Utility.DesktopModuleFolderName;
            }
        }

        public string GetItemLinkUrl(object itemId)
        {
            return Utility.GetItemLinkUrl(itemId, PortalId, TabId, ModuleId, PageId, BuildOtherParameters());
            #region "old code"


            //if (itemId != null)
            //{
            //    int id = Convert.ToInt32(itemId, CultureInfo.InvariantCulture);
            //    int typeId = Item.GetItemTypeId(id);
            //    ItemType type = ItemType.GetFromId(typeId, typeof(ItemType));
            //    Item i = null;
            //    if (type.Name == ItemType.Article.Name)
            //    {
            //        i = Article.GetArticle(id);
            //    }
            //    else
            //    {
            //        i = Category.GetCategory(id);
            //    }
            //    if (i ==  null)
            //    {
            //        //there is no current version of this ITEM. Can't view it currently because ItemLink.aspx doesn't
            //        //support versions. hk
            //        return string.Empty;
            //    }
            //    else
            //    {
            //        //see DisplayOnCurrentPage is true for this item.
            //        if (i.DisplayOnCurrentPage())
            //        {
            //            //check if there is an overrideable module on this page, if not be sure to pass the moduleid
            //            if (Utility.IsPageOverrideable(PortalId, TabId))
            //            {
            //                return ApplicationUrl + DesktopModuleFolderName + "itemlink.aspx?itemId=" + id.ToString(CultureInfo.InvariantCulture) + "&tabid=" + TabId.ToString(CultureInfo.InvariantCulture);
            //            }
            //            else
            //            {
            //                return ApplicationUrl + DesktopModuleFolderName + "itemlink.aspx?itemId=" + id.ToString(CultureInfo.InvariantCulture) + "&tabid=" + TabId.ToString(CultureInfo.InvariantCulture) + "&modid=" + ModuleId;
            //            }
            //        }
            //        else
            //        {
            //            return ApplicationUrl + DesktopModuleFolderName + "itemlink.aspx?itemId=" + id.ToString(CultureInfo.InvariantCulture);
            //        }
            //    }
            //}
            //else
            //{
            //    return string.Empty;
            //}
            #endregion
        }

        public string GetItemLinkTarget(object itemId)
        {
            int curItemId;
            Int32.TryParse(itemId.ToString(), out curItemId);
            if (curItemId > 0)
            {
                Item i = BindItemData(curItemId);
                if (i.NewWindow)
                {
                    return "_blank";
                }
            }
            return "";
        }



        protected string BuildOtherParameters()
        {
            //add to this if necessary, currently we're only looking for language
            object o = Request.QueryString["language"];
            if (o != null)
            {
                //if languages are turned on we should pass the language querystring parameter
                if (UserId > -1 && UserInfo.Profile.PreferredLocale != null)
                {
                    if (UserInfo.Profile.PreferredLocale != CultureInfo.CurrentCulture.Name)
                    {
                        return "&language=" + UserInfo.Profile.PreferredLocale.ToString();
                    }
                }
                    return "&language=" + o.ToString();
            }
            return string.Empty;
        }

        public string GetItemLinkUrl(object itemId, int portalId)
        {
            return Util.Utility.IsDisabled(Convert.ToInt32(itemId, CultureInfo.InvariantCulture), portalId) ? string.Empty : GetItemLinkUrl((itemId));

        }

        public string GetItemLinkUrlExternal(object itemId)
        {
            if (IsShortLinkEnabled)
            {
                return "http://" + PortalAlias.HTTPAlias  + "/itemlink.aspx?itemId=" + itemId;
            }
            else
            {
                return "http://" + PortalAlias.HTTPAlias + DesktopModuleFolderName + "itemlink.aspx?itemId=" + itemId;
            }

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
            url.Append(tags);

            return url.ToString();
        }

        protected string GetEditUrl(string itemId)
        {
            return EditUrl("itemId", itemId, "Edit");
        }

        public void SetPageTitle()
        {

            //TODO: should we also allow for setting the module title here?
            if (AllowTitleUpdate)
            {
                DotNetNuke.Framework.CDefault tp = (DotNetNuke.Framework.CDefault)this.Page;
                tp.Title = Utility.HasValue(this.VersionInfoObject.MetaTitle) ? this.versionInfoObject.MetaTitle : this.versionInfoObject.Name;

                if (this.LogBreadcrumb)
                {
                    AddBreadcrumb(this.versionInfoObject.Name);
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
                Page.SetFocus(tp.ClientID);
            }
        }

        public void SetRssUrl(string rssUrl, string rssTitle)
        {
            if (rssUrl != null && rssTitle != null)
            {
                LiteralControl lc = new LiteralControl();
                StringBuilder sb = new StringBuilder(400);
                sb.Append("<link rel=\"alternate\" type=\"application/rss+xml\" href=\"");
                sb.Append(HttpUtility.UrlEncode(rssUrl.ToString()));
                sb.Append("\" title=\"");
                sb.Append(rssTitle.ToString());
                sb.Append("\" />");
                lc.Text = sb.ToString();

                DotNetNuke.Framework.CDefault tp = (DotNetNuke.Framework.CDefault)this.Page;
                if (tp != null)
                {
                    tp.Header.Controls.Add(lc);
                }
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Compiler doesn't see validation")]
        public string GetThumbnailUrl(object objFileName)
        {
            string fileName = objFileName as string ?? string.Empty;
            if (!Utility.HasValue(fileName))
            {
                return ApplicationUrl + "/images/spacer.gif";
            }
            else
            {
                //check if we're storing another URL, if it doesn't start with a / then we have trouble brewing
                if (fileName.StartsWith(ThumbnailSubdirectory, StringComparison.OrdinalIgnoreCase) ||
                    (!fileName.StartsWith("/", StringComparison.Ordinal) && !fileName.StartsWith("http", StringComparison.OrdinalIgnoreCase)))
                {
                    return GenerateLocalThumbnailUrl(fileName, PortalId);
                }
                else
                {
                    return fileName;
                }
            }
        }

        private string GenerateLocalThumbnailUrl(string fileName, int portalId)
        {
            //DotNetNuke.Entities.Portals.PortalSettings ps = Utility.GetPortalSettings(portalId);
            return Request.Url.Scheme + "://" + Request.Url.Host + PortalSettings.HomeDirectory + fileName;
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
                    parentCategoryId = Category.GetParentCategory(this.VersionInfoObject.ItemId, PortalId);
                }
            }

            return DotNetNuke.Common.Globals.NavigateURL(TabId, string.Empty, "ctl=" + Utility.AdminContainer, "mid=" + ModuleId.ToString(CultureInfo.InvariantCulture),
                "adminType=" + type.Name + "list", "categoryId=" + parentCategoryId.ToString(CultureInfo.InvariantCulture));
        }
    }
}

