//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.


namespace Engage.Dnn.Publish.Search
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Search;
    using Util;

    /// <summary>
	/// Summary description for SearchProvider.
	/// </summary>
	public class SearchProvider// : ISearchable
	{
		#region ISearchable Members

		public SearchItemInfoCollection GetSearchItems(ModuleInfo modInfo)
		{
			var items = new SearchItemInfoCollection();
			AddArticleSearchItems(items, modInfo);
			return items;
		}

		#endregion

        private static void AddArticleSearchItems(SearchItemInfoCollection items, ModuleInfo modInfo)
        {
            //get all the updated items
            //DataTable dt = Article.GetArticlesSearchIndexingUpdated(modInfo.PortalID, modInfo.ModuleDefID, modInfo.TabID);

            //TODO: we should get articles by ModuleID and only perform indexing by ModuleID 
            DataTable dt = Article.GetArticles(modInfo.PortalID);
            SearchArticleIndex(dt, items, modInfo);

       }

        private static void SearchArticleIndex(DataTable dt, SearchItemInfoCollection items, ModuleInfo modInfo)
        {
            for (int i = 0; i < dt.Rows.Count; i++) 
            {
                DataRow row = dt.Rows[i];
                    var searchedContent = new StringBuilder(8192);
                    //article name
                    string name = HtmlUtils.Clean(row["Name"].ToString().Trim(), false);

                    if (Utility.HasValue(name))
                    {
                        searchedContent.AppendFormat("{0}{1}", name, " ");
                    }
                    else
                    {
                        //do we bother with the rest?
                        continue;
                    }

                    //article text
                    string articleText = row["ArticleText"].ToString().Trim();
                    if (Utility.HasValue(articleText))
                    {
                        searchedContent.AppendFormat("{0}{1}", articleText, " ");
                    }

                    //article description
                    string description = row["Description"].ToString().Trim();
                    if (Utility.HasValue(description))
                    {
                        searchedContent.AppendFormat("{0}{1}", description, " ");
                    }

                    //article metakeyword
                    string keyword = row["MetaKeywords"].ToString().Trim();
                    if (Utility.HasValue(keyword))
                    {
                        searchedContent.AppendFormat("{0}{1}", keyword, " ");
                    }

                    //article metadescription
                    string metaDescription = row["MetaDescription"].ToString().Trim();
                    if (Utility.HasValue(metaDescription))
                    {
                        searchedContent.AppendFormat("{0}{1}", metaDescription, " ");
                    }

                    //article metatitle
                    string metaTitle = row["MetaTitle"].ToString().Trim();
                    if (Utility.HasValue(metaTitle))
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
                                           ModuleId = modInfo.ModuleID,
                                           SearchKey = "Article-" + itemId,
                                           Content =
                                                   HtmlUtils.StripWhiteSpace(
                                                   HtmlUtils.Clean(searchedContent.ToString(), false), true),
                                           GUID = "itemid=" + itemId
                                   };

                items.Add(item);

                    //Check if the Portal is setup to enable venexus indexing
                    if (ModuleBase.AllowVenexusSearchForPortal(modInfo.PortalID))
                    {
                        string indexUrl = Utility.GetItemLinkUrl(Convert.ToInt32(itemId, CultureInfo.InvariantCulture), modInfo.PortalID, modInfo.TabID, modInfo.ModuleID);

                        //UpdateVenexusBraindump(IDbTransaction trans, string indexTitle, string indexContent, string indexWashedContent)
                        Data.DataProvider.Instance().UpdateVenexusBraindump(Convert.ToInt32(itemId, CultureInfo.InvariantCulture), name, articleText, HtmlUtils.Clean(articleText, false), modInfo.PortalID, indexUrl);
                    }
                //}
            } 
        }
    }
}