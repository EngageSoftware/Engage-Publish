//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish
{
    // Need to determine whether or not ItemLink.aspx should/can handle versions of an item. This
    // would help from the admin controls where we're previewing an old version of an article and that version
    // didn't have a display page set and we can't display it. ItemPreview.ascx can handle previewing it
    // if this page handled it correctly and passed the versionId along to the control. hk

    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using DotNetNuke.Common;
    using DotNetNuke.Entities.Host;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Entities.Tabs;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;

    using Engage.Dnn.Publish.Util;

    public partial class ItemLink : PageBase
    {
        private int _itemId;

        private string _itemType;

        private string _language = string.Empty;

        private int _modid = -1;

        private int _pageid = 1;

        private int _tabid = -1;

        protected override void OnInit(EventArgs e)
        {
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            InitializeComponent();
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
            // check if item.URL is populated, if so figure out where to redirect.
            if (Utility.HasValue(item.Url))
            {
                // do our redirect now
                this.Response.Status = "301 Moved Permanently";
                this.Response.RedirectLocation = item.GetItemExternalUrl;
            }
            else
            {
                int defaultTabId = -1;
                object o = HostSettings.GetHostSetting(Utility.PublishDefaultDisplayPage + item.PortalId);
                if (o != null && Utility.HasValue(o.ToString()))
                {
                    defaultTabId = Convert.ToInt32(o, CultureInfo.InvariantCulture);
                }

                // build language parameter
                string friendlyLanguageValue = string.Empty;
                string languageValue = string.Empty;
                if (!string.IsNullOrEmpty(this._language))
                {
                    languageValue = "&language=" + this._language;
                    friendlyLanguageValue = "/language/" + this._language + "/";
                }

                if (item != null)
                {
                    if (item.IsLinkable())
                    {
                        if (HostSettings.GetHostSetting("UseFriendlyUrls") == "Y" && ModuleBase.EnablePublishFriendlyUrlsForPortal(item.PortalId))
                        {
                            string pageName = item.Name.Trim();
                            if (pageName.Length > 50)
                            {
                                pageName = item.Name.Substring(0, 50);
                            }

                            pageName = Utility.OnlyAlphanumericCharacters(pageName);

                            // Global.asax Application_BeginRequest checks for these values and will try to redirect to the non-existent page
                            if (pageName.EndsWith("install", StringComparison.CurrentCultureIgnoreCase) ||
                                pageName.EndsWith("installwizard", StringComparison.CurrentCultureIgnoreCase))
                            {
                                pageName = pageName.Substring(0, pageName.Length - 1);
                            }

                            pageName = pageName + ".aspx";

                            PortalSettings ps = Utility.GetPortalSettings(item.PortalId);

                            var tc = new TabController();
                            TabInfo ti;

                            // if the setting to "force display on this page" is set, be sure to send them there.
                            if (item.ForceDisplayOnPage())
                            {
                                ti = tc.GetTab(item.DisplayTabId, item.PortalId, false);
                                if (ti.IsDeleted)
                                {
                                    if (defaultTabId > 0)
                                    {
                                        ti = tc.GetTab(defaultTabId, item.PortalId, false);
                                    }
                                }

                                this.Response.Status = "301 Moved Permanently";
                                this.Response.RedirectLocation = Globals.FriendlyUrl(
                                    ti, 
                                    "/tabid/" + ti.TabID.ToString(CultureInfo.InvariantCulture) + "/itemid/" +
                                    item.ItemId.ToString(CultureInfo.InvariantCulture) + this.UsePageId(true), 
                                    pageName, 
                                    ps);
                            }
                            else if (this._tabid > 0 && item.DisplayOnCurrentPage())
                            {
                                ti = tc.GetTab(this._tabid, item.PortalId, false);
                                if (ti.IsDeleted)
                                {
                                    ti = tc.GetTab(defaultTabId, item.PortalId, false);
                                }

                                // check if there is a ModuleID passed in the querystring, if so then send it in the querystring as well
                                if (this._modid > 0)
                                {
                                    this.Response.Status = "301 Moved Permanently";
                                    this.Response.RedirectLocation = Globals.FriendlyUrl(
                                        ti, 
                                        "/tabid/" + ti.TabID.ToString(CultureInfo.InvariantCulture) + "/itemid/" +
                                        item.ItemId.ToString(CultureInfo.InvariantCulture) + "/modid/" +
                                        this._modid.ToString(CultureInfo.InvariantCulture) + this.UsePageId(true) + friendlyLanguageValue, 
                                        pageName, 
                                        ps);
                                }
                                else
                                {
                                    this.Response.Status = "301 Moved Permanently";
                                    this.Response.RedirectLocation = Globals.FriendlyUrl(
                                        ti, 
                                        "/tabid/" + ti.TabID.ToString(CultureInfo.InvariantCulture) + "/itemid/" +
                                        item.ItemId.ToString(CultureInfo.InvariantCulture) + this.UsePageId(true) + friendlyLanguageValue, 
                                        pageName, 
                                        ps);
                                }
                            }
                            else
                            {
                                ti = tc.GetTab(item.DisplayTabId, item.PortalId, false);
                                if (ti.IsDeleted)
                                {
                                    ti = tc.GetTab(defaultTabId, item.PortalId, false);
                                }

                                this.Response.Status = "301 Moved Permanently";
                                this.Response.RedirectLocation = Globals.FriendlyUrl(
                                    ti, 
                                    "/tabid/" + ti.TabID.ToString(CultureInfo.InvariantCulture) + "/itemid/" +
                                    item.ItemId.ToString(CultureInfo.InvariantCulture) + this.UsePageId(true) + friendlyLanguageValue, 
                                    pageName, 
                                    ps);
                            }
                        }
                        else
                        {
                            // we need to check for ForceOnCurrentPage
                            var tc = new TabController();
                            TabInfo ti;
                            PortalSettings ps = Utility.GetPortalSettings(item.PortalId);

                            // if we are passing in a TabId use it
                            if (item.ForceDisplayOnPage())
                            {
                                ti = tc.GetTab(item.DisplayTabId, item.PortalId, false);
                                this.Response.Status = "301 Moved Permanently";
                                this.Response.RedirectLocation = Globals.NavigateURL(
                                    ti.TabID, 
                                    ps, 
                                    string.Empty, 
                                    "itemid=" + item.ItemId.ToString(CultureInfo.InvariantCulture) + this.UsePageId(false) + languageValue);
                            }

                            if (this._tabid > 0)
                            {
                                if (this._modid > 0)
                                {
                                    this.Response.Status = "301 Moved Permanently";
                                    this.Response.RedirectLocation = Globals.NavigateURL(
                                        this._tabid, 
                                        ps, 
                                        string.Empty, 
                                        "itemid=" + item.ItemId.ToString(CultureInfo.InvariantCulture) + "&modid=" +
                                        this._modid.ToString(CultureInfo.InvariantCulture) + this.UsePageId(false) + languageValue);
                                }
                                else
                                {
                                    this.Response.Status = "301 Moved Permanently";
                                    this.Response.RedirectLocation = Globals.NavigateURL(
                                        this._tabid, ps, string.Empty, "itemid=" + item.ItemId + this.UsePageId(false) + languageValue);
                                }
                            }

                            this.Response.Status = "301 Moved Permanently";
                            this.Response.RedirectLocation = Globals.NavigateURL(
                                item.DisplayTabId, ps, string.Empty, "itemid=" + item.ItemId + this.UsePageId(false) + languageValue);
                        }
                    }
                    else
                    {
                        // display on the current page or send them elsewhere.
                        // display broken link information
                        // DisplayBrokenLinkMessage(item);
                        if (defaultTabId > -1)
                        {
                            // send them to the Default Display Page
                            this.Response.Status = "301 Moved Permanently";
                            this.Response.RedirectLocation = Globals.NavigateURL(
                                defaultTabId, 
                                this.PortalSettings, 
                                string.Empty, 
                                "itemid=" + item.ItemId.ToString(CultureInfo.InvariantCulture) + this.UsePageId(false) + languageValue);
                        }
                        else
                        {
                            this.DisplayBrokenLinkMessage(item);
                        }
                    }
                }
                else
                {
                    this.Response.Status = "301 Moved Permanently";
                    this.Response.RedirectLocation = Globals.NavigateURL();
                }
            }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Load += Page_Load;
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
                if (Utility.HasValue(m))
                {
                    this._modid = Convert.ToInt32(m, CultureInfo.InvariantCulture);
                }
            }

            if (pi != null)
            {
                if (Utility.HasValue(pi))
                {
                    this._pageid = Convert.ToInt32(pi, CultureInfo.InvariantCulture);
                }
            }

            if (o != null)
            {
                if (Utility.HasValue(o))
                {
                    this._tabid = Convert.ToInt32(o, CultureInfo.InvariantCulture);
                }
            }

            if (lang != null)
            {
                if (Utility.HasValue(lang))
                {
                    this._language = lang;
                }
            }

            if (i != null)
            {
                // look up the _itemType if ItemId passed in.
                this._itemId = Convert.ToInt32(i, CultureInfo.InvariantCulture);
                this._itemType = Item.GetItemType(this._itemId).ToUpperInvariant();
            }
            else if (a != null)
            {
                this._itemType = ItemType.Article.Name.ToUpperInvariant();
                this._itemId = Convert.ToInt32(a, CultureInfo.InvariantCulture);
            }
            else if (olda != null)
            {
                this._itemType = "OLDARTICLE";
                this._itemId = Convert.ToInt32(olda, CultureInfo.InvariantCulture);
            }
            else if (c != null)
            {
                this._itemType = ItemType.Category.Name.ToUpperInvariant();
                this._itemId = Convert.ToInt32(c, CultureInfo.InvariantCulture);
            }
            else
            {
                this._itemId = -1;
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

            string path = "/tabid/" + tabId + "/ctl/ItemPreview/itemid/" + this._itemId.ToString(CultureInfo.InvariantCulture) + "/";
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
                if (this._itemType != null)
                {
                    // TODO: we need to figure out PortalID so we can get the folloing items from Cache
                    if (this._itemType.Equals(ItemType.Category.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        // TODO: where can we get portalid from? NEED NEW METHOD - HK
                        Category category = Category.GetCategory(this._itemId);
                        this.DisplayItem(category);
                    }
                    else if (this._itemType.Equals(ItemType.Article.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        Article article = Article.GetArticle(this._itemId);
                        this.DisplayItem(article);
                    }
                    else if (this._itemType.Equals("OLDARTICLE", StringComparison.OrdinalIgnoreCase))
                    {
                        int newId = Article.GetOldArticleId(this._itemId);
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

        private string UsePageId(bool friendly)
        {
            // we don't want to put Pageid in the URL if we're going for Page1
            if (this._pageid > 1)
            {
                if (friendly)
                {
                    return "/pageid/" + this._pageid.ToString(CultureInfo.InvariantCulture);
                }

                return "&pageid=" + this._pageid.ToString(CultureInfo.InvariantCulture);
            }

            return string.Empty;
        }
    }
}