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
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Publish.Data;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.Admin
{
    using System.Web;
    using System.Web.UI;

    public partial class AdminMenu : ModuleBase
    {
        #region Event Handlers
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);

            BindItemData();
            ConfigureMenus();
        }

        private void InitializeComponent()
        {
            this.Load += this.Page_Load;
        }

        private void Page_Load(object sender, System.EventArgs e)
        {
            int itemId = ItemId;
            if (itemId != -1 && !VersionInfoObject.IsNew)
            {
            }

            try
            {
                //check VI for null then set information
                if (!Page.IsPostBack)
                {
                    //check if the user is logged in and an admin. If so let them approve items
                    if (IsAdmin && !VersionInfoObject.IsNew)
                    {
                        if (UseApprovals && Item.GetItemType(itemId,PortalId).Equals("ARTICLE", StringComparison.OrdinalIgnoreCase))
                        {
                            //ddlApprovalStatus.Attributes.Clear();
                            //ddlApprovalStatus.Attributes.Add("onchange", "javascript:if (!confirm('" + ClientAPI.GetSafeJSString(Localization.GetString("DeleteConfirmation", LocalResourceFile)) + "')) resetDDLIndex(); else ");

                            //ClientAPI.AddButtonConfirm(ddlApprovalStatus, Localization.GetString("DeleteConfirmation", LocalResourceFile));
                            FillDropDownList();
                        }
                        else
                        {
                            ddlApprovalStatus.Visible = false;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        //private void LoadToolBar()
        //{
        //    //Register the code for the DNNToolBar being used.
        //    //toolBarActionHandler is a javascript handler, look at the ASCX file for what it currently consists of.

        //    //DNNLabelEdit dle = new DNNLabelEdit();

        //    //if (!ClientAPI.IsClientScriptBlockRegistered(this.Page, "dnn.controls.dnnlabeledit.js"))
        //    //{
        //    //    ClientAPI.RegisterClientScriptBlock(this.Page, "dnn.controls.dnnlabeledit.js", "<script src=\"" + dle.LabelEditScriptPath + "dnn.controls.dnnlabeledit.js\"></script>");
        //    //}
        //    //this.tbEPAdmin.RegisterToolBar(AdminMenuToolBarWrapper, "onmouseover", "onmouseout", "toolBarActionHandler");

        //    if (!Page.IsPostBack)
        //    {
        //        if (ItemId > -1)
        //        {
        //            string currentItemType = Item.GetItemType(ItemId,PortalId);

        //            //Load the Add new Article Link
        //            DNNToolBarButton dtbAddArticle = new DNNToolBarButton();
        //            dtbAddArticle.ControlAction = "navigate";
        //            dtbAddArticle.ID = "dtbAddArticle";
        //            dtbAddArticle.CssClass = "tbButton";
        //            dtbAddArticle.CssClassHover = "tbButtonHover";
        //            dtbAddArticle.NavigateUrl = BuildAddArticleUrl();
        //            dtbAddArticle.Text = Localization.GetString("AddNew", LocalResourceFile) + " " + Localization.GetString("Article", LocalResourceFile);
        //            tbEPAdmin.Buttons.Add(dtbAddArticle);

                    


        //            DNNToolBarButton dtbCategoryList = new DNNToolBarButton();
        //            dtbCategoryList.ControlAction = "navigate";
        //            dtbCategoryList.ID = "dtbCategoryList";
        //            dtbCategoryList.CssClass = "tbButton";
        //            dtbCategoryList.CssClassHover = "tbButtonHover";
        //            dtbCategoryList.NavigateUrl = BuildCategoryListUrl();
        //            dtbCategoryList.Text = Localization.GetString("ArticleList", LocalResourceFile);
        //            tbEPAdmin.Buttons.Add(dtbCategoryList);


        //            //Load toolbar button for Edit Item
        //            DNNToolBarButton dtbEditItem = new DNNToolBarButton();
        //            dtbEditItem.ControlAction = "navigate";
        //            dtbEditItem.ID = "dtbEditItem";
        //            dtbEditItem.CssClass = "tbButton";
        //            dtbEditItem.CssClassHover = "tbButtonHover";
        //            dtbEditItem.NavigateUrl = BuildEditUrl();
        //            dtbEditItem.Text = Localization.GetString("Edit", LocalResourceFile) + " " + Localization.GetString(currentItemType, LocalResourceFile);
        //            tbEPAdmin.Buttons.Add(dtbEditItem);

        //            DNNToolBarButton dtbItemVersions = new DNNToolBarButton();
        //            dtbItemVersions.ControlAction = "navigate";
        //            dtbItemVersions.ID = "dtbItemVersions";
        //            dtbItemVersions.CssClass = "tbButton";
        //            dtbItemVersions.CssClassHover = "tbButtonHover";
        //            dtbItemVersions.NavigateUrl = BuildVersionsUrl();
        //            dtbItemVersions.Text = Localization.GetString(currentItemType, LocalResourceFile) + " " + Localization.GetString("Versions", LocalResourceFile);
        //            tbEPAdmin.Buttons.Add(dtbItemVersions);

        //        }
        //    }
        //}

        private void FillDropDownList()
        {
            
            ddlApprovalStatus.DataSource = DataProvider.Instance().GetApprovalStatusTypes(PortalId); ;
            ddlApprovalStatus.DataValueField = "ApprovalStatusID";
            ddlApprovalStatus.DataTextField = "ApprovalStatusName";
            ddlApprovalStatus.DataBind();
            //set the current approval status
            ListItem li = ddlApprovalStatus.Items.FindByValue(VersionInfoObject.ApprovalStatusId.ToString(CultureInfo.InvariantCulture));
            if (li != null)
            {
                li.Selected = true;
            }
        }

        public string BuildEditUrl(string currentItemType)
        {
            string url = string.Empty;
            try
            {
                //find the location of the ams admin module on the site.
                //DotNetNuke.Entities.Modules.ModuleController objModules = new ModuleController();

                if (this.ItemId > -1)
                {
                    int versionId = -1;
                    if (!this.VersionInfoObject.IsNew)
                    {
                        versionId = this.VersionInfoObject.ItemVersionId;
                    }

                    url = DotNetNuke.Common.Globals.NavigateURL(this.TabId, string.Empty, "ctl=" + Utility.AdminContainer,
                        "mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture), "adminType=" + currentItemType + "Edit",
                        "versionId=" + versionId.ToString(CultureInfo.InvariantCulture), "returnUrl=" + HttpUtility.UrlEncode(this.Request.RawUrl));
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

            return url;
        }

        public string BuildAddArticleUrl()
        {
            if (ItemId > -1)
            {
                int parentCategoryId = -1;

                if (!VersionInfoObject.IsNew)
                {
                    if (VersionInfoObject.ItemTypeId == ItemType.Category.GetId())
                    {
                        parentCategoryId = VersionInfoObject.ItemId;
                    }
                    else
                    {
                        parentCategoryId = VersionInfoObject.GetParentCategoryId();
                    }
                }

                return DotNetNuke.Common.Globals.NavigateURL(this.TabId, string.Empty, "ctl=" + Utility.AdminContainer,
                    "mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture), "adminType=articleedit",
                    "parentId=" + parentCategoryId.ToString(CultureInfo.InvariantCulture), "returnUrl=" + HttpUtility.UrlEncode(Request.RawUrl));
            }
            else
            {
                return string.Empty;
            }
        }


        public string BuildCategoryListUrl()
        {
            //find the location of the ams admin module on the site.
            //DotNetNuke.Entities.Modules.ModuleController objModules = new ModuleController();
            if (ItemId > -1)
            {
                int parentCategoryId = -1;

                if (!VersionInfoObject.IsNew)
                {
                    if (VersionInfoObject.ItemTypeId == ItemType.Category.GetId())
                    {
                        parentCategoryId = VersionInfoObject.ItemId;
                    }
                    else
                    {
                        //find the parent category ID from an item
                        parentCategoryId = Category.GetParentCategory(VersionInfoObject.ItemId, PortalId);
                    }
                }

                //string currentItemType = Item.GetItemType(ItemId,PortalId);
                return DotNetNuke.Common.Globals.NavigateURL(this.TabId, string.Empty, "ctl=" + Utility.AdminContainer,
                    "mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture), "adminType=articlelist",
                    "categoryId=" + parentCategoryId.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                return string.Empty;
            }
        }


        #endregion

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void ddlApprovalStatus_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            CallUpdateApprovalStatus();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.Web.UI.ITextControl.set_Text(System.String)", Justification = "Literal, '-', does not change by locale")]
        private void ConfigureMenus()
        {
            int itemId = ItemId;
            bool isAuthorOnly = this.IsAuthor && !this.IsAdmin;

            divAdminMenu.Visible = true;
            this.lnkUpdateStatus.Visible = !isAuthorOnly && this.UseApprovals;

            // Load stats
            // TODO: hide this if necessary
            phStats.Visible = true;
            const string pathToStatsControl = "QuickStats.ascx";
            ModuleBase statsControl = (ModuleBase)LoadControl(pathToStatsControl);
            statsControl.ModuleConfiguration = ModuleConfiguration;
            statsControl.ID = System.IO.Path.GetFileNameWithoutExtension(pathToStatsControl);
            phStats.Controls.Add(statsControl);

            phLink.Visible = true;

            // TODO: IsNew is itemId != -1, do we need to check both?
            if (itemId != -1 && !VersionInfoObject.IsNew)
            {
                this.BuildAdminMenu(itemId, isAuthorOnly);
            }
            else
            {
                //Hide the phAdminControl placeholder for the admin controls.
                PlaceHolder container = (PlaceHolder)this.Parent;
                container.Visible = false;
            }
        }

        private void BuildAdminMenu(int itemId, bool isAuthorOnly)
        {
            string currentItemType = Item.GetItemType(itemId, this.PortalId);
            string localizedItemTypeName = Localization.GetString(currentItemType, this.LocalResourceFile);

            // the following dynamicly builds the Admin Menu for an item when viewing the item display control.
            this.AddMenuLink(Localization.GetString("AddNew", this.LocalResourceFile) + " " + Localization.GetString("Article", this.LocalResourceFile), this.BuildAddArticleUrl());

            //Article List and Add New should load even if there isn't a valid item.
            this.AddMenuLink(Localization.GetString("ArticleList", this.LocalResourceFile), this.BuildCategoryListUrl());

            if (!currentItemType.Equals("TOPLEVELCATEGORY", StringComparison.OrdinalIgnoreCase))
            {
                if (currentItemType.Equals("ARTICLE", StringComparison.OrdinalIgnoreCase) || !isAuthorOnly || AllowAuthorEditCategory(this.PortalId))
                {
                    this.AddMenuLink(Localization.GetString("Edit", this.LocalResourceFile) + " " + localizedItemTypeName, this.BuildEditUrl(currentItemType));
                }

                if (currentItemType.Equals("CATEGORY", StringComparison.OrdinalIgnoreCase))
                {
                    this.lnkUpdateStatus.Visible = false;
                }

                this.AddMenuLink(localizedItemTypeName + " " + Localization.GetString("Versions", LocalSharedResourceFile), this.BuildVersionsUrl());
            }
        }

        private void AddMenuLink(string text, string navigateUrl)
        {
            this.phLink.Controls.Add(new LiteralControl("<li>"));

            HyperLink menuLink = new HyperLink();
            menuLink.NavigateUrl = navigateUrl;
            menuLink.Text = text;
            this.phLink.Controls.Add(menuLink);

            this.phLink.Controls.Add(new LiteralControl("</li>"));
        }

        protected void lnkSaveApprovalStatus_Click(object sender, EventArgs e)
        {
            CallUpdateApprovalStatus();
        }

        protected void CallUpdateApprovalStatus()
        {
            if (!VersionInfoObject.IsNew)
            {
                VersionInfoObject.ApprovalStatusId = Convert.ToInt32(ddlApprovalStatus.SelectedValue, CultureInfo.InvariantCulture);
                if (txtApprovalComments.Text.Trim().Length > 0)
                {
                    VersionInfoObject.ApprovalComments = txtApprovalComments.Text.Trim();
                }
                else
                {
                    VersionInfoObject.ApprovalComments = Localization.GetString("DefaultApprovalComment", LocalResourceFile);
                }
                VersionInfoObject.UpdateApprovalStatus();

                //Utility.ClearPublishCache(PortalId);

                Response.Redirect(BuildVersionsUrl(), false);

                //redirect to the versions list for this item.
            }
        }

        protected void lnkUpdateStatus_Click(object sender, EventArgs e)
        {
            divApprovalStatus.Visible = true;
            
            //check if we're editing an article, if so show version comments
            if (Item.GetItemType(ItemId, PortalId).Equals("ARTICLE", StringComparison.OrdinalIgnoreCase))
            {
                if (ItemVersionId == -1)
                {
                    Article a = Article.GetArticle(ItemId, PortalId, true, true, true);
                    lblCurrentVersionComments.Text = a.VersionDescription;
                }
                else
                {
                    Article a = Article.GetArticleVersion(ItemVersionId, PortalId);
                    lblCurrentVersionComments.Text = a.VersionDescription;
                }
                

                divVersionComments.Visible = true;
            }
            else
            {
                divVersionComments.Visible = false;
            }

            txtApprovalComments.Text = this.VersionInfoObject.ApprovalComments;
        }

        protected void lnkSaveApprovalStatusCancel_Click(object sender, EventArgs e)
        {
            divApprovalStatus.Visible = false;
        }
    }
}

