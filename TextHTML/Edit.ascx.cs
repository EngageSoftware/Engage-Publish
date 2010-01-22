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
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Util;

    public partial class Edit : ModuleBase
    {


        override protected void OnInit(EventArgs e)
        {
            Load += Page_Load;
            base.OnInit(e);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //load the article id (itemid) from the module settings.
                LocalizeText();
                if (!Page.IsPostBack)
                {
                    LoadArticle();

                    epApprovals.Visible = UseApprovals;
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void LocalizeText()
        {
            btnSubmit.Text = Localization.GetString("btnSubmit", LocalSharedResourceFile);
            lblApproval.Text = UseApprovals ? Localization.GetString("ApprovalStatus", LocalSharedResourceFile) : Localization.GetString("ApprovalsDisabled", LocalSharedResourceFile);
        }

        protected void LoadArticle()
        {
            if(Settings.Contains("ItemId"))
            {
                SetItemId(Convert.ToInt32(Settings["ItemId"].ToString()));
                Article a = Article.GetArticle(Convert.ToInt32(Settings["ItemId"]), PortalId, true, true, true);
                if (a != null)
                {
                    teArticleText.Text = a.ArticleText;
                    publishTextHTMLEntry.Visible = true;
                    divPublishNotifications.Visible = false;
                }
                else
                {

                    //we need to fill the versioninfoobject because that's what the buildversionsurl needs to use

                    VersionInfoObject = new Article {ItemId = ItemId};

                    //there aren't any approved versions of this article, provide a link to the versions page.
                    publishTextHTMLEntry.Visible = false;
                    divPublishNotifications.Visible = true;
                    lblPublishMessages.Text = String.Format(Localization.GetString("notApproved", LocalResourceFile), BuildVersionsUrl());
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            var articleName = new StringBuilder(255);
            
            //replace name with the Page Name and Module Name
            var mc = new ModuleController();
            ModuleInfo mi = mc.GetModule(ModuleId, TabId);
            articleName.Append(mi.ModuleTitle);
            
            if (articleName.Length < 1)
            {//check to see if the moduletitle was set as the title for the article, otherwise use the following
                articleName.Append("TabId-");
                articleName.Append(TabId.ToString());
                articleName.Append("ModuleId-");
                articleName.Append(ModuleId.ToString());
            }
            //string articleDescription = String.Format(Localization.GetString("description", LocalResourceFile), DateTime.Now.ToString(CultureInfo.CurrentCulture));

            string articleText = teArticleText.Text;
            string description = DotNetNuke.Common.Utilities.HtmlUtils.StripTags(articleText, false);
            string articleDescription = Utility.TrimDescription(3997, description) + "...";// description + "...";
          
            
            //save article
            //if the article id (itemid) already exists in the module settings let's update, otherwise create new
            if (Settings.Contains("ItemId"))
            {
                Article a = Article.GetArticle(Convert.ToInt32(Settings["ItemId"]), PortalId, true, true,true);
                
                a.ArticleText = teArticleText.Text;
                a.Description = articleDescription;
                //trim the content entered for a description
                
                a.DisplayTabId = TabId;
                                
                //force display on specific page
                Setting setting = Setting.ArticleSettingForceDisplay;
                var itemVersionSetting = new ItemVersionSetting(setting);
                a.VersionSettings[itemVersionSetting].PropertyValue = "true";

                a.ModuleId = ModuleId;

                a.ApprovalStatusId = UseApprovals ? epApprovals.ApprovalStatusId : ApprovalStatus.Approved.GetId();

                a.Save(UserId);
                
                //this is likely unneccesary as we already have the itemid set in the settings
                mc.UpdateTabModuleSetting(TabModuleId, "ItemId", a.ItemId.ToString());

                mc.UpdateTabModuleSetting(TabModuleId, "DisplayType", "texthtml");
            }
            else
            {
                Article a = Article.Create(articleName.ToString(), articleDescription, teArticleText.Text, UserId, DefaultTextHtmlCategory, ModuleId, PortalId);
                a.DisplayTabId = TabId;

                //force display on specific page
                Setting setting = Setting.ArticleSettingForceDisplay;
                
                var itemVersionSetting = new ItemVersionSetting(setting);
                a.VersionSettings[itemVersionSetting].PropertyValue = "true";

                a.ModuleId = ModuleId; 
                a.ApprovalStatusId = UseApprovals ? epApprovals.ApprovalStatusId : ApprovalStatus.Approved.GetId();

                a.Save(UserId);
                mc.UpdateTabModuleSetting(TabModuleId, "ItemId", a.ItemId.ToString());
                mc.UpdateTabModuleSetting(TabModuleId, "DisplayType", "texthtml");
            }

            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
        }
    }
}

