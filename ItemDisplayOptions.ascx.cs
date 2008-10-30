//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.IO;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using System.Globalization;
using Engage.Dnn.Publish.ArticleControls;

namespace Engage.Dnn.Publish
{
    public partial class ItemDisplayOptions : ModuleSettingsBase
    {   
        //private List<ModuleSettingsBase> settingsChildren = new List<ModuleSettingsBase>();
        private ModuleSettingsBase currentSettingsBase;

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.Load += ItemDisplayOptions_Load;
            if (DotNetNuke.Framework.AJAX.IsInstalled())
            {
                DotNetNuke.Framework.AJAX.RegisterScriptManager();
            }
        }

        protected void ItemDisplayOptions_Load(object sender, EventArgs e)
        {
            try
            {
                //by now ViewState has been restored so we can set the Settings control.
                DisplaySettingsControl();
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
                ModuleController modules = new ModuleController();
                if (this.ddlChooseDisplayType.SelectedIndex > 0)
                {
                    modules.UpdateTabModuleSetting(this.TabModuleId, "DisplayType", ddlChooseDisplayType.SelectedValue.ToString());
                }

                modules.UpdateTabModuleSetting(this.TabModuleId, "LogBreadCrumb", (chkLogBreadcrumb.Checked ? "true" : "false"));
                modules.UpdateTabModuleSetting(this.TabModuleId, "Overrideable", (chkOverrideable.Checked ? "true" : "false"));// && ddlChooseDisplayType.SelectedValue != "ItemListing" ? "true" : "false"));
                modules.UpdateTabModuleSetting(this.TabModuleId, "AllowTitleUpdate", (chkAllowTitleUpdate.Checked ? "true" : "false"));
                modules.UpdateTabModuleSetting(this.TabModuleId, "CacheTime", (txtCacheTime.Text.Trim()));

                modules.UpdateTabModuleSetting(this.TabModuleId, "SupportWLW", (chkEnableWLWSupport.Checked ? "true" : "false"));
                
                //foreach (ModuleSettingsBase settingsControl in settingsChildren)
                //{
                //    settingsControl.UpdateSettings();
                //}

                currentSettingsBase.UpdateSettings();

                if (divArticleDisplay.Visible && divArticleDisplay.Controls.Count > 0)
                {
                    ModuleSettingsBase articleOverrideSettings = divArticleDisplay.Controls[0] as ModuleSettingsBase;
                    if (articleOverrideSettings != null)
                    {
                        articleOverrideSettings.UpdateSettings();
                    }
                }
                if (divCategoryDisplay.Visible && divCategoryDisplay.Controls.Count > 0)
                {
                    ModuleSettingsBase categoryOverrideSettings = divCategoryDisplay.Controls[0] as ModuleSettingsBase;
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
                    ddlChooseDisplayType.Items.Add(new ListItem(Localization.GetString("CategoryFeatureDisplay", LocalResourceFile), "CategoryFeatureDisplay"));
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
                chkOverrideable.Checked = true;     //default to checked when not set.
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
                bool supportWLW;
                if (bool.TryParse(Settings["SupportWLW"].ToString(), out supportWLW))
                {
                    chkEnableWLWSupport.Checked = supportWLW;
                }
            }
            else
            {
                chkEnableWLWSupport.Checked = false;     //default to not checked when not set.
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void ddlChooseDisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplaySettingsControl();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void btnConfigure_Click(object sender, EventArgs e)
        {
            pnlConfigureOverrideable.Visible = true;
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
                    LoadSettingsControl("controls/CustomDisplayOptions.ascx"/*, "CustomDisplayOptions"*/);
                    break;
                case "ArticleDisplay":
                    LoadSettingsControl("articlecontrols/ArticleDisplayOptions.ascx",true);///*, "ArticleDisplayOptions"*/);
                    //configure ShowArticle=true
                    break;
                case "CategoryDisplay":
                    LoadSettingsControl("categorycontrols/CategoryDisplayOptions.ascx"/*, "CategoryDisplayOptions"*/);
                    break;
                case "CategoryNLevels":
                    LoadSettingsControl("categorycontrols/CategoryNLevelsOptions.ascx"/*, "CategoryNLevelsOptions"*/);
                    break;
                case "CategoryFeatureDisplay":
                    LoadSettingsControl("categorycontrols/CategoryFeatureOptions.ascx"/*, "CategoryFeatureOptions"*/);
                    break;
                case "CategorySearch":
                    LoadSettingsControl("categorycontrols/CategorySearchOptions.ascx"/*, "CategorySearchOptions"*/);
                    break;
                case "ItemListing":
                    LoadSettingsControl("controls/ItemListingOptions.ascx"/*, "ItemListingOptions"*/);
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
                //only expand one of the sections if both are displayed.  BD
                //we are making Article Display the choice which is closed if both are shown, because article display doesn't do any callbacks (presently),
                //so you don't have the problem of making a settings change and then the callback un-expands what you were working on.
                shArticleDisplay.IsExpanded = !shCategoryDisplay.IsExpanded;
                divArticleDisplay.Controls.Clear();
                divArticleDisplay.Controls.Add(CreateSettingsControl("articlecontrols/ArticleDisplayOptions.ascx"));
            }

            //Currently on Article, Category and the new Custom Display (subject to change) allows override on querystring.
            bool supportsOverride = SupportsOverride(selectedDisplayType);
            chkOverrideable.Enabled = supportsOverride;
            if (!supportsOverride)
            {
                chkOverrideable.Checked = false;
            }
            btnConfigure.Visible = supportsOverride;
        }

        private static bool SupportsOverride(string selectedDisplayType)
        {
            return selectedDisplayType == "ArticleDisplay" || selectedDisplayType == "CategoryDisplay" || selectedDisplayType == "CustomDisplay" || selectedDisplayType == "ItemListing" || string.IsNullOrEmpty(selectedDisplayType);
        }

        private void LoadSettingsControl(string controlName)
        {
            this.phControls.EnableViewState = false;

            //this.phControls.Controls.Add(new LiteralControl("<br/>"));
            //Label l = new Label();
            //l.Text = Localization.GetString(resourceSetting, LocalResourceFile);
            //l.CssClass = "Head";
            //this.phControls.Controls.Add(l);

            currentSettingsBase = CreateSettingsControl(controlName);
            this.phControls.Controls.Add(currentSettingsBase);
        }

        private void LoadSettingsControl(string controlName, bool showArticles)
        {
            this.phControls.EnableViewState = false;

            currentSettingsBase = CreateSettingsControl(controlName, showArticles);

            this.phControls.Controls.Add(currentSettingsBase);
        }


        private ModuleSettingsBase CreateSettingsControl(string controlName)
        {
            ModuleSettingsBase settingsControl = (ModuleSettingsBase)LoadControl(controlName);
            ModuleController mc = new ModuleController();
            ModuleInfo mi = mc.GetModule(ModuleId, TabId);
            settingsControl.ModuleConfiguration = mi;

            //SEE LINE BELOW remove the following two lines for 4.6 because 4.6 no longer supports setting the moduleid, you have to get it through the module configuration.
            //the following appears to work fine in 4.6.2 now
            settingsControl.ModuleId = ModuleId;
            settingsControl.TabModuleId = TabModuleId;
            
            settingsControl.ID = Path.GetFileNameWithoutExtension(controlName);
            settingsControl.LoadSettings();

            return settingsControl;
        }

        private ModuleSettingsBase CreateSettingsControl(string controlName, bool showArticles)
        {
            ModuleSettingsBase settingsControl = (ModuleSettingsBase)LoadControl(controlName);
            ModuleController mc = new ModuleController();
            ModuleInfo mi = mc.GetModule(ModuleId, TabId);
            settingsControl.ModuleConfiguration = mi;

            //SEE LINE BELOW remove the following two lines for 4.6 because 4.6 no longer supports setting the moduleid, you have to get it through the module configuration.
            //the following appears to work fine in 4.6.2 now
            settingsControl.ModuleId = ModuleId;
            settingsControl.TabModuleId = TabModuleId;

            settingsControl.ID = Path.GetFileNameWithoutExtension(controlName);

            ArticleDisplayOptions ado = (ArticleDisplayOptions)settingsControl;
            ado.ShowArticles = true;


            ado.LoadSettings();

            return ado;
        }

        //This is the cachetime used by Publish modules
        public int CacheTime
        {
            get
            {
                object o = Settings["CacheTime"];
                if (o != null)
                {
                    return Convert.ToInt32(o.ToString(), CultureInfo.InvariantCulture);
                }
                else if (ModuleBase.GetDefaultCacheSetting(PortalId) > 0)
                {
                    return ModuleBase.GetDefaultCacheSetting(PortalId);
                }
                return 0;
            }
        }

    }
}

