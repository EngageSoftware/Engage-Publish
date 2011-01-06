//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Admin
{
    using System;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    public partial class AdminMain : ModuleBase, IActionable
    {
        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actions = new ModuleActionCollection();
                actions.Add(
                    this.GetNextActionID(), 
                    Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile), 
                    ModuleActionType.AddContent, 
                    string.Empty, 
                    string.Empty, 
                    this.EditUrl(), 
                    false, 
                    SecurityAccessLevel.Edit, 
                    true, 
                    false);
                return actions;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.admin_item_articles.Attributes.Add(
                    "onmouseover", "document.getElementById('" + this.admin_item_articles.ClientID + "').className='admin_item_articles_sel';");
                this.admin_item_articles.Attributes.Add(
                    "onmouseout", "document.getElementById('" + this.admin_item_articles.ClientID + "').className='admin_item_articles';");

                this.admin_item_categories.Attributes.Add(
                    "onmouseover", "document.getElementById('" + this.admin_item_categories.ClientID + "').className='admin_item_categories_sel';");
                this.admin_item_categories.Attributes.Add(
                    "onmouseout", "document.getElementById('" + this.admin_item_categories.ClientID + "').className='admin_item_categories';");

                this.admin_item_settings.Attributes.Add(
                    "onmouseover", "document.getElementById('" + this.admin_item_settings.ClientID + "').className='admin_item_settings_sel';");
                this.admin_item_settings.Attributes.Add(
                    "onmouseout", "document.getElementById('" + this.admin_item_settings.ClientID + "').className='admin_item_settings';");

                // admin_item_delete.Attributes.Add("onmouseover", "document.getElementById('" + admin_item_help.ClientID + "').className='admin_item_help_sel';");
                // admin_item_delete.Attributes.Add("onmouseout", "document.getElementById('" + admin_item_help.ClientID + "').className='admin_item_help';");
                this.admin_item_comments.Attributes.Add(
                    "onmouseover", "document.getElementById('" + this.admin_item_comments.ClientID + "').className='admin_item_comments_sel';");
                this.admin_item_comments.Attributes.Add(
                    "onmouseout", "document.getElementById('" + this.admin_item_comments.ClientID + "').className='admin_item_comments';");

                this.admin_item_delete.Attributes.Add(
                    "onmouseover", "document.getElementById('" + this.admin_item_delete.ClientID + "').className='admin_item_delete_sel';");
                this.admin_item_delete.Attributes.Add(
                    "onmouseout", "document.getElementById('" + this.admin_item_delete.ClientID + "').className='admin_item_delete';");

                this.admin_item_syndication.Attributes.Add(
                    "onmouseover", "document.getElementById('" + this.admin_item_syndication.ClientID + "').className='admin_item_syndication_sel';");
                this.admin_item_syndication.Attributes.Add(
                    "onmouseout", "document.getElementById('" + this.admin_item_syndication.ClientID + "').className='admin_item_syndication';");

                this.admin_item_admintools.Attributes.Add(
                    "onmouseover", "document.getElementById('" + this.admin_item_admintools.ClientID + "').className='admin_item_admintools_sel';");
                this.admin_item_admintools.Attributes.Add(
                    "onmouseout", "document.getElementById('" + this.admin_item_admintools.ClientID + "').className='admin_item_admintools';");

                // check VI for null then set information
                if (!this.Page.IsPostBack)
                {
                    // check if the admin settings for AMS have been set, if not load the settings page.
                    if (this.IsAdmin)
                    {
                        this.divSettings.Visible = true;
                        this.divDelete.Visible = true;
                        this.divComments.Visible = true;

                        // TODO: if we enable syndication move this to admin settings
                        this.divSyndication.Visible = false;
                        this.divAdminTools.Visible = true;
                        this.divCategories.Visible = true;
                    }
                    else
                    {
                        this.divSettings.Visible = false;
                        this.divDelete.Visible = false;
                        this.divComments.Visible = true;
                        this.divSyndication.Visible = false;
                        this.divAdminTools.Visible = false;
                        if (this.IsCommentsEnabled)
                        {
                            this.divComments.Visible = true;
                        }
                    }

                    if (AllowAuthorEditCategory(this.PortalId) && !this.IsAdmin)
                    {
                        this.divCategories.Visible = true;
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