// <copyright file="Item.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2010
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Xml.Serialization;

    using DotNetNuke.Common;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Entities.Tabs;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Security.Roles;
    using DotNetNuke.Services.Mail;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Portability;
    using Engage.Dnn.Publish.Util;

    /// <summary>
    /// Summary description for ItemInfo.
    /// </summary>
    public abstract class Item : TransportableElement
    {
        private readonly ItemRelationshipCollection relationships;

        private readonly ItemTagCollection tags;

        private readonly ItemVersionSettingCollection versionSettings;

        private string approvalComments = string.Empty;

        private string approvalDate = string.Empty;

        private int approvalStatusId = -1;

        private int approvalUserId = -1;

        private int approvedItemVersionId = -1;

        private int authorUserId = -1;

        private string createdDate = string.Empty;

        private string description = string.Empty;

        private bool disabled;

        private int displayTabId = -1;

        private string displayTabName = string.Empty;

        private string endDate;

        private int itemId = -1;

        private Guid itemIdentifier;

        private int itemTypeId = -1;

        private string itemVersionDate = string.Empty;

        private int itemVersionId = -1;

        private Guid itemVersionIdentifier;

        private int languageId = -1;

        private string lastUpdated = string.Empty;

        private string metaDescription = string.Empty;

        private string metaKeywords = string.Empty;

        private string metaTitle = string.Empty;

        private int moduleId = -1;

        private string name = string.Empty;

        private bool newWindow;

        private string originalApprovalUser = string.Empty;

        private string originalAuthor = string.Empty;

        private int originalItemVersionId = -1;

        private string originalRevisingUser = string.Empty;

        private int portalId = -1;

        private int revisingUserId = -1;

        private string startDate = string.Empty;

        private string thumbnail = string.Empty;

        private string url = string.Empty;

        protected Item()
        {
            this.startDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            this.relationships = new ItemRelationshipCollection();
            this.versionSettings = new ItemVersionSettingCollection();
            this.tags = new ItemTagCollection();
        }

        [XmlElement(Order = 26)]
        public string ApprovalComments
        {
            get { return this.approvalComments; }
            set { this.approvalComments = value; }
        }

        [XmlElement(Order = 23)]
        public string ApprovalDate
        {
            get { return this.approvalDate; }
            set { this.approvalDate = value; }
        }

        [XmlElement(Order = 21)]
        public int ApprovalStatusId
        {
            get { return this.approvalStatusId; }
            set { this.approvalStatusId = value; }
        }

        [XmlElement(Order = 22)]
        public string ApprovalStatusName
        {
            get { return ApprovalStatus.GetFromId(this.approvalStatusId, typeof(ApprovalStatus)).Name; }
            set { }
        }

        [XmlElement(Order = 25)]
        public string ApprovalUser
        {
            get { return this.originalApprovalUser; }
            set { this.originalApprovalUser = value; }
        }

        [XmlElement(Order = 24)]
        public int ApprovalUserId
        {
            get { return this.approvalUserId; }
            set { this.approvalUserId = value; }
        }

        [XmlElement(Order = 6)]
        public int ApprovedItemVersionId
        {
            get { return this.approvedItemVersionId; }
            set { this.approvedItemVersionId = value; }
        }

        [XmlElement(Order = 7)]
        public string ApprovedItemVersionIdentifier
        {
            get
            {
                if (this.approvedItemVersionId == 0)
                {
                    return this.ItemVersionIdentifier.ToString();
                }

                // must resolve id to guid
                string approvedItemVersionIdentifier = string.Empty;
                using (IDataReader dr = DataProvider.Instance().GetItemVersionInfo(this.approvedItemVersionId))
                {
                    if (dr.Read())
                    {
                        approvedItemVersionIdentifier = dr["ItemVersionIdentifier"].ToString();
                    }
                }

                return approvedItemVersionIdentifier;
            }

            set { }
        }

        [XmlElement(Order = 20)]
        public string Author
        {
            get
            {
                // UserController controller = new UserController();
                ////verify that the user is a user in this system. 
                // UserInfo user = controller.GetUserByUsername(_portalId, _originalAuthor);
                // if (user != null)
                // {
                // return user.Username;
                // }
                // else
                // {
                // return string.Empty;
                // }
                var authorNameSetting = ItemVersionSetting.GetItemVersionSetting(this.ItemVersionId, "lblAuthorName", "Text", this.PortalId);

                if (authorNameSetting != null && authorNameSetting.ToString().Trim().Length > 0)
                {
                    this.originalAuthor = authorNameSetting.PropertyValue;
                }
                else
                {
                    var uc = new UserController();
                    UserInfo ui = uc.GetUser(this.portalId, this.authorUserId);
                    if (ui != null)
                    {
                        this.originalAuthor = ui.DisplayName;
                    }
                }

                return this.originalAuthor;
            }

            set { this.originalAuthor = value; }
        }

        [XmlElement(Order = 19)]
        public int AuthorUserId
        {
            get { return this.authorUserId; }
            set { this.authorUserId = value; }
        }

        [XmlIgnore]
        public int CommentCount { get; set; }

        [XmlElement(Order = 8)]
        public string CreatedDate
        {
            get { return this.createdDate; }
            set { this.createdDate = value; }
        }

        [XmlElement(Order = 14)]
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        [XmlElement(Order = 33)]
        public bool Disabled
        {
            get { return this.disabled; }
            set { this.disabled = value; }
        }

        [XmlElement(Order = 30)]
        public int DisplayTabId
        {
            get { return this.displayTabId; }
            set { this.displayTabId = value; }
        }

        [XmlElement(Order = 31)]
        public string DisplayTabName
        {
            get
            {
                if (this.displayTabName.Length == 0)
                {
                    using (IDataReader dr = DataProvider.Instance().GetPublishTabName(this.displayTabId, this.portalId))
                    {
                        if (dr.Read())
                        {
                            this.displayTabName = dr["TabName"].ToString();
                        }
                    }
                }

                return this.displayTabName;
            }

            set { this.displayTabName = value; }
        }

        public abstract string EmailApprovalBody { get; }

        public abstract string EmailApprovalSubject { get; }

        public abstract string EmailStatusChangeBody { get; }

        public abstract string EmailStatusChangeSubject { get; }

        [XmlElement(Order = 17)]
        public string EndDate
        {
            get { return this.endDate; }
            set { this.endDate = Engage.Utility.HasValue(value) ? value : null; }
        }

        [XmlIgnore]
        public string GetItemExternalUrl
        {
            get
            {
                string strUrl = string.Empty;
                switch (Globals.GetURLType(this.url))
                {
                    case TabType.Normal:
                        strUrl = Globals.NavigateURL(this.displayTabId);
                        break;
                    case TabType.Tab:
                        strUrl = Globals.NavigateURL(Convert.ToInt32(this.url, CultureInfo.InvariantCulture));
                        break;
                    case TabType.File:
                        strUrl = Globals.LinkClick(this.url, this.displayTabId, Null.NullInteger);
                        break;
                    case TabType.Url:
                        strUrl = this.url;
                        break;
                }

                return strUrl;
            }
        }

        public bool IsNew
        {
            get { return this.itemId == -1; }
        }

        [XmlElement(Order = 10)]
        public int ItemId
        {
            get { return this.itemId; }
            set { this.itemId = value; }
        }

        [XmlElement(Order = 4)]
        public Guid ItemIdentifier
        {
            get { return this.itemIdentifier; }
            set { this.itemIdentifier = value; }
        }

        [XmlIgnore]
        public int ItemTypeId
        {
            get { return this.itemTypeId; }
            set { this.itemTypeId = value; }
        }

        [XmlElement(Order = 15)]
        public string ItemVersionDate
        {
            get { return this.itemVersionDate; }
            set { this.itemVersionDate = value; }
        }

        [XmlElement(Order = 9)]
        public int ItemVersionId
        {
            get { return this.itemVersionId; }
            set { this.itemVersionId = value; }
        }

        [XmlElement(Order = 5)]
        public Guid ItemVersionIdentifier
        {
            get { return this.itemVersionIdentifier; }
            set { this.itemVersionIdentifier = value; }
        }

        [XmlElement(Order = 18)]
        public int LanguageId
        {
            get { return this.languageId; }
            set { this.languageId = value; }
        }

        [XmlElement(Order = 32)]
        public string LastUpdated
        {
            get { return this.lastUpdated; }
            set { this.lastUpdated = value; }
        }

        [XmlElement(Order = 28)]
        public string MetaDescription
        {
            get { return this.metaDescription; }
            set { this.metaDescription = value; }
        }

        [XmlElement(Order = 27)]
        public string MetaKeywords
        {
            get { return this.metaKeywords; }
            set { this.metaKeywords = value; }
        }

        [XmlElement(Order = 29)]
        public string MetaTitle
        {
            get { return this.metaTitle; }
            set { this.metaTitle = value; }
        }

        [XmlElement(Order = 1)]
        public int ModuleId
        {
            get { return this.moduleId; }
            set { this.moduleId = value; }
        }

        [XmlElement(Order = 2)]
        public string ModuleTitle
        {
            get
            {
                string moduleTitle = string.Empty;
                using (IDataReader dr = DataProvider.Instance().GetModuleInfo(this.moduleId))
                {
                    if (dr.Read())
                    {
                        moduleTitle = dr["ModuleTitle"].ToString();
                    }
                }

                return moduleTitle;
            }

            set { }
        }

        [XmlElement(Order = 13)]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        [XmlElement(Order = 36)]
        public bool NewWindow
        {
            get { return this.newWindow; }
            set { this.newWindow = value; }
        }

        [XmlElement(Order = 11)]
        public int OriginalItemVersionId
        {
            get { return this.originalItemVersionId; }
            set { this.originalItemVersionId = value; }
        }

        [XmlElement(Order = 12)]
        public string OriginalItemVersionIdentifier
        {
            get
            {
                if (this.originalItemVersionId <= 0)
                {
                    return this.ItemVersionIdentifier.ToString();
                }

                // must resolve id to guid
                string originalItemVersionIdentifier = string.Empty;
                using (IDataReader dr = DataProvider.Instance().GetItemVersionInfo(this.originalItemVersionId))
                {
                    if (dr.Read())
                    {
                        originalItemVersionIdentifier = dr["ItemVersionIdentifier"].ToString();
                    }
                }

                return originalItemVersionIdentifier;
            }

            set { }
        }

        [XmlElement(Order = 3)]
        public int PortalId
        {
            get { return this.portalId; }
            set { this.portalId = value; }
        }

        [XmlIgnore]
        public ItemRelationshipCollection Relationships
        {
            get { return this.relationships; }
        }

        [XmlElement(Order = 37)]
        public string RevisingUser
        {
            get { return this.originalRevisingUser; }
            set { this.originalRevisingUser = value; }
        }

        [XmlElement(Order = 38)]
        public int RevisingUserId
        {
            get { return this.revisingUserId; }
            set { this.revisingUserId = value; }
        }

        [XmlElement(Order = 16)]
        public string StartDate
        {
            get { return this.startDate; }
            set { this.startDate = Engage.Utility.HasValue(value) ? value : null; }
        }

        [XmlIgnore]
        public ItemTagCollection Tags
        {
            get { return this.tags; }
        }

        [XmlElement(Order = 34)]
        public string Thumbnail
        {
            get { return this.thumbnail; }
            set { this.thumbnail = value; }
        }

        [XmlElement(Order = 35)]
        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }

        [XmlIgnore]
        public ItemVersionSettingCollection VersionSettings
        {
            get { return this.versionSettings; }
        }

        [XmlIgnore]
        public int ViewCount { get; set; }

        public static int AddItem(IDbTransaction trans, int itemTypeId, int portalId, int moduleId, Guid itemIdentifier)
        {
            return DataProvider.Instance().AddItem(trans, itemTypeId, portalId, moduleId, itemIdentifier);
        }

        public static int AddItemVersion(
            int itemId, 
            int originalItemVersionId, 
            string name, 
            string description, 
            string startDate, 
            string endDate, 
            int languageId, 
            int authorUserId, 
            string metaKeywords, 
            string metaDescription, 
            string metaTitle, 
            int displayTabId, 
            bool disabled, 
            string thumbnail, 
            Guid itemVersionIdentifier, 
            string url, 
            bool newWindow, 
            int revisingUserId)
        {
            return DataProvider.Instance().AddItemVersion(
                itemId, 
                originalItemVersionId, 
                name, 
                description, 
                startDate, 
                endDate, 
                languageId, 
                authorUserId, 
                metaKeywords, 
                metaDescription, 
                metaTitle, 
                displayTabId, 
                disabled, 
                thumbnail, 
                itemVersionIdentifier, 
                url, 
                newWindow, 
                revisingUserId);
        }

        public static int AddItemVersion(
            IDbTransaction trans, 
            int itemId, 
            int originalItemVersionId, 
            string name, 
            string description, 
            string startDate, 
            string endDate, 
            int languageId, 
            int authorUserId, 
            string metaKeywords, 
            string metaDescription, 
            string metaTitle, 
            int displayTabId, 
            bool disabled, 
            string thumbnail, 
            Guid itemVersionIdentifier, 
            string url, 
            bool newWindow, 
            int revisingUserId)
        {
            return DataProvider.Instance().AddItemVersion(
                trans, 
                itemId, 
                originalItemVersionId, 
                name, 
                description, 
                startDate, 
                endDate, 
                languageId, 
                authorUserId, 
                metaKeywords, 
                metaDescription, 
                metaTitle, 
                displayTabId, 
                disabled, 
                thumbnail, 
                itemVersionIdentifier, 
                url, 
                newWindow, 
                revisingUserId);
        }

        /// <summary>
        /// Clears the comment count on the item table to 0 for all items within a portal
        /// </summary>
        /// <param name="portalId">The Portal in which the items will be cleared</param>
        /// <returns></returns>
        public static void ClearItemsCommentCount(int portalId)
        {
            DataProvider.Instance().ClearItemsCommentCount(portalId);
        }

        /// <summary>
        /// Clears the view count on the item table to 0 for all items within a portal
        /// </summary>
        /// <param name="portalId">The Portal in which the items will be cleared</param>
        /// <returns></returns>
        public static void ClearItemsViewCount(int portalId)
        {
            DataProvider.Instance().ClearItemsViewCount(portalId);
        }

        [Obsolete("This method signature should not be used, please use the signature that accepts PortalId as a parameter so that the cache is cleared properly. DeleteItem(int _itemId, int _portalId).", false)]
        public static void DeleteItem(int itemId)
        {
            DataProvider.Instance().DeleteItem(itemId);
        }

        public static void DeleteItem(int itemId, int portalId)
        {
            DataProvider.Instance().DeleteItem(itemId);
            Utility.ClearPublishCache(portalId);
        }

        public static bool DoesItemExist(string name, int authorUserId)
        {
            // try loading the item, if we get an ItemID back we know this already exists.
            if (DataProvider.Instance().FindItemId(name, authorUserId) > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks to see if an item exists by a specific name, from a specific author, in a specific category.
        /// </summary>
        /// <param name="name">The name of the item</param>
        /// <param name="authorUserId">The ID of the author</param>
        /// <param name="categoryId">The ID of the category</param>
        /// <returns>true or false</returns>
        public static bool DoesItemExist(string name, int authorUserId, int categoryId)
        {
            // try loading the item, if we get an ItemID back we know this already exists.
            return DataProvider.Instance().FindItemId(name, authorUserId, categoryId) > 0;
        }

        public static DataSet GetAllChildren(int parentItemId, int relationshipTypeId, int portalId)
        {
            return DataProvider.Instance().GetAllChildren(parentItemId, relationshipTypeId, portalId);
        }

        public static DataSet GetAllChildren(int itemTypeId, int parentItemId, int relationshipTypeId, int portalId)
        {
            return DataProvider.Instance().GetAllChildren(itemTypeId, parentItemId, relationshipTypeId, portalId);
        }

        public static DataSet GetAllChildren(int itemTypeId, int parentItemId, int relationshipTypeId, int otherRelationshipTypeId, int portalId)
        {
            return DataProvider.Instance().GetAllChildren(itemTypeId, parentItemId, relationshipTypeId, otherRelationshipTypeId, portalId);
        }

        public static IDataReader GetAllChildrenAsDataReader(
            int itemTypeId, int parentItemId, int relationshipTypeId, int otherRelationshipTypeId, int portalId)
        {
            return DataProvider.Instance().GetAllChildrenAsDataReader(itemTypeId, parentItemId, relationshipTypeId, otherRelationshipTypeId, portalId);
        }

        [Obsolete("This method is not used.")]
        public static DataSet GetChildren(int parentItemId, int relationshipTypeId, int portalId)
        {
            return DataProvider.Instance().GetChildren(parentItemId, relationshipTypeId, portalId);
        }

        public static Item GetItem(int itemId, int portalId, int itemTypeId, bool isCurrent)
        {
            string cacheKey = Utility.CacheKeyPublishItem + itemId.ToString(CultureInfo.InvariantCulture);
            Item i;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    i = (Item)o;
                }
                else
                {
                    IDataReader dr = DataProvider.Instance().GetItem(itemId, portalId, isCurrent);
                    ItemType it = ItemType.GetFromId(itemTypeId, typeof(ItemType));

                    i = (Item)CBO.FillObject(dr, it.GetItemType);

                    // ReSharper disable ConditionIsAlwaysTrueOrFalse
                    if (i != null)
                    {
                        // ReSharper restore ConditionIsAlwaysTrueOrFalse
                        i.CorrectDates();
                        DataCache.SetCache(cacheKey, i, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                        Utility.AddCacheKey(cacheKey, portalId);
                    }
                }
            }
            else
            {
                IDataReader dr = DataProvider.Instance().GetItem(itemId, portalId, isCurrent);
                ItemType it = ItemType.GetFromId(itemTypeId, typeof(ItemType));

                i = (Item)CBO.FillObject(dr, it.GetItemType);
                i.CorrectDates();
            }

            return i;

            // IDataReader dr = DataProvider.Instance().GetItem(_itemId, _portalId, isCurrent);

            // ItemType it = ItemType.GetFromId(_itemTypeId, typeof(ItemType));

            // Item a = (Item)CBO.FillObject(dr, it.GetItemType);
            // a.CorrectDates();
            // return a;
        }

        public static int GetItemIdFromVersion(int itemVersionId, int portalId)
        {
            return DataProvider.Instance().GetItemIdFromVersion(itemVersionId, portalId);
        }

        public static int GetItemIdFromVersion(int itemVersionId)
        {
            return DataProvider.Instance().GetItemIdFromVersion(itemVersionId);
        }

        public static string GetItemType(int itemId)
        {
            return DataProvider.Instance().GetItemType(itemId);
        }

        public static string GetItemType(int itemId, int portalId)
        {
            string itemType;

            // return DataProvider.Instance().GetItemType(_itemId);
            string cacheKey = Utility.CacheKeyPublishItemTypeNameItemId + itemId.ToString(CultureInfo.InvariantCulture); // +"PageId";
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                itemType = o != null ? o.ToString() : GetItemType(itemId);
                if (itemType != null)
                {
                    DataCache.SetCache(cacheKey, itemType, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                itemType = GetItemType(itemId);
            }

            return itemType;
        }

        public static int GetItemTypeId(int itemId)
        {
            return DataProvider.Instance().GetItemTypeId(itemId);
        }

        public static int GetItemTypeId(int itemId, int portalId)
        {
            int itemTypeId;
            string cacheKey = Utility.CacheKeyPublishItemTypeIntForItemId + itemId.ToString(CultureInfo.InvariantCulture); // +"PageId";
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                itemTypeId = o != null ? Convert.ToInt32(o.ToString()) : GetItemTypeId(itemId);
                if (itemTypeId != -1)
                {
                    DataCache.SetCache(cacheKey, itemTypeId, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                itemTypeId = GetItemTypeId(itemId);
            }

            return itemTypeId;
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not displaying properties of this class.")]
        public static DataTable GetItemTypes()
        {
            // cached version below
            return DataProvider.Instance().GetItemTypes();
        }

        public static DataTable GetItemTypes(int portalId)
        {
            DataTable dt;
            string cacheKey = Utility.CacheKeyPublishItemTypesDT + portalId.ToString(CultureInfo.InvariantCulture); // +"PageId";
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    dt = (DataTable)o;
                }
                else
                {
                    dt = GetItemTypes();
                }

                if (dt != null)
                {
                    DataCache.SetCache(cacheKey, dt, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                dt = GetItemTypes();
            }

            return dt;

            // return DataProvider.Instance().GetItemTypes();
        }

        public static DataSet GetItemVersions(int itemId, int portalId)
        {
            return DataProvider.Instance().GetItemVersions(itemId, portalId);
        }

        public static IDataReader GetItems(int itemTypeId, int portalId)
        {
            return DataProvider.Instance().GetItems(itemTypeId, portalId);
        }

        public static DataSet GetItems(int parentItemId, int portalId, int relationshipTypeId)
        {
            return DataProvider.Instance().GetItems(parentItemId, portalId, relationshipTypeId);
        }

        public static DataSet GetItems(int parentItemId, int portalId, int relationshipTypeId, int itemTypeId)
        {
            return DataProvider.Instance().GetItems(parentItemId, portalId, relationshipTypeId, itemTypeId);
        }

        public static DataSet GetItems(int parentItemId, int portalId, int relationshipTypeId, int otherRelationshipTypeId, int itemTypeId)
        {
            return DataProvider.Instance().GetItems(parentItemId, portalId, relationshipTypeId, otherRelationshipTypeId, itemTypeId);
        }

        public static DataSet GetParentItems(int itemId, int portalId, int relationshipTypeId)
        {
            return DataProvider.Instance().GetParentItems(itemId, portalId, relationshipTypeId);
        }

        /// <summary>
        /// Runs the stored procedure to calculate the views and comment counts for all items.
        /// </summary>
        /// <returns></returns>
        public static void RunPublishStats()
        {
            DataProvider.Instance().RunPublishStats();
        }

        public static void UpdateItem(IDbTransaction trans, int itemId, int moduleId)
        {
            DataProvider.Instance().UpdateItem(trans, itemId, moduleId);
        }

        public static void UpdateItemVersion(
            IDbTransaction trans, int itemId, int itemVersionId, int approvalStatusId, int userId, string approvalComments)
        {
            DataProvider.Instance().UpdateItemVersion(trans, itemId, itemVersionId, approvalStatusId, userId, approvalComments);
        }

        public void AddView(int userId, int tabId, string ipAddress, string userAgent, string httpReferrer, string siteUrl)
        {
            if (ModuleBase.IsViewTrackingEnabledForPortal(this.PortalId))
            {
                DataProvider.Instance().AddItemView(this.itemId, this.itemVersionId, userId, tabId, ipAddress, userAgent, httpReferrer, siteUrl);
            }
        }

        public void CorrectDates()
        {
            if (!string.IsNullOrEmpty(this.ApprovalDate))
            {
                this.ApprovalDate = Convert.ToDateTime(this.ApprovalDate, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(this.EndDate))
            {
                this.EndDate = Convert.ToDateTime(this.EndDate, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(this.StartDate))
            {
                this.StartDate = Convert.ToDateTime(this.StartDate, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(this.CreatedDate))
            {
                this.CreatedDate = Convert.ToDateTime(this.CreatedDate, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(this.ItemVersionDate))
            {
                this.ItemVersionDate = Convert.ToDateTime(this.ItemVersionDate, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(this.LastUpdated))
            {
                this.LastUpdated = Convert.ToDateTime(this.LastUpdated, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture);
            }
        }

        public bool DisplayOnCurrentPage()
        {
            return Utility.GetValueFromCache(
                this.PortalId, 
                Utility.CacheKeyPublishDisplayOnCurrentPage + this.itemVersionId.ToString(CultureInfo.InvariantCulture), 
                delegate
                    {
                        ItemType type = ItemType.GetFromId(this.ItemTypeId, typeof(ItemType));
                        var currentPageSetting = ItemVersionSetting.GetItemVersionSetting(
                            this.ItemVersionId, type.Name + "Settings", "DisplayOnCurrentPage", this.portalId);
                        return currentPageSetting != null && Convert.ToBoolean(currentPageSetting.PropertyValue, CultureInfo.InvariantCulture);
                    });
        }

        /// <summary>
        /// Determines whether this <see cref="Item"/> should be forced to always display on its assigned <see cref="DisplayTabId"/>, or whether it can display on any tab.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this <see cref="Item"/> should be forced to always display on its assigned <see cref="DisplayTabId"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool ForceDisplayOnPage()
        {
            return Utility.GetValueFromCache(
                this.PortalId, 
                Utility.CacheKeyPublishForceDisplayOn + this.itemVersionId.ToString(CultureInfo.InvariantCulture), 
                delegate
                    {
                        ItemType type = ItemType.GetFromId(this.ItemTypeId, typeof(ItemType));
                        var forceDisplaySetting = ItemVersionSetting.GetItemVersionSetting(
                            this.ItemVersionId, type.Name + "Settings", "ForceDisplayOnPage", this.portalId);
                        return forceDisplaySetting != null && Convert.ToBoolean(forceDisplaySetting.PropertyValue, CultureInfo.InvariantCulture);
                    });
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", 
            Justification =
                "The method performs a time-consuming operation. The method is perceivably slower than the time it takes to set or get a field's value."
            )]
        public string GetApprovalStatusTypeName()
        {
            return DataProvider.Instance().GetApprovalStatusTypeName(this.ApprovalStatusId);
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Calls database")]
        public int GetParentCategoryId()
        {
            // find the parent category ID from an item
            return Category.GetParentCategory(this.ItemId, this.PortalId);
        }

        /// <summary>
        /// this is a single article to be displayed as a sub section of a page.
        /// </summary>
        /// <param name="articlePortalId">Portal in which the related article lives</param>
        /// <returns></returns>
        public Article GetRelatedArticle(int articlePortalId)
        {
            ArrayList al = ItemRelationship.GetItemRelationships(
                this.itemId, this.itemVersionId, RelationshipType.ItemToArticleLinks.GetId(), true, this.portalId);

            // ArrayList m = new ArrayList();
            Article a = null;
            foreach (ItemRelationship ir in al)
            {
                a = Article.GetArticle(ir.ParentItemId, articlePortalId);
            }

            return a;
        }

        /// <summary>
        /// Gets all articles related to this article
        /// </summary>
        /// <param name="articlePortalId">The Portal in which the related articles live</param>
        /// <returns></returns>
        public Article[] GetRelatedArticles(int articlePortalId)
        {
            ArrayList al = ItemRelationship.GetItemRelationships(
                this.ItemId, this.ItemVersionId, RelationshipType.ItemToRelatedArticle.GetId(), true, this.portalId);

            var m = new ArrayList();
            foreach (ItemRelationship ir in al)
            {
                m.Add(Article.GetArticle(ir.ParentItemId, articlePortalId));
            }

            return (Article[])m.ToArray(typeof(Article));
        }

        /// <summary>
        /// This method currently verifies that the item is assigned to a display page. Future versions
        /// will eliminate this requirement all together but for now this is needed by ItemLink.aspx when
        /// linking occurs. This could be used to test other settings to be valid before displaying.
        /// </summary>
        /// <returns></returns>
        public bool IsLinkable()
        {
            if (this.ForceDisplayOnPage())
            {
                return true;
            }

            bool isValid = Utility.IsPageOverrideable(this.portalId, this.displayTabId);
            return isValid;
        }

        public abstract void Save(int revisingUserId);

        public abstract void UpdateApprovalStatus();

        public void UpdateDescription()
        {
            DataProvider.Instance().UpdateDescription(this.itemVersionId, this.description, this.metaDescription);
            Utility.ClearPublishCache(this.PortalId);
        }

        protected void LoadItemVersionSettings()
        {
            this.VersionSettings.Clear();
            foreach (ItemVersionSetting ivr in ItemVersionSetting.GetItemVersionSettings(this.ItemVersionId))
            {
                this.VersionSettings.Add(ivr);
            }
        }

        /// <summary>
        /// Loads the <see cref="ItemRelationship"/>s for this <see cref="Item"/>, clearing any _relationships already in the <see cref="Item.Relationships"/> collection.
        /// </summary>
        protected void LoadRelationships()
        {
            this.Relationships.Clear();
            foreach (ItemRelationship relationship in ItemRelationship.GetItemRelationships(this.ItemId, this.ItemVersionId, true))
            {
                relationship.CorrectDates();
                this.Relationships.Add(relationship);
            }
        }

        /// <summary>
        /// Loads the <see cref="ItemTag"/>s for this <see cref="Item"/>, clearing any tags already in the <see cref="Item.Tags"/> collection.
        /// </summary>
        protected void LoadTags()
        {
            this.Tags.Clear();
            foreach (ItemTag tag in ItemTag.GetItemTags(this.ItemVersionId))
            {
                this.Tags.Add(tag);
            }
        }

        protected virtual void ResolveIds(int currentModuleId)
        {
            // If the XML doesn't specify a start date we'll default to Today.
            if (string.IsNullOrEmpty(this.StartDate))
            {
                this.StartDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            }

            UserInfo user = UserController.GetUserByName(this.portalId, this.Author);
            this.authorUserId = user != null ? user.UserID : UserController.GetCurrentUserInfo().UserID;

            // Revising user.
            user = UserController.GetUserByName(this.portalId, this.RevisingUser);
            this.revisingUserId = user != null ? user.UserID : UserController.GetCurrentUserInfo().UserID;

            // Approving user.
            user = UserController.GetUserByName(this.portalId, this.ApprovalUser);
            this.approvalUserId = user != null ? user.UserID : UserController.GetCurrentUserInfo().UserID;

            bool found = false;

            // display tab - try and resolve from name in XML file.
            using (IDataReader dr = DataProvider.Instance().GetPublishTabId(this.DisplayTabName, this.portalId))
            {
                if (dr.Read())
                {
                    found = true;
                    this.displayTabId = (int)dr["TabId"];
                }
            }

            if (found == false)
            {
                // The TabId couldn't be resolved using the DisplayTabName, let's try to resolve the current _moduleId
                // to a TabId.
                using (IDataReader dr = DataProvider.Instance().GetModulesByModuleId(currentModuleId))
                {
                    if (dr.Read())
                    {
                        this.displayTabId = (int)dr["TabId"];
                    }
                    else
                    {
                        // Default to setting for module
                        string settingName = Utility.PublishDefaultDisplayPage + this.PortalId.ToString(CultureInfo.InvariantCulture);
                        string setting = HostSettings.GetHostSetting(settingName);
                        this.displayTabId = Convert.ToInt32(setting, CultureInfo.InvariantCulture);
                    }
                }
            }

            // module title
            using (IDataReader dr = DataProvider.Instance().GetPublishModuleId(this.ModuleTitle, this.portalId))
            {
                if (dr.Read())
                {
                    this.moduleId = (int)dr["ModuleId"];
                }
            }

            // Language ID one day.

            // Approval Status
            ApprovalStatus status = ApprovalStatus.GetFromName(this.ApprovalStatusName, typeof(ApprovalStatus));
            this.approvalStatusId = status.GetId();
        }

        protected void SaveInfo(IDbTransaction trans, int revisingId)
        {
            // insert new version or not
            // AuthorUserId = authorId;
            this.RevisingUserId = revisingId;
            if (this.IsNew)
            {
                if (this.itemIdentifier == Guid.Empty)
                {
                    this.itemIdentifier = Guid.NewGuid();
                }

                this.itemId = AddItem(trans, this.itemTypeId, this.portalId, this.moduleId, this.itemIdentifier);
            }

            if (this.itemVersionId > 1 || this.ItemVersionIdentifier == Guid.Empty)
            {
                this.itemVersionIdentifier = Guid.NewGuid();
            }
            else
            {
                this.itemVersionIdentifier = this.ItemVersionIdentifier;
            }

            // if we aren't populating the meta description we should use the VersionInfoObject.Description                                       
            if (this.metaDescription.Trim() == string.Empty && this.description.Trim() != string.Empty)
            {
                string itemDescription = HtmlUtils.StripTags(this.description.Trim(), false);
                this.metaDescription = Utility.TrimDescription(399, itemDescription);
            }

            int ivd = AddItemVersion(
                trans, 
                this.itemId, 
                this.originalItemVersionId, 
                this.Name, 
                this.Description.Replace("<br>", "<br />"), 
                this.startDate, 
                this.endDate, 
                this.languageId, 
                this.authorUserId, 
                this.metaKeywords, 
                this.metaDescription, 
                this.metaTitle, 
                this.displayTabId, 
                this.disabled, 
                this.thumbnail, 
                this.itemVersionIdentifier, 
                this.url, 
                this.newWindow, 
                this.revisingUserId);
            this.originalItemVersionId = this.ItemVersionId;
            this.itemVersionId = ivd;
        }

        protected void SaveItemVersionSettings(IDbTransaction trans)
        {
            for (int i = 0; i < this.VersionSettings.Count; i++)
            {
                ItemVersionSetting ir = this.VersionSettings[i];

                ir.ItemVersionId = this.ItemVersionId;

                ItemVersionSetting.AddItemVersionSetting(trans, ir.ItemVersionId, ir.ControlName, ir.PropertyName, ir.PropertyValue);
            }
        }

        protected void SaveItemVersionSettings()
        {
            for (int i = 0; i < this.VersionSettings.Count; i++)
            {
                ItemVersionSetting ir = this.VersionSettings[i];

                ir.ItemVersionId = this.ItemVersionId;

                ItemVersionSetting.AddItemVersionSetting(ir.ItemVersionId, ir.ControlName, ir.PropertyName, ir.PropertyValue);
            }
        }

        protected void SaveRelationships(IDbTransaction trans)
        {
            for (int i = 0; i < this.Relationships.Count; i++)
            {
                ItemRelationship ir = this.Relationships[i];
                ir.ChildItemId = this.ItemId;
                ir.ChildItemVersionId = this.ItemVersionId;
                if (ir.StartDate == null)
                {
                    ir.StartDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                }

                if (ir.HasSortOrderBeenSet)
                {
                    ItemRelationship.AddItemRelationship(
                        trans, ir.ChildItemId, ir.ChildItemVersionId, ir.ParentItemId, ir.RelationshipTypeId, ir.StartDate, ir.EndDate, ir.SortOrder);
                }
                else
                {
                    ItemRelationship.AddItemRelationshipWithOriginalSortOrder(
                        trans, 
                        ir.ChildItemId, 
                        ir.ChildItemVersionId, 
                        ir.ParentItemId, 
                        ir.RelationshipTypeId, 
                        ir.StartDate, 
                        ir.EndDate, 
                        this.OriginalItemVersionId);
                }
            }
        }

        /// <summary>
        /// if we remove a tag from a version we should decrement the TotalItems for a tag.
        /// </summary>
        /// <param name="trans"></param>
        protected void SaveTags(IDbTransaction trans)
        {
            for (int i = 0; i < this.Tags.Count; i++)
            {
                ItemTag it = this.Tags[i];

                // if this item tag relationship already existed for another versionID don't increment the count;
                // if (!ItemTag.CheckItemTag(trans, this.ItemId, it.TagId))
                // {
                // Tag t = Tag.GetTag(it.TagId, PortalId);
                // t.TotalItems++;
                // t.Save(trans);
                // }
                it.ItemVersionId = this.ItemVersionId;

                // ad the itemtag relationship
                ItemTag.AddItemTag(trans, it.ItemVersionId, it.TagId);
            }
        }

        protected void SaveTags()
        {
            for (int i = 0; i < this.Tags.Count; i++)
            {
                ItemTag it = this.Tags[i];

                // if this item tag relationship already existed for another versionID don't increment the count;
                if (!ItemTag.CheckItemTag(this.ItemId, it.TagId))
                {
                    Tag t = Tag.GetTag(it.TagId, this.PortalId);
                    t.TotalItems++;
                    t.Save();
                }

                it.ItemVersionId = this.ItemVersionId;

                // ad the itemtag relationship
                ItemTag.AddItemTag(it.ItemVersionId, it.TagId);
            }
        }

        protected void UpdateApprovalStatus(IDbTransaction trans)
        {
            if (this.ApprovalStatusId == ApprovalStatus.Waiting.GetId())
            {
                if (ModuleBase.ApprovalEmailsEnabled(this.PortalId))
                {
                    if (!ModuleBase.IsUserAdmin(this.PortalId))
                    {
                        this.SendApprovalEmail();
                    }
                }
            }

            if (ModuleBase.ApprovalEmailsEnabled(this.PortalId))
            {
                this.SendStatusUpdateEmail();
            }

            UpdateItemVersion(trans, this.itemId, this.itemVersionId, this.approvalStatusId, this.revisingUserId, this.approvalComments);
        }

        /// <summary>
        /// Makes the URL absolute, based on the current request.
        /// </summary>
        /// <param name="url">The link URL.</param>
        /// <returns>The given URL, rooted to the current request's website</returns>
        private static string MakeUrlAbsolute(string url)
        {
            if (HttpContext.Current != null)
            {
                return Engage.Utility.MakeUrlAbsolute(HttpContext.Current.Request.Url, url);
            }

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return url;
            }

            throw new InvalidOperationException(
                string.Format("Cannot make URL ({0}) absolute because there is no current request to base it on", url));
        }

        /// <summary>
        /// Gets an instance of Engage: Publish within this item's portal.
        /// </summary>
        /// <returns>A <see cref="ModuleInfo"/> instance, or <c>null</c> if no Publish module exists in this portal</returns>
        private ModuleInfo GetAnyPublishModule()
        {
            foreach (ModuleInfo mi in new ModuleController().GetModulesByDefinition(this.PortalId, Utility.DnnFriendlyModuleName))
            {
                if (mi.IsDeleted || mi.TabID == -1 || new TabController().GetTab(mi.TabID, mi.PortalID, false).IsDeleted)
                {
                    continue;
                }

                return mi;
            }

            return null;
        }

        /// <summary>
        /// Sends an email to the users in the <see cref="Utility.PublishEmailNotificationRole"/> indicating that an item was approved.
        /// </summary>
        private void SendApprovalEmail()
        {
            UserInfo revisingUser = UserController.GetCurrentUserInfo();
            if (revisingUser.Username != null)
            {
                ArrayList users = new RoleController().GetUsersByRoleName(
                    revisingUser.PortalID, HostSettings.GetHostSetting(Utility.PublishEmailNotificationRole + this.PortalId));

                this.SendTemplatedEmail(revisingUser, (UserInfo[])users.ToArray(typeof(UserInfo)), this.EmailApprovalBody, this.EmailApprovalSubject);
            }
        }

        /// <summary>
        /// Sends an email to the author of an item's version indicating that the version's status changed.
        /// </summary>
        private void SendStatusUpdateEmail()
        {
            UserInfo revisingUser = UserController.GetCurrentUserInfo();
            if (revisingUser.Username != null)
            {
                UserInfo versionAuthor = UserController.GetUser(this.PortalId, this.authorUserId, false);

                // if this is the same user, don't email them notification.
                if (versionAuthor != null && versionAuthor.Email != revisingUser.Email)
                {
                    this.SendTemplatedEmail(
                        revisingUser, 
                        new[]
                            {
                                versionAuthor
                            }, 
                        this.EmailStatusChangeBody, 
                        this.EmailStatusChangeSubject);
                }
            }
        }

        /// <summary>
        /// Sends an email with a templated body to the given recipients' email.
        /// </summary>
        /// <param name="revisingUser">The revising user.</param>
        /// <param name="emailRecipients">The email recipients.</param>
        /// <param name="emailBodyTemplate">The email body template.</param>
        /// <param name="emailSubject">The email subject.</param>
        private void SendTemplatedEmail(UserInfo revisingUser, IEnumerable<UserInfo> emailRecipients, string emailBodyTemplate, string emailSubject)
        {
            int editModuleId = -1;
            int editTabId = -1;
            var editModule = this.GetAnyPublishModule();
            if (editModule != null)
            {
                editModuleId = editModule.ModuleID;
                editTabId = editModule.TabID;
            }

            string linkUrl = Globals.NavigateURL(
                this.DisplayTabId, string.Empty, "VersionId=" + this.ItemVersionId.ToString(CultureInfo.InvariantCulture), "modid=" + this.ModuleId);

            string linksUrl = Globals.NavigateURL(
                editTabId, 
                string.Empty, 
                "ctl=" + Utility.AdminContainer, 
                "mid=" + editModuleId.ToString(CultureInfo.InvariantCulture), 
                "adminType=VersionsList", 
                "_itemId=" + this.ItemId);

            var emailBodyBuilder = new StringBuilder(emailBodyTemplate).Replace("[ENGAGEITEMNAME]", this.name).Replace("[ENGAGEITEMLINK]", MakeUrlAbsolute(linkUrl)).
                    Replace("[ENGAGEITEMSLINK]", MakeUrlAbsolute(linksUrl)).Replace("[ADMINNAME]", revisingUser.DisplayName).Replace(
                        "[ENGAGEITEMCOMMENTS]", this.approvalComments).Replace(
                            "[ENGAGESTATUS]", ApprovalStatus.GetFromId(this.ApprovalStatusId, typeof(ApprovalStatus)).Name);

            foreach (var recipient in emailRecipients)
            {
                Mail.SendMail(
                    PortalController.GetCurrentPortalSettings().Email, 
                    recipient.Email, 
                    string.Empty, 
                    emailSubject, 
                    emailBodyBuilder.ToString(), 
                    string.Empty, 
                    "HTML", 
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    string.Empty);
            }
        }
    }
}