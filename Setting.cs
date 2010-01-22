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
    using System.Diagnostics;
    using System.Text;
    public class Setting
    {
        //Article Settings - NOTE: Some are shared i.e. UseApprovals. hk
        public static readonly Setting PrinterFriendly = new Setting("pnlPrinterFriendly", "Visible");//, "Hide/Show the printer friendly link on the module");
        public static readonly Setting EmailAFriend = new Setting("pnlEmailAFriend", "Visible");//, "Hide/Show the Email a Friend link on the module");
        public static readonly Setting Rating = new Setting("upnlRating", "Visible");//, "Hide/Show the rating options.");
        public static readonly Setting Comments = new Setting("pnlComments", "Visible");//, "Hide/Show the comments about the item.");
        public static readonly Setting ForumComments = new Setting("chkForumComments", "Checked");
        public static readonly Setting CommentForumThreadId = new Setting("ArticleSetting", "CommentForumThreadId");
        public static readonly Setting ArticleSettingIncludeCategories = new Setting("ArticleSettings", "IncludeParentCategoryArticles");//, "Whether or not the display will include parent categories in the display.");
        public static readonly Setting ArticleSettingCurrentDisplay = new Setting("ArticleSettings", "DisplayOnCurrentPage");//, "Automatically include other articles from the same Article List(s).");
        public static readonly Setting ArticleSettingForceDisplay = new Setting("ArticleSettings", "ForceDisplayOnPage");
        public static readonly Setting ArticleSettingReturnToList = new Setting("ArticleSettings", "DisplayReturnToList");
        public static readonly Setting Author = new Setting("pnlAuthor", "Visible");//, "Display the Author's name on the module.");
        public static readonly Setting AuthorName = new Setting("lblAuthorName", "Text");//, "Value of author's name to display.");
        public static readonly Setting ShowTags = new Setting("pnlTags", "Visible");//, "Show the TagList on an article.");
        public static readonly Setting UseApprovals = new Setting("chkUseApprovals", "Checked");//, "If checked, approval workflow is disabled for this item.");
        public static readonly Setting UseSimpleGalleryAlbum = new Setting("ddlSimpleGalleryAlbum", "SelectedValue");
        public static readonly Setting UseUltraMediaGalleryAlbum = new Setting("ddlUltraMediaGalleryAlbum", "SelectedValue");
        public static readonly Setting ArticleAttachment = new Setting("ArticleSettings", "ArticleAttachment");

        //Categories Settings
        public static readonly Setting CategorySettingsCurrentDisplay = new Setting("CategorySettings", "DisplayOnCurrentPage");
        public static readonly Setting CategorySettingsForceDisplay = new Setting("CategorySettings", "ForceDisplayOnPage");
        public static readonly Setting CategorySettingsCommentForumId = new Setting("CategorySettings", "CommentForumId");
        public static readonly Setting CategorySettingsRssUrl = new Setting("CategorySettings", "RssUrl");

        private Setting(string controlName, string propertyName)//, string description)
        {
            _controlName = controlName;
            _propertyName = propertyName;
            //this._description = description;
        }

        #region Public Properties

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _controlName = string.Empty;
        public string ControlName
        {
            [DebuggerStepThrough]
            get { return _controlName; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _propertyName = string.Empty;
        public string PropertyName
        {
            [DebuggerStepThrough]
            get { return _propertyName; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _propertyValue = string.Empty;
        public string PropertyValue
        {
            [DebuggerStepThrough]
            get { return _propertyValue; }
            [DebuggerStepThrough]
            set { _propertyValue = value; }
        }

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //private string _description = string.Empty;
        //public string Description
        //{
        //    [DebuggerStepThrough]
        //    get { return _description; }
        //    [DebuggerStepThrough]
        //    set { _description = value; }
        //}

        #endregion

        //public static List<Setting> GetList(Type ct)
        //{
        //    if (ct == null) throw new ArgumentNullException("ct");

        //    List<Setting> settings = new List<Setting>();

        //    Type type = ct;
        //    while (type.BaseType != null)
        //    {
        //        FieldInfo[] fi = type.GetFields();

        //        foreach (FieldInfo f in fi)
        //        {
        //            Setting o = f.GetValue(type) as Setting;
        //            if (o != null)
        //            {
        //                settings.Add(o);
        //            }
        //        }

        //        type = type.BaseType; //check the super type 
        //    }

        //    return settings;
        //}

        public override string ToString()
        {
            var builder = new StringBuilder(128);

            builder.Append("Control Name: ");
            builder.Append(_controlName);
            builder.Append(" Property Name: ");
            builder.Append(_propertyName);
            builder.Append(" Property Value: ");
            builder.Append(_propertyValue);

            return builder.ToString();
        }

   }
}
