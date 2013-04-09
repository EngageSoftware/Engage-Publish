// <copyright file="RoleBasedSecurityFilter.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Security
{
    using System.Collections;
    using System.Data;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using DotNetNuke.Services.Search;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Util;

    /// <summary>
    /// Summary description for RoleBasedSecurityFilter.
    /// </summary>
    internal class RoleBasedSecurityFilter : SecurityFilter
    {
        private static readonly SecurityFilter instance = new RoleBasedSecurityFilter();

        private RoleBasedSecurityFilter()
        {
        }

        public new static SecurityFilter Instance
        {
            get { return instance; }
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Using assert")]
        public override void FilterArticles(SearchResultsInfoCollection data)
        {
            Debug.Assert(data != null, "data must not be null");

            // some conditions that we don't need to bother
            if (data.Count == 0)
            {
                return;
            }

            IDictionary viewableIds = DataProvider.Instance().GetViewableArticleIds(PermissionType.View.GetId());

            var al = new ArrayList();
            foreach (SearchResultsInfo result in data)
            {
                int articleId = Utility.GetArticleId(result);
                if (viewableIds.Contains(articleId) == false)
                {
                    // remove this row from the results
                    al.Add(result);
                }
            }

            // remove all the rows that I'm not allowed to see
            foreach (SearchResultsInfo result in al)
            {
                data.Remove(result);
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Using Asset")]
        public override void FilterCategories(DataTable data)
        {
            Debug.Assert(data != null, "data must not be null");

            // some conditions that we don't need to bother
            if (data.Rows.Count == 0)
            {
                return;
            }

            IDictionary d = DataProvider.Instance().GetViewableCategoryIds(PermissionType.View.GetId());

            var al = new ArrayList();
            foreach (DataRow r in data.Rows)
            {
                if (d.Contains(r["ItemId"]) == false)
                {
                    // remove this row from the DataTable
                    al.Add(r);
                }
            }

            // remove all the rows where the names don't match
            foreach (DataRow r in al)
            {
                data.Rows.Remove(r);
            }
        }
    }
}