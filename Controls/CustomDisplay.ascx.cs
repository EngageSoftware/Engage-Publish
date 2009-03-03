//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Publish.Data;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.Controls
{
    public partial class CustomDisplay : ModuleBase, IActionable
    {
        private CustomDisplaySettings customDisplaySettings;

        protected Boolean visibility;
        protected string editText;
        private int categoryId;
        private string qsTags;
        private ArrayList tagQuery;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _usePaging;// = false;
        public bool UsePaging
        {
            [DebuggerStepThrough]
            get { return _usePaging; }
            [DebuggerStepThrough]
            set { _usePaging = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _useCustomSort;// = false;
        public bool UseCustomSort
        {
            [DebuggerStepThrough]
            get { return _useCustomSort; }
            [DebuggerStepThrough]
            set { _useCustomSort = value; }
        }


        #region Event Handlers
        override protected void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);

            this.BindItemData();

            //read the tags querystring parameter and see if we have any to store into the tagQuery arraylist
            if (this.AllowTags && this.Overrideable)
            {
                string tags = Request.QueryString["Tags"];
                if (tags != null)
                {
                    this.qsTags = tags;
                    char[] seperator = { '-' };
                    ArrayList tagList = Tag.ParseTags(this.qsTags, this.PortalId, seperator, false);
                    this.tagQuery = new ArrayList(tagList.Count);
                    foreach (Tag tg in tagList)
                    {
                        //create a list of tagids to query the database
                        this.tagQuery.Add(tg.TagId);
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            this.Load += this.Page_Load;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            //store the "view" for this item
            RecordView();

            //SetPageTitle();
            categoryId = ItemId;
            customDisplaySettings = new CustomDisplaySettings(Settings, TabModuleId);

            UsePaging = customDisplaySettings.AllowPaging;
            UseCustomSort = customDisplaySettings.UseCustomSort;

            if (customDisplaySettings.GetParentFromQueryString)
            {
                //CHECK IF THERE'S ANYTHING IN THE QS AND REACT
                object o = Request.QueryString["ItemId"];
                if (o != null)
                {
                    int itemId;
                    if (int.TryParse(o.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out itemId))
                    {
                        categoryId = Category.GetParentCategory(itemId, PortalId);
                    }
                }
            }

            if (customDisplaySettings.EnableRss)
            {

                //TODO: replace the hyperlink control on the display side and insert our Link/IMAGE dynamically so we can set the alt text.
                lnkRss.Visible = true;
                string rssImage = Localization.GetString("rssImage", LocalResourceFile);
#if DEBUG
                rssImage = rssImage.Replace("[L]", string.Empty);
#endif

                imgRss.Src= ApplicationUrl + rssImage; //"/images/xml.gif";
                imgRss.Alt = Localization.GetString("rssAlt", LocalResourceFile);
                
                lnkRss.Attributes.Add("type", "application/rss+xml");
                lnkRss.ToolTip = Localization.GetString("rssAlt", LocalResourceFile);
               

                if (AllowTags && tagQuery != null && tagQuery.Count > 0)
                {
                    lnkRss.NavigateUrl = GetRssLinkUrl(PortalId, "TagFeed", qsTags);
                    SetRssUrl(lnkRss.NavigateUrl.ToString(), Localization.GetString("rssAlt", LocalResourceFile));
                }
                else
                {
                    //check for a setting of an external URL
                    ItemVersionSetting rssSetting = ItemVersionSetting.GetItemVersionSetting(VersionInfoObject.ItemVersionId, "CategorySettings", "RssUrl", PortalId);
                    if (rssSetting != null && rssSetting.PropertyValue!=string.Empty)
                    {
                        lnkRss.NavigateUrl = rssSetting.PropertyValue;
                        SetExternalRssUrl(lnkRss.NavigateUrl.ToString(), Localization.GetString("rssAlt", LocalResourceFile));
                        
                    }
                    else
                    {
                        //TODO: configure the # of items for an RSS feed
                        lnkRss.NavigateUrl = GetRssLinkUrl(categoryId, 25, ItemType.Article.GetId(), PortalId, "ItemListing");
                        SetRssUrl(lnkRss.NavigateUrl.ToString(), Localization.GetString("rssAlt", LocalResourceFile));
                    }

                }
                
            }

            //store the URL into session for the return to list options
            if (UseSessionForReturnToList(PortalId))
            {
                Session["PublishListLink"] = Request.Url.PathAndQuery.ToString();
            }

            //check if admin, enable edit links
            if ((IsAdmin || IsAuthor) && IsEditable)
            {
                visibility = true;
                editText = Localization.GetString("EditText", LocalResourceFile);
            }
            else
            {
                visibility = false;
                editText = string.Empty;
            }

            try
            {
                if (customDisplaySettings.ItemTypeId == -2)
                {
                    this.lblMessage.Text = Localization.GetString("SetupItemType", LocalResourceFile);
                    return;
                }

                this.lstItems.DataSource = GetData();
                this.lstItems.DataBind();

                if (customDisplaySettings.ShowParent && categoryId != -1)
                {
                    lblCategory.Visible = true;
                    Category parentCategory = Category.GetCategory(categoryId, PortalId);
                    lblCategory.Text = parentCategory.Name;
                    //show the category description if enabled.
                    if (customDisplaySettings.ShowParentDescription)

                        lblCategoryDescription.Text = Utility.ReplaceTokens(parentCategory.Description);
                    
                }
                else
                {
                    lblCategory.Visible = false;
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void lstItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e != null)
            {
                Panel pnlThumbnail = (Panel)e.Item.FindControl("pnlThumbnail");
                //Panel pnlCategory = (Panel)e.Item.FindControl("pnlCategory");
                Panel pnlTitle = (Panel)e.Item.FindControl("pnlTitle");
                Panel pnlDate = (Panel)e.Item.FindControl("pnlDate");

                Panel pnlAuthor = (Panel)e.Item.FindControl("pnlAuthor");
                Panel pnlDescription = (Panel)e.Item.FindControl("pnlDescription");
                Panel pnlReadMore = (Panel)e.Item.FindControl("pnlReadMore");
                HyperLink lnkTitle = (HyperLink)e.Item.FindControl("lnkTitle");
                //Label lblDate = (Label)e.Item.FindControl("lblDate");

                if (pnlThumbnail != null)
                {
                    pnlThumbnail.Visible = customDisplaySettings.DisplayOptionThumbnail; // (DataDisplayFormat == ArticleViewOption.Thumbnail || DataDisplayFormat == ArticleViewOption.TitleAndThumbnail);
                }
                if (pnlDescription != null)
                {
                    pnlDescription.Visible = customDisplaySettings.DisplayOptionAbstract; //(DataDisplayFormat == ArticleViewOption.Thumbnail || DataDisplayFormat == ArticleViewOption.Abstract);
                }
                if (pnlReadMore != null)
                {
                    pnlReadMore.Visible = customDisplaySettings.DisplayOptionReadMore; //(DataDisplayFormat == ArticleViewOption.Thumbnail || DataDisplayFormat == ArticleViewOption.Abstract);
                }
                if (pnlDate != null)
                {
                    pnlDate.Visible = customDisplaySettings.DisplayOptionDate; //(DataDisplayFormat == ArticleViewOption.Thumbnail || DataDisplayFormat == ArticleViewOption.Abstract);
                }
                if (pnlAuthor != null)
                {
                    pnlAuthor.Visible = customDisplaySettings.DisplayOptionAuthor;
                }

                if (pnlTitle != null)
                {
                    pnlTitle.Visible = customDisplaySettings.DisplayOptionTitle;
                }
                //if (pnlCategory != null)
                //{
                //    pnlCategory.Visible = true; //ShowParent;
                //}


                //This code makes sure that an article that is disabled does not get a link.
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRow dr = ((DataRowView)e.Item.DataItem).Row;
                    int childItemId = (int)dr["ChildItemId"];

                    if (Utility.IsDisabled(childItemId, PortalId))
                    {
                        lnkTitle.NavigateUrl = string.Empty;
                        if (pnlReadMore != null)
                        {
                            pnlReadMore.Visible = false;
                        }
                    }

                    if (pnlThumbnail != null && (dr["Thumbnail"] == null || !Utility.HasValue(dr["Thumbnail"].ToString())))
                    {
                        pnlThumbnail.CssClass += " item_listing_nothumbnail";
                    }
                }
            }
        }
        #endregion

        private DataTable GetData()
        {
            DataTable dt = GetDataTable();

            if (customDisplaySettings.GetParentFromQueryString)
            {
                if (dt.Rows.Count < 1)
                {
                    //if we got here we have a category list that didn't return anything, instead of using the querystring's parent let's load the module setting
                    categoryId = ItemId;
                    dt = GetDataTable();
                }
            }
            if (UsePaging && tagQuery == null)
            {
                if (dt.Rows.Count > 0)
                {
                    int totalRows;
                    if (int.TryParse(dt.Rows[0]["TotalRows"].ToString(), out totalRows) && totalRows > 0)
                    {
                        BuildPageList(totalRows);
                    }
                }
            }

            if (!UseCustomSort)
            {
                return SortTable(dt);
            }
            else
                return dt;
            
        }


        private void BuildPageList(int totalItems)
        {
            float numberOfPages = totalItems / (float)customDisplaySettings.MaxDisplayItems;
            int intNumberOfPages = Convert.ToInt32(numberOfPages);
            if (numberOfPages > intNumberOfPages)
            {
                intNumberOfPages++;
            }
            //intNumberOfPages++;

            NameValueCollection queryString = Request.QueryString;
            SetPagingLink(queryString, lnkNext, PageId < intNumberOfPages, PageId + 1, TabId);
            SetPagingLink(queryString, lnkPrevious, PageId - 1 > 0, PageId - 1, TabId);
        }

        // ReSharper disable SuggestBaseTypeForParameter
        private static void SetPagingLink(NameValueCollection queryString, HyperLink link, bool showLink, int linkedPageId, int tabId)
        // ReSharper restore SuggestBaseTypeForParameter
        {
            if (showLink)
            {
                link.Visible = true;

                queryString = new NameValueCollection(queryString);
                queryString["catpageid"] = linkedPageId.ToString(CultureInfo.InvariantCulture);
                List<string> additionalParameters = new List<string>(queryString.Count);

                for (int i = 0; i < queryString.Count; i++)
                {
                    if (string.Equals(queryString.GetKey(i), "TABID", StringComparison.OrdinalIgnoreCase))
                    {
                        int newTabId;
                        if (int.TryParse(queryString[i], NumberStyles.Integer, CultureInfo.InvariantCulture, out newTabId))
                        {
                            tabId = newTabId;
                        }
                    }
                    else if (!string.Equals(queryString.GetKey(i), "LANGUAGE", StringComparison.OrdinalIgnoreCase))
                    {
                        additionalParameters.Add(queryString.GetKey(i) + "=" + queryString[i]);
                    }
                }

                link.NavigateUrl = Globals.NavigateURL(tabId, string.Empty, additionalParameters.ToArray());
            }
            else
            {
                link.Visible = false;
            }
        }

        //TODO: we're sorting the data AFTER getting the data, we need to sort when we get the data.
        private DataTable GetDataTable()
        {
            //setup the caching for CustomDisplay

            string cacheKey = Utility.CacheKeyPublishCustomDisplay +
                    customDisplaySettings.SortOption.Replace(" ", string.Empty).ToString(CultureInfo.InvariantCulture) +
                        categoryId.ToString(CultureInfo.InvariantCulture) + "PageSize" + customDisplaySettings.MaxDisplayItems.ToString(CultureInfo.InvariantCulture) +
                            "PageId" + PageId.ToString(CultureInfo.InvariantCulture);
            DataTable dt = DataCache.GetCache(cacheKey) as DataTable;
            //check for tags
            if (AllowTags && tagQuery != null && tagQuery.Count > 0)
            {
                string tagCacheKey = Utility.CacheKeyPublishTag + PortalId.ToString(CultureInfo.InvariantCulture) 
                    + customDisplaySettings.ItemTypeId.ToString(CultureInfo.InvariantCulture)
                    + "PageSize" + customDisplaySettings.MaxDisplayItems.ToString(CultureInfo.InvariantCulture) 
                    + "PageId" + PageId.ToString(CultureInfo.InvariantCulture)
                    + qsTags; // +"PageId";
                dt = DataCache.GetCache(tagCacheKey) as DataTable;
                if (dt == null)
                {
                    dt = Tag.GetItemsFromTagsPaging(PortalId, tagQuery, customDisplaySettings.MaxDisplayItems, PageId - 1, SortOrder());
                    //dt = Tag.GetItemsFromTags(PortalId, tagQuery);

                    DataCache.SetCache(tagCacheKey, dt, DateTime.Now.AddMinutes(CacheTime));
                    Utility.AddCacheKey(tagCacheKey, PortalId);
                }
            }

            if (dt == null)
            {
                if (customDisplaySettings.SortOption == CustomDisplaySettings.MostPopularSort)
                {
                    dt = DataProvider.Instance().GetMostPopular(categoryId, customDisplaySettings.ItemTypeId, customDisplaySettings.MaxDisplayItems, PortalId);
                }
                else if (categoryId != -1 && categoryId != TopLevelCategoryItemType.Category.GetId())
                {
                    dt = Category.GetChildrenInCategoryPaging(categoryId, customDisplaySettings.ItemTypeId, customDisplaySettings.MaxDisplayItems, PortalId, UseCustomSort, true, SortOrder(), PageId - 1, customDisplaySettings.MaxDisplayItems);
                }
                else //top level category
                {
                    dt = Category.GetChildrenInCategoryPaging(TopLevelCategoryItemType.Category.GetId(), customDisplaySettings.ItemTypeId, customDisplaySettings.MaxDisplayItems, PortalId, customDisplaySettings.UseCustomSort, true, SortOrder(), PageId - 1, customDisplaySettings.MaxDisplayItems);
                }

                //else if (categoryId != -1)
                //{
                //    dt = DataProvider.Instance().GetChildrenInCategory(categoryId, customDisplaySettings.ItemTypeId, customDisplaySettings.MaxDisplayItems, PortalId, SortOrder());
                //}
                //else //top level category
                //{
                //    DataSet ds = DataProvider.Instance().GetItems(TopLevelCategoryItemType.Category.GetId(), PortalId, RelationshipType.CategoryToTopLevelCategory.GetId(), customDisplaySettings.ItemTypeId);
                //    dt = ds.Tables[0];
                //    dt = FormatDataTable(dt);
                //}


                //Set the object into cache
                if (dt != null)
                {
                    DataCache.SetCache(cacheKey, dt, DateTime.Now.AddMinutes(CacheTime));
                    Utility.AddCacheKey(cacheKey, PortalId);
                }
            }
            return dt;
        }

        private DataTable SortTable(DataTable dt)
        {
            string sortDirection = "Asc";
            if (customDisplaySettings.SortDirection == "1")
            {
                sortDirection = "Desc";
            }

            string column;
            switch (customDisplaySettings.SortOption)
            {
                case (CustomDisplaySettings.DateSort):
                    column = "CreatedDate";
                    break;
                case (CustomDisplaySettings.LastUpdatedSort):
                    column = "LastUpdated";
                    break;
                case (CustomDisplaySettings.StartDateSort):
                    column = "StartDate";
                    break;

                case (CustomDisplaySettings.MostPopularSort):
                    column = "TimesViewed";
                    break;
                //case (CustomDisplaySettings.TitleSort):
                default:
                    //if we're doing the title sort we don't need to pass as this is already in the function
                    column = "ChildName";
                    break;
            }
            dt.DefaultView.Sort = column + " " + sortDirection;
            return dt;
        }

        private string SortOrder()
        {
            string sortDirection = "Asc";
            if (customDisplaySettings.SortDirection == "1")
            {
                sortDirection = "Desc";
            }

            string column;
            switch (customDisplaySettings.SortOption)
            {
                case (CustomDisplaySettings.DateSort):
                    column = "CreatedDate";
                    break;
                case (CustomDisplaySettings.LastUpdatedSort):
                    column = "LastUpdated";
                    break;
                case (CustomDisplaySettings.MostPopularSort):
                    column = "TimesViewed";
                    break;
                case (CustomDisplaySettings.StartDateSort):
                    column = "StartDate";
                    break;
                //case (CustomDisplaySettings.TitleSort):
                default:
                    column = "ChildName";
                    break;
            }

            return column + " " + sortDirection;
        }
        /*
                private static DataTable FormatDataTable(DataTable dt)
                {
                    //This method renames the columns of the datatable so that the databinding will work correctly for some of the methods which return columns that are not named the same.  BD
                    DataTable newDataTable = new DataTable(dt.TableName, dt.Namespace);
                    newDataTable.Locale = CultureInfo.InvariantCulture;
                    newDataTable.Columns.Add("CategoryName", typeof(string), "");
                    newDataTable.Columns.Add("Thumbnail");
                    newDataTable.Columns.Add("ChildName");
                    newDataTable.Columns.Add("ChildItemId", typeof(int));
                    newDataTable.Columns.Add("ChildDescription");
                    newDataTable.Columns.Add("LastUpdated");
                    newDataTable.Columns.Add("StartDate");
                    newDataTable.Columns.Add("CreatedDate", typeof(DateTime));
                    newDataTable.Columns.Add("DisplayName");
                    newDataTable.Columns.Add("ChildItemTypeId", typeof(int));

                    foreach (DataRow row in dt.Rows)
                    {
                        DataRow newRow = newDataTable.NewRow();

                        newRow["Thumbnail"] = row["Thumbnail"];
                        newRow["ChildName"] = row["Name"];
                        newRow["ChildItemId"] = Convert.ToInt32(row["ItemId"], CultureInfo.InvariantCulture);
                        newRow["ChildDescription"] = row["Description"];
                        newRow["LastUpdated"] = row["LastUpdated"];
                        newRow["StartDate"] = row["StartDate"];
                        newRow["CreatedDate"] = row["CreatedDate"];
                        newRow["DisplayName"] = row["DisplayName"];
                        newRow["ChildItemTypeId"] = row["ItemTypeId"];

                        newDataTable.Rows.Add(newRow);
                    }

                    return newDataTable;
                }
        */
        protected string FormatDate(object date)
        {
            if (date != null)
            {
                DateTime dt = Convert.ToDateTime(date, CultureInfo.InvariantCulture);
                return dt.ToString(customDisplaySettings.DateFormat, CultureInfo.CurrentCulture);
            }
            return string.Empty;
        }

        /// <summary>
        /// Format Text currently just looks for the token 
        /// </summary>
        protected static string FormatText(object text)
        {
            if (text != null)
            {
                //if <strip> is the first item in our text we need to strip all HTML
                if (text.ToString().IndexOf("[PublishStrip]", StringComparison.Ordinal) > -1)
                {
                    string result = text.ToString().Replace("[PublishStrip]", string.Empty);

                    //return HtmlUtils.StripEntities(result, false);
                    return HttpUtility.HtmlDecode(result);

                }
                return text.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// Record a Viewing.
        /// </summary>
        private void RecordView()
        {
            if (VersionInfoObject != null && !VersionInfoObject.IsNew)
            {
                string referrer = string.Empty;
                if (HttpContext.Current.Request.UrlReferrer != null)
                {
                    referrer = HttpContext.Current.Request.UrlReferrer.ToString();
                }
                string url = string.Empty;
                if (HttpContext.Current.Request.RawUrl != null)
                {
                    url = HttpContext.Current.Request.RawUrl.ToString();
                }
                
                this.VersionInfoObject.AddView(UserId, TabId, HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.UserAgent, referrer, url);
            }
        }

        protected string GetItemTypeCssClass(object dataItem)
        {
            return ItemType.GetItemTypeName((int)DataBinder.Eval(dataItem, "ChildItemTypeId"), UseCache, PortalId, CacheTime);
        }



        #region Optional Interfaces
        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actions = new ModuleActionCollection();
                actions.Add(GetNextActionID(), Localization.GetString("Settings", LocalResourceFile), ModuleActionType.AddContent, string.Empty, string.Empty, EditUrl("Settings"), false, SecurityAccessLevel.Edit, true, false);
                return actions;
            }
        }


        #endregion

        protected static string BuildEditUrl(int itemId, int tabId, int moduleId, int portalId)
        {
            return Utility.BuildEditUrl(itemId, tabId, moduleId, portalId);
        }
    }
}

