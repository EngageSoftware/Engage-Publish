//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.ArticleControls
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.Utilities;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Util;

    public partial class ArticleList : ModuleBase
    {
        private int CategoryId
        {
            get
            {
                string id = this.Request.QueryString["categoryid"];
                return id == null ? -1 : Convert.ToInt32(id, CultureInfo.InvariantCulture);
            }
        }

        private string GridViewSortDirection
        {
            get { return this.ViewState["SortDirection"] as string ?? "ASC"; }
            set { this.ViewState["SortDirection"] = value; }
        }

        private string GridViewSortExpression
        {
            get { return this.ViewState["SortExpression"] as string ?? string.Empty; }
            set { this.ViewState["SortExpression"] = value; }
        }

        // private string CategoryName
        // {
        // get	{return (Convert.ToString(Request.QueryString["category"]));}
        // }
        private int TopLevelId
        {
            get
            {
                string s = this.Request.QueryString["topLevelId"];
                return s == null ? -1 : Convert.ToInt32(s, CultureInfo.InvariantCulture);
            }
        }

        protected static string GetDescription(object description)
        {
            if (description != null)
            {
                return HtmlUtils.Shorten(HtmlUtils.Clean(description.ToString(), true), 200, string.Empty) + "&nbsp";
            }

            return string.Empty;
        }

        protected string GetArticleEditUrl(object itemVersionId)
        {
            if (itemVersionId != null)
            {
                return
                    this.BuildLinkUrl(
                        "&ctl=" + Utility.AdminContainer + "&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) +
                        "&adminType=articleEdit&versionid=" + itemVersionId);
            }

            return string.Empty;
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Does not return class state information")]
        protected string GetLocalizedEditText()
        {
            return Localization.GetString("Edit", this.LocalResourceFile);
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Does not return class state information")]
        protected string GetLocalizedVersionText()
        {
            return Localization.GetString("Versions", this.LocalSharedResourceFile);
        }

        protected string GetVersionsUrl(object itemId)
        {
            if (itemId != null)
            {
                string categoryId = this.cboCategories.SelectedValue.ToString(CultureInfo.InvariantCulture);
                return
                    this.BuildLinkUrl(
                        "&ctl=" + Utility.AdminContainer + "&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) +
                        "&adminType=versionslist&itemid=" + itemId + "&categoryid=" + categoryId);
            }

            return string.Empty;
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        protected DataView SortDataTable(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                var dataView = new DataView(dataTable);
                if (!string.IsNullOrEmpty(this.GridViewSortExpression))
                {
                    dataView.Sort = isPageIndexChanging
                                        ? string.Format(
                                            CultureInfo.InvariantCulture, "{0} {1}", this.GridViewSortExpression, this.GridViewSortDirection)
                                        : string.Format(CultureInfo.InvariantCulture, "{0} {1}", this.GridViewSortExpression, this.GetSortDirection());
                }

                return dataView;
            }

            return new DataView();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void cmdApprove_Click(object sender, EventArgs e)
        {
            // parse through the checked items in the list and approve them.
            try
            {
                foreach (GridViewRow gvr in this.dgItems.Rows)
                {
                    var lblItemVersionId = (Label)gvr.FindControl("lblItemVersionId");
                    var cb = (CheckBox)gvr.FindControl("chkSelect");
                    if (lblItemVersionId != null && cb != null && cb.Checked)
                    {
                        // approve
                        var a = Article.GetArticleVersion(Convert.ToInt32(lblItemVersionId.Text), this.PortalId);
                        a.ApprovalStatusId = ApprovalStatus.Approved.GetId();
                        a.UpdateApprovalStatus();
                    }
                }

                // Utility.ClearPublishCache(PortalId);
                this.BindData();
                this.lblMessage.Text = Localization.GetString("ArticlesApproved", this.LocalResourceFile);
                this.lblMessage.Visible = true;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void cmdArchive_Click(object sender, EventArgs e)
        {
            // parse through the checked items in the list and archive them.
            try
            {
                foreach (GridViewRow gvr in this.dgItems.Rows)
                {
                    var hlId = (HyperLink)gvr.FindControl("hlId");
                    var cb = (CheckBox)gvr.FindControl("chkSelect");
                    if (hlId != null && cb != null && cb.Checked)
                    {
                        // approve
                        var a = (Article)Item.GetItem(Convert.ToInt32(hlId.Text), this.PortalId, ItemType.Article.GetId(), false);
                        a.ApprovalStatusId = ApprovalStatus.Archived.GetId();
                        a.UpdateApprovalStatus();
                    }
                }

                // Utility.ClearPublishCache(PortalId);
                this.BindData();
                this.lblMessage.Text = Localization.GetString("ArticlesArchived", this.LocalResourceFile);
                this.lblMessage.Visible = true;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void cmdBack_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(this.BuildLinkUrl(string.Empty), true);
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow gvr in this.dgItems.Rows)
                {
                    var hlId = (HyperLink)gvr.FindControl("hlId");
                    var chkSelect = (CheckBox)gvr.FindControl("chkSelect");
                    if (hlId != null && chkSelect != null && chkSelect.Checked)
                    {
                        Item.DeleteItem(Convert.ToInt32(hlId.Text, CultureInfo.CurrentCulture), this.PortalId);
                    }
                }

                // Utility.ClearPublishCache(PortalId);
                this.BindData();
                this.lblMessage.Text = Localization.GetString("ArticlesDeleted", this.LocalResourceFile);
                this.lblMessage.Visible = true;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void dgItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.dgItems.DataSource = this.SortDataTable(this.GetGridData(), true);
            this.dgItems.PageIndex = e.NewPageIndex;
            this.dgItems.DataBind();
        }

        protected void dgItems_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridViewSortExpression = e.SortExpression;
            int pageIndex = this.dgItems.PageIndex;
            this.dgItems.DataSource = this.SortDataTable(this.GetGridData(), true);

            // dgItems.DataSource = SortDataTable(dgItems.DataSource as DataTable, false);
            this.dgItems.DataBind();
            this.dgItems.PageIndex = pageIndex;
        }

        private void BindData()
        {
            this.dgItems.DataSource = this.GetGridData();
            this.dgItems.DataBind();

            this.ConfigureAddLink();

            this.dgItems.Visible = true;
            this.lblMessage.Visible = false;

            if (this.dgItems.Rows.Count < 1)
            {
                this.lblMessage.Text = this.UseApprovals
                                           ? String.Format(
                                               CultureInfo.CurrentCulture, 
                                               Localization.GetString("NoArticlesFound", this.LocalResourceFile), 
                                               this.cboCategories.SelectedItem, 
                                               this.cboWorkflow.SelectedItem)
                                           : String.Format(
                                               CultureInfo.CurrentCulture, 
                                               Localization.GetString("NoArticlesFoundNoApproval", this.LocalResourceFile), 
                                               this.cboCategories.SelectedItem);

                this.dgItems.Visible = false;
                this.lblMessage.Visible = true;
            }
        }

        private void CboCategoriesSelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData();
        }

        private void CboWorkflowSelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData();
        }

        private void ConfigureAddLink()
        {
            if (this.TopLevelId == -1)
            {
                string s = this.cboCategories.SelectedValue;
                int categoryId = Engage.Utility.HasValue(s) ? Convert.ToInt32(s, CultureInfo.InvariantCulture) : -1;
                if (categoryId == -1)
                {
                    if (this.CategoryId > -1)
                    {
                        this.lnkAddNewArticle.NavigateUrl =
                            this.BuildLinkUrl(
                                "&ctl=" + Utility.AdminContainer + "&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) +
                                "&adminType=articleEdit&topLevelId=" + this.CategoryId.ToString(CultureInfo.InvariantCulture) + "&parentId=" +
                                this.CategoryId.ToString(CultureInfo.InvariantCulture));
                        this.lnkAddNewArticle.Visible = true;
                    }
                    else
                    {
                        this.lnkAddNewArticle.NavigateUrl =
                            this.BuildLinkUrl(
                                "&ctl=" + Utility.AdminContainer + "&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) +
                                "&adminType=articleEdit");
                        this.lnkAddNewArticle.Visible = false;
                    }
                }
                else
                {
                    this.lnkAddNewArticle.NavigateUrl =
                        this.BuildLinkUrl(
                            "&ctl=" + Utility.AdminContainer + "&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) +
                            "&adminType=articleEdit&topLevelId=" + categoryId.ToString(CultureInfo.InvariantCulture) + "&parentId=" +
                            categoryId.ToString(CultureInfo.InvariantCulture));
                    this.lnkAddNewArticle.Visible = true;
                }
            }
            else
            {
                this.lnkAddNewArticle.NavigateUrl =
                    this.BuildLinkUrl(
                        "&ctl=" + Utility.AdminContainer + "&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) +
                        "&adminType=articleEdit&topLevelId=" + this.TopLevelId.ToString(CultureInfo.InvariantCulture) + "&parentId=" +
                        this.CategoryId.ToString(CultureInfo.InvariantCulture));
                this.lnkAddNewArticle.Visible = true;
            }
        }

        private void FillDropDown()
        {
            ItemRelationship.DisplayCategoryHierarchy(this.cboCategories, -1, this.PortalId, false);

            var li = new ListItem(Localization.GetString("ChooseOne", this.LocalResourceFile), "-1");
            this.cboCategories.Items.Insert(0, li);

            li = this.cboCategories.Items.FindByValue(this.CategoryId.ToString(CultureInfo.InvariantCulture));
            if (li != null)
            {
                li.Selected = true;
            }

            this.cboWorkflow.Visible = this.UseApprovals;
            this.lblWorkflow.Visible = this.UseApprovals;
            if (this.UseApprovals)
            {
                this.cboWorkflow.DataSource = DataProvider.Instance().GetApprovalStatusTypes(this.PortalId);
                this.cboWorkflow.DataValueField = "ApprovalStatusID";
                this.cboWorkflow.DataTextField = "ApprovalStatusName";
                this.cboWorkflow.DataBind();
                li = this.cboWorkflow.Items.FindByText(ApprovalStatus.Waiting.Name);
                if (li != null)
                {
                    li.Selected = true;
                }
            }
        }

        private DataTable GetGridData()
        {
            int categoryId = Convert.ToInt32(this.cboCategories.SelectedValue, CultureInfo.InvariantCulture);

            // set the approval status ID to approved by default, if we're using approvals look for the selected value
            int approvalStatusId = ApprovalStatus.Approved.GetId();

            if (this.UseApprovals)
            {
                approvalStatusId = Convert.ToInt32(this.cboWorkflow.SelectedValue, CultureInfo.InvariantCulture);
            }

            this.dgItems.DataSourceID = string.Empty;
            DataSet ds;
            if (this.txtArticleSearch.Text.Trim() != string.Empty)
            {
                var objSecurity = new PortalSecurity();
                string searchKey = objSecurity.InputFilter(this.txtArticleSearch.Text.Trim(), PortalSecurity.FilterFlag.NoSQL);
                ds = DataProvider.Instance().GetAdminItemListingSearchKey(
                    categoryId, 
                    ItemType.Article.GetId(), 
                    RelationshipType.ItemToParentCategory.GetId(), 
                    RelationshipType.ItemToRelatedCategory.GetId(), 
                    approvalStatusId, 
                    " vi.createddate desc ", 
                    searchKey, 
                    this.PortalId);
            }
            else
            {
                ds = DataProvider.Instance().GetAdminItemListing(
                    categoryId, 
                    ItemType.Article.GetId(), 
                    RelationshipType.ItemToParentCategory.GetId(), 
                    RelationshipType.ItemToRelatedCategory.GetId(), 
                    approvalStatusId, 
                    " vi.createddate desc ", 
                    this.PortalId);
            }


            return ds.Tables[0];
        }

        private string GetSortDirection()
        {
            switch (this.GridViewSortDirection)
            {
                case "ASC":
                    this.GridViewSortDirection = "DESC";
                    break;

                case "DESC":
                    this.GridViewSortDirection = "ASC";
                    break;
            }

            return this.GridViewSortDirection;
        }

        private void InitializeComponent()
        {
            this.cboCategories.SelectedIndexChanged += this.CboCategoriesSelectedIndexChanged;
            this.cboWorkflow.SelectedIndexChanged += this.CboWorkflowSelectedIndexChanged;

            this.Load += this.Page_Load;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClientAPI.AddButtonConfirm(this.cmdDelete, Localization.GetString("DeleteConfirm", this.LocalResourceFile));
                if (!this.Page.IsPostBack)
                {
                    Utility.LocalizeGridView(this.dgItems, this.LocalResourceFile);
                    this.ConfigureAddLink();
                    this.FillDropDown();
                    this.BindData();
                }

                if (this.IsAdmin)
                {
                    this.cmdApprove.Visible = this.cmdArchive.Visible = this.cmdDelete.Visible = true;
                }
                else
                {
                    this.cmdApprove.Visible = this.cmdArchive.Visible = this.cmdDelete.Visible = false;
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}