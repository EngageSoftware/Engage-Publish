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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.UI.WebControls;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Services.Exceptions;

    using Engage.Dnn.Publish.Util;

    public partial class RelatedArticleLinks : RelatedArticleLinksBase, IActionable
    {
        public bool LinksPopulated { get; set; }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection();
                return actions;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);

            this.BindItemData();
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void lstItems_DataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e != null)
            {
                var lnkRelatedArticle = (HyperLink)e.Item.FindControl("lnkRelatedArticle");

                ////This code makes sure that an article that is disabled does not get a link.
                if (e.Item.ItemType == ListItemType.Item)
                {
                    var a = (Article)e.Item.DataItem;
                    int linkedItemId = a.ItemId;

                    if (Utility.IsDisabled(linkedItemId, this.PortalId))
                    {
                        lnkRelatedArticle.NavigateUrl = string.Empty;
                    }
                }
            }
        }

        private void BtnShowRelatedItemClick(object sender, EventArgs e)
        {
            this.divRelatedLinks.Visible = true;

            Article[] related = this.VersionInfoObject.GetRelatedArticles(this.PortalId);
            if (related.Length == 0)
            {
                // what to do?
            }
            else
            {
                this.lstItems.DataSource = related;
                this.lstItems.DataBind();
            }
        }

        private void InitializeComponent()
        {
            this.btnShowRelatedItem.Click += this.BtnShowRelatedItemClick;
            this.Load += this.Page_Load;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            try
            {
                this.btnShowRelatedItem.Visible = false;
                this.divRelatedLinks.Visible = false;

                var related = new List<Article>(this.VersionInfoObject.GetRelatedArticles(this.PortalId));

                ItemVersionSetting parentRelationshipSetting = ItemVersionSetting.GetItemVersionSetting(
                    this.VersionInfoObject.ItemVersionId, "ArticleSettings", "IncludeParentCategoryArticles", this.PortalId);
                if (parentRelationshipSetting != null && Convert.ToBoolean(parentRelationshipSetting.PropertyValue, CultureInfo.InvariantCulture))
                {
                    int parentCategoryId = Category.GetParentCategory(this.VersionInfoObject.ItemId, this.PortalId);
                    if (parentCategoryId > 0)
                    {
                        // get all articles in the same category, then removes this current article from that list.  BD
                        List<Article> categoryArticles = Category.GetCategoryArticles(parentCategoryId, this.PortalId);
                        categoryArticles.RemoveAll(a => a.ItemId == this.VersionInfoObject.ItemId);
                        related.AddRange(categoryArticles);
                    }
                }

                if (related.Count < 1)
                {
                    this.btnShowRelatedItem.Visible = false;
                    this.divRelatedLinks.Visible = false;
                    this.LinksPopulated = false;
                }
                else
                {
                    this.lstItems.DataSource = related;
                    this.lstItems.DataBind();
                    this.divRelatedLinks.Visible = true;
                    this.LinksPopulated = true;
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}