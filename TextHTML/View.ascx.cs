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
            SetItemId();
            this.Load += this.Page_Load;
            base.OnInit(e);
            BindItemData(true);
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
                //todo: check to see if the default Text/HTML category has been set in the Publish Settings, if not display a message.
                //load the article id (itemid) from the module settings.
                LoadArticle();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void SetItemId()
        {
            //TODO: look at setting itemid in the ItemId property off module base

            //TODO: make the preview mode work
            if (LocalItemId > 0)
            {
                this.SetItemId(LocalItemId);
            }
        }

        private void LoadArticle()
        {
            //TODO: can we use binditem from modulebase?
            //check if we should be loading a preview version
            object o = Request.QueryString["VersionId"];
            object m = Request.QueryString["modid"];
            Article a = null;
            if (o != null && m != null)
            {
                if (m.ToString() == ModuleId.ToString())
                {
                    a = Article.GetArticleVersion(Convert.ToInt32(o.ToString()), PortalId);
                }
            }
            else
            {
                a = Article.GetArticle(ItemId, PortalId);
            }
            if (a != null) lblArticleText.Text = a.ArticleText;
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                ModuleActionCollection actions = new ModuleActionCollection();
                actions.Add(GetNextActionID(), Localization.GetString("Edit", LocalSharedResourceFile), "", "", "", EditUrl(), false, SecurityAccessLevel.Edit, true, false);
                //                actions.Add(GetNextActionID(), Localization.GetString("Administration", LocalSharedResourceFile), "", "", "", EditUrl(Utility.AdminContainer), false, SecurityAccessLevel.Edit, true, false);
                if (LocalItemId > 0)
                {
                    actions.Add(GetNextActionID(), Localization.GetString("Versions", LocalSharedResourceFile), "", "", "", BuildVersionsUrl(), false, SecurityAccessLevel.Edit, true, false);
                }
                return actions;
            }
        }
    }
}

