// <copyright file="CommentList.ascx.cs" company="Engage Software">
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
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;
    using Engage.Dnn.UserFeedback;

    using DataProvider = Engage.Dnn.Publish.Data.DataProvider;

    public partial class CommentList : ModuleBase, IActionable
    {
        public ModuleActionCollection ModuleActions
        {
            get
            {
                return new ModuleActionCollection
                    {
                        {
                            this.GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile), 
                            ModuleActionType.AddContent, string.Empty, string.Empty, string.Empty, false, SecurityAccessLevel.Edit, true, false
                            }
                    };
            }
        }

        private int CategoryId
        {
            get
            {
                string id = this.Request.QueryString["categoryid"];
                return id == null ? -1 : Convert.ToInt32(id, CultureInfo.InvariantCulture);
            }
        }

        // private string CategoryName
        // {
        // get	{return (Convert.ToString(Request.QueryString["category"]));}
        // }

        // private int TopLevelId
        // {
        // get	
        // {
        // string s = Request.QueryString["topLevelId"];
        // return (s == null ? -1 : Convert.ToInt32(s));
        // }
        // }
        protected static string GetCommentText(object commentText)
        {
            return commentText != null ? HtmlUtils.StripTags(commentText.ToString(), true) : string.Empty;
        }

        protected static string GetShortCommentText(object commentText)
        {
            return commentText != null
                       ? HtmlUtils.Shorten(HtmlUtils.StripTags(commentText.ToString(), true), 200, string.Empty) + "&nbsp"
                       : string.Empty;
        }

        protected string BuildName(object firstName, object lastName)
        {
            return string.Format(CultureInfo.CurrentCulture, Localization.GetString("NameFormat", this.LocalResourceFile), firstName, lastName);
        }

        protected string GetCommentEditUrl(object commentId)
        {
            return commentId != null
                       ? this.BuildLinkUrl(
                           "&ctl=" + Utility.AdminContainer + "&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) +
                           "&adminType=commentEdit&commentid=" + commentId)
                       : string.Empty;
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Does not return class state information")]
        protected string GetLocalizedEditText()
        {
            return Localization.GetString("Edit", this.LocalResourceFile);
        }

        protected override void OnInit(EventArgs e)
        {
            this.cboCategories.SelectedIndexChanged += this.cboCategories_SelectedIndexChanged;
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            this.BindData();
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void cboWorkflow_SelectedIndexChanged(object sender, EventArgs e)
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
                    var commentId = (Label)gvr.FindControl("lblCommentId");
                    var cb = (CheckBox)gvr.FindControl("chkSelect");
                    if (commentId != null && cb != null && cb.Checked)
                    {
                        // approve
                        Comment c = Comment.GetComment(Convert.ToInt32(commentId.Text, CultureInfo.CurrentCulture), DataProvider.ModuleQualifier);
                        c.ApprovalStatusId = ApprovalStatus.Approved.GetId();
                        c.Save(DataProvider.ModuleQualifier);

                        // TODO: we need to increment the comment count when approved.
                    }
                }

                Utility.ClearPublishCache(this.PortalId);
                this.BindData();

                this.lblMessage.Text = Localization.GetString("CommentsApproved", this.LocalResourceFile);

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
                    var lblCommentId = (Label)gvr.FindControl("lblCommentId");
                    var chkSelect = (CheckBox)gvr.FindControl("chkSelect");
                    if (lblCommentId != null && chkSelect != null && chkSelect.Checked)
                    {
                        // approve
                        Comment c = Comment.GetComment(Convert.ToInt32(lblCommentId.Text, CultureInfo.CurrentCulture), DataProvider.ModuleQualifier);
                        c.ApprovalStatusId = ApprovalStatus.Approved.GetId();
                        c.Delete(DataProvider.ModuleQualifier);
                    }
                }

                this.BindData();
                this.lblMessage.Text = Localization.GetString("CommentsDeleted", this.LocalResourceFile);

                this.lblMessage.Visible = true;
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void dgItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.BindData();
            this.dgItems.PageIndex = e.NewPageIndex;
            this.dgItems.DataBind();
        }

        private void BindData()
        {
            int categoryId = Convert.ToInt32(this.cboCategories.SelectedValue, CultureInfo.InvariantCulture);
            int authorUserId = -1;
            if (!this.IsAdmin)
            {
                authorUserId = this.UserId;
            }

            string articleSearch = string.Empty;

            if (this.txtArticleSearch.Text.Trim() != string.Empty)
            {
                var objSecurity = new PortalSecurity();
                articleSearch = objSecurity.InputFilter(this.txtArticleSearch.Text.Trim(), PortalSecurity.FilterFlag.NoSQL);
            }

            DataSet ds = DataProvider.Instance().GetAdminCommentListing(
                categoryId, Convert.ToInt32(this.cboWorkflow.SelectedValue, CultureInfo.InvariantCulture), this.PortalId, authorUserId, articleSearch);
            if (ds.Tables[0].Rows.Count == 0)
            {
                this.lblMessage.Text = Localization.GetString("NoCommentsFound", this.LocalResourceFile) + " " + this.cboCategories.SelectedItem;
                this.dgItems.Visible = false;
                this.lblMessage.Visible = true;
            }
            else
            {
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "CreatedDate desc";
                this.dgItems.DataSource = dv;
                this.dgItems.DataBind();

                this.dgItems.Visible = true;
                this.lblMessage.Visible = false;
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

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.Page.IsPostBack)
                {
                    Utility.LocalizeGridView(this.dgItems, this.LocalResourceFile);
                    this.FillDropDown();
                    this.BindData();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void cboCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindData();
        }
    }
}