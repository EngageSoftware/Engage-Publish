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
    using System.Text;

    using DotNetNuke.Common;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    public partial class Edit : ModuleBase
    {
        protected void LoadArticle()
        {
            if (this.Settings.Contains("ItemId"))
            {
                this.SetItemId(Convert.ToInt32(this.Settings["ItemId"].ToString()));
                Article a = Article.GetArticle(Convert.ToInt32(this.Settings["ItemId"]), this.PortalId, true, true, true);
                if (a != null)
                {
                    this.teArticleText.Text = a.ArticleText;
                    this.publishTextHTMLEntry.Visible = true;
                    this.divPublishNotifications.Visible = false;
                }
                else
                {
                    // we need to fill the versioninfoobject because that's what the buildversionsurl needs to use
                    this.VersionInfoObject = new Article
                        {
                            ItemId = this.ItemId
                        };

                    // there aren't any approved versions of this article, provide a link to the versions page.
                    this.publishTextHTMLEntry.Visible = false;
                    this.divPublishNotifications.Visible = true;
                    this.lblPublishMessages.Text = String.Format(
                        Localization.GetString("notApproved", this.LocalResourceFile), this.BuildVersionsUrl());
                }
            }
        }

        protected void LocalizeText()
        {
            this.btnSubmit.Text = Localization.GetString("btnSubmit", this.LocalSharedResourceFile);
            this.lblApproval.Text = this.UseApprovals
                                        ? Localization.GetString("ApprovalStatus", this.LocalSharedResourceFile)
                                        : Localization.GetString("ApprovalsDisabled", this.LocalSharedResourceFile);
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var articleName = new StringBuilder(255);

            // replace name with the Page Name and Module Name
            var mc = new ModuleController();
            ModuleInfo mi = mc.GetModule(this.ModuleId, this.TabId);
            articleName.Append(mi.ModuleTitle);

            if (articleName.Length < 1)
            {
                // check to see if the moduletitle was set as the title for the article, otherwise use the following
                articleName.Append("TabId-");
                articleName.Append(this.TabId.ToString());
                articleName.Append("ModuleId-");
                articleName.Append(this.ModuleId.ToString());
            }

            // string articleDescription = String.Format(Localization.GetString("description", LocalResourceFile), DateTime.Now.ToString(CultureInfo.CurrentCulture));
            string articleText = this.teArticleText.Text;
            string description = HtmlUtils.StripTags(articleText, false);
            string articleDescription = Utility.TrimDescription(3997, description) + "..."; // description + "...";

            // save article
            // if the article id (itemid) already exists in the module settings let's update, otherwise create new
            if (this.Settings.Contains("ItemId"))
            {
                Article a = Article.GetArticle(Convert.ToInt32(this.Settings["ItemId"]), this.PortalId, true, true, true);

                a.ArticleText = this.teArticleText.Text;
                a.Description = articleDescription;

                // trim the content entered for a description
                a.DisplayTabId = this.TabId;

                // force display on specific page
                Setting setting = Setting.ArticleSettingForceDisplay;
                var itemVersionSetting = new ItemVersionSetting(setting);
                a.VersionSettings[itemVersionSetting].PropertyValue = "true";

                a.ModuleId = this.ModuleId;

                a.ApprovalStatusId = this.UseApprovals ? this.epApprovals.ApprovalStatusId : ApprovalStatus.Approved.GetId();

                a.Save(this.UserId);

                // this is likely unneccesary as we already have the itemid set in the settings
                mc.UpdateTabModuleSetting(this.TabModuleId, "ItemId", a.ItemId.ToString());

                mc.UpdateTabModuleSetting(this.TabModuleId, "DisplayType", "texthtml");
            }
            else
            {
                Article a = Article.Create(
                    articleName.ToString(), 
                    articleDescription, 
                    this.teArticleText.Text, 
                    this.UserId, 
                    this.DefaultTextHtmlCategory, 
                    this.ModuleId, 
                    this.PortalId);
                a.DisplayTabId = this.TabId;

                // force display on specific page
                Setting setting = Setting.ArticleSettingForceDisplay;

                var itemVersionSetting = new ItemVersionSetting(setting);
                a.VersionSettings[itemVersionSetting].PropertyValue = "true";

                a.ModuleId = this.ModuleId;
                a.ApprovalStatusId = this.UseApprovals ? this.epApprovals.ApprovalStatusId : ApprovalStatus.Approved.GetId();

                a.Save(this.UserId);
                mc.UpdateTabModuleSetting(this.TabModuleId, "ItemId", a.ItemId.ToString());
                mc.UpdateTabModuleSetting(this.TabModuleId, "DisplayType", "texthtml");
            }

            this.Response.Redirect(Globals.NavigateURL());
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // load the article id (itemid) from the module settings.
                this.LocalizeText();
                if (!this.Page.IsPostBack)
                {
                    this.LoadArticle();

                    this.epApprovals.Visible = this.UseApprovals;
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}