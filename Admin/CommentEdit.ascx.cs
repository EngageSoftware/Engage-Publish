// <copyright file="CommentEdit.ascx.cs" company="Engage Software">
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
    using System.Web.UI;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.UI.UserControls;
    using DotNetNuke.UI.Utilities;

    using Engage.Dnn.Publish.Controls;
    using Engage.Dnn.Publish.Util;
    using Engage.Dnn.UserFeedback;

    using DataProvider = Engage.Dnn.Publish.Data.DataProvider;
    using Globals = DotNetNuke.Common.Globals;

    public partial class CommentEdit : ModuleBase, IActionable
    {
        protected ItemApproval Ias;

        protected TextEditor TeArticleText;

        private const string ApprovalControlToLoad = "../controls/ItemApproval.ascx";

        private Comment thisComment;

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

        protected override void OnInit(EventArgs e)
        {
            this.cmdUpdate.Click += this.cmdUpdate_Click;
            this.cmdCancel.Click += this.cmdCancel_Click;
            this.Load += this.Page_Load;
            this.LoadControls();
            base.OnInit(e);

            // the following loads our common properties control to edit.
            // must be loaded in the OnInit, once you get to page load the properties 
            // for this control are gone from viewstate if not loaded in OnInit
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", 
            Justification = "Controls use lower case prefix")]
        protected void cmdDelete_Click(object sender, EventArgs e)
        {
            this.thisComment.Delete(DataProvider.ModuleQualifier);
            this.txtMessage.Text = Localization.GetString("DeleteSuccess", this.LocalResourceFile);
            this.txtMessage.Visible = true;

            this.ShowOnlyMessage();
        }

        private void LoadControls()
        {
            this.thisComment = Comment.GetComment(this.CommentId, DataProvider.ModuleQualifier);
            this.cmdDelete.Visible = this.IsAdmin;

            // load text editor entries
            var l1 = (LabelControl)this.LoadControl("~/controls/LabelControl.ascx");
            l1.ResourceKey = "CommentText";
            this.phCommentText.Controls.Add(l1);
            this.TeArticleText = (TextEditor)this.LoadControl("~/controls/TextEditor.ascx");
            this.TeArticleText.HtmlEncode = false;
            this.TeArticleText.TextRenderMode = "Raw";
            this.TeArticleText.Width = 700;
            this.TeArticleText.Height = 400;
            this.TeArticleText.ChooseMode = false;
            this.phCommentText.Controls.Add(this.TeArticleText);

            // load approval status
            this.Ias = (ItemApproval)this.LoadControl(ApprovalControlToLoad);
            this.Ias.ModuleConfiguration = this.ModuleConfiguration;
            this.Ias.ID = Path.GetFileNameWithoutExtension(ApprovalControlToLoad);
            this.phApproval.Controls.Add(this.Ias);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ClientAPI.AddButtonConfirm(this.cmdDelete, Localization.GetString("DeleteConfirm", this.LocalResourceFile));

                if (!this.Page.IsPostBack)
                {
                    this.txtCommentId.Text = this.thisComment.CommentId.ToString(CultureInfo.CurrentCulture);
                    this.txtUserId.Text = this.thisComment.UserId.HasValue
                                               ? this.thisComment.UserId.Value.ToString(CultureInfo.CurrentCulture)
                                               : Localization.GetString("Unauthenticated", this.LocalResourceFile);
                    this.txtFirstName.Text = this.thisComment.FirstName;
                    this.txtLastName.Text = this.thisComment.LastName;
                    this.txtEmailAddress.Text = this.thisComment.EmailAddress;
                    this.TeArticleText.Text = this.thisComment.CommentText;
                    this.Ias.ApprovalStatusId = this.thisComment.ApprovalStatusId;

                    this.txtUrl.Text = this.thisComment.Url;

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

        private void ShowOnlyMessage()
        {
            foreach (Control cntl in this.Controls)
            {
                cntl.Visible = false;
            }

            this.txtMessage.Visible = true;
            this.txtMessage.Parent.Visible = true;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(Globals.NavigateURL());
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtMessage.Text = string.Empty;

                if (!this.Ias.IsValid)
                {
                    this.txtMessage.Text += Localization.GetString("ErrorApprovalStatus.Text", this.LocalResourceFile);
                    this.txtMessage.Visible = true;
                    return;
                }

                this.thisComment.CommentText = this.TeArticleText.Text;
                this.thisComment.FirstName = this.txtFirstName.Text;
                this.thisComment.LastName = this.txtLastName.Text;
                this.thisComment.EmailAddress = this.txtEmailAddress.Text;
                this.thisComment.ApprovalStatusId = this.Ias.ApprovalStatusId;
                this.thisComment.Url = this.txtUrl.Text.Trim();

                this.thisComment.Save(DataProvider.ModuleQualifier);
                Utility.ClearPublishCache(this.PortalId);
                this.txtMessage.Text = Localization.GetString("CommentEditted", this.LocalResourceFile);
                this.ShowOnlyMessage();

                // Response.Redirect(Globals.NavigateURL(TabId, "", "", "ctl=" + Utility.AdminContainer + "&mid=" + ModuleId.ToString() + "&adminType=itemCreated&itemId=" + VersionInfoObject.ItemId), true);
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}