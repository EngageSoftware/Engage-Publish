// <copyright file="Category.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2011
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
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Xml.Serialization;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Controllers;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Util;

    using Localize = DotNetNuke.Services.Localization.Localization;

    /// <summary>
    /// A grouping of <see cref="Item"/>s (i.e. Articles and other Categories)
    /// </summary>
    [XmlRoot(ElementName = "category", IsNullable = false)]
    public class Category : Item
    {
        // [XmlAttribute(AttributeName="noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
        // public string noNamespaceSchemaLocation = "Content.Publish.xsd";
        private int childDisplayTabId = -1;

        private string childDisplayTabName = string.Empty;

        private int sortOrder = 5;

        public Category()
        {
            // this type is always a Category
            this.ItemTypeId = ItemType.Category.GetId();
        }

        [XmlElement(Order = 40)]
        public int ChildDisplayTabId
        {
            get { return this.childDisplayTabId; }
            set { this.childDisplayTabId = value; }
        }

        [XmlElement(Order = 41)]
        public string ChildDisplayTabName
        {
            get
            {
                if (this.childDisplayTabName.Length == 0)
                {
                    using (IDataReader dr = DataProvider.Instance().GetPublishTabName(this.childDisplayTabId, this.PortalId))
                    {
                        if (dr.Read())
                        {
                            this.childDisplayTabName = dr["TabName"].ToString();
                        }
                    }
                }

                return this.childDisplayTabName;
            }

            set
            {
                this.childDisplayTabName = value;
            }
        }

        public override string EmailApprovalBody
        {
            get
            {
                return Localize.GetString(
                    "EMAIL_APPROVAL_BODY", "~" + Utility.DesktopModuleFolderName + "categorycontrols/App_LocalResources/categoryedit");
            }
        }

        public override string EmailApprovalSubject
        {
            get
            {
                return Localize.GetString(
                    "EMAIL_STATUSCHANGE_SUBJECT", "~" + Utility.DesktopModuleFolderName + "categorycontrols/App_LocalResources/categoryedit");
            }
        }

        public override string EmailStatusChangeBody
        {
            get
            {
                return Localize.GetString(
                    "EMAIL_STATUSCHANGE_BODY", "~" + Utility.DesktopModuleFolderName + "categorycontrols/App_LocalResources/categoryedit");
            }
        }

        public override string EmailStatusChangeSubject
        {
            get
            {
                return Localize.GetString(
                    "EMAIL_APPROVAL_SUBJECT", "~" + Utility.DesktopModuleFolderName + "categorycontrols/App_LocalResources/categoryedit");
            }
        }

        [XmlElement(Order = 39)]
        public int SortOrder
        {
            get { return this.sortOrder; }
            set { this.sortOrder = value; }
        }

        public static void AddCategoryVersion(int itemVersionId, int itemId, int sortOrder, int childDisplayTabId)
        {
            DataProvider.Instance().AddCategoryVersion(itemVersionId, itemId, sortOrder, childDisplayTabId);
        }

        public static void AddCategoryVersion(IDbTransaction trans, int itemVersionId, int itemId, int sortOrder, int childDisplayTabId)
        {
            DataProvider.Instance().AddCategoryVersion(trans, itemVersionId, itemId, sortOrder, childDisplayTabId);
        }

        /// <summary>
        /// Creates an empty Category object
        /// </summary>
        /// <param name="portalId">The Portal ID of the portal this category belongs to.</param>
        /// <returns>A <see cref="Category" /> with the assigned values.</returns>
        public static Category Create(int portalId)
        {
            var i = new Category
                {
                    PortalId = portalId
                };
            return i;
        }

        /// <summary>
        /// Creates a Category object that you can continue to modify or save back into the database., 
        /// </summary>
        /// <param name="name">Name of the Category to be created.</param>
        /// <param name="description">The description/abstract of the category to be created.</param>
        /// <param name="authorUserId">The ID of the author of this category.</param>
        /// <param name="moduleId">The moduleid for where this category will most likely be displayed.</param>
        /// <param name="portalId">The Portal ID of the portal this category belongs to.</param>
        /// <param name="displayTabId">The Tab ID of the page this Category should be displayed on.</param>
        /// <returns>A <see cref="Category" /> with the assigned values.</returns>
        public static Category Create(string name, string description, int authorUserId, int moduleId, int portalId, int displayTabId)
        {
            var c = new Category
                {
                    Name = name, 
                    Description = description, 
                    AuthorUserId = authorUserId
                };

            // default to the top level item type of category
            var irel = new ItemRelationship
                {
                    RelationshipTypeId = RelationshipType.ItemToParentCategory.GetId(), 
                    ParentItemId = TopLevelCategoryItemType.Category.GetId()
                };

            c.Relationships.Add(irel);
            c.StartDate = c.LastUpdated = c.CreatedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            c.PortalId = portalId;
            c.ModuleId = moduleId;

            c.ApprovalStatusId = ApprovalStatus.Approved.GetId();
            c.NewWindow = false;

            return c;
        }

        /// <summary>
        /// Creates a Category object that you can continue to modify or save back into the database. You should use the Category.Create method instead of this. 
        /// </summary>
        /// <param name="name">Name of the Category to be created.</param>
        /// <param name="description">The description/abstract of the category to be created.</param>
        /// <param name="authorUserId">The ID of the author of this category.</param>
        /// <param name="moduleId">The moduleid for where this category will most likely be displayed.</param>
        /// <param name="portalId">The Portal ID of the portal this category belongs to.</param>
        /// <param name="displayTabId">The Tab ID of the page this Category should be displayed on.</param>
        /// <returns>A <see cref="Category" /> with the assigned values.</returns>
        [Obsolete("This method should not be used, please use Category.Create. Example: Create(string name, string description, int authorUserId, int moduleId, int portalId, int displayTabId).", false)]
        public static Category CreateCategory(string name, string description, int authorUserId, int moduleId, int portalId, int displayTabId)
        {
            return Create(name, description, authorUserId, moduleId, portalId, displayTabId);
        }

        public static DataTable GetAllChildCategories(int parentItemId, int portalId)
        {
            // return DataProvider.Instance().GetAllChildCategories(parentItemId, portalId);
            string cacheKey = Utility.CacheKeyPublishAllChildCategories + parentItemId.ToString(CultureInfo.InvariantCulture);
            DataTable dt;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    dt = (DataTable)o;
                }
                else
                {
                    dt = DataProvider.Instance().GetAllChildCategories(parentItemId, portalId);
                }

                if (dt != null)
                {
                    DataCache.SetCache(cacheKey, dt, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                dt = DataProvider.Instance().GetAllChildCategories(parentItemId, portalId);
            }

            return dt;
        }

        public static DataTable GetCategories(int portalId)
        {
            return DataProvider.Instance().GetCategories(portalId);
        }

        public static DataTable GetCategoriesByModuleId(int moduleId)
        {
            return DataProvider.Instance().GetCategoriesByModuleId(moduleId);
        }

        public static DataTable GetCategoriesByPortalId(int portalId)
        {
            return DataProvider.Instance().GetCategoriesByPortalId(portalId);
        }

        public static DataTable GetCategoriesHierarchy(int portalId)
        {
            return DataProvider.Instance().GetCategoriesHierarchy(portalId);
        }

        public static Category GetCategory(string categoryName, int portalId)
        {
            int itemId = DataProvider.Instance().GetCategoryItemId(categoryName, portalId);
            Category c = GetCategory(itemId, portalId);

            return c;
        }

        public static Category GetCategory(int itemId)
        {
            return GetCategory(itemId, false, false);
        }

        public static Category GetCategory(int itemId, bool loadRelationships, bool loadTags)
        {
            return GetCategory(itemId, loadRelationships, loadTags, false);
        }

        public static Category GetCategory(int itemId, bool loadRelationships, bool loadTags, bool loadItemVersionSettings)
        {
            // cache?
            var c = (Category)CBO.FillObject(DataProvider.Instance().GetCategory(itemId), typeof(Category));
            if (c != null)
            {
                c.CorrectDates();

                if (loadRelationships)
                {
                    c.LoadRelationships();
                }

                if (loadTags && ModuleBase.AllowTagsForPortal(c.PortalId))
                {
                    c.LoadTags();
                }

                if (loadItemVersionSettings)
                {
                    c.LoadItemVersionSettings();
                }
            }

            return c;
        }

        public static Category GetCategory(int itemId, int portalId, bool loadRelationships, bool loadTags, bool loadItemVersionSettings)
        {
            return GetCategory(itemId, portalId, loadRelationships, loadTags, loadItemVersionSettings, false);
        }

        public static Category GetCategory(int itemId, int portalId, bool loadRelationships, bool loadTags, bool loadItemVersionSettings, bool ignoreCache)
        {
            // cache?
            // var c = (Category)CBO.FillObject(DataProvider.Instance().GetCategory(itemId), typeof(Category));
            // if (c != null)
            // {
            // c.CorrectDates();

            // if (loadRelationships)
            // {
            // c.LoadRelationships();
            // }

            // if (loadTags && ModuleBase.AllowTagsForPortal(c.PortalId))
            // {
            // c.LoadTags();
            // }
            // if (loadItemVersionSettings)
            // {
            // c.LoadItemVersionSettings();
            // }
            // }
            string cacheKey = Utility.CacheKeyPublishCategory + itemId.ToString(CultureInfo.InvariantCulture) + "loadRelationships" +
                              loadRelationships + "loadTags" + loadTags + "loadItemVersionSettings" + loadItemVersionSettings;
            Category c;
            if (!ignoreCache && ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    c = (Category)o;
                }
                else
                {
                    c = (Category)CBO.FillObject(DataProvider.Instance().GetCategory(itemId), typeof(Category));
                    if (c != null)
                    {
                        c.CorrectDates();

                        if (loadRelationships)
                        {
                            c.LoadRelationships();
                        }

                        if (loadTags && ModuleBase.AllowTagsForPortal(c.PortalId))
                        {
                            c.LoadTags();
                        }

                        if (loadItemVersionSettings)
                        {
                            c.LoadItemVersionSettings();
                        }

                        DataCache.SetCache(cacheKey, c, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                        Utility.AddCacheKey(cacheKey, portalId);
                    }
                }
            }
            else
            {
                c = (Category)CBO.FillObject(DataProvider.Instance().GetCategory(itemId), typeof(Category));
                if (c != null)
                {
                    c.CorrectDates();
                    if (loadRelationships)
                    {
                        c.LoadRelationships();
                    }

                    if (loadTags && ModuleBase.AllowTagsForPortal(c.PortalId))
                    {
                        c.LoadTags();
                    }

                    if (loadItemVersionSettings)
                    {
                        c.LoadItemVersionSettings();
                    }
                }
            }

            return c;
        }

        public static Category GetCategory(int itemId, int portalId)
        {
            return GetCategory(itemId, portalId, false);
        }

        public static Category GetCategory(int itemId, int portalId, bool ignoreCache)
        {
            string cacheKey = Utility.CacheKeyPublishCategory + itemId.ToString(CultureInfo.InvariantCulture);
            Category c;
            if (!ignoreCache && ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    c = (Category)o;
                }
                else
                {
                    c = (Category)CBO.FillObject(DataProvider.Instance().GetCategory(itemId), typeof(Category));
                    if (c != null)
                    {
                        c.CorrectDates();
                        DataCache.SetCache(cacheKey, c, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                        Utility.AddCacheKey(cacheKey, portalId);
                    }
                }
            }
            else
            {
                c = (Category)CBO.FillObject(DataProvider.Instance().GetCategory(itemId), typeof(Category));
                if (c != null)
                {
                    c.CorrectDates();
                }
            }

            return c;
        }

        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Not a reusable library")]
        public static List<Article> GetCategoryArticles(int itemId, int portalId)
        {
            DataTable children =
                GetAllChildren(
                    ItemType.Article.GetId(), 
                    itemId, 
                    RelationshipType.ItemToParentCategory.GetId(), 
                    RelationshipType.ItemToRelatedCategory.GetId(), 
                    portalId).Tables[0];
            var articles = new List<Article>(children.Rows.Count);

            foreach (DataRow row in children.Rows)
            {
                articles.Add(Article.GetArticle((int)row["ItemId"], portalId));
            }

            return articles;
        }

        public static IDataReader GetCategoryListing(int parentItemId, int portalId)
        {
            return DataProvider.Instance().GetCategoryListing(parentItemId, portalId);
        }

        public static Category GetCategoryVersion(int itemVersionId, int portalId)
        {
            return GetCategoryVersion(itemVersionId, portalId, false);
        }

        public static Category GetCategoryVersion(int itemVersionId, int portalId, bool ignoreCache)
        {
            string cacheKey = Utility.CacheKeyPublishCategoryVersion + itemVersionId.ToString(CultureInfo.InvariantCulture);
            Category c;
            if (!ignoreCache && ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    c = (Category)o;
                }
                else
                {
                    c = (Category)CBO.FillObject(DataProvider.Instance().GetCategoryVersion(itemVersionId, portalId), typeof(Category));
                    if (c != null)
                    {
                        c.CorrectDates();
                    }
                }

                if (c != null)
                {
                    DataCache.SetCache(cacheKey, c, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                c = (Category)CBO.FillObject(DataProvider.Instance().GetCategoryVersion(itemVersionId, portalId), typeof(Category));
                if (c != null)
                {
                    c.CorrectDates();
                }
            }

            return c;
        }

        public static DataTable GetChildCategories(int parentItemId, int portalId)
        {
            // return DataProvider.Instance().GetChildCategories(parentItemId, portalId);
            string cacheKey = Utility.CacheKeyPublishChildCategories + parentItemId.ToString(CultureInfo.InvariantCulture);
            DataTable dt;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    dt = (DataTable)o;
                }
                else
                {
                    dt = DataProvider.Instance().GetChildCategories(parentItemId, portalId);
                }

                if (dt != null)
                {
                    DataCache.SetCache(cacheKey, dt, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                dt = DataProvider.Instance().GetChildCategories(parentItemId, portalId);
            }

            return dt;
        }

        public static DataTable GetChildCategories(int parentItemId, int portalId, int itemTypeId)
        {
            // return DataProvider.Instance().GetChildCategories(parentItemId, portalId, itemTypeId);
            string cacheKey = Utility.CacheKeyPublishChildCategoriesItemType + parentItemId.ToString(CultureInfo.InvariantCulture) + "_" +
                              itemTypeId.ToString(CultureInfo.InvariantCulture);
            DataTable dt;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    dt = (DataTable)o;
                }
                else
                {
                    dt = DataProvider.Instance().GetChildCategories(parentItemId, portalId, itemTypeId);
                }

                if (dt != null)
                {
                    DataCache.SetCache(cacheKey, dt, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                dt = DataProvider.Instance().GetChildCategories(parentItemId, portalId, itemTypeId);
            }

            return dt;
        }

        public static DataTable GetChildrenInCategoryPaging(
            int categoryId, 
            int childTypeId, 
            int maxItems, 
            int portalId, 
            bool customSort, 
            bool customSortDirection, 
            string sortOrder, 
            int index, 
            int pageSize)
        {
            return DataProvider.Instance().GetChildrenInCategoryPaging(
                categoryId, childTypeId, maxItems, portalId, customSort, customSortDirection, sortOrder, index, pageSize);
        }

        public static int GetOldCategoryId(int itemId)
        {
            return DataProvider.Instance().GetOldCategoryId(itemId);
        }

        public static int GetParentCategory(int childItemId, int portalId)
        {
            int parentId;
            string cacheKey = Utility.CacheKeyPublishItemParentCategoryId + childItemId.ToString(CultureInfo.InvariantCulture); // +"PageId";
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                parentId = o != null ? Convert.ToInt32(o.ToString()) : DataProvider.Instance().GetParentCategory(childItemId, portalId);
                if (parentId != -1)
                {
                    DataCache.SetCache(cacheKey, parentId, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                parentId = DataProvider.Instance().GetParentCategory(childItemId, portalId);
            }

            return parentId;
        }

        /// <summary>
        /// Returns a dataset of the top level categories available for a specific portal
        /// </summary>
        /// <param name="portalId">The Portal ID that we want to return data for.</param>
        /// <returns>A <see cref="DataSet" /> with the categories available.</returns>
        public static DataSet GetTopLevelCategories(int portalId)
        {
            string cacheKey = Utility.CacheKeyPublishTopLevelCategories + portalId.ToString(CultureInfo.InvariantCulture);
            DataSet ds;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    ds = (DataSet)o;
                }
                else
                {
                    ds = DataProvider.Instance().GetTopLevelCategories(portalId);
                }

                if (ds != null)
                {
                    DataCache.SetCache(cacheKey, ds, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                ds = DataProvider.Instance().GetTopLevelCategories(portalId);
            }

            return ds;
        }

        /// <summary>
        /// Updates the <see cref="Item.DisplayTabId"/> and <see cref="ChildDisplayTabId"/> settings of all children of this <see cref="Category"/> (and their children's children, etc.)
        /// </summary>
        /// <param name="revisingUser">The revising user.</param>
        /// <returns>The number of affected <see cref="Item"/>s</returns>
        public int CascadeChildDisplayTab(int revisingUser)
        {
            int count = 0;
            foreach (DataRow itemRow in GetAllChildren(this.ItemId, RelationshipType.ItemToParentCategory.GetId(), this.PortalId).Tables[0].Rows)
            {
                Item childItem;
                var itemId = (int)itemRow["itemId"];
                if (GetItemTypeId(itemId) == ItemType.Article.GetId())
                {
                    childItem = Article.GetArticle(itemId, this.PortalId, true, true, true);
                }
                else
                {
                    childItem = GetCategory(itemId, true, true);
                }

                childItem.DisplayTabId = this.ChildDisplayTabId;

                var childCategory = childItem as Category;
                if (childCategory != null)
                {
                    childCategory.ChildDisplayTabId = this.ChildDisplayTabId;
                }

                Setting displayOnCurrentPageSetting = Setting.ArticleSettingCurrentDisplay;
                displayOnCurrentPageSetting.PropertyValue = false.ToString(CultureInfo.InvariantCulture);
                childItem.VersionSettings.Add(new ItemVersionSetting(displayOnCurrentPageSetting));

                childItem.Save(revisingUser);
                count++;
            }

            return count;
        }

        /// <summary>
        /// This method is invoked by the Import mechanism and has to take this instance of a Category and resolve
        /// all the id's using the names supplied in the export. hk
        /// </summary>
        public override void Import(int currentModuleId, int portalId)
        {
            // The very first thing is that PortalID needs to be changed to the current portal where content is being
            // imported. Several methods resolving Id's is expecting the correct PortalId (current). hk
            this.PortalId = portalId;
            this.ResolveIds(currentModuleId);
        }

        public override void Save(int authorId)
        {
            IDbConnection newConnection = DataProvider.Instance().GetConnection();
            IDbTransaction trans = newConnection.BeginTransaction();

            // int relationTypeId = RelationshipType.ItemToParentCategory.GetId();

            // create a transaction
            // get a connection
            try
            {
                this.SaveInfo(trans, authorId);
                UpdateItem(trans, this.ItemId, this.ModuleId);

                // TODO: only do the following if admin
                this.UpdateApprovalStatus(trans);

                // update category version now
                AddCategoryVersion(trans, this.ItemVersionId, this.ItemId, this.SortOrder, this.ChildDisplayTabId);
                this.SaveRelationships(trans);
                trans.Commit();
            }
            catch
            {
                trans.Rollback();

                // rollback and throw an error
                this.ItemVersionId = this.OriginalItemVersionId;
                throw;
            }
            finally
            {
                // clean up connection stuff
                newConnection.Close();
            }

            // Save Tags
            this.SaveTags();
            this.SaveItemVersionSettings();

            Utility.ClearPublishCache(this.PortalId);
        }

        public override void UpdateApprovalStatus()
        {
            IDbConnection newConnection = DataProvider.Instance().GetConnection();
            IDbTransaction trans = newConnection.BeginTransaction();
            try
            {
                this.UpdateApprovalStatus(trans);
                trans.Commit();
                Utility.ClearPublishCache(this.PortalId);
            }
            catch
            {
                trans.Rollback();
                throw;
            }
            finally
            {
                // clean up connection stuff
                newConnection.Close();
            }
        }

        protected override void ResolveIds(int currentModuleId)
        {
            base.ResolveIds(currentModuleId);

            // display tab
            using (IDataReader dr = DataProvider.Instance().GetPublishTabId(this.ChildDisplayTabName, this.PortalId))
            {
                if (dr.Read())
                {
                    this.childDisplayTabId = (int)dr["TabId"];
                }
                else
                {
                    // Default to setting for module
                    string settingName = Utility.PublishDefaultDisplayPage + this.PortalId.ToString(CultureInfo.InvariantCulture);
                    string setting = HostController.Instance.GetString(settingName);
                    if (!int.TryParse(setting, NumberStyles.Integer, CultureInfo.InvariantCulture, out this.childDisplayTabId))
                    {
                        throw new InvalidOperationException("Default Display Page setting must be set in order to import items");
                    }
                }
            }

            // For situations where the user is importing content from another system (file not generated from Publish)
            // they have no way of knowing what the top level category GUIDS are nor to include the entries in the 
            // relationships section of the file. Note, the stored procedure verifies the relationship doesn't exist
            // before inserting a new row.
            var relationship = new ItemRelationship
                {
                    RelationshipTypeId = RelationshipType.CategoryToTopLevelCategory.GetId(), 
                    ParentItemId = TopLevelCategoryItemType.Category.GetId()
                };

            this.Relationships.Add(relationship);
            bool save = false;

            // now the Unique Id's
            // Does this ItemVersion exist in my db?
            using (IDataReader dr = DataProvider.Instance().GetItemVersion(this.ItemVersionIdentifier, this.PortalId))
            {
                if (dr.Read())
                {
                    // this item already exists
                    // update some stuff???
                }
                else
                {
                    // this version does not exist.
                    this.ItemId = -1;
                    this.ItemVersionId = -1;
                    this.ModuleId = currentModuleId;
                    save = true;
                }
            }

            if (save)
            {
                this.Save(this.RevisingUserId);
            }
        }
    }
}