//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Publish.Util;
using System.Globalization;

namespace Engage.Dnn.Publish.Tags
{
	public partial class TagCloud :  ModuleBase
	{
        private ArrayList tagQuery;
        private string qsTags = string.Empty;

		#region Event Handlers
		override protected void OnInit(EventArgs e)
		{
		    this.Load += this.Page_Load;
		    base.OnInit(e);
            LoadTagInfo();
		}

	    private void Page_Load(object sender, EventArgs e)
		{
			try 
			{
				//check VI for null then set information
				//if (!Page.IsPostBack)
				//{
                    SetTagPageTitle();
                    LoadTagList();
				//}
			} 
			catch (Exception exc) 
			{
				Exceptions.ProcessModuleLoadException(this, exc);
			}
		}
		#endregion

        private void SetTagPageTitle()
        {
            if (AllowTitleUpdate)
            {
                DotNetNuke.Framework.CDefault tp = (DotNetNuke.Framework.CDefault)this.Page;
                tp.Title += " " + qsTags.ToString();
            }
        }

        private void LoadTagList()
        {
            if (popularTagsTotal > 0)
            {
                string tagCacheKey = Utility.CacheKeyPublishTag + PortalId.ToString(CultureInfo.InvariantCulture) + qsTags + popularTagCount.ToString(CultureInfo.InvariantCulture); // +"PageId";
                DataTable dt = DataCache.GetCache(tagCacheKey) as DataTable ?? Tag.GetPopularTags(PortalId, tagQuery, popularTagCount);

                if (dt != null && dt.Rows.Count > 0)
                {
                    Utility.SortDataTableSingleParam(dt, "TotalItems desc");
                    //Get the most popular and lease popular tag totals
                    mostPopularTagCount = Convert.ToInt32(dt.Rows[0]["TotalItems"].ToString());
                    leastPopularTagCount = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["TotalItems"].ToString());

                    Utility.SortDataTableSingleParam(dt, "name asc");

                    DataCache.SetCache(tagCacheKey, dt, DateTime.Now.AddMinutes(CacheTime));
                    Utility.AddCacheKey(tagCacheKey, PortalId);
                    phTagCloud.Controls.Clear();
                    string itemsWithTag = Localization.GetString("ItemsWithTag", LocalResourceFile);
                    foreach (DataRow dr in dt.DefaultView.Table.Rows)
                    {
                        int totalItems = (int)dr["TotalItems"];
                        string tagName = (string)dr["Name"];
                        Literal lnkTag = new Literal();
                        StringBuilder sb = new StringBuilder(255);
                        sb.Append("<li class=\"");
                        sb.Append(tagSizeClass(totalItems));
                        sb.Append("\"><span>");
                        sb.Append(totalItems.ToString(CultureInfo.CurrentCulture));
                        sb.Append(itemsWithTag);
                        sb.Append("</span>");
                        sb.Append("<a href=\"");
                        sb.Append(BuildTagLink(tagName));
                        sb.Append("\" class=\"tag\">");
                        sb.Append(tagName);
                        sb.Append("</a> ");
                        lnkTag.Text = sb.ToString();
                        phTagCloud.Controls.Add(lnkTag);
                    }
                }
                else
                {
                    //ToDo: display a message about tags not found
                }
            }
        }

        private string tagSizeClass(int itemCount)
        {
            //mostPopularTagCount
            //leastPopularTagCount

            int tagCountSpread = mostPopularTagCount - leastPopularTagCount;
            

            double result = Convert.ToDouble(itemCount) / Convert.ToDouble(tagCountSpread);
            string resultString = "size3";
            if ((0 <= result) &&  (result <=1666))
            {
                resultString = "size1";     
            }
            if ((.1667 < result) && (result <= .3333))
            {
                resultString = "size2";
            }

            if ((.3334 < result) && (result <= .4999))
            {
                resultString = "size3";
            }

            if ((.5 < result) && (result <= .6666))
            {
                resultString = "size4";
            }

            if ((.6667 < result) && (result <= .8333))
            {
                resultString = "size5";
            }

            if ((.8334 < result))
            {
                resultString = "size6";
            }

            return resultString;
        }

        private bool popularTagCount
        {
            get
            {
                object o = Settings["tcPopularTagBool"];
                return (o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }

        private int popularTagsTotal;
        private int mostPopularTagCount;
        private int leastPopularTagCount;


        private string BuildTagLink(string name)
        {
            object o = Request.QueryString["tags"];
            string existingTags = string.Empty;
            if (o != null)
            {
                existingTags = o.ToString() + "-";
            }
            
            return DotNetNuke.Common.Globals.NavigateURL(DefaultTagDisplayTabId, "", "&tags=" + existingTags + name);
            //return DotNetNuke.Common.Globals.NavigateURL(TabId, "", "&tags=" + existingTags + name);
        }


        private void LoadTagInfo()
        {
            if (AllowTags)
            {
                object t = Request.QueryString["Tags"];
                if (t != null)
                {
                    DotNetNuke.Security.PortalSecurity objSecurity = new DotNetNuke.Security.PortalSecurity();

                    qsTags = objSecurity.InputFilter(HttpUtility.UrlDecode(t.ToString()), DotNetNuke.Security.PortalSecurity.FilterFlag.NoSQL);
                    //qsTags = HttpUtility.UrlDecode(t.ToString());

                    char[] seperator = { '-' };
                    tagQuery = new ArrayList(Tag.ParseTags(qsTags, PortalId, seperator, false).Count);
                    foreach (Tag tg in Tag.ParseTags(qsTags, PortalId, seperator, false))
                    {
                        //create a list of tagids to query the database
                        tagQuery.Add(tg.TagId);
                    }
                }
                popularTagsTotal = Tag.GetPopularTagsCount(PortalId, tagQuery, true);

                //if (popularTagsTotal == null) popularTagsTotal = 0;
            }

        }



	}
}

