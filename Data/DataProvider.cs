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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.Data
{
    public abstract class DataProvider
    {
        public const string ModuleQualifier = "Publish_";

        #region Shared/Static Methods
        // singleton reference to the instantiated object 
        //private static DataProvider provider = ((DataProvider)DotNetNuke.Framework.Reflection.CreateObject("data", "Engage.Dnn.Publish.Data", ""));

        private static DataProvider provider;

        // return the provider
        public static DataProvider Instance()
        {
            if (provider == null)
            {
                const string assembly = "Engage.Dnn.Publish.Data.SqlDataprovider,EngagePublish";
                Type objectType = Type.GetType(assembly, true, true);

                provider = (DataProvider)Activator.CreateInstance(objectType);
                DataCache.SetCache(objectType.FullName, provider);
            }

            return provider;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not returning class state information")]
        public static IDbConnection GetConnection()
        {
            const string providerType = "data";
            ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);

            Provider objProvider = ((Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);
            string _connectionString;
            if (!String.IsNullOrEmpty(objProvider.Attributes["connectionStringName"]) && !String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]]))
            {
                _connectionString = System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]];
            }
            else
            {
                _connectionString = objProvider.Attributes["connectionString"];
            }

            IDbConnection newConnection = new System.Data.SqlClient.SqlConnection();
            newConnection.ConnectionString = _connectionString;
            newConnection.Open();
            return newConnection;
        }

        #endregion

        #region Abstract Methods

        #region "Get Methods"
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a simple/cheap operation")]
        public abstract DataTable GetItemTypes();
        public abstract IDataReader GetItem(int itemId, int portalId, bool isCurrent);

        public abstract IDataReader GetItems(int itemTypeId, int portalId);
        public abstract DataSet GetItems(int parentItemId, int portalId, int relationshipTypeId);
        public abstract DataSet GetItems(int parentItemId, int portalId, int relationshipTypeId, int itemTypeId);
        public abstract DataSet GetItems(int parentItemId, int portalId, int relationshipTypeId, int otherRelationshipTypeId, int itemTypeId);
        public abstract DataSet GetParentItems(int itemId, int portalId, int relationshipTypeId);

        //public abstract DataTable GetCategoriesDt(int portalId);
        public abstract DataTable GetCategories(int portalId);
        public abstract DataTable GetCategoriesByModuleId(int moduleId);
        public abstract DataTable GetCategoriesByPortalId(int portalId);
        public abstract DataTable GetCategoriesHierarchy(int portalId);
        [Obsolete("This method is not used")]
        public abstract IDataReader GetCategories(int itemTypeId, int portalId);
        public abstract DataSet GetTopLevelCategories(int portalId);
        public abstract DataTable GetChildCategories(int parentItemId, int portalId);
        public abstract DataTable GetChildCategories(int parentItemId, int portalId, int itemTypeId);
        public abstract IDataReader GetCategoryListing(int parentItemId, int portalId);

        public abstract DataSet GetAdminItemListing(int parentItemId, int itemTypeId, int relationshipTypeId, int approvalStatusId, int portalId);
        public abstract DataSet GetAdminItemListing(int parentItemId, int itemTypeId, int relationshipTypeId, int approvalStatusId, string orderBy, int portalId);
        public abstract DataSet GetAdminItemListing(int parentItemId, int itemTypeId, int relationshipTypeId, int otherRelationshipTypeId, int approvalStatusId, int portalId);
        public abstract DataSet GetAdminItemListing(int parentItemId, int itemTypeId, int relationshipTypeId, int otherRelationshipTypeId, int approvalStatusId, string orderBy, int portalId);
        public abstract DataSet GetAdminItemListingSearchKey(int parentItemId, int itemTypeId, int relationshipTypeId, int otherRelationshipTypeId, int approvalStatusId, string orderBy, string searchKey, int portalId);

        public abstract DataSet GetAdminCommentListing(int categoryId, int approvalStatusId, int portalId, int authorUserId, string articleSearch);

        public abstract int GetParentCategory(int childItemId, int portalId);
        public abstract DataTable GetParentCategories(int articleId, int portalId);

        public abstract IDataReader GetCategory(int itemId);
        public abstract int GetCategoryItemId(string categoryName, int portalId);
        public abstract IDataReader GetCategory(int itemId, int portalId);
        public abstract IDataReader GetCategoryVersion(int itemVersionId, int portalId);

        public abstract int GetOldCategoryId(int itemId);

        public abstract IDataReader GetItemRelationships(int childItemId, int childItemVersionId, int relationshipTypeId, bool isActive);
        public abstract DataSet GetItemRelationshipByItemRelationshipId(int itemRelationshipId);
        public abstract IDataReader GetAllRelationships(int moduleId);
        public abstract IDataReader GetAllRelationshipsByPortalId(int portalId);

        public abstract IDataReader GetItemChildRelationships(int parentItemId, int relationshipTypeId);
        public abstract DataSet GetAllChildren(int parentId, int relationshipTypeId, int portalId);
        public abstract DataSet GetAllChildren(int itemTypeId, int parentId, int relationshipTypeId, int portalId);
        public abstract DataSet GetAllChildren(int itemTypeId, int parentId, int relationshipTypeId, int otherRelationshipTypeId, int portalId);

        public abstract DataTable GetChildrenInCategoryPaging(int categoryId, int childTypeId, int maxItems, int portalId, bool customSort, bool customSortDirection, string sortOrder, int index, int pageSize);



        public abstract IDataReader GetAllChildrenAsDataReader(int itemTypeId, int parentId, int relationshipTypeId, int otherRelationshipTypeId, int portalId);
        [Obsolete("This method is not used.")]
        public abstract DataSet GetAllChildrenFromTwoParents(int itemTypeId, int parentId, int relationshipTypeId, int otherParentId, int otherRelationshipTypeId, int portalId);
        [Obsolete("This method is not used.")]
        public abstract DataSet GetChildren(int parentId, int relationshipTypeId, int portalId);

        public abstract DataTable GetAllChildrenNLevels(int parentCategoryId, int nLevels, int mItems, int portalId);

        public abstract DataSet GetItemVersions(int itemId, int portalId);
        public abstract IDataReader GetItemVersionInfo(int itemVersionId);
        public abstract int GetItemIdFromVersion(int itemVersionId, int portalId);
        public abstract int GetItemIdFromVersion(int itemVersionId);

        public abstract IDataReader GetRelationshipType(string relationshipName);
        public abstract IDataReader GetItemType(string itemName);
        public abstract string GetItemType(int itemId);
        public abstract int GetItemTypeId(int itemId);
        public abstract string GetItemTypeFromVersion(int itemVersionId);

        public abstract string GetItemName(int itemId);

        public abstract string GetItemTypeName(int itemTypeId);
        public abstract string GetItemTypeName(int itemTypeId, bool useCache, int portalId, int cacheTime);

        public abstract IDataReader GetTopLevelCategoryItem(string itemName);
        public abstract IDataReader GetApprovalStatusId(string itemName);

        public abstract DataTable GetDataTable(string sql, int portalId);

        public abstract DataSet GetCategoryItems(int categoryId, int itemTypeId, int approvalStatusId, SearchSortOption sort);
        public abstract DataSet GetCategoryItems(int categoryId, int itemTypeId, int approvalStatusId);
        public abstract DataSet GetCategoryItems(int categoryId, int itemTypeId);

        public abstract DataSet GetAdminKeywordSearch(string searchString, int itemTypeId, int approvalStatusId, int portalId);
        public abstract DataSet GetAdminKeywordSearch(string searchString, int itemTypeId, int portalId);


        public abstract IDataReader GetPermissionType(string permissionName);


        #region "Article Functions"

        public abstract IDataReader GetArticleVersion(int itemVersionId, int portalId);
        public abstract DataTable GetArticles(int portalId);
        public abstract DataTable GetArticlesByPortalId(int portalId);
        public abstract DataTable GetArticlesByModuleId(int moduleId);
        public abstract DataTable GetArticlesSearchIndexingUpdated(int portalId, int moduleDefId, int displayTabId);
        public abstract DataTable GetArticlesSearchIndexingNew(int portalId, int displayTabId);



        public abstract DataTable GetArticles(int parentItemId, int portalId);
        public abstract IDataReader GetArticle(int itemId, int portalId);
        public abstract IDataReader GetArticle(int itemId);
        public abstract int GetOldArticleId(int itemId);

        #endregion



        public abstract DataTable GetAllChildCategories(int parentItemId, int portalId);
        public abstract DataSet GetApprovalStatusTypes(int portalId);
        public abstract DataSet GetApprovalStatusTypesForAuthors(int portalId);
        public abstract string GetApprovalStatusTypeName(int approvalStatusId);

        #region "Item Version Settings"
        public abstract IDataReader GetItemVersionSetting(int itemVersionId, string controlName, string propertyName);
        public abstract IDataReader GetItemVersionSettings(int itemVersionId, string controlName);

        public abstract IDataReader GetItemVersionSettings(int itemVersionId);
        #endregion


        #endregion

        #region "Add Methods"
        //public abstract int AddItem(int itemTypeId, int portalId);
        public abstract int AddItem(IDbTransaction trans, int itemTypeId, int portalId, int moduleId, Guid itemIdentifier);

        public abstract int AddItemVersion(int itemId, int originalItemVersionId, string name, string description, string startDate, string endDate, int languageId, int authorUserId, string metaKeywords, string metaDescription, string metaTitle, int displayTabId, bool disabled, string thumbnail, Guid itemVersionIdentifier, string url, bool newWindow, int revisingUserId);
        public abstract int AddItemVersion(IDbTransaction trans, int itemId, int originalItemVersionId, string name, string description, string startDate, string endDate, int languageId, int authorUserId, string metaKeywords, string metaDescription, string metaTitle, int displayTabId, bool disabled, string thumbnail, Guid itemVersionIdentifier, string url, bool newWindow, int revisingUserId);

        public abstract void AddCategoryVersion(int itemVersionId, int itemId, int sortOrder, int childDisplayTabId);
        public abstract void AddCategoryVersion(IDbTransaction trans, int itemVersionId, int itemId, int sortOrder, int childDisplayTabId);

        public abstract void AddItemRelationship(int childItemId, int childItemVersionId, int parentItemId, int relationshipTypeId, string startDate, string endDate, int sortOrder);
        public abstract void AddItemRelationshipWithOriginalSortOrder(IDbTransaction trans, int childItemId, int childItemVersionId, int parentItemId, int relationshipTypeId, string startDate, string endDate, int originalItemVersionId);
        public abstract void AddItemRelationship(IDbTransaction trans, int childItemId, int childItemVersionId, int parentItemId, int relationshipTypeId, string startDate, string endDate, int sortOrder);


        public abstract void AddArticleVersion(int itemVersionId, int itemId, string versionNumber, string versionDescription, string articleText, string referenceNumber);
        public abstract void AddArticleVersion(IDbTransaction trans, int itemVersionId, int itemId, string versionNumber, string versionDescription, string articleText, string referenceNumber);

        public abstract void AddItemView(int itemId, int itemVersionId, int userId, int tabId, string ipAddress, string userAgent, string httpReferrer, string siteUrl);

        public abstract void AddItemVersionSetting(int itemVersionId, string controlName, string propertyName, string propertyValue);
        public abstract void AddItemVersionSetting(IDbTransaction trans, int itemVersionId, string controlName, string propertyName, string propertyValue);

        #endregion

        #region "Update Methods"

        public abstract void UpdateItemVersion(IDbTransaction trans, int itemId, int itemVersionId, int approvalStatusId, int userId, string approvalComments);
        public abstract void UpdateVenexusBraindump(int itemId, string indexTitle, string indexContent, string indexWashedContent, int portalId, string indexUrl);

        public abstract void UpdateDescription(int itemVersionId, string description, string metaDescription);

        public abstract void UpdateItemRelationship(int itemRelationshipId, int sortOrder);

        #endregion

        #region "Delete Method"
        public abstract void DeleteItem(int itemId);
        public abstract void DeleteItems();
        #endregion


        #endregion

        public abstract IDictionary GetViewableCategoryIds(int permissionId);
        public abstract IDictionary GetViewableArticleIds(int permissionId);

        public abstract void InsertPermission(int categoryId, int roleId, int permissionId, int userId);
        public abstract void DeletePermissions(int categoryId);
        public abstract DataTable GetAssignedRoles(int categoryId);
        public abstract DataTable GetChildrenInCategory(int categoryId, int childTypeId, int maxItems, int portalId);
        public abstract DataTable GetChildrenInCategory(int categoryId, int childTypeId, int maxItems, int portalId, string sortOrder);
        public abstract DataTable GetMostPopular(int categoryId, int childTypeId, int maxItems, int portalId);
        public abstract DataTable GetMostRecent(int childTypeId, int maxItems, int portalId);
        public abstract DataTable GetMostRecentByCategoryId(int categoryId, int childTypeId, int maxItems, int portalId);

        public abstract IDataReader GetComments(int itemId, int approvalStatusId);

        public abstract IDataReader GetSimpleGalleryAlbums(int moduleId);
        public abstract DataTable GetSimpleGalleryPhotos(int albumId, int? maxCount);
        internal abstract string GetSimpleGalleryVersion();

        public abstract IDataReader GetUltraMediaGalleryAlbums(int moduleId);
        public abstract DataTable GetUltraMediaGalleryPhotos(int albumId, int? maxCount);

        internal abstract IDataReader GetModuleByModuleId(int moduleId);

#region "Find Items"
        public abstract int FindItemId(string name, int authorUserId);
#endregion


        #region stats
        public abstract int WaitingForApprovalCount(int portalId);
        public abstract int CommentsWaitingForApprovalCount(int portalId, int authorUserId);
        #endregion

        #region Publish Reports
        public abstract DataTable GetItemViewPaging(int itemTypeId, int categoryId, int pageIndex, int pageSize, string sortOrder, string startDate, string endDate, int portalId);
        #endregion

        #region tags

        public abstract DataTable GetTags(int portalId);

        public abstract DataTable GetTagsByString(string partialTag, int portalId);
        public abstract DataTable GetPopularTags(int portalId, ArrayList tagList, bool selectTop);
        public abstract int GetPopularTagsCount(int portalId, ArrayList tagList, bool selectTop);
        
        public abstract DataTable GetItemsFromTags(int portalId, ArrayList tagList);
        public abstract DataTable GetItemsFromTagsPaging(int portalId, ArrayList tagList, int maxItems, int pageId);

        public abstract DataTable GetTag(string tag, int portalId);
        public abstract DataTable GetTag(int tagId);
        public abstract IDataReader GetItemTags(int itemVersionId);

        public abstract int CheckItemTag(IDbTransaction trans, int itemId, int tagId);
        public abstract int CheckItemTag(int itemId, int tagId);

        public abstract void AddItemTag(int itemVersionId, int tagId);
        public abstract void AddItemTag(IDbTransaction trans, int itemVersionId, int tagId);

        public abstract int AddTag(Tag tag);
        public abstract int AddTag(IDbTransaction trans,Tag tag);

        public abstract void UpdateTag(IDbTransaction trans, Tag tag);

        public abstract void UpdateTag(Tag tag);

        #endregion

        #region IPortable Methods

        //Once we've moved up to DNN 4.6.x we can nuke some of these methods. hk
        public abstract IDataReader GetModuleInfo(int moduleId);
        public abstract IDataReader GetPublishTabName(int tabId, int portalId);
        public abstract IDataReader GetPublishTabId(string tabName, int portalId);
        public abstract IDataReader GetPublishModuleId(string moduleTitle, int portalId);
        public abstract IDataReader GetItemVersion(Guid guid, int portalId);
        public abstract IDataReader GetItemRelationshipByIdentifiers(Guid parentItemIdentifier, Guid childItemVersionIdentifier, int currentPortalId);
        public abstract IDataReader GetItemVersionSettingsByModuleId(int moduleId);
        public abstract IDataReader GetItemVersionSettingsByPortalId(int portalId);

        #endregion

    }
}

