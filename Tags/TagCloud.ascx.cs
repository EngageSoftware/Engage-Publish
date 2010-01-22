//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.


namespace Engage.Dnn.Publish.Tags
{

    using System;
    using System.Collections;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;
    using Util;

    public partial class TagCloud : ModuleBase
    {
        private ArrayList _tagQuery;
        private string _qsTags = string.Empty;
        private int _popularTagsTotal;
        private int _mostPopularTagCount;
        private int _leastPopularTagCount;

        private bool UsePopularTags
        {
            get
            {
                object o = Settings["tcPopularTagBool"];
                return (o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture));
            }
        }

        override protected void OnInit(EventArgs e)
        {
            Load += Page_Load;
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
                lnkTagFilters.NavigateUrl = DotNetNuke.Common.Globals.NavigateURL(DefaultTagDisplayTabIdForPortal(PortalId));
                SetTagPageTitle();
                LoadTagList();
                //}
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void SetTagPageTitle()
        {
            if (AllowTitleUpdate)
            {
                var tp = (DotNetNuke.Framework.CDefault)Page;
                tp.Title += " " + _qsTags;
            }
        }

        private void LoadTagList()
        {
            if (_popularTagsTotal > 0)
            {
                //string tagCacheKey = Utility.CacheKeyPublishTag + PortalId.ToString(CultureInfo.InvariantCulture) + _qsTags + UsePopularTags.ToString(CultureInfo.InvariantCulture); // +"PageId";
                //DataTable dt = DataCache.GetCache(tagCacheKey) as DataTable ?? Tag.GetPopularTags(PortalId, _tagQuery, UsePopularTags);

                DataTable dt = Tag.GetPopularTags(PortalId, _tagQuery, UsePopularTags);

                if (dt != null && dt.Rows.Count > 0)
                {
                    //this doesn't work
                    //dt.DefaultView.Sort = "TotalItems DESC";
                    Utility.SortDataTableSingleParam(dt, "TotalItems desc");
                    //Get the most popular and lease popular tag totals
                    _mostPopularTagCount = Convert.ToInt32(dt.Rows[0]["TotalItems"].ToString());
                    _leastPopularTagCount = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["TotalItems"].ToString());
                    
                    //this doesn't work
                    //dt.DefaultView.Sort = "Name ASC";
                    
                    
                    Utility.SortDataTableSingleParam(dt, "name asc");

                    //DataCache.SetCache(tagCacheKey, dt, DateTime.Now.AddMinutes(CacheTime));
                    //Utility.AddCacheKey(tagCacheKey, PortalId);
                    phTagCloud.Controls.Clear();
                    string itemsWithTag = Localization.GetString("ItemsWithTag", LocalResourceFile);
                    foreach (DataRow dr in dt.DefaultView.Table.Rows)
                    {
                        var totalItems = (int)dr["TotalItems"];
                        var tagName = (string)dr["Name"];
                        var lnkTag = new Literal();
                        var sb = new StringBuilder(255);
                        sb.Append("<li class=\"");
                        sb.Append(GetTagSizeClass(totalItems));
                        sb.Append("\"><span>");
                        sb.Append(totalItems.ToString(CultureInfo.CurrentCulture));
                        sb.Append(itemsWithTag);
                        sb.Append("</span>");
                        sb.Append("<a href=\"");
                        sb.Append(BuildTagLink(tagName, true, string.Empty));
                        sb.Append("\" class=\"tag\">");
                        sb.Append(HttpUtility.HtmlEncode(tagName));
                        sb.Append("</a> </li>");
                        lnkTag.Text = sb.ToString();
                        phTagCloud.Controls.Add(lnkTag);
                    }
                }
                else
                {
                    //display a message about tags not found
                    phTagCloud.Controls.Add(new LiteralControl(Localization.GetString("NoTags.Text", LocalResourceFile)));
                }
            }
            else
            {
                //display a message about tags not found
                phTagCloud.Controls.Add(new LiteralControl(Localization.GetString("NoTags.Text", LocalResourceFile)));
            }
        }

        private string GetTagSizeClass(int itemCount)
        {
            int tagCountSpread = _mostPopularTagCount - _leastPopularTagCount;
            double result = Convert.ToDouble(itemCount) / Convert.ToDouble(tagCountSpread);

            string resultString;
            if (result <= .1666)
            {
                resultString = "size1";
            }
            else if (result <= .3333)
            {
                resultString = "size2";
            }
            else if (result <= .4999)
            {
                resultString = "size3";
            }
            else if (result <= .6666)
            {
                resultString = "size4";
            }
            else if (result <= .8333)
            {
                resultString = "size5";
            }
            else
            {
                resultString = "size6";
            }
            return resultString;
        }

        private string BuildTagLink(string name, bool useExisting, string useOthers)
        {
            object o = Request.QueryString["tags"];
            string existingTags;
            if (o != null && useExisting)
            {
                existingTags = o + "-";
            }
            else
            {
                existingTags = useOthers;
            }   
            return DotNetNuke.Common.Globals.NavigateURL(DefaultTagDisplayTabId, string.Empty, "tags=" + existingTags + HttpUtility.UrlEncode(name));
        }

        private void LoadTagInfo()
        {
            if (AllowTags)
            {
                string tags = Request.QueryString["Tags"];
                if (tags != null)
                {
                    _qsTags = tags;

                    char[] seperator = { '-' };
                    ArrayList tagList = Tag.ParseTags(_qsTags, PortalId, seperator, false);
                    _tagQuery = new ArrayList(tagList.Count);
                    string useOthers = string.Empty;

                    //create a list of tagids to query the database
                    foreach (Tag tg in tagList)
                    {
                        //Add the tag to the filtered list
                        _tagQuery.Add(tg.TagId);

                        //add the seperator in first
                        phTagFilters.Controls.Add(new LiteralControl(Localization.GetString("TagSeperator.Text", LocalResourceFile)));

                        var sb = new StringBuilder(255);
                        sb.Append("<li class=\"PublishFilterList");
                        sb.Append("\">");
                        sb.Append("<a href=\"");
                        sb.Append(BuildTagLink(tg.Name, false, useOthers));
                        sb.Append("\" class=\"tag\">");
                        sb.Append(HttpUtility.HtmlEncode(tg.Name));
                        sb.Append("</a> ");

                        phTagFilters.Controls.Add(new LiteralControl(sb.ToString()));

                        useOthers += tg.Name + "-";
                    }
                }
                _popularTagsTotal = Tag.GetPopularTagsCount(PortalId, _tagQuery, true);
            }
        }
    }
}

