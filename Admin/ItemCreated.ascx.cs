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
    using System.Globalization;

    using DotNetNuke.Common;
    using DotNetNuke.Services.Exceptions;

    using Engage.Dnn.Publish.Util;

    public partial class ItemCreated : ModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
            this.BindItemData();
        }

        private void InitializeComponent()
        {
            this.Load += this.Page_Load;
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // check VI for null then set information
                if (!this.Page.IsPostBack)
                {
                    // ItemId
                    this.lblItemIdValue.Text = this.ItemId.ToString(CultureInfo.InvariantCulture);
                    this.lnkItemVersion.NavigateUrl = Globals.NavigateURL(
                        this.TabId, 
                        string.Empty, 
                        string.Empty, 
                        "ctl=" + Utility.AdminContainer + "&mid=" + this.ModuleId.ToString(CultureInfo.InvariantCulture) +
                        "&adminType=versionslist&itemId=" + this.ItemId.ToString(CultureInfo.InvariantCulture));
                    this.lnkCategoryList.NavigateUrl = this.BuildCategoryListUrl(ItemType.Article);
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
    }
}