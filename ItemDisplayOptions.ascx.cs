// <copyright file="ItemDisplayOptions.ascx.cs" company="Engage Software">
// Engage: Publish - http://www.engagesoftware.com
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
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    /// <summary>
    /// The main settings for Engage: Publish, loads the settings controls for the chosen display type
    /// </summary>
    public partial class ItemDisplayOptions : ModuleSettingsBase
    {
        private ModuleSettingsBase _currentSettingsBase;

        public int CacheTime
        {
            get
            {
                object o = this.Settings["CacheTime"];
                if (o != null)
                {
                    return Convert.ToInt32(o.ToString(), CultureInfo.InvariantCulture);
                }

                int defaultCacheSetting = ModuleBase.GetDefaultCacheSetting(this.PortalId);
                if (defaultCacheSetting > 0)
                {
                    return defaultCacheSetting;
                }

                return 0;
            }
        }

        public override void LoadSettings()
        {
            base.LoadSettings();
            try
            {
                if (this.Page.IsPostBack == false)
                {
                    this.ddlChooseDisplayType.Items.Add(
                        new ListItem(Localization.GetString("CustomDisplay", this.LocalResourceFile), "CustomDisplay"));
                    this.ddlChooseDisplayType.Items.Add(
                        new ListItem(Localization.GetString("ArticleDisplay", this.LocalResourceFile), "ArticleDisplay"));
                    this.ddlChooseDisplayType.Items.Add(
                        new ListItem(Localization.GetString("CategoryNLevels", this.LocalResourceFile), "CategoryNLevels"));
                    this.ddlChooseDisplayType.Items.Add(
                        new ListItem(Localization.GetString("CategorySearch", this.LocalResourceFile), "CategorySearch"));
                    this.ddlChooseDisplayType.Items.Add(
                        new ListItem(Localization.GetString("CategoryFeatureDisplay", this.LocalResourceFile), "CategoryFeatureDisplay"));
                    this.ddlChooseDisplayType.Items.Add(
                        new ListItem(Localization.GetString("CategoryDisplay", this.LocalResourceFile), "CategoryDisplay"));
                    this.ddlChooseDisplayType.Items.Add(new ListItem(Localization.GetString("ItemListing", this.LocalResourceFile), "ItemListing"));
                    this.ddlChooseDisplayType.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", this.LocalResourceFile), string.Empty));

                    this.SetOptions();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public override void UpdateSettings()
        {
            try
            {
                var modules = new ModuleController();
                if (this.ddlChooseDisplayType.SelectedIndex > 0)
                {
                    modules.UpdateTabModuleSetting(this.TabModuleId, "DisplayType", this.ddlChooseDisplayType.SelectedValue);
                }

                modules.UpdateTabModuleSetting(this.TabModuleId, "LogBreadCrumb", this.chkLogBreadcrumb.Checked ? "true" : "false");
                modules.UpdateTabModuleSetting(this.TabModuleId, "Overrideable", this.chkOverrideable.Checked ? "true" : "false");

                //// && ddlChooseDisplayType.SelectedValue != "ItemListing" ? "true" : "false"));
                modules.UpdateTabModuleSetting(this.TabModuleId, "AllowTitleUpdate", this.chkAllowTitleUpdate.Checked ? "true" : "false");
                modules.UpdateTabModuleSetting(this.TabModuleId, "CacheTime", this.txtCacheTime.Text.Trim());

                modules.UpdateTabModuleSetting(this.TabModuleId, "SupportWLW", this.chkEnableWLWSupport.Checked ? "true" : "false");

                ////foreach (ModuleSettingsBase settingsControl in settingsChildren)
                ////{
                ////    settingsControl.UpdateSettings();
                ////}
                this._currentSettingsBase.UpdateSettings();

                if (this.divArticleDisplay.Visible && this.divArticleDisplay.Controls.Count > 0)
                {
                    var articleOverrideSettings = this.divArticleDisplay.Controls[0] as ModuleSettingsBase;
                    if (articleOverrideSettings != null)
                    {
                        articleOverrideSettings.UpdateSettings();
                    }
                }

                if (this.divCategoryDisplay.Visible && this.divCategoryDisplay.Controls.Count > 0)
                {
                    var categoryOverrideSettings = this.divCategoryDisplay.Controls[0] as ModuleSettingsBase;
                    if (categoryOverrideSettings != null)
                    {
                        categoryOverrideSettings.UpdateSettings();
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += this.Page_Load;
            this.btnConfigure.Click += this.BtnConfigureClick;
            this.ddlChooseDisplayType.SelectedIndexChanged += this.DdlChooseDisplayTypeSelectedIndexChanged;
            
            var moduleCssUrl = this.ResolveUrl("Module.css");
            ((CDefault)this.Page).AddStyleSheet(Globals.CreateValidID(moduleCssUrl), moduleCssUrl, true);

            if (AJAX.IsInstalled())
            {
                AJAX.RegisterScriptManager();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the given display type supports QueryString overriding.
        /// </summary>
        /// <param name="selectedDisplayType">Display type of the selected.</param>
        /// <returns><c>true</c> if the given display type supports QueryString overriding, otherwise <c>false</c></returns>
        private static bool SupportsOverride(string selectedDisplayType)
        {
            return selectedDisplayType == "ArticleDisplay" || selectedDisplayType == "CategoryDisplay" || selectedDisplayType == "CustomDisplay" ||
                   selectedDisplayType == "ItemListing" || string.IsNullOrEmpty(selectedDisplayType);
        }

        /// <summary>
        /// Handles the Click event of the btnConfigure control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        private void BtnConfigureClick(object sender, EventArgs e)
        {
            this.pnlConfigureOverrideable.Visible = true;
        }

        private ModuleSettingsBase CreateSettingsControl(string controlName)
        {
            var settingsControl = (ModuleSettingsBase)this.LoadControl(controlName);
            settingsControl.ModuleConfiguration = new ModuleController().GetModule(this.ModuleId, this.TabId);

            settingsControl.ModuleId = this.ModuleId;
            settingsControl.TabModuleId = this.TabModuleId;

            settingsControl.ID = Path.GetFileNameWithoutExtension(controlName);

            var overrideableSettingsControl = settingsControl as OverrideableDisplayOptionsBase;
            if (overrideableSettingsControl != null)
            {
                overrideableSettingsControl.ForceSetInitialValues = true;
            }

            settingsControl.LoadSettings();

            return settingsControl;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlChooseDisplayType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        private void DdlChooseDisplayTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            this.DisplaySettingsControl();
        }

        private void DisplaySettingsControl()
        {
            if (this.ddlChooseDisplayType.SelectedItem == null)
            {
                return;
            }

            if (this.phControls.Controls.Count > 0)
            {
                this.phControls.Controls.Clear();
            }

            string selectedDisplayType = this.ddlChooseDisplayType.SelectedValue;
            switch (selectedDisplayType)
            {
                case "CustomDisplay":
                    this.LoadSettingsControl("controls/CustomDisplayOptions.ascx" /*, "CustomDisplayOptions"*/);
                    break;
                case "ArticleDisplay":
                    this.LoadSettingsControl("articlecontrols/ArticleDisplayOptions.ascx" /*, "ArticleDisplayOptions"*/);

                    // configure ShowArticle=true
                    break;
                case "CategoryDisplay":
                    this.LoadSettingsControl("categorycontrols/CategoryDisplayOptions.ascx" /*, "CategoryDisplayOptions"*/);
                    break;
                case "CategoryNLevels":
                    this.LoadSettingsControl("categorycontrols/CategoryNLevelsOptions.ascx" /*, "CategoryNLevelsOptions"*/);
                    break;
                case "CategoryFeatureDisplay":
                    this.LoadSettingsControl("categorycontrols/CategoryFeatureOptions.ascx" /*, "CategoryFeatureOptions"*/);
                    break;
                case "CategorySearch":
                    this.LoadSettingsControl("categorycontrols/CategorySearchOptions.ascx" /*, "CategorySearchOptions"*/);
                    break;
                case "ItemListing":
                    this.LoadSettingsControl("controls/ItemListingOptions.ascx" /*, "ItemListingOptions"*/);
                    break;
                default:
                    break;
            }

            if (selectedDisplayType == "CustomDisplay")
            {
                this.shCategoryDisplay.Visible = false;
                this.shCategoryDisplay.IsExpanded = false;
                this.divCategoryDisplay.Controls.Clear();
            }
            else
            {
                this.shCategoryDisplay.Visible = true;
                this.shCategoryDisplay.IsExpanded = true;
                this.divCategoryDisplay.Controls.Clear();
                this.divCategoryDisplay.Controls.Add(this.CreateSettingsControl("controls/CustomDisplayOptions.ascx"));
            }

            if (selectedDisplayType == "ArticleDisplay")
            {
                this.shArticleDisplay.Visible = false;
                this.shArticleDisplay.IsExpanded = false;
                this.divArticleDisplay.Controls.Clear();
            }
            else
            {
                this.shArticleDisplay.Visible = true;

                // only expand one of the sections if both are displayed.  BD
                // we are making Article Display the choice which is closed if both are shown, because article display doesn't do any callbacks (presently),
                // so you don't have the problem of making a settings change and then the callback un-expands what you were working on.
                this.shArticleDisplay.IsExpanded = !this.shCategoryDisplay.IsExpanded;
                this.divArticleDisplay.Controls.Clear();
                this.divArticleDisplay.Controls.Add(this.CreateSettingsControl("articlecontrols/ArticleDisplayOptions.ascx"));
            }

            // Currently on Article, Category and the new Custom Display (subject to change) allows override on querystring.
            bool supportsOverride = SupportsOverride(selectedDisplayType);
            this.chkOverrideable.Enabled = supportsOverride;
            if (!supportsOverride)
            {
                this.chkOverrideable.Checked = false;
            }

            this.btnConfigure.Visible = supportsOverride;
        }

        private void LoadSettingsControl(string controlName)
        {
            this.phControls.EnableViewState = false;
            this._currentSettingsBase = this.CreateSettingsControl(controlName);
            this.phControls.Controls.Add(this._currentSettingsBase);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // by now ViewState has been restored so we can set the Settings control.
                this.DisplaySettingsControl();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void SetOptions()
        {
            object o = this.Settings["DisplayType"];
            if (o != null && !string.IsNullOrEmpty(o.ToString()))
            {
                ListItem li = this.ddlChooseDisplayType.Items.FindByValue(this.Settings["DisplayType"].ToString());
                if (li != null)
                {
                    li.Selected = true;
                }
            }

            o = this.Settings["Overrideable"];
            if (o != null && !string.IsNullOrEmpty(o.ToString()))
            {
                bool overrideable;
                if (bool.TryParse(this.Settings["Overrideable"].ToString(), out overrideable))
                {
                    this.chkOverrideable.Checked = overrideable;
                }
            }
            else
            {
                // default to checked when not set.
                this.chkOverrideable.Checked = true;
            }

            o = this.Settings["AllowTitleUpdate"];
            if (o != null && !string.IsNullOrEmpty(o.ToString()))
            {
                bool allowTitleUpdate;
                if (bool.TryParse(this.Settings["AllowTitleUpdate"].ToString(), out allowTitleUpdate))
                {
                    this.chkAllowTitleUpdate.Checked = allowTitleUpdate;
                }
            }

            o = this.Settings["CacheTime"];
            if (o != null && !string.IsNullOrEmpty(o.ToString()))
            {
                this.txtCacheTime.Text = o.ToString();
            }
            else
            {
                this.txtCacheTime.Text = this.CacheTime.ToString(CultureInfo.InvariantCulture);
            }

            o = this.Settings["LogBreadCrumb"];
            if (o != null && !string.IsNullOrEmpty(o.ToString()))
            {
                this.chkLogBreadcrumb.Checked = o.ToString().Equals("true");
            }

            o = this.Settings["SupportWLW"];
            if (o != null && !string.IsNullOrEmpty(o.ToString()))
            {
                bool supportWlw;
                if (bool.TryParse(this.Settings["SupportWLW"].ToString(), out supportWlw))
                {
                    this.chkEnableWLWSupport.Checked = supportWlw;
                }
            }
            else
            {
                // default to not checked when not set.
                this.chkEnableWLWSupport.Checked = false;
            }
        }
    }
}