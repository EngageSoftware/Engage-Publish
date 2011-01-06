//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.ArticleControls
{
    using System;

    using DotNetNuke.Entities.Users;

    public partial class AuthorToken : ModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        private void LoadAuthorInfo()
        {
            var uc = new UserController();
            UserInfo ui = uc.GetUser(this.PortalId, this.VersionInfoObject.AuthorUserId);

            // configure author link
            this.lblAuthorLink.NavigateUrl = ui.Profile.Website;
            this.lblAuthorLink.Text = ui.DisplayName;

            this.lblAuthorInfo.Text = ui.Profile.GetPropertyValue("Bio");
        }

        private void Page_Load(object sender, EventArgs e)
        {
            this.LoadAuthorInfo();
        }
    }
}