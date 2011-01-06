// <copyright file="ItemLink.aspx.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2011
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using DotNetNuke.Common;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Tabs;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;

    using Engage.Dnn.Publish.Util;

    /// <summary>
    /// Links to an item based on a passed-in ID.  Deprecated
    /// </summary>
    /// <remarks>
    /// Need to determine whether or not ItemLink.aspx should/can handle versions of an item. This
    /// would help from the admin controls where we're previewing an old version of an article and that version
    /// didn't have a display page set and we can't display it. ItemPreview.ascx can handle previewing it
    /// if this page handled it correctly and passed the versionId along to the control. hk
    /// </remarks>
    public partial class ItemLink : PageBase
    {
        private int itemId;

        private string itemType;

        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);

            InitializeLocalVariables();
        }

        private void DisplayBrokenLinkMessage(Item item)
        {
            if (this.Request.UrlReferrer != null)
            {
                if (this.Request.UrlReferrer.Authority == this.Request.Url.Authority)
                {
                    // The caller is another page on this site so we can use the internal error message
                    // to the user (InvalidLink.ascx).
                    this.NavigateInvalidLinkInternal(item);
                }
                else
                {
                    this.DisplayInvalidLinkMessage();
                }
            }
        }

        private void DisplayInvalidLinkMessage()
        {
            if (this.Request.UrlReferrer != null)
            {
                string href = this.Request.UrlReferrer.OriginalString;
                this.metaRefresh.Attributes["content"] = Utility.RedirectInSeconds.ToString(CultureInfo.InvariantCulture) + ";url=" + href;
            }
        }

        private void DisplayItem(Item item)
        {
            if (item != null && !item.IsLinkable() && Null.IsNull(ModuleBase.DefaultDisplayTabIdForPortal(item.PortalId)))
            {
                this.DisplayBrokenLinkMessage(item);
            }
            else
            {
                this.Response.Status = "301 Moved Permanently";
                this.Response.RedirectLocation = item != null ? UrlGenerator.GetItemLinkUrl(item) : Globals.NavigateURL();
            }
        }

        [Obsolete("This method should somehow call ModuleBase.ItemId but because ModuleSettings for the module are needed not sure how to.", false)]
        private void InitializeLocalVariables()
        {
            string i = this.Request.QueryString["itemId"];
            string pi = this.Request.QueryString["pageid"];
            string lang = this.Request.QueryString["language"];
            string a = this.Request.QueryString["articleId"];
            string olda = this.Request.QueryString["aid"];
            string c = this.Request.QueryString["categoryId"];
            string o = this.Request.QueryString["tabid"];
            string m = this.Request.QueryString["modid"];

            if (m != null)
            {
                if (Engage.Utility.HasValue(m))
                {
                    Convert.ToInt32(m, CultureInfo.InvariantCulture);
                }
            }

            if (pi != null)
            {
                if (Engage.Utility.HasValue(pi))
                {
                    Convert.ToInt32(pi, CultureInfo.InvariantCulture);
                }
            }

            if (o != null)
            {
                if (Engage.Utility.HasValue(o))
                {
                    Convert.ToInt32(o, CultureInfo.InvariantCulture);
                }
            }

            if (lang != null)
            {
                if (Engage.Utility.HasValue(lang))
                {
                }
            }

            if (i != null)
            {
                // look up the _itemType if ItemId passed in.
                this.itemId = Convert.ToInt32(i, CultureInfo.InvariantCulture);
                this.itemType = Item.GetItemType(this.itemId).ToUpperInvariant();
            }
            else if (a != null)
            {
                this.itemType = ItemType.Article.Name.ToUpperInvariant();
                this.itemId = Convert.ToInt32(a, CultureInfo.InvariantCulture);
            }
            else if (olda != null)
            {
                this.itemType = "OLDARTICLE";
                this.itemId = Convert.ToInt32(olda, CultureInfo.InvariantCulture);
            }
            else if (c != null)
            {
                this.itemType = ItemType.Category.Name.ToUpperInvariant();
                this.itemId = Convert.ToInt32(c, CultureInfo.InvariantCulture);
            }
            else
            {
                this.itemId = -1;
            }
        }

        private void LocalizeControls()
        {
            // todo: why are these not working properly in DNN5.1?
            // lblFooter.Text = Localization.GetString("lblFooter", "app_localResources/ItemLink.aspx.resx");
            // lblMessage.Text = Localization.GetString("lblMessage", "app_localResources/ItemLink.aspx.resx");
            // lblPossible.Text = Localization.GetString("lblPossible", "app_localResources/ItemLink.aspx.resx");
            // lblPossibleA.Text = Localization.GetString("lblPossibleA", "app_localResources/ItemLink.aspx.resx");
            // lblPossibleB.Text = Localization.GetString("lblPossibleB", "app_localResources/ItemLink.aspx.resx");
            // lblTitle.Text = Localization.GetString("lblTitle", "app_localResources/ItemLink.aspx.resx");
        }

        private void NavigateInvalidLinkInternal(Item item)
        {
            // Need tab ID from referrer URL NOT the one associated with the item, that's why we're here because that
            // page/tab doesn't have an Engage Publish module on it. HK
            int start = this.Request.UrlReferrer.PathAndQuery.IndexOf("tabid/", StringComparison.OrdinalIgnoreCase) + 6;
            int end = this.Request.UrlReferrer.PathAndQuery.IndexOf("/", start, StringComparison.OrdinalIgnoreCase);
            string tabId = this.Request.UrlReferrer.PathAndQuery.Substring(start, end - start).ToUpperInvariant();

            // DotNetNuke.Entities.Portals.PortalSettings ps = Util.Utility.GetPortalSettings(item.PortalId);
            var tc = new TabController();
            TabInfo ti = tc.GetTab(Convert.ToInt32(tabId, CultureInfo.InvariantCulture), item.PortalId, false);
            var mc = new ModuleController();
            Dictionary<int, ModuleInfo> list = mc.GetTabModules(ti.TabID);
            ModuleInfo mi = null;
            foreach (ModuleInfo info in list.Values)
            {
                if (info.FriendlyName == Utility.DnnFriendlyModuleName)
                {
                    mi = info;
                    break;
                }
            }

            string path = "/tabid/" + tabId + "/ctl/ItemPreview/itemid/" + this.itemId.ToString(CultureInfo.InvariantCulture) + "/";
            if (mi != null)
            {
                path += "mid/" + mi.ModuleID;
            }

            string href = Globals.FriendlyUrl(ti, path);
            this.Response.Redirect(href, true);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.LocalizeControls();
                if (this.itemType != null)
                {
                    // TODO: we need to figure out PortalID so we can get the folloing items from Cache
                    if (this.itemType.Equals(ItemType.Category.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        // TODO: where can we get portalid from? NEED NEW METHOD - HK
                        Category category = Category.GetCategory(this.itemId);
                        this.DisplayItem(category);
                    }
                    else if (this.itemType.Equals(ItemType.Article.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        Article article = Article.GetArticle(this.itemId);
                        this.DisplayItem(article);
                    }
                    else if (this.itemType.Equals("OLDARTICLE", StringComparison.OrdinalIgnoreCase))
                    {
                        int newId = Article.GetOldArticleId(this.itemId);
                        Article article = Article.GetArticle(newId);
                        this.DisplayItem(article);
                    }
                }
            }
            catch (Exception ec)
            {
                Exceptions.ProcessPageLoadException(ec);
            }
        }
    }
}
