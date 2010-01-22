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
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Localization;
    using DotNetNuke.Services.Exceptions;

    public partial class AdminMain : ModuleBase, IActionable
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
                admin_item_articles.Attributes.Add("onmouseover", "document.getElementById('" + admin_item_articles.ClientID + "').className='admin_item_articles_sel';");
                admin_item_articles.Attributes.Add("onmouseout", "document.getElementById('" + admin_item_articles.ClientID + "').className='admin_item_articles';");

                admin_item_categories.Attributes.Add("onmouseover", "document.getElementById('" + admin_item_categories.ClientID + "').className='admin_item_categories_sel';");
                admin_item_categories.Attributes.Add("onmouseout", "document.getElementById('" + admin_item_categories.ClientID + "').className='admin_item_categories';");

                admin_item_settings.Attributes.Add("onmouseover", "document.getElementById('" + admin_item_settings.ClientID + "').className='admin_item_settings_sel';");
                admin_item_settings.Attributes.Add("onmouseout", "document.getElementById('" + admin_item_settings.ClientID + "').className='admin_item_settings';");

                //admin_item_delete.Attributes.Add("onmouseover", "document.getElementById('" + admin_item_help.ClientID + "').className='admin_item_help_sel';");
                //admin_item_delete.Attributes.Add("onmouseout", "document.getElementById('" + admin_item_help.ClientID + "').className='admin_item_help';");

                admin_item_comments.Attributes.Add("onmouseover", "document.getElementById('" + admin_item_comments.ClientID + "').className='admin_item_comments_sel';");
                admin_item_comments.Attributes.Add("onmouseout", "document.getElementById('" + admin_item_comments.ClientID + "').className='admin_item_comments';");
                
                admin_item_delete.Attributes.Add("onmouseover", "document.getElementById('" + admin_item_delete.ClientID + "').className='admin_item_delete_sel';");
                admin_item_delete.Attributes.Add("onmouseout", "document.getElementById('" + admin_item_delete.ClientID + "').className='admin_item_delete';");

                admin_item_syndication.Attributes.Add("onmouseover", "document.getElementById('" + admin_item_syndication.ClientID + "').className='admin_item_syndication_sel';");
                admin_item_syndication.Attributes.Add("onmouseout", "document.getElementById('" + admin_item_syndication.ClientID + "').className='admin_item_syndication';");


                admin_item_admintools.Attributes.Add("onmouseover", "document.getElementById('" + admin_item_admintools.ClientID + "').className='admin_item_admintools_sel';");
                admin_item_admintools.Attributes.Add("onmouseout", "document.getElementById('" + admin_item_admintools.ClientID + "').className='admin_item_admintools';");


				//check VI for null then set information
				if (!Page.IsPostBack)
				{
                    //check if the admin settings for AMS have been set, if not load the settings page.
					if(IsAdmin)
					{
						divSettings.Visible=true;
						divDelete.Visible=true;
                        divComments.Visible = true;
                        //TODO: if we enable syndication move this to admin settings
                        divSyndication.Visible = false;
                        divAdminTools.Visible = true;
                        divCategories.Visible = true;
					}
					else
					{
						divSettings.Visible=false;
						divDelete.Visible=false;
                        divComments.Visible = true;
                        divSyndication.Visible = false;
                        divAdminTools.Visible = false;
                        if (IsCommentsEnabled)
                        {
                            divComments.Visible = true;
                        }
                    }
                    if (AllowAuthorEditCategory(PortalId) && !IsAdmin)
                    {
                        divCategories.Visible = true;
                    }
				}
			} 
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}

		#endregion

		#region Optional Interfaces

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions 
		{
			get 
			{
				DotNetNuke.Entities.Modules.Actions.ModuleActionCollection actions = new DotNetNuke.Entities.Modules.Actions.ModuleActionCollection();
				actions.Add(GetNextActionID(), Localization.GetString(DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), DotNetNuke.Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl(), false, DotNetNuke.Security.SecurityAccessLevel.Edit, true, false);
				return actions;
			}
		}

		#endregion

	}
}

