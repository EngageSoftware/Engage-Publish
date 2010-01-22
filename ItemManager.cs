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
            _module = module;
        }

        public int ItemId
        {
            get
            {
                int id = -1;
                object o = _module.Settings["ItemId"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    id = Convert.ToInt32(o, CultureInfo.InvariantCulture);
                }
                return id;
             }
        }

        public string DisplayType
        {
            get
            {
                string displayType = string.Empty;
                Object o = _module.Settings["DisplayType"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    displayType =  o.ToString();
                }

                return displayType;
            }
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
                    prefix = "";
                    break;
                default:
                    prefix = "cd";
                    break;
            }
            return prefix;

        }

        public int PreviousArticleId
        {
            get
            {
                int id = -1;
                object o = _module.Settings["OldArticleId"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
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
                string settingPrefix = GetDisplayTypePrefix(DisplayType);
                object o = _module.Settings[settingPrefix + "DisplayOption"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    displayOption = o.ToString();
                }

                return displayOption;
            }
        }

        public int PreviousCategoryId
        {
            get
            {
                int id = -1;
                object o = _module.Settings["OldCategoryId"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
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
                string settingPrefix = GetDisplayTypePrefix(DisplayType);
                object o = _module.Settings[settingPrefix + "CategoryId"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    id = Convert.ToInt32(o, CultureInfo.InvariantCulture);
                }

                return id;
            }
        }

        public int AdArticleId
        {
            get
            {
                int id = -1;
                object o = _module.Settings["adArticleId"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
                {
                    id = Convert.ToInt32(o, CultureInfo.InvariantCulture);
                }

                return id;
            }
        }

        public bool IsOverrideable
        {
            get
            {
                object o = _module.Settings["Overrideable"];
                if (o != null && !String.IsNullOrEmpty(o.ToString()))
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

        public int ResolveId()
        {
            //TODO: Need to test this method with a database (children's) that has all the scenarios.
            //NOTE: At this point I'm not sure of the presedence here. The other issue is that within the
            //presedence issue there may be "exit" situations which is hard to control/debug. Need valid
            //test cases to further refactor.hk

            int id = -1;
            //there is an oldArticleId and displayOption is "Article"
            //NOTE: TO make these work you must verify that the mapping tables exists.
            //Publish_CategoryMapping
            //Columns:  NewItemId and OldArticleID
            //Publish_ArticleMapping
            //Columns: NewItemId and OldCateogryID

            if (PreviousArticleId != -1 && string.Compare(DisplayOption, "texthtml", StringComparison.OrdinalIgnoreCase) == 0)
            {
                id = ItemId;
            }
            
            //Verify that these tables have been populated correctly! hk
            if (PreviousArticleId != -1 && string.Compare(DisplayOption, "article", StringComparison.OrdinalIgnoreCase) == 0)
            {
                id = Article.GetOldArticleId(PreviousArticleId);
            }

            //there is an oldArticleID and displayOption is either "Title" or "Abstract"
            if (PreviousCategoryId != -1 && (string.Compare(DisplayOption, "title", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(DisplayOption, "abstract", StringComparison.OrdinalIgnoreCase) == 0))
            {
                id = Category.GetOldCategoryId(PreviousCategoryId);
            }

            //Old article ID passed in (aid).
            object oAid = HttpContext.Current.Request.QueryString["aid"];
            if (oAid != null)
            {
                id = Article.GetOldArticleId(Convert.ToInt32(oAid, CultureInfo.InvariantCulture));
            }

            //There is an ArticleId 
            if (AdArticleId != -1 && DisplayType == "ArticleDisplay")
            {
                if (!IsOverrideable || (IsOverrideable && id == -1))
                {
                    id = AdArticleId;
                }
            }
            else if (AdArticleId != -1 && String.IsNullOrEmpty(DisplayType))
            {
                id = AdArticleId;
            }

            //There is a CategoryId
            if (CategoryId != -1 && (DisplayType == "CategoryDisplay" || DisplayType == "ItemListing" || DisplayType == "CategoryFeatureDisplay" || DisplayType == "CategorySearch" || DisplayType == "CategoryNLevels" || DisplayType == "CustomDisplay"))
            {
                if (!IsOverrideable || (IsOverrideable && id == -1))
                {
                    id = CategoryId;
                }
            }

            if (CategoryId != -1 && String.IsNullOrEmpty(DisplayType))
            {
                id = CategoryId;
            }

            return id;       
        }
    }
}