// <copyright file="CustomDisplayOptions.ascx.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Controls
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    public partial class CustomDisplayOptions : OverrideableDisplayOptionsBase
    {
        private CustomDisplaySettings _customDisplaySettings;

        public override void LoadSettings()
        {
            this._customDisplaySettings = new CustomDisplaySettings(this.Settings, this.TabModuleId);

            try
            {
                this.ddlItemTypeList.DataTextField = "Name";
                this.ddlItemTypeList.DataValueField = "ItemTypeId";
                this.ddlItemTypeList.DataSource = Item.GetItemTypes(this.PortalId);
                this.ddlItemTypeList.DataBind();

                this.ddlItemTypeList.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", this.LocalResourceFile), "-2"));
                this.ddlItemTypeList.Items.Insert(1, new ListItem(Localization.GetString("CategoriesAndArticles", this.LocalResourceFile), "-1"));
                ListItem li = this.ddlItemTypeList.Items.FindByValue(this._customDisplaySettings.ItemTypeId.ToString(CultureInfo.InvariantCulture));
                li.Selected = true;

                li = new ListItem(Localization.GetString(DisplayOption.Title.ToString(), this.LocalResourceFile), DisplayOption.Title.ToString())
                    {
                        Selected = this._customDisplaySettings.DisplayOptionTitle
                    };
                this.chkDisplayOptions.Items.Add(li);
                li = new ListItem(Localization.GetString(DisplayOption.Author.ToString(), this.LocalResourceFile), DisplayOption.Author.ToString())
                    {
                        Selected = this._customDisplaySettings.DisplayOptionAuthor
                    };
                this.chkDisplayOptions.Items.Add(li);
                li = new ListItem(Localization.GetString(DisplayOption.Date.ToString(), this.LocalResourceFile), DisplayOption.Date.ToString())
                    {
                        Selected = this._customDisplaySettings.DisplayOptionDate
                    };
                this.chkDisplayOptions.Items.Add(li);

                li = new ListItem(
                    Localization.GetString(DisplayOption.Abstract.ToString(), this.LocalResourceFile), DisplayOption.Abstract.ToString())
                    {
                        Selected = this._customDisplaySettings.DisplayOptionAbstract
                    };
                this.chkDisplayOptions.Items.Add(li);
                li = new ListItem(
                    Localization.GetString(DisplayOption.Thumbnail.ToString(), this.LocalResourceFile), DisplayOption.Thumbnail.ToString())
                    {
                        Selected = this._customDisplaySettings.DisplayOptionThumbnail
                    };
                this.chkDisplayOptions.Items.Add(li);
                li = new ListItem(
                    Localization.GetString(DisplayOption.ReadMore.ToString(), this.LocalResourceFile), DisplayOption.ReadMore.ToString())
                    {
                        Selected = this._customDisplaySettings.DisplayOptionReadMore
                    };
                this.chkDisplayOptions.Items.Add(li);

                li = new ListItem(Localization.GetString(DisplayOption.Stats.ToString(), this.LocalResourceFile), DisplayOption.Stats.ToString())
                    {
                        Selected = this._customDisplaySettings.DisplayOptionStats
                    };
                this.chkDisplayOptions.Items.Add(li);

                int maxItems = this._customDisplaySettings.MaxDisplayItems;
                if (maxItems > -1)
                {
                    this.txtMaxItems.Text = maxItems.ToString(CultureInfo.InvariantCulture);
                }

                this.txtMaxItems.Enabled = maxItems > -1;
                this.chkShowAll.Checked = maxItems == -1;

                ItemRelationship.DisplayCategoryHierarchy(this.ddlCategory, -1, this.PortalId, false);
                if (this.ddlItemTypeList.SelectedValue != ItemType.Article.GetId().ToString(CultureInfo.InvariantCulture) &&
                    this.ddlCategory.Items.FindByValue("-1") == null)
                {
                    this.ddlCategory.Items.Insert(
                        0, 
                        new ListItem(
                            Localization.GetString("NoCategory", this.LocalResourceFile), 
                            TopLevelCategoryItemType.Category.GetId().ToString(CultureInfo.InvariantCulture)));

                    // ddlItemTypeList.Items.Insert(1, new ListItem(Localization.GetString("CategoriesAndArticles", LocalResourceFile), "-1"));
                }

                if (this._customDisplaySettings.CategoryId > 0)
                {
                    li = this.ddlCategory.Items.FindByValue(this._customDisplaySettings.CategoryId.ToString(CultureInfo.InvariantCulture));
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                if (this.ddlCategory.SelectedValue == "-1" ||
                    this.ddlCategory.SelectedValue == TopLevelCategoryItemType.Category.GetId().ToString(CultureInfo.InvariantCulture))
                {
                    this.lblShowParent.Visible = false;
                    this.chkShowParent.Visible = false;
                    this.lblShowParentDescription.Visible = false;
                    this.chkShowParentDescription.Visible = false;
                }
                else
                {
                    this.lblShowParent.Visible = true;
                    this.chkShowParent.Visible = true;
                }

                this.chkShowParent.Checked = this._customDisplaySettings.ShowParent;
                this.chkShowParentDescription.Checked = this._customDisplaySettings.ShowParentDescription;

                this.chkRelatedItem.Checked = this._customDisplaySettings.GetParentFromQueryString;

                this.chkRelatedItemLevel.Checked = this._customDisplaySettings.GetRelatedChildren;

                this.chkAllowPaging.Checked = this._customDisplaySettings.AllowPaging;

                this.ddlSortOption.Items.Add(
                    new ListItem(Localization.GetString("TitleSort", this.LocalResourceFile), CustomDisplaySettings.TitleSort));
                this.ddlSortOption.Items.Add(new ListItem(Localization.GetString("DateSort", this.LocalResourceFile), CustomDisplaySettings.DateSort));
                this.ddlSortOption.Items.Add(
                    new ListItem(Localization.GetString("LastUpdatedSort", this.LocalResourceFile), CustomDisplaySettings.LastUpdatedSort));
                this.ddlSortOption.Items.Add(
                    new ListItem(Localization.GetString("StartDateSort", this.LocalResourceFile), CustomDisplaySettings.StartDateSort));
                if (ModuleBase.IsViewTrackingEnabledForPortal(this.PortalId) &&
                    this.ddlItemTypeList.SelectedValue == ItemType.Article.GetId().ToString(CultureInfo.InvariantCulture))
                {
                    // only show this if the Portal is configured to track views and the item type is Article. hk
                    this.ddlSortOption.Items.Add(
                        new ListItem(Localization.GetString("MostPopularSort", this.LocalResourceFile), CustomDisplaySettings.MostPopularSort));
                }

                li = this.ddlSortOption.Items.FindByValue(this._customDisplaySettings.SortOption);
                if (li != null)
                {
                    li.Selected = true;
                }

                li = this.rbSortDirection.Items.FindByValue(this._customDisplaySettings.SortDirection);
                if (li != null)
                {
                    li.Selected = true;
                }

                this.txtDateFormat.Text = this._customDisplaySettings.DateFormat;

                this.chkEnableRss.Checked = this._customDisplaySettings.EnableRss;

                this.chkUseCustomSort.Checked = this._customDisplaySettings.UseCustomSort;

                this.lnkSortCategory.NavigateUrl = this.BuildSortUrl();
                this.lnkSortCategory.Target = "_blank";
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public override void UpdateSettings()
        {
            base.UpdateSettings();

            if (this.Page.IsValid)
            {
                // Item type.
                this._customDisplaySettings.ItemTypeId = Convert.ToInt32(this.ddlItemTypeList.SelectedValue, CultureInfo.InvariantCulture);

                // Category to display
                this._customDisplaySettings.CategoryId = Convert.ToInt32(this.ddlCategory.SelectedValue, CultureInfo.InvariantCulture);

                // Max items to display
                this._customDisplaySettings.MaxDisplayItems = 0;
                if (this.chkShowAll.Checked)
                {
                    this._customDisplaySettings.MaxDisplayItems = -1;
                }
                else if (Engage.Utility.HasValue(this.txtMaxItems.Text))
                {
                    this._customDisplaySettings.MaxDisplayItems = Convert.ToInt32(this.txtMaxItems.Text, CultureInfo.InvariantCulture);
                }

                // not sure if we need this. hk
                this._customDisplaySettings.ShowParent = this.chkShowParent.Checked;

                this._customDisplaySettings.ShowParentDescription = this.chkShowParentDescription.Checked;

                // Display Options
                ListItem li = this.chkDisplayOptions.Items.FindByValue(DisplayOption.Title.ToString());
                this._customDisplaySettings.DisplayOptionTitle = li.Selected;

                li = this.chkDisplayOptions.Items.FindByValue(DisplayOption.Abstract.ToString());
                this._customDisplaySettings.DisplayOptionAbstract = li.Selected;

                li = this.chkDisplayOptions.Items.FindByValue(DisplayOption.Thumbnail.ToString());
                this._customDisplaySettings.DisplayOptionThumbnail = li.Selected;

                li = this.chkDisplayOptions.Items.FindByValue(DisplayOption.Date.ToString());
                this._customDisplaySettings.DisplayOptionDate = li.Selected;

                li = this.chkDisplayOptions.Items.FindByValue(DisplayOption.Author.ToString());
                this._customDisplaySettings.DisplayOptionAuthor = li.Selected;

                li = this.chkDisplayOptions.Items.FindByValue(DisplayOption.ReadMore.ToString());
                this._customDisplaySettings.DisplayOptionReadMore = li.Selected;

                li = this.chkDisplayOptions.Items.FindByValue(DisplayOption.Stats.ToString());
                this._customDisplaySettings.DisplayOptionStats = li.Selected;

                // Direction to sort ASC or DESC
                this._customDisplaySettings.SortDirection = this.rbSortDirection.SelectedValue;

                this._customDisplaySettings.SortOption = this.ddlSortOption.SelectedValue;

                this._customDisplaySettings.DateFormat = this.txtDateFormat.Text;

                this._customDisplaySettings.GetParentFromQueryString = this.chkRelatedItem.Checked;

                this._customDisplaySettings.GetRelatedChildren = this.chkRelatedItemLevel.Checked;

                this._customDisplaySettings.EnableRss = this.chkEnableRss.Checked;

                this._customDisplaySettings.AllowPaging = this.chkAllowPaging.Checked;

                this._customDisplaySettings.UseCustomSort = this.chkUseCustomSort.Checked;
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void chkShowAll_CheckedChanged(object sender, EventArgs e)
        {
            this.txtMaxItems.Enabled = !this.chkShowAll.Checked;
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            // there is no parent category to display for Top Level items - BD
            if (this.ddlCategory.SelectedValue == "-1")
            {
                this.lblShowParent.Visible = false;
                this.chkShowParent.Visible = false;
                this.chkShowParentDescription.Visible = false;
                this.lblShowParentDescription.Visible = false;
            }
            else
            {
                this.lblShowParent.Visible = true;
                this.chkShowParent.Visible = true;
                this.chkShowParentDescription.Visible = true;
                this.lblShowParentDescription.Visible = true;
            }

            var qsp = new QueryStringParameters();
            qsp.ClearKeys();
            qsp.Add("ctl", Utility.AdminContainer);
            qsp.Add("mid", this.ModuleId.ToString(CultureInfo.InvariantCulture));
            qsp.Add("adminType", "categorysort");
            qsp.Add("itemid", this.ddlCategory.SelectedValue);

            this.lnkSortCategory.NavigateUrl = this.BuildSortUrl();
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void ddlItemTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlItemTypeList.SelectedValue != ItemType.Category.GetId().ToString(CultureInfo.InvariantCulture))
            {
                // pull out top level. hk
                // we pull Article from the list, but we still want to be able to display articles and categories
                this.ddlCategory.Items.Remove(this.ddlCategory.Items.FindByValue("-2"));
            }
            else if (this.ddlCategory.Items.FindByValue("-2") == null)
            {
                this.ddlCategory.Items.Insert(0, new ListItem(Localization.GetString("NoCategory", this.LocalResourceFile), "-2"));
            }

            if (!ModuleBase.IsViewTrackingEnabledForPortal(this.PortalId) ||
                this.ddlItemTypeList.SelectedValue != ItemType.Article.GetId().ToString(CultureInfo.InvariantCulture))
            {
                // only show this if the Portal is configured to track views and the item type is Article. hk
                this.ddlSortOption.Items.Remove(CustomDisplaySettings.MostPopularSort);
            }
            else if (this.ddlSortOption.Items.FindByValue(CustomDisplaySettings.MostPopularSort) == null)
            {
                this.ddlSortOption.Items.Add(
                    new ListItem(Localization.GetString("MostPopularSort", this.LocalResourceFile), CustomDisplaySettings.MostPopularSort));
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member", 
            Justification = "Controls use lower case prefix (Not an acronym)")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void fvMaxItems_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!this.chkShowAll.Checked)
            {
                if (args != null)
                {
                    int max;
                    args.IsValid = int.TryParse(args.Value, out max);
                }
            }
        }

        private string BuildSortUrl()
        {
            var qsp = new QueryStringParameters();
            qsp.ClearKeys();
            qsp.Add("ctl", Utility.AdminContainer);
            qsp.Add("mid", this.ModuleId.ToString(CultureInfo.InvariantCulture));
            qsp.Add("adminType", "categorysort");
            qsp.Add("windowClose", "true");
            qsp.Add("itemid", this.ddlCategory.SelectedValue);
            return Globals.NavigateURL(this.TabId, string.Empty, qsp.ToString());
        }
    }
}