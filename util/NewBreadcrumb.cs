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
    using System.Collections;
    /// <summary>
	/// Summary description for Breadcrumb.
	/// </summary>
    public class BreadcrumbCollection : CollectionBase
	{
		public void InsertBeginning(string pageName, string pageUrl)
		{            
			var bci = new BreadcrumbItem {PageName = pageName, PageUrl = pageUrl};
		    InnerList.Insert(0,bci);
		}
        public void Add(string pageName, string pageUrl)
        {
            var bci = new BreadcrumbItem {PageName = pageName, PageUrl = pageUrl};
            InnerList.Add(bci);
        }

        public BreadcrumbItem this[int index]
        {
            get { return (BreadcrumbItem)InnerList[index]; }
        }

	}
}

