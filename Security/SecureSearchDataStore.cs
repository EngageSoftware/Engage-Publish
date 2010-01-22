//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.




namespace Engage.Dnn.Publish.Security
{
    using System.Collections;
    using DotNetNuke.Services.Search;
    using Util;

	/// <summary>
	/// Summary description for SecureSearchDataStore.
	/// </summary>
    public class SecureSearchDataStore : SearchDataStore
	{
		public override SearchResultsInfoCollection GetSearchResults(int portalId, string criteria)
		{
			SearchResultsInfoCollection results = base.GetSearchResults(portalId, criteria);

			SecurityFilter sf = SecurityFilter.Instance;
			sf.FilterArticles(results);
			CleanSearchList(results);
			return results;
		}

		public override SearchResultsInfoCollection GetSearchItems(int portalId, int tabId, int moduleId)
		{
			//TODO: need to filter this also?
			//how is this invoked? do we need to filter?
            SearchResultsInfoCollection results = base.GetSearchItems(portalId, tabId, moduleId);
			//SecurityFilter sf = SecurityFilter.Instance;
			//return sf.FilterArticles(results);

			return results;
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Code Analysis doesn't see validation")]
        public static SearchResultsInfoCollection CleanSearchList(SearchResultsInfoCollection data)
		{
			//get rid of the duplicates
			//some conditions that we don't need to bother
			if (data == null || data.Count == 0) return data;

			IDictionary d = new Hashtable();

			var al = new ArrayList();
			foreach (SearchResultsInfo result in data)
			{
                //IDictionary listOfIds = GetSearchArticleIds(data);
				int articleId = Utility.GetArticleId(result);
				
				if (d.Contains(articleId))
				{
					//remove this row from the results
					al.Add(result);
				}
				else
				{
					d.Add(articleId, null);
				}
			}

			//remove all the rows that I'm not allowed to see
			foreach (SearchResultsInfo result in al)
			{
				data.Remove(result);
			}
			return data;
		}

        //private static IDictionary GetSearchArticleIds(SearchResultsInfoCollection data)
        //{
        //    IDictionary d = new Hashtable();
        //    foreach (SearchResultsInfo result in data)
        //    {
        //        //int articleId = Utility.GetArticleId(result);
        //        d.Add(result, null);
        //    }
        //    return d;
        //}
	}
}

