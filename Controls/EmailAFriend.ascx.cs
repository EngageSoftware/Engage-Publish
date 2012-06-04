//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Controls
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Mail;

    public partial class EmailAFriend : ModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void btnCancel_Click1(object sender, EventArgs e)
        {
            this.ClearCommentInput();
        }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Member", Justification = "Controls use lower case prefix")]
        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                string message = this.Localize("EmailAFriend");
                message = message.Replace("[Engage:Recipient]", this.txtTo.Text.Trim());
                message = message.Replace("[Engage:Url]", this.GetItemLinkUrlExternal(this.ItemId));
                message = message.Replace("[Engage:From]", this.txtFrom.Text.Trim());
                message = message.Replace("[Engage:Message]", this.txtMessage.Text.Trim());

                string subject = this.Localize("EmailAFriendSubject");
                subject = subject.Replace("[Engage:Portal]", this.PortalSettings.PortalName);

                Mail.SendMail(
                    this.PortalSettings.Email,
                    this.txtTo.Text.Trim(),
                    string.Empty,
                    subject,
                    message,
                    string.Empty,
                    "HTML",
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty);
                this.ClearCommentInput();
            }
            catch (Exception ex)
            {
                // the email or emails entered are invalid or mail services are not configured
                Exceptions.LogException(ex);
            }
        }

        private void ClearCommentInput()
        {
            this.txtFrom.Text = string.Empty;
            this.txtMessage.Text = string.Empty;
            this.txtTo.Text = string.Empty;
            this.txtFrom.Text = this.UserInfo != null ? this.UserInfo.Email : string.Empty;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            this.AddJQueryReference();
            this.Page.ClientScript.RegisterClientScriptInclude("Engage_Publish_ModalPopup", this.ResolveUrl("../Scripts/ModalPopup.js"));
            this.EmailAFriendPopupTriggerLink.Attributes["data-modal-target-id"] = this.pnlEmailAFriend.ClientID;

            if (!this.IsPostBack)
            {
                this.txtFrom.Text = this.UserInfo != null ? this.UserInfo.Email : string.Empty;
            }
        }
    }
}