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
    using System.Collections;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Services.Search;

    using Engage.Dnn.Publish.Util;

    public partial class CategorySearch : ModuleBase, IActionable
    {
        public ModuleActionCollection ModuleActions
        {
            get
            {
                return new ModuleActionCollection
                    {
                        {
                            this.GetNextActionID(), Localization.GetString("Settings", this.LocalResourceFile), ModuleActionType.AddContent, 
                            string.Empty, string.Empty, this.EditUrl("Settings"), false, SecurityAccessLevel.Edit, true, false
                            }
                    };
            }
        }

        private int CategoryId
        {
            get
            {
                object o = this.Settings["csCategoryId"];
                if (o != null)
                {
                    int categoryId;
                    if (int.TryParse(o.ToString(), out categoryId))
                    {
                        return categoryId;
                    }
                }

                return -1;
            }
        }

        [Obsolete("This method doesn't do anything")]
        public static string FormatDate(string pubDate)
        {
            return pubDate; // .ToString();
        }

        public static string FormatUrl(int tabId, string link)
        {
            string strURL = String.IsNullOrEmpty(link) ? Globals.NavigateURL(tabId) : Globals.NavigateURL(tabId, string.Empty, link);
            return strURL;
        }

        public string FormatRelevance(int relevance)
        {
            return Localization.GetString("Relevance", this.LocalResourceFile) + relevance.ToString(CultureInfo.CurrentCulture);
        }

        public bool ShowCategorySelection()
        {
            object o = this.Settings["csAllowCategorySelection"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                return o.ToString() == "Y";
            }

            return false;
        }

        public bool ShowDescription()
        {
            object o = this.Settings["csShowDescription"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                return o.ToString() == "Y";
            }

            return false;
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void BindData()
        {
            object o;
            var results = new SearchResultsInfoCollection();
            if (this.ddlCategoryList.SelectedIndex == 0)
            {
                o = this.Settings["csCategoryId"];
                if (o != null)
                {
                    int categoryOption;
                    if (int.TryParse(o.ToString(), out categoryOption))
                    {
                        results = this.Search(categoryOption, this.txtCategorySearch.Text.Trim());
                    }
                }
            }
            else
            {
                results = this.Search(
                    Convert.ToInt32(this.ddlCategoryList.SelectedValue, CultureInfo.InvariantCulture), this.txtCategorySearch.Text.Trim());
            }

            var dt = new DataTable
                {
                    Locale = CultureInfo.InvariantCulture
                };

            // DataColumn dc = new DataColumn("TabId");
            dt.Columns.Add(new DataColumn("TabId", typeof(int)));
            dt.Columns.Add(new DataColumn("Guid", typeof(string)));
            dt.Columns.Add(new DataColumn("Title", typeof(string)));
            dt.Columns.Add(new DataColumn("Relevance", typeof(int)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("PubDate", typeof(DateTime)));

            // Get the maximum items to display
            int maxItems;
            o = this.Settings["csMaxResults"];
            if (o == null || !int.TryParse(o.ToString(), out maxItems))
            {
                maxItems = results.Count;
            }

            if (results.Count < maxItems || maxItems < 1)
            {
                maxItems = results.Count;
            }

            int itemsPage = 10;
            o = this.Settings["csPerPage"];
            if (o != null)
            {
                // itemsPage = Convert.ToInt32(Settings["perpage"]);			
                if (!int.TryParse(o.ToString(), out itemsPage))
                {
                    itemsPage = 10;
                }
            }

            int titleLength = 0;
            o = this.Settings["csTitleLength"];
            if (o != null)
            {
                if (!int.TryParse(o.ToString(), out titleLength))
                {
                    titleLength = 10;
                }
            }

            int descLength = 0;
            o = this.Settings["csDescriptionLength"];
            if (o != null)
            {
                if (!int.TryParse(o.ToString(), out descLength))
                {
                    descLength = 10;
                }
            }

            for (int i = 0; i < maxItems; i++)
            {
                SearchResultsInfo resultItem = results[i];
                DataRow dr = dt.NewRow();
                dr["TabId"] = resultItem.TabId;
                dr["Guid"] = resultItem.Guid;
                if (titleLength > 0 && titleLength < resultItem.Title.Length)
                {
                    dr["Title"] = resultItem.Title.Substring(0, titleLength);
                }
                else
                {
                    dr["Title"] = resultItem.Title;
                }

                dr["Relevance"] = resultItem.Relevance;
                if (descLength > 0 && descLength < resultItem.Description.Length)
                {
                    dr["Description"] = resultItem.Description.Substring(0, descLength);
                }
                else
                {
                    dr["Description"] = resultItem.Description;
                }

                dr["PubDate"] = resultItem.PubDate;

                dt.Rows.Add(dr);
            }

            using (var dv = new DataView(dt))
            {
                dv.Sort = "Relevance DESC";
                if (itemsPage < 1)
                {
                    this.dgResults.AllowPaging = false;
                    this.dgResults.PagerStyle.Visible = false;
                }
                else
                {
                    this.dgResults.PageSize = itemsPage;
                    this.dgResults.PagerStyle.Visible = results.Count >= this.dgResults.PageSize;
                }

                this.dgResults.DataSource = dv;
                this.dgResults.DataBind();

                this.dgResults.Visible = results.Count != 0;
                this.lblNoResults.Visible = results.Count == 0;
            }
        }

        private void BtnCategorySearchClick(object sender, EventArgs e)
        {
            try
            {
                this.divSearchResults.Visible = true;
                object o = this.Settings["csSearchEmptyRedirectUrl"];
                if ((String.IsNullOrEmpty(this.txtCategorySearch.Text) ||
                     this.txtCategorySearch.Text.Trim() == Localization.GetString("txtCategorySearch", this.LocalResourceFile)) && o != null)
                {
                    // redirect if no search string was passed
                    if (Uri.IsWellFormedUriString(o.ToString(), UriKind.RelativeOrAbsolute))
                    {
                        this.Response.Redirect(o.ToString());
                    }
                }

                this.BindData();
            }
            catch (Exception ex)
            {
                Exceptions.LogException(ex);
            }
        }

        private void DgResultsItemDataBound(object source, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                var row = e.Item.DataItem as DataRowView;
                if (row != null)
                {
                    int itemId;
                    var lnkTitle = e.Item.FindControl("lnkTitle") as HyperLink;

                    string guid = row["Guid"].ToString();
                    int guidLocation = guid.IndexOf("itemid=", StringComparison.Ordinal);
                    guid = guidLocation > -1 ? guid.Substring("itemid=".Length) : string.Empty;

                    if (lnkTitle != null && int.TryParse(guid, out itemId) && Utility.IsDisabled(itemId, this.PortalId))
                    {
                        lnkTitle.NavigateUrl = string.Empty;
                    }
                }
            }
        }

        private void DgResultsPageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            this.dgResults.CurrentPageIndex = e.NewPageIndex;
            this.BindData();
        }

        private void InitializeComponent()
        {
            this.btnCategorySearch.Click += this.BtnCategorySearchClick;
            this.dgResults.PageIndexChanged += this.DgResultsPageIndexChanged;
            this.dgResults.ItemDataBound += this.DgResultsItemDataBound;
            this.Load += this.Page_Load;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                int id = this.CategoryId;

                ItemRelationship.DisplayCategoryHierarchy(this.ddlCategoryList, id, this.PortalId, true);

                if (id == -1)
                {
                    this.ddlCategoryList.Items.Insert(0, new ListItem(Localization.GetString("AllCategories", this.LocalResourceFile), "-1"));
                }

                this.divSearchCategorySelection.Visible = this.ShowCategorySelection() && this.ddlCategoryList.Items.Count > 1;

                if (String.IsNullOrEmpty(this.txtCategorySearch.Text))
                {
                    this.txtCategorySearch.Text = Localization.GetString("txtCategorySearch", this.LocalResourceFile);
                }

                string searchValue = this.Request.QueryString["search"];
                if (!string.IsNullOrEmpty(searchValue))
                {
                    this.divSearchResults.Visible = true;
                    this.txtCategorySearch.Text = searchValue;
                    this.BindData();
                }
            }
        }

        private SearchResultsInfoCollection Search(int categoryId, string criteria)
        {
            SearchResultsInfoCollection results = null;

            try
            {
                // this will apply thenormal security search, 
                // filtering out anything that shouldn't be displayed based on permissions
                results = SearchDataStoreProvider.Instance().GetSearchResults(this.PortalId, criteria);

                // will contain all the articles beneath the given category or subcategory
                IDictionary articles = new Hashtable();

                DataTable dt;
                if (categoryId == -1)
                {
                    dt = Article.GetArticles(this.PortalId);
                }
                else
                {
                    DataSet ds = Item.GetAllChildren(
                        ItemType.Article.GetId(), 
                        categoryId, 
                        RelationshipType.ItemToParentCategory.GetId(), 
                        RelationshipType.ItemToRelatedCategory.GetId(), 
                        this.PortalId);
                    dt = ds.Tables[0];
                }

                foreach (DataRow row in dt.Rows)
                {
                    int articleId = Convert.ToInt32(row["ItemId"], CultureInfo.InvariantCulture);
                    if (articles.Contains(articleId) == false)
                    {
                        articles.Add(articleId, null);
                    }
                }

                // now filter out anything not in the list of articles beneath the given category
                var al = new ArrayList();
                foreach (SearchResultsInfo result in results)
                {
                    int articleId = Utility.GetArticleId(result);
                    if (articles.Contains(articleId) == false || articleId == 0)
                    {
                        // remove this row from the results
                        al.Add(result);
                    }
                }

                foreach (SearchResultsInfo result in al)
                {
                    results.Remove(result);
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

            return results;
        }
    }
}