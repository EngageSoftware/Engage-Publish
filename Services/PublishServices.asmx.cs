// <copyright file="PublishServices.asmx.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Services;
    using System.Web.UI;

    using DotNetNuke.Security;
    using DotNetNuke.Services.Exceptions;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Util;

    /// <summary>
    /// A web service that provides access to the Engage: Publish module on this site
    /// </summary>
    [WebService(Namespace = "http://www.engagesoftware.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class PublishServices : WebService
    {
        /// <summary>
        /// Gets a list of article names and IDs for the category with the given ID.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns>A list of article names and IDs for the given category</returns>
        [WebMethod]
        [ScriptMethod]
        public Pair[] GetArticlesByCategory(int categoryId)
        {
            try
            {
                IEnumerable<Pair> articles;
                var category = Publish.Category.GetCategory(categoryId);
                if (category == null)
                {
                    articles = Enumerable.Empty<Pair>();
                }
                else
                {
                    var articlesTable =
                        DataProvider.Instance().GetAdminItemListing(
                            category.ItemId,
                            ItemType.Article.GetId(),
                            RelationshipType.ItemToParentCategory.GetId(),
                            RelationshipType.ItemToRelatedCategory.GetId(),
                            ApprovalStatus.Approved.GetId(),
                            category.PortalId).Tables[0];

                    articles = from DataRow articleRow in articlesTable.Rows
                               select new Pair(articleRow["name"].ToString(), (int)articleRow["itemId"]);
                }

                return articles.ToArray();
            }
            catch (Exception exc)
            {
                Exceptions.LogException(exc);
                throw;
            }
        }

        /// <summary>
        /// Gets a list of <paramref name="count"/> tags that start with <paramref name="prefixText"/> in the given portal.
        /// </summary>
        /// <param name="prefixText">The text to search on.</param>
        /// <param name="count">The number of tags to retrieve.</param>
        /// <param name="contextKey">The portal ID</param>
        /// <returns>A list of the names of all tags that this search returns</returns>
        [WebMethod]
        [ScriptMethod]
        public string[] GetTagsCompletionList(string prefixText, int count, string contextKey)
        {
            try
            {
                var objSecurity = new PortalSecurity();

                DataTable dt = Tag.GetTagsByString(
                    objSecurity.InputFilter(HttpUtility.UrlDecode(prefixText), PortalSecurity.FilterFlag.NoSQL), 
                    Convert.ToInt32(contextKey, CultureInfo.InvariantCulture));

                var returnTags = new string[dt.Rows.Count];
                foreach (DataRow dr in dt.Rows)
                {
                    returnTags[0] = dr["name"].ToString();
                }

                return returnTags;
            }
            catch (Exception exc)
            {
                Exceptions.LogException(exc);
                throw;
            }
        }
    }
}