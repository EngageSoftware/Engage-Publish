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
    using System.Collections;
    using System.Text;
    using System.Web;
    using System.Web.SessionState;
    using DotNetNuke.Services.Localization;

    /// <summary>
	/// Summary description for Breadcrumb.
	/// </summary>
	public static class Breadcrumb
	{
		private const int MaxBreadCrumbs = 4;

		public static void Add(string pageName, string pageUrl)
		{
			ArrayList q = GetBreadCrumbs();
			var bci = new BreadcrumbItem {PageName = pageName, PageUrl = pageUrl};

		    q.Remove(bci);
			q.Add(bci);

			if (q.Count > MaxBreadCrumbs) q.RemoveAt(0);

			//if (q.Contains(bci) == false) q.Enqueue(bci);
			//if (q.Count >= MaxBreadCrumbs) q.Dequeue();	
		}

		private static ArrayList GetBreadCrumbs()
		{
            //TODO: Deal with potential for HttpContext.Current to be null
			HttpSessionState session = HttpContext.Current.Session;
			var q = (ArrayList) session["breadCrumbs"];
			if (q == null)
			{
				q = new ArrayList(MaxBreadCrumbs);
				session.Add("breadCrumbs", q);
			}

			return q;
		}

		public new static string ToString()
		{
			ArrayList q = GetBreadCrumbs();
			var sb = new StringBuilder(q.Count * 100);
        
            //TODO: breadcrumb text is hardcoded!


			//sb.Append("Recent&nbsp;Pages&nbsp;");
            sb.Append(Localization.GetString("RecentPages", Utility.DesktopModuleFolderName + "util/app_localresources/Breadcrumb.cs.resx"));
			
			IEnumerator ie = q.GetEnumerator();
			while (ie.MoveNext())
			{
				var bci = (BreadcrumbItem) ie.Current;
				sb.Append("&gt;&gt; <a href=\"");
				sb.Append(bci.PageUrl);
				sb.Append("\" class=\"RecentPagesLink\">");
                sb.Append(bci.PageName);
				sb.Append("</a> ");
			}

			return sb.ToString();
		}
	}

    [Serializable]
	public class BreadcrumbItem
	{
		private string _pageName;
		private string _pageUrl;

		public string PageName
		{
			get {return (_pageName ?? string.Empty);}
			set {_pageName = value;}
		}

		public string PageUrl
		{
			get {return (_pageUrl ?? string.Empty);}
			set {_pageUrl = value;}
		}

		public override bool Equals(object obj)
		{
			var bci = obj as BreadcrumbItem;
			if (bci == null)
			{
				return false;
			}
		    return (bci._pageName == _pageName && bci._pageUrl == _pageUrl);
		}

		public override int GetHashCode()
		{
			return (_pageName.GetHashCode() + _pageUrl.GetHashCode());
		}
	}
}

