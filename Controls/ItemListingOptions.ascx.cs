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
    using System.Globalization;
    using System.Web.UI.WebControls;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Services.Exceptions;
    using Util;

    public partial class ItemListingOptions : ModuleSettingsBase
    {
        #region Event Handlers

        public override void LoadSettings()
        {
            try
            {
                //we want to allow for both articles and categories to be selected here, all the time CJH
                //if (CategoryId != -1)
                //{
                    DataBindItemTypeList();
                    ListItem li = ddlItemTypeList.Items.FindByValue(ItemTypeId.ToString(CultureInfo.InvariantCulture));
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                //}
                ////if the category is top level, only allow Category for the item type, there are only categories under top level. BD
                //else
                //{
                //    ddlItemTypeList.Items.Add(new ListItem(ItemType.Category.Name, ItemType.Category.GetId().ToString(CultureInfo.InvariantCulture)));
                //}

                ddlDataType.Items.Add(new ListItem(Localization.GetString("ItemListing", LocalResourceFile), "Item Listing"));

                if (ModuleBase.IsViewTrackingEnabledForPortal(PortalId))
                {
                    ddlDataType.Items.Add(new ListItem(Localization.GetString("MostPopular", LocalResourceFile), "Most Popular"));
                }               
                ddlDataType.Items.Add(new ListItem(Localization.GetString("MostRecent", LocalResourceFile), "Most Recent"));

                li = ddlDataType.Items.FindByValue(DataType);
                if (li != null)
                {
                    li.Selected = true;
                }

                ddlDisplayFormat.Items.Add(new ListItem(Localization.GetString(ArticleViewOption.Title.ToString(), LocalResourceFile), ArticleViewOption.Title.ToString()));
                ddlDisplayFormat.Items.Add(new ListItem(Localization.GetString(ArticleViewOption.Abstract.ToString(), LocalResourceFile), ArticleViewOption.Abstract.ToString()));
                ddlDisplayFormat.Items.Add(new ListItem(Localization.GetString(ArticleViewOption.TitleAndThumbnail.ToString(), LocalResourceFile), ArticleViewOption.TitleAndThumbnail.ToString()));
                ddlDisplayFormat.Items.Add(new ListItem(Localization.GetString(ArticleViewOption.Thumbnail.ToString(), LocalResourceFile), ArticleViewOption.Thumbnail.ToString()));

                li = ddlDisplayFormat.Items.FindByValue(DataDisplayFormat);
                if (li != null)
                {
                    li.Selected = true;
                }

                txtMaxItems.Text = MaxDisplayItems.ToString(CultureInfo.CurrentCulture);

                ItemRelationship.DisplayCategoryHierarchy(ddlCategory, -1, PortalId, false);
                ddlCategory.Items.Insert(0, new ListItem(Localization.GetString("NoCategory", LocalResourceFile), "-1"));
                ListItem liCat = ddlCategory.Items.FindByValue(CategoryId.ToString(CultureInfo.InvariantCulture));
                if (liCat != null)
                {
                    liCat.Selected = true;
                }

                if (ddlDataType.SelectedValue == "Most Recent")
                {
                    chkEnableRss.Visible = true;
                    lblEnableRss.Visible = true;
                }
                else
                {
                    chkEnableRss.Visible = false;
                    lblEnableRss.Visible = false;
                }
                chkEnableRss.Checked = EnableRss;

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
                chkShowParent.Checked = ShowParent;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        /// <summary>
        /// Binds all item types to the item type DropDownList, as well as a default "Choose One" option.
        /// </summary>
        private void DataBindItemTypeList()
        {
            ddlItemTypeList.DataTextField = "Name";
            ddlItemTypeList.DataValueField = "ItemTypeId";
            ddlItemTypeList.DataSource = Item.GetItemTypes(PortalId);
            ddlItemTypeList.DataBind();
            
            //ddlItemTypeList.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1"));
            //ddlItemTypeList.Items[0].Enabled = false;
        }

        #endregion

        public override void UpdateSettings()
        {
            if (Page.IsValid)
            {
                ItemTypeId = Convert.ToInt32(ddlItemTypeList.SelectedValue, CultureInfo.InvariantCulture);
                CategoryId = Convert.ToInt32(ddlCategory.SelectedValue, CultureInfo.InvariantCulture);
                MaxDisplayItems = 0;
                if (Utility.HasValue(txtMaxItems.Text))
                {
                    MaxDisplayItems = Convert.ToInt32(txtMaxItems.Text, CultureInfo.InvariantCulture);
                }
                //MaxDisplaySubItems = Convert.ToInt32(this.txtMaxSubItems.Text);
                DataType = ddlDataType.SelectedValue;
                DataDisplayFormat = ddlDisplayFormat.SelectedValue;
                EnableRss = chkEnableRss.Checked;
                ShowParent = chkShowParent.Checked;
            }
        }

        private int ItemTypeId
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(TabModuleId, "ilItemTypeId", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = Settings["ilItemTypeId"];
                return (o == null ? -1 : Convert.ToInt32(o, CultureInfo.InvariantCulture));
            }
        }

        private int CategoryId
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(TabModuleId, "ilCategoryId", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = Settings["ilCategoryId"];
                return (o == null ? -1 : Convert.ToInt32(o, CultureInfo.InvariantCulture));
            }
        }

        private bool EnableRss
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(TabModuleId, "ilEnableRss", value.ToString());
            }

            get
            {
                object o = Settings["ilEnableRss"];
                return (o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }

        private bool ShowParent
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(TabModuleId, "ilShowParent", value.ToString());
            }

            get
            {
                object o = Settings["ilShowParent"];
                return (o == null ? false : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }

        private int MaxDisplayItems
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(TabModuleId, "ilMaxDisplayItems", value.ToString(CultureInfo.InvariantCulture));
            }

            get
            {
                object o = Settings["ilMaxDisplayItems"];
                return (o == null ? 10 : Convert.ToInt32(o, CultureInfo.InvariantCulture));
            }
        }

        //private int MaxDisplaySubItems
        //{
        //    set
        //    {
        //        ModuleController modules = new ModuleController();
        //        modules.UpdateTabModuleSetting(TabModuleId, "ilMaxDisplaySubItems", value.ToString());
        //    }

        //    get
        //    {
        //        object o = Settings["ilMaxDisplaySubItems"];
        //        return (o == null ? 10 : Convert.ToInt32(o));
        //    }
        //}

        //TODO: make this DisplayType like the rest of the modules
        private string DataDisplayFormat
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(TabModuleId, "ilDataDisplayFormat", value);
            }

            get
            {
                object o = Settings["ilDataDisplayFormat"];
                return (o == null ? ArticleViewOption.Abstract.ToString() : o.ToString());
            }
        }

        private string DataType
        {
            set
            {
                var modules = new ModuleController();
                modules.UpdateTabModuleSetting(TabModuleId, "ilDataType", value);
            }

            get
            {
                object o = Settings["ilDataType"];
                return (o == null ? "Item Listing" : o.ToString());
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member", Justification = "Controls use lower case prefix (Not an acronym)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void fvMaxItems_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args != null)
            {
                int max;
                args.IsValid = int.TryParse(args.Value, out max);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void ddlDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlDataType.SelectedValue == "Most Recent")
            {
                chkEnableRss.Visible = true;
                lblEnableRss.Visible = true;
            }
            else
            {
                chkEnableRss.Visible = false;
                lblEnableRss.Visible = false;
                chkEnableRss.Checked = false;
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

                ddlItemTypeList.Items.Clear();
                ddlItemTypeList.Items.Add(new ListItem(ItemType.Category.Name, ItemType.Category.GetId().ToString(CultureInfo.InvariantCulture)));
            }
            else
            {
                lblShowParent.Visible = true;
                chkShowParent.Visible = true;

                DataBindItemTypeList();
            }
        }
    }
}

