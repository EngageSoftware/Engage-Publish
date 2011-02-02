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
    using System.Globalization;
    using System.Web;

    using DotNetNuke.Entities.Modules;

    /// <summary>
    /// Summary description for ItemManager
    /// </summary>
    public sealed class ItemManager
    {
        private readonly PortalModuleBase _module;

        public ItemManager(PortalModuleBase module)
        {
            this._module = module;
        }

        public int AdArticleId
        {
            get
            {
                int id = -1;
                object o = this._module.Settings["adArticleId"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    id = Convert.ToInt32(o, CultureInfo.InvariantCulture);
                }

                return id;
            }
        }

        public int CategoryId
        {
            get
            {
                int id = -1;
                string settingPrefix = GetDisplayTypePrefix(this.DisplayType);
                object o = this._module.Settings[settingPrefix + "CategoryId"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    id = Convert.ToInt32(o, CultureInfo.InvariantCulture);
                }

                return id;
            }
        }

        public string DisplayOption
        {
            get
            {
                string displayOption = string.Empty;
                string settingPrefix = GetDisplayTypePrefix(this.DisplayType);
                object o = this._module.Settings[settingPrefix + "DisplayOption"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    displayOption = o.ToString();
                }

                return displayOption;
            }
        }

        public string DisplayType
        {
            get
            {
                string displayType = string.Empty;
                object o = this._module.Settings["DisplayType"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    displayType = o.ToString();
                }

                return displayType;
            }
        }

        public bool IsOverrideable
        {
            get
            {
                object o = this._module.Settings["Overrideable"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    bool overrideable;
                    if (bool.TryParse(o.ToString(), out overrideable))
                    {
                        return overrideable;
                    }
                }

                return true;
            }
        }

        public int ItemId
        {
            get
            {
                int id = -1;
                object o = this._module.Settings["ItemId"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    id = Convert.ToInt32(o, CultureInfo.InvariantCulture);
                }

                return id;
            }
        }

        public int PreviousArticleId
        {
            get
            {
                int id = -1;
                object o = this._module.Settings["OldArticleId"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    id = Convert.ToInt32(o, CultureInfo.InvariantCulture);
                }

                return id;
            }
        }

        public int PreviousCategoryId
        {
            get
            {
                int id = -1;
                object o = this._module.Settings["OldCategoryId"];
                if (o != null && !string.IsNullOrEmpty(o.ToString()))
                {
                    id = Convert.ToInt32(o, CultureInfo.InvariantCulture);
                }

                return id;
            }
        }

        public int ResolveId()
        {
            // TODO: Need to test this method with a database (children's) that has all the scenarios.
            // NOTE: At this point I'm not sure of the presedence here. The other issue is that within the
            // presedence issue there may be "exit" situations which is hard to control/debug. Need valid
            // test cases to further refactor.hk
            int id = -1;

            // there is an oldArticleId and displayOption is "Article"
            // NOTE: TO make these work you must verify that the mapping tables exists.
            // Publish_CategoryMapping
            // Columns:  NewItemId and OldArticleID
            // Publish_ArticleMapping
            // Columns: NewItemId and OldCateogryID
            if (this.PreviousArticleId != -1 && string.Compare(this.DisplayOption, "texthtml", StringComparison.OrdinalIgnoreCase) == 0)
            {
                id = this.ItemId;
            }

            // Verify that these tables have been populated correctly! hk
            if (this.PreviousArticleId != -1 && string.Compare(this.DisplayOption, "article", StringComparison.OrdinalIgnoreCase) == 0)
            {
                id = Article.GetOldArticleId(this.PreviousArticleId);
            }

            // there is an oldArticleID and displayOption is either "Title" or "Abstract"
            if (this.PreviousCategoryId != -1 &&
                (string.Compare(this.DisplayOption, "title", StringComparison.OrdinalIgnoreCase) == 0 ||
                 string.Compare(this.DisplayOption, "abstract", StringComparison.OrdinalIgnoreCase) == 0))
            {
                id = Category.GetOldCategoryId(this.PreviousCategoryId);
            }

            // Old article ID passed in (aid).
            object oAid = HttpContext.Current.Request.QueryString["aid"];
            if (oAid != null)
            {
                id = Article.GetOldArticleId(Convert.ToInt32(oAid, CultureInfo.InvariantCulture));
            }

            // There is an ArticleId 
            if (this.AdArticleId != -1 && this.DisplayType == "ArticleDisplay")
            {
                if (!this.IsOverrideable || (this.IsOverrideable && id == -1))
                {
                    id = this.AdArticleId;
                }
            }
            else if (this.AdArticleId != -1 && string.IsNullOrEmpty(this.DisplayType))
            {
                id = this.AdArticleId;
            }

            // There is a CategoryId
            if (this.CategoryId != -1 &&
                (this.DisplayType == "CategoryDisplay" || this.DisplayType == "ItemListing" || this.DisplayType == "CategoryFeatureDisplay" ||
                 this.DisplayType == "CategorySearch" || this.DisplayType == "CategoryNLevels" || this.DisplayType == "CustomDisplay"))
            {
                if (!this.IsOverrideable || (this.IsOverrideable && id == -1))
                {
                    id = this.CategoryId;
                }
            }

            if (this.CategoryId != -1 && string.IsNullOrEmpty(this.DisplayType))
            {
                id = this.CategoryId;
            }

            return id;
        }

        private static string GetDisplayTypePrefix(string displayType)
        {
            string prefix;

            switch (displayType)
            {
                case "CategoryFeatureDisplay":
                    prefix = "cf";
                    break;
                case "ItemListing":
                    prefix = "il";
                    break;
                case "CategorySearch":
                    prefix = "cs";
                    break;
                case "CategoryNLevels":
                    prefix = "n";
                    break;
                case "CustomDisplay":
                    prefix = string.Empty;
                    break;
                default:
                    prefix = "cd";
                    break;
            }

            return prefix;
        }
    }
}