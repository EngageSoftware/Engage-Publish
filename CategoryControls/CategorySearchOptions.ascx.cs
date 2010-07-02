//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.CategoryControls
{
    using System;
    using System.Web.UI.WebControls;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    public partial class CategorySearchOptions : ModuleSettingsBase
    {
        public override void LoadSettings()
        {
            base.LoadSettings();
            try
            {
                object o = this.Settings["csMaxResults"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    this.txtResults.Text = o.ToString();
                }

                o = this.Settings["csPerPage"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    this.txtPage.Text = o.ToString();
                }

                o = this.Settings["csTitleLength"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    this.txtTitle.Text = o.ToString();
                }

                o = this.Settings["csDescriptionLength"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    this.txtDescription.Text = o.ToString();
                }

                o = this.Settings["csSearchEmptyRedirectUrl"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    this.txtSearchUrl.Text = o.ToString();
                }

                ItemRelationship.DisplayCategoryHierarchy(this.ddlCategorySearchList, -1, this.PortalId, false);
                this.ddlCategorySearchList.Items.Insert(0, new ListItem(Localization.GetString("AllCategories", this.LocalResourceFile), "-1"));

                o = this.Settings["csCategoryId"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    ListItem li = this.ddlCategorySearchList.Items.FindByValue(o.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                this.chkDescription.Checked = false;
                o = this.Settings["csShowDescription"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    this.chkDescription.Checked = o.ToString().Equals("Y");
                }

                this.chkAllowCategorySelection.Checked = true;
                o = this.Settings["csAllowCategorySelection"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    this.chkAllowCategorySelection.Checked = o.ToString().Equals("Y");
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public override void UpdateSettings()
        {
            // save the new setting
            var modules = new ModuleController();
            modules.UpdateTabModuleSetting(this.TabModuleId, "csMaxResults", this.txtResults.Text);
            modules.UpdateTabModuleSetting(this.TabModuleId, "csPerPage", this.txtPage.Text);
            modules.UpdateTabModuleSetting(this.TabModuleId, "csTitleLength", this.txtTitle.Text);
            modules.UpdateTabModuleSetting(this.TabModuleId, "csDescriptionLength", this.txtDescription.Text);
            modules.UpdateTabModuleSetting(this.TabModuleId, "csCategoryId", this.ddlCategorySearchList.SelectedValue);
            modules.UpdateTabModuleSetting(this.TabModuleId, "csSearchEmptyRedirectUrl", this.txtSearchUrl.Text.Trim());
            modules.UpdateTabModuleSetting(this.TabModuleId, "csShowDescription", this.chkDescription.Checked ? "Y" : "N");
            modules.UpdateTabModuleSetting(this.TabModuleId, "csAllowCategorySelection", this.chkAllowCategorySelection.Checked ? "Y" : "N");
        }

        // #region Optional Interfaces

        // public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        // {
        // get
        // {
        // DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
        // Actions.Add(GetNextActionID(), Localization.GetString(DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", "", false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
        // return Actions;
        // }
        // }

        // #endregion
    }
}