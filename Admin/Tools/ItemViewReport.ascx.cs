//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using DotNetNuke.Services.Localization;
using System.Globalization;
using DotNetNuke.Services.Exceptions;
using System.Data;
using System.Web.UI.WebControls;
using Engage.Dnn.Publish.Data;
using System.ComponentModel;

//TODO: this page doesn't work yet. Fix the objectdatasource
namespace Engage.Dnn.Publish.Admin.Tools
{
    public partial class ItemViewReport : ModuleBase
    {
        #region Event Handlers

        override protected void OnInit(EventArgs e)
        {
            Load += Page_Load;
            base.OnInit(e);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    lblPortalId.Text = PortalId.ToString(CultureInfo.CurrentCulture);

                    gvReport.PagerSettings.FirstPageText = Localization.GetString("FirstPageText", LocalResourceFile);
                    gvReport.PagerSettings.LastPageText = Localization.GetString("LastPageText", LocalResourceFile);


                    LoadItemTypes();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        public void LoadItemTypes()
        {
            //load the item types dropdown
            // load "ALL"
            // load "Articles"
            // load "Categories"
            ddlItemType.DataTextField = "Name";
            ddlItemType.DataValueField = "ItemTypeId";
            ddlItemType.DataSource = Item.GetItemTypes(PortalId);
            ddlItemType.DataBind();
            ddlItemType.Items.Insert(0, new ListItem(Localization.GetString("CategoriesAndArticles", LocalResourceFile), "-1"));

        }

        protected void lnkGenerate_Click(object sender, EventArgs e)
        {

            odsItemViewReport.Select();

            gvReport.PageSize = Convert.ToInt32(ddlPageNumbers.SelectedValue, CultureInfo.InvariantCulture);
            //            DataTable dt = DataProvider.Instance().GetItemViewPaging(Convert.ToInt32(ddlItemType.SelectedValue), 0, 0, Convert.ToInt32(ddlPageNumbers.SelectedValue), " count(vi.ItemId) desc ", txtStartDate.Text, txtEndDate.Text, PortalId);

            gvReport.AllowPaging = true;
            gvReport.AllowSorting = true;

            gvReport.DataSource = odsItemViewReport;
            gvReport.DataBind();


        }

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvReport.PageIndex = e.NewPageIndex;
            //gvReport.DataSource = 
            //gvReport.DataBind();           
        }

    }

    public class ItemViewReportObject
    {
        private int itemViewId = -1;
        public int ItemViewId
        {
            get { return itemViewId; }
            set { itemViewId = value; }
        }

        private string _name;
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        private int _count;
        public int Count
        {
            get { return this._count; }
            set { this._count = value; }
        }
        private int _totalRows;
        public int TotalRows
        {
            get { return this._totalRows; }
            set { this._totalRows = value; }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataTable GetItemViewsPage(int pageIndex, int itemTypeId, int pageSize, string startDate, string endDate, int portalId)
        {
            DataTable dt = DataProvider.Instance().GetItemViewPaging(itemTypeId, 0, pageIndex, pageSize, " count(vi.ItemId) desc ", startDate, endDate, portalId);
            int.TryParse(dt.Rows[0]["TotalRows"].ToString(), out _totalRows);
            return dt;
        }

        //        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public int GetItemViewsPageCount(int pageIndex, int itemTypeId, int pageSize, string startDate, string endDate, int portalId)
        //public int GetItemViewsPageCount()
        {
            return _totalRows;
        }

        public int GetItemViewsPageCount(int itemTypeId, string startDate, string endDate, int portalId)
        //public int GetItemViewsPageCount()
        {
            return _totalRows;
        }
        public int GetItemViewsPageCount()
        //public int GetItemViewsPageCount()
        {
            return _totalRows;
        }
    }
}
