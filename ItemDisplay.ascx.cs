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
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using DotNetNuke.Common;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    public partial class ItemDisplay : ModuleBase, IActionable
    {
        private string _controlToLoad;

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            this.GetNextActionID(), Localization.GetString("Administration", this.LocalSharedResourceFile), string.Empty, string.Empty
                            , string.Empty, this.EditUrl(Utility.AdminContainer), false, SecurityAccessLevel.Edit, true, false
                            }
                    };

                // actions.Add(GetNextActionID(), Localization.GetString("ClearCache", LocalSharedResourceFile), "", "", "", EditUrl(Utility.AdminContainer), false, SecurityAccessLevel.Edit, true, false);
                if (this.IsAdmin)
                {
                    actions.Add(
                        this.GetNextActionID(), 
                        Localization.GetString("ClearCache", this.LocalSharedResourceFile), 
                        string.Empty, 
                        string.Empty, 
                        "action_refresh.gif", 
                        Globals.NavigateURL(string.Empty, "&clearcache=true"), 
                        false, 
                        SecurityAccessLevel.Edit, 
                        true, 
                        false);
                }

                return actions;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CheckClearCache();
            this.SetWlwSupport();
            ReadItemType();
            LoadControlType();

            // add on a query string param so that we can grab ALL content, not just for this moduleId. hk
            string title = Localization.GetString(ModuleActionType.ExportModule, Localization.GlobalResourceFile);
            foreach (ModuleAction action in Actions)
            {
                if (action.Title == title)
                {
                    // todo: duplicate the action to allow for exporting all content, or just local
                    ModuleAction ma = new ModuleAction(
                        GetNextActionID(), 
                        action.Title, 
                        action.CommandName, 
                        action.CommandArgument, 
                        action.Icon, 
                        action.Url, 
                        action.ClientScript, 
                        action.UseActionEvent, 
                        action.Secure, 
                        action.Visible, 
                        action.NewWindow);
                    action.Url = action.Url + "?all=1";
                    action.Title = Localization.GetString("ExportAll", LocalSharedResourceFile);

                    Actions.Insert(action.ID - 1, ma);
                    break;
                }
            }
        }

        private void CheckClearCache()
        {
            if (this.IsAdmin)
            {
                object o = this.Request.QueryString["clearcache"];
                if (o != null)
                {
                    if (Convert.ToBoolean(o.ToString()))
                    {
                        Utility.ClearPublishCache(this.PortalId);
                    }

                    this.Response.Redirect(Globals.NavigateURL());
                }
            }
        }

        /// <summary>
        /// Check's to see if the item being loaded in this module should be displayed on this tabid/moduleid, if not does a 301 redirect to the proper page.
        /// </summary>
        private void CheckItemUrl()
        {
            if (this.VersionInfoObject != null)
            {
                // check to see if this Item should be redirected to a different URL
                if (Engage.Utility.HasValue(this.VersionInfoObject.Url) && (this.VersionInfoObject.Url != this.Request.Url.ToString()))
                {
                    // do our redirect now
                    this.Response.Status = "301 Moved Permanently";
                    this.Response.RedirectLocation = this.VersionInfoObject.GetItemExternalUrl;
                }

                // check if we're on the correct URL before progressing
                if (this.VersionInfoObject.ForceDisplayOnPage() && (this.TabId != this.VersionInfoObject.DisplayTabId) && !this.IsAdmin)
                {
                    this.Response.Status = "301 Moved Permanently";
                    this.Response.RedirectLocation = this.GetItemLinkUrl(this.VersionInfoObject.ItemId);
                }
                else if (this.VersionInfoObject.ForceDisplayOnPage() && (this.TabId != this.VersionInfoObject.DisplayTabId) && this.IsAdmin)
                {
                    this.lblPublishMessages.Text = Localization.GetString("PublishForceAdminMessage", this.LocalSharedResourceFile);
                    this.divPublishNotifications.Visible = true;
                }
            }
        }

        private void LoadControlType()
        {
            try
            {
                var mb = (ModuleBase)this.LoadControl(this._controlToLoad);
                mb.ModuleConfiguration = this.ModuleConfiguration;
                mb.ID = Path.GetFileNameWithoutExtension(this._controlToLoad);
                this.phControls.Controls.Add(mb);

                // Don't show the menu if we're in VIEW mode

                // Don't show the menu at the top if the control is not configured to display anything. hk
                if ((this.IsAdmin || this.IsAuthor) && mb.ItemId != -1 && this.IsEditable)
                {
                    // Don't show the menu if we're on a category search, itemlisting or CategoryNLevels control
                    string displayType = string.Empty;
                    if (this.Settings.Contains("DisplayType"))
                    {
                        displayType = this.Settings["DisplayType"].ToString();
                    }

                    if (displayType != "CategorySearch" && displayType != "ItemListing" && displayType != "CategoryNLevels")
                    {
                        const string adminControlToLoad = "Admin/AdminMenu.ascx";
                        var mbl = (ModuleBase)this.LoadControl(adminControlToLoad);
                        mbl.ModuleConfiguration = this.ModuleConfiguration;
                        mbl.ID = Path.GetFileNameWithoutExtension(adminControlToLoad);
                        this.phAdminControls.Controls.Add(mbl);
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", 
            Justification = "Code paths are easy to understand, test, and maintain")]
        private void ReadItemType()
        {
            this.BindItemData();

            // check for a valid itemid for this module
            this.CheckItemUrl();

            // here we are looking to see if any old publish URLs are being used, if so redirect to the new URL
            object oid = this.Request.Params["aid"];
            if (oid != null)
            {
                // build the full url
                // made this a 301 redirect for better SEO
                string href = ApplicationUrl + DesktopModuleFolderName + "itemlink.aspx?aid=" + oid;
                this.Response.Status = "301 Moved Permanently";
                this.Response.RedirectLocation = href;
            }

            string displayType = string.Empty;
            if (this.Settings.Contains("DisplayType"))
            {
                displayType = this.Settings["DisplayType"].ToString();
            }

            ItemType t = this.TypeOfItem;

            if (t != null)
            {
                if (t.Name.Equals("ARTICLE", StringComparison.OrdinalIgnoreCase))
                {
                    this._controlToLoad = displayType == "ArticleDisplay"
                                              ? "ArticleControls/ArticleDisplay.ascx"
                                              : "ArticleControls/ArticleDisplay.ascx";
                }
                else if (t.Name.Equals("CATEGORY", StringComparison.OrdinalIgnoreCase))
                {
                    if (displayType == "CustomDisplay")
                    {
                        this._controlToLoad = "Controls/CustomDisplay.ascx";
                    }
                    else if (displayType == "CategoryDisplay")
                    {
                        this._controlToLoad = "CategoryControls/CategoryDisplay.ascx";
                    }
                    else if (displayType == "CategoryFeatureDisplay")
                    {
                        this._controlToLoad = "CategoryControls/CategoryFeature.ascx";
                    }
                    else if (displayType == "CategorySearch")
                    {
                        this._controlToLoad = "CategoryControls/CategorySearch.ascx";
                    }
                    else if (displayType == "ItemListing")
                    {
                        this._controlToLoad = "Controls/ItemListing.ascx";
                    }
                    else if (displayType == "CategoryNLevels")
                    {
                        this._controlToLoad = "CategoryControls/CategoryNLevels.ascx";
                    }
                    else
                    {
                        this._controlToLoad = "Controls/CustomDisplay.ascx";
                    }
                }
                else if (t.Name.Equals("TOPLEVELCATEGORY", StringComparison.OrdinalIgnoreCase))
                {
                    if (displayType == "CustomDisplay")
                    {
                        this._controlToLoad = "Controls/CustomDisplay.ascx";
                    }
                    else if (displayType == "CategoryDisplay")
                    {
                        this._controlToLoad = "CategoryControls/CategoryDisplay.ascx";
                    }
                    else if (displayType == "CategoryFeatureDisplay")
                    {
                        this._controlToLoad = "CategoryControls/CategoryFeature.ascx";
                    }
                    else if (displayType == "CategorySearch")
                    {
                        this._controlToLoad = "CategoryControls/CategorySearch.ascx";
                    }
                    else if (displayType == "ItemListing")
                    {
                        this._controlToLoad = "Controls/ItemListing.ascx";
                    }
                    else if (displayType == "CategoryNLevels")
                    {
                        this._controlToLoad = "CategoryControls/CategoryNLevels.ascx";
                    }
                    else
                    {
                        this._controlToLoad = "CategoryControls/CategoryFeature.ascx";
                    }
                }
                else
                {
                    if (displayType == "CustomDisplay")
                    {
                        this._controlToLoad = "Controls/CustomDisplay.ascx";
                    }
                    else if (displayType == "ItemListing")
                    {
                        this._controlToLoad = "Controls/ItemListing.ascx";
                    }
                    else if (displayType == "CategorySearch")
                    {
                        this._controlToLoad = "CategoryControls/CategorySearch.ascx";
                    }
                    else
                    {
                        this._controlToLoad = "ArticleControls/ArticleDisplay.ascx";
                    }
                }
            }
            else
            {
                if (displayType == "CustomDisplay")
                {
                    this._controlToLoad = "Controls/CustomDisplay.ascx";
                }
                else if (displayType == "ItemListing")
                {
                    this._controlToLoad = "Controls/ItemListing.ascx";
                }
                else if (displayType == "CategorySearch")
                {
                    this._controlToLoad = "CategoryControls/CategorySearch.ascx";
                }
                else
                {
                    this._controlToLoad = "ArticleControls/ArticleDisplay.ascx";
                }
            }
        }
    }
}