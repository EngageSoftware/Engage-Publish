//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Xml.Serialization;
using DotNetNuke.Entities.Host;
using Engage.Dnn.Publish.Data;
using Engage.Dnn.Publish.Util;
using Localize = DotNetNuke.Services.Localization.Localization;
using DotNetNuke.Common.Utilities;

namespace Engage.Dnn.Publish
{
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
		#endregion

        [XmlElement(Order = 39)]
		public int SortOrder
		{
			get { return sortOrder;}
			set { sortOrder = value;}
		}

        [XmlElement(Order = 40)]
		public int ChildDisplayTabId
		{
			get { return childDisplayTabId;}
			set { childDisplayTabId = value;}
		}


        private string childDisplayTabName = string.Empty;
        [XmlElement(Order = 41)]
        public string ChildDisplayTabName
        {
            get 
            {
                if (childDisplayTabName.Length == 0)
                {
                    using (IDataReader dr = Data.DataProvider.Instance().GetPublishTabName(childDisplayTabId, PortalId))
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
			IDbTransaction trans;// = null;
			IDbConnection newConnection = DataProvider.GetConnection();
			trans = newConnection.BeginTransaction();
			
			//int relationTypeId = RelationshipType.ItemToParentCategory.GetId();

			//create a transaction
			//get a connection
			try
			{
				base.SaveInfo(trans, authorId);
				
				//TODO: only do the following if admin
				base.UpdateApprovalStatus(trans);

				//update category version now
				Category.AddCategoryVersion(trans, base.ItemVersionId, base.ItemId, this.SortOrder, this.ChildDisplayTabId);

                SaveItemVersionSettings(trans);

				SaveRelationships(trans);

                //Save Tags
                SaveTags(trans);


				//do all category save
				trans.Commit();
			}
			catch
			{
				trans.Rollback();
				//rollback and throw an error
				base.ItemVersionId = base.OriginalItemVersionId;
				throw;
			}
			finally
			{
				//clean up connection stuff
				newConnection.Close();
			}
		}

        public override void UpdateApprovalStatus()
        {
            IDbTransaction trans;// = null;
            IDbConnection newConnection = DataProvider.GetConnection();
            trans = newConnection.BeginTransaction();
            try
            {
                base.UpdateApprovalStatus(trans);
                trans.Commit();
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
                return Localize.GetString("EMAIL_APPROVAL_SUBJECT", "~" + Utility.DesktopModuleFolderName + "categorycontrols/App_LocalResources/categoryedit");
            }
        }

        public override string EmailStatusChangeSubject
        {
            get
            {
                return Localize.GetString("EMAIL_STATUSCHANGE_BODY", "~" + Utility.DesktopModuleFolderName + "categorycontrols/App_LocalResources/categoryedit");
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

		public static Category Create(int portalId)
		{
			Category i = new Category();
			i.PortalId = portalId;
			return i;
		}

		public static DataSet GetTopLevelCategories(int portalId)
		{	
			return DataProvider.Instance().GetTopLevelCategories(portalId);
		}

		public static DataTable GetChildCategories(int parentItemId, int portalId)
		{	
			return DataProvider.Instance().GetChildCategories(parentItemId, portalId);
		}

        public static DataTable GetChildCategories(int parentItemId, int portalId, int itemTypeId)
        {
            return DataProvider.Instance().GetChildCategories(parentItemId, portalId, itemTypeId);
        }

		public static DataTable GetAllChildCategories(int parentItemId, int portalId)
		{	
			return DataProvider.Instance().GetAllChildCategories(parentItemId, portalId);
		}

		public static int GetParentCategory(int childItemId, int portalId)
		{	
			return DataProvider.Instance().GetParentCategory(childItemId, portalId);
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

            //cache this 
			Category c = (Category)DotNetNuke.Common.Utilities.CBO.FillObject(DataProvider.Instance().GetCategoryVersion(itemVersionId, portalId), typeof(Category));
            if (c != null)
            {
                c.CorrectDates();
            }
            return c;
		}

        public static Category GetCategory(string categoryName, int portalId)
        {
            //cache this
            int itemId = DataProvider.Instance().GetCategoryItemId(categoryName, portalId);
            Category c = GetCategory(itemId, portalId);

            return c;
        }

        public static Category GetCategory(int itemId)
        {

            Category c = (Category)DotNetNuke.Common.Utilities.CBO.FillObject(DataProvider.Instance().GetCategory(itemId), typeof(Category));
            if (c != null)
            {
                c.CorrectDates();
            }
            return c;
        }

		public static Category GetCategory(int itemId, int portalId)
		{
            string cacheKey = Utility.CacheKeyPublishCategory + itemId.ToString(CultureInfo.InvariantCulture);
            Category c = new Category();
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey) as object;
                if (o != null)
                {
                    c = (Category)o;
                }
                else
                {
                    c = (Category)DotNetNuke.Common.Utilities.CBO.FillObject(DataProvider.Instance().GetCategory(itemId), typeof(Category));
                    if (c != null)
                    {
                        c.CorrectDates();
                    }
                }
                DataCache.SetCache(cacheKey, c, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                Utility.AddCacheKey(cacheKey, portalId);
            }
            else
            {
                c = (Category)DotNetNuke.Common.Utilities.CBO.FillObject(DataProvider.Instance().GetCategory(itemId), typeof(Category));
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
            DataTable children = Item.GetAllChildren(ItemType.Article.GetId(), itemId, RelationshipType.ItemToParentCategory.GetId(), RelationshipType.ItemToRelatedCategory.GetId(), portalId).Tables[0];
            List<Article> articles = new List<Article>(children.Rows.Count);

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
            using (IDataReader dr = Data.DataProvider.Instance().GetPublishTabId(ChildDisplayTabName, PortalId))
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
            ItemRelationship relationship = new ItemRelationship();

            relationship.RelationshipTypeId = RelationshipType.CategoryToTopLevelCategory.GetId();
            relationship.ParentItemId = TopLevelCategoryItemType.Category.GetId();
            this.Relationships.Add(relationship);

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
                    Save(this.RevisingUserId);
                }
            }
        }

        #endregion
    }
}

