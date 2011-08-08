//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
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
    using System.Diagnostics.CodeAnalysis;
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

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Util;

    public partial class CustomDisplay : ModuleBase, IActionable
    {
        protected string EditText;

        protected bool Visibility;

        private int _categoryId;

        private CustomDisplaySettings _customDisplaySettings;

        private string _qsTags;

        private ArrayList _tagQuery;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _useCustomSort; // = false;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool _usePaging; // = false;

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

        public bool UseCustomSort
        {
            [DebuggerStepThrough]
            get { return this._useCustomSort; }
            [DebuggerStepThrough]
            set { this._useCustomSort = value; }
        }

        public bool UsePaging
        {
            [DebuggerStepThrough]
            get { return this._usePaging; }
            [DebuggerStepThrough]
            set { this._usePaging = value; }
        }

        protected static string BuildEditUrl(int itemId, int tabId, int moduleId, int portalId)
        {
            return UrlGenerator.BuildEditUrl(itemId, tabId, moduleId, portalId);
        }

        /// <summary>
        /// Format Text currently just looks for the token 
        /// </summary>
        protected static string FormatText(object text)
        {
            if (text != null)
            {
                // if <strip> is the first item in our text we need to strip all HTML
                if (text.ToString().IndexOf("[PublishStrip]", StringComparison.Ordinal) > -1)
                {
                    string result = text.ToString().Replace("[PublishStrip]", string.Empty);

                    // return HtmlUtils.StripEntities(result, false);
                    return HttpUtility.HtmlDecode(Utility.ReplaceTokens(result));
                }

                return Utility.ReplaceTokens(text.ToString());
            }

            return string.Empty;
        }

        protected static string GetAuthor(object author, object authorUserId, int portalId)
        {
            if (author.ToString().Trim().Length > 0)
            {
                return author.ToString();
            }

            var uc = new UserController();
            return uc.GetUser(portalId, Convert.ToInt32(authorUserId)).DisplayName;
        }

        /// <summary>
        /// Format Text currently just looks for the token 
        /// </summary>
        protected string DisplayItemCommentCount(object commentCount)
        {
            if (commentCount != null)
            {
                return string.Format(Localization.GetString("CommentStats", this.LocalResourceFile), commentCount);
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
                return string.Format(Localization.GetString("ViewStats", this.LocalResourceFile), viewCount);
            }

            return string.Empty;
        }

        protected string FormatDate(object date)
        {
            if (date != null)
            {
                DateTime dt = Convert.ToDateTime(date, CultureInfo.InvariantCulture);
                return dt.ToString(this._customDisplaySettings.DateFormat, CultureInfo.CurrentCulture);
            }

            return string.Empty;
        }

        protected string GetItemTypeCssClass(object dataItem)
        {
            return ItemType.GetItemTypeName((int)DataBinder.Eval(dataItem, "ChildItemTypeId"), this.UseCache, this.PortalId, this.CacheTime);
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);

            this.BindItemData();

            // read the tags querystring parameter and see if we have any to store into the tagQuery arraylist
            if (this.AllowTags && this.Overrideable)
            {
                string tags = this.Request.QueryString["Tags"];
                if (tags != null)
                {
                    this._qsTags = tags;
                    char[] seperator = {
                                           '-'
                                       };
                    ArrayList tagList = Tag.ParseTags(this._qsTags, this.PortalId, seperator, false);
                    this._tagQuery = new ArrayList(tagList.Count);
                    foreach (Tag tg in tagList)
                    {
                        // create a list of tagids to query the database
                        this._tagQuery.Add(tg.TagId);
                    }
                }
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void lstItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e != null)
            {
                var pnlThumbnail = (Panel)e.Item.FindControl("pnlThumbnail");

                // Panel pnlCategory = (Panel)e.Item.FindControl("pnlCategory");
                var pnlTitle = (Panel)e.Item.FindControl("pnlTitle");
                var pnlDate = (Panel)e.Item.FindControl("pnlDate");

                var pnlAuthor = (Panel)e.Item.FindControl("pnlAuthor");
                var pnlDescription = (Panel)e.Item.FindControl("pnlDescription");
                var pnlReadMore = (Panel)e.Item.FindControl("pnlReadMore");
                var pnlStats = (Panel)e.Item.FindControl("pnlStats");
                var lnkTitle = (HyperLink)e.Item.FindControl("lnkTitle");
                var lblTitle = (Label)e.Item.FindControl("lblTitle");

                // Label lblDate = (Label)e.Item.FindControl("lblDate");
                if (pnlThumbnail != null)
                {
                    pnlThumbnail.Visible = this._customDisplaySettings.DisplayOptionThumbnail;
                }

                if (pnlDescription != null)
                {
                    pnlDescription.Visible = this._customDisplaySettings.DisplayOptionAbstract;
                }

                if (pnlReadMore != null)
                {
                    pnlReadMore.Visible = this._customDisplaySettings.DisplayOptionReadMore;
                }

                if (pnlStats != null)
                {
                    pnlStats.Visible = this._customDisplaySettings.DisplayOptionStats;
                }

                if (pnlDate != null)
                {
                    pnlDate.Visible = this._customDisplaySettings.DisplayOptionDate;
                        
                        // (DataDisplayFormat == ArticleViewOption.Thumbnail || DataDisplayFormat == ArticleViewOption.Abstract);
                }

                if (pnlAuthor != null)
                {
                    pnlAuthor.Visible = this._customDisplaySettings.DisplayOptionAuthor;
                }

                if (pnlTitle != null)
                {
                    pnlTitle.Visible = this._customDisplaySettings.DisplayOptionTitle;
                }

                // if (pnlCategory != null)
                // {
                // pnlCategory.Visible = true; //ShowParent;
                // }

                // This code makes sure that an article that is disabled does not get a link.
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRow dr = ((DataRowView)e.Item.DataItem).Row;
                    var childItemId = (int)dr["ChildItemId"];

                    if (Utility.IsDisabled(childItemId, this.PortalId))
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

                    if (pnlThumbnail != null && (dr["Thumbnail"] == null || !Engage.Utility.HasValue(dr["Thumbnail"].ToString())))
                    {
                        pnlThumbnail.CssClass += " item_listing_nothumbnail";
                    }
                }
            }
        }

        // ReSharper disable SuggestBaseTypeForParameter
        private static void SetPagingLink(NameValueCollection queryString, HyperLink link, bool showLink, int linkedPageId, int tabId)
        {
            // ReSharper restore SuggestBaseTypeForParameter
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

        private void BuildPageList(int totalItems)
        {
            float numberOfPages = totalItems / (float)this._customDisplaySettings.MaxDisplayItems;
            int intNumberOfPages = Convert.ToInt32(numberOfPages);
            if (numberOfPages > intNumberOfPages)
            {
                intNumberOfPages++;
            }

            // intNumberOfPages++;
            NameValueCollection queryString = this.Request.QueryString;
            SetPagingLink(queryString, this.lnkNext, this.PageId < intNumberOfPages, this.PageId + 1, this.TabId);
            SetPagingLink(queryString, this.lnkPrevious, this.PageId - 1 > 0, this.PageId - 1, this.TabId);
        }

        private DataTable GetData()
        {
            DataTable dt = this.GetDataTable();

            // _customDisplaySettings.GetRelatedChildren
            if (this._customDisplaySettings.GetParentFromQueryString)
            {
                if (dt.Rows.Count < 1)
                {
                    // if we got here we have a category list that didn't return anything, instead of using the querystring's parent let's load the module setting
                    this._categoryId = this.ItemId;
                    dt = this.GetDataTable();
                }
            }

            if (this.UsePaging && this._tagQuery == null)
            {
                if (dt.Rows.Count > 0)
                {
                    int totalRows;
                    if (int.TryParse(dt.Rows[0]["TotalRows"].ToString(), out totalRows) && totalRows > 0)
                    {
                        this.BuildPageList(totalRows);
                    }
                }
            }

            if (!this.UseCustomSort)
            {
                return this.SortTable(dt);
            }

            return dt;
        }

        // TODO: we're sorting the data AFTER getting the data, we need to sort when we get the data.
        private DataTable GetDataTable()
        {
            // setup the caching for CustomDisplay
            string cacheKey = Utility.CacheKeyPublishCustomDisplay +
                              this._customDisplaySettings.SortOption.Replace(" ", string.Empty).ToString(CultureInfo.InvariantCulture) +
                              this._categoryId.ToString(CultureInfo.InvariantCulture) + "PageSize" +
                              this._customDisplaySettings.MaxDisplayItems.ToString(CultureInfo.InvariantCulture) + "ItemType" +
                              this._customDisplaySettings.ItemTypeId + "PageId" + this.PageId.ToString(CultureInfo.InvariantCulture);
            var dt = DataCache.GetCache(cacheKey) as DataTable;

            // check for tags
            if (this.AllowTags && this._tagQuery != null && this._tagQuery.Count > 0)
            {
                string tagCacheKey = Utility.CacheKeyPublishTag + this.PortalId.ToString(CultureInfo.InvariantCulture) +
                                     this._customDisplaySettings.ItemTypeId.ToString(CultureInfo.InvariantCulture) + "PageSize" +
                                     this._customDisplaySettings.MaxDisplayItems.ToString(CultureInfo.InvariantCulture) + "PageId" +
                                     this.PageId.ToString(CultureInfo.InvariantCulture) + this._qsTags; // +"PageId";
                dt = DataCache.GetCache(tagCacheKey) as DataTable;
                if (dt == null)
                {
                    dt = Tag.GetItemsFromTagsPaging(
                        this.PortalId, this._tagQuery, this._customDisplaySettings.MaxDisplayItems, this.PageId - 1, this.SortOrder());

                    // dt = Tag.GetItemsFromTags(PortalId, tagQuery);
                    DataCache.SetCache(tagCacheKey, dt, DateTime.Now.AddMinutes(this.CacheTime));
                    Utility.AddCacheKey(tagCacheKey, this.PortalId);
                }
            }

            if (dt == null)
            {
                if (this._customDisplaySettings.SortOption == CustomDisplaySettings.MostPopularSort)
                {
                    dt = DataProvider.Instance().GetMostPopular(
                        this._categoryId, this._customDisplaySettings.ItemTypeId, this._customDisplaySettings.MaxDisplayItems, this.PortalId);
                }
                else if (this._categoryId != -1 && this._categoryId != TopLevelCategoryItemType.Category.GetId())
                {
                    dt = Category.GetChildrenInCategoryPaging(
                        this._categoryId, 
                        this._customDisplaySettings.ItemTypeId, 
                        this._customDisplaySettings.MaxDisplayItems, 
                        this.PortalId, 
                        this.UseCustomSort, 
                        true, 
                        this.SortOrder(), 
                        this.PageId - 1, 
                        this._customDisplaySettings.MaxDisplayItems);
                }
                else
                {
                    // top level category
                    dt = Category.GetChildrenInCategoryPaging(
                        TopLevelCategoryItemType.Category.GetId(), 
                        this._customDisplaySettings.ItemTypeId, 
                        this._customDisplaySettings.MaxDisplayItems, 
                        this.PortalId, 
                        this._customDisplaySettings.UseCustomSort, 
                        true, 
                        this.SortOrder(), 
                        this.PageId - 1, 
                        this._customDisplaySettings.MaxDisplayItems);
                }

                // else if (_categoryId != -1)
                // {
                // dt = DataProvider.Instance().GetChildrenInCategory(_categoryId, _customDisplaySettings.ItemTypeId, _customDisplaySettings.MaxDisplayItems, PortalId, SortOrder());
                // }
                // else //top level category
                // {
                // DataSet ds = DataProvider.Instance().GetItems(TopLevelCategoryItemType.Category.GetId(), PortalId, RelationshipType.CategoryToTopLevelCategory.GetId(), _customDisplaySettings.ItemTypeId);
                // dt = ds.Tables[0];
                // dt = FormatDataTable(dt);
                // }

                // Set the object into cache
                if (dt != null)
                {
                    DataCache.SetCache(cacheKey, dt, DateTime.Now.AddMinutes(this.CacheTime));
                    Utility.AddCacheKey(cacheKey, this.PortalId);
                }
            }

            return dt;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            // store the "view" for this item
            this.RecordView();

            // SetPageTitle();
            this._categoryId = this.ItemId;
            this._customDisplaySettings = new CustomDisplaySettings(this.Settings, this.TabModuleId);

            this.UsePaging = this._customDisplaySettings.AllowPaging;
            this.UseCustomSort = this._customDisplaySettings.UseCustomSort;

            if (this._customDisplaySettings.GetParentFromQueryString)
            {
                // CHECK IF THERE'S ANYTHING IN THE QS AND REACT
                object o = this.Request.QueryString["ItemId"];
                if (o != null)
                {
                    int itemId;
                    if (int.TryParse(o.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out itemId))
                    {
                        // we need to load the children, rather than siblings if _customDisplaySettings.GetRelatedChildren is enabled and the itemid is for a category, not an article
                        if (this._customDisplaySettings.GetRelatedChildren && Item.GetItemType(itemId) == ItemType.Category.Name)
                        {
                            this._categoryId = itemId;
                        }
                            
                            // otherwise we're going to get the parent category for the itemid passed in. 
                        else
                        {
                            this._categoryId = Category.GetParentCategory(itemId, this.PortalId);
                        }
                    }
                }
            }

            if (this._customDisplaySettings.EnableRss)
            {
                // TODO: replace the hyperlink control on the display side and insert our Link/IMAGE dynamically so we can set the alt text.
                this.lnkRss.Visible = true;
                string rssImage = Localization.GetString("rssImage", this.LocalResourceFile);
#if DEBUG
                rssImage = rssImage.Replace("[L]", string.Empty);
#endif

                this.imgRss.Src = ApplicationUrl + rssImage; // "/images/xml.gif";
                this.imgRss.Alt = Localization.GetString("rssAlt", this.LocalResourceFile);
                this.lnkRss.ToolTip = Localization.GetString("rssAlt", this.LocalResourceFile);

                this.lnkRss.Attributes.Add("type", "application/rss+xml");

                if (this.AllowTags && this._tagQuery != null && this._tagQuery.Count > 0)
                {
                    this.lnkRss.NavigateUrl = GetRssLinkUrl(this.PortalId, "TagFeed", this._qsTags);
                    this.SetRssUrl(this.lnkRss.NavigateUrl, Localization.GetString("rssAlt", this.LocalResourceFile));
                }
                else
                {
                    // check for a setting of an external URL
                    ItemVersionSetting rssSetting = ItemVersionSetting.GetItemVersionSetting(
                        this.VersionInfoObject.ItemVersionId, "CategorySettings", "RssUrl", this.PortalId);
                    if (rssSetting != null && !string.IsNullOrEmpty(rssSetting.PropertyValue))
                    {
                        this.lnkRss.NavigateUrl = rssSetting.PropertyValue;
                        this.SetExternalRssUrl(this.lnkRss.NavigateUrl, Localization.GetString("rssAlt", this.LocalResourceFile));
                    }
                    else
                    {
                        // TODO: configure the # of items for an RSS feed
                        this.lnkRss.NavigateUrl = GetRssLinkUrl(
                            this._categoryId, 25, this._customDisplaySettings.ItemTypeId, this.PortalId, "ItemListing");
                        this.SetRssUrl(this.lnkRss.NavigateUrl, Localization.GetString("rssAlt", this.LocalResourceFile));
                    }
                }
            }

            // store the URL into session for the return to list options
            if (UseSessionForReturnToList(this.PortalId))
            {
                this.Session["PublishListLink"] = this.Request.Url.PathAndQuery;
            }

            // check if admin, enable edit links
            if ((this.IsAdmin || this.IsAuthor) && this.IsEditable)
            {
                this.Visibility = true;
                this.EditText = Localization.GetString("EditText", this.LocalResourceFile);
            }
            else
            {
                this.Visibility = false;
                this.EditText = string.Empty;
            }

            try
            {
                if (this._customDisplaySettings.ItemTypeId == -2)
                {
                    this.lblMessage.Text = Localization.GetString("SetupItemType", this.LocalResourceFile);
                    return;
                }

                this.lstItems.DataSource = this.GetData();
                this.lstItems.DataBind();

                if ((this._customDisplaySettings.ShowParent || this._customDisplaySettings.ShowParentDescription) && this._categoryId != -1)
                {
                    Category parentCategory = Category.GetCategory(this._categoryId, this.PortalId);
                    if (this._customDisplaySettings.ShowParent)
                    {
                        this.divParentCategoryName.Visible = true;
                        this.lblCategory.Text = parentCategory.Name;
                    }

                    // show the category description if enabled.
                    if (this._customDisplaySettings.ShowParentDescription)
                    {
                        this.divParentCategoryDescription.Visible = true;
                        this.lblCategoryDescription.Text = Utility.ReplaceTokens(parentCategory.Description);
                    }
                }
                else
                {
                    this.lblCategory.Visible = false;
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Record a Viewing.
        /// </summary>
        private void RecordView()
        {
            if (this.VersionInfoObject != null && !this.VersionInfoObject.IsNew)
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

                this.VersionInfoObject.AddView(
                    this.UserId, this.TabId, HttpContext.Current.Request.UserHostAddress, HttpContext.Current.Request.UserAgent, referrer, url);
            }
        }

        private string SortOrder()
        {
            string sortDirection = "Asc";
            if (this._customDisplaySettings.SortDirection == "1")
            {
                sortDirection = "Desc";
            }

            string column;
            switch (this._customDisplaySettings.SortOption)
            {
                case CustomDisplaySettings.DateSort:
                    column = "CreatedDate";
                    break;
                case CustomDisplaySettings.LastUpdatedSort:
                    column = "LastUpdated";
                    break;
                case CustomDisplaySettings.MostPopularSort:
                    column = "TimesViewed";
                    break;
                case CustomDisplaySettings.StartDateSort:
                    column = "StartDate";
                    break;

                    // case (CustomDisplaySettings.TitleSort):
                default:
                    column = "ChildName";
                    break;
            }

            return column + " " + sortDirection;
        }

        private DataTable SortTable(DataTable dt)
        {
            string sortDirection = "Asc";
            if (this._customDisplaySettings.SortDirection == "1")
            {
                sortDirection = "Desc";
            }

            string column;
            switch (this._customDisplaySettings.SortOption)
            {
                case CustomDisplaySettings.DateSort:
                    column = "CreatedDate";
                    break;
                case CustomDisplaySettings.LastUpdatedSort:
                    column = "LastUpdated";
                    break;
                case CustomDisplaySettings.StartDateSort:
                    column = "StartDate";
                    break;

                case CustomDisplaySettings.MostPopularSort:
                    column = "TimesViewed";
                    break;

                    // case (CustomDisplaySettings.TitleSort):
                default:

                    // if we're doing the title sort we don't need to pass as this is already in the function
                    column = "ChildName";
                    break;
            }

            dt.DefaultView.Sort = column + " " + sortDirection;
            return dt;
        }
    }
}