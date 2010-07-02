//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

 //TODO: this page doesn't work yet. Fix the objectdatasource

namespace Engage.Dnn.Publish.Admin.Tools
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Data;

    public partial class ItemViewReport : ModuleBase
    {
        public void LoadItemTypes()
        {
            // load the item types dropdown
            // load "ALL"
            // load "Articles"
            // load "Categories"
            this.ddlItemType.DataTextField = "Name";
            this.ddlItemType.DataValueField = "ItemTypeId";
            this.ddlItemType.DataSource = Item.GetItemTypes(this.PortalId);
            this.ddlItemType.DataBind();
            this.ddlItemType.Items.Insert(0, new ListItem(Localization.GetString("CategoriesAndArticles", this.LocalResourceFile), "-1"));
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        protected void gvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // gvReport.PageIndex = e.NewPageIndex;
            // gvReport.DataSource = 
            // gvReport.DataBind();           
        }

        protected void lnkGenerate_Click(object sender, EventArgs e)
        {
            this.odsItemViewReport.Select();

            this.gvReport.PageSize = Convert.ToInt32(this.ddlPageNumbers.SelectedValue, CultureInfo.InvariantCulture);

            // DataTable dt = DataProvider.Instance().GetItemViewPaging(Convert.ToInt32(ddlItemType.SelectedValue), 0, 0, Convert.ToInt32(ddlPageNumbers.SelectedValue), " count(vi.ItemId) desc ", txtStartDate.Text, txtEndDate.Text, PortalId);
            this.gvReport.AllowPaging = true;
            this.gvReport.AllowSorting = true;

            this.gvReport.DataSource = this.odsItemViewReport;
            this.gvReport.DataBind();
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.Page.IsPostBack)
                {
                    this.lblPortalId.Text = this.PortalId.ToString(CultureInfo.CurrentCulture);

                    this.gvReport.PagerSettings.FirstPageText = Localization.GetString("FirstPageText", this.LocalResourceFile);
                    this.gvReport.PagerSettings.LastPageText = Localization.GetString("LastPageText", this.LocalResourceFile);

                    this.LoadItemTypes();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }

    public class ItemViewReportObject
    {
        private int _totalRows;

        private int itemViewId = -1;

        public int Count { get; set; }

        public int ItemViewId
        {
            get { return this.itemViewId; }
            set { this.itemViewId = value; }
        }

        public string Name { get; set; }

        public int TotalRows
        {
            get { return this._totalRows; }
            set { this._totalRows = value; }
        }

        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataTable GetItemViewsPage(int pageIndex, int itemTypeId, int pageSize, string startDate, string endDate, int portalId)
        {
            DataTable dt = DataProvider.Instance().GetItemViewPaging(
                itemTypeId, 0, pageIndex, pageSize, " count(vi.ItemId) desc ", startDate, endDate, portalId);
            int.TryParse(dt.Rows[0]["TotalRows"].ToString(), out this._totalRows);
            return dt;
        }

        // [DataObjectMethod(DataObjectMethodType.Select, false)]
        public int GetItemViewsPageCount(int pageIndex, int itemTypeId, int pageSize, string startDate, string endDate, int portalId)
        {
            // public int GetItemViewsPageCount()
            return this._totalRows;
        }

        public int GetItemViewsPageCount(int itemTypeId, string startDate, string endDate, int portalId)
        {
            // public int GetItemViewsPageCount()
            return this._totalRows;
        }

        public int GetItemViewsPageCount()
        {
            // public int GetItemViewsPageCount()
            return this._totalRows;
        }
    }
}