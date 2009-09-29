//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Xml.Serialization;
    using Data;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Portals;
    using Util;
    using Localize = DotNetNuke.Services.Localization.Localization;

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

        public static Article Create(int portalId)
        {
            var a = new Article {PortalId = portalId};
            return a;
        }

        /// <summary>
        /// Creates an Article object that you can continue to modify or save back into the database. 
        /// </summary>
        /// <param name="name">Name of the Category to be created.</param>
        /// <param name="description">The description/abstract of the category to be created.</param>
        /// <param name="articleText"></param>
        /// <param name="authorUserId">The ID of the author of this category.</param>
        /// <param name="parentCategoryId"></param>
        /// <param name="moduleId">The moduleid for where this category will most likely be displayed.</param>
        /// <param name="portalId">The Portal ID of the portal this category belongs to.</param>
        /// <returns>A <see cref="Article" /> with the assigned values.</returns>
        public static Article Create(string name, string description, string articleText, int authorUserId, int parentCategoryId, int moduleId, int portalId)
        {
            var a = new Article
                        {
                                Name = name,
                                Description = description.Replace("<br>", "<br />"),
                                articleText = articleText.Replace("<br>", "<br />"),
                                AuthorUserId = authorUserId
                        };
            //should we strip <br> tags now?

            var irel = new ItemRelationship
                           {
                                   RelationshipTypeId = RelationshipType.ItemToParentCategory.GetId(),
                                   ParentItemId = parentCategoryId
                           };
            a.Relationships.Add(irel);
            a.StartDate = a.LastUpdated = a.CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            a.PortalId = portalId;
            a.ModuleId = moduleId;
            Category c = Category.GetCategory(parentCategoryId, portalId);
            a.DisplayTabId = c.ChildDisplayTabId;
            a.ApprovalStatusId = ApprovalStatus.Approved.GetId();
            a.NewWindow = false;
            a.SetDefaultItemVersionSettings();
            return a;
        }

        [Obsolete("This method should not be used, please use Article.Create. Example: Create(string name, string description, int authorUserId, int moduleId, int portalId, int displayTabId).", false)]
        public static Article CreateArticle(string name, string description, string articleText, int authorUserId, int parentCategoryId, int moduleId, int portalId)
        {
            Article a = Create(name, description, articleText, authorUserId, parentCategoryId, moduleId, portalId);
            return a;
        }

        /// <summary>
        /// This method will configure the default ItemVersionSettings for an article, it is called from the Create method on article so that users of the API do not have to pass in itemversionsettings.
        /// </summary>
        private void SetDefaultItemVersionSettings()
        {
            //Printer Friendly
            string hostPrinterFriendlySetting = HostSettings.GetHostSetting(Utility.PublishDefaultPrinterFriendly + PortalId.ToString(CultureInfo.InvariantCulture));
            Setting setting = Setting.PrinterFriendly;
            setting.PropertyValue = Convert.ToBoolean(hostPrinterFriendlySetting, CultureInfo.InvariantCulture).ToString();
            var itemVersionSetting = new ItemVersionSetting(setting);
            VersionSettings.Add(itemVersionSetting);

            //Email A Friend
            string hostEmailFriendSetting = HostSettings.GetHostSetting(Utility.PublishDefaultEmailAFriend + PortalId.ToString(CultureInfo.InvariantCulture));
            setting = Setting.EmailAFriend;
            setting.PropertyValue = Convert.ToBoolean(hostEmailFriendSetting, CultureInfo.InvariantCulture).ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            VersionSettings.Add(itemVersionSetting);

            //ratings
            string hostRatingSetting = HostSettings.GetHostSetting(Utility.PublishDefaultRatings + PortalId.ToString(CultureInfo.InvariantCulture));
            setting = Setting.Rating;
            setting.PropertyValue = Convert.ToBoolean(hostRatingSetting, CultureInfo.InvariantCulture).ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            VersionSettings.Add(itemVersionSetting);

            //comments
            string hostCommentSetting = HostSettings.GetHostSetting(Utility.PublishDefaultComments + PortalId.ToString(CultureInfo.InvariantCulture));
            setting = Setting.Comments;
            setting.PropertyValue = Convert.ToBoolean(hostCommentSetting, CultureInfo.InvariantCulture).ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            VersionSettings.Add(itemVersionSetting);

            if (ModuleBase.IsPublishCommentTypeForPortal(PortalId))
            {
                //forum comments
                setting = Setting.ForumComments;
                setting.PropertyValue = Convert.ToBoolean(hostCommentSetting, CultureInfo.InvariantCulture).ToString();
                itemVersionSetting = new ItemVersionSetting(setting);
                VersionSettings.Add(itemVersionSetting);
            }

            //include all articles from the parent category
            setting = Setting.ArticleSettingIncludeCategories;
            setting.PropertyValue = false.ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            VersionSettings.Add(itemVersionSetting);

            //display on current page option
            setting = Setting.ArticleSettingCurrentDisplay;
            setting.PropertyValue = false.ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            VersionSettings.Add(itemVersionSetting);

            //force display on specific page
            setting = Setting.ArticleSettingForceDisplay;
            setting.PropertyValue = false.ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            VersionSettings.Add(itemVersionSetting);

            //display return to list
            setting = Setting.ArticleSettingReturnToList;
            setting.PropertyValue = false.ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            VersionSettings.Add(itemVersionSetting);

            //show author
            string hostAuthorSetting = HostSettings.GetHostSetting(Utility.PublishDefaultShowAuthor + PortalId.ToString(CultureInfo.InvariantCulture));
            setting = Setting.Author;
            setting.PropertyValue = Convert.ToBoolean(hostAuthorSetting, CultureInfo.InvariantCulture).ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            VersionSettings.Add(itemVersionSetting);

            //show tags
            string hostTagsSetting = HostSettings.GetHostSetting(Utility.PublishDefaultShowTags + PortalId.ToString(CultureInfo.InvariantCulture));
            setting = Setting.ShowTags;
            setting.PropertyValue = Convert.ToBoolean(hostTagsSetting, CultureInfo.InvariantCulture).ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            VersionSettings.Add(itemVersionSetting);


            //use approvals
            string hostUseApprovalsSetting = HostSettings.GetHostSetting(Utility.PublishUseApprovals + PortalId.ToString(CultureInfo.InvariantCulture));
            setting = Setting.UseApprovals;
            setting.PropertyValue = Convert.ToBoolean(hostUseApprovalsSetting, CultureInfo.InvariantCulture).ToString();
            itemVersionSetting = new ItemVersionSetting(setting);
            VersionSettings.Add(itemVersionSetting);

        }

        #region Item method implementation

        public override void Save(int revisingUserId)
        {
            IDbConnection newConnection = DataProvider.GetConnection();
            IDbTransaction trans = newConnection.BeginTransaction();

            try
            {
                SaveInfo(trans, revisingUserId);
                UpdateItem(trans, ItemId, ModuleId);
                UpdateApprovalStatus(trans);

                //update article version now
                //replace <br> with <br />
                AddArticleVersion(trans, ItemVersionId, ItemId, VersionNumber, VersionDescription, ArticleText.Replace("<br>","<br />"), ReferenceNumber);
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

            Utility.ClearPublishCache(PortalId);
            
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
                if (Utility.IsPingEnabledForPortal(PortalId))
                {
                    if (ApprovalStatusId == ApprovalStatus.Approved.GetId())
                    {
                        string surl = HostSettings.GetHostSetting(Utility.PublishPingChangedUrl + PortalId.ToString(CultureInfo.InvariantCulture));
                        string changedUrl = Utility.HasValue(surl) ? surl : DotNetNuke.Common.Globals.NavigateURL(DisplayTabId);
                        PortalSettings ps = Utility.GetPortalSettings(PortalId);

                        //ping
                        Ping.SendPing(ps.PortalName, ps.PortalAlias.ToString(), changedUrl, PortalId);
                    }
                }
            }
            catch (Exception exc)
            {
                /// WHAT IS THIS? Why try/catch and gooble up errors???
                /// this was added because some exceptions occur for ping servers that we don't need to let the users know about.
                /// 
                //catch the ping exception but let everything else proceed.
                /// localize this error

                DotNetNuke.Services.Exceptions.Exceptions.LogException(exc);
                //Exceptions.ProcessModuleLoadException(Localize.GetString("PingError", LocalResourceFile), exc);

            }

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
            get { return articleText; }
            set { articleText = value; }
        }

        [XmlElement(Order = 40)]
        public string VersionNumber
        {
            get { return versionNumber; }
            set { versionNumber = value; }
        }

        [XmlElement(Order = 41)]
        public string VersionDescription
        {
            get { return versionDescription; }
            set { versionDescription = value; }
        }

        [XmlElement(Order = 42)]
        public string ReferenceNumber
        {
            get { return referenceNumber; }
            set { referenceNumber = value; }
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
                averageRating = value != float.MinValue ? value : 0;
            }
        }


        public void AddRating(int rating, int? userId)
        {
            UserFeedback.Rating.AddRating(ItemVersionId, userId, rating, DataProvider.ModuleQualifier);
        }

        public string GetPage(int pageId)
        {
            string[] pageLocations = articleText.Split(pageSeperator, StringSplitOptions.None);

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

                string[] pagelocations = articleText.Split(pageSeperator, StringSplitOptions.None);
                return pagelocations.Length;
            }
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

        public static DataTable GetArticlesByModuleId(int moduleId, bool isCurrent)
        {
            if(isCurrent)
            {
                return DataProvider.Instance().GetArticlesByModuleIdCurrent(moduleId);
            }
            
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

        [Obsolete("This version does not using Caching. Please use GetArticle(itemId, portalId) or version that loads relationships and tags.", false)]
        public static Article GetArticle(int itemId)
        {
            IDataReader dr = DataProvider.Instance().GetArticle(itemId);
            var a = (Article)CBO.FillObject(dr, typeof(Article));
            if (a != null)
            {
                a.CorrectDates();
            }
            return a;
        }

        public static Article GetArticle(int itemId, int portalId, bool loadRelationships, bool loadTags)
        {
            return GetArticle(itemId, portalId, loadRelationships, loadTags, false);
        }

        public static Article GetArticle(int itemId, int portalId, bool loadRelationships, bool loadTags, bool loadItemVersionSettings)
        {
            string cacheKey = Utility.CacheKeyPublishArticle + itemId.ToString(CultureInfo.InvariantCulture)

                + "loadRelationships" + loadRelationships + "loadTags" + loadTags + "loadItemVersionSettings" + loadItemVersionSettings; 
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
                    if (a != null)
                    {
                        if (loadRelationships)
                        {
                            a.LoadRelationships();
                        }
                        //we don't need to get the tags if the portal doesn't allow it
                        if (loadTags && ModuleBase.AllowTagsForPortal(portalId))
                        {
                            a.LoadTags();
                        }
                        if (loadItemVersionSettings)
                        {
                            a.LoadItemVersionSettings();
                        }
                    }

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
                if (a != null)
                {
                    if (loadRelationships)
                    {
                        a.LoadRelationships();
                    }
                    //we don't need to get the tags if the portal doesn't allow it
                    if (loadTags && ModuleBase.AllowTagsForPortal(portalId))
                    {
                        a.LoadTags();
                    }
                    if (loadItemVersionSettings)
                    {
                        a.LoadItemVersionSettings();
                    }
                }

            }


            return a;
        }



        public static Article GetArticle(int itemId, int portalId)
        {
            return GetArticle(itemId, portalId, false, false);
        }

        public static int GetOldArticleId(int itemId)
        {
            return DataProvider.Instance().GetOldArticleId(itemId);
        }

        public bool DisplayReturnToList()
        {
            ItemVersionSetting rlSetting = ItemVersionSetting.GetItemVersionSetting(ItemVersionId, "ArticleSettings", "DisplayReturnToList", PortalId);
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

            if (save) Save(RevisingUserId);
        }

        #endregion
    }
}

