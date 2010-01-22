//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.



namespace Engage.Dnn.Publish.Controls
{
    using System;
    using System.Globalization;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Data;

	public partial class ItemApproval :  ModuleBase, IActionable
	{
		#region Event Handlers
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{
			Load += Page_Load;
		}

		private void Page_Load(object sender, EventArgs e)
		{
			try 
			{
				//check VI for null then set information
				if (!IsPostBack)
				{
                    if (IsAdmin)
                    {
                        LoadApprovalTypes();
                        divApprovalStatus.Visible = true;
                        divSubmitForApproval.Visible = false;
                    }
                    else
                    {
                        divApprovalStatus.Visible = false;
                        divSubmitForApproval.Visible = true;
                    }
                    LocalizeText();
				}
                //else
                //{
                //    if (VersionInfoObject != null)
                //    {
                //        VersionInfoObject.ApprovalStatusId = GetApprovalId();
                //    }
                //}
			} 
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}
		#endregion

        private void LocalizeText()
        {
            chkSubmitForApproval.Text = Localization.GetString("chkSubmitForApproval", LocalSharedResourceFile);
        }

		private int GetApprovalId()
		{
            int approvalId;
            if (IsAdmin)
            {
                approvalId = Convert.ToInt32(radApprovalStatus.SelectedValue, CultureInfo.InvariantCulture);
            }
            else
            {
                approvalId = chkSubmitForApproval.Checked ? Util.ApprovalStatus.Waiting.GetId() : Util.ApprovalStatus.Edit.GetId();
            }
            return approvalId;
		}

		private void LoadApprovalTypes()
		{
			if (IsAdmin)
			{
				radApprovalStatus.DataSource= DataProvider.Instance().GetApprovalStatusTypes(PortalId);
			}
							
			radApprovalStatus.DataValueField = "ApprovalStatusID";
			radApprovalStatus.DataTextField = "ApprovalStatusName";
			radApprovalStatus.DataBind();
			radApprovalStatus.Items[0].Selected=true;
		}

        public int ApprovalStatusId
        {
            get
            {
                return GetApprovalId();
            }
            set
            {
                radApprovalStatus.SelectedValue = value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public bool IsValid
        {
            get
            {
                return chkSubmitForApproval.Visible || radApprovalStatus.SelectedIndex > -1;
            }
        }

		#region Optional Interfaces

		public ModuleActionCollection ModuleActions 
		{
			get 
			{
			    return new ModuleActionCollection
			               {
			                       {
			                               GetNextActionID(),
			                               Localization.GetString(ModuleActionType.AddContent, LocalResourceFile),
			                               DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", "", false
			                               , DotNetNuke.Security.SecurityAccessLevel.Edit, true, false
			                               }
			               };
			}
		}


		#endregion
	}
}

