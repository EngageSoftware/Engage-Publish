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
using System.Xml.Serialization;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Portals;
using Engage.Dnn.Publish.Data;
using Engage.Dnn.Publish.Util;
using Localize = DotNetNuke.Services.Localization.Localization;

namespace Engage.Dnn.Publish
{
    /// <summary>
    /// Summary description for ArticleInfo.
    /// ArticleInfo holds all of the article specific information
    /// </summary>
    [XmlRootAttribute(ElementName = "article", IsNullable = false)]
    public class Article : Item
    {
        private string articleText = "";
        private string versionNumber = "";
        private string versionDescription = "";
        private string referenceNumber = "";
        private float averageRating;

        private readonly string[] pageSeperator = new[] { "[PAGE]" };

        public Article()
        {
            ItemTypeId = ItemType.Article.GetId();
        }

        public static Article CreateArticle(string name, string description, string articleText, int authorUserId, int parentCategoryId, int moduleId, int portalId)
        {
            Article a = new Article();
            a.Name = name;
            a.Description = description;
            a.articleText = articleText;
            a.AuthorUserId = authorUserId;

            ItemRelationship irel = new ItemRelationship();
            irel.RelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
            irel.ParentItemId = parentCategoryId;

            a.Relationships.Add(irel);
            a.StartDate = a.LastUpdated = a.CreatedDate = DateTime.Now.ToString();
            a.PortalId = portalId;
            a.ModuleId = moduleId;

            Category c = Category.GetCategory(parentCategoryId, portalId);
            a.DisplayTabId = c.ChildDisplayTabId;

            a.ApprovalStatusId = ApprovalStatus.Approved.GetId();
            a.NewWindow = false;

            a.SetDefaultItemVersionSettings();

            return a;

        }

        private void SetDefaultItemVersionSettings()
        {
            //Printer Friendly
            string hostPrinterFriendlySetting = HostSettings.GetHostSetting(Utility.PublishDefaultPrinterFriendly + PortalId.ToString(CultureInfo.InvariantCulture));
            Setting setting = Setting.PrinterFriendly;
            setting.PropertyValue = Convert.ToBoolean(hostPrinterFriendlySetting, CultureInfo.InvariantCulture).ToString();
            ItemVersionSetting itemVersionSetting = new ItemVersionSetting(setting);
            this.VersionSettings.Add(itemVersionSetting);

            //Email A Friend
            string hostEmailFriendSetting = HostSettings.GetHostSetting(Utility.PublishDefaultEmailAFriend + PortalId.ToString(CultureInfo.InvariantCulture));

            setting = Setting.EmailAFriend;
            setting.PropertyValue = Convert.ToBoolean(hostEmailFriendSetting, CultureInfo.InvariantCulture).ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            this.VersionSettings.Add(itemVersionSetting);

            //ratings
            string hostRatingSetting = HostSettings.GetHostSetting(Utility.PublishDefaultRatings + PortalId.ToString(CultureInfo.InvariantCulture));
            setting = Setting.Rating;
            setting.PropertyValue = Convert.ToBoolean(hostRatingSetting, CultureInfo.InvariantCulture).ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            this.VersionSettings.Add(itemVersionSetting);

            //comments
            string hostCommentSetting = HostSettings.GetHostSetting(Utility.PublishDefaultComments + PortalId.ToString(CultureInfo.InvariantCulture));
            setting = Setting.Comments;
            setting.PropertyValue = Convert.ToBoolean(hostCommentSetting, CultureInfo.InvariantCulture).ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            this.VersionSettings.Add(itemVersionSetting);

            if (ModuleBase.IsPublishCommentTypeForPortal(PortalId))
            {
                //forum comments
                setting = Setting.ForumComments;
                setting.PropertyValue = Convert.ToBoolean(hostCommentSetting, CultureInfo.InvariantCulture).ToString();
                itemVersionSetting = new ItemVersionSetting(setting);
                this.VersionSettings.Add(itemVersionSetting);
            }

            //include all articles from the parent category
            setting = Setting.ArticleSettingIncludeCategories;
            setting.PropertyValue = false.ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            this.VersionSettings.Add(itemVersionSetting);

            //display on current page option
            setting = Setting.ArticleSettingCurrentDisplay;
            setting.PropertyValue = false.ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            this.VersionSettings.Add(itemVersionSetting);

            //force display on specific page
            setting = Setting.ArticleSettingForceDisplay;
            setting.PropertyValue = false.ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            this.VersionSettings.Add(itemVersionSetting);

            //display return to list
            setting = Setting.ArticleSettingReturnToList;
            setting.PropertyValue = false.ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            this.VersionSettings.Add(itemVersionSetting);

            //show author
            string hostAuthorSetting = HostSettings.GetHostSetting(Utility.PublishDefaultShowAuthor + PortalId.ToString(CultureInfo.InvariantCulture));
            setting = Setting.Author;
            setting.PropertyValue = Convert.ToBoolean(hostAuthorSetting, CultureInfo.InvariantCulture).ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            this.VersionSettings.Add(itemVersionSetting);

            //show tags
            string hostTagsSetting = HostSettings.GetHostSetting(Utility.PublishDefaultShowTags + PortalId.ToString(CultureInfo.InvariantCulture));
            setting = Setting.ShowTags;
            setting.PropertyValue = Convert.ToBoolean(hostTagsSetting, CultureInfo.InvariantCulture).ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            this.VersionSettings.Add(itemVersionSetting);


            //use approvals
            string hostUseApprovalsSetting = HostSettings.GetHostSetting(Utility.PublishUseApprovals + PortalId.ToString(CultureInfo.InvariantCulture));
            setting = Setting.UseApprovals;
            setting.PropertyValue = Convert.ToBoolean(hostUseApprovalsSetting, CultureInfo.InvariantCulture).ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            this.VersionSettings.Add(itemVersionSetting);

        }

        #region Item method implementation

        public override void Save(int revisingUserId)
        {
            IDbConnection newConnection = DataProvider.GetConnection();
            IDbTransaction trans = newConnection.BeginTransaction();

            try
            {
                SaveInfo(trans, revisingUserId);
                UpdateApprovalStatus(trans);

                //update category version now
                AddArticleVersion(trans, ItemVersionId, ItemId, this.VersionNumber, this.VersionDescription, this.ArticleText, this.ReferenceNumber);
                //Save the Relationships
                SaveRelationships(trans);

                //Save the ItemVersionSettings
                //SaveItemVersionSettings(trans);

                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                //Rolling back to the original version, exception thrown.
                ItemVersionId = OriginalItemVersionId;
                throw;
            }
            finally
            {
                //clean up connection stuff
                newConnection.Close();
            }

            SaveItemVersionSettings();

            string s = HostSettings.GetHostSetting(Utility.PublishEnableTags + PortalId.ToString(CultureInfo.InvariantCulture));
            if (Utility.HasValue(s))
            {
                if (Convert.ToBoolean(s, CultureInfo.InvariantCulture))
                {
                    //Save Tags
                    //SaveTags(trans);
                    SaveTags();
                }
            }

            try
            {
                if (Utility.IsPingEnabledForPortal(this.PortalId))
                {
                    if (this.ApprovalStatusId == ApprovalStatus.Approved.GetId())
                    {
                        string surl = HostSettings.GetHostSetting(Utility.PublishPingChangedUrl + PortalId.ToString(CultureInfo.InvariantCulture));
                        string changedUrl = Utility.HasValue(surl) ? s : DotNetNuke.Common.Globals.NavigateURL(this.DisplayTabId);
                        Hashtable ht = PortalSettings.GetSiteSettings(PortalId);

                        //ping
                        Ping.SendPing(ht["PortalName"].ToString(), ht["PortalAlias"].ToString(), changedUrl, PortalId);
                    }
                }
            }
            catch (Exception)
            {
                /// WHAT IS THIS? Why try/catch and gooble up errors???
                //catch the ping exception but let everything else proceed.
                /// localize this error
                //Exceptions.ProcessModuleLoadException("Ping Error", null, exc);
                //Exceptions.ProcessModuleLoadException(Localize.GetString("PingError", LocalResourceFile), exc);

            }
            Utility.ClearPublishCache(PortalId);


        }

