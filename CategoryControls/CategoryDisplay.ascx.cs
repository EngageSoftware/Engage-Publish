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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using Engage.Dnn.Publish.Data;
using Engage.Dnn.Publish.Util;
using System.Text;

//TODO: allow for paging?
//TODO: allow sort order - possible admin function (1-10 ranking)
namespace Engage.Dnn.Publish.CategoryControls
{
    public partial class CategoryDisplay : ModuleBase, IActionable
    {
        //private category id set from display loader
        private int setCategoryId;
        private ArticleViewOption displayOption;
        private string sortOption = string.Empty;
        private int itemTypeId = ItemType.Article.GetId();
        private string categoryDisplayShowChild = string.Empty;
        private bool showAll; //=false;

        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);

            BindItemData();
            SetupOptions();
            SetPageTitle();
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dlCategories.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlCategories_ItemDataBound);
            this.dlItems.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlItems_ItemDataBound);
            this.Load += new System.EventHandler(this.Page_Load);

        }

        #endregion


        #region Event Handlers

        private void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                RecordView();

                    //default to category in case no Module setting exists.
                    int relationshipTypeId = Util.RelationshipType.ItemToParentCategory.GetId();
                
                int otherRelationshipTypeId = Util.RelationshipType.ItemToRelatedCategory.GetId();

                //N Levels M Items

                if (this.showAll == false)
                {
                    if (!String.IsNullOrEmpty(categoryDisplayShowChild))
                    {
                        if (categoryDisplayShowChild != "ShowAll")
                        {
                            //This method isn't currently called but when we added the multi-level option to allow
                            //user to select "n" level of categories to display, it may be used.
                            DisplayChildCategories();
                        }
                        else
                        {
                            //Currently, this is the ONLY method that will run since the drop down list is hidden
                            //to be Regular. 
                            DisplayItems(relationshipTypeId, otherRelationshipTypeId);
                        }
                    }
                    else
                    {
                        //No module setting defined for what to display. Will display base on itemTypeid if possible.
                        DisplayNoConfigurationView(relationshipTypeId, otherRelationshipTypeId);
                    }
                }
                else
                {
                    ShowNoContextView(relationshipTypeId, otherRelationshipTypeId);
                }

            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void dlCategories_ItemDataBound(Object sender, DataListItemEventArgs e)
        {
            DataRow dr = ((DataRowView)e.Item.DataItem).Row;
            int itemId = (int)dr["ItemId"];
            Category c = Category.GetCategory(itemId, PortalId);
            if (c != null)
            {
                HyperLink lnkName = (HyperLink)e.Item.FindControl("lnkName");
                if (lnkName != null)
                {
                    if (!c.Disabled)
                    {
                        lnkName.NavigateUrl = GetItemLinkUrl(c.ItemId);
                        if (c.NewWindow) lnkName.Target = "_blank";
                    }
                 }

                HyperLink lnkThumbnail = (HyperLink)e.Item.FindControl("lnkThumbnail");
                if (lnkThumbnail != null)
                {
                    //if (!Utility.HasValue(c.Thumbnail))
                    //{
                    //    lnkThumbnail.CssClass += " item_listing_nothumbnail";
                    //}
                    lnkThumbnail.ImageUrl = GetThumbnailUrl(c.Thumbnail);
                    lnkThumbnail.Visible = (displayOption == ArticleViewOption.Thumbnail || displayOption == ArticleViewOption.TitleAndThumbnail);
                    if (!c.Disabled)
                    {
                        lnkThumbnail.NavigateUrl = GetItemLinkUrl(c.ItemId);
                        if (c.NewWindow) lnkThumbnail.Target = "_blank";
                    }
                }

                DataList dlChildItems = (DataList)e.Item.FindControl("dlChildItems");
                if (dlChildItems != null)
                {
                    DataTable dsp = Article.GetArticles(itemId, PortalId);
                    dlChildItems.ItemDataBound += this.dlItems_ItemDataBound;
                    DataView dvp = dsp.DefaultView;
                    dvp.Sort = " Name ASC";
                    dlChildItems.DataSource = dvp;
                    dlChildItems.DataBind();
                }
            }
        }

        private void dlItems_ItemDataBound(Object sender, DataListItemEventArgs e)
        {	/* This function fires when the articles are bound
			 * it builds a string of the article information within divs so that the divs can be turned off via CSS
			 * 
			 */
            DataRow dr = ((DataRowView)e.Item.DataItem).Row;

            Item a = Item.GetItem((int)dr["ItemId"], PortalId, Item.GetItemTypeId((int)dr["ItemID"], PortalId), true);
            
            HyperLink lnkThumbnail = (HyperLink)e.Item.FindControl("lnkThumbnail");
            HyperLink lnkTitle = (HyperLink)e.Item.FindControl("lnkTitle");
            Label lblDescription = (Label)e.Item.FindControl("lblDescription");

            if (a.ItemTypeId == Util.ItemType.Category.GetId())
            {
                e.Item.CssClass = "categoryDisplayCategory";
            }
            else
            {
                e.Item.CssClass = "categoryDisplayArticle";
            }
            
            if (lnkThumbnail != null)
            {
                //if (!Utility.HasValue(a.Thumbnail))
                //{
                //    lnkThumbnail.CssClass += " item_listing_nothumbnail";
                //}
                lnkThumbnail.ImageUrl = GetThumbnailUrl(a.Thumbnail);
                lnkThumbnail.Visible = (displayOption == ArticleViewOption.Thumbnail || displayOption == ArticleViewOption.TitleAndThumbnail);
                if (!a.Disabled)
                {
                    lnkThumbnail.NavigateUrl = GetItemLinkUrl(a.ItemId);
                }
            }

            if (lnkTitle != null)
            {
                if (!a.Disabled)
                {
                    lnkTitle.NavigateUrl = GetItemLinkUrl(a.ItemId);
                }
            }

            if (lblDescription != null)
            {
                lblDescription.Text = a.Description;
                lblDescription.Visible = (displayOption == ArticleViewOption.Abstract || displayOption == ArticleViewOption.Thumbnail);
            }

            dlItems.Visible = true;
        }

        #endregion
        
        #region Interface Members

        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actions = new ModuleActionCollection();
                actions.Add(GetNextActionID(), Localization.GetString("Settings", LocalResourceFile), ModuleActionType.AddContent, "", "", EditUrl("Settings"), false, SecurityAccessLevel.Edit, true, false);
                return actions;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "0#", Justification = "Interface Implementation"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Interface Implementation"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "ModInfo", Justification = "Interface implementation")]
        public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(ModuleInfo ModInfo)
        {
            // included as a stub only so that the core knows this module Implements Entities.Modules.ISearchable
            return null;
        }



        #endregion

        //private int CategoryId
        //{
        //    get
        //    {
        //        string s = Request.QueryString["catid"];
        //        return (s == null ? -1 : Convert.ToInt32(s));
        //    }
        //}

        private void SetupOptions()
        {
            object o = Settings["cdSortOption"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                sortOption = o.ToString();
            }

            o = Settings["cdItemTypeId"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                itemTypeId = Convert.ToInt32(o, CultureInfo.InvariantCulture);
            }

            o = Settings["cdDisplayOption"];
            if (Enum.IsDefined(typeof(ArticleViewOption), o))
            {
                displayOption = (ArticleViewOption)Enum.Parse(typeof(ArticleViewOption), o.ToString(), true);
            }

            o = Settings["cdChildDisplayOption"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
             categoryDisplayShowChild = o.ToString();
            }
        }

        /// <summary>
        /// Record a Viewing.
        /// </summary>
        private void RecordView()
        {
            if (!VersionInfoObject.IsNew)
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

        public int SetCategoryId
        {
            get
            {
                return this.setCategoryId;
            }
            set
            {
                this.setCategoryId = value;
            }
        }

        /// <summary>
        /// This method is exclusively used for cases where there is no "context" for the Category that a user has 
        /// linked to. If this is set to true, when the control loads both child categories and child articles are displayed. hk
        /// </summary>
        public bool ShowAll
        {
            get
            {
                return showAll;
            }
            set
            {
                this.showAll = value;
            }
        }
        private string GetSortOrder()
        {
            string sort = string.Empty;

            if (sortOption == "Alpha Descending")
            {
                sort = "NAME DESC";
            }
            else if (sortOption == "Created Ascending")
            {
                sort = "CreatedDate ASC";
            }
            else if (sortOption == "Created Descending")
            {
                sort = "CreatedDate DESC";
            }

            else if (sortOption == "Last Updated Ascending")
            {
                sort = "LastUpdated ASC";
            }
            else if (sortOption == "Last Updated Descending")
            {
                sort = "LastUpdated DESC";
            }
            else
            {
                sort = "NAME ASC";
            }

            return sort;
        }

        private void DisplayItems(int relationshipTypeId, int otherRelationshipTypeId)
        {
            DataSet dsp = null;
            if (!VersionInfoObject.IsNew)
            {
                DataView dv = null;

                string cacheKey = Utility.CacheKeyPublishCategoryDisplay + VersionInfoObject.ItemId.ToString(CultureInfo.InvariantCulture); // +"PageId";
                
                if (UseCache) dv = DataCache.GetCache(cacheKey) as DataView;
                
                if (dv == null)
                {
                    if (itemTypeId > -1)
                    {
                        dsp = Item.GetItems(VersionInfoObject.ItemId, PortalId, relationshipTypeId, otherRelationshipTypeId, itemTypeId);
                    }
                    else
                    {
                        dsp = Item.GetItems(VersionInfoObject.ItemId, PortalId, relationshipTypeId, otherRelationshipTypeId, -1);
                    }

                    dv = dsp.Tables[0].DefaultView;
                    
                    if (dv != null)
                    {
                        DataCache.SetCache(cacheKey, dv, DateTime.Now.AddMinutes(CacheTime));
                        Utility.AddCacheKey(cacheKey, PortalId);
                    }
                }
                dv.Sort = GetSortOrder();
                dlItems.DataSource = dv;
                dlItems.DataBind();
            }

            if (VersionInfoObject.IsNew && IsAdmin)
            {
                //based on the user display a message (admin only))
                this.lblNoData.Text = Localization.GetString("NoApprovedVersion", LocalResourceFile);
                this.lblNoData.Visible = true;
                this.dlCategories.Visible = false;
                this.dlItems.Visible = false;
            }
        }

        private void DisplayNoConfigurationView(int relationshipTypeId, int otherRelationshipTypeId)
        {
            DataSet dsp = null;

            DataView dv = null;

            string cacheKey = Utility.CacheKeyPublishCategory + "DisplayNoConfigurationView" + VersionInfoObject.ItemId; // +"PageId";
            if (UseCache) dv = DataCache.GetCache(cacheKey) as DataView;

            if (dv == null)
            {
                if (itemTypeId > -1)
                {
                    dsp = Item.GetItems(VersionInfoObject.ItemId, PortalId, relationshipTypeId, otherRelationshipTypeId, itemTypeId);
                }
                else
                {
                    dsp = Item.GetItems(VersionInfoObject.ItemId, PortalId, relationshipTypeId, otherRelationshipTypeId, -1);
                }
               dv = dsp.Tables[0].DefaultView;
               

               if (dv != null)
               {
                   DataCache.SetCache(cacheKey, dv, DateTime.Now.AddMinutes(CacheTime));
                   Utility.AddCacheKey(cacheKey, PortalId);
               }
            }
            dv.Sort = GetSortOrder();

            dlItems.DataSource = dv;
            dlItems.DataBind();
        }

        private void DisplayChildCategories()
        {
            DataTable dsc = null;
            DataView dv = null;

            string cacheKey = Utility.CacheKeyPublishCategory + "CategoryDisplayChildren" + VersionInfoObject.ItemId; // +"PageId";
            if (UseCache) dv = DataCache.GetCache(cacheKey) as DataView;

            if (dv == null)
            {
                if (itemTypeId > -1)
                {
                    dsc = Category.GetChildCategories(VersionInfoObject.ItemId, PortalId, itemTypeId);
                }
                else
                {
                    dsc = Category.GetChildCategories(VersionInfoObject.ItemId, PortalId);
                }
                dv = dsc.DefaultView;

                
                //Set the object into cache
                if (dv != null)
                {
                    DataCache.SetCache(cacheKey, dv, DateTime.Now.AddMinutes(CacheTime));
                    Utility.AddCacheKey(cacheKey, PortalId);
                }

            }
            dv.Sort = GetSortOrder();
            dlCategories.DataSource = dv;
            dlCategories.DataBind();

        }

        private void ShowNoContextView(int relationshipTypeId, int otherRelationshipTypeId)
        {
            //this control shows all items under a category, categories, and articles

            //DisplayParentCategory();
            DisplayChildCategories();


            DisplayItems(relationshipTypeId, relationshipTypeId);
            DisplayItems(relationshipTypeId, otherRelationshipTypeId);

            DataView categories = (DataView)dlCategories.DataSource;
            DataView articles = (DataView)dlItems.DataSource;

            if ((categories.Table.Rows.Count == 0) && (articles.Table.Rows.Count == 0))
            {
                lblNoData.Visible = true;
                lblNoData.Text = Localization.GetString("NoData", LocalResourceFile);
            }
        }
    }
}

