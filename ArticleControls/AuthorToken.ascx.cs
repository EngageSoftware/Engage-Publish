//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Mail;
using DotNetNuke.Entities.Users;
using System.Text;

namespace Engage.Dnn.Publish.ArticleControls
{
    public partial class AuthorToken : ModuleBase
	{
		#region Event Handlers
        protected override void OnInit(EventArgs e)
        {
            this.Load += Page_Load;
            base.OnInit(e);
        }

        void Page_Load(object sender, EventArgs e)
        {
            LoadAuthorInfo();
        }

#endregion
        private void LoadAuthorInfo()
        {
            UserInfo ui = UserController.GetUser(PortalId, VersionInfoObject.AuthorUserId);

            //configure author link
            lblAuthorLink.NavigateUrl = ui.Profile.Website;
            lblAuthorLink.Text = ui.DisplayName;
            
            StringBuilder sb = new StringBuilder(500);
            lblAuthorInfo.Text = ui.Profile.GetPropertyValue("Bio").ToString();        

        }

      
        
      
	}
}

