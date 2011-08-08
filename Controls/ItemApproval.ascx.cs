//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
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
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Util;

    public partial class ItemApproval : ModuleBase, IActionable
    {
        public int ApprovalStatusId
        {
            get { return this.GetApprovalId(); }
            set { this.radApprovalStatus.SelectedValue = value.ToString(CultureInfo.InvariantCulture); }
        }

        public bool IsValid
        {
            get { return this.chkSubmitForApproval.Visible || this.radApprovalStatus.SelectedIndex > -1; }
        }

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
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        private int GetApprovalId()
        {
            int approvalId;
            if (this.IsAdmin)
            {
                approvalId = Convert.ToInt32(this.radApprovalStatus.SelectedValue, CultureInfo.InvariantCulture);
            }
            else
            {
                approvalId = this.chkSubmitForApproval.Checked ? ApprovalStatus.Waiting.GetId() : ApprovalStatus.Edit.GetId();
            }

            return approvalId;
        }

        private void LoadApprovalTypes()
        {
            if (this.IsAdmin)
            {
                this.radApprovalStatus.DataSource = DataProvider.Instance().GetApprovalStatusTypes(this.PortalId);
            }

            this.radApprovalStatus.DataValueField = "ApprovalStatusID";
            this.radApprovalStatus.DataTextField = "ApprovalStatusName";
            this.radApprovalStatus.DataBind();
            this.radApprovalStatus.Items[0].Selected = true;
        }

        private void LocalizeText()
        {
            this.chkSubmitForApproval.Text = Localization.GetString("chkSubmitForApproval", this.LocalSharedResourceFile);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // check VI for null then set information
                if (!this.IsPostBack)
                {
                    if (this.IsAdmin)
                    {
                        this.LoadApprovalTypes();
                        this.divApprovalStatus.Visible = true;
                        this.divSubmitForApproval.Visible = false;
                    }
                    else
                    {
                        this.divApprovalStatus.Visible = false;
                        this.divSubmitForApproval.Visible = true;
                    }

                    this.LocalizeText();
                }

                // else
                // {
                // if (VersionInfoObject != null)
                // {
                // VersionInfoObject.ApprovalStatusId = GetApprovalId();
                // }
                // }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}