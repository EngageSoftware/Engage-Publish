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

namespace Engage.Dnn.Publish.Controls
{
	public partial class EmailAFriend :  ModuleBase
	{
		#region Event Handlers
        protected override void OnInit(EventArgs e)
        {
            this.Load += Page_Load;
            base.OnInit(e);
        }

        void Page_Load(object sender, EventArgs e)
        {
            txtFrom.Text = UserInfo != null ? UserInfo.Email : string.Empty;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            ClearCommentInput();
            mpeEmailAFriend.Hide();
        }

        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string message = Localization.GetString("EmailAFriend", LocalResourceFile);
                message = message.Replace("[Engage:Recipient]", txtTo.Text.Trim());
                message = message.Replace("[Engage:Url]", GetItemLinkUrlExternal(ItemId));
                message = message.Replace("[Engage:From]", txtFrom.Text.Trim());
                message = message.Replace("[Engage:Message]", txtMessage.Text.Trim());

                string subject = Localization.GetString("EmailAFriendSubject", LocalResourceFile);
                subject = subject.Replace("[Engage:Portal]", PortalSettings.PortalName);

                Mail.SendMail(PortalSettings.Email.ToString(), txtTo.Text.Trim(), "", subject, message, "", "HTML", "", "", "", "");
                ClearCommentInput();
                mpeEmailAFriend.Hide();
                
            }
            catch (Exception ex)
            {
                //the email or emails entered are invalid or mail services are not configured
                Exceptions.LogException(ex);
            }
        }
		#endregion

		private void ClearCommentInput()
        {
            txtFrom.Text = string.Empty;
            txtMessage.Text = string.Empty;
            txtTo.Text = string.Empty;
        }
	}
}

