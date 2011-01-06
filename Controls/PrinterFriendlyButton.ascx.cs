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
    using System.Globalization;

    using DotNetNuke.Services.Exceptions;

    public partial class PrinterFriendlyButton : ModuleBase
    {
        public static string GetPrintFriendlyLinkUrl(int itemId, int portalId, int tabId)
        {
            return ApplicationUrl + DesktopModuleFolderName + "printerfriendly.aspx?itemId=" + itemId.ToString(CultureInfo.InvariantCulture) +
                   "&PortalId=" + portalId.ToString(CultureInfo.InvariantCulture) + "&TabId=" + tabId.ToString(CultureInfo.InvariantCulture);
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
                this.lnkPrinterFriendly.NavigateUrl = GetPrintFriendlyLinkUrl(this.ItemId, this.PortalId, this.TabId);
                this.lnkPrinterFriendly.Target = "_blank";
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}