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
    using Data;
    using Util;


    public partial class QuickStats : ModuleBase
	{

        #region Event Handlers
		override protected void OnInit(EventArgs e)
		{
		    Load += Page_Load;
		    base.OnInit(e);

		}

		private void Page_Load(object sender, EventArgs e)
		{
			try 
			{
    			//check VI for null then set information
				if (!Page.IsPostBack)
				{
                    //check if the admin settings for AMS have been set, if not load the settings page.
					if(IsAdmin)
					{
                        //Admin specific stats
                        if (UseApprovals)
                        {
                            lnkWaitingForApproval.Text = String.Format(Localization.GetString("lnkWaitingForApproval", LocalResourceFile), DataProvider.Instance().WaitingForApprovalCount(this.PortalId));
                            lnkWaitingForApproval.Visible = true;
                            lnkWaitingForApproval.NavigateUrl = EditUrl(Utility.AdminContainer);
                        }
                        else
                        {
                            lnkWaitingForApproval.Visible = false;
                        }
                        //Comments always require approval
                        if (IsCommentsEnabled && IsPublishCommentTypeForPortal(PortalId))
                        {
                            lnkCommentsForApproval.Text = String.Format(Localization.GetString("lnkCommentsForApproval", LocalResourceFile), Comment.CommentsWaitingForApprovalCount(this.PortalId, this.UserId));
                            lnkCommentsForApproval.Visible = true;
                            lnkCommentsForApproval.NavigateUrl = EditUrl("","",Utility.AdminContainer, "&adminType=commentList");
                        }
                        else
                        {
                            lnkCommentsForApproval.Visible = false;
                        }
                    }
					else
					{
                        //Generate author stats
                        lnkWaitingForApproval.Visible = false;
                        lnkCommentsForApproval.Visible = false;
    				}
				}
			} 
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		#endregion

		
	}
}

