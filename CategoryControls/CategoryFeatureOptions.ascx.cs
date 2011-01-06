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
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    public partial class CategoryFeatureOptions : ModuleSettingsBase
    {
        public override void LoadSettings()
        {
            try
            {
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

                ItemRelationship.DisplayCategoryHierarchy(this.ddlCategoryList, -1, this.PortalId, false);

                object o = this.Settings["cfCategoryId"];
                if (o != null && Engage.Utility.HasValue(o.ToString()))
                {
                    ListItem li = this.ddlCategoryList.Items.FindByValue(o.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                o = this.Settings["cfDisplayOption"];
                if (o != null && Engage.Utility.HasValue(o.ToString()))
                {
                    ListItem li = this.ddlViewOptions.Items.FindByValue(o.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                o = this.Settings["cfEnableRss"];
                bool enableRss;
                if (o != null && bool.TryParse(o.ToString(), out enableRss))
                {
                    this.chkEnableRss.Checked = enableRss;
                }

                o = this.Settings["cfRandomize"];
                bool randomize;
                if (o != null && bool.TryParse(o.ToString(), out randomize))
                {
                    this.chkRandomize.Checked = randomize;
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
            modules.UpdateTabModuleSetting(this.TabModuleId, "cfCategoryId", this.ddlCategoryList.SelectedValue);
            modules.UpdateTabModuleSetting(this.TabModuleId, "cfDisplayOption", this.ddlViewOptions.SelectedValue);
            modules.UpdateTabModuleSetting(this.TabModuleId, "cfEnableRss", this.chkEnableRss.Checked.ToString());
            modules.UpdateTabModuleSetting(this.TabModuleId, "cfRandomize", this.chkRandomize.Checked.ToString());
        }
    }
}