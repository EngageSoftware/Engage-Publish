//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Framework;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Publish.Util;
using DotNetNuke.Security;

namespace Engage.Dnn.Publish
{
    public partial class PrinterFriendly : PageBase
    {
        public string cssStyle = "module.css";

        public static string ApplicationUrl
        {
            get
            {
                return HttpContext.Current.Request.ApplicationPath.ToString() == "/" ? "" : HttpContext.Current.Request.ApplicationPath.ToString();
            }
        }

        public int ItemId
        {
            get
            {
                object i = Request.Params["itemId"];
                if (i != null)
                {
                    // look up the itemType if ItemId passed in.
                    ItemType = Item.GetItemType(Convert.ToInt32(i, CultureInfo.InvariantCulture)).ToUpperInvariant();
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }
                else
                {
                    return -1;
                }
            }
        }

        public int PortalId
        {
            get
            {
                object i = Request.Params["portalId"];
                if (i != null)
                {
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }
                else
                {
                    return -1;
                }
            }
        }

        public int TabId
        {
            get
            {
                string value = Request.QueryString["TabId"];
                int tabId;
                if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out tabId))
                {
                    return tabId;
                }
                else
                {
                    return -1;
                }
            }
        }

        private string _itemType;
        public string ItemType
        {
            get { return _itemType; }
            set { _itemType = value; }
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Article a = Article.GetArticle(ItemId, PortalId);

                if (a != null && ((!a.ForceDisplayOnPage() && UserHasRights(TabId)) || UserHasRights(a.DisplayTabId)))
                {
                    lblArticleTitle.Text = pageTitle.Text = a.Name;
                    lblArticleText.Text = a.ArticleText.Replace("[PAGE]", "");

                    //TODO: configure this page to allow for displaying author, date, etc based on the itemversionsettings

                    //ItemVersionSetting auSetting = ItemVersionSetting.GetItemVersionSetting(article.ItemVersionId, "pnlAuthor", "Visible", PortalId);
                    //if (auSetting != null)
                    //{
                    //    ShowAuthor = Convert.ToBoolean(auSetting.PropertyValue, CultureInfo.InvariantCulture);
                    //}


                    lnkPortalLogo.NavigateUrl = "http://" + PortalSettings.PortalAlias.HTTPAlias.ToString();
                    lnkPortalLogo.ImageUrl = PortalSettings.HomeDirectory + PortalSettings.LogoFile;

                    cssStyle = PortalSettings.ActiveTab.SkinPath + "skin.css";
                }
                else
                {
                    lblArticleTitle.Text = pageTitle.Text = Localization.GetString("Permission", LocalResourceFile);
                }
            }
            catch (Exception exc)
            {
                DotNetNuke.Services.Exceptions.Exceptions.ProcessPageLoadException(exc);
            }
        }

        private bool UserHasRights(int tabId)
        {
            ModuleController modules = new ModuleController();
            Dictionary<int, ModuleInfo> tabModules = modules.GetTabModules(tabId);

            foreach (ModuleInfo module in tabModules.Values)
            {
                if (module.FriendlyName.Equals(Utility.DnnFriendlyModuleName))
                {
                    bool overrideable;
                    object overrideableObj = modules.GetTabModuleSettings(module.TabModuleID)["Overrideable"];
                    if (overrideableObj != null && bool.TryParse(overrideableObj.ToString(), out overrideable) && overrideable)
                    {
                        //keep checking until there is an overrideable module that the user is authorized to see.  BD
                        if (PortalSecurity.HasNecessaryPermission(SecurityAccessLevel.View, PortalSettings, module))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method has side effect of setting public member cssStyle")]
        protected string GetCssStyle()
        {
            PortalSettings ps = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            cssStyle = ps.ActiveTab.SkinPath + "skin.css";
            return cssStyle;
        }
    }
}
