//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
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
using DotNetNuke.UI.Utilities;
using DotNetNuke.UI.WebControls;
using Engage.Dnn.Publish.Data;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.Admin
{
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
                        if (UseApprovals && Item.GetItemType(itemId).Equals("ARTICLE", StringComparison.OrdinalIgnoreCase))
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
        //            string currentItemType = Item.GetItemType(ItemId);

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

        public string BuildEditUrl()
        {
            string url = string.Empty;
            try
            {
                //find the location of the ams admin module on the site.
                //DotNetNuke.Entities.Modules.ModuleController objModules = new ModuleController();
                int edittabid = TabId;


                if (ItemId > -1)
                {
                    string currentItemType = Item.GetItemType(ItemId);
                    int versionId = -1;
                    if (!VersionInfoObject.IsNew)
                    {
                        versionId = VersionInfoObject.ItemVersionId;
                    }

                    url = DotNetNuke.Common.Globals.NavigateURL(edittabid, "", "ctl=" + Utility.AdminContainer,
                        "mid=" + ModuleId.ToString(CultureInfo.InvariantCulture), "adminType=" + currentItemType + "Edit",
                        "versionId=" + versionId.ToString(CultureInfo.InvariantCulture), "returnUrl=" + Server.UrlEncode(Request.RawUrl));
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

                return DotNetNuke.Common.Globals.NavigateURL(TabId, "", "ctl=" + Utility.AdminContainer,
                    "mid=" + ModuleId.ToString(CultureInfo.InvariantCulture), "adminType=articleedit",
                    "parentId=" + parentCategoryId.ToString(CultureInfo.InvariantCulture), "returnUrl=" + Server.UrlEncode(Request.RawUrl));
            }
            else
            {
                return string.Empty;
            }
        }

        public string BuildVersionsUrl()
        {
            //find the location of the ams admin module on the site.
            //DotNetNuke.Entities.Modules.ModuleController objModules = new ModuleController();
            int edittabid = TabId;
            if (ItemId > -1)
            {
                //string currentItemType = Item.GetItemType(ItemId);
                int itemId = -1;
                if (!VersionInfoObject.IsNew)
                {
                    itemId = VersionInfoObject.ItemId;
                }

                return (DotNetNuke.Common.Globals.NavigateURL(edittabid, "", "&ctl=" + Utility.AdminContainer + "&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&adminType=VersionsList&itemId=" + itemId.ToString(CultureInfo.InvariantCulture)));
            }
            else return "";
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

                //string currentItemType = Item.GetItemType(ItemId);
                return DotNetNuke.Common.Globals.NavigateURL(TabId, string.Empty, "ctl=" + Utility.AdminContainer,
                    "mid=" + ModuleId.ToString(CultureInfo.InvariantCulture), "adminType=articlelist",
                    "categoryId=" + parentCategoryId.ToString(CultureInfo.InvariantCulture));
            }
            else return "";
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


            divAdminMenu.Visible = true;

            if ((IsAuthor && !IsAdmin) || !UseApprovals)
            {
                lnkUpdateStatus.Visible = false;
            }
            else
            {
                lnkUpdateStatus.Visible = true;

            }

            //Load stats
            //TODO: hide this if necessary
            phStats.Visible = true;
            string statControlToLoad = "QuickStats.ascx";
            ModuleBase mbl = (ModuleBase)LoadControl(statControlToLoad);
            mbl.ModuleConfiguration = ModuleConfiguration;
            mbl.ID = System.IO.Path.GetFileNameWithoutExtension(statControlToLoad);
            phStats.Controls.Add(mbl);

            phLink.Visible = true;


            if (itemId != -1 && !VersionInfoObject.IsNew)
            {
                string currentItemType = Item.GetItemType(itemId);

                //the following dynamicly builds the Admin Menu for an item when viewing the item display control.
                Literal lc = new Literal();
                lc.Text = "<li>";

                phLink.Controls.Add(lc);

                HyperLink itemAdd = new HyperLink();
                itemAdd.NavigateUrl = BuildAddArticleUrl();
                itemAdd.Text = Localization.GetString("AddNew", LocalResourceFile) + " " + Localization.GetString("Article", LocalResourceFile);
                phLink.Controls.Add(itemAdd);
                lc = new Literal();
                lc.Text = "</li><li>";

                phLink.Controls.Add(lc);

                //Article List and Add New should load even if there isn't a valid item.
                HyperLink itemList = new HyperLink();
                itemList.NavigateUrl = BuildCategoryListUrl();
                itemList.Text = Localization.GetString("ArticleList", LocalResourceFile);
                phLink.Controls.Add(itemList);
                lc = new Literal();
                lc.Text = "</li><li>";

                phLink.Controls.Add(lc);

                if (currentItemType.Equals("CATEGORY", StringComparison.OrdinalIgnoreCase))
                {
                    lnkUpdateStatus.Visible = false;
                    if ((IsAuthor && !IsAdmin) && !AllowAuthorEditCategory(PortalId))
                    {


                    }
                    else
                    {
                        HyperLink itemEdit = new HyperLink();
                        itemEdit.NavigateUrl = BuildEditUrl();
                        itemEdit.Text = Localization.GetString("Edit", LocalResourceFile) + " " + Localization.GetString(currentItemType, LocalResourceFile);
                        phLink.Controls.Add(itemEdit);
                        lc = new Literal();
                        lc.Text = "</li><li>";

                        phLink.Controls.Add(lc);
                    }
                }
                else
                {
                    HyperLink itemEdit = new HyperLink();
                    itemEdit.NavigateUrl = BuildEditUrl();
                    itemEdit.Text = Localization.GetString("Edit", LocalResourceFile) + " " + Localization.GetString(currentItemType, LocalResourceFile);
                    phLink.Controls.Add(itemEdit);
                    lc = new Literal();
                    lc.Text = "</li><li>";

                    phLink.Controls.Add(lc);
                }


                HyperLink itemVersions = new HyperLink();
                itemVersions.NavigateUrl = BuildVersionsUrl();
                itemVersions.Text = Localization.GetString(currentItemType, LocalResourceFile) + " " + Localization.GetString("Versions", LocalResourceFile);
                phLink.Controls.Add(itemVersions);

                lc = new Literal();
                lc.Text = "</li>";

                phLink.Controls.Add(lc);

                //Label l1 = new Label();
                //l1.Text = " - ";
                //phLink.Controls.Add(l1);


                //Label l2 = new Label();
                //l2.Text = " - ";
                //phLink.Controls.Add(l2);

                
                //Label l3 = new Label();
                //l3.Text = " - ";
                //phLink.Controls.Add(l3);


               
                
            }
            else
            {
                //Hide the phAdminControl placeholder for the admin controls.
                PlaceHolder container = (PlaceHolder)this.Parent;
                container.Visible = false;
            }


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

                Utility.ClearPublishCache(PortalId);

                Response.Redirect(BuildVersionsUrl(), false);

                //redirect to the versions list for this item.
            }
        }

        protected void lnkUpdateStatus_Click(object sender, EventArgs e)
        {
            divApprovalStatus.Visible = true;
            
            //check if we're editing an article, if so show version comments
            if (Item.GetItemType(ItemId).Equals("ARTICLE", StringComparison.OrdinalIgnoreCase))
            {
                if (ItemVersionId == -1)
                {
                    Article a = Article.GetArticle(ItemId, PortalId);
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

