//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.


namespace Engage.Dnn.Publish.Controls
{
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
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Data;
    using Util;

    public partial class CustomDisplay : ModuleBase, IActionable
    {
        private CustomDisplaySettings _customDisplaySettings;

        protected Boolean Visibility;
        protected string EditText;
        private int _categoryId;
        private string _qsTags;
        private ArrayList _tagQuery;

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
            InitializeComponent();
            base.OnInit(e);

            BindItemData();

            //read the tags querystring parameter and see if we have any to store into the tagQuery arraylist
            if (AllowTags && Overrideable)
            {
                string tags = Request.QueryString["Tags"];
                if (tags != null)
                {
                    _qsTags = tags;
                    char[] seperator = { '-' };
                    ArrayList tagList = Tag.ParseTags(_qsTags, PortalId, seperator, false);
                    _tagQuery = new ArrayList(tagList.Count);
                    foreach (Tag tg in tagList)
                    {
                        //create a list of tagids to query the database
                        _tagQuery.Add(tg.TagId);
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            Load += Page_Load;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            //store the "view" for this item
            RecordView();

            //SetPageTitle();
            _categoryId = ItemId;
            _customDisplaySettings = new CustomDisplaySettings(Settings, TabModuleId);

            UsePaging = _customDisplaySettings.AllowPaging;
            UseCustomSort = _customDisplaySettings.UseCustomSort;

            if (_customDisplaySettings.GetParentFromQueryString)
            {
                //CHECK IF THERE'S ANYTHING IN THE QS AND REACT

                object o = Request.QueryString["ItemId"];
                if (o != null)
                {
                    int itemId;
                    if (int.TryParse(o.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out itemId))
                    {
                        //we need to load the children, rather than siblings if _customDisplaySettings.GetRelatedChildren is enabled and the itemid is for a category, not an article
                        if (_customDisplaySettings.GetRelatedChildren && Item.GetItemType(itemId) == ItemType.Category.Name)
                        {
                            _categoryId = itemId;
                        }
                        //otherwise we're going to get the parent category for the itemid passed in. 
                        else
                            _categoryId = Category.GetParentCategory(itemId, PortalId);
                    }
                }
            }

            if (_customDisplaySettings.EnableRss)
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
               

                if (AllowTags && _tagQuery != null && _tagQuery.Count > 0)
                {
                    lnkRss.NavigateUrl = GetRssLinkUrl(PortalId, "TagFeed", _qsTags);
                    SetRssUrl(lnkRss.NavigateUrl, Localization.GetString("rssAlt", LocalResourceFile));
                }
                else
                {
                    //check for a setting of an external URL
                    ItemVersionSetting rssSetting = ItemVersionSetting.GetItemVersionSetting(VersionInfoObject.ItemVersionId, "CategorySettings", "RssUrl", PortalId);
                    if (rssSetting != null && rssSetting.PropertyValue!=string.Empty)
                    {
                        lnkRss.NavigateUrl = rssSetting.PropertyValue;
                        SetExternalRssUrl(lnkRss.NavigateUrl, Localization.GetString("rssAlt", LocalResourceFile));
                        
                    }
                    else
                    {
                        //TODO: configure the # of items for an RSS feed
                        lnkRss.NavigateUrl = GetRssLinkUrl(_categoryId, 25, ItemType.Article.GetId(), PortalId, "ItemListing");
                        SetRssUrl(lnkRss.NavigateUrl, Localization.GetString("rssAlt", LocalResourceFile));
                    }

                }
                
            }

            //store the URL into session for the return to list options
            if (UseSessionForReturnToList(PortalId))
            {
                Session["PublishListLink"] = Request.Url.PathAndQuery;
            }

            //check if admin, enable edit links
            if ((IsAdmin || IsAuthor) && IsEditable)
            {
                Visibility = true;
                EditText = Localization.GetString("EditText", LocalResourceFile);
            }
            else
            {
                Visibility = false;
                EditText = string.Empty;
            }

            try
            {
                if (_customDisplaySettings.ItemTypeId == -2)
                {
                    lblMessage.Text = Localization.GetString("SetupItemType", LocalResourceFile);
                    return;
                }

                lstItems.DataSource = GetData();
                lstItems.DataBind();

                if ((_customDisplaySettings.ShowParent || _customDisplaySettings.ShowParentDescription) && _categoryId != -1)
                {
                    
                    Category parentCategory = Category.GetCategory(_categoryId, PortalId);
                    if (_customDisplaySettings.ShowParent)
                    {
                        divParentCategoryName.Visible = true;
                        lblCategory.Text = parentCategory.Name;
                    }

                    //show the category description if enabled.
                    if (_customDisplaySettings.ShowParentDescription)
                    {
                        divParentCategoryDescription.Visible = true;
                        lblCategoryDescription.Text = Utility.ReplaceTokens(parentCategory.Description);
                    }
                    
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
                var pnlThumbnail = (Panel)e.Item.FindControl("pnlThumbnail");
                //Panel pnlCategory = (Panel)e.Item.FindControl("pnlCategory");
                var pnlTitle = (Panel)e.Item.FindControl("pnlTitle");
                var pnlDate = (Panel)e.Item.FindControl("pnlDate");

                var pnlAuthor = (Panel)e.Item.FindControl("pnlAuthor");
                var pnlDescription = (Panel)e.Item.FindControl("pnlDescription");
                var pnlReadMore = (Panel)e.Item.FindControl("pnlReadMore");
                var pnlStats = (Panel)e.Item.FindControl("pnlStats");
                var lnkTitle = (HyperLink)e.Item.FindControl("lnkTitle");
                var lblTitle = (Label)e.Item.FindControl("lblTitle");
                //Label lblDate = (Label)e.Item.FindControl("lblDate");

                if (pnlThumbnail != null)
                {
                    pnlThumbnail.Visible = _customDisplaySettings.DisplayOptionThumbnail;
                }
                if (pnlDescription != null)
                {
                    pnlDescription.Visible = _customDisplaySettings.DisplayOptionAbstract;
                }
                if (pnlReadMore != null)
                {
                    pnlReadMore.Visible = _customDisplaySettings.DisplayOptionReadMore;
                }

                if (pnlStats != null)
                {
                    pnlStats.Visible = _customDisplaySettings.DisplayOptionStats; 
                }
                if (pnlDate != null)
                {
                    pnlDate.Visible = _customDisplaySettings.DisplayOptionDate; //(DataDisplayFormat == ArticleViewOption.Thumbnail || DataDisplayFormat == ArticleViewOption.Abstract);
                }
                if (pnlAuthor != null)
                {
                    pnlAuthor.Visible = _customDisplaySettings.DisplayOptionAuthor;
                }

                if (pnlTitle != null)
                {
                    
                    pnlTitle.Visible = _customDisplaySettings.DisplayOptionTitle;
                   
                }
                //if (pnlCategory != null)
                //{
                //    pnlCategory.Visible = true; //ShowParent;
                //}


                //This code makes sure that an article that is disabled does not get a link.
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRow dr = ((DataRowView)e.Item.DataItem).Row;
                    var childItemId = (int)dr["ChildItemId"];

                    if (Utility.IsDisabled(childItemId, PortalId))
                    {
                        lnkTitle.NavigateUrl = string.Empty;
                        lblTitle.Visible = true;
                        lnkTitle.Visible = false;

                        if (pnlReadMore != null)
                        {
                            pnlReadMore.Visible = false;
                        }

                    }
                    else
                    {
                        lblTitle.Visible = false;
                        lnkTitle.Visible = true;

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

            //_customDisplaySettings.GetRelatedChildren
            if (_customDisplaySettings.GetParentFromQueryString)
            {
                if (dt.Rows.Count < 1)
                {
                    //if we got here we have a category list that didn't return anything, instead of using the querystring's parent let's load the module setting
                    _categoryId = ItemId;
                    dt = GetDataTable();
                }
            }
            if (UsePaging && _tagQuery == null)
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
            return dt;
        }


        private void BuildPageList(int totalItems)
        {
            float numberOfPages = totalItems / (float)_customDisplaySettings.MaxDisplayItems;
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
                var additionalParameters = new List<string>(queryString.Count);

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
                    _customDisplaySettings.SortOption.Replace(" ", string.Empty).ToString(CultureInfo.InvariantCulture) +
                        _categoryId.ToString(CultureInfo.InvariantCulture) + "PageSize" 
                        + _customDisplaySettings.MaxDisplayItems.ToString(CultureInfo.InvariantCulture)
                        + "ItemType" + _customDisplaySettings.ItemTypeId
                        + "PageId" + PageId.ToString(CultureInfo.InvariantCulture);
            var dt = DataCache.GetCache(cacheKey) as DataTable;
            //check for tags
            if (AllowTags && _tagQuery != null && _tagQuery.Count > 0)
            {
                string tagCacheKey = Utility.CacheKeyPublishTag + PortalId.ToString(CultureInfo.InvariantCulture) 
                    + _customDisplaySettings.ItemTypeId.ToString(CultureInfo.InvariantCulture)
                    + "PageSize" + _customDisplaySettings.MaxDisplayItems.ToString(CultureInfo.InvariantCulture) 
                    + "PageId" + PageId.ToString(CultureInfo.InvariantCulture)
                    + _qsTags; // +"PageId";
                dt = DataCache.GetCache(tagCacheKey) as DataTable;
                if (dt == null)
                {
                    dt = Tag.GetItemsFromTagsPaging(PortalId, _tagQuery, _customDisplaySettings.MaxDisplayItems, PageId - 1, SortOrder());
                    //dt = Tag.GetItemsFromTags(PortalId, tagQuery);

                    DataCache.SetCache(tagCacheKey, dt, DateTime.Now.AddMinutes(CacheTime));
                    Utility.AddCacheKey(tagCacheKey, PortalId);
                }
            }

            if (dt == null)
            {
                if (_customDisplaySettings.SortOption == CustomDisplaySettings.MostPopularSort)
                {
                    dt = DataProvider.Instance().GetMostPopular(_categoryId, _customDisplaySettings.ItemTypeId, _customDisplaySettings.MaxDisplayItems, PortalId);
                }
                else if (_categoryId != -1 && _categoryId != TopLevelCategoryItemType.Category.GetId())
                {
                    dt = Category.GetChildrenInCategoryPaging(_categoryId, _customDisplaySettings.ItemTypeId, _customDisplaySettings.MaxDisplayItems, PortalId, UseCustomSort, true, SortOrder(), PageId - 1, _customDisplaySettings.MaxDisplayItems);
                }
                else //top level category
                {
                    dt = Category.GetChildrenInCategoryPaging(TopLevelCategoryItemType.Category.GetId(), _customDisplaySettings.ItemTypeId, _customDisplaySettings.MaxDisplayItems, PortalId, _customDisplaySettings.UseCustomSort, true, SortOrder(), PageId - 1, _customDisplaySettings.MaxDisplayItems);
                }

                //else if (_categoryId != -1)
                //{
                //    dt = DataProvider.Instance().GetChildrenInCategory(_categoryId, _customDisplaySettings.ItemTypeId, _customDisplaySettings.MaxDisplayItems, PortalId, SortOrder());
                //}
                //else //top level category
                //{
                //    DataSet ds = DataProvider.Instance().GetItems(TopLevelCategoryItemType.Category.GetId(), PortalId, RelationshipType.CategoryToTopLevelCategory.GetId(), _customDisplaySettings.ItemTypeId);
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
            if (_customDisplaySettings.SortDirection == "1")
            {
                sortDirection = "Desc";
            }

            string column;
            switch (_customDisplaySettings.SortOption)
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
            if (_customDisplaySettings.SortDirection == "1")
            {
                sortDirection = "Desc";
            }

            string column;
            switch (_customDisplaySettings.SortOption)
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
                return dt.ToString(_customDisplaySettings.DateFormat, CultureInfo.CurrentCulture);
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
                    return HttpUtility.HtmlDecode(Utility.ReplaceTokens(result));

                }

                return Utility.ReplaceTokens(text.ToString());
            }
            return string.Empty;
        }
          /// <summary>
        /// Format Text currently just looks for the token 
        /// </summary>
        protected string DisplayItemCommentCount(object commentCount)
        {
            if (commentCount != null)
            {
                return String.Format(Localization.GetString("CommentStats", LocalResourceFile),commentCount);
            }
              return string.Empty;
        }

        /// <summary>
        /// Format Text currently just looks for the token 
        /// </summary>
        protected string DisplayItemViewCount(object viewCount)
        {
            if (viewCount != null)
            {
                return String.Format(Localization.GetString("ViewStats", LocalResourceFile), viewCount);
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
                    url = HttpContext.Current.Request.RawUrl;
                }
                
                VersionInfoObject.AddView(UserId, TabId, HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.UserAgent, referrer, url);
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
                return new ModuleActionCollection
                           {
                                   {
                                           GetNextActionID(),
                                           Localization.GetString("Settings", LocalResourceFile),
                                           ModuleActionType.AddContent, string.Empty, string.Empty,
                                           EditUrl("Settings"), false, SecurityAccessLevel.Edit, true, false
                                           }
                           };
            }
        }


        #endregion

        protected static string BuildEditUrl(int itemId, int tabId, int moduleId, int portalId)
        {
            return Utility.BuildEditUrl(itemId, tabId, moduleId, portalId);
        }

        protected static string GetAuthor(object author, object authorUserId, int portalId)
        {
            if (author.ToString().Trim().Length > 0)
                return author.ToString();
            var uc = new UserController();
            return uc.GetUser(portalId, Convert.ToInt32(authorUserId)).DisplayName;
                

        }
    }
}

