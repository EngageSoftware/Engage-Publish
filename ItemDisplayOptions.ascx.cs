// <copyright file="ItemDisplayOptions.ascx.cs" company="Engage Software">
// Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
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
                object o = Settings["CacheTime"];
                if (o != null)
                {
                    return Convert.ToInt32(o.ToString(), CultureInfo.InvariantCulture);
                }

                int defaultCacheSetting = ModuleBase.GetDefaultCacheSetting(PortalId);
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
                if (Page.IsPostBack == false)
                {
                    ddlChooseDisplayType.Items.Add(new ListItem(Localization.GetString("CustomDisplay", LocalResourceFile), "CustomDisplay"));
                    ddlChooseDisplayType.Items.Add(new ListItem(Localization.GetString("ArticleDisplay", LocalResourceFile), "ArticleDisplay"));
                    ddlChooseDisplayType.Items.Add(new ListItem(Localization.GetString("CategoryNLevels", LocalResourceFile), "CategoryNLevels"));
                    ddlChooseDisplayType.Items.Add(new ListItem(Localization.GetString("CategorySearch", LocalResourceFile), "CategorySearch"));
                    ddlChooseDisplayType.Items.Add(
                        new ListItem(Localization.GetString("CategoryFeatureDisplay", LocalResourceFile), "CategoryFeatureDisplay"));
                    ddlChooseDisplayType.Items.Add(new ListItem(Localization.GetString("CategoryDisplay", LocalResourceFile), "CategoryDisplay"));
                    ddlChooseDisplayType.Items.Add(new ListItem(Localization.GetString("ItemListing", LocalResourceFile), "ItemListing"));
                    ddlChooseDisplayType.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), string.Empty));

                    SetOptions();
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
                if (ddlChooseDisplayType.SelectedIndex > 0)
                {
                    modules.UpdateTabModuleSetting(TabModuleId, "DisplayType", ddlChooseDisplayType.SelectedValue);
                }

                modules.UpdateTabModuleSetting(TabModuleId, "LogBreadCrumb", (chkLogBreadcrumb.Checked ? "true" : "false"));
                modules.UpdateTabModuleSetting(TabModuleId, "Overrideable", (chkOverrideable.Checked ? "true" : "false"));
                //// && ddlChooseDisplayType.SelectedValue != "ItemListing" ? "true" : "false"));
                modules.UpdateTabModuleSetting(TabModuleId, "AllowTitleUpdate", (chkAllowTitleUpdate.Checked ? "true" : "false"));
                modules.UpdateTabModuleSetting(TabModuleId, "CacheTime", txtCacheTime.Text.Trim());

                modules.UpdateTabModuleSetting(TabModuleId, "SupportWLW", (chkEnableWLWSupport.Checked ? "true" : "false"));

                ////foreach (ModuleSettingsBase settingsControl in settingsChildren)
                ////{
                ////    settingsControl.UpdateSettings();
                ////}

                _currentSettingsBase.UpdateSettings();

                if (divArticleDisplay.Visible && divArticleDisplay.Controls.Count > 0)
                {
                    var articleOverrideSettings = divArticleDisplay.Controls[0] as ModuleSettingsBase;
                    if (articleOverrideSettings != null)
                    {
                        articleOverrideSettings.UpdateSettings();
                    }
                }

                if (divCategoryDisplay.Visible && divCategoryDisplay.Controls.Count > 0)
                {
                    var categoryOverrideSettings = divCategoryDisplay.Controls[0] as ModuleSettingsBase;
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
            Load += Page_Load;
            btnConfigure.Click += BtnConfigureClick;
            ddlChooseDisplayType.SelectedIndexChanged += DdlChooseDisplayTypeSelectedIndexChanged;

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
            return selectedDisplayType == "ArticleDisplay" 
                   || selectedDisplayType == "CategoryDisplay" 
                   || selectedDisplayType == "CustomDisplay"
                   || selectedDisplayType == "ItemListing" 
                   || string.IsNullOrEmpty(selectedDisplayType);
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
                DisplaySettingsControl();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnConfigure control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        private void BtnConfigureClick(object sender, EventArgs e)
        {
            pnlConfigureOverrideable.Visible = true;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddlChooseDisplayType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        private void DdlChooseDisplayTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            DisplaySettingsControl();
        }

        private void SetOptions()
        {
            object o = Settings["DisplayType"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                ListItem li = ddlChooseDisplayType.Items.FindByValue(Settings["DisplayType"].ToString());
                if (li != null)
                {
                    li.Selected = true;
                }
            }

            o = Settings["Overrideable"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                bool overrideable;
                if (bool.TryParse(Settings["Overrideable"].ToString(), out overrideable))
                {
                    chkOverrideable.Checked = overrideable;
                }
            }
            else
            {
                // default to checked when not set.
                chkOverrideable.Checked = true; 
            }

            o = Settings["AllowTitleUpdate"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                bool allowTitleUpdate;
                if (bool.TryParse(Settings["AllowTitleUpdate"].ToString(), out allowTitleUpdate))
                {
                    chkAllowTitleUpdate.Checked = allowTitleUpdate;
                }
            }

            o = Settings["CacheTime"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                txtCacheTime.Text = o.ToString();
            }
            else
            {
                txtCacheTime.Text = CacheTime.ToString(CultureInfo.InvariantCulture);
            }

            o = Settings["LogBreadCrumb"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                chkLogBreadcrumb.Checked = o.ToString().Equals("true");
            }

            o = Settings["SupportWLW"];
            if (o != null && !String.IsNullOrEmpty(o.ToString()))
            {
                bool supportWlw;
                if (bool.TryParse(Settings["SupportWLW"].ToString(), out supportWlw))
                {
                    chkEnableWLWSupport.Checked = supportWlw;
                }
            }
            else
            {
                // default to not checked when not set.
                chkEnableWLWSupport.Checked = false; 
            }
        }

        private void DisplaySettingsControl()
        {
            if (ddlChooseDisplayType.SelectedItem == null)
            {
                return;
            }

            if (phControls.Controls.Count > 0)
            {
                phControls.Controls.Clear();
            }

            string selectedDisplayType = ddlChooseDisplayType.SelectedValue;
            switch (selectedDisplayType)
            {
                case "CustomDisplay":
                    LoadSettingsControl("controls/CustomDisplayOptions.ascx" /*, "CustomDisplayOptions"*/);
                    break;
                case "ArticleDisplay":
                    LoadSettingsControl("articlecontrols/ArticleDisplayOptions.ascx" /*, "ArticleDisplayOptions"*/); 

                    // configure ShowArticle=true
                    break;
                case "CategoryDisplay":
                    LoadSettingsControl("categorycontrols/CategoryDisplayOptions.ascx" /*, "CategoryDisplayOptions"*/);
                    break;
                case "CategoryNLevels":
                    LoadSettingsControl("categorycontrols/CategoryNLevelsOptions.ascx" /*, "CategoryNLevelsOptions"*/);
                    break;
                case "CategoryFeatureDisplay":
                    LoadSettingsControl("categorycontrols/CategoryFeatureOptions.ascx" /*, "CategoryFeatureOptions"*/);
                    break;
                case "CategorySearch":
                    LoadSettingsControl("categorycontrols/CategorySearchOptions.ascx" /*, "CategorySearchOptions"*/);
                    break;
                case "ItemListing":
                    LoadSettingsControl("controls/ItemListingOptions.ascx" /*, "ItemListingOptions"*/);
                    break;
                default:
                    break;
            }

            if (selectedDisplayType == "CustomDisplay")
            {
                shCategoryDisplay.Visible = false;
                shCategoryDisplay.IsExpanded = false;
                divCategoryDisplay.Controls.Clear();
            }
            else
            {
                shCategoryDisplay.Visible = true;
                shCategoryDisplay.IsExpanded = true;
                divCategoryDisplay.Controls.Clear();
                divCategoryDisplay.Controls.Add(CreateSettingsControl("controls/CustomDisplayOptions.ascx"));
            }

            if (selectedDisplayType == "ArticleDisplay")
            {
                shArticleDisplay.Visible = false;
                shArticleDisplay.IsExpanded = false;
                divArticleDisplay.Controls.Clear();
            }
            else
            {
                shArticleDisplay.Visible = true;

                // only expand one of the sections if both are displayed.  BD
                // we are making Article Display the choice which is closed if both are shown, because article display doesn't do any callbacks (presently),
                // so you don't have the problem of making a settings change and then the callback un-expands what you were working on.
                shArticleDisplay.IsExpanded = !shCategoryDisplay.IsExpanded;
                divArticleDisplay.Controls.Clear();
                divArticleDisplay.Controls.Add(CreateSettingsControl("articlecontrols/ArticleDisplayOptions.ascx"));
            }

            // Currently on Article, Category and the new Custom Display (subject to change) allows override on querystring.
            bool supportsOverride = SupportsOverride(selectedDisplayType);
            chkOverrideable.Enabled = supportsOverride;
            if (!supportsOverride)
            {
                chkOverrideable.Checked = false;
            }

            btnConfigure.Visible = supportsOverride;
        }

        private void LoadSettingsControl(string controlName)
        {
            phControls.EnableViewState = false;
            _currentSettingsBase = CreateSettingsControl(controlName);
            phControls.Controls.Add(_currentSettingsBase);
        }

        private ModuleSettingsBase CreateSettingsControl(string controlName)
        {
            var settingsControl = (ModuleSettingsBase)LoadControl(controlName);
            settingsControl.ModuleConfiguration = new ModuleController().GetModule(ModuleId, TabId);

            settingsControl.ModuleId = ModuleId;
            settingsControl.TabModuleId = TabModuleId;

            settingsControl.ID = Path.GetFileNameWithoutExtension(controlName);
            settingsControl.LoadSettings();

            return settingsControl;
        }
    }
}