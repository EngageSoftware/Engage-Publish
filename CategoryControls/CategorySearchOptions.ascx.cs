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
                object o = Settings["csMaxResults"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    txtResults.Text = o.ToString();
                }

                o = Settings["csPerPage"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    txtPage.Text = o.ToString();
                }

                o = Settings["csTitleLength"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    txtTitle.Text = o.ToString();
                }

                o = Settings["csDescriptionLength"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    txtDescription.Text = o.ToString();
                }

                o = Settings["csSearchEmptyRedirectUrl"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    txtSearchUrl.Text = o.ToString();
                }

                ItemRelationship.DisplayCategoryHierarchy(ddlCategorySearchList, -1, PortalId, false);
                ddlCategorySearchList.Items.Insert(0, new ListItem(Localization.GetString("AllCategories", LocalResourceFile), "-1"));

                o = Settings["csCategoryId"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    ListItem li = ddlCategorySearchList.Items.FindByValue(o.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                chkDescription.Checked = false;
                o = Settings["csShowDescription"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    chkDescription.Checked = o.ToString().Equals("Y");
                }

                chkAllowCategorySelection.Checked = true;
                o = Settings["csAllowCategorySelection"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    chkAllowCategorySelection.Checked = o.ToString().Equals("Y");
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public override void UpdateSettings()
        {
            //save the new setting
            var modules = new ModuleController();
            modules.UpdateTabModuleSetting(TabModuleId, "csMaxResults", txtResults.Text);
            modules.UpdateTabModuleSetting(TabModuleId, "csPerPage", txtPage.Text);
            modules.UpdateTabModuleSetting(TabModuleId, "csTitleLength", txtTitle.Text);
            modules.UpdateTabModuleSetting(TabModuleId, "csDescriptionLength", txtDescription.Text);
            modules.UpdateTabModuleSetting(TabModuleId, "csCategoryId", ddlCategorySearchList.SelectedValue);
            modules.UpdateTabModuleSetting(TabModuleId, "csSearchEmptyRedirectUrl", txtSearchUrl.Text.Trim());
            modules.UpdateTabModuleSetting(TabModuleId, "csShowDescription", (chkDescription.Checked ? "Y" : "N"));
            modules.UpdateTabModuleSetting(TabModuleId, "csAllowCategorySelection", (chkAllowCategorySelection.Checked ? "Y" : "N"));
        }

        //#region Optional Interfaces

        //public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        //{
        //    get
        //    {
        //        DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
        //        Actions.Add(GetNextActionID(), Localization.GetString(DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", "", false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
        //        return Actions;
        //    }
        //}

       
        //#endregion


    }
}

