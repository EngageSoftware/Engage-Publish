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

    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Util;

    public partial class QuickStats : ModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // check VI for null then set information
                if (!this.Page.IsPostBack)
                {
                    // check if the admin settings for AMS have been set, if not load the settings page.
                    if (this.IsAdmin)
                    {
                        // Admin specific stats
                        if (this.UseApprovals)
                        {
                            this.lnkWaitingForApproval.Text = String.Format(
                                Localization.GetString("lnkWaitingForApproval", this.LocalResourceFile), 
                                DataProvider.Instance().WaitingForApprovalCount(this.PortalId));
                            this.lnkWaitingForApproval.Visible = true;
                            this.lnkWaitingForApproval.NavigateUrl = this.EditUrl(Utility.AdminContainer);
                        }
                        else
                        {
                            this.lnkWaitingForApproval.Visible = false;
                        }

                        // Comments always require approval
                        if (this.IsCommentsEnabled && IsPublishCommentTypeForPortal(this.PortalId))
                        {
                            var authorUserId = -1;
                            if (!this.IsAdmin)
                            {
                                authorUserId = this.UserId;
                            }

                            this.lnkCommentsForApproval.Text = String.Format(
                                Localization.GetString("lnkCommentsForApproval", this.LocalResourceFile), 
                                Comment.CommentsWaitingForApprovalCount(this.PortalId, authorUserId));
                            this.lnkCommentsForApproval.Visible = true;
                            this.lnkCommentsForApproval.NavigateUrl = this.EditUrl(
                                string.Empty, string.Empty, Utility.AdminContainer, "&adminType=commentList");
                        }
                        else
                        {
                            this.lnkCommentsForApproval.Visible = false;
                        }
                    }
                    else
                    {
                        // Generate author stats
                        this.lnkWaitingForApproval.Visible = false;
                        this.lnkCommentsForApproval.Visible = false;
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