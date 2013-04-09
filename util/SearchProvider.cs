// <copyright file="SearchProvider.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Util
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Search;

    using Engage.Dnn.Publish.Data;

    /// <summary>
    /// Gets the search items for a module
    /// </summary>
    public class SearchProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchProvider"/> class.
        /// </summary>
        /// <param name="module">The module for which to get articles.</param>
        /// <param name="setGuid">if set to <c>true</c> sets <see cref="SearchItemInfo.GUID"/>, otherwise leaves it blank.</param>
        public SearchProvider(ModuleInfo module, bool setGuid)
        {
            this.Module = module;
            this.SetGuid = setGuid;
        }

        /// <summary>
        /// Gets the module whose articles are to be indexed.
        /// </summary>
        public ModuleInfo Module { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="SearchItemInfo.GUID"/> field should be set (i.e. whether an ItemId parameter should be included on the querystring for the search result).
        /// </summary>
        /// <value>
        /// <c>true</c> if <see cref="SearchItemInfo.GUID"/> is set; otherwise, <c>false</c>.
        /// </value>
        public bool SetGuid { get; private set; }

        /// <summary>
        /// Gets the search items for the module's articles.
        /// </summary>
        /// <returns>The collection of search items for the <see cref="Module"/>'s articles</returns>
        public SearchItemInfoCollection GetSearchItems()
        {
            return new SearchItemInfoCollection(this.GetSearchItemsImpl().ToArray());
        }

        /// <summary>
        /// Gets the search items for the module's articles.
        /// </summary>
        /// <returns>The collection of search items for the <see cref="Module"/>'s articles</returns>
        private IEnumerable<SearchItemInfo> GetSearchItemsImpl()
        {
            var articlesTable = Article.GetArticlesByModuleId(this.Module.ModuleID, true);
            foreach (DataRow row in articlesTable.Rows)
            {
                var searchedContent = new StringBuilder(8192);

                // article name
                string name = HtmlUtils.Clean(row["Name"].ToString().Trim(), false);

                if (Engage.Utility.HasValue(name))
                {
                    searchedContent.AppendFormat("{0}{1}", name, " ");
                }
                else
                {
                    // do we bother with the rest?
                    continue;
                }

                // article text
                string articleText = row["ArticleText"].ToString().Trim();
                if (Engage.Utility.HasValue(articleText))
                {
                    searchedContent.AppendFormat("{0}{1}", articleText, " ");
                }

                // article description
                string description = row["Description"].ToString().Trim();
                if (Engage.Utility.HasValue(description))
                {
                    searchedContent.AppendFormat("{0}{1}", description, " ");
                }

                // article metakeyword
                string keyword = row["MetaKeywords"].ToString().Trim();
                if (Engage.Utility.HasValue(keyword))
                {
                    searchedContent.AppendFormat("{0}{1}", keyword, " ");
                }

                // article metadescription
                string metaDescription = row["MetaDescription"].ToString().Trim();
                if (Engage.Utility.HasValue(metaDescription))
                {
                    searchedContent.AppendFormat("{0}{1}", metaDescription, " ");
                }

                // article metatitle
                string metaTitle = row["MetaTitle"].ToString().Trim();
                if (Engage.Utility.HasValue(metaTitle))
                {
                    searchedContent.AppendFormat("{0}{1}", metaTitle, " ");
                }

                string itemId = row["ItemId"].ToString();
                var item = new SearchItemInfo
                    {
                        Title = name, 
                        Description = HtmlUtils.Clean(description, false), 
                        Author = Convert.ToInt32(row["AuthorUserId"], CultureInfo.InvariantCulture), 
                        PubDate = Convert.ToDateTime(row["LastUpdated"], CultureInfo.InvariantCulture),
                        ModuleId = this.Module.ModuleID, 
                        SearchKey = "Article-" + itemId, 
                        Content = HtmlUtils.StripWhiteSpace(HtmlUtils.Clean(searchedContent.ToString(), false), true), 
                    };

                if (this.SetGuid)
                {
                    item.GUID = "itemid=" + itemId;
                }

                if (ModuleBase.AllowVenexusSearchForPortal(this.Module.PortalID))
                {
                    string indexUrl = UrlGenerator.GetItemLinkUrl(
                        Convert.ToInt32(itemId, CultureInfo.InvariantCulture),
                        Utility.GetPortalSettings(this.Module.PortalID),
                        this.Module.TabID,
                        this.Module.ModuleID);

                    // UpdateVenexusBraindump(IDbTransaction trans, string indexTitle, string indexContent, string indexWashedContent)
                    DataProvider.Instance().UpdateVenexusBraindump(
                        Convert.ToInt32(itemId, CultureInfo.InvariantCulture), 
                        name, 
                        articleText, 
                        HtmlUtils.Clean(articleText, false),
                        this.Module.PortalID, 
                        indexUrl);
                }

                yield return item;
            }
        }
    }
}