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
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Data;
    using Util;

    public partial class View : ModuleBase, IActionable
    {


        override protected void OnInit(EventArgs e)
        {
            SetItemId();
            Load += Page_Load;
            base.OnInit(e);
            BindItemData();
        }

        public int LocalItemId
        {
            get
            {
                if (Settings.Contains("ItemId"))
                {
                    return Convert.ToInt32(Settings["ItemId"]);
                }
                return -1;
            }
        }


        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (DefaultTextHtmlCategory > 0)
                {
                    LoadArticle();
                }
                else
                {
                    //display message about module not being configured properly.
                    string notConfigured = Localization.GetString("NotConfigured", LocalResourceFile);
                    string adminSettingsLink = BuildLinkUrl("&amp;mid=" + ModuleId + "&amp;ctl=admincontainer&amp;adminType=amsSettings&amp;returnUrl=" + HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl));
                    lblArticleText.Text = String.Format(notConfigured, adminSettingsLink);
                    
                    //disable the edit link
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void SetItemId()
        {
            if (LocalItemId > 0)
            {
                SetItemId(LocalItemId);
            }
        }

        private void LoadArticle()
        {
            var a = (Article)VersionInfoObject;
            if (a != null)
            {
                //VersionInfoObject.IsNew = false;
                if (a.ArticleText.Trim() == string.Empty)
                {
                    lblArticleText.Text = Localization.GetString("NothingSaved", LocalResourceFile);
                }
                else
                {
                    string articleText = a.ArticleText;
                    //removed until we move forward with a newer version of DNN 4.6.2 or greater.
                    //for enterprise licenses you can uncomment the following if you put the Dotnetnuke.dll (4.6.2+) in the engagepublish/references folder and recompile

                    articleText = Utility.ReplaceTokens(articleText);

                    var tr = new DotNetNuke.Services.Tokens.TokenReplace
                                 {
                                         AccessingUser = UserInfo,
                                         DebugMessages = !DotNetNuke.Common.Globals.IsTabPreview()
                                 };

                    articleText = tr.ReplaceEnvironmentTokens(articleText);
                    lblArticleText.Text = articleText;
                }

                
            object m = Request.QueryString["modid"];
            if (m != null)
            {
                if (m.ToString() == ModuleId.ToString())
                {
                    //check if module id querystring is current moduleid
                    if ((IsAdmin && !VersionInfoObject.IsNew))
                    {
                        divAdminMenuWrapper.Visible = true;
                        divPublishApprovals.Visible = true;
                        divApprovalStatus.Visible = true;
                        if (UseApprovals && Item.GetItemType(ItemId, PortalId).Equals("ARTICLE", StringComparison.OrdinalIgnoreCase))
                        {
                            FillDropDownList();
                        }
                        else
                        {
                            ddlApprovalStatus.Visible = false;
                        }
                    }
                }
                else
                {
                    divAdminMenuWrapper.Visible = false;
                }
            }
            }
            else
            {
                lblArticleText.Text = Localization.GetString("NothingSaved", LocalResourceFile);
            }
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection();
                if (DefaultTextHtmlCategory > 0)
                {
                    actions.Add(GetNextActionID(), Localization.GetString("Edit", LocalSharedResourceFile), "", "", "", EditUrl(), false, SecurityAccessLevel.Edit, true, false);
                    //                actions.Add(GetNextActionID(), Localization.GetString("Administration", LocalSharedResourceFile), "", "", "", EditUrl(Utility.AdminContainer), false, SecurityAccessLevel.Edit, true, false);
                    if (LocalItemId > 0)
                    {
                        actions.Add(GetNextActionID(), Localization.GetString("Versions", LocalSharedResourceFile), "", "", "", BuildVersionsUrl(), false, SecurityAccessLevel.Edit, true, false);
                    }
                }
                return actions;
            }
        }

        protected void lnkSaveApprovalStatus_Click(object sender, EventArgs e)
        {
            CallUpdateApprovalStatus();
        }

        protected void CallUpdateApprovalStatus()
        {
            if (!VersionInfoObject.IsNew)
            {
                VersionInfoObject.ApprovalStatusId = Convert.ToInt32(ddlApprovalStatus.SelectedValue, CultureInfo.InvariantCulture);
                VersionInfoObject.ApprovalComments = txtApprovalComments.Text.Trim().Length > 0 ? txtApprovalComments.Text.Trim() : Localization.GetString("DefaultApprovalComment", LocalResourceFile);
                VersionInfoObject.UpdateApprovalStatus();
                Response.Redirect(BuildVersionsUrl(), false);
            }
        }

        protected void LnkUpdateStatusClick(object sender, EventArgs e)
        {
            divApprovalStatus.Visible = true;
            txtApprovalComments.Text = VersionInfoObject.ApprovalComments;
        }

        protected void lnkSaveApprovalStatusCancel_Click(object sender, EventArgs e)
        {
            divApprovalStatus.Visible = false;
        }

        private void FillDropDownList()
        {
            if (!Page.IsPostBack)
            {
                ddlApprovalStatus.DataSource = DataProvider.Instance().GetApprovalStatusTypes(PortalId);
                ddlApprovalStatus.DataValueField = "ApprovalStatusID";
                ddlApprovalStatus.DataTextField = "ApprovalStatusName";
                ddlApprovalStatus.DataBind();
                //set the current approval status
                ListItem li = ddlApprovalStatus.Items.FindByValue(VersionInfoObject.ApprovalStatusId.ToString(CultureInfo.InvariantCulture));
                if (li != null)
                {
                    li.Selected = true;
                }
            }
        }
    }
}

