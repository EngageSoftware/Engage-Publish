//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.UI.UserControls;
using Engage.Dnn.Publish.Data;
using Engage.Dnn.Publish.Controls;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.CategoryControls
{
    public partial class CategoryDisplayOptions : ModuleSettingsBase
    {
        #region Event Handlers

        public override void LoadSettings()
        {
            try
            {
                ddlViewOptions.Items.Add(new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1"));
                ddlViewOptions.Items.Add(new ListItem(Localization.GetString(ArticleViewOption.Title.ToString(), LocalResourceFile), ArticleViewOption.Title.ToString()));
                ddlViewOptions.Items.Add(new ListItem(Localization.GetString(ArticleViewOption.Abstract.ToString(), LocalResourceFile), ArticleViewOption.Abstract.ToString()));
                ddlViewOptions.Items.Add(new ListItem(Localization.GetString(ArticleViewOption.TitleAndThumbnail.ToString(), LocalResourceFile), ArticleViewOption.TitleAndThumbnail.ToString()));
                ddlViewOptions.Items.Add(new ListItem(Localization.GetString(ArticleViewOption.Thumbnail.ToString(), LocalResourceFile), ArticleViewOption.Thumbnail.ToString()));

                ddlChildDisplay.Items.Add(new ListItem(Localization.GetString("ShowAll", LocalResourceFile), "ShowAll"));
                ddlChildDisplay.Visible = false;
                lblChooseChildDisplay.Visible = false;
                
                ddlSortOption.Items.Add(new ListItem(Localization.GetString("AlphaAscending", LocalResourceFile), "Alpha Ascending"));
                ddlSortOption.Items.Add(new ListItem(Localization.GetString("AlphaDescending", LocalResourceFile), "Alpha Descending"));
                ddlSortOption.Items.Add(new ListItem(Localization.GetString("CreatedAscending", LocalResourceFile), "Created Ascending"));
                ddlSortOption.Items.Add(new ListItem(Localization.GetString("CreatedDescending", LocalResourceFile), "Created Descending"));
                ddlSortOption.Items.Add(new ListItem(Localization.GetString("LastUpdatedAscending", LocalResourceFile), "Last Updated Ascending"));
                ddlSortOption.Items.Add(new ListItem(Localization.GetString("LastUpdatedDescending", LocalResourceFile), "Last Updated Descending"));

                ddlItemTypeList.DataTextField = "Name";
                ddlItemTypeList.DataValueField = "ItemTypeId";
                ddlItemTypeList.DataSource = Item.GetItemTypes(PortalId);
                ddlItemTypeList.DataBind();
                ddlItemTypeList.Items.Insert(0, new ListItem(Localization.GetString("All", LocalResourceFile), "-1"));

                ItemRelationship.DisplayCategoryHierarchy(ddlCategoryList, -1, PortalId, false);

                //ddlCategoryList.Items.Insert(0,new ListItem(Localization.GetString("TopLevel", LocalResourceFile), TopLevelCategoryItemType.Category.GetId().ToString()));

                object o = Settings["cdItemTypeId"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    ListItem li = ddlItemTypeList.Items.FindByValue(o.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                o = Settings["cdCategoryId"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    ListItem li = ddlCategoryList.Items.FindByValue(o.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                o = Settings["cdDisplayOption"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    ListItem li = ddlViewOptions.Items.FindByValue(o.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                o = Settings["cdChildDisplayOption"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    ListItem li = ddlChildDisplay.Items.FindByValue(o.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                o = Settings["cdSortOption"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    ListItem li = ddlSortOption.Items.FindByValue(o.ToString());
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

        #endregion

        #region Optional Interfaces

        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
                Actions.Add(GetNextActionID(), Localization.GetString(DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", "", false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
                return Actions;
            }
        }


        #endregion

        private void save()
        {
            ModuleController modules = new ModuleController();
            modules.UpdateTabModuleSetting(this.TabModuleId, "cdItemTypeId", this.ddlItemTypeList.SelectedValue.ToString());
            modules.UpdateTabModuleSetting(this.TabModuleId, "cdCategoryId", this.ddlCategoryList.SelectedValue.ToString());
            modules.UpdateTabModuleSetting(this.TabModuleId, "cdDisplayOption", this.ddlViewOptions.SelectedValue.ToString());
            modules.UpdateTabModuleSetting(this.TabModuleId, "cdChildDisplayOption", this.ddlChildDisplay.SelectedValue.ToString());
            modules.UpdateTabModuleSetting(this.TabModuleId, "cdSortOption", this.ddlSortOption.SelectedValue.ToString());

        }
        public override void UpdateSettings()
        {
            save();
        }
    }
}
