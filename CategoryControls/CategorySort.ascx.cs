//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2009
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
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using AjaxControlToolkit;

namespace Engage.Dnn.Publish.CategoryControls
{
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
            get { return this._name; }
            set { this._name = value; }
        }

        private int _sortOrder;
        public int SortOrder
        {
            get { return this._sortOrder; }
            set { this._sortOrder = value; }
        }

        #region IEquatable<ItemRelationshipSort> Members

        public bool Equals(ItemRelationshipSort other)
        {
            if (other != null)
            {
                return this.ItemRelationshipId == other.ItemRelationshipId;
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
            this.Load += this.Page_Load;
        }

        #endregion

        private bool windowClose;

        const string SortList = "SortList";
        const string UnSortedList = "UnSortedList";

        private List<ItemRelationshipSort> SortItems = new List<ItemRelationshipSort>();
        private List<ItemRelationshipSort> UnsortedItems = new List<ItemRelationshipSort>();

        #region Event Handlers

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                object o = Request.Params["windowclose"];
                if (o != null)
                {
                    windowClose = Convert.ToBoolean(o.ToString(), CultureInfo.InvariantCulture);
                }
                if (!Page.IsPostBack)
                {
                    Category c = Category.GetCategory(CategoryId, PortalId);
                    if (c != null)
                    {
                        lblCategory.Text = String.Format(CultureInfo.CurrentCulture, Localization.GetString("lblCategory", LocalResourceFile),c.Name);

                        Session[SortList] = SortItems;
                        Session[UnSortedList] = UnsortedItems;
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
                ModuleActionCollection actions = new ModuleActionCollection();
                actions.Add(GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, LocalResourceFile), DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", "", false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
                return actions;
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

            List<DataRow> sortedRows = new List<DataRow>();
            List<DataRow> unsortedRows = new List<DataRow>();

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

            SortItems = FilterSortedItems(sorted);

            rlCategorySort.DataSource = SortItems;
            rlCategorySort.DataBind();

            Session[SortList] = SortItems;
            
        }

        private List<ItemRelationshipSort> FilterSortedItems(DataTable dt)
        {
            SortItems = new List<ItemRelationshipSort>();
            foreach(DataRow dr in dt.Rows)
            {                
                int sortOrder;
                if ((Int32.TryParse(dr["SortOrder"].ToString(), out sortOrder) && sortOrder > 0))
                {
                    ItemRelationshipSort ir = new ItemRelationshipSort();
                    ir.ItemRelationshipId = Convert.ToInt32(dr["ItemRelationshipId"], CultureInfo.InvariantCulture);
                    ir.SortOrder = sortOrder;//Convert.ToInt32(dr["SortOrder"], CultureInfo.InvariantCulture);
                    ir.Name = dr["ChildName"].ToString();
                    if (SortItems.Count > ir.SortOrder)
                    {
                        SortItems.Insert(ir.SortOrder, ir);
                    }
                    else
                    {
                        SortItems.Add(ir);
                    }
                }
            }
            Session[SortList] = SortItems;
            return SortItems;
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

            SortItems = Session[SortList] as List<ItemRelationshipSort>;
            UnsortedItems = Session[UnSortedList] as List<ItemRelationshipSort>;

            DataTable selectedRelationship = ItemRelationship.GetItemRelationshipByItemRelationshipId(Convert.ToInt32(lbCategoryItems.SelectedValue, CultureInfo.InvariantCulture)).Tables[0];

            foreach (DataRow dr in selectedRelationship.Rows)
            {
                ItemRelationshipSort irs = new ItemRelationshipSort();
                irs.ItemRelationshipId = Convert.ToInt32(dr["ItemRelationshipId"], CultureInfo.InvariantCulture);
                irs.SortOrder = Convert.ToInt32(dr["SortOrder"], CultureInfo.InvariantCulture);
                irs.Name = dr["Name"].ToString();

                //If we've previously removed this from the SortItems list we need to remove it from our session list of UnSorted items
                
                if(UnsortedItems.Contains(irs))
                {
                    UnsortedItems.Remove(irs);
                }
                SortItems.Add(irs);
                //remove item from original list
                
            }
            
            ListItem li = lbCategoryItems.SelectedItem;
            lbCategoryItems.Items.Remove(li);           
            
            //Util.Utility.SortDataTableSingleParam(dt, "SortOrder ASC");

            //get the already sorted items for a category
            rlCategorySort.DataSource = SortItems;
            rlCategorySort.DataBind();


            Session[UnSortedList] = UnsortedItems;

            Session[SortList] = SortItems;
            
        }

        //protected void rlCategorySort_Reorder(object sender, EventArgs e)
        protected void rlCategorySort_Reorder(object sender, ReorderListItemReorderEventArgs e)
        {

            SortItems = Session[SortList] as List<ItemRelationshipSort>;

            ItemRelationshipSort irs = SortItems[e.OldIndex];
            SortItems.Remove(irs);
            SortItems.Insert(e.NewIndex, irs);
            
            rlCategorySort.DataSource = SortItems;
            rlCategorySort.DataBind();

            Session[SortList] = SortItems;

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
            
            SortItems = Session[SortList] as List<ItemRelationshipSort>;
            UnsortedItems = Session[UnSortedList] as List<ItemRelationshipSort>;
            foreach (ItemRelationshipSort irs in SortItems)
            {
                //int itemRelationshipId, sortorder = 0;
                ItemRelationship.UpdateItemRelationship(irs.ItemRelationshipId, SortItems.IndexOf(irs)+1);
            }
            
            //modify the sort order of any items we've removed from the SortList
            foreach (ItemRelationshipSort irs in UnsortedItems)
            {
                //int itemRelationshipId, sortorder = 0;
                ItemRelationship.UpdateItemRelationship(irs.ItemRelationshipId, 0);
            }
            
            Session.Remove(SortList);
            Session.Remove(UnSortedList);
            
            lblMessage.Visible = true;
            lblMessage.Text = Localization.GetString("SaveSuccess", LocalResourceFile);
            pnlSortList.Visible = false;
            if (windowClose)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "close window", "window.close();", true);
            }
        }

        protected void lbCancel_Click(object sender, EventArgs e)
        {
            if (windowClose)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "close window", "window.close();", true);
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

            SortItems = Session[SortList] as List<ItemRelationshipSort>;
            UnsortedItems = Session[UnSortedList] as List<ItemRelationshipSort>;
            foreach (ItemRelationshipSort irs in SortItems)
            {
                if (irs.ItemRelationshipId == Convert.ToInt32(e.CommandArgument, CultureInfo.InvariantCulture))
                {
                    SortItems.Remove(irs);                    
                    UnsortedItems.Add(irs);

                    ListItem li = new ListItem(irs.Name, irs.ItemRelationshipId.ToString(CultureInfo.InvariantCulture));
                    lbCategoryItems.Items.Add(li);
                    break;
                }
            }

            rlCategorySort.DataSource = SortItems;
            rlCategorySort.DataBind();

            Session[SortList] = SortItems;
            Session[UnSortedList] = UnsortedItems;
            //e.CommandArgument
            //add item to UnSortedList
        }
    }
}