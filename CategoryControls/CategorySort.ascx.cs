//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.CategoryControls
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.UI.WebControls;

    using AjaxControlToolkit;

    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Utility = Engage.Dnn.Publish.Util.Utility;

    [Serializable]
    public class ItemRelationshipSort : IEquatable<ItemRelationshipSort>
    {
        private int _itemRelationshipId = -1;

        private string _name;

        private int _sortOrder;

        public int ItemRelationshipId
        {
            get { return this._itemRelationshipId; }
            set { this._itemRelationshipId = value; }
        }

        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        public int SortOrder
        {
            get { return this._sortOrder; }
            set { this._sortOrder = value; }
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || this.Equals(obj as ItemRelationshipSort);
        }

        public override int GetHashCode()
        {
            return this._itemRelationshipId;
        }

        public bool Equals(ItemRelationshipSort other)
        {
            if (other != null)
            {
                return this.ItemRelationshipId == other.ItemRelationshipId;
            }

            return false;
        }
    }

    public partial class CategorySort : ModuleBase
    {
        private const string SortList = "SortList";

        private const string UnSortedList = "UnSortedList";

        private List<ItemRelationshipSort> _sortItems = new List<ItemRelationshipSort>();

        private List<ItemRelationshipSort> _unsortedItems = new List<ItemRelationshipSort>();

        private bool _windowClose;

        public ModuleActionCollection ModuleActions
        {
            get
            {
                return new ModuleActionCollection
                    {
                        {
                            this.GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile), 
                            ModuleActionType.AddContent, string.Empty, string.Empty, string.Empty, false, SecurityAccessLevel.Edit, true, false
                            }
                    };
            }
        }

        private int CategoryId
        {
            get
            {
                string s = this.Request.QueryString["itemid"];
                return s == null ? -1 : Convert.ToInt32(s, CultureInfo.InvariantCulture);
            }
        }

        protected void DeleteItem(object sender, ReorderListCommandEventArgs e)
        {
            // remove from the sorted list, add to the removeFromSorted list
            this._sortItems = this.Session[SortList] as List<ItemRelationshipSort>;
            this._unsortedItems = this.Session[UnSortedList] as List<ItemRelationshipSort>;
            if (this._sortItems != null)
            {
                foreach (ItemRelationshipSort irs in this._sortItems)
                {
                    if (irs.ItemRelationshipId == Convert.ToInt32(e.CommandArgument, CultureInfo.InvariantCulture))
                    {
                        this._sortItems.Remove(irs);
                        if (this._unsortedItems != null)
                        {
                            this._unsortedItems.Add(irs);
                        }

                        var li = new ListItem(irs.Name, irs.ItemRelationshipId.ToString(CultureInfo.InvariantCulture));
                        this.lbCategoryItems.Items.Add(li);
                        break;
                    }
                }

                this.rlCategorySort.DataSource = this._sortItems;
            }

            this.rlCategorySort.DataBind();

            this.Session[SortList] = this._sortItems;
            this.Session[UnSortedList] = this._unsortedItems;

            // e.CommandArgument
            // add item to UnSortedList
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        protected void lbCancel_Click(object sender, EventArgs e)
        {
            if (this._windowClose)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "close window", "window.close();", true);

                // Response.Write("<script language='JavaScript'>window.close();</script>");
            }
            else
            {
                this.Response.Redirect(this.BuildLinkUrl("&amp;mid=" + this.ModuleId + "&amp;ctl=admincontainer&amp;adminType=categorylist"), true);
            }
        }

        protected void lbMoveToSort_Click(object sender, EventArgs e)
        {
            // ReorderDataTable();
            this._sortItems = this.Session[SortList] as List<ItemRelationshipSort>;
            this._unsortedItems = this.Session[UnSortedList] as List<ItemRelationshipSort>;

            DataTable selectedRelationship =
                ItemRelationship.GetItemRelationshipByItemRelationshipId(
                    Convert.ToInt32(this.lbCategoryItems.SelectedValue, CultureInfo.InvariantCulture)).Tables[0];

            foreach (DataRow dr in selectedRelationship.Rows)
            {
                var irs = new ItemRelationshipSort
                    {
                        ItemRelationshipId = Convert.ToInt32(dr["ItemRelationshipId"], CultureInfo.InvariantCulture), 
                        SortOrder = Convert.ToInt32(dr["SortOrder"], CultureInfo.InvariantCulture), 
                        Name = dr["Name"].ToString()
                    };

                // If we've previously removed this from the _sortItems list we need to remove it from our session list of UnSorted items
                if (this._unsortedItems != null)
                {
                    if (this._unsortedItems.Contains(irs))
                    {
                        this._unsortedItems.Remove(irs);
                    }
                }

                if (this._sortItems != null)
                {
                    this._sortItems.Add(irs);
                }

                // remove item from original list
            }

            ListItem li = this.lbCategoryItems.SelectedItem;
            this.lbCategoryItems.Items.Remove(li);

            // Util.Utility.SortDataTableSingleParam(dt, "SortOrder ASC");

            // get the already sorted items for a category
            this.rlCategorySort.DataSource = this._sortItems;
            this.rlCategorySort.DataBind();

            this.Session[UnSortedList] = this._unsortedItems;

            this.Session[SortList] = this._sortItems;
        }

        // protected void rlCategorySort_Reorder(object sender, EventArgs e)

        /*
        private void UpdateDataTable(DataTable dt)
        {
            DataTable newTable = dt.Clone();

            foreach (ReorderListItem rli in rlCategorySort.Items)
            {
                Label lblRelationshipId = (Label)rli.FindControl("lblItemRelationshipId");
                int itemrelationshipid = Convert.ToInt32(lblRelationshipId.Text);
                foreach (DataRow dr in dt.Rows)
                {
                    if (Convert.ToInt32(dr["ItemRelationshipId"]) == itemrelationshipid)
                    {
                        dr["SortOrder"] = rli.ItemIndex;
                    }
                }
            }

        }
*/

        protected void lbSaveSort_Click(object sender, EventArgs e)
        {
            this._sortItems = this.Session[SortList] as List<ItemRelationshipSort>;
            this._unsortedItems = this.Session[UnSortedList] as List<ItemRelationshipSort>;
            if (this._sortItems != null)
            {
                foreach (ItemRelationshipSort irs in this._sortItems)
                {
                    // int itemRelationshipId, sortorder = 0;
                    ItemRelationship.UpdateItemRelationship(irs.ItemRelationshipId, this._sortItems.IndexOf(irs) + 1);
                }
            }

            // modify the sort order of any items we've removed from the SortList
            if (this._unsortedItems != null)
            {
                foreach (ItemRelationshipSort irs in this._unsortedItems)
                {
                    // int itemRelationshipId, sortorder = 0;
                    ItemRelationship.UpdateItemRelationship(irs.ItemRelationshipId, 0);
                }
            }

            this.Session.Remove(SortList);
            this.Session.Remove(UnSortedList);
            this.lblMessage.Visible = true;
            this.lblMessage.Text = Localization.GetString("SaveSuccess", this.LocalResourceFile);
            this.pnlSortList.Visible = false;
            if (this._windowClose)
            {
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "close window", "window.close();", true);
            }
        }

        protected void rlCategorySort_Reorder(object sender, ReorderListItemReorderEventArgs e)
        {
            this._sortItems = this.Session[SortList] as List<ItemRelationshipSort>;

            if (this._sortItems != null)
            {
                ItemRelationshipSort irs = this._sortItems[e.OldIndex];
                this._sortItems.Remove(irs);
                this._sortItems.Insert(e.NewIndex, irs);
            }

            this.rlCategorySort.DataSource = this._sortItems;
            this.rlCategorySort.DataBind();

            this.Session[SortList] = this._sortItems;
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", 
            MessageId = "System.Web.UI.ITextControl.set_Text(System.String)", Justification = "Literal is HTML")]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", 
            MessageId = "System.Web.UI.WebControls.TableCell.set_Text(System.String)", Justification = "Literal is HTML")]
        private void BindData()
        {
            // get all child items for a category
            DataTable dt = Category.GetChildrenInCategoryPaging(this.CategoryId, -1, 10000, this.PortalId, true, true, " childname asc ", 0, 10000);

            Utility.SortDataTableSingleParam(dt, "childname asc");

            DataTable unSorted = dt.Copy();
            DataTable sorted = dt.Copy();

            var sortedRows = new List<DataRow>();
            var unsortedRows = new List<DataRow>();

            // setup list of sorted rows
            foreach (DataRow dr in unSorted.Rows)
            {
                int sortOrder;
                if (int.TryParse(dr["SortOrder"].ToString(), out sortOrder) && sortOrder > 0)
                {
                    sortedRows.Add(dr);
                }
            }

            // setup list of unsorted rows
            foreach (DataRow dr in sorted.Rows)
            {
                int sortOrder;
                if (!int.TryParse(dr["SortOrder"].ToString(), out sortOrder) || sortOrder == 0)
                {
                    unsortedRows.Add(dr);
                }
            }

            foreach (DataRow dr in sortedRows)
            {
                unSorted.Rows.Remove(dr);
            }

            foreach (DataRow dr in unsortedRows)
            {
                sorted.Rows.Remove(dr);
            }

            Utility.SortDataTableSingleParam(sorted, "sortorder asc");

            this.lbCategoryItems.DataSource = unSorted;
            this.lbCategoryItems.DataBind();

            // get the already sorted items for a category
            // dt = Session[DT_SortTable] as DataTable;
            this._sortItems = this.FilterSortedItems(sorted);

            this.rlCategorySort.DataSource = this._sortItems;
            this.rlCategorySort.DataBind();

            this.Session[SortList] = this._sortItems;
        }

        private List<ItemRelationshipSort> FilterSortedItems(DataTable dt)
        {
            this._sortItems = new List<ItemRelationshipSort>();
            foreach (DataRow dr in dt.Rows)
            {
                int sortOrder;
                if (Int32.TryParse(dr["SortOrder"].ToString(), out sortOrder) && sortOrder > 0)
                {
                    var ir = new ItemRelationshipSort
                        {
                            ItemRelationshipId = Convert.ToInt32(dr["ItemRelationshipId"], CultureInfo.InvariantCulture), 
                            SortOrder = sortOrder, 
                            Name = dr["ChildName"].ToString()
                        };
                    if (this._sortItems.Count > ir.SortOrder)
                    {
                        this._sortItems.Insert(ir.SortOrder, ir);
                    }
                    else
                    {
                        this._sortItems.Add(ir);
                    }
                }
            }

            this.Session[SortList] = this._sortItems;
            return this._sortItems;
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Load += Page_Load;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                object o = this.Request.Params["windowclose"];
                if (o != null)
                {
                    this._windowClose = Convert.ToBoolean(o.ToString(), CultureInfo.InvariantCulture);
                }

                if (!this.Page.IsPostBack)
                {
                    Category c = Category.GetCategory(this.CategoryId, this.PortalId);
                    if (c != null)
                    {
                        this.lblCategory.Text = string.Format(
                            CultureInfo.CurrentCulture, Localization.GetString("lblCategory", this.LocalResourceFile), c.Name);

                        this.Session[SortList] = this._sortItems;
                        this.Session[UnSortedList] = this._unsortedItems;
                        this.BindData();
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}