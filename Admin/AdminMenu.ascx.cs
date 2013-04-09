// <copyright file="AdminMenu.ascx.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Admin
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Util;

    public partial class AdminMenu : ModuleBase
    {
        public string BuildAddArticleUrl()
        {
            if (this.ItemId <= -1)
            {
                return string.Empty;
            }

            int parentCategoryId = -1;

            if (!this.VersionInfoObject.IsNew)
            {
                parentCategoryId = this.VersionInfoObject.ItemTypeId == ItemType.Category.GetId()
                                       ? this.VersionInfoObject.ItemId
                                       : this.VersionInfoObject.GetParentCategoryId();
            }

            return Globals.NavigateURL(
                this.TabId, 
                string.Empty, 
                "ctl=" + Utility.AdminContainer, 
                "mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture), 
                "adminType=articleedit", 
                "parentId=" + parentCategoryId.ToString(CultureInfo.InvariantCulture), 
                "returnUrl=" + HttpUtility.UrlEncode(this.Request.RawUrl));
        }

        public string BuildCategoryListUrl()
        {
            // find the location of the ams admin module on the site.
            // DotNetNuke.Entities.Modules.ModuleController objModules = new ModuleController();
            if (this.ItemId > -1)
            {
                int parentCategoryId = -1;

                if (!this.VersionInfoObject.IsNew)
                {
                    parentCategoryId = this.VersionInfoObject.ItemTypeId == ItemType.Category.GetId()
                                           ? this.VersionInfoObject.ItemId
                                           : Category.GetParentCategory(this.VersionInfoObject.ItemId, this.PortalId);
                }

                // string currentItemType = Item.GetItemType(ItemId,PortalId);
                return Globals.NavigateURL(
                    this.TabId, 
                    string.Empty, 
                    "ctl=" + Utility.AdminContainer, 
                    "mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture), 
                    "adminType=articlelist", 
                    "categoryId=" + parentCategoryId.ToString(CultureInfo.InvariantCulture));
            }

            return string.Empty;
        }

        public string BuildEditUrl(string currentItemType)
        {
            string url = string.Empty;
            try
            {
                // find the location of the ams admin module on the site.
                // DotNetNuke.Entities.Modules.ModuleController objModules = new ModuleController();

                if (this.ItemId > -1)
                {
                    int versionId = -1;
                    if (!this.VersionInfoObject.IsNew)
                    {
                        versionId = this.VersionInfoObject.ItemVersionId;
                    }

                    url = Globals.NavigateURL(
                        this.TabId, 
                        string.Empty, 
                        "ctl=" + Utility.AdminContainer, 
                        "mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture), 
                        "adminType=" + currentItemType + "Edit", 
                        "versionId=" + versionId.ToString(CultureInfo.InvariantCulture), 
                        "returnUrl=" + HttpUtility.UrlEncode(this.Request.RawUrl));
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

            return url;
        }

        // [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        // protected void ddlApprovalStatus_SelectedIndexChanged(object sender, EventArgs e)
        // {
        // CallUpdateApprovalStatus();
        // }

        protected void CallUpdateApprovalStatus()
        {
            if (!this.VersionInfoObject.IsNew)
            {
                this.VersionInfoObject.ApprovalStatusId = Convert.ToInt32(this.ApprovalStatusDropDownList.SelectedValue, CultureInfo.InvariantCulture);
                this.VersionInfoObject.ApprovalComments = this.txtApprovalComments.Text.Trim().Length > 0
                                                              ? this.txtApprovalComments.Text.Trim()
                                                              : Localization.GetString("DefaultApprovalComment", this.LocalResourceFile);
                this.VersionInfoObject.UpdateApprovalStatus();

                // Utility.ClearPublishCache(PortalId);
                this.Response.Redirect(this.BuildVersionsUrl(), false);
                // redirect to the versions list for this item.
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);

            this.BindItemData();
            this.ConfigureMenus();
        }

        protected void lnkSaveApprovalStatusCancel_Click(object sender, EventArgs e)
        {
            this.divApprovalStatus.Visible = false;
        }

        protected void lnkSaveApprovalStatus_Click(object sender, EventArgs e)
        {
            this.CallUpdateApprovalStatus();
        }

        protected void lnkUpdateStatus_Click(object sender, EventArgs e)
        {
            if (this.divApprovalStatus != null)
            {
                this.divApprovalStatus.Visible = true;
            }

            // check if we're editing an article, if so show version comments
            if (Item.GetItemType(this.ItemId, this.PortalId).Equals("ARTICLE", StringComparison.OrdinalIgnoreCase))
            {
                if (this.ItemVersionId == -1)
                {
                    Article a = Article.GetArticle(this.ItemId, this.PortalId, true, true, true);
                    this.lblCurrentVersionComments.Text = a.VersionDescription;
                }
                else
                {
                    Article a = Article.GetArticleVersion(this.ItemVersionId, this.PortalId);
                    this.lblCurrentVersionComments.Text = a.VersionDescription;
                }

                this.divVersionComments.Visible = true;
            }
            else
            {
                this.divVersionComments.Visible = false;
            }

            this.txtApprovalComments.Text = this.VersionInfoObject.ApprovalComments;
        }

        private void AddMenuLink(string text, string navigateUrl)
        {
            this.phLink.Controls.Add(new LiteralControl("<li>"));

            var menuLink = new HyperLink
                {
                    NavigateUrl = navigateUrl, 
                    Text = text
                };
            this.phLink.Controls.Add(menuLink);

            this.phLink.Controls.Add(new LiteralControl("</li>"));
        }

        private void BuildAdminMenu(int itemId, bool isAuthorOnly)
        {
            string currentItemType = Item.GetItemType(itemId, this.PortalId);
            string localizedItemTypeName = Localization.GetString(currentItemType, this.LocalResourceFile);

            // the following dynamicly builds the Admin Menu for an item when viewing the item display control.
            this.AddMenuLink(
                Localization.GetString("AddNew", this.LocalResourceFile) + " " + Localization.GetString("Article", this.LocalResourceFile), 
                this.BuildAddArticleUrl());

            // Article List and Add New should load even if there isn't a valid item.
            this.AddMenuLink(Localization.GetString("ArticleList", this.LocalResourceFile), this.BuildCategoryListUrl());

            if (!currentItemType.Equals("TOPLEVELCATEGORY", StringComparison.OrdinalIgnoreCase))
            {
                if (currentItemType.Equals("ARTICLE", StringComparison.OrdinalIgnoreCase) || !isAuthorOnly || AllowAuthorEditCategory(this.PortalId))
                {
                    this.AddMenuLink(
                        Localization.GetString("Edit", this.LocalResourceFile) + " " + localizedItemTypeName, this.BuildEditUrl(currentItemType));
                }

                if (currentItemType.Equals("CATEGORY", StringComparison.OrdinalIgnoreCase))
                {
                    this.lnkUpdateStatus.Visible = false;
                }

                this.AddMenuLink(
                    localizedItemTypeName + " " + Localization.GetString("Versions", this.LocalSharedResourceFile), this.BuildVersionsUrl());
            }
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", 
            MessageId = "System.Web.UI.ITextControl.set_Text(System.String)", Justification = "Literal, '-', does not change by locale")]
        private void ConfigureMenus()
        {
            int itemId = this.ItemId;
            bool isAuthorOnly = this.IsAuthor && !this.IsAdmin;

            this.divAdminMenu.Visible = true;
            this.lnkUpdateStatus.Visible = !isAuthorOnly && this.UseApprovals;

            // Load stats
            // TODO: hide this if necessary
            this.phStats.Visible = true;
            const string pathToStatsControl = "QuickStats.ascx";
            var statsControl = (ModuleBase)this.LoadControl(pathToStatsControl);
            statsControl.ModuleConfiguration = this.ModuleConfiguration;
            statsControl.ID = Path.GetFileNameWithoutExtension(pathToStatsControl);
            this.phStats.Controls.Add(statsControl);

            this.phLink.Visible = true;

            // TODO: IsNew is itemId != -1, do we need to check both?
            if (itemId != -1 && !this.VersionInfoObject.IsNew)
            {
                this.BuildAdminMenu(itemId, isAuthorOnly);
            }
            else
            {
                // Hide the phAdminControl placeholder for the admin controls.
                var container = (PlaceHolder)this.Parent;
                container.Visible = false;
            }
        }

        private void FillDropDownList()
        {
            this.ApprovalStatusDropDownList.DataSource = DataProvider.Instance().GetApprovalStatusTypes(this.PortalId);
            this.ApprovalStatusDropDownList.DataValueField = "ApprovalStatusID";
            this.ApprovalStatusDropDownList.DataTextField = "ApprovalStatusName";
            this.ApprovalStatusDropDownList.DataBind();

            // set the current approval status
            ListItem li =
                this.ApprovalStatusDropDownList.Items.FindByValue(this.VersionInfoObject.ApprovalStatusId.ToString(CultureInfo.InvariantCulture));
            if (li != null)
            {
                li.Selected = true;
            }
        }

        private void Page_Load(object sender, EventArgs e)
        {
            int itemId = this.ItemId;
            if (itemId != -1 && !this.VersionInfoObject.IsNew)
            {
            }

            try
            {
                // check VI for null then set information
                if (!this.Page.IsPostBack)
                {
                    // check if the user is logged in and an admin. If so let them approve items
                    if (this.IsAdmin && !this.VersionInfoObject.IsNew)
                    {
                        if (this.UseApprovals && Item.GetItemType(itemId, this.PortalId).Equals("ARTICLE", StringComparison.OrdinalIgnoreCase))
                        {
                            // ApprovalStatusDropDownList.Attributes.Clear();
                            // ApprovalStatusDropDownList.Attributes.Add("onchange", "javascript:if (!confirm('" + ClientAPI.GetSafeJSString(Localization.GetString("DeleteConfirmation", LocalResourceFile)) + "')) resetDDLIndex(); else ");

                            // ClientAPI.AddButtonConfirm(ApprovalStatusDropDownList, Localization.GetString("DeleteConfirmation", LocalResourceFile));
                            this.FillDropDownList();
                        }
                        else
                        {
                            this.ApprovalStatusDropDownList.Visible = false;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}