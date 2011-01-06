//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Xml;

    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Framework;
    using DotNetNuke.Services.FileSystem;
    using DotNetNuke.UI.Utilities;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Util;

    using FileInfo = DotNetNuke.Services.FileSystem.FileInfo;

    public partial class EpRss : PageBase
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _itemType;

        private string _qsTags;

        private ArrayList _tagQuery;

        public static string ApplicationUrl
        {
            get { return HttpContext.Current.Request.ApplicationPath == "/" ? string.Empty : HttpContext.Current.Request.ApplicationPath; }
        }

        public bool AllowTags
        {
            get { return ModuleBase.AllowTagsForPortal(this.PortalId); }
        }

        public string DisplayType
        {
            get
            {
                string i = this.Request.Params["DisplayType"];
                if (!string.IsNullOrEmpty(i))
                {
                    return i;
                }

                return null;
            }
        }

        public int ItemId
        {
            get
            {
                string i = this.Request.Params["itemId"];
                if (i != null)
                {
                    // look up the _itemType if ItemId passed in.
                    this.ItemType = Item.GetItemType(Convert.ToInt32(i, CultureInfo.InvariantCulture), this.PortalId).ToUpperInvariant();
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }

                return -1;
            }
        }

        public string ItemType
        {
            [DebuggerStepThrough]
            get { return this._itemType; }
            [DebuggerStepThrough]
            set { this._itemType = value; }
        }

        public int ItemTypeId
        {
            get
            {
                string i = this.Request.Params["itemTypeId"];
                if (i != null)
                {
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }

                return -1;
            }
        }

        public int NumberOfItems
        {
            get
            {
                string i = this.Request.Params["numberOfItems"];
                if (i != null)
                {
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }

                return -1;
            }
        }

        public int PortalId
        {
            get
            {
                string i = this.Request.Params["portalId"];
                if (i != null)
                {
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }

                return -1;
            }
        }

        public int RelationshipTypeId
        {
            get
            {
                string i = this.Request.Params["RelationshipTypeId"];
                if (!string.IsNullOrEmpty(i))
                {
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }

                return -1;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.Load += this.Page_Load;
            base.OnInit(e);

            // TODO: refactor this between here, CustomDisplay.OnInit, TagCloud.LoadTagInfo
            // read the tags querystring parameter and see if we have any to store into the _tagQuery arraylist
            if (this.AllowTags)
            {
                string tags = this.Request.QueryString["Tags"];
                if (tags != null)
                {
                    this._qsTags = HttpUtility.UrlDecode(tags);
                    char[] seperator = {
                                           '-'
                                       };
                    ArrayList tagList = Tag.ParseTags(this._qsTags, this.PortalId, seperator, false);
                    this._tagQuery = new ArrayList(tagList.Count);
                    foreach (Tag tg in tagList)
                    {
                        // create a list of tagids to query the database
                        this._tagQuery.Add(tg.TagId);
                    }
                }
            }
        }

        private void Page_Load(object sender, EventArgs e)
        {
            // PortalSettings ps = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            PortalSettings ps = Utility.GetPortalSettings(this.PortalId);

            this.Response.ContentType = "text/xml";
            this.Response.ContentEncoding = Encoding.UTF8;

            var sw = new StringWriter(CultureInfo.InvariantCulture);
            var wr = new XmlTextWriter(sw);

            wr.WriteStartElement("rss");
            wr.WriteAttributeString("version", "2.0");
            wr.WriteAttributeString("xmlns:wfw", "http://wellformedweb.org/CommentAPI/");
            wr.WriteAttributeString("xmlns:slash", "http://purl.org/rss/1.0/modules/slash/");
            wr.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
            wr.WriteAttributeString("xmlns:trackback", "http://madskills.com/public/xml/rss/module/trackback/");

            wr.WriteStartElement("channel");
            wr.WriteElementString("title", ps.PortalName);
            if (ps.PortalAlias.HTTPAlias.IndexOf("http://", StringComparison.OrdinalIgnoreCase) == -1)
            {
                wr.WriteElementString("link", "http://" + ps.PortalAlias.HTTPAlias);
            }
            else
            {
                wr.WriteElementString("link", ps.PortalAlias.HTTPAlias);
            }

            wr.WriteElementString("description", "RSS Feed for " + ps.PortalName);
            wr.WriteElementString("ttl", "120");

            // TODO: look into options for how to display the "Title" of the RSS feed
            var dt = new DataTable
                {
                    Locale = CultureInfo.InvariantCulture
                };
            if (this.DisplayType == "ItemListing" || this.DisplayType == null)
            {
                dt = this.ItemId == -1
                         ? DataProvider.Instance().GetMostRecent(this.ItemTypeId, this.NumberOfItems, this.PortalId)
                         : DataProvider.Instance().GetMostRecentByCategoryId(this.ItemId, this.ItemTypeId, this.NumberOfItems, this.PortalId);
            }
            else if (this.DisplayType == "CategoryFeature")
            {
                DataSet ds = DataProvider.Instance().GetParentItems(this.ItemId, this.PortalId, this.RelationshipTypeId);
                dt = ds.Tables[0];
            }
            else if (this.DisplayType == "TagFeed")
            {
                if (this.AllowTags && this._tagQuery != null && this._tagQuery.Count > 0)
                {
                    string tagCacheKey = Utility.CacheKeyPublishTag + this.PortalId + this.ItemTypeId.ToString(CultureInfo.InvariantCulture) +
                                         this._qsTags;

                    // +"PageId";
                    dt = DataCache.GetCache(tagCacheKey) as DataTable;
                    if (dt == null)
                    {
                        // ToDo: we need to make getitemsfromtags use the numberofitems value
                        dt = Tag.GetItemsFromTags(this.PortalId, this._tagQuery);

                        // TODO: we should sort the tags 
                        // TODO: should we set a 5 minute cache on RSS? 
                        DataCache.SetCache(tagCacheKey, dt, DateTime.Now.AddMinutes(5));
                        Utility.AddCacheKey(tagCacheKey, this.PortalId);
                    }
                }
            }

            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                if (dv.Table.Columns.IndexOf("dateColumn") > 0)
                {
                    dv.Table.Columns.Add("dateColumn", typeof(DateTime));
                    foreach (DataRowView dr in dv)
                    {
                        dr["dateColumn"] = Convert.ToDateTime(dr["startdate"]);
                    }

                    dv.Sort = " dateColumn desc ";
                }

                for (int i = 0; i < dv.Count; i++)
                {
                    // DataRow r = dt.Rows[i];
                    DataRow r = dv[i].Row;
                    wr.WriteStartElement("item");

                    // 				wr.WriteElementString("slash:comments", objArticle.CommentCount.ToString());
                    // wr.WriteElementString("wfw:commentRss", "http://" & Request.Url.Host & Me.ResolveUrl("RssComments.aspx?TabID=" & m_tabID & "&ModuleID=" & m_moduleID & "&ArticleID=" & objArticle.ArticleID.ToString()).Replace(" ", "%20"));
                    // wr.WriteElementString("trackback:ping", "http://" & Request.Url.Host & Me.ResolveUrl("Tracking/Trackback.aspx?ArticleID=" & objArticle.ArticleID.ToString() & "&PortalID=" & _portalSettings.PortalId.ToString() & "&TabID=" & _portalSettings.ActiveTab.TabID.ToString()).Replace(" ", "%20"));
                    string title = String.Empty, 
                           description = String.Empty, 
                           childItemId = String.Empty, 
                           thumbnail = String.Empty, 
                           guid = String.Empty, 
                           author = string.Empty;

                    DateTime startDate = DateTime.MinValue;

                    if (this.DisplayType == null || string.Equals(this.DisplayType, "ItemListing", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(this.DisplayType, "TagFeed", StringComparison.OrdinalIgnoreCase))
                    {
                        title = r["ChildName"].ToString();
                        description = r["ChildDescription"].ToString();
                        childItemId = r["ChilditemId"].ToString();
                        guid = r["itemVersionIdentifier"].ToString();
                        startDate = (DateTime)r["StartDate"];
                        thumbnail = r["Thumbnail"].ToString();
                        author = r["Author"].ToString();

                        // UserController uc = new UserController();
                        // UserInfo ui = uc.GetUser(PortalId, Convert.ToInt32(r["AuthorUserId"].ToString()));
                        // if(ui!=null)
                        // author = ui.DisplayName;
                    }
                    else if (string.Equals(this.DisplayType, "CategoryFeature", StringComparison.OrdinalIgnoreCase))
                    {
                        title = r["Name"].ToString();
                        description = r["Description"].ToString();
                        childItemId = r["itemId"].ToString();
                        guid = r["itemVersionIdentifier"].ToString();
                        startDate = (DateTime)r["StartDate"];
                        thumbnail = r["Thumbnail"].ToString();
                        author = r["Author"].ToString();

                        // UserController uc = new UserController();
                        // UserInfo ui = uc.GetUser(PortalId, Convert.ToInt32(r["AuthorUserId"].ToString()));
                        // if (ui != null)
                        // author = ui.DisplayName;
                    }

                    if (!Uri.IsWellFormedUriString(thumbnail, UriKind.Absolute) && thumbnail != string.Empty)
                    {
                        var thumnailLink = new Uri(this.Request.Url, ps.HomeDirectory + thumbnail);
                        thumbnail = thumnailLink.ToString();
                    }

                    wr.WriteElementString("title", title);

                    // if the item isn't disabled add the link
                    if (!Utility.IsDisabled(Convert.ToInt32(childItemId, CultureInfo.InvariantCulture), this.PortalId))
                    {
                        wr.WriteElementString("link", UrlGenerator.GetItemLinkUrl(childItemId, this.PortalId));
                    }

                    // wr.WriteElementString("description", Utility.StripTags(this.Server.HtmlDecode(description)));
                    description = Utility.ReplaceTokens(description);
                    wr.WriteElementString("description", this.Server.HtmlDecode(description));

                    // wr.WriteElementString("author", Utility.StripTags(this.Server.HtmlDecode(author)));
                    wr.WriteElementString("thumbnail", thumbnail);

                    wr.WriteElementString("dc:creator", author);

                    wr.WriteElementString("pubDate", startDate.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture));

                    // file attachment enclosure
                    ItemVersionSetting attachmentSetting = ItemVersionSetting.GetItemVersionSetting(
                        Convert.ToInt32(r["ItemVersionId"].ToString()), "ArticleSettings", "ArticleAttachment", this.PortalId);
                    if (attachmentSetting != null)
                    {
                        if (attachmentSetting.PropertyValue.Length > 7)
                        {
                            var fileController = new FileController();
                            int fileId = Convert.ToInt32(attachmentSetting.PropertyValue.Substring(7));
                            FileInfo fi = fileController.GetFileById(fileId, this.PortalId);
                            string fileurl = "http://" + this.PortalSettings.PortalAlias.HTTPAlias + this.PortalSettings.HomeDirectory + fi.Folder +
                                             fi.FileName;
                            wr.WriteStartElement("enclosure");
                            wr.WriteAttributeString("url", fileurl);
                            wr.WriteAttributeString("length", fi.Size.ToString());
                            wr.WriteAttributeString("type", fi.ContentType);
                            wr.WriteEndElement();
                        }
                    }

                    wr.WriteStartElement("guid");

                    wr.WriteAttributeString("isPermaLink", "false");

                    wr.WriteString(guid);

                    // wr.WriteString(itemVersionId);
                    wr.WriteEndElement();

                    wr.WriteEndElement();
                }
            }

            wr.WriteEndElement();
            wr.WriteEndElement();
            this.Response.Write(sw.ToString());
        }
    }
}
