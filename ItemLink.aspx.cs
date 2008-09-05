//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Globalization;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Framework;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish
{
    //TODO: Need to determine whether or not ItemLink.aspx should/can handle versions of an item. This
    //would help from the admin controls where we're previewing an old version of an article and that version
    //didn't have a display page set and we can't display it. ItemPreview.ascx can handle previewing it
    //if this page handled it correctly and passed the versionId along to the control. hk

    public partial class ItemLink : PageBase
    {
        private int itemId;
        private string itemType;

        int tabid = -1;
        int modid = -1;
        int pageid = 1;
        string language = "";

        #region Web Form Designer generated code

        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);

            InitializeLocalVariables();

        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += this.Page_Load;
        }

        #endregion

        [Obsolete("This method should somehow call ModuleBase.ItemId but because ModuleSettings for the module are needed not sure how to.", false)]
        private void InitializeLocalVariables()
        {
            string i = Request.QueryString["itemId"];
            string pi = Request.QueryString["pageid"];
            string lang = Request.QueryString["language"];
            string a = Request.QueryString["articleId"];
            string olda = Request.QueryString["aid"];
            string c = Request.QueryString["categoryId"];
            string o = Request.QueryString["tabid"];
            string m = Request.QueryString["modid"];

            if (m != null)
            {
                if (Utility.HasValue(m.ToString()))
                {
                    modid = Convert.ToInt32(m.ToString(), CultureInfo.InvariantCulture);
                }
            }

            if (pi != null)
            {
                if (Utility.HasValue(pi.ToString()))
                {
                    pageid = Convert.ToInt32(pi.ToString(), CultureInfo.InvariantCulture);
                }
            }

            if (o != null)
            {
                if (Utility.HasValue(o.ToString()))
                {
                    tabid = Convert.ToInt32(o.ToString(), CultureInfo.InvariantCulture);
                }
            }

            if (lang != null)
            {
                if (Utility.HasValue(lang.ToString()))
                {
                    language = lang.ToString();
                }
            }

            if (i != null)
            {
                // look up the itemType if ItemId passed in.
                itemId = Convert.ToInt32(i, CultureInfo.InvariantCulture);
                itemType = Item.GetItemType(itemId).ToUpperInvariant();
            }
            else if (a != null)
            {
                itemType = ItemType.Article.Name.ToUpperInvariant();
                itemId = Convert.ToInt32(a, CultureInfo.InvariantCulture);
            }
            else if (olda != null)
            {
                itemType = "OLDARTICLE";
                itemId = Convert.ToInt32(olda, CultureInfo.InvariantCulture);
            }
            else if (c != null)
            {
                itemType = ItemType.Category.Name.ToUpperInvariant();
                itemId = Convert.ToInt32(c, CultureInfo.InvariantCulture);
            }
            else
            {
                itemId = -1;
            }
        }

        

        private void DisplayItem(Item item)
        {

            //TODO: check if item.URL is populated, if so figure out where to redirect.

            if (Utility.HasValue(item.Url))
            {
                //do our redirect now
                Response.Status = "301 Moved Permanently";
                Response.RedirectLocation = item.GetItemExternalUrl;

            }
            else
            {


                int defaultTabId = -1;
                object o = HostSettings.GetHostSetting(Utility.PublishDefaultDisplayPage + item.PortalId);
                if (o != null && Utility.HasValue(o.ToString()))
                {
                    defaultTabId = Convert.ToInt32(o, CultureInfo.InvariantCulture);
                }

                //build language parameter
                string friendlyLanguageValue = string.Empty;
                string languageValue = string.Empty;
                if (!string.IsNullOrEmpty(language))
                {
                    languageValue = "&language=" + language.ToString();
                    friendlyLanguageValue = "/language/" + language.ToString() + "/";
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
                            //Global.asax Application_BeginRequest checks for these values and will try to redirect to the non-existent page
                            if (pageName.EndsWith("install", StringComparison.CurrentCultureIgnoreCase) || pageName.EndsWith("installwizard", StringComparison.CurrentCultureIgnoreCase))
                            {
                                pageName = pageName.Substring(0, pageName.Length - 1);
                            }
                            pageName = pageName + ".aspx";

                            TabController tc = new TabController();
                            TabInfo ti;

                            //if the setting to "force display on this page" is set, be sure to send them there.
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
                                Response.Status = "301 Moved Permanently";
                                Response.RedirectLocation = DotNetNuke.Common.Globals.FriendlyUrl(ti,
                                     "/tabid/" + ti.TabID.ToString(CultureInfo.InvariantCulture) + "/itemid/"
                                     + item.ItemId.ToString(CultureInfo.InvariantCulture) + UsePageId(true), pageName);

                            }
                            else if (tabid > 0 && item.DisplayOnCurrentPage())
                            {

                                ti = tc.GetTab(tabid, item.PortalId, false);
                                if (ti.IsDeleted)
                                {
                                    ti = tc.GetTab(defaultTabId, item.PortalId, false);
                                }
                                //check if there is a ModuleID passed in the querystring, if so then send it in the querystring as well
                                if (modid > 0)
                                {

                                    Response.Status = "301 Moved Permanently";
                                    Response.RedirectLocation = DotNetNuke.Common.Globals.FriendlyUrl(ti, "/tabid/" + ti.TabID.ToString(CultureInfo.InvariantCulture) + "/itemid/" + item.ItemId.ToString(CultureInfo.InvariantCulture) + "/modid/" + modid.ToString(CultureInfo.InvariantCulture) + UsePageId(true) + friendlyLanguageValue, pageName);
                                }
                                else
                                {
                                    Response.Status = "301 Moved Permanently";
                                    Response.RedirectLocation = DotNetNuke.Common.Globals.FriendlyUrl(ti, "/tabid/" + ti.TabID.ToString(CultureInfo.InvariantCulture) + "/itemid/" + item.ItemId.ToString(CultureInfo.InvariantCulture) + UsePageId(true) + friendlyLanguageValue, pageName);
                                }
                            }
                            else
                            {
                                ti = tc.GetTab(item.DisplayTabId, item.PortalId, false);
                                if (ti.IsDeleted)
                                {
                                    ti = tc.GetTab(defaultTabId, item.PortalId, false);
                                }
                                Response.Status = "301 Moved Permanently";
                                Response.RedirectLocation = DotNetNuke.Common.Globals.FriendlyUrl(ti, "/tabid/" + ti.TabID.ToString(CultureInfo.InvariantCulture) + "/itemid/" + item.ItemId.ToString(CultureInfo.InvariantCulture) + UsePageId(true) + friendlyLanguageValue, pageName);
                            }
                        }
                        else
                        {
                            //we need to check for ForceOnCurrentPage
                            TabController tc = new TabController();
                            TabInfo ti;
                            DotNetNuke.Entities.Portals.PortalSettings ps = Utility.GetPortalSettings(item.PortalId);
                            //if we are passing in a TabId use it

                            if (item.ForceDisplayOnPage())
                            {
                                ti = tc.GetTab(item.DisplayTabId, item.PortalId, false);
                                Response.Status = "301 Moved Permanently";
                                Response.RedirectLocation = DotNetNuke.Common.Globals.NavigateURL(ti.TabID, ps, "", "itemid=" + item.ItemId.ToString(CultureInfo.InvariantCulture) + UsePageId(false) + languageValue);

                            }

                            if (tabid > 0)
                            {

                                if (modid > 0)
                                {
                                    Response.Status = "301 Moved Permanently";
                                    Response.RedirectLocation = DotNetNuke.Common.Globals.NavigateURL(tabid, ps, "", "itemid=" + item.ItemId.ToString(CultureInfo.InvariantCulture) + "&modid=" + modid.ToString(CultureInfo.InvariantCulture) + UsePageId(false) + languageValue);

                                }
                                else
                                {
                                    Response.Status = "301 Moved Permanently";
                                    Response.RedirectLocation = DotNetNuke.Common.Globals.NavigateURL(tabid, ps, "", "itemid=" + item.ItemId + UsePageId(false) + languageValue);
                                }
                            }


                            Response.Status = "301 Moved Permanently";
                            Response.RedirectLocation = DotNetNuke.Common.Globals.NavigateURL(item.DisplayTabId, ps, "", "itemid=" + item.ItemId + UsePageId(false) + languageValue);
                        }
                    }
                    else
                    {
                        //display on the current page or send them elsewhere.
                        //display broken link information
                        //DisplayBrokenLinkMessage(item);

                        if (defaultTabId > -1)
                        {
                            //send them to the Default Display Page
                            Response.Status = "301 Moved Permanently";
                            Response.RedirectLocation = DotNetNuke.Common.Globals.NavigateURL(defaultTabId, PortalSettings, "", "itemid=" + item.ItemId.ToString(CultureInfo.InvariantCulture) + UsePageId(false) + languageValue);
                        }
                        else
                        {
                            DisplayBrokenLinkMessage(item);
                        }
                    }
                }
                else
                {
                    Response.Status = "301 Moved Permanently";
                    Response.RedirectLocation = DotNetNuke.Common.Globals.NavigateURL();
                }
            }
        }

        private string UsePageId(bool friendly)
        {
            //we don't want to put Pageid in the URL if we're going for Page1
            if (pageid > 1)
            {
                if (friendly)
                {
                    return "/pageid/" + pageid.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    return "&pageid=" + pageid.ToString(CultureInfo.InvariantCulture);
                }
            }
            else return string.Empty;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                LocalizeControls();
                if (itemType != null)
                {
                    //TODO: we need to figure out PortalID so we can get the folloing items from Cache
                    if (itemType.Equals(ItemType.Category.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        //TODO: where can we get portalid from?
                        Category category = Category.GetCategory(itemId);
                        DisplayItem(category);
                    }
                    else if (itemType.Equals(ItemType.Article.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        Article article = Article.GetArticle(itemId);
                        DisplayItem(article);
                    }
                    else if (itemType.Equals("OLDARTICLE", StringComparison.OrdinalIgnoreCase))
                    {
                        int newId = Article.GetOldArticleId(itemId);
                        Article article = Article.GetArticle(newId);
                        DisplayItem(article);
                    }
                }
            }
            catch (Exception ec)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessPageLoadException(ec);
            }
        }

        private void LocalizeControls()
        {
            lblFooter.Text = Localization.GetString("lblFooter", "app_localResources/ItemLink.aspx.resx");
            lblMessage.Text = Localization.GetString("lblMessage", "app_localResources/ItemLink.aspx.resx");
            lblPossible.Text = Localization.GetString("lblPossible", "app_localResources/ItemLink.aspx.resx");
            lblPossibleA.Text = Localization.GetString("lblPossibleA", "app_localResources/ItemLink.aspx.resx");
            lblPossibleB.Text = Localization.GetString("lblPossibleB", "app_localResources/ItemLink.aspx.resx");
            lblTitle.Text = Localization.GetString("lblTitle", "app_localResources/ItemLink.aspx.resx");
        }

        private void DisplayBrokenLinkMessage(Item item)
        {
            if (Request.UrlReferrer != null)
            {
                if (Request.UrlReferrer.Authority == Request.Url.Authority)
                {
                    //The caller is another page on this site so we can use the internal error message
                    //to the user (InvalidLink.ascx).
                    NavigateInvalidLinkInternal(item);
                }
                else
                {
                    DisplayInvalidLinkMessage();
                }

            }
        }

        private void DisplayInvalidLinkMessage()
        {
            if (Request.UrlReferrer != null)
            {
                string href = Request.UrlReferrer.OriginalString;
                metaRefresh.Attributes["content"] = Utility.RedirectInSeconds.ToString(CultureInfo.InvariantCulture) + ";url=" + href;
            }
        }

        private void NavigateInvalidLinkInternal(Item item)
        {
            //Need tab ID from referrer URL NOT the one associated with the item, that's why we're here because that
            //page/tab doesn't have an Engage Publish module on it. HK
            int start = Request.UrlReferrer.PathAndQuery.IndexOf("tabid/", StringComparison.OrdinalIgnoreCase) + 6;
            int end = Request.UrlReferrer.PathAndQuery.IndexOf("/", start, StringComparison.OrdinalIgnoreCase);
            string tabId = Request.UrlReferrer.PathAndQuery.Substring(start, (end - start)).ToUpperInvariant();

            //DotNetNuke.Entities.Portals.PortalSettings ps = Util.Utility.GetPortalSettings(item.PortalId);
            TabController tc = new TabController();
            TabInfo ti = tc.GetTab(Convert.ToInt32(tabId, CultureInfo.InvariantCulture), item.PortalId, false);
            ModuleController mc = new ModuleController();
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

            string path = "/tabid/" + tabId + "/ctl/ItemPreview/itemid/" + itemId.ToString(CultureInfo.InvariantCulture) + "/";
            if (mi != null)
            {
                path += "mid/" + mi.ModuleID ;
            }
            string href = DotNetNuke.Common.Globals.FriendlyUrl(ti, path);
            Response.Redirect(href, true);
        }                         
    }
}

