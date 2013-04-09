//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Framework;
    using DotNetNuke.Security;
    using DotNetNuke.Security.Permissions;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    public partial class PrinterFriendly : PageBase
    {
        public string CssStyle = "module.css";

        public static string ApplicationUrl
        {
            get { return HttpContext.Current.Request.ApplicationPath == "/" ? string.Empty : HttpContext.Current.Request.ApplicationPath; }
        }

        public int ItemId
        {
            get
            {
                object i = this.Request.Params["itemId"];
                if (i != null)
                {
                    // look up the itemType if ItemId passed in.
                    this.ItemType = Item.GetItemType(Convert.ToInt32(i, CultureInfo.InvariantCulture)).ToUpperInvariant();
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }

                return -1;
            }
        }

        public string ItemType { get; set; }

        public int PortalId
        {
            get
            {
                object i = this.Request.Params["portalId"];
                if (i != null)
                {
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }

                return -1;
            }
        }

        public int TabId
        {
            get
            {
                string value = this.Request.QueryString["TabId"];
                int tabId;
                if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out tabId))
                {
                    return tabId;
                }

                return -1;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", 
            Justification = "Method has side effect of setting public member CssStyle")]
        protected string GetCssStyle()
        {
            var ps = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            this.CssStyle = ps.ActiveTab.SkinPath + "skin.css";
            return this.CssStyle;
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Article a = Article.GetArticle(this.ItemId, this.PortalId);

                if (a != null && ((!a.ForceDisplayOnPage() && this.UserHasRights(this.TabId)) || this.UserHasRights(a.DisplayTabId)))
                {
                    this.lblArticleTitle.Text = this.pageTitle.Text = a.Name;
                    this.lblArticleText.Text = a.ArticleText.Replace("[PAGE]", string.Empty);

                    // TODO: configure this page to allow for displaying author, date, etc based on the itemversionsettings

                    // ItemVersionSetting auSetting = ItemVersionSetting.GetItemVersionSetting(article.ItemVersionId, "pnlAuthor", "Visible", PortalId);
                    // if (auSetting != null)
                    // {
                    // ShowAuthor = Convert.ToBoolean(auSetting.PropertyValue, CultureInfo.InvariantCulture);
                    // }
                    this.lnkPortalLogo.NavigateUrl = "http://" + this.PortalSettings.PortalAlias.HTTPAlias;
                    this.lnkPortalLogo.ImageUrl = this.PortalSettings.HomeDirectory + this.PortalSettings.LogoFile;

                    this.CssStyle = this.PortalSettings.ActiveTab.SkinPath + "skin.css";
                }
                else
                {
                    this.lblArticleTitle.Text = this.pageTitle.Text = Localization.GetString("Permission", this.LocalResourceFile);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessPageLoadException(exc);
            }
        }

        private bool UserHasRights(int tabId)
        {
            var modules = new ModuleController();
            Dictionary<int, ModuleInfo> tabModules = modules.GetTabModules(tabId);

            foreach (ModuleInfo module in tabModules.Values)
            {
                if (module.DesktopModule.FriendlyName.Equals(Utility.DnnFriendlyModuleName))
                {
                    bool overrideable;
                    object overrideableObj = modules.GetTabModuleSettings(module.TabModuleID)["Overrideable"];
                    if (overrideableObj != null && bool.TryParse(overrideableObj.ToString(), out overrideable) && overrideable)
                    {
                        // keep checking until there is an overrideable module that the user is authorized to see.  BD
                        if (ModulePermissionController.HasModuleAccess(SecurityAccessLevel.View, string.Empty, module))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}