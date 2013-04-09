// <copyright file="NullSecurityFilter.cs" company="Engage Software">
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
    using System.Data;
    using System.Diagnostics;

    using DotNetNuke.Services.Search;

    /// <summary>
    /// Summary description for NullSecurityFilter.
    /// </summary>
    internal sealed class NullSecurityFilter : SecurityFilter
    {
        private static readonly SecurityFilter instance = new NullSecurityFilter();

        private NullSecurityFilter()
        {
        }

        public new static SecurityFilter Instance
        {
            get { return instance; }
        }

        public override void FilterArticles(SearchResultsInfoCollection data)
        {
            Debug.Assert(data != null, "data must not be null");

            // does nothing
        }

        public override void FilterCategories(DataTable data)
        {
            Debug.Assert(data != null, "data must not be null");

            // does nothing
        }
    }
}