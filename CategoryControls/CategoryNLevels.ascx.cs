// <copyright file="CategoryNLevels.ascx.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2010
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

    using TreeNode = System.Windows.Forms.TreeNode;

    /// <summary>
    /// Displays items in a nested, unordered list
    /// </summary>
    public partial class CategoryNLevels : ModuleBase, IActionable
    {
        /// <summary>
        /// The CSS Class added to highlight the current item
        /// </summary>
        private const string HighlightCssClass = "NLevelCurrentItem";

        /// <summary>
        /// Whether to highlight the current item with the <see cref="HighlightCssClass"/>
        /// </summary>
        private bool highlightCurrentItem;

        /// <summary>
        /// The number of levels to display, or <c>-1</c> to display all levels
        /// </summary>
        private int numberOfLevels = -1;

        /// <summary>
        /// Whether to show the parent item at the top level or only show the children
        /// </summary>
        private bool showParentItem = true;

        /// <summary>
        /// Gets the list of actions implemented by this module.
        /// </summary>
        /// <value>The module's actions.</value>
        public ModuleActionCollection ModuleActions
        {
            get
            {
                return new ModuleActionCollection
                    {
                        {
                            this.GetNextActionID(), 
                            Localization.GetString("Settings", this.LocalResourceFile), 
                            ModuleActionType.AddContent, 
                            string.Empty, 
                            string.Empty, 
                            this.EditUrl("Settings"), 
                            false, 
                            SecurityAccessLevel.Edit, 
                            true, 
                            false
                        }
                    };
            }
        }

        public int SetCategoryId { get; set; }

        /// <remarks>
        /// This method is exclusively used for cases where there is no "context" for the Category that a user has
        /// linked to. If this is set to true, when the control loads both child categories and child articles are displayed. hk
        /// </remarks>
        public bool ShowAll { get; set; }

        /// <summary>
        /// Raises the <see cref="Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);

            this.BindItemData();
            this.SetupOptions();
            this.SetPageTitle();
        }

        private void BindData()
        {
            // TODO: NLevels number of items is not currently used

            // ToDO: figure out how to get the treeview to use the sorted datatable
            string cacheKey = Utility.CacheKeyPublishCategoryNLevels + this.ItemId; // +"PageId";
            var root = DataCache.GetCache(cacheKey) as TreeNode;

            if (root == null)
            {
                root = ItemRelationship.GetAllChildrenNLevels(this.ItemId, this.numberOfLevels, -1, this.PortalId);
                if (root != null)
                {
                    DataCache.SetCache(cacheKey, root, DateTime.Now.AddMinutes(this.CacheTime));
                    Utility.AddCacheKey(cacheKey, this.PortalId);
                }
            }

            // add the parent category to the list first.
            if (this.showParentItem)
            {
                Category parentItem = Category.GetCategory(this.ItemId, this.PortalId);

                if (parentItem != null)
                {
                    this.phNLevels.Controls.Add(new LiteralControl("<ul>"));
                    this.phNLevels.Controls.Add(new LiteralControl("<li>"));

                    var parentLink = new HyperLink();

                    if (parentItem.Disabled == false)
                    {
                        parentLink.NavigateUrl = this.GetItemLinkUrl(parentItem.ItemId);
                    }

                    parentLink.Text = parentItem.Name;

                    if (this.highlightCurrentItem)
                    {
                        int itemId; // =0;
                        object o = this.Request.QueryString["ItemId"];
                        if (o != null && int.TryParse(o.ToString(), out itemId))
                        {
                            if (itemId == parentItem.ItemId)
                            {
                                parentLink.CssClass = HighlightCssClass;
                            }
                        }
                    }

                    this.phNLevels.Controls.Add(parentLink);
                    this.phNLevels.Controls.Add(new LiteralControl("</li>"));

                    this.FillNLevelList(root, this.phNLevels);
                    this.phNLevels.Controls.Add(new LiteralControl("</ul>"));
                }
                else
                {
                    this.FillNLevelList(root, this.phNLevels);
                }
            }
            else
            {
                this.FillNLevelList(root, this.phNLevels);
            }
        }

        private void FillNLevelList(TreeNode root, PlaceHolder ph)
        {
            for (int i = 0; i < root.Nodes.Count; i++)
            {
                TreeNode child = root.Nodes[i];
                if (child.Text.Length > 0)
                {
                    var hl = new HyperLink();
                    int itemId = Convert.ToInt32(child.Tag, CultureInfo.InvariantCulture);
                    var it = Item.GetItemType(itemId);

                    if (!Utility.IsDisabled(itemId, this.PortalId))
                    {
                        Item curItem = BindItemData(itemId);
                        hl.NavigateUrl = this.GetItemLinkUrl(itemId);
                        if (curItem.NewWindow)
                        {
                            hl.Target = "_blank";
                        }
                    }

                    hl.Text = child.Text;

                    ph.Controls.Add(new LiteralControl("<li class=\"" + it + "\">"));

                    // check if we should hightlight the current item, if so set the CSSClass
                    if (this.highlightCurrentItem)
                    {
                        int querystringItemId;
                        object o = this.Request.QueryString["ItemId"];
                        if (o != null && int.TryParse(o.ToString(), out querystringItemId))
                        {
                            if (querystringItemId == itemId)
                            {
                                hl.CssClass = HighlightCssClass;
                            }
                        }
                    }

                    ph.Controls.Add(hl);

                    if (child.Nodes.Count > 0)
                    {
                        ph.Controls.Add(new LiteralControl("<ul>"));
                        this.FillNLevelList(child, ph);
                        ph.Controls.Add(new LiteralControl("</ul>"));
                        child.Nodes.Clear();
                    }

                    ph.Controls.Add(new LiteralControl("</li>"));
                }

                if (child.Nodes.Count > 0)
                {
                    ph.Controls.Add(new LiteralControl("<ul class=\"EP_N_UL\">"));
                    this.FillNLevelList(child, ph);
                    ph.Controls.Add(new LiteralControl("</ul>"));
                }
            }
        }

        /// <summary>
        /// Handles the <see cref="Control.Load"/> event of this control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.RecordView();

                // N Levels M Items
                this.BindData();
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

        private void SetupOptions()
        {
            object o = this.Settings["nLevels"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                this.numberOfLevels = Convert.ToInt32(o, CultureInfo.InvariantCulture);
            }

            // object m = Settings["mItems"];
            // if (m != null && !String.IsNullOrEmpty(m.ToString()))
            // {
            // mItems = Convert.ToInt32(m, CultureInfo.InvariantCulture);
            // }
            o = this.Settings["HighlightCurrentItem"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                this.highlightCurrentItem = Convert.ToBoolean(o.ToString(), CultureInfo.InvariantCulture);
            }

            o = this.Settings["ShowParentItem"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                this.showParentItem = Convert.ToBoolean(o.ToString(), CultureInfo.InvariantCulture);
            }
        }
    }
}