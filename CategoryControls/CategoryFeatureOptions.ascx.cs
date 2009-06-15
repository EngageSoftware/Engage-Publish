//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.CategoryControls
{
    public partial class CategoryFeatureOptions : ModuleSettingsBase
    {
        public override void LoadSettings()
        {
            try
            {
                ddlViewOptions.Items.Add(new ListItem(Localization.GetString(ArticleViewOption.Title.ToString(), LocalResourceFile), ArticleViewOption.Title.ToString()));
                ddlViewOptions.Items.Add(new ListItem(Localization.GetString(ArticleViewOption.Abstract.ToString(), LocalResourceFile), ArticleViewOption.Abstract.ToString()));
                ddlViewOptions.Items.Add(new ListItem(Localization.GetString(ArticleViewOption.TitleAndThumbnail.ToString(), LocalResourceFile), ArticleViewOption.TitleAndThumbnail.ToString()));
                ddlViewOptions.Items.Add(new ListItem(Localization.GetString(ArticleViewOption.Thumbnail.ToString(), LocalResourceFile), ArticleViewOption.Thumbnail.ToString()));

                ItemRelationship.DisplayCategoryHierarchy(ddlCategoryList, -1, PortalId, false);

                object o = Settings["cfCategoryId"];
                if (o != null && Utility.HasValue(o.ToString()))
                {
                    ListItem li = ddlCategoryList.Items.FindByValue(o.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                o = Settings["cfDisplayOption"];
                if (o != null && Utility.HasValue(o.ToString()))
                {
                    ListItem li = ddlViewOptions.Items.FindByValue(o.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                o = Settings["cfEnableRss"];
                bool enableRss;
                if (o != null &&  bool.TryParse(o.ToString(), out enableRss))
                {
                   chkEnableRss.Checked = enableRss;
                }

                o = Settings["cfRandomize"];
                bool randomize;
                if (o != null && bool.TryParse(o.ToString(), out randomize))
                {
                    chkRandomize.Checked = randomize;
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
            ModuleController modules = new ModuleController();
            modules.UpdateTabModuleSetting(this.TabModuleId, "cfCategoryId", this.ddlCategoryList.SelectedValue.ToString());
            modules.UpdateTabModuleSetting(this.TabModuleId, "cfDisplayOption", this.ddlViewOptions.SelectedValue.ToString());
            modules.UpdateTabModuleSetting(this.TabModuleId, "cfEnableRss", this.chkEnableRss.Checked.ToString());
            modules.UpdateTabModuleSetting(this.TabModuleId, "cfRandomize", this.chkRandomize.Checked.ToString());
        }
    }
}

