//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
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
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    public partial class CategoryDisplayOptions : ModuleSettingsBase
    {
        public ModuleActionCollection ModuleActions
        {
            get
            {
                new ModuleActionCollection().Add(
                    this.GetNextActionID(), 
                    Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile), 
                    ModuleActionType.AddContent, 
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    false, 
                    SecurityAccessLevel.Edit, 
                    true, 
                    false);
                return new ModuleActionCollection();
            }
        }

        public override void LoadSettings()
        {
            try
            {
                this.ddlViewOptions.Items.Add(new ListItem(Localization.GetString("ChooseOne", this.LocalResourceFile), "-1"));
                this.ddlViewOptions.Items.Add(
                    new ListItem(
                        Localization.GetString(ArticleViewOption.Title.ToString(), this.LocalResourceFile), ArticleViewOption.Title.ToString()));
                this.ddlViewOptions.Items.Add(
                    new ListItem(
                        Localization.GetString(ArticleViewOption.Abstract.ToString(), this.LocalResourceFile), ArticleViewOption.Abstract.ToString()));
                this.ddlViewOptions.Items.Add(
                    new ListItem(
                        Localization.GetString(ArticleViewOption.TitleAndThumbnail.ToString(), this.LocalResourceFile), 
                        ArticleViewOption.TitleAndThumbnail.ToString()));
                this.ddlViewOptions.Items.Add(
                    new ListItem(
                        Localization.GetString(ArticleViewOption.Thumbnail.ToString(), this.LocalResourceFile), ArticleViewOption.Thumbnail.ToString()));

                this.ddlChildDisplay.Items.Add(new ListItem(Localization.GetString("ShowAll", this.LocalResourceFile), "ShowAll"));
                this.ddlChildDisplay.Visible = false;
                this.lblChooseChildDisplay.Visible = false;

                this.ddlSortOption.Items.Add(new ListItem(Localization.GetString("AlphaAscending", this.LocalResourceFile), "Alpha Ascending"));
                this.ddlSortOption.Items.Add(new ListItem(Localization.GetString("AlphaDescending", this.LocalResourceFile), "Alpha Descending"));
                this.ddlSortOption.Items.Add(new ListItem(Localization.GetString("CreatedAscending", this.LocalResourceFile), "Created Ascending"));
                this.ddlSortOption.Items.Add(new ListItem(Localization.GetString("CreatedDescending", this.LocalResourceFile), "Created Descending"));
                this.ddlSortOption.Items.Add(
                    new ListItem(Localization.GetString("LastUpdatedAscending", this.LocalResourceFile), "Last Updated Ascending"));
                this.ddlSortOption.Items.Add(
                    new ListItem(Localization.GetString("LastUpdatedDescending", this.LocalResourceFile), "Last Updated Descending"));

                this.ddlItemTypeList.DataTextField = "Name";
                this.ddlItemTypeList.DataValueField = "ItemTypeId";
                this.ddlItemTypeList.DataSource = Item.GetItemTypes(this.PortalId);
                this.ddlItemTypeList.DataBind();
                this.ddlItemTypeList.Items.Insert(0, new ListItem(Localization.GetString("All", this.LocalResourceFile), "-1"));

                ItemRelationship.DisplayCategoryHierarchy(this.ddlCategoryList, -1, this.PortalId, false);

                // ddlCategoryList.Items.Insert(0,new ListItem(Localization.GetString("TopLevel", LocalResourceFile), TopLevelCategoryItemType.Category.GetId().ToString()));
                object o = this.Settings["cdItemTypeId"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    ListItem li = this.ddlItemTypeList.Items.FindByValue(o.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                o = this.Settings["cdCategoryId"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    ListItem li = this.ddlCategoryList.Items.FindByValue(o.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                o = this.Settings["cdDisplayOption"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    ListItem li = this.ddlViewOptions.Items.FindByValue(o.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                o = this.Settings["cdChildDisplayOption"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    ListItem li = this.ddlChildDisplay.Items.FindByValue(o.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                o = this.Settings["cdSortOption"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    ListItem li = this.ddlSortOption.Items.FindByValue(o.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public override void UpdateSettings()
        {
            this.Save();
        }

        private void Save()
        {
            var modules = new ModuleController();
            modules.UpdateTabModuleSetting(this.TabModuleId, "cdItemTypeId", this.ddlItemTypeList.SelectedValue);
            modules.UpdateTabModuleSetting(this.TabModuleId, "cdCategoryId", this.ddlCategoryList.SelectedValue);
            modules.UpdateTabModuleSetting(this.TabModuleId, "cdDisplayOption", this.ddlViewOptions.SelectedValue);
            modules.UpdateTabModuleSetting(this.TabModuleId, "cdChildDisplayOption", this.ddlChildDisplay.SelectedValue);
            modules.UpdateTabModuleSetting(this.TabModuleId, "cdSortOption", this.ddlSortOption.SelectedValue);
        }
    }
}