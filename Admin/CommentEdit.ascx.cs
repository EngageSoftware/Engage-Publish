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
    using System.Globalization;
    using System.Web.UI;
    using DotNetNuke.Common;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.UserControls;
    using Publish;
    using Controls;
    using Data;
    using Util;


	public partial class CommentEdit :  ModuleBase, IActionable
	{
        private UserFeedback.Comment thisComment;
        protected ItemApproval Ias;
    	protected TextEditor TeArticleText;
		private const string ApprovalControlToLoad = "../controls/ItemApproval.ascx";

		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			LoadControls();
			base.OnInit(e);
			
			// the following loads our common properties control to edit.
			// must be loaded in the OnInit, once you get to page load the properties 
			// for this control are gone from viewstate if not loaded in OnInit
		}
		
        private void InitializeComponent()
		{
			this.cmdUpdate.Click += this.cmdUpdate_Click;
			this.cmdCancel.Click += this.cmdCancel_Click;
			Load += Page_Load;
		}

		private void LoadControls()
		{
            thisComment = UserFeedback.Comment.GetComment(CommentId, DataProvider.ModuleQualifier);	
            this.cmdDelete.Visible = IsAdmin;

			//load text editor entries
		    var l1 = (LabelControl)LoadControl("~/controls/LabelControl.ascx");
            l1.ResourceKey = "CommentText";
			this.phCommentText.Controls.Add(l1);
			this.TeArticleText = (TextEditor)LoadControl("~/controls/TextEditor.ascx");
			this.TeArticleText.HtmlEncode = false;
			this.TeArticleText.TextRenderMode = "Raw";
			this.TeArticleText.Width = 700;
			this.TeArticleText.Height = 400;
			this.TeArticleText.ChooseMode = false;
			this.phCommentText.Controls.Add(this.TeArticleText);

			//load approval status
			this.Ias = (ItemApproval) LoadControl(ApprovalControlToLoad);
			this.Ias.ModuleConfiguration = ModuleConfiguration;
			this.Ias.ID = System.IO.Path.GetFileNameWithoutExtension(ApprovalControlToLoad);
			this.phApproval.Controls.Add(this.Ias);
		}

		#region Event Handlers

		private void Page_Load(object sender, EventArgs e)
		{	
			try 
			{
                DotNetNuke.UI.Utilities.ClientAPI.AddButtonConfirm(cmdDelete, Localization.GetString("DeleteConfirm", LocalResourceFile));
				
				if (!Page.IsPostBack)
				{
					this.txtCommentId.Text = thisComment.CommentId.ToString(CultureInfo.CurrentCulture);
                    this.txtUserId.Text = (thisComment.UserId.HasValue ? thisComment.UserId.Value.ToString(CultureInfo.CurrentCulture) : Localization.GetString("Unauthenticated", LocalResourceFile));
                    this.txtFirstName.Text = thisComment.FirstName;
                    this.txtLastName.Text = thisComment.LastName;
                    this.txtEmailAddress.Text = thisComment.EmailAddress;
					this.TeArticleText.Text = thisComment.CommentText;
                    this.Ias.ApprovalStatusId = thisComment.ApprovalStatusId;

                    this.txtUrl.Text = thisComment.Url;

                    this.txtFirstName.MaxLength = UserFeedback.Comment.NameSizeLimit;
                    this.txtLastName.MaxLength = UserFeedback.Comment.NameSizeLimit;
                    this.txtEmailAddress.MaxLength = UserFeedback.Comment.EmailAddressSizeLimit;
				}
			}
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		private void cmdCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect(Globals.NavigateURL());
		}

		private void cmdUpdate_Click(object sender, EventArgs e)
		{
			try
			{			
				this.txtMessage.Text = string.Empty;

				if (!this.Ias.IsValid)
				{
                    this.txtMessage.Text += Localization.GetString("ErrorApprovalStatus.Text", LocalResourceFile); 
					this.txtMessage.Visible = true;
					return;
				}

                thisComment.CommentText = this.TeArticleText.Text;
                thisComment.FirstName = txtFirstName.Text;
                thisComment.LastName = txtLastName.Text;
                thisComment.EmailAddress = txtEmailAddress.Text;
                thisComment.ApprovalStatusId = this.Ias.ApprovalStatusId;
                thisComment.Url = txtUrl.Text.Trim();

                thisComment.Save(DataProvider.ModuleQualifier);
                Utility.ClearPublishCache(PortalId);
                this.txtMessage.Text = Localization.GetString("CommentEditted", LocalResourceFile);
                ShowOnlyMessage();
                //Response.Redirect(Globals.NavigateURL(TabId, "", "", "ctl=" + Utility.AdminContainer + "&mid=" + ModuleId.ToString() + "&adminType=itemCreated&itemId=" + VersionInfoObject.ItemId), true);
            }
			catch (Exception exc)
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            thisComment.Delete(DataProvider.ModuleQualifier);
            this.txtMessage.Text = Localization.GetString("DeleteSuccess", LocalResourceFile);
            this.txtMessage.Visible = true;

            ShowOnlyMessage();
        }

        private void ShowOnlyMessage()
        {
            foreach (Control cntl in this.Controls)
            {
                cntl.Visible = false;
            }

            this.txtMessage.Visible = true;
            this.txtMessage.Parent.Visible = true;
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
			                               DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent,
			                               this.LocalResourceFile),
			                               DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", "", false
			                               , DotNetNuke.Security.SecurityAccessLevel.Edit, true, false
			                               }
			               };
			}
		}

		#endregion
	}
}

