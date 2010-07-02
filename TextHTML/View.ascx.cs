//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.TextHtml
{
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.UI.WebControls;

    using DotNetNuke.Common;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Util;

    using TokenReplace = DotNetNuke.Services.Tokens.TokenReplace;

    public partial class View : ModuleBase, IActionable
    {
        public int LocalItemId
        {
            get
            {
                if (this.Settings.Contains("ItemId"))
                {
                    return Convert.ToInt32(this.Settings["ItemId"]);
                }

                return -1;
            }
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection();
                if (this.DefaultTextHtmlCategory > 0)
                {
                    actions.Add(
                        this.GetNextActionID(), 
                        Localization.GetString("Edit", this.LocalSharedResourceFile), 
                        string.Empty, 
                        string.Empty, 
                        string.Empty, 
                        this.EditUrl(), 
                        false, 
                        SecurityAccessLevel.Edit, 
                        true, 
                        false);

                    // actions.Add(GetNextActionID(), Localization.GetString("Administration", LocalSharedResourceFile), "", "", "", EditUrl(Utility.AdminContainer), false, SecurityAccessLevel.Edit, true, false);
                    if (this.LocalItemId > 0)
                    {
                        actions.Add(
                            this.GetNextActionID(), 
                            Localization.GetString("Versions", this.LocalSharedResourceFile), 
                            string.Empty, 
                            string.Empty, 
                            string.Empty, 
                            this.BuildVersionsUrl(), 
                            false, 
                            SecurityAccessLevel.Edit, 
                            true, 
                            false);
                    }
                }

                return actions;
            }
        }

        protected void CallUpdateApprovalStatus()
        {
            if (!this.VersionInfoObject.IsNew)
            {
                this.VersionInfoObject.ApprovalStatusId = Convert.ToInt32(this.ddlApprovalStatus.SelectedValue, CultureInfo.InvariantCulture);
                this.VersionInfoObject.ApprovalComments = this.txtApprovalComments.Text.Trim().Length > 0
                                                              ? this.txtApprovalComments.Text.Trim()
                                                              : Localization.GetString("DefaultApprovalComment", this.LocalResourceFile);
                this.VersionInfoObject.UpdateApprovalStatus();
                this.Response.Redirect(this.BuildVersionsUrl(), false);
            }
        }

        protected void LnkUpdateStatusClick(object sender, EventArgs e)
        {
            this.divApprovalStatus.Visible = true;
            this.txtApprovalComments.Text = this.VersionInfoObject.ApprovalComments;
        }

        protected override void OnInit(EventArgs e)
        {
            this.SetItemId();
            this.Load += this.Page_Load;
            base.OnInit(e);
            this.BindItemData();
        }

        protected void lnkSaveApprovalStatusCancel_Click(object sender, EventArgs e)
        {
            this.divApprovalStatus.Visible = false;
        }

        protected void lnkSaveApprovalStatus_Click(object sender, EventArgs e)
        {
            this.CallUpdateApprovalStatus();
        }

        private void FillDropDownList()
        {
            if (!this.Page.IsPostBack)
            {
                this.ddlApprovalStatus.DataSource = DataProvider.Instance().GetApprovalStatusTypes(this.PortalId);
                this.ddlApprovalStatus.DataValueField = "ApprovalStatusID";
                this.ddlApprovalStatus.DataTextField = "ApprovalStatusName";
                this.ddlApprovalStatus.DataBind();

                // set the current approval status
                ListItem li = this.ddlApprovalStatus.Items.FindByValue(this.VersionInfoObject.ApprovalStatusId.ToString(CultureInfo.InvariantCulture));
                if (li != null)
                {
                    li.Selected = true;
                }
            }
        }

        private void LoadArticle()
        {
            var a = (Article)this.VersionInfoObject;
            if (a != null)
            {
                // VersionInfoObject.IsNew = false;
                if (a.ArticleText.Trim() == string.Empty)
                {
                    this.lblArticleText.Text = Localization.GetString("NothingSaved", this.LocalResourceFile);
                }
                else
                {
                    string articleText = a.ArticleText;

                    // removed until we move forward with a newer version of DNN 4.6.2 or greater.
                    // for enterprise licenses you can uncomment the following if you put the Dotnetnuke.dll (4.6.2+) in the engagepublish/references folder and recompile
                    articleText = Utility.ReplaceTokens(articleText);

                    var tr = new TokenReplace
                        {
                            AccessingUser = this.UserInfo, 
                            DebugMessages = !Globals.IsTabPreview()
                        };

                    articleText = tr.ReplaceEnvironmentTokens(articleText);
                    this.lblArticleText.Text = articleText;
                }

                object m = this.Request.QueryString["modid"];
                if (m != null)
                {
                    if (m.ToString() == this.ModuleId.ToString())
                    {
                        // check if module id querystring is current moduleid
                        if (this.IsAdmin && !this.VersionInfoObject.IsNew)
                        {
                            this.divAdminMenuWrapper.Visible = true;
                            this.divPublishApprovals.Visible = true;
                            this.divApprovalStatus.Visible = true;
                            if (this.UseApprovals &&
                                Item.GetItemType(this.ItemId, this.PortalId).Equals("ARTICLE", StringComparison.OrdinalIgnoreCase))
                            {
                                this.FillDropDownList();
                            }
                            else
                            {
                                this.ddlApprovalStatus.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        this.divAdminMenuWrapper.Visible = false;
                    }
                }
            }
            else
            {
                this.lblArticleText.Text = Localization.GetString("NothingSaved", this.LocalResourceFile);
            }
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.DefaultTextHtmlCategory > 0)
                {
                    this.LoadArticle();
                }
                else
                {
                    // display message about module not being configured properly.
                    string notConfigured = Localization.GetString("NotConfigured", this.LocalResourceFile);
                    string adminSettingsLink =
                        this.BuildLinkUrl(
                            "&amp;mid=" + this.ModuleId + "&amp;ctl=admincontainer&amp;adminType=amsSettings&amp;returnUrl=" +
                            HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl));
                    this.lblArticleText.Text = String.Format(notConfigured, adminSettingsLink);

                    // disable the edit link
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void SetItemId()
        {
            if (this.LocalItemId > 0)
            {
                this.SetItemId(this.LocalItemId);
            }
        }
    }
}