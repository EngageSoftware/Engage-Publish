//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.UI.UserControls;
using Engage.Dnn.Publish;
using Engage.Dnn.Publish.Util;
using Engage.Dnn.Publish.Controls;


namespace Engage.Dnn.Publish.Admin
{
	public partial class CommentEdit :  ModuleBase, IActionable
	{
        private Comment thisComment;
        protected ItemApproval ias;
    	protected TextEditor teArticleText;
		private const string approvalControlToLoad = "../controls/ItemApproval.ascx";

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
			this.cmdUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
			this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
			this.Load += new System.EventHandler(this.Page_Load);
		}

		private void LoadControls()
		{
            thisComment = Comment.GetComment(CommentId);	
            this.cmdDelete.Visible = IsAdmin;

			//load text editor entries
            DotNetNuke.UI.UserControls.LabelControl l1;
            l1 = (DotNetNuke.UI.UserControls.LabelControl)LoadControl("~/controls/LabelControl.ascx");
            l1.ResourceKey = "CommentText";
			this.phCommentText.Controls.Add(l1);
			teArticleText = (TextEditor)LoadControl("~/controls/TextEditor.ascx");
			teArticleText.HtmlEncode = false;
			teArticleText.TextRenderMode = "Raw";
			teArticleText.Width = 700;
			teArticleText.Height = 400;
			teArticleText.ChooseMode = false;
			this.phCommentText.Controls.Add(teArticleText);

			//load approval status
			ias = (ItemApproval) LoadControl(approvalControlToLoad);
			ias.ModuleConfiguration = ModuleConfiguration;
			ias.ID = System.IO.Path.GetFileNameWithoutExtension(approvalControlToLoad);
			this.phApproval.Controls.Add(ias);
		}

		#region Event Handlers

		private void Page_Load(object sender, System.EventArgs e)
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
					this.teArticleText.Text = thisComment.CommentText;
                    this.ias.ApprovalStatusId = thisComment.ApprovalStatusId;

                    this.txtFirstName.MaxLength = Comment.NameSizeLimit;
                    this.txtLastName.MaxLength = Comment.NameSizeLimit;
                    this.txtEmailAddress.MaxLength = Comment.EmailAddressSizeLimit;
				}
			}
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			Response.Redirect(Globals.NavigateURL());
		}

		private void cmdUpdate_Click(object sender, System.EventArgs e)
		{
			try
			{			
				this.txtMessage.Text = string.Empty;

				if (!this.ias.IsValid)
				{
                    this.txtMessage.Text += Localization.GetString("ErrorApprovalStatus.Text", LocalResourceFile); 
					this.txtMessage.Visible = true;
					return;
				}

                thisComment.CommentText = teArticleText.Text;
                thisComment.FirstName = txtFirstName.Text;
                thisComment.LastName = txtLastName.Text;
                thisComment.EmailAddress = txtEmailAddress.Text;
                thisComment.ApprovalStatusId = ias.ApprovalStatusId;

                thisComment.Save();
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
            thisComment.Delete();
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

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions 
		{
			get 
			{
				DotNetNuke.Entities.Modules.Actions.ModuleActionCollection Actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
				Actions.Add(GetNextActionID(), Localization.GetString(DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", "", false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
				return Actions;
			}
		}

		

		#endregion
	}
}

