//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Exceptions;
using System.Web.UI.WebControls;

namespace Engage.Dnn.Publish.Admin.Tools
{
    public partial class Dashboard : ModuleBase
    {
        #region Event Handlers

        override protected void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {

                //TODO: uncomment below when reporting works
                //hl.NavigateUrl = BuildLinkUrl("&amp;mid=" + ModuleId + "&amp;ctl=admincontainer&amp;adminType=admintools&amp;tool=itemviewreport");
                //hl.Text = Localization.GetString("ItemViewReport", LocalResourceFile);
                //phAdminTools.Controls.Add(hl);

                //Literal lit = new Literal();
                //lit.Text = " &nbsp; ";
                //phAdminTools.Controls.Add(lit);

                HyperLink hl = new HyperLink();
                hl.NavigateUrl = BuildLinkUrl("&amp;mid=" + ModuleId + "&amp;ctl=admincontainer&amp;adminType=admintools&amp;tool=descriptionreplace");
                hl.Text = Localization.GetString("DescriptionReplace", LocalResourceFile);
                phAdminTools.Controls.Add(hl);

                //TODO: build a way to clear Publish cache

            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion
    }
}

