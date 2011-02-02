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
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.UI.WebControls;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    public partial class CategoryNLevelsOptions : ModuleSettingsBase
    {
        public override void LoadSettings()
        {
            try
            {
                ItemRelationship.DisplayCategoryHierarchy(this.ddlCategoryList, -1, this.PortalId, false);

                this.ddlCategoryList.Items.Insert(0, new ListItem(Localization.GetString("ChooseOne", this.LocalResourceFile), "-1"));
                this.ddlCategoryList.Items.Insert(
                    1, 
                    new ListItem(
                        Localization.GetString("TopLevel", this.LocalResourceFile), 
                        TopLevelCategoryItemType.Category.GetId().ToString(CultureInfo.InvariantCulture)));
                object o = this.Settings["nLevels"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    this.txtNLevels.Text = o.ToString();
                }

                // o = Settings["mItems"];
                // if (o != null && !string.IsNullOrEmpty(o.ToString()))
                // {
                // txtMItems.Text = o.ToString();
                // }

                // chkHighlightCurrentItem
                o = this.Settings["HighlightCurrentItem"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    this.chkHighlightCurrentItem.Checked = Convert.ToBoolean(o.ToString(), CultureInfo.InvariantCulture);
                }

                o = this.Settings["ShowParentItem"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    this.chkShowParentItem.Checked = Convert.ToBoolean(o.ToString(), CultureInfo.InvariantCulture);
                }

                o = this.Settings["nCategoryId"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    ListItem li = this.ddlCategoryList.Items.FindByValue(o.ToString());
                    if (li != null)
                    {
                        li.Selected = true;
                    }
                }

                // if (string.IsNullOrEmpty(OrderString))
                // {
                // o = Settings["nSortOrder"];
                // if (o != null)
                // {
                // OrderString = Settings["nSortOrder"].ToString();
                // }
                // }
                // else
                // {
                // o = OrderString;
                // }

                // if (o != null && !string.IsNullOrEmpty(o.ToString()) )
                // {
                // DataTable dt = GetAllChildrenDataTable();
                // if (dt != null)
                // {
                // lstItems.DataSource = Utility.SortDataTable(dt, o.ToString());
                // lstItems.DataBind();
                // }
                // }
                // else 
                // {
                // DataTable dt = GetAllChildrenDataTable();
                // if (dt != null)
                // {
                // lstItems.DataSource = dt;
                // lstItems.DataBind();
                // }
                // }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public override void UpdateSettings()
        {
            var modules = new ModuleController();
            modules.UpdateTabModuleSetting(this.TabModuleId, "nCategoryId", this.ddlCategoryList.SelectedValue);
            modules.UpdateTabModuleSetting(this.TabModuleId, "nLevels", this.txtNLevels.Text);

            // modules.UpdateTabModuleSetting(this.TabModuleId, "mItems", this.txtMItems.Text.ToString());
            modules.UpdateTabModuleSetting(this.TabModuleId, "HighlightCurrentItem", this.chkHighlightCurrentItem.Checked.ToString());
            modules.UpdateTabModuleSetting(this.TabModuleId, "ShowParentItem", this.chkShowParentItem.Checked.ToString());

            // create a sorted list of items in the listbox for use when displaying.
            // OrderString = BuildOrderList();
            // modules.UpdateTabModuleSetting(this.TabModuleId, "nSortOrder", OrderString);
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void DdlCategoryListSelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlCategoryList.SelectedValue == TopLevelCategoryItemType.Category.GetId().ToString(CultureInfo.InvariantCulture))
            {
                this.chkShowParentItem.Checked = false;
                this.chkShowParentItem.Enabled = false;
            }
            else
            {
                this.chkShowParentItem.Enabled = true;
            }
        }

        // private DataTable GetAllChildrenDataTable()
        // {
        // if (ddlCategoryList.SelectedValue != "-1")
        // {
        // DataTable dt = ItemRelationship.GetAllChildrenNLevelsInDataTable(Convert.ToInt32(ddlCategoryList.SelectedValue, CultureInfo.InvariantCulture), -1, -1, PortalId);
        // return dt;
        // }
        // else
        // {
        // return null;    
        // }
        // }

        // [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        // protected void imgUp_Click(object sender, ImageClickEventArgs e)
        // {
        // int index = this.lstItems.SelectedIndex;

        // if (index < 1) { return; }

        // ListItem li = this.lstItems.SelectedItem;
        // this.lstItems.Items.Remove(li);
        // this.lstItems.Items.Insert(index - 1, li);
        // OrderString = BuildOrderList();
        // }

        // [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        // protected void imgDown_Click(object sender, ImageClickEventArgs e)
        // {
        // int index = this.lstItems.SelectedIndex;
        // if (index == this.lstItems.Items.Count - 1) { return; }

        // ListItem li = this.lstItems.SelectedItem;
        // this.lstItems.Items.Remove(li);
        // this.lstItems.Items.Insert(index + 1, li);
        // OrderString = BuildOrderList();
        // }

        // private string BuildOrderList()
        // {
        // StringBuilder sb = new StringBuilder(400);
        // foreach (ListItem li in lstItems.Items)
        // {
        // sb.Append(li.Value);
        // sb.Append(",");
        // }
        // return sb.ToString();
        // }

        // private string OrderString
        // {
        // set
        // {
        // Session["orderString" + TabModuleId.ToString(CultureInfo.InvariantCulture)] = value;
        // }
        // get
        // {
        // object o = Session["orderString" + TabModuleId.ToString(CultureInfo.InvariantCulture)];

        // if (o != null)
        // {
        // return o.ToString();
        // }

        // return string.Empty;
        // }
        // }
    }
}