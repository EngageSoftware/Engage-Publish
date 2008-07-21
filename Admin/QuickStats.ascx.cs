//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using Engage.Dnn.Publish.Data;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.Admin
{
    public partial class QuickStats : ModuleBase
	{

        #region Event Handlers
		override protected void OnInit(EventArgs e)
		{
		    this.Load += this.Page_Load;
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
                            lnkWaitingForApproval.Text = String.Format(Localization.GetString("lnkWaitingForApproval", LocalResourceFile), DataProvider.Instance().WaitingForApprovalCount(PortalId).ToString());
                            lnkWaitingForApproval.Visible = true;
                            lnkWaitingForApproval.NavigateUrl = EditUrl(Utility.AdminContainer);
                        }
                        else
                        {
                            lnkWaitingForApproval.Visible = false;
                        }
                        //Comments always require approval
                        if (IsCommentsEnabled)
                        {
                            lnkCommentsForApproval.Text = String.Format(Localization.GetString("lnkCommentsForApproval", LocalResourceFile), DataProvider.Instance().CommentsWaitingForApprovalCount(PortalId, UserId).ToString());
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

