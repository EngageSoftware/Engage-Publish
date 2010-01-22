//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
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
    using System.Globalization;
    using System.Web.UI.WebControls;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Services.Exceptions;
    using AjaxControlToolkit;


    [Serializable]
    public class ItemRelationshipSort : IEquatable<ItemRelationshipSort>
    {
        private int _itemRelationshipId = -1;
        public int ItemRelationshipId
        {
            get { return _itemRelationshipId; }
            set { _itemRelationshipId = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _sortOrder;
        public int SortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

        #region IEquatable<ItemRelationshipSort> Members

        public bool Equals(ItemRelationshipSort other)
        {
            if (other != null)
            {
                return ItemRelationshipId == other.ItemRelationshipId;
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || Equals(obj as ItemRelationshipSort);
        }

        public override int GetHashCode()
        {
            return _itemRelationshipId;
        }

        #endregion
    }

    public partial class CategorySort : ModuleBase
    {
        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Load += Page_Load;
        }

        #endregion

        private bool _windowClose;

        const string SortList = "SortList";
        const string UnSortedList = "UnSortedList";

        private List<ItemRelationshipSort> _sortItems = new List<ItemRelationshipSort>();
        private List<ItemRelationshipSort> _unsortedItems = new List<ItemRelationshipSort>();

        #region Event Handlers

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                object o = Request.Params["windowclose"];
                if (o != null)
                {
                    _windowClose = Convert.ToBoolean(o.ToString(), CultureInfo.InvariantCulture);
                }
                if (!Page.IsPostBack)
                {
                    Category c = Category.GetCategory(CategoryId, PortalId);
                    if (c != null)
                    {
                        lblCategory.Text = String.Format(CultureInfo.CurrentCulture, Localization.GetString("lblCategory", LocalResourceFile),c.Name);

                        Session[SortList] = _sortItems;
                        Session[UnSortedList] = _unsortedItems;
                        BindData();
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion


        #region Optional Interfaces

        public ModuleActionCollection ModuleActions
        {
            get
            {
                return new ModuleActionCollection
                           {
                                   {
                                           GetNextActionID(),
                                           Localization.GetString(
                                           ModuleActionType.AddContent, LocalResourceFile),
                                           DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "",
                                           "", "", false, DotNetNuke.Security.SecurityAccessLevel.Edit, true,
                                           false
                                           }
                           };
            }
        }


        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.Web.UI.ITextControl.set_Text(System.String)", Justification = "Literal is HTML"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.Web.UI.WebControls.TableCell.set_Text(System.String)", Justification = "Literal is HTML")]
        private void BindData()
        {
            //get all child items for a category


            DataTable dt = Category.GetChildrenInCategoryPaging(CategoryId, -1, 10000, PortalId, true, true, " childname asc ", 0, 10000);

            Util.Utility.SortDataTableSingleParam(dt, "childname asc");

            DataTable unSorted = dt.Copy();
            DataTable sorted = dt.Copy();

            var sortedRows = new List<DataRow>();
            var unsortedRows = new List<DataRow>();

            //setup list of sorted rows
            foreach (DataRow dr in unSorted.Rows)
            {
                int sortOrder;
                if (int.TryParse(dr["SortOrder"].ToString(), out sortOrder) && sortOrder > 0)
                {
                    sortedRows.Add(dr);
                }
            }

            //setup list of unsorted rows
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

            Util.Utility.SortDataTableSingleParam(sorted,"sortorder asc");

            lbCategoryItems.DataSource = unSorted;
            lbCategoryItems.DataBind();

            //get the already sorted items for a category
            //dt = Session[DT_SortTable] as DataTable;

            _sortItems = FilterSortedItems(sorted);

            rlCategorySort.DataSource = _sortItems;
            rlCategorySort.DataBind();

            Session[SortList] = _sortItems;
            
        }

        private List<ItemRelationshipSort> FilterSortedItems(DataTable dt)
        {
            _sortItems = new List<ItemRelationshipSort>();
            foreach(DataRow dr in dt.Rows)
            {                
                int sortOrder;
                if ((Int32.TryParse(dr["SortOrder"].ToString(), out sortOrder) && sortOrder > 0))
                {
                    var ir = new ItemRelationshipSort
                                 {
                                         ItemRelationshipId =
                                                 Convert.ToInt32(dr["ItemRelationshipId"], CultureInfo.InvariantCulture),
                                         SortOrder = sortOrder,
                                         Name = dr["ChildName"].ToString()
                                 };
                    if (_sortItems.Count > ir.SortOrder)
                    {
                        _sortItems.Insert(ir.SortOrder, ir);
                    }
                    else
                    {
                        _sortItems.Add(ir);
                    }
                }
            }
            Session[SortList] = _sortItems;
            return _sortItems;
        }


        private int CategoryId
        {
            get
            {
                string s = Request.QueryString["itemid"];
                return (s == null ? -1 : Convert.ToInt32(s, CultureInfo.InvariantCulture));
            }
        }

        protected void lbMoveToSort_Click(object sender, EventArgs e)
        {

            //ReorderDataTable();

            _sortItems = Session[SortList] as List<ItemRelationshipSort>;
            _unsortedItems = Session[UnSortedList] as List<ItemRelationshipSort>;

            DataTable selectedRelationship = ItemRelationship.GetItemRelationshipByItemRelationshipId(Convert.ToInt32(lbCategoryItems.SelectedValue, CultureInfo.InvariantCulture)).Tables[0];

            foreach (DataRow dr in selectedRelationship.Rows)
            {
                var irs = new ItemRelationshipSort
                              {
                                      ItemRelationshipId =
                                              Convert.ToInt32(dr["ItemRelationshipId"], CultureInfo.InvariantCulture),
                                      SortOrder = Convert.ToInt32(dr["SortOrder"], CultureInfo.InvariantCulture),
                                      Name = dr["Name"].ToString()
                              };

                //If we've previously removed this from the _sortItems list we need to remove it from our session list of UnSorted items

                if (_unsortedItems != null)
                    if(_unsortedItems.Contains(irs))
                    {
                        _unsortedItems.Remove(irs);
                    }
                if (_sortItems != null) _sortItems.Add(irs);
                //remove item from original list
                
            }
            
            ListItem li = lbCategoryItems.SelectedItem;
            lbCategoryItems.Items.Remove(li);           
            
            //Util.Utility.SortDataTableSingleParam(dt, "SortOrder ASC");

            //get the already sorted items for a category
            rlCategorySort.DataSource = _sortItems;
            rlCategorySort.DataBind();


            Session[UnSortedList] = _unsortedItems;

            Session[SortList] = _sortItems;
            
        }

        //protected void rlCategorySort_Reorder(object sender, EventArgs e)
        protected void rlCategorySort_Reorder(object sender, ReorderListItemReorderEventArgs e)
        {

            _sortItems = Session[SortList] as List<ItemRelationshipSort>;

            if (_sortItems != null)
            {
                ItemRelationshipSort irs = _sortItems[e.OldIndex];
                _sortItems.Remove(irs);
                _sortItems.Insert(e.NewIndex, irs);
            }

            rlCategorySort.DataSource = _sortItems;
            rlCategorySort.DataBind();

            Session[SortList] = _sortItems;

        }


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
            
            _sortItems = Session[SortList] as List<ItemRelationshipSort>;
            _unsortedItems = Session[UnSortedList] as List<ItemRelationshipSort>;
            if (_sortItems != null)
                foreach (ItemRelationshipSort irs in _sortItems)
                {
                    //int itemRelationshipId, sortorder = 0;
                    ItemRelationship.UpdateItemRelationship(irs.ItemRelationshipId, _sortItems.IndexOf(irs)+1);
                }

            //modify the sort order of any items we've removed from the SortList
            if (_unsortedItems != null)
                foreach (ItemRelationshipSort irs in _unsortedItems)
                {
                    //int itemRelationshipId, sortorder = 0;
                    ItemRelationship.UpdateItemRelationship(irs.ItemRelationshipId, 0);
                }

            Session.Remove(SortList);
            Session.Remove(UnSortedList);
            
            lblMessage.Visible = true;
            lblMessage.Text = Localization.GetString("SaveSuccess", LocalResourceFile);
            pnlSortList.Visible = false;
            if (_windowClose)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "close window", "window.close();", true);
            }
        }

        protected void lbCancel_Click(object sender, EventArgs e)
        {
            if (_windowClose)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "close window", "window.close();", true);
                //Response.Write("<script language='JavaScript'>window.close();</script>");
            }
            else
            {
                Response.Redirect(BuildLinkUrl("&amp;mid=" + ModuleId + "&amp;ctl=admincontainer&amp;adminType=categorylist"), true);
            }
        }
        
        protected void DeleteItem(object sender, ReorderListCommandEventArgs e)
        {
            //remove from the sorted list, add to the removeFromSorted list

            _sortItems = Session[SortList] as List<ItemRelationshipSort>;
            _unsortedItems = Session[UnSortedList] as List<ItemRelationshipSort>;
            if (_sortItems != null)
            {
                foreach (ItemRelationshipSort irs in _sortItems)
                {
                    if (irs.ItemRelationshipId == Convert.ToInt32(e.CommandArgument, CultureInfo.InvariantCulture))
                    {
                        _sortItems.Remove(irs);
                        if (_unsortedItems != null) _unsortedItems.Add(irs);

                        var li = new ListItem(irs.Name, irs.ItemRelationshipId.ToString(CultureInfo.InvariantCulture));
                        lbCategoryItems.Items.Add(li);
                        break;
                    }
                }
                rlCategorySort.DataSource = _sortItems;
            }
            rlCategorySort.DataBind();

            Session[SortList] = _sortItems;
            Session[UnSortedList] = _unsortedItems;
            //e.CommandArgument
            //add item to UnSortedList
        }
    }
}