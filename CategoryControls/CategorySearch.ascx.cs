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
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Search;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.CategoryControls
{
    public partial class CategorySearch : ModuleBase, IActionable
    {
        #region Event Handlers

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.btnCategorySearch.Click += this.btnCategorySearch_Click;
            this.dgResults.PageIndexChanged += this.dgResults_PageIndexChanged;
            this.dgResults.ItemDataBound += this.dgResults_ItemDataBound;
            this.Load += this.Page_Load;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int id = CategoryId;
                
                ItemRelationship.DisplayCategoryHierarchy(ddlCategoryList, id, PortalId, true);

                if (id == -1)
                {
                    ddlCategoryList.Items.Insert(0, new ListItem(Localization.GetString("AllCategories", LocalResourceFile), "-1"));
                }
                divSearchCategorySelection.Visible = ShowCategorySelection() && ddlCategoryList.Items.Count > 1;

                if (String.IsNullOrEmpty(txtCategorySearch.Text))
                {
                    txtCategorySearch.Text = Localization.GetString("txtCategorySearch", LocalResourceFile);
                }
                
                string searchValue = Request.QueryString["search"];
                if (!string.IsNullOrEmpty(searchValue))
                {
                    divSearchResults.Visible = true;
                    txtCategorySearch.Text = searchValue;
                    BindData();
                }
            }
        }

        #endregion

        private int CategoryId
        {
            get
            {
                object o = Settings["csCategoryId"];
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
        #region Optional Interfaces

        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actions = new ModuleActionCollection();
                actions.Add(GetNextActionID(), Localization.GetString("Settings", LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl("Settings"), false, SecurityAccessLevel.Edit, true, false);
                return actions;
            }
        }

       

        #endregion

        private void BindData()
        {
            object o;
            SearchResultsInfoCollection results = new SearchResultsInfoCollection();
            if (ddlCategoryList.SelectedIndex == 0)
            {
                o = Settings["csCategoryId"];
                if (o != null)
                {
                    int CategoryOption;
                    if (int.TryParse(o.ToString(), out CategoryOption))
                    {
                        results = Search(CategoryOption, txtCategorySearch.Text.Trim());
                    }
                }
                else
                {
                    //else this module hasn't been setup yet?
                }
            }
            else
            {
                results = Search(Convert.ToInt32(ddlCategoryList.SelectedValue, CultureInfo.InvariantCulture), txtCategorySearch.Text.Trim());
            }

            DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            //DataColumn dc = new DataColumn("TabId");
            dt.Columns.Add(new DataColumn("TabId", typeof(int)));
            dt.Columns.Add(new DataColumn("Guid", typeof(string)));
            dt.Columns.Add(new DataColumn("Title", typeof(string)));
            dt.Columns.Add(new DataColumn("Relevance", typeof(int)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("PubDate", typeof(DateTime)));

            //Get the maximum items to display
            int maxItems;
            o = Settings["csMaxResults"];
            if (o == null || !int.TryParse(o.ToString(), out maxItems))
            {
                maxItems = results.Count;
            }

            if (results.Count < maxItems || maxItems < 1)
            {
                maxItems = results.Count;
            }

            int itemsPage = 10;
            o = Settings["csPerPage"];
            if (o != null)
            {
                //itemsPage = Convert.ToInt32(Settings["perpage"]);			
                if (!int.TryParse(o.ToString(), out itemsPage))
                {
                    itemsPage = 10;
                }
            }

            int titleLength = 0;
            o = Settings["csTitleLength"];
            if (o != null)
            {
                if (!int.TryParse(o.ToString(), out titleLength))
                {
                    titleLength = 10;
                }
            }

            int descLength = 0;
            o = Settings["csDescriptionLength"];
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

            using (DataView dv = new DataView(dt))
            {
                dv.Sort = "Relevance DESC";
                if (itemsPage < 1)
                {
                    dgResults.AllowPaging = false;
                    dgResults.PagerStyle.Visible = false;
                }
                else
                {
                    dgResults.PageSize = itemsPage;
                    dgResults.PagerStyle.Visible = results.Count >= dgResults.PageSize;
                }
                dgResults.DataSource = dv;
                dgResults.DataBind();

                dgResults.Visible = results.Count != 0;
                lblNoResults.Visible = results.Count == 0;
            }
        }

        private SearchResultsInfoCollection Search(int categoryId, string criteria)
        {

            SearchResultsInfoCollection results = null;

            try
            {
                //this will apply thenormal security search, 
                //filtering out anything that shouldn't be displayed based on permissions
                results = SearchDataStoreProvider.Instance().GetSearchResults(PortalId, criteria);

                //will contain all the articles beneath the given category or subcategory
                IDictionary articles = new Hashtable();

                DataTable dt;
                if (categoryId == -1)
                {
                    dt = Article.GetArticles(PortalId);
                }
                else
                {
                    DataSet ds = Item.GetAllChildren(ItemType.Article.GetId(), categoryId, RelationshipType.ItemToParentCategory.GetId(), RelationshipType.ItemToRelatedCategory.GetId(), PortalId);
                    dt = ds.Tables[0];
                }
                foreach (DataRow row in dt.Rows)
                {
                    int articleId = Convert.ToInt32(row["ItemId"], CultureInfo.InvariantCulture);
                    if (articles.Contains(articleId) == false)
                        articles.Add(articleId, null);
                }

                //now filter out anything not in the list of articles beneath the given category
                ArrayList al = new ArrayList();
                foreach (SearchResultsInfo result in results)
                {
                    int articleId = Utility.GetArticleId(result);
                    if (articles.Contains(articleId) == false || articleId==0)
                    {
                        //remove this row from the results
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
                DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(this, ex);
            }

            return results;
        }


        public static string FormatUrl(int tabId, string link)
        {
            string strURL;
            if (String.IsNullOrEmpty(link))
            {
                strURL = DotNetNuke.Common.Globals.NavigateURL(tabId);
            }
            else
            {
                strURL = DotNetNuke.Common.Globals.NavigateURL(tabId, "", link);
            }
            return strURL;
        }



        public bool ShowDescription()
        {
            object o = Settings["csShowDescription"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                return o.ToString() == "Y";
            }
            return false;
        }

        public bool ShowCategorySelection()
        {
            object o = Settings["csAllowCategorySelection"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                return o.ToString() == "Y";
            }
            return false;
        }

        public string FormatRelevance(int relevance)
        {
            return Localization.GetString("Relevance", this.LocalResourceFile) + relevance.ToString(CultureInfo.CurrentCulture);
        }

        [Obsolete("This method doesn't do anything")]
        public static string FormatDate(string pubDate)
        {
            return pubDate; //.ToString();
        }

        private void dgResults_ItemDataBound(object source, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                DataRowView row = e.Item.DataItem as DataRowView;
                if (row != null)
                {
                    int itemId;
                    HyperLink lnkTitle = e.Item.FindControl("lnkTitle") as HyperLink;

                    string guid = row["Guid"].ToString();
                    int guidLocation = guid.IndexOf("itemid=", StringComparison.Ordinal);
                    guid = guidLocation > -1 ? guid.Substring("itemid=".Length) : string.Empty;

                    if (lnkTitle != null && int.TryParse(guid, out itemId) && Utility.IsDisabled(itemId, PortalId))
                    {
                        lnkTitle.NavigateUrl = string.Empty;
                    }
                }
            }
        }

        private void dgResults_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgResults.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }

        private void btnCategorySearch_Click(object sender, EventArgs e)
        {
            try
            {
                divSearchResults.Visible = true;
                object o = Settings["csSearchEmptyRedirectUrl"];
                if ((String.IsNullOrEmpty(txtCategorySearch.Text) || txtCategorySearch.Text.Trim() == Localization.GetString("txtCategorySearch", LocalResourceFile)) && o != null)
                {
                    //redirect if no search string was passed
                    if (Uri.IsWellFormedUriString(o.ToString(), UriKind.RelativeOrAbsolute))
                    {
                        Response.Redirect(o.ToString());
                    }
                }

                BindData();
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
            }

        }
    }
}
