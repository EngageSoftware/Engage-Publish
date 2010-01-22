//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.



namespace Engage.Dnn.Publish.Util
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Xml.XPath;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Search;
    using Portability;

    /// <summary>
    /// Features Controller Class supports IPortable currently.
    /// </summary>
    public class FeaturesController : IPortable, ISearchable
    {
        #region IPortable Members

        /// <summary>
        /// Method is invoked when portal template is imported or user selects Import content from menu.
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="content"></param>
        /// <param name="version"></param>
        /// <param name="userId"></param>
        public void ImportModule(int moduleId, string content, string version, int userId)
        {

            var validator = new TransportableXmlValidator();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            if (validator.Validate(stream) == false)
            {
                var invalidXml = new Exception("Unable to import publish content due to incompatible XML file. Error: " + validator.Errors[0]);
                Exceptions.LogException(invalidXml);
                throw invalidXml;
            }

            //The DNN ValidatorBase closes the stream? Must re-create. hk
            stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var doc = new XPathDocument(stream);
            var builder = new XmlTransporter(moduleId);

            try
            {
                XmlDirector.Deconstruct(builder, doc);
            }
            catch (Exception e)
            {
                Exceptions.LogException(new Exception(e.ToString()));
                throw;
            }
        }


        /// <summary>
        /// Method is invoked when portal template is created or user selects Export Content from menu.
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public string ExportModule(int moduleId)
        {
            bool exportAll = false;

            //check query string for a "All" param to signal all rows, not just for a moduleId
            if (HttpContext.Current != null && HttpContext.Current.Request.QueryString["all"] != null)
            {
                exportAll = true;
            }
            XmlTransporter builder;
            try
            {
                builder = new XmlTransporter(moduleId);
                XmlDirector.Construct(builder, exportAll);
            }
            catch (Exception e)
            {
                Exceptions.LogException(new Exception(e.ToString()));
                throw;
            }

            return builder.Document.OuterXml;
        }

        #endregion

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
            DataTable dt = Article.GetArticlesByModuleId(modInfo.ModuleID, true);
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