        public override void UpdateApprovalStatus()
        {
            IDbConnection newConnection = DataProvider.GetConnection();
            IDbTransaction trans = newConnection.BeginTransaction();
            try
            {
                UpdateApprovalStatus(trans);
                trans.Commit();
                Utility.ClearPublishCache(PortalId);
            }
            catch
            {
                trans.Rollback();
                throw;
            }
        }

        public override string EmailApprovalBody
        {
            get
            {
                string body = Localize.GetString("EMAIL_APPROVAL_BODY", "~" + Utility.DesktopModuleFolderName + "articlecontrols/App_LocalResources/articleedit");
                return body;
            }
        }

        public override string EmailApprovalSubject
        {
            get
            {
                string subject = Localize.GetString("EMAIL_APPROVAL_SUBJECT", "~" + Utility.DesktopModuleFolderName + "articlecontrols/App_LocalResources/articleedit");
                return subject;
            }
        }

        public override string EmailStatusChangeBody
        {
            get
            {
                string body = Localize.GetString("EMAIL_STATUSCHANGE_BODY", "~" + Utility.DesktopModuleFolderName + "articlecontrols/App_LocalResources/articleedit");
                return body;
            }
        }

        public override string EmailStatusChangeSubject
        {
            get
            {
                string body = Localize.GetString("EMAIL_STATUSCHANGE_SUBJECT", "~" + Utility.DesktopModuleFolderName + "articlecontrols/App_LocalResources/articleedit");
                return body;
            }
        }

        #endregion

        [XmlElement(Order = 39)]
        public string ArticleText
        {
            get { return this.articleText; }
            set { this.articleText = value; }
        }

        [XmlElement(Order = 40)]
        public string VersionNumber
        {
            get { return this.versionNumber; }
            set { this.versionNumber = value; }
        }

        [XmlElement(Order = 41)]
        public string VersionDescription
        {
            get { return this.versionDescription; }
            set { this.versionDescription = value; }
        }

        [XmlElement(Order = 42)]
        public string ReferenceNumber
        {
            get { return this.referenceNumber; }
            set { this.referenceNumber = value; }
        }

        /// <summary>
        /// Gets the average rating of this article.
        /// </summary>
        /// <value>The average rating for this article.</value>
        [XmlIgnore]
        public float AverageRating
        {
            get
            {
                return averageRating;
            }
            set
            {
                //if value is NULL in database, CBO fills it with MinValue
                this.averageRating = value != float.MinValue ? value : 0;
            }
        }


        public void AddRating(int rating, int? userId)
        {
            UserFeedback.Rating.AddRating(this.ItemVersionId, userId, rating, DataProvider.ModuleQualifier);
        }

        public string GetPage(int pageId)
        {
            string[] pageLocations = this.articleText.Split(pageSeperator, StringSplitOptions.None);

            if (pageLocations.Length > pageId)
            {
                return pageLocations[pageId - 1];
            }
            return pageLocations[pageLocations.Length - 1];
        }


        public int GetNumberOfPages
        {
            get
            {
                //string[] stringSeparators = new string[] { "[PAGE]" };

                string[] pagelocations = this.articleText.Split(pageSeperator, StringSplitOptions.None);
                return pagelocations.Length;
            }
        }

        public static Article Create(int portalId)
        {
            Article a = new Article();
            a.PortalId = portalId;
            return a;
        }

        public static Article GetArticleVersion(int articleVersionId, int portalId)
        {
            string cacheKey = Utility.CacheKeyPublishArticleVersion + articleVersionId.ToString(CultureInfo.InvariantCulture);
            Article a;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    a = (Article)o;
                }
                else
                {
                    a = (Article)CBO.FillObject(DataProvider.Instance().GetArticleVersion(articleVersionId, portalId), typeof(Article));
                    if (a != null)
                    {
                        a.CorrectDates();
                    }
                    DataCache.SetCache(cacheKey, a, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                a = (Article)CBO.FillObject(DataProvider.Instance().GetArticleVersion(articleVersionId, portalId), typeof(Article));
                if (a != null)
                {
                    a.CorrectDates();
                }
            }
            return a;
        }

        public static void AddArticleVersion(int itemVersionId, int itemId, string versionNumber, string versionDescription, string articleText, string referenceNumber)
        { DataProvider.Instance().AddArticleVersion(itemVersionId, itemId, versionNumber, versionDescription, articleText, referenceNumber); }

