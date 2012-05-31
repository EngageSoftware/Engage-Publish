// <copyright file="CategoryFeature.ascx.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2012
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
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;
    using System.Web.UI.WebControls;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;

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
                            this.GetNextActionID(),
                            this.Localize("Settings"),
                            ModuleActionType.AddContent,
                            string.Empty,
                            string.Empty,
                            this.EditUrl("Settings"),
                            false,
                            SecurityAccessLevel.Edit,
                            true,
                            false,
                        },
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
                return o != null && Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }
        }

        private bool RandomArticle
        {
            get
            {
                object o = this.Settings["cfRandomize"];
                return o != null && Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }
        }

        public string GetThumb(string fileName)
        {
            return string.IsNullOrEmpty(fileName) ? string.Empty : this.GetThumbnailUrl(fileName);
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);

            this.VersionInfoObject = this.ItemId == -1
                                         ? Category.Create(this.PortalId)
                                         : Category.GetCategory(this.ItemId, this.PortalId);

            if (ItemId > 0 && (Item.GetItemType(ItemId, PortalId) == "Category" || Item.GetItemType(ItemId, PortalId) == "TopLevelCategory"))
            {
                this.VersionInfoObject = Category.GetCategory(ItemId, PortalId);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member", Justification = "Controls use lower case prefix (Not an acronym)")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void dlItems_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            var pnlThumbnail = (Panel)e.Item.FindControl("pnlThumbnail");
            var pnlReadMore = (Panel)e.Item.FindControl("pnlReadMore");
            var lblDescription = (Literal)e.Item.FindControl("lblDescription");
            var lnkName = (HyperLink)e.Item.FindControl("lnkName");
            var imgThumbnail = (HyperLink)e.Item.FindControl("imgThumbnail");

            if (pnlThumbnail != null)
            {
                pnlThumbnail.Visible = this.DisplayOption == ArticleViewOption.TitleAndThumbnail || this.DisplayOption == ArticleViewOption.Thumbnail;
            }

            if (lblDescription != null)
            {
                lblDescription.Visible = this.DisplayOption == ArticleViewOption.Abstract || this.DisplayOption == ArticleViewOption.Thumbnail;
            }

            if (pnlReadMore != null)
            {
                pnlReadMore.Visible = this.DisplayOption == ArticleViewOption.Abstract || this.DisplayOption == ArticleViewOption.Thumbnail;
            }

            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
            {
                return;
            }

            // This code makes sure that an article that is disabled does not get a link.
            var dr = ((DataRowView)e.Item.DataItem).Row;
            var linkedItemId = (int)dr["ItemId"];

            if (!Utility.IsDisabled(linkedItemId, this.PortalId))
            {
                return;
            }

            lnkName.NavigateUrl = string.Empty;
            imgThumbnail.NavigateUrl = string.Empty;
            pnlReadMore.Visible = false;
        }

        private void BindData()
        {
            if (!this.VersionInfoObject.IsNew)
            {
                // get relationship type based on selection
                var relationshipTypeId = RelationshipType.ItemToFeaturedItem.GetId();

                if (this.EnableRss)
                {
                    this.lnkRss.Visible = true;

                    string rssImage = this.Localize("rssImage");
#if DEBUG
                    rssImage = rssImage.Replace("[L]", string.Empty);
#endif

                    this.lnkRss.ImageUrl = ApplicationUrl + rssImage; // "/images/xml.gif";
                    this.lnkRss.Attributes.Add("alt", this.Localize("rssAlt"));
                    this.lnkRss.NavigateUrl = new StringBuilder(ApplicationUrl)
                        .Append(DesktopModuleFolderName)
                        .Append("epRss.aspx?")
                        .AppendFormat("ItemId={0}", this.VersionInfoObject.ItemId)
                        .AppendFormat("&RelationshipTypeId={0}&PortalId={1}&DisplayType=CategoryFeature", relationshipTypeId, this.PortalId)
                        .ToString();
                    this.SetRssUrl(this.lnkRss.NavigateUrl, this.Localize("rssText"));
                }

                // get initial data set
                var cacheKey = Utility.CacheKeyPublishCategoryFeature + this.VersionInfoObject.ItemId.ToString(CultureInfo.InvariantCulture);
                    
                    // +"PageId";
                var dv = Utility.GetValueFromCache(this.PortalId, cacheKey, () => 
                    Item.GetParentItems(this.VersionInfoObject.ItemId, this.PortalId, relationshipTypeId).Tables[0].DefaultView);
                this.dlItems.DataSource = this.RandomArticle ? Utility.GetRandomItem(dv) : dv;
                this.dlItems.DataBind();
            }

            if (!this.VersionInfoObject.IsNew || !this.IsAdmin)
            {
                return;
            }

            // based on the user display a message (admin only))
            this.lblNoData.Text = this.Localize("NoApprovedVersion");
            this.lblNoData.Visible = true;
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