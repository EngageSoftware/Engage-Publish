//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish
{

    using System;
    using System.Globalization;
    using System.IO;

    using DotNetNuke.Services.Localization;
    using ArticleControls;
    using CategoryControls;
    using Util;

    public partial class ItemPreview : ModuleBase
	{

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
            base.OnInit(e);
            base.Load += new EventHandler(ItemPreview_Load);
			InitializeComponent();
		}

        void ItemPreview_Load(object sender, EventArgs e)
        {
            DisplayItem();
        }
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            SetItemId(Convert.ToInt32(this.Request.QueryString["itemid"], CultureInfo.InvariantCulture));

            if (TypeOfItem == ItemType.Article)
            {
                ModuleConfiguration.ModuleTitle = Localization.GetString("ArticlePreview", LocalResourceFile);
            }
            else
            {
                ModuleConfiguration.ModuleTitle = Localization.GetString("CategoryPreview", LocalResourceFile);
            }
		}
		#endregion	

		#region Event Handlers

		#endregion

        private void DisplayItem()
        {

            //First figure out if we're looking at the current item or a version of it.
            if (Request.QueryString["itemversionid"] == null)
            {
                DisplayCurrentVersion();
            }
            else
            {
                //TODO: Implement this in the case where the use in in ItemVersion.ascx and clicks a link to 
                //an item that is not pointing to a valid displaytabid. hk
                DisplayVersion();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.InvalidOperationException.#ctor(System.String)", Justification = "Message is for internal use only"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.Exception.#ctor(System.String)", Justification = "Message is for internal use only")]
        private void DisplayCurrentVersion()
        {
            SetItemId(Convert.ToInt32(Request.QueryString["itemid"], CultureInfo.InvariantCulture));

            if (TypeOfItem == ItemType.Article)
            {
                string articleControlToLoad = "~" + DesktopModuleFolderName + "ArticleControls/articleDisplay.ascx";
                Article a = Article.GetArticle(ItemId, PortalId);
                if (a == null)
                {
                    throw new InvalidOperationException("Article not found");
                }
                var ad = (ArticleDisplay)LoadControl(articleControlToLoad);
                ad.ModuleConfiguration = ModuleConfiguration;
                ad.ID = Path.GetFileNameWithoutExtension(articleControlToLoad);
                ad.Overrideable = true;
                ad.DisplayPrinterFriendly = false;
                ad.DisplayRelatedArticle = false;
                ad.DisplayRelatedLinks = false;
                ad.DisplayEmailAFriend = false;
                ad.SetItemId(a.ItemId);
                phItem.Controls.Add(ad);
            }
            else if (TypeOfItem == ItemType.Category)
            {
                string categoryControlToLoad = "~" + DesktopModuleFolderName + "CategoryControls/CategoryDisplay.ascx";
                Category category = Category.GetCategory(ItemId, PortalId);
                if (category == null)
                {
                    throw new InvalidOperationException("Category not found");
                }
                var cd = (CategoryDisplay)LoadControl(categoryControlToLoad);
                cd.ModuleConfiguration = ModuleConfiguration;
                cd.ID = Path.GetFileNameWithoutExtension(categoryControlToLoad);
                cd.Overrideable = true;
                cd.ShowAll = true;
                cd.SetItemId(category.ItemId);
                phItem.Controls.Add(cd);
            }
            else
            {
                throw new InvalidOperationException("Invalid Item Type");
            }
           
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Not yet implemented")]
        private void DisplayVersion()
        {
            //TODO: See top of this file for comments about why/should we implement this. hk
        }

	}
}

