//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.CategoryControls
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    public partial class CategoryFeature : ModuleBase, IActionable
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

        public int SetCategoryId { get; set; }

        private ArticleViewOption DisplayOption
        {
            get
            {
                object o = this.Settings["cfDisplayOption"];
                if (o != null && Enum.IsDefined(typeof(ArticleViewOption), o))
                {
                    return (ArticleViewOption)Enum.Parse(typeof(ArticleViewOption), o.ToString());
                }

                return ArticleViewOption.Abstract;
            }
        }

        private bool EnableRss
        {
            get
            {
                object o = this.Settings["cfEnableRss"];
                return o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }
        }

        private bool RandomArticle
        {
            get
            {
                object o = this.Settings["cfRandomize"];
                return o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }
        }

        public string GetThumb(string fileName)
        {
            return String.IsNullOrEmpty(fileName) ? string.Empty : this.GetThumbnailUrl(fileName);
        }

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);

            if (ItemId == -1)
            {
                this.VersionInfoObject = Category.Create(PortalId);
            }
            else
            {
                this.VersionInfoObject = Category.GetCategory(ItemId, PortalId);
            }

            if (ItemId > 0 && (Item.GetItemType(ItemId, PortalId) == "Category" || Item.GetItemType(ItemId, PortalId) == "TopLevelCategory"))
            {
                this.VersionInfoObject = Category.GetCategory(ItemId, PortalId);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member", 
            Justification = "Controls use lower case prefix (Not an acronym)")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void dlItems_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e != null)
            {
                Panel pnlThumbnail = (Panel)e.Item.FindControl("pnlThumbnail");
                Panel pnlReadMore = (Panel)e.Item.FindControl("pnlReadMore");
                Literal lblDescription = (Literal)e.Item.FindControl("lblDescription");
                HyperLink lnkName = (HyperLink)e.Item.FindControl("lnkName");
                HyperLink imgThumbnail = (HyperLink)e.Item.FindControl("imgThumbnail");

                if (pnlThumbnail != null)
                {
                    pnlThumbnail.Visible = DisplayOption == ArticleViewOption.TitleAndThumbnail || DisplayOption == ArticleViewOption.Thumbnail;
                }

                if (lblDescription != null)
                {
                    lblDescription.Visible = DisplayOption == ArticleViewOption.Abstract || DisplayOption == ArticleViewOption.Thumbnail;
                }

                if (pnlReadMore != null)
                {
                    pnlReadMore.Visible = DisplayOption == ArticleViewOption.Abstract || DisplayOption == ArticleViewOption.Thumbnail;
                }

                ////This code makes sure that an article that is disabled does not get a link.
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DataRow dr = ((DataRowView)e.Item.DataItem).Row;
                    int linkedItemId = (int)dr["ItemId"];

                    if (Engage.Dnn.Publish.Util.Utility.IsDisabled(linkedItemId, PortalId))
                    {
                        lnkName.NavigateUrl = string.Empty;
                        imgThumbnail.NavigateUrl = string.Empty;
                        pnlReadMore.Visible = false;
                    }

                    // if (pnlThumbnail != null && (dr["Thumbnail"] == null || !Utility.HasValue(dr["Thumbnail"].ToString())))
                    // {
                    // pnlThumbnail.CssClass += " item_listing_nothumbnail";
                    // }
                }
            }
        }

        private void BindData()
        {
            if (!this.VersionInfoObject.IsNew)
            {
                // get relationship type based on selection
                int relationshipTypeId = RelationshipType.ItemToFeaturedItem.GetId();

                if (this.EnableRss)
                {
                    this.lnkRss.Visible = true;

                    string rssImage = Localization.GetString("rssImage", this.LocalResourceFile);
#if DEBUG
                    rssImage = rssImage.Replace("[L]", string.Empty);
#endif

                    this.lnkRss.ImageUrl = ApplicationUrl + rssImage; // "/images/xml.gif";
                    this.lnkRss.Attributes.Add("alt", Localization.GetString("rssAlt", this.LocalResourceFile));
                    var rssUrl = new StringBuilder();
                    rssUrl.Append(ApplicationUrl);
                    rssUrl.Append(DesktopModuleFolderName);
                    rssUrl.Append("epRss.aspx?");
                    rssUrl.AppendFormat("ItemId={0}", this.VersionInfoObject.ItemId);
                    rssUrl.AppendFormat("&RelationshipTypeId={0}&PortalId={1}&DisplayType=CategoryFeature", relationshipTypeId, this.PortalId);
                    this.lnkRss.NavigateUrl = rssUrl.ToString();
                    this.SetRssUrl(this.lnkRss.NavigateUrl, Localization.GetString("rssText", this.LocalResourceFile));
                }

                // get initial data set
                string cacheKey = Utility.CacheKeyPublishCategoryFeature + this.VersionInfoObject.ItemId.ToString(CultureInfo.InvariantCulture);
                    
                    // +"PageId";
                var dv = DataCache.GetCache(cacheKey) as DataView;

                if (dv == null)
                {
                    DataSet dsp = Item.GetParentItems(this.VersionInfoObject.ItemId, this.PortalId, relationshipTypeId);
                    dv = dsp.Tables[0].DefaultView;
                    if (dv != null)
                    {
                        DataCache.SetCache(cacheKey, dv, DateTime.Now.AddMinutes(this.CacheTime));
                        Utility.AddCacheKey(cacheKey, this.PortalId);
                    }
                }

                this.dlItems.DataSource = this.RandomArticle ? Utility.GetRandomItem(dv) : dv;
                this.dlItems.DataBind();
            }

            if (this.VersionInfoObject.IsNew && this.IsAdmin)
            {
                // based on the user display a message (admin only))
                this.lblNoData.Text = Localization.GetString("NoApprovedVersion", this.LocalResourceFile);
                this.lblNoData.Visible = true;

                // this.dlCategories.Visible = false;
                // this.dlItems.Visible = false;
            }
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.BindItemData();
                this.BindData();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}