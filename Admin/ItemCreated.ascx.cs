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
    using System.Globalization;
    using DotNetNuke.Services.Exceptions;
    using Util;

	public partial class ItemCreated : ModuleBase
	{
		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		    BindItemData();
		}
		
		private void InitializeComponent()
		{
			Load += Page_Load;
		}

		private void Page_Load(object sender, EventArgs e)
		{
			try 
			{
				//check VI for null then set information
				if (!Page.IsPostBack)
				{
					//ItemId
					lblItemIdValue.Text = ItemId.ToString(CultureInfo.InvariantCulture);
                    lnkItemVersion.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(TabId, "", "", "ctl=" + Utility.AdminContainer + "&mid=" + ModuleId.ToString(CultureInfo.InvariantCulture) + "&adminType=versionslist&itemId=" + ItemId.ToString(CultureInfo.InvariantCulture));
				    this.lnkCategoryList.NavigateUrl = BuildCategoryListUrl(ItemType.Article);
				}
			} 
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}
	}
}

