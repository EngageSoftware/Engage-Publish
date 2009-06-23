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
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Xml.Serialization;
    using Data;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Host;
    using Util;
    using Localize = DotNetNuke.Services.Localization.Localization;

    /// <summary>
    /// Summary description for Category.
    /// </summary>
    [XmlRootAttribute(ElementName = "category", IsNullable = false)]
    public class Category : Item
    {
        //[XmlAttribute(AttributeName="noNamespaceSchemaLocation", Namespace="http://www.w3.org/2001/XMLSchema-instance")]
        //public string noNamespaceSchemaLocation = "Content.Publish.xsd";

        #region "Private Properties"
        private int sortOrder = 5;

        private int childDisplayTabId = -1;

        private string childDisplayTabName = string.Empty;

        #endregion

        #region "Public Properties"
        [XmlElement(Order = 39)]
        public int SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }

        [XmlElement(Order = 40)]
        public int ChildDisplayTabId
        {
            get { return childDisplayTabId; }
            set { childDisplayTabId = value; }
        }
        [XmlElement(Order = 41)]
        public string ChildDisplayTabName
        {
            get
            {
                if (childDisplayTabName.Length == 0)
                {
                    using (IDataReader dr = DataProvider.Instance().GetPublishTabName(childDisplayTabId, PortalId))
                    {
                        if (dr.Read())
                        {
                            childDisplayTabName = dr["TabName"].ToString();
                        }
                    }
                }
                return childDisplayTabName;
            }
            set
            {
                childDisplayTabName = value;
            }
        }



        #endregion

        #region "Public Methods"

        public Category()
        {
            //this type is always a Category
            ItemTypeId = ItemType.Category.GetId();
        }



        #endregion

        #region Item method implementation

        public override void Save(int authorId)
        {
            IDbConnection newConnection = DataProvider.GetConnection();
            IDbTransaction trans = newConnection.BeginTransaction();

            //int relationTypeId = RelationshipType.ItemToParentCategory.GetId();

            //create a transaction
            //get a connection
            try
            {
                SaveInfo(trans, authorId);
                UpdateItem(trans, ItemId, ModuleId);
                //TODO: only do the following if admin
                UpdateApprovalStatus(trans);
                //update category version now
                AddCategoryVersion(trans, ItemVersionId, ItemId, this.SortOrder, this.ChildDisplayTabId);
                SaveRelationships(trans);
                trans.Commit();
            }
            catch
            {
                trans.Rollback();
                //rollback and throw an error
                ItemVersionId = OriginalItemVersionId;
                throw;
            }
            finally
            {
                //clean up connection stuff
                newConnection.Close();
            }

            //Save Tags
            SaveTags();
            SaveItemVersionSettings();


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
            finally
            {
                //clean up connection stuff
                newConnection.Close();
            }
        }

        public override string EmailApprovalBody
        {
            get
            {
                return Localize.GetString("EMAIL_APPROVAL_BODY", "~" + Utility.DesktopModuleFolderName + "categorycontrols/App_LocalResources/categoryedit");
            }
        }

        public override string EmailStatusChangeBody
        {
            get
            {
                return Localize.GetString("EMAIL_STATUSCHANGE_BODY", "~" + Utility.DesktopModuleFolderName + "categorycontrols/App_LocalResources/categoryedit");
            }
        }

        public override string EmailStatusChangeSubject
        {
            get
            {
                return Localize.GetString("EMAIL_APPROVAL_SUBJECT", "~" + Utility.DesktopModuleFolderName + "categorycontrols/App_LocalResources/categoryedit");
            }
        }

        public override string EmailApprovalSubject
        {
            get
            {
                return Localize.GetString("EMAIL_STATUSCHANGE_SUBJECT", "~" + Utility.DesktopModuleFolderName + "categorycontrols/App_LocalResources/categoryedit");
            }
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Creates an empty Category object
        /// </summary>
        /// <param name="portalId">The Portal ID of the portal this category belongs to.</param>
        /// <returns>A <see cref="Category" /> with the assigned values.</returns>

        public static Category Create(int portalId)
        {
            var i = new Category {PortalId = portalId};
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
            var c = new Category {Name = name, Description = description, AuthorUserId = authorUserId};

            //default to the top level item type of category
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
            Category c = Create(name, description, authorUserId, moduleId, portalId, displayTabId);
            return c;
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

        public static DataTable GetChildCategories(int parentItemId, int portalId)
        {
            //return DataProvider.Instance().GetChildCategories(parentItemId, portalId);
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
            //return DataProvider.Instance().GetChildCategories(parentItemId, portalId, itemTypeId);

            string cacheKey = Utility.CacheKeyPublishChildCategoriesItemType + parentItemId.ToString(CultureInfo.InvariantCulture) + "_" + itemTypeId.ToString(CultureInfo.InvariantCulture);
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

        public static DataTable GetAllChildCategories(int parentItemId, int portalId)
        {
            //return DataProvider.Instance().GetAllChildCategories(parentItemId, portalId);

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

        public static void AddCategoryVersion(int itemVersionId, int itemId, int sortOrder, int childDisplayTabId)
        {
            DataProvider.Instance().AddCategoryVersion(itemVersionId, itemId, sortOrder, childDisplayTabId);
        }

        public static void AddCategoryVersion(IDbTransaction trans, int itemVersionId, int itemId, int sortOrder, int childDisplayTabId)
        {
            DataProvider.Instance().AddCategoryVersion(trans, itemVersionId, itemId, sortOrder, childDisplayTabId);
        }

        public static DataTable GetChildrenInCategoryPaging(int categoryId, int childTypeId, int maxItems, int portalId, bool customSort, bool customSortDirection, string sortOrder, int index, int pageSize)
        {
            return DataProvider.Instance().GetChildrenInCategoryPaging(categoryId, childTypeId, maxItems, portalId, customSort, customSortDirection, sortOrder, index, pageSize);
        }

        public static Category GetCategoryVersion(int itemVersionId, int portalId)
        {
            string cacheKey = Utility.CacheKeyPublishCategoryVersion + itemVersionId.ToString(CultureInfo.InvariantCulture);
            Category c;
            if (ModuleBase.UseCachePortal(portalId))
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
            var c = (Category)CBO.FillObject(DataProvider.Instance().GetCategory(itemId), typeof(Category));
            if (c != null)
            {
                c.CorrectDates();

                if (loadRelationships)
                {
                    c.LoadRelationships();
                }

                if (loadTags)
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

        public static Category GetCategory(int itemId, int portalId)
        {
            string cacheKey = Utility.CacheKeyPublishCategory + itemId.ToString(CultureInfo.InvariantCulture);
            Category c;
            if (ModuleBase.UseCachePortal(portalId))
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

        //		public static IDataReader GetCategories(int portalId)
        //		{
        //			return DataProvider.Instance().GetCategories(portalId);
        //		}

        public static IDataReader GetCategoryListing(int parentItemId, int portalId)
        {
            return DataProvider.Instance().GetCategoryListing(parentItemId, portalId);
        }

        public static int GetOldCategoryId(int itemId)
        {
            return DataProvider.Instance().GetOldCategoryId(itemId);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Not a reusable library")]
        public static List<Article> GetCategoryArticles(int itemId, int portalId)
        {
            DataTable children = GetAllChildren(ItemType.Article.GetId(), itemId, RelationshipType.ItemToParentCategory.GetId(), RelationshipType.ItemToRelatedCategory.GetId(), portalId).Tables[0];
            var articles = new List<Article>(children.Rows.Count);

            foreach (DataRow row in children.Rows)
            {
                articles.Add(Article.GetArticle((int)row["ItemId"], portalId));
            }
            return articles;
        }

        #endregion

        #region TransportableElement Methods

        /// <summary>
        /// This method is invoked by the Import mechanism and has to take this instance of a Category and resolve
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

            //display tab
            using (IDataReader dr = DataProvider.Instance().GetPublishTabId(ChildDisplayTabName, PortalId))
            {
                if (dr.Read())
                {
                    childDisplayTabId = (int)dr["TabId"];
                }
                else
                {
                    //Default to setting for module
                    string settingName = Utility.PublishDefaultDisplayPage + PortalId.ToString(CultureInfo.InvariantCulture);
                    string setting = HostSettings.GetHostSetting(settingName);
                    childDisplayTabId = Convert.ToInt32(setting, CultureInfo.InvariantCulture);
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
    }
}

