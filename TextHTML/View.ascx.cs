//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.TextHtml
{
    using System.Web.UI;
    using DotNetNuke.Entities.Modules.Actions;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Security;

    public partial class View : ModuleBase, IActionable
    {


        override protected void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //todo: check to see if the default Text/HTML category has been set in the Publish Settings, if not display a message.
                
                //load the article id (itemid) from the module settings.
                
                LoadArticle();

            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
        private void LoadArticle()
        {
            if(Settings.Contains("ItemId"))
            {
                Article a = Article.GetArticle(Convert.ToInt32(Settings["ItemId"]), PortalId);
                lblArticleText.Text = a.ArticleText;
            }
        }


        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actions = new ModuleActionCollection();
                actions.Add(GetNextActionID(), Localization.GetString("Edit", GlobalResourceFile), "", "", "", EditUrl(), false, SecurityAccessLevel.Edit, true, false);
                return actions;
            }
        }
    }
}