        public static void AddArticleVersion(IDbTransaction trans, int itemVersionId, int itemId, string versionNumber, string versionDescription, string articleText, string referenceNumber)
        { DataProvider.Instance().AddArticleVersion(trans, itemVersionId, itemId, versionNumber, versionDescription, articleText, referenceNumber); }

        public static DataTable GetArticles(int portalId)
        {
            return DataProvider.Instance().GetArticles(portalId);
        }

        public static DataTable GetArticlesByPortalId(int portalId)
        {
            return DataProvider.Instance().GetArticlesByPortalId(portalId);
        }

        public static DataTable GetArticlesByModuleId(int moduleId)
        {
            return DataProvider.Instance().GetArticlesByModuleId(moduleId);
        }

        public static DataTable GetArticlesSearchIndexingUpdated(int portalId, int moduleDefId, int displayTabId)
        {
            return DataProvider.Instance().GetArticlesSearchIndexingUpdated(portalId, moduleDefId, displayTabId);
        }

        public static DataTable GetArticlesSearchIndexingNew(int portalId, int displayTabId)
        {
            return DataProvider.Instance().GetArticlesSearchIndexingNew(portalId, displayTabId);
        }

        public static DataTable GetArticles(int parentItemId, int portalId)
        {
            return DataProvider.Instance().GetArticles(parentItemId, portalId);
        }

        public static Article GetArticle(int itemId)
        {
            IDataReader dr = DataProvider.Instance().GetArticle(itemId);
            Article a = (Article)CBO.FillObject(dr, typeof(Article));
            if (a != null)
            {
                a.CorrectDates();
            }
            return a;
        }

        public static Article GetArticle(int itemId, int portalId)
        {
            string cacheKey = Utility.CacheKeyPublishArticle + itemId.ToString(CultureInfo.InvariantCulture);
            Article a;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    a = (Article)o;
                }
                else
                {
                    a = GetArticle(itemId);
                    //if (a != null)
                    //{
                    //    a.CorrectDates();
                    //}
                }
                if (a != null)
                {
                    DataCache.SetCache(cacheKey, a, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                a = GetArticle(itemId);
                //if (a != null)
                //{
                //    a.CorrectDates();
                //}
            }
            return a;

            //IDataReader dr = DataProvider.Instance().GetArticle(itemId, portalId);
            //Article a = (Article) CBO.FillObject(dr, typeof(Article));
            //if (a != null)
            //{
            //    a.CorrectDates();
            //}
            //return a;
        }

        public static int GetOldArticleId(int itemId)
        {
            return DataProvider.Instance().GetOldArticleId(itemId);
        }

        public bool DisplayReturnToList()
        {
            ItemVersionSetting rlSetting = ItemVersionSetting.GetItemVersionSetting(this.ItemVersionId, "ArticleSettings", "DisplayReturnToList", PortalId);
            if (rlSetting != null)
            {
                return Convert.ToBoolean(rlSetting.PropertyValue, CultureInfo.InvariantCulture);
            }
            return false;
        }


        #region TransportableElement Methods

        /// <summary>
        /// This method is invoked by the Import mechanism and has to take this instance of a Article and resolve
        /// all the id's using the names supplied in the export. hk
        /// </summary>
        public override void Import(int currentModuleId, int portalId)
        {
            //The very first thing is that PortalID needs to be changed to the current portal where content is being
            //imported. Several methods resolving Id's is expecting the correct PortalId (current). hk
            PortalId = portalId;

            ResolveIds(currentModuleId);
        }

        protected override void ResolveIds(int currentModuleId)
        {
            base.ResolveIds(currentModuleId);
            bool save = false;

            //now the Unique Id's
            //Does this ItemVersion exist in my db?
            using (IDataReader dr = DataProvider.Instance().GetItemVersion(ItemVersionIdentifier, PortalId))
            {
                if (dr.Read())
                {
                    //this item already exists
                    //update some stuff???
                }
                else
                {
                    //this version does not exist.
                    ItemId = -1;
                    ItemVersionId = -1;
                    ModuleId = currentModuleId;
                    save = true;
                }
            }

            if (save) Save(this.RevisingUserId);
        }

        #endregion
    }
}

