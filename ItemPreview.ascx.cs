//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;

    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.ArticleControls;
    using Engage.Dnn.Publish.CategoryControls;
    using Engage.Dnn.Publish.Util;

    public partial class ItemPreview : ModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.Load += new EventHandler(ItemPreview_Load);
            InitializeComponent();
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", 
            MessageId = "System.InvalidOperationException.#ctor(System.String)", Justification = "Message is for internal use only")]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", 
            MessageId = "System.Exception.#ctor(System.String)", Justification = "Message is for internal use only")]
        private void DisplayCurrentVersion()
        {
            this.SetItemId(Convert.ToInt32(this.Request.QueryString["itemid"], CultureInfo.InvariantCulture));

            if (this.TypeOfItem == ItemType.Article)
            {
                string articleControlToLoad = "~" + DesktopModuleFolderName + "ArticleControls/articleDisplay.ascx";
                Article a = Article.GetArticle(this.ItemId, this.PortalId);
                if (a == null)
                {
                    throw new InvalidOperationException("Article not found");
                }

                var ad = (ArticleDisplay)this.LoadControl(articleControlToLoad);
                ad.ModuleConfiguration = this.ModuleConfiguration;
                ad.ID = Path.GetFileNameWithoutExtension(articleControlToLoad);
                ad.Overrideable = true;
                ad.DisplayPrinterFriendly = false;
                ad.DisplayRelatedArticle = false;
                ad.DisplayRelatedLinks = false;
                ad.DisplayEmailAFriend = false;
                ad.SetItemId(a.ItemId);
                this.phItem.Controls.Add(ad);
            }
            else if (this.TypeOfItem == ItemType.Category)
            {
                string categoryControlToLoad = "~" + DesktopModuleFolderName + "CategoryControls/CategoryDisplay.ascx";
                Category category = Category.GetCategory(this.ItemId, this.PortalId);
                if (category == null)
                {
                    throw new InvalidOperationException("Category not found");
                }

                var cd = (CategoryDisplay)this.LoadControl(categoryControlToLoad);
                cd.ModuleConfiguration = this.ModuleConfiguration;
                cd.ID = Path.GetFileNameWithoutExtension(categoryControlToLoad);
                cd.Overrideable = true;
                cd.ShowAll = true;
                cd.SetItemId(category.ItemId);
                this.phItem.Controls.Add(cd);
            }
            else
            {
                throw new InvalidOperationException("Invalid Item Type");
            }
        }

        private void DisplayItem()
        {
            // First figure out if we're looking at the current item or a version of it.
            if (this.Request.QueryString["itemversionid"] == null)
            {
                this.DisplayCurrentVersion();
            }
            else
            {
                // TODO: Implement this in the case where the use in in ItemVersion.ascx and clicks a link to 
                // an item that is not pointing to a valid displaytabid. hk
                this.DisplayVersion();
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Not yet implemented")]
        private void DisplayVersion()
        {
            // TODO: See top of this file for comments about why/should we implement this. hk
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

        private void ItemPreview_Load(object sender, EventArgs e)
        {
            DisplayItem();
        }
    }
}