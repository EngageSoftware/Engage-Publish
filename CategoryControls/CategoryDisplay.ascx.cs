// <copyright file="CategoryDisplay.ascx.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2011
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.CategoryControls
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    public partial class CategoryDisplay : ModuleBase, IActionable
    {
        // private category id set from display loader
        private string categoryDisplayShowChild = string.Empty;

        private ArticleViewOption displayOption;

        private int itemTypeId = ItemType.Article.GetId();

        private bool showAll; // =false;

        private string sortOption = string.Empty;

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

        public int SetCategoryId { get; set; }

        /// <summary>
        /// This method is exclusively used for cases where there is no "context" for the Category that a user has 
        /// linked to. If this is set to true, when the control loads both child categories and child articles are displayed. hk
        /// </summary>
        public bool ShowAll
        {
            get { return this.showAll; }
            set { this.showAll = value; }
        }

        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.dlCategories.ItemDataBound += this.dlCategories_ItemDataBound;
            this.dlItems.ItemDataBound += this.dlItems_ItemDataBound;
            this.Load += this.Page_Load;

            base.OnInit(e);

            BindItemData();
            this.SetupOptions();
            SetPageTitle();
        }

        private void DisplayChildCategories()
        {
            DataTable dsc;
            DataView dv = null;

            string cacheKey = Utility.CacheKeyPublishCategory + "CategoryDisplayChildren" + this.VersionInfoObject.ItemId; // +"PageId";
            if (this.UseCache)
            {
                dv = DataCache.GetCache(cacheKey) as DataView;
            }

            if (dv == null)
            {
                dsc = this.itemTypeId > -1
                          ? Category.GetChildCategories(this.VersionInfoObject.ItemId, this.PortalId, this.itemTypeId)
                          : Category.GetChildCategories(this.VersionInfoObject.ItemId, this.PortalId);
                dv = dsc.DefaultView;

                // Set the object into cache
                if (dv != null)
                {
                    DataCache.SetCache(cacheKey, dv, DateTime.Now.AddMinutes(this.CacheTime));
                    Utility.AddCacheKey(cacheKey, this.PortalId);
                }
            }

            if (dv != null)
            {
                dv.Sort = this.GetSortOrder();
                this.dlCategories.DataSource = dv;
            }

            this.dlCategories.DataBind();
        }

        private void DisplayItems(int relationshipTypeId, int otherRelationshipTypeId)
        {
            DataSet dsp;
            if (!this.VersionInfoObject.IsNew)
            {
                DataView dv = null;

                string cacheKey = Utility.CacheKeyPublishCategoryDisplay + this.VersionInfoObject.ItemId.ToString(CultureInfo.InvariantCulture);
                    
                    // +"PageId";
                if (this.UseCache)
                {
                    dv = DataCache.GetCache(cacheKey) as DataView;
                }

                if (dv == null)
                {
                    dsp = this.itemTypeId > -1
                              ? Item.GetItems(
                                  this.VersionInfoObject.ItemId, this.PortalId, relationshipTypeId, otherRelationshipTypeId, this.itemTypeId)
                              : Item.GetItems(this.VersionInfoObject.ItemId, this.PortalId, relationshipTypeId, otherRelationshipTypeId, -1);

                    dv = dsp.Tables[0].DefaultView;

                    if (dv != null)
                    {
                        DataCache.SetCache(cacheKey, dv, DateTime.Now.AddMinutes(this.CacheTime));
                        Utility.AddCacheKey(cacheKey, this.PortalId);
                    }
                }

                if (dv != null)
                {
                    dv.Sort = this.GetSortOrder();
                    this.dlItems.DataSource = dv;
                }

                this.dlItems.DataBind();
            }

            if (this.VersionInfoObject.IsNew && this.IsAdmin)
            {
                // based on the user display a message (admin only))
                this.lblNoData.Text = Localization.GetString("NoApprovedVersion", this.LocalResourceFile);
                this.lblNoData.Visible = true;
                this.dlCategories.Visible = false;
                this.dlItems.Visible = false;
            }
        }

        private void DisplayNoConfigurationView(int relationshipTypeId, int otherRelationshipTypeId)
        {
            DataSet dsp;

            DataView dv = null;

            string cacheKey = Utility.CacheKeyPublishCategory + "DisplayNoConfigurationView" + this.VersionInfoObject.ItemId; // +"PageId";
            if (this.UseCache)
            {
                dv = DataCache.GetCache(cacheKey) as DataView;
            }

            if (dv == null)
            {
                dsp = this.itemTypeId > -1
                          ? Item.GetItems(this.VersionInfoObject.ItemId, this.PortalId, relationshipTypeId, otherRelationshipTypeId, this.itemTypeId)
                          : Item.GetItems(this.VersionInfoObject.ItemId, this.PortalId, relationshipTypeId, otherRelationshipTypeId, -1);
                dv = dsp.Tables[0].DefaultView;

                if (dv != null)
                {
                    DataCache.SetCache(cacheKey, dv, DateTime.Now.AddMinutes(this.CacheTime));
                    Utility.AddCacheKey(cacheKey, this.PortalId);
                }
            }

            if (dv != null)
            {
                dv.Sort = this.GetSortOrder();

                this.dlItems.DataSource = dv;
            }

            this.dlItems.DataBind();
        }

        private string GetSortOrder()
        {
            string sort;

            if (this.sortOption == "Alpha Descending")
            {
                sort = "NAME DESC";
            }
            else if (this.sortOption == "Created Ascending")
            {
                sort = "CreatedDate ASC";
            }
            else if (this.sortOption == "Created Descending")
            {
                sort = "CreatedDate DESC";
            }
            else if (this.sortOption == "Last Updated Ascending")
            {
                sort = "LastUpdated ASC";
            }
            else if (this.sortOption == "Last Updated Descending")
            {
                sort = "LastUpdated DESC";
            }
            else
            {
                sort = "NAME ASC";
            }

            return sort;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.RecordView();

                // default to category in case no Module setting exists.
                int relationshipTypeId = RelationshipType.ItemToParentCategory.GetId();

                int otherRelationshipTypeId = RelationshipType.ItemToRelatedCategory.GetId();

                // N Levels M Items
                if (this.showAll == false)
                {
                    if (!string.IsNullOrEmpty(this.categoryDisplayShowChild))
                    {
                        if (this.categoryDisplayShowChild != "ShowAll")
                        {
                            // This method isn't currently called but when we added the multi-level option to allow
                            // user to select "n" level of categories to display, it may be used.
                            this.DisplayChildCategories();
                        }
                        else
                        {
                            // Currently, this is the ONLY method that will run since the drop down list is hidden
                            // to be Regular. 
                            this.DisplayItems(relationshipTypeId, otherRelationshipTypeId);
                        }
                    }
                    else
                    {
                        // No module setting defined for what to display. Will display base on itemTypeid if possible.
                        this.DisplayNoConfigurationView(relationshipTypeId, otherRelationshipTypeId);
                    }
                }
                else
                {
                    this.ShowNoContextView(relationshipTypeId, otherRelationshipTypeId);
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
            if (!this.VersionInfoObject.IsNew)
            {
                var referrer = string.Empty;
                if (HttpContext.Current.Request.UrlReferrer != null)
                {
                    referrer = HttpContext.Current.Request.UrlReferrer.ToString();
                }

                this.VersionInfoObject.AddView(
                    this.UserId,
                    this.TabId,
                    HttpContext.Current.Request.UserHostAddress,
                    HttpContext.Current.Request.UserAgent,
                    referrer,
                    HttpContext.Current.Request.RawUrl);
            }
        }

        private void SetupOptions()
        {
            object o = this.Settings["cdSortOption"];
            if (o != null && !string.IsNullOrEmpty(o.ToString()))
            {
                this.sortOption = o.ToString();
            }

            o = this.Settings["cdItemTypeId"];
            if (o != null && !string.IsNullOrEmpty(o.ToString()))
            {
                this.itemTypeId = Convert.ToInt32(o, CultureInfo.InvariantCulture);
            }

            o = this.Settings["cdDisplayOption"];
            if (Enum.IsDefined(typeof(ArticleViewOption), o))
            {
                this.displayOption = (ArticleViewOption)Enum.Parse(typeof(ArticleViewOption), o.ToString(), true);
            }

            o = this.Settings["cdChildDisplayOption"];
            if (o != null && !string.IsNullOrEmpty(o.ToString()))
            {
                this.categoryDisplayShowChild = o.ToString();
            }
        }

        private void ShowNoContextView(int relationshipTypeId, int otherRelationshipTypeId)
        {
            // this control shows all items under a category, categories, and articles

            // DisplayParentCategory();
            this.DisplayChildCategories();

            this.DisplayItems(relationshipTypeId, relationshipTypeId);
            this.DisplayItems(relationshipTypeId, otherRelationshipTypeId);

            var categories = (DataView)this.dlCategories.DataSource;
            var articles = (DataView)this.dlItems.DataSource;

            if ((categories.Table.Rows.Count == 0) && (articles.Table.Rows.Count == 0))
            {
                this.lblNoData.Visible = true;
                this.lblNoData.Text = Localization.GetString("NoData", this.LocalResourceFile);
            }
        }

        private void dlCategories_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DataRow dr = ((DataRowView)e.Item.DataItem).Row;
            var itemId = (int)dr["ItemId"];
            Category c = Category.GetCategory(itemId, this.PortalId);
            if (c != null)
            {
                var lnkName = (HyperLink)e.Item.FindControl("lnkName");
                if (lnkName != null)
                {
                    if (!c.Disabled)
                    {
                        lnkName.NavigateUrl = this.GetItemLinkUrl(c.ItemId);
                        if (c.NewWindow)
                        {
                            lnkName.Target = "_blank";
                        }
                    }
                }

                var lnkThumbnail = (HyperLink)e.Item.FindControl("lnkThumbnail");
                if (lnkThumbnail != null)
                {
                    // if (!Utility.HasValue(c.Thumbnail))
                    // {
                    // lnkThumbnail.CssClass += " item_listing_nothumbnail";
                    // }
                    lnkThumbnail.ImageUrl = this.GetThumbnailUrl(c.Thumbnail);
                    lnkThumbnail.Visible = this.displayOption == ArticleViewOption.Thumbnail ||
                                            this.displayOption == ArticleViewOption.TitleAndThumbnail;
                    if (!c.Disabled)
                    {
                        lnkThumbnail.NavigateUrl = this.GetItemLinkUrl(c.ItemId);
                        if (c.NewWindow)
                        {
                            lnkThumbnail.Target = "_blank";
                        }
                    }
                }

                var dlChildItems = (DataList)e.Item.FindControl("dlChildItems");
                if (dlChildItems != null)
                {
                    DataTable dsp = Article.GetArticles(itemId, this.PortalId);
                    dlChildItems.ItemDataBound += this.dlItems_ItemDataBound;
                    DataView dvp = dsp.DefaultView;
                    dvp.Sort = " Name ASC";
                    dlChildItems.DataSource = dvp;
                    dlChildItems.DataBind();
                }
            }
        }

        private void dlItems_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            // This function fires when the articles are bound
            // it builds a string of the article information within divs so that the divs can be turned off via CSS
            DataRow dr = ((DataRowView)e.Item.DataItem).Row;

            Item a = Item.GetItem((int)dr["ItemId"], this.PortalId, Item.GetItemTypeId((int)dr["ItemID"], this.PortalId), true);

            var lnkThumbnail = (HyperLink)e.Item.FindControl("lnkThumbnail");
            var lnkTitle = (HyperLink)e.Item.FindControl("lnkTitle");
            var lblDescription = (Literal)e.Item.FindControl("lblDescription");

            e.Item.CssClass = a.ItemTypeId == ItemType.Category.GetId() ? "categoryDisplayCategory" : "categoryDisplayArticle";

            if (lnkThumbnail != null)
            {
                // if (!Utility.HasValue(a.Thumbnail))
                // {
                // lnkThumbnail.CssClass += " item_listing_nothumbnail";
                // }
                lnkThumbnail.ImageUrl = this.GetThumbnailUrl(a.Thumbnail);
                lnkThumbnail.Visible = this.displayOption == ArticleViewOption.Thumbnail || this.displayOption == ArticleViewOption.TitleAndThumbnail;
                if (!a.Disabled)
                {
                    lnkThumbnail.NavigateUrl = this.GetItemLinkUrl(a.ItemId);
                }
            }

            if (lnkTitle != null)
            {
                if (!a.Disabled)
                {
                    lnkTitle.NavigateUrl = this.GetItemLinkUrl(a.ItemId);
                }
            }

            if (lblDescription != null)
            {
                lblDescription.Text = a.Description;
                lblDescription.Visible = this.displayOption == ArticleViewOption.Abstract || this.displayOption == ArticleViewOption.Thumbnail;
            }

            this.dlItems.Visible = true;
        }
    }
}