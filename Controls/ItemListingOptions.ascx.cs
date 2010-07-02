//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Controls
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.UI.WebControls;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    public partial class ItemListingOptions : ModuleSettingsBase
    {
        private int CategoryId
        {
            get
            {
                object o = this.Settings["ilCategoryId"];
                return o == null ? -1 : Convert.ToInt32(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "ilCategoryId", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        // private int MaxDisplaySubItems
        // {
        // set
        // {
        // ModuleController modules = new ModuleController();
        // modules.UpdateTabModuleSetting(TabModuleId, "ilMaxDisplaySubItems", value.ToString());
        // }

        // get
        // {
        // object o = Settings["ilMaxDisplaySubItems"];
        // return (o == null ? 10 : Convert.ToInt32(o));
        // }
        // }

        // TODO: make this DisplayType like the rest of the modules
        private string DataDisplayFormat
        {
            get
            {
                object o = this.Settings["ilDataDisplayFormat"];
                return o == null ? ArticleViewOption.Abstract.ToString() : o.ToString();
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "ilDataDisplayFormat", value);
            }
        }

        private string DataType
        {
            get
            {
                object o = this.Settings["ilDataType"];
                return o == null ? "Item Listing" : o.ToString();
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "ilDataType", value);
            }
        }

        private bool EnableRss
        {
            get
            {
                object o = this.Settings["ilEnableRss"];
                return o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "ilEnableRss", value.ToString());
            }
        }

        private int ItemTypeId
        {
            get
            {
                object o = this.Settings["ilItemTypeId"];
                return o == null ? -1 : Convert.ToInt32(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "ilItemTypeId", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        private int MaxDisplayItems
        {
            get
            {
                object o = this.Settings["ilMaxDisplayItems"];
                return o == null ? 10 : Convert.ToInt32(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "ilMaxDisplayItems", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        private bool ShowParent
        {
            get
            {
                object o = this.Settings["ilShowParent"];
                return o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }

            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(this.TabModuleId, "ilShowParent", value.ToString());
            }
        }

        public override void LoadSettings()
        {
            try
            {
                // we want to allow for both articles and categories to be selected here, all the time CJH
                // if (CategoryId != -1)
                // {
                this.DataBindItemTypeList();
                ListItem li = this.ddlItemTypeList.Items.FindByValue(this.ItemTypeId.ToString(CultureInfo.InvariantCulture));
                if (li != null)
                {
                    li.Selected = true;
                }

                // }
                ////if the category is top level, only allow Category for the item type, there are only categories under top level. BD
                // else
                // {
                // ddlItemTypeList.Items.Add(new ListItem(ItemType.Category.Name, ItemType.Category.GetId().ToString(CultureInfo.InvariantCulture)));
                // }
                this.ddlDataType.Items.Add(new ListItem(Localization.GetString("ItemListing", this.LocalResourceFile), "Item Listing"));

                if (ModuleBase.IsViewTrackingEnabledForPortal(this.PortalId))
                {
                    this.ddlDataType.Items.Add(new ListItem(Localization.GetString("MostPopular", this.LocalResourceFile), "Most Popular"));
                }

                this.ddlDataType.Items.Add(new ListItem(Localization.GetString("MostRecent", this.LocalResourceFile), "Most Recent"));

                li = this.ddlDataType.Items.FindByValue(this.DataType);
                if (li != null)
                {
                    li.Selected = true;
                }

                this.ddlDisplayFormat.Items.Add(
                    new ListItem(
                        Localization.GetString(ArticleViewOption.Title.ToString(), this.LocalResourceFile), ArticleViewOption.Title.ToString()));
                this.ddlDisplayFormat.Items.Add(
                    new ListItem(
                        Localization.GetString(ArticleViewOption.Abstract.ToString(), this.LocalResourceFile), ArticleViewOption.Abstract.ToString()));
                this.ddlDisplayFormat.Items.Add(
                    new ListItem(
                        Localization.GetString(ArticleViewOption.TitleAndThumbnail.ToString(), this.LocalResourceFile), 
                        ArticleViewOption.TitleAndThumbnail.ToString()));
                this.ddlDisplayFormat.Items.Add(
                    new ListItem(
                        Localization.GetString(ArticleViewOption.Thumbnail.ToString(), this.LocalResourceFile), ArticleViewOption.Thumbnail.ToString()));

                li = this.ddlDisplayFormat.Items.FindByValue(this.DataDisplayFormat);
                if (li != null)
                {
                    li.Selected = true;
                }

                this.txtMaxItems.Text = this.MaxDisplayItems.ToString(CultureInfo.CurrentCulture);

                ItemRelationship.DisplayCategoryHierarchy(this.ddlCategory, -1, this.PortalId, false);
                this.ddlCategory.Items.Insert(0, new ListItem(Localization.GetString("NoCategory", this.LocalResourceFile), "-1"));
                ListItem liCat = this.ddlCategory.Items.FindByValue(this.CategoryId.ToString(CultureInfo.InvariantCulture));
                if (liCat != null)
                {
                    liCat.Selected = true;
                }

                if (this.ddlDataType.SelectedValue == "Most Recent")
                {
                    this.chkEnableRss.Visible = true;
                    this.lblEnableRss.Visible = true;
                }
                else
                {
                    this.chkEnableRss.Visible = false;
                    this.lblEnableRss.Visible = false;
                }

                this.chkEnableRss.Checked = this.EnableRss;

                if (this.ddlCategory.SelectedValue == "-1")
                {
                    this.lblShowParent.Visible = false;
                    this.chkShowParent.Visible = false;
                }
                else
                {
                    this.lblShowParent.Visible = true;
                    this.chkShowParent.Visible = true;
                }

                this.chkShowParent.Checked = this.ShowParent;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public override void UpdateSettings()
        {
            if (this.Page.IsValid)
            {
                this.ItemTypeId = Convert.ToInt32(this.ddlItemTypeList.SelectedValue, CultureInfo.InvariantCulture);
                this.CategoryId = Convert.ToInt32(this.ddlCategory.SelectedValue, CultureInfo.InvariantCulture);
                this.MaxDisplayItems = 0;
                if (Utility.HasValue(this.txtMaxItems.Text))
                {
                    this.MaxDisplayItems = Convert.ToInt32(this.txtMaxItems.Text, CultureInfo.InvariantCulture);
                }

                // MaxDisplaySubItems = Convert.ToInt32(this.txtMaxSubItems.Text);
                this.DataType = this.ddlDataType.SelectedValue;
                this.DataDisplayFormat = this.ddlDisplayFormat.SelectedValue;
                this.EnableRss = this.chkEnableRss.Checked;
                this.ShowParent = this.chkShowParent.Checked;
            }
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

                this.ddlItemTypeList.Items.Clear();
                this.ddlItemTypeList.Items.Add(new ListItem(ItemType.Category.Name, ItemType.Category.GetId().ToString(CultureInfo.InvariantCulture)));
            }
            else
            {
                this.lblShowParent.Visible = true;
                this.chkShowParent.Visible = true;

                this.DataBindItemTypeList();
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void ddlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlDataType.SelectedValue == "Most Recent")
            {
                this.chkEnableRss.Visible = true;
                this.lblEnableRss.Visible = true;
            }
            else
            {
                this.chkEnableRss.Visible = false;
                this.lblEnableRss.Visible = false;
                this.chkEnableRss.Checked = false;
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member", 
            Justification = "Controls use lower case prefix (Not an acronym)")]
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void fvMaxItems_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args != null)
            {
                int max;
                args.IsValid = int.TryParse(args.Value, out max);
            }
        }

        /// <summary>
        /// Binds all item types to the item type DropDownList, as well as a default "Choose One" option.
        /// </summary>
        private void DataBindItemTypeList()
        {
            this.ddlItemTypeList.DataTextField = "Name";
            this.ddlItemTypeList.DataValueField = "ItemTypeId";
            this.ddlItemTypeList.DataSource = Item.GetItemTypes(this.PortalId);
            this.ddlItemTypeList.DataBind();

            // ddlItemTypeList.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1"));
            // ddlItemTypeList.Items[0].Enabled = false;
        }
    }
}