//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using Engage.Dnn.Publish.Controls;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.Controls
{
    public partial class CustomDisplayOptions : ModuleSettingsBase
    {

        private CustomDisplaySettings customDisplaySettings;

        public override void LoadSettings()
        {
            customDisplaySettings = new CustomDisplaySettings(Settings, TabModuleId);

            try
            {
                ddlItemTypeList.DataTextField = "Name";
                ddlItemTypeList.DataValueField = "ItemTypeId";
                ddlItemTypeList.DataSource = Item.GetItemTypes(PortalId);
                ddlItemTypeList.DataBind();

                ddlItemTypeList.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-2"));
                ddlItemTypeList.Items.Insert(1, new ListItem(Localization.GetString("CategoriesAndArticles", LocalResourceFile), "-1"));
                ListItem li = ddlItemTypeList.Items.FindByValue(customDisplaySettings.ItemTypeId.ToString(CultureInfo.InvariantCulture));
                li.Selected = true;

                li = new ListItem(Localization.GetString(DisplayOption.Title.ToString(), LocalResourceFile), DisplayOption.Title.ToString());
                li.Selected = customDisplaySettings.DisplayOptionTitle;
                chkDisplayOptions.Items.Add(li);
                li = new ListItem(Localization.GetString(DisplayOption.Author.ToString(), LocalResourceFile), DisplayOption.Author.ToString());
                li.Selected = customDisplaySettings.DisplayOptionAuthor;
                chkDisplayOptions.Items.Add(li);
                li = new ListItem(Localization.GetString(DisplayOption.Date.ToString(), LocalResourceFile), DisplayOption.Date.ToString());
                li.Selected = customDisplaySettings.DisplayOptionDate;
                chkDisplayOptions.Items.Add(li);

                li = new ListItem(Localization.GetString(DisplayOption.Abstract.ToString(), LocalResourceFile), DisplayOption.Abstract.ToString());
                li.Selected = customDisplaySettings.DisplayOptionAbstract;
                chkDisplayOptions.Items.Add(li);
                li = new ListItem(Localization.GetString(DisplayOption.Thumbnail.ToString(), LocalResourceFile), DisplayOption.Thumbnail.ToString());
                li.Selected = customDisplaySettings.DisplayOptionThumbnail;
                chkDisplayOptions.Items.Add(li);
                li = new ListItem(Localization.GetString(DisplayOption.ReadMore.ToString(), LocalResourceFile), DisplayOption.ReadMore.ToString());
                li.Selected = customDisplaySettings.DisplayOptionReadMore;
                chkDisplayOptions.Items.Add(li);

                int maxItems = customDisplaySettings.MaxDisplayItems;
                if (maxItems > -1)
                {
                    this.txtMaxItems.Text = maxItems.ToString(CultureInfo.InvariantCulture);
                }
                txtMaxItems.Enabled = (maxItems > -1);
                chkShowAll.Checked = (maxItems == -1);

                ItemRelationship.DisplayCategoryHierarchy(ddlCategory, -1, PortalId, false);
                if (ddlItemTypeList.SelectedValue != ItemType.Article.GetId().ToString(CultureInfo.InvariantCulture) && ddlCategory.Items.FindByValue("-1") == null)
                {
                    ddlCategory.Items.Insert(0, new ListItem(Localization.GetString("NoCategory", LocalResourceFile), TopLevelCategoryItemType.Category.GetId().ToString(CultureInfo.InvariantCulture)));
                    //ddlItemTypeList.Items.Insert(1, new ListItem(Localization.GetString("CategoriesAndArticles", LocalResourceFile), "-1"));
                }

                if (customDisplaySettings.CategoryId > 0)
                {
                    li = ddlCategory.Items.FindByValue(customDisplaySettings.CategoryId.ToString(CultureInfo.InvariantCulture));
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                if (ddlCategory.SelectedValue == "-1" || ddlCategory.SelectedValue == TopLevelCategoryItemType.Category.GetId().ToString(CultureInfo.InvariantCulture))
                {
                    lblShowParent.Visible = false;
                    chkShowParent.Visible = false;
                }
                else
                {
                    lblShowParent.Visible = true;
                    chkShowParent.Visible = true;
                }
                chkShowParent.Checked = customDisplaySettings.ShowParent;

                chkRelatedItem.Checked = customDisplaySettings.GetParentFromQueryString;

                chkAllowPaging.Checked = customDisplaySettings.AllowPaging;

                ddlSortOption.Items.Add(new ListItem(Localization.GetString("TitleSort", LocalResourceFile), CustomDisplaySettings.TitleSort));
                ddlSortOption.Items.Add(new ListItem(Localization.GetString("DateSort", LocalResourceFile), CustomDisplaySettings.DateSort));
                ddlSortOption.Items.Add(new ListItem(Localization.GetString("LastUpdatedSort", LocalResourceFile), CustomDisplaySettings.LastUpdatedSort));
                ddlSortOption.Items.Add(new ListItem(Localization.GetString("StartDateSort", LocalResourceFile), CustomDisplaySettings.StartDateSort));
                if (ModuleBase.IsViewTrackingEnabledForPortal(PortalId) && ddlItemTypeList.SelectedValue == ItemType.Article.GetId().ToString(CultureInfo.InvariantCulture))
                {
                    //only show this if the Portal is configured to track views and the item type is Article. hk
                    ddlSortOption.Items.Add(new ListItem(Localization.GetString("MostPopularSort", LocalResourceFile), CustomDisplaySettings.MostPopularSort));
                }

                li = ddlSortOption.Items.FindByValue(customDisplaySettings.SortOption);
                if (li != null)
                {
                    li.Selected = true;
                }

                li = rbSortDirection.Items.FindByValue(customDisplaySettings.SortDirection);
                if (li != null)
                {
                    li.Selected = true;
                }
                txtDateFormat.Text = customDisplaySettings.DateFormat;

                chkEnableRss.Checked = customDisplaySettings.EnableRss;

                chkUseCustomSort.Checked = customDisplaySettings.UseCustomSort;

                lnkSortCategory.NavigateUrl = BuildSortUrl();
                lnkSortCategory.Target = "_blank";
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public override void UpdateSettings()
        {
            base.UpdateSettings();

            if (Page.IsValid)
            {   
                //Item type.
                customDisplaySettings.ItemTypeId = Convert.ToInt32(this.ddlItemTypeList.SelectedValue, CultureInfo.InvariantCulture);
                
                //Category to display
                customDisplaySettings.CategoryId = Convert.ToInt32(this.ddlCategory.SelectedValue, CultureInfo.InvariantCulture);

                //Max items to display
                customDisplaySettings.MaxDisplayItems = 0;
                if (chkShowAll.Checked)
                {
                    customDisplaySettings.MaxDisplayItems = -1;
                }
                else if (Utility.HasValue(this.txtMaxItems.Text))
                {
                    customDisplaySettings.MaxDisplayItems = Convert.ToInt32(this.txtMaxItems.Text, CultureInfo.InvariantCulture);
                }

                //not sure if we need this. hk
                customDisplaySettings.ShowParent = this.chkShowParent.Checked;

                //Display Options
                ListItem li = chkDisplayOptions.Items.FindByValue(DisplayOption.Title.ToString());
                customDisplaySettings.DisplayOptionTitle = li.Selected;

                li = chkDisplayOptions.Items.FindByValue(DisplayOption.Abstract.ToString());
                customDisplaySettings.DisplayOptionAbstract = li.Selected;

                li = chkDisplayOptions.Items.FindByValue(DisplayOption.Thumbnail.ToString());
                customDisplaySettings.DisplayOptionThumbnail = li.Selected;

                li = chkDisplayOptions.Items.FindByValue(DisplayOption.Date.ToString());
                customDisplaySettings.DisplayOptionDate = li.Selected;

                li = chkDisplayOptions.Items.FindByValue(DisplayOption.Author.ToString());
                customDisplaySettings.DisplayOptionAuthor = li.Selected;

                li = chkDisplayOptions.Items.FindByValue(DisplayOption.ReadMore.ToString());
                customDisplaySettings.DisplayOptionReadMore = li.Selected;

                //Direction to sort ASC or DESC
                customDisplaySettings.SortDirection = rbSortDirection.SelectedValue;

                customDisplaySettings.SortOption = ddlSortOption.SelectedValue;

                customDisplaySettings.DateFormat = txtDateFormat.Text;

                customDisplaySettings.GetParentFromQueryString = chkRelatedItem.Checked;

                customDisplaySettings.EnableRss = chkEnableRss.Checked;

                customDisplaySettings.AllowPaging = chkAllowPaging.Checked;

                customDisplaySettings.UseCustomSort = chkUseCustomSort.Checked;
            }
        }

        #region Event Handlers

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member", Justification = "Controls use lower case prefix (Not an acronym)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void fvMaxItems_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!chkShowAll.Checked)
            {
                if (args != null)
                {
                    int max;
                    args.IsValid = int.TryParse(args.Value, out max);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //there is no parent category to display for Top Level items - BD
            if (ddlCategory.SelectedValue == "-1")
            {
                lblShowParent.Visible = false;
                chkShowParent.Visible = false;
            }
            else
            {
                lblShowParent.Visible = true;
                chkShowParent.Visible = true;
            }

            QueryStringParameters qsp = new QueryStringParameters();
            qsp.ClearKeys();
            qsp.Add("ctl", Utility.AdminContainer);
            qsp.Add("mid", ModuleId.ToString(CultureInfo.InvariantCulture));
            qsp.Add("adminType", "categorysort");
            qsp.Add("itemid", ddlCategory.SelectedValue);


            lnkSortCategory.NavigateUrl = BuildSortUrl();
        }

        private string BuildSortUrl()
        {
            QueryStringParameters qsp = new QueryStringParameters();
            qsp.ClearKeys();
            qsp.Add("ctl", Utility.AdminContainer);
            qsp.Add("mid", ModuleId.ToString(CultureInfo.InvariantCulture));
            qsp.Add("adminType", "categorysort");
            qsp.Add("windowClose", "true");
            qsp.Add("itemid", ddlCategory.SelectedValue);
            return DotNetNuke.Common.Globals.NavigateURL(TabId, "", qsp.ToString());
        
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            txtMaxItems.Enabled = !chkShowAll.Checked;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void ddlItemTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlItemTypeList.SelectedValue != ItemType.Category.GetId().ToString(CultureInfo.InvariantCulture))
            {
                //pull out top level. hk
                //we pull Article from the list, but we still want to be able to display articles and categories
                ddlCategory.Items.Remove(ddlCategory.Items.FindByValue("-2"));
            }
            else if (ddlCategory.Items.FindByValue("-2") == null)
            {
                ddlCategory.Items.Insert(0, new ListItem(Localization.GetString("NoCategory", LocalResourceFile), "-2"));
            }

            if (!ModuleBase.IsViewTrackingEnabledForPortal(PortalId) || ddlItemTypeList.SelectedValue != ItemType.Article.GetId().ToString(CultureInfo.InvariantCulture))
            {
                //only show this if the Portal is configured to track views and the item type is Article. hk
                ddlSortOption.Items.Remove(CustomDisplaySettings.MostPopularSort);
            }
            else if (ddlSortOption.Items.FindByValue(CustomDisplaySettings.MostPopularSort) == null)
            {
                ddlSortOption.Items.Add(new ListItem(Localization.GetString("MostPopularSort", LocalResourceFile), CustomDisplaySettings.MostPopularSort));
            }           
        }
        #endregion
    }
}

