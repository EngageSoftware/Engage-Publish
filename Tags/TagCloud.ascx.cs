// <copyright file="TagCloud.ascx.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

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

    using DotNetNuke.Common;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    public partial class TagCloud : ModuleBase
    {
        private int _leastPopularTagCount;

        private int _mostPopularTagCount;

        private int _popularTagsTotal;

        private string _qsTags = string.Empty;

        private ArrayList _tagQuery;

        private bool UsePopularTags
        {
            get
            {
                object o = this.Settings["tcPopularTagBool"];
                return o == null ? true : Convert.ToBoolean(o, CultureInfo.InvariantCulture);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);
            this.LoadTagInfo();
        }

        private string BuildTagLink(string name, bool useExisting, string useOthers)
        {
            object o = this.Request.QueryString["tags"];
            string existingTags;
            if (o != null && useExisting)
            {
                existingTags = o + "-";
            }
            else
            {
                existingTags = useOthers;
            }

            return Globals.NavigateURL(this.DefaultTagDisplayTabId, string.Empty, "tags=" + existingTags + HttpUtility.UrlEncode(name));
        }

        private string GetTagSizeClass(int itemCount)
        {
            int tagCountSpread = this._mostPopularTagCount - this._leastPopularTagCount;
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

        private void LoadTagInfo()
        {
            if (this.AllowTags)
            {
                string tags = this.Request.QueryString["Tags"];
                if (tags != null)
                {
                    this._qsTags = tags;

                    char[] seperator = {
                                           '-'
                                       };
                    ArrayList tagList = Tag.ParseTags(this._qsTags, this.PortalId, seperator, false);
                    this._tagQuery = new ArrayList(tagList.Count);
                    string useOthers = string.Empty;

                    // create a list of tagids to query the database
                    foreach (Tag tg in tagList)
                    {
                        // Add the tag to the filtered list
                        this._tagQuery.Add(tg.TagId);

                        // add the seperator in first
                        this.phTagFilters.Controls.Add(new LiteralControl(Localization.GetString("TagSeperator.Text", this.LocalResourceFile)));

                        var sb = new StringBuilder(255);
                        sb.Append("<li class=\"PublishFilterList");
                        sb.Append("\">");
                        sb.Append("<a href=\"");
                        sb.Append(this.BuildTagLink(tg.Name, false, useOthers));
                        sb.Append("\" class=\"tag\">");
                        sb.Append(HttpUtility.HtmlEncode(tg.Name));
                        sb.Append("</a> ");

                        this.phTagFilters.Controls.Add(new LiteralControl(sb.ToString()));

                        useOthers += tg.Name + "-";
                    }
                }

                this._popularTagsTotal = Tag.GetPopularTagsCount(this.PortalId, this._tagQuery, true);
            }
        }

        private void LoadTagList()
        {
            if (this._popularTagsTotal > 0)
            {
                // string tagCacheKey = Utility.CacheKeyPublishTag + PortalId.ToString(CultureInfo.InvariantCulture) + _qsTags + UsePopularTags.ToString(CultureInfo.InvariantCulture); // +"PageId";
                // DataTable dt = DataCache.GetCache(tagCacheKey) as DataTable ?? Tag.GetPopularTags(PortalId, _tagQuery, UsePopularTags);
                DataTable dt = Tag.GetPopularTags(this.PortalId, this._tagQuery, this.UsePopularTags);

                if (dt != null && dt.Rows.Count > 0)
                {
                    // this doesn't work
                    // dt.DefaultView.Sort = "TotalItems DESC";
                    Utility.SortDataTableSingleParam(dt, "TotalItems desc");

                    // Get the most popular and lease popular tag totals
                    this._mostPopularTagCount = Convert.ToInt32(dt.Rows[0]["TotalItems"].ToString());
                    this._leastPopularTagCount = Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["TotalItems"].ToString());

                    // this doesn't work
                    // dt.DefaultView.Sort = "Name ASC";
                    Utility.SortDataTableSingleParam(dt, "name asc");

                    // DataCache.SetCache(tagCacheKey, dt, DateTime.Now.AddMinutes(CacheTime));
                    // Utility.AddCacheKey(tagCacheKey, PortalId);
                    this.phTagCloud.Controls.Clear();
                    string itemsWithTag = Localization.GetString("ItemsWithTag", this.LocalResourceFile);
                    foreach (DataRow dr in dt.DefaultView.Table.Rows)
                    {
                        var totalItems = (int)dr["TotalItems"];
                        var tagName = (string)dr["Name"];
                        var lnkTag = new Literal();
                        var sb = new StringBuilder(255);
                        sb.Append("<li class=\"");
                        sb.Append(this.GetTagSizeClass(totalItems));
                        sb.Append("\"><span>");
                        sb.Append(totalItems.ToString(CultureInfo.CurrentCulture));
                        sb.Append(itemsWithTag);
                        sb.Append("</span>");
                        sb.Append("<a href=\"");
                        sb.Append(this.BuildTagLink(tagName, true, string.Empty));
                        sb.Append("\" class=\"tag\">");
                        sb.Append(HttpUtility.HtmlEncode(tagName));
                        sb.Append("</a> </li>");
                        lnkTag.Text = sb.ToString();
                        this.phTagCloud.Controls.Add(lnkTag);
                    }
                }
                else
                {
                    // display a message about tags not found
                    this.phTagCloud.Controls.Add(new LiteralControl(Localization.GetString("NoTags.Text", this.LocalResourceFile)));
                }
            }
            else
            {
                // display a message about tags not found
                this.phTagCloud.Controls.Add(new LiteralControl(Localization.GetString("NoTags.Text", this.LocalResourceFile)));
            }
        }

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // check VI for null then set information
                // if (!Page.IsPostBack)
                // {
                this.lnkTagFilters.NavigateUrl = Globals.NavigateURL(DefaultTagDisplayTabIdForPortal(this.PortalId));
                this.SetTagPageTitle();
                this.LoadTagList();

                // }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void SetTagPageTitle()
        {
            if (this.AllowTitleUpdate)
            {
                var tp = (CDefault)this.Page;
                tp.Title += " " + this._qsTags;
            }
        }
    }
}