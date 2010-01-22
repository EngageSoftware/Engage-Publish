//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.


namespace Engage.Dnn.Publish.Admin
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Web.UI.WebControls;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Data;
    using Util;

	public partial class CommentList :  ModuleBase, IActionable
	{
		#region Event Handlers
		override protected void OnInit(EventArgs e)
		{
		    this.cboCategories.SelectedIndexChanged += this.cboCategories_SelectedIndexChanged;
		    Load += Page_Load;
		    base.OnInit(e);
		}

	    private void Page_Load(object sender, EventArgs e)
		{
			try 
			{
				if (!Page.IsPostBack)
				{
                    Utility.LocalizeGridView(dgItems, LocalResourceFile);
                    FillDropDown();
                    BindData();
				}	
			} 
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

        private void cboCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void cmdBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(BuildLinkUrl(string.Empty), true);
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void cmdApprove_Click(object sender, EventArgs e)
        {
            //parse through the checked items in the list and approve them.
            try
            {
                foreach (GridViewRow gvr in dgItems.Rows)
                {
                    var commentId = (Label)gvr.FindControl("lblCommentId");
                    var cb = (CheckBox)gvr.FindControl("chkSelect");
                    if (commentId != null && cb!=null && cb.Checked)
                    {
                        //approve
                        UserFeedback.Comment c = UserFeedback.Comment.GetComment(Convert.ToInt32(commentId.Text, CultureInfo.CurrentCulture), DataProvider.ModuleQualifier);
                        c.ApprovalStatusId = ApprovalStatus.Approved.GetId();
                        c.Save(DataProvider.ModuleQualifier);

                        //TODO: we need to increment the comment count when approved.
                    }
                }

                Utility.ClearPublishCache(PortalId);
                BindData();
                
                this.lblMessage.Text = Localization.GetString("CommentsApproved", LocalResourceFile);
                
                lblMessage.Visible = true;

                
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow gvr in dgItems.Rows)
                {
                    var lblCommentId = (Label)gvr.FindControl("lblCommentId");
                    var chkSelect = (CheckBox)gvr.FindControl("chkSelect");
                    if (lblCommentId != null && chkSelect != null && chkSelect.Checked)
                    {
                        //approve
                        UserFeedback.Comment c = UserFeedback.Comment.GetComment(Convert.ToInt32(lblCommentId.Text, CultureInfo.CurrentCulture), DataProvider.ModuleQualifier);
                        c.ApprovalStatusId = ApprovalStatus.Approved.GetId();
                        c.Delete(DataProvider.ModuleQualifier);
                    }
                }

                BindData();
                this.lblMessage.Text = Localization.GetString("CommentsDeleted", LocalResourceFile);

                lblMessage.Visible = true;

            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }


        protected void dgItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            BindData();
            dgItems.PageIndex = e.NewPageIndex;
            dgItems.DataBind();
        }

		#endregion

        #region Private Methods

        private void FillDropDown()
        {

            ItemRelationship.DisplayCategoryHierarchy(cboCategories, -1, PortalId, false);

            var li = new ListItem(Localization.GetString("ChooseOne", LocalResourceFile), "-1");
            this.cboCategories.Items.Insert(0, li);

            li = cboCategories.Items.FindByValue(CategoryId.ToString(CultureInfo.InvariantCulture));
            if (li != null)
            {
                li.Selected = true;
            }

            cboWorkflow.DataSource = DataProvider.Instance().GetApprovalStatusTypes(PortalId);
            cboWorkflow.DataValueField = "ApprovalStatusID";
            cboWorkflow.DataTextField = "ApprovalStatusName";
            cboWorkflow.DataBind();
            li = cboWorkflow.Items.FindByText(ApprovalStatus.Waiting.Name);
            if (li != null)
            {
                li.Selected = true;
            }
        }

        private void BindData()
		{
            int categoryId = Convert.ToInt32(this.cboCategories.SelectedValue, CultureInfo.InvariantCulture);
            int authorUserId = -1;
            if (!IsAdmin)
                authorUserId = UserId;

            string articleSearch = string.Empty;
            
            if (txtArticleSearch.Text.Trim() != string.Empty)
            {
                var objSecurity = new DotNetNuke.Security.PortalSecurity();
                articleSearch = objSecurity.InputFilter(txtArticleSearch.Text.Trim(),DotNetNuke.Security.PortalSecurity.FilterFlag.NoSQL);
            }

			DataSet ds = DataProvider.Instance().GetAdminCommentListing(categoryId, Convert.ToInt32(cboWorkflow.SelectedValue, CultureInfo.InvariantCulture), PortalId, authorUserId, articleSearch);
            if (ds.Tables[0].Rows.Count == 0)
            {
                this.lblMessage.Text = Localization.GetString("NoCommentsFound", LocalResourceFile) + " " + this.cboCategories.SelectedItem;
                dgItems.Visible = false;
                lblMessage.Visible = true;
            }
            else
            {
                DataView dv = ds.Tables[0].DefaultView;
                dv.Sort = "CreatedDate desc";
                dgItems.DataSource = dv;
                dgItems.DataBind();

                dgItems.Visible = true;
                lblMessage.Visible = false;
            }
    	}
              

        private int CategoryId
		{
			get
			{
				string id = Request.QueryString["categoryid"];
				return (id == null ? -1 : Convert.ToInt32(id, CultureInfo.InvariantCulture));
			}
		}

        //private string CategoryName
        //{
        //    get	{return (Convert.ToString(Request.QueryString["category"]));}
        //}

        //private int TopLevelId
        //{
        //    get	
        //    {
        //        string s = Request.QueryString["topLevelId"];
        //        return (s == null ? -1 : Convert.ToInt32(s));
        //    }
        //}

        #endregion

        #region Protected Methods

        protected static string GetCommentText(object commentText)
        {
            return commentText != null ? HtmlUtils.StripTags(commentText.ToString(), true) : string.Empty;
        }

        protected static string GetShortCommentText(object commentText)
        {
            return commentText != null ? HtmlUtils.Shorten(HtmlUtils.StripTags(commentText.ToString(), true), 200, string.Empty) + "&nbsp" : string.Empty;
        }

        protected string GetCommentEditUrl(object commentId)
        {
            return commentId != null ? 
                BuildLinkUrl("&ctl=" + Utility.AdminContainer + "&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&adminType=commentEdit&commentid=" + commentId) : 
                string.Empty;
        }

	    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Does not return class state information")]
        protected string GetLocalizedEditText()
        {
            return Localization.GetString("Edit", LocalResourceFile);
        }

        protected string BuildName(object firstName, object lastName)
        {
            return String.Format(CultureInfo.CurrentCulture, Localization.GetString("NameFormat", LocalResourceFile), firstName, lastName);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void cboWorkflow_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        #endregion

        #region Optional Interfaces

        public ModuleActionCollection ModuleActions
        {
            get
            {
                return new ModuleActionCollection
                           {
                                   {
                                           this.GetNextActionID(),
                                           Localization.GetString(
                                           ModuleActionType.AddContent, this.LocalResourceFile),
                                           DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "",
                                           "", "", false, DotNetNuke.Security.SecurityAccessLevel.Edit, true,
                                           false
                                           }
                           };
            }
        }

       
        #endregion

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            BindData();
        }
    }
}