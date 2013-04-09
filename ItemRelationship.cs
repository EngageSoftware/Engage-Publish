// <copyright file="ItemRelationship.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
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
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.UI.WebControls;
    using System.Xml.Serialization;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Services.Exceptions;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Portability;
    using Engage.Dnn.Publish.Util;

    using TreeNode = System.Windows.Forms.TreeNode;
    using TreeView = System.Windows.Forms.TreeView;

    /// <summary>
    /// Summary description for ItemRelationship.
    /// </summary>
    /// <remarks>This class should remain public, it is used by the Publish TreeView module</remarks>
    [XmlRoot(ElementName = "relationship", IsNullable = false)]
    public class ItemRelationship : TransportableElement
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _childItemId = -1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Guid _childItemIdentifier;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _childItemVersionId = -1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Guid _childItemVersionIdentifier;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _endDate;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _itemRelationshipId = -1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _itemTypeId = -1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _parentItemId = -1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Guid _parentItemIdentifier;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _relationshipTypeId = -1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _relationshipTypeName;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int? _sortOrder;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _startDate;

        public ItemRelationship()
        {
            this._startDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        }

        [XmlElement(Order = 2)]
        public int ChildItemId
        {
            [DebuggerStepThrough]
            get { return this._childItemId; }
            [DebuggerStepThrough]
            set { this._childItemId = value; }
        }

        [XmlElement(Order = 3)]
        public Guid ChildItemIdentifier
        {
            [DebuggerStepThrough]
            get { return this._childItemIdentifier; }
            [DebuggerStepThrough]
            set { this._childItemIdentifier = value; }
        }

        [XmlElement(Order = 4)]
        public int ChildItemVersionId
        {
            [DebuggerStepThrough]
            get { return this._childItemVersionId; }
            [DebuggerStepThrough]
            set { this._childItemVersionId = value; }
        }

        [XmlElement(Order = 5)]
        public Guid ChildItemVersionIdentifier
        {
            [DebuggerStepThrough]
            get { return this._childItemVersionIdentifier; }
            [DebuggerStepThrough]
            set { this._childItemVersionIdentifier = value; }
        }

        [XmlElement(Order = 11)]
        public string EndDate
        {
            [DebuggerStepThrough]
            get { return this._endDate; }
            [DebuggerStepThrough]
            set { this._endDate = Engage.Utility.HasValue(value) ? value : null; }
        }

        [XmlIgnore]
        public bool HasSortOrderBeenSet
        {
            get { return this._sortOrder.HasValue; }
        }

        [XmlElement(Order = 1)]
        public int ItemRelationshipId
        {
            [DebuggerStepThrough]
            get { return this._itemRelationshipId; }
            [DebuggerStepThrough]
            set { this._itemRelationshipId = value; }
        }

        [XmlIgnore]
        public int ItemTypeId
        {
            [DebuggerStepThrough]
            get { return this._itemTypeId; }
            [DebuggerStepThrough]
            set { this._itemTypeId = value; }
        }

        [XmlElement(Order = 6)]
        public int ParentItemId
        {
            [DebuggerStepThrough]
            get { return this._parentItemId; }
            [DebuggerStepThrough]
            set { this._parentItemId = value; }
        }

        [XmlElement(Order = 7)]
        public Guid ParentItemIdentifier
        {
            [DebuggerStepThrough]
            get { return this._parentItemIdentifier; }
            [DebuggerStepThrough]
            set { this._parentItemIdentifier = value; }
        }

        [XmlElement(Order = 8)]
        public int RelationshipTypeId
        {
            [DebuggerStepThrough]
            get { return this._relationshipTypeId; }
            [DebuggerStepThrough]
            set { this._relationshipTypeId = value; }
        }

        [XmlElement(Order = 9)]
        public string RelationshipTypeName
        {
            [DebuggerStepThrough]
            get { return this._relationshipTypeName; }
            [DebuggerStepThrough]
            set { this._relationshipTypeName = value; }
        }

        [XmlElement(Order = 12)]
        public int SortOrder
        {
            [DebuggerStepThrough]
            get { return this._sortOrder ?? 0; }
            [DebuggerStepThrough]
            set { this._sortOrder = value; }
        }

        [XmlElement(Order = 10)]
        public string StartDate
        {
            [DebuggerStepThrough]
            get { return this._startDate; }
            [DebuggerStepThrough]
            set { this._startDate = Engage.Utility.HasValue(value) ? value : null; }
        }

        public static void AddItemRelationship(
            int childItemId, int childItemVersionId, int parentItemId, int relationshipTypeId, string startDate, string endDate, int sortOrder)
        {
            DataProvider.Instance().AddItemRelationship(
                childItemId, childItemVersionId, parentItemId, relationshipTypeId, startDate, endDate, sortOrder);
        }

        public static void AddItemRelationship(
            IDbTransaction trans, 
            int childItemId, 
            int childItemVersionId, 
            int parentItemId, 
            int relationshipTypeId, 
            string startDate, 
            string endDate, 
            int sortOrder)
        {
            DataProvider.Instance().AddItemRelationship(
                trans, childItemId, childItemVersionId, parentItemId, relationshipTypeId, startDate, endDate, sortOrder);
        }

        public static void AddItemRelationshipWithOriginalSortOrder(
            IDbTransaction trans, 
            int childItemId, 
            int childItemVersionId, 
            int parentItemId, 
            int relationshipTypeId, 
            string startDate, 
            string endDate, 
            int originalItemVersionId)
        {
            DataProvider.Instance().AddItemRelationshipWithOriginalSortOrder(
                trans, childItemId, childItemVersionId, parentItemId, relationshipTypeId, startDate, endDate, originalItemVersionId);
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "TreeNode.#ctor(System.String)", 
            Justification = "Literal 'Root' is only used internally")]
        public static TreeNode BuildHierarchy(DataTable dt)
        {
            if (dt != null)
            {
                var nodes = new Dictionary<object, TreeNode>(dt.Rows.Count + 1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow r = dt.Rows[i];

                    object parentId = r["ParentItemId"];
                    object childId = r["ItemId"];
                    string text = r["Name"].ToString();

                    TreeNode parent;
                    if (!nodes.TryGetValue(parentId, out parent))
                    {
                        parent = new TreeNode
                            {
                                Tag = parentId
                            };
                        nodes.Add(parentId, parent);
                    }

                    TreeNode child;
                    if (!nodes.TryGetValue(childId, out child))
                    {
                        child = new TreeNode
                            {
                                Tag = childId
                            };
                        nodes.Add(childId, child);
                    }

                    child.Text = text;
                    parent.Nodes.Add(child);
                }

                var root = new TreeNode("Root");
                foreach (var node in nodes.Values)
                {
                    if (node.Parent == null || (string.IsNullOrEmpty(node.Parent.Text) && node.Parent.Tag == null))
                    {
                        root.Nodes.Add(node);
                    }
                }

                // Full path can only be retrieved when a TreeNode has been added to a TreeView
                using (var treeView = new TreeView())
                    treeView.Nodes.Add(root);

                return root;
            }

            return null;
        }

        [Obsolete(
            "There is a public constructor that initializes data and CBO.FillCollection uses reflection to create an instance of this class you must provide a public constructor. Just use new ItemRelationship()."
            , false)]
        public static ItemRelationship Create()
        {
            return new ItemRelationship();
        }

        /// <summary>
        /// Inserts a collection of <see cref="ListItem"/> into a <see cref="System.Web.UI.WebControls.ListControl"/>.  
        /// Each item has a value of the name of the category plus ticks to indicate hierarchy between categories.  
        /// Each item has a value of the category's itemId.
        /// If you have problems showing all Categories, you can try using <see cref="DisplayChildren(ListControl, int, int?)"/> on an instantiated ItemRelationship (see <see cref="Controls.ItemRelationships.UpdateAvailableItems"/> for an example).
        /// </summary>
        /// <param name="lc">The <see cref="System.Web.UI.WebControls.ListControl"/> into which the category options will be added.</param>
        /// <param name="categoryId">The id of the parent category from which the hierarchy will start, or -1 if all categories.</param>
        /// <param name="portalId">The id of the portal in which these categories reside.</param>
        /// <param name="includeParentCategory">if set to <c>true</c> includes the parent category in the list, otherwise only shows the parent's children.  This is ignored if the TopLevelCategory is selected.</param>
        public static void DisplayCategoryHierarchy(ListControl lc, int categoryId, int portalId, bool includeParentCategory)
        {
            DisplayCategoryHierarchy(lc, categoryId, portalId, includeParentCategory, -1);
        }

        /// <summary>
        /// Inserts a collection of <see cref="ListItem"/> into a <see cref="System.Web.UI.WebControls.ListControl"/>.
        /// Each item has a value of the name of the category plus ticks to indicate hierarchy between categories.
        /// Each item has a value of the category's itemId.
        /// If you have problems showing all Categories, you can try using <see cref="DisplayChildren(ListControl, int, int?)"/> on an instantiated ItemRelationship (see <see cref="Controls.ItemRelationships.UpdateAvailableItems"/> for an example).
        /// </summary>
        /// <param name="lc">The <see cref="System.Web.UI.WebControls.ListControl"/> into which the category options will be added.</param>
        /// <param name="categoryId">The id of the parent category from which the hierarchy will start, or -1 if all categories.</param>
        /// <param name="portalId">The id of the portal in which these categories reside.</param>
        /// <param name="includeParentCategory">if set to <c>true</c> includes the parent category in the list, otherwise only shows the parent's children.  This is ignored if the TopLevelCategory is selected.</param>
        /// <param name="itemToExclude">An item which you want to exclude from the list (including its children).  This is typically used to keep circular relationships from being possibe options.</param>
        public static void DisplayCategoryHierarchy(ListControl lc, int categoryId, int portalId, bool includeParentCategory, int itemToExclude)
        {
            DataTable dt;

            if (categoryId < 1)
            {
                dt = Category.GetCategoriesHierarchy(portalId);

                // we ignore includeParentCategory if it is the TopLevelCategory
            }
            else
            {
                dt = Item.GetAllChildren(ItemType.Category.GetId(), categoryId, RelationshipType.ItemToParentCategory.GetId(), portalId).Tables[0];

                if (includeParentCategory)
                {
                    Category parentCategory = Category.GetCategory(categoryId, portalId);
                    if (parentCategory != null)
                    {
                        DataRow parentRow = dt.NewRow();
                        parentRow["ParentItemId"] = "-1";
                        parentRow["ItemId"] = categoryId;
                        parentRow["Name"] = parentCategory.Name;
                        dt.Rows.InsertAt(parentRow, 0);
                    }
                }
            }

            TreeNode root = BuildHierarchy(dt);
            FillListControl(root, lc, itemToExclude);
        }

        public static DataSet GetAllChildren(int parentId, int relationshipTypeId, int portalId)
        {
            return DataProvider.Instance().GetAllChildren(parentId, relationshipTypeId, portalId);
        }

        public static TreeNode GetAllChildrenNLevels(int parentCategoryId, int nLevels, int mItems, int portalId)
        {
            DataTable dt = DataProvider.Instance().GetAllChildrenNLevels(parentCategoryId, nLevels, mItems, portalId);

            return BuildHierarchy(dt);
        }

        public static DataTable GetAllChildrenNLevelsInDataTable(int parentCategoryId, int nLevels, int mItems, int portalId)
        {
            return DataProvider.Instance().GetAllChildrenNLevels(parentCategoryId, nLevels, mItems, portalId);
        }

        public static List<ItemRelationship> GetAllRelationships(int moduleId)
        {
            return CBO.FillCollection<ItemRelationship>(DataProvider.Instance().GetAllRelationships(moduleId));
        }

        public static List<ItemRelationship> GetAllRelationshipsByPortalId(int portalId)
        {
            return CBO.FillCollection<ItemRelationship>(DataProvider.Instance().GetAllRelationshipsByPortalId(portalId));
        }

        public static ArrayList GetItemChildRelationships(int parentItemId, int relationshipTypeId)
        {
            return CBO.FillCollection(DataProvider.Instance().GetItemChildRelationships(parentItemId, relationshipTypeId), typeof(ItemRelationship));
        }

        public static ArrayList GetItemChildRelationships(int parentItemId, int relationshipTypeId, int portalId)
        {
            string cacheKey = Utility.CacheKeyPublishChildItemRelationships + parentItemId.ToString(CultureInfo.InvariantCulture) + "_" +
                              relationshipTypeId.ToString(CultureInfo.InvariantCulture);
            ArrayList al;

            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    al = (ArrayList)o;
                }
                else
                {
                    al = CBO.FillCollection(
                        DataProvider.Instance().GetItemChildRelationships(parentItemId, relationshipTypeId), typeof(ItemRelationship));
                    DataCache.SetCache(cacheKey, al, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                al = CBO.FillCollection(DataProvider.Instance().GetItemChildRelationships(parentItemId, relationshipTypeId), typeof(ItemRelationship));
            }

            return al;
        }

        public static DataSet GetItemRelationshipByItemRelationshipId(int itemRelationshipId)
        {
            return DataProvider.Instance().GetItemRelationshipByItemRelationshipId(itemRelationshipId);
        }

        public static List<ItemRelationship> GetItemRelationships(int childItemId, int childItemVersionId, bool isActive)
        {
            return CBO.FillCollection<ItemRelationship>(DataProvider.Instance().GetItemRelationships(childItemId, childItemVersionId, isActive));
        }

        public static ArrayList GetItemRelationships(int childItemId, int childItemVersionId, int relationshipTypeId, bool isActive)
        {
            return CBO.FillCollection(
                DataProvider.Instance().GetItemRelationships(childItemId, childItemVersionId, relationshipTypeId, isActive), typeof(ItemRelationship));
        }

        public static ArrayList GetItemRelationships(int childItemId, int childItemVersionId, int relationshipTypeId, bool isActive, int portalId)
        {
            string cacheKey = Utility.CacheKeyPublishItemRelationships + childItemId.ToString(CultureInfo.InvariantCulture) + "_" +
                              childItemVersionId.ToString(CultureInfo.InvariantCulture) + "_" +
                              relationshipTypeId.ToString(CultureInfo.InvariantCulture);
            ArrayList al;

            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    al = (ArrayList)o;
                }
                else
                {
                    al =
                        CBO.FillCollection(
                            DataProvider.Instance().GetItemRelationships(childItemId, childItemVersionId, relationshipTypeId, isActive), 
                            typeof(ItemRelationship));
                    DataCache.SetCache(cacheKey, al, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                al = CBO.FillCollection(
                    DataProvider.Instance().GetItemRelationships(childItemId, childItemVersionId, relationshipTypeId, isActive), 
                    typeof(ItemRelationship));
            }

            return al;
        }

        public static void UpdateItemRelationship(int itemRelationshipId, int sortOrder)
        {
            DataProvider.Instance().UpdateItemRelationship(itemRelationshipId, sortOrder);
        }

        public void CorrectDates()
        {
            if (!string.IsNullOrEmpty(this.StartDate))
            {
                this.StartDate = Convert.ToDateTime(this.StartDate, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(this.EndDate))
            {
                this.EndDate = Convert.ToDateTime(this.EndDate, CultureInfo.CurrentCulture).ToString(CultureInfo.InvariantCulture);
            }
        }

        public void DisplayChildren(ListControl lc, int portalId)
        {
            this.DisplayChildren(lc, portalId, null);
        }

        public void DisplayChildren(ListControl lc, int portalId, int? itemToExclude)
        {
            DataTable dt;
            DataSet ds;
            if (this._parentItemId < 1)
            {
                dt = Category.GetCategoriesHierarchy(portalId);
            }
            else
            {
                ds = Item.GetAllChildren(this._parentItemId, this._relationshipTypeId, portalId);
                dt = ds.Tables[0];
            }

            TreeNode root = BuildHierarchy(dt);
            FillListControl(root, lc, itemToExclude);
        }

        // 		public DataTable GetChildren(int childTypeId, int maxItems, int portalId)
        // 		{
        // 			StringBuilder sql = new StringBuilder();
        // 			sql.Append("engageams_spItemListing ");
        // 			sql.Append(childTypeId);
        // 			sql.Append(", ");
        // 			sql.Append(portalId);
        // 			DataTable dt = DataProvider.Instance().GetDataTable(sql.ToString(), portalId);
        // 			SecurityFilter sf = SecurityFilter.Instance;
        // 			sf.FilterCategories(dt);
        // 			//remove rows over the limit
        // 			if (maxItems != -1)
        // 			{
        // 				ArrayList al = new ArrayList();
        // 				for (int i = maxItems; i < dt.Rows.Count; i++)
        // 				{
        // 					al.Add(dt.Rows[i]);
        // 				}
        // 				foreach (DataRow r in al)
        // 				{
        // 					data.Rows.Remove(r);
        // 				}
        // 			}
        // 			return dt;
        // 		}

        /// <summary>
        /// This method is invoked by the Import mechanism and has to take this instance of a ItemRelationship and resolve
        /// all the id's using the names supplied in the export. hk
        /// </summary>
        public override void Import(int currentModuleId, int portalId)
        {
            try
            {
                RelationshipType type = RelationshipType.GetFromName(this.RelationshipTypeName, typeof(RelationshipType));
                this._relationshipTypeId = type.GetId();

                // Does this exist in my db?
                using (
                    IDataReader dr = DataProvider.Instance().GetItemRelationshipByIdentifiers(
                        this.ParentItemIdentifier, this.ChildItemVersionIdentifier, portalId))
                {
                    if (dr.Read())
                    {
                        // this version does not exist.
                        if (dr["ParentItemId"] is DBNull || dr["ChildItemId"] is DBNull)
                        {
                            // no matching parent or child doesn't exist. Could throw error and stop
                            // but for now we will handle gracefully.
                        }
                        else
                        {
                            this._childItemId = (int)dr["ChildItemId"];
                            this._childItemVersionId = (int)dr["ChildItemVersionId"];
                            this._parentItemId = (int)dr["ParentItemId"];
                            AddItemRelationship(
                                this.ChildItemId, 
                                this.ChildItemVersionId, 
                                this.ParentItemId, 
                                this.RelationshipTypeId, 
                                this.StartDate, 
                                this.EndDate, 
                                this.SortOrder);
                        }
                    }
                    else
                    {
                        Exceptions.LogException(new Exception("No matching Parent or Child could be found to create relationship."));
                    }
                }
            }
            catch (Exception e)
            {
                Exceptions.LogException(e);
                throw;
            }
        }

        private static void FillListControl(TreeNode root, ListControl lc, int? itemToExclude)
        {
            for (int i = 0; i < root.Nodes.Count; i++)
            {
                TreeNode child = root.Nodes[i];
                if (child.Text.Length > 0)
                {
                    // int level = child.FullPath.Split('\\').Length;
                    int level = child.FullPath.Split('\\').Length;

                    // ListItem li = new ListItem(pad + pad + child.Text, child.Tag.ToString());
                    int itemId = Convert.ToInt32(child.Tag, CultureInfo.InvariantCulture);

                    if (itemToExclude.HasValue && itemId == itemToExclude.Value)
                    {
                        // skip this item and its children
                        continue;
                    }

                    int paddingWidth = (level - 3) * 2;
                    var padding = new string('-', paddingWidth);
                    var li = new ListItem(padding + child.Text, itemId.ToString(CultureInfo.InvariantCulture));
                    lc.Items.Add(li);
                }

                FillListControl(child, lc, itemToExclude);
            }
        }
    }
}