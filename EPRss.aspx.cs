//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using DotNetNuke.Framework;
using Engage.Dnn.Publish.Data;
using System.Collections;
using DotNetNuke.UI.Utilities;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish
{
	public partial class EPRss : PageBase
	{
		#region Web Form Designer generated code

		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);

            
            //read the tags querystring parameter and see if we have any to store into the tagQuery arraylist
            if (AllowTags)
            {
                object t = Request.QueryString["Tags"];
                if (t != null)
                {
                    qsTags = HttpUtility.UrlDecode(t.ToString());
                    char[] seperator = { '-' };
                    tagQuery = new ArrayList(Tag.ParseTags(qsTags, PortalId, seperator, false).Count);
                    foreach (Tag tg in Tag.ParseTags(qsTags, PortalId, seperator, false))
                    {
                        //create a list of tagids to query the database
                        tagQuery.Add(tg.TagId);
                    }
                }
            }

		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		
		#endregion

        public bool AllowTags
        {
            get
            {
                return ModuleBase.AllowTagsForPortal(PortalId);
            }
        }


		public static string ApplicationUrl
		{
			get
			{			
				return HttpContext.Current.Request.ApplicationPath.ToString() == "/" ? "" : HttpContext.Current.Request.ApplicationPath.ToString();  
			}		
		}

        private string qsTags;
        private ArrayList tagQuery;


		public int ItemId
		{
			get 
			{ 
				string i = Request.Params["itemId"];
				if (i != null)
				{
					// look up the itemType if ItemId passed in.
					ItemType = Item.GetItemType(Convert.ToInt32(i, CultureInfo.InvariantCulture)).ToUpperInvariant();
					return Convert.ToInt32(i, CultureInfo.InvariantCulture);
				}
				else return -1;
			}
		}

		public int ItemTypeId
		{
			get 
			{ 
				string i = Request.Params["itemTypeId"];
				if (i != null)
				{
					return Convert.ToInt32(i, CultureInfo.InvariantCulture);
				}
				else return -1;
			}
		}

		public int NumberOfItems
		{
			get 
			{ 
				string i = Request.Params["numberOfItems"];
				if (i != null)
				{
					return Convert.ToInt32(i, CultureInfo.InvariantCulture);
				}
				else return -1;
			}
		}

		public int PortalId
		{
			get 
			{ 
				string i = Request.Params["portalId"];
                if (i != null)
                {
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }
                else
                {
                    return -1;
                }
			}
		}

        public string DisplayType
        {
            get
            {
                string i = Request.Params["DisplayType"];
                if (!string.IsNullOrEmpty(i))
                {
                    return i;
                }
                else
                {
                    return null;
                }
            }
        }

        public int RelationshipTypeId
        {
            get
            {
                string i = Request.Params["RelationshipTypeId"];
                if (!string.IsNullOrEmpty(i))
                {
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }
                else
                {
                    return -1;
                }
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _itemType;
		public string ItemType
		{	
            [DebuggerStepThrough]
			get	{ return _itemType ; }
            [DebuggerStepThrough]
			set  { _itemType  = value; }
		}

		private void Page_Load(object sender, System.EventArgs e)
		{	

			//PortalSettings ps = (PortalSettings)HttpContext.Current.Items["PortalSettings"];
            DotNetNuke.Entities.Portals.PortalSettings ps = Util.Utility.GetPortalSettings(PortalId);

			Response.ContentType = "text/xml";
			Response.ContentEncoding = Encoding.UTF8;


			StringWriter sw = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter wr = new XmlTextWriter(sw);

			wr.WriteStartElement("rss");
			wr.WriteAttributeString("version", "2.0");
			wr.WriteAttributeString("xmlns:wfw", "http://wellformedweb.org/CommentAPI/");
			wr.WriteAttributeString("xmlns:slash", "http://purl.org/rss/1.0/modules/slash/");
            wr.WriteAttributeString("xmlns:dc", "http://purl.org/dc/elements/1.1/");
            wr.WriteAttributeString("xmlns:trackback", "http://madskills.com/public/xml/rss/module/trackback/");

		    wr.WriteStartElement("channel");
			wr.WriteElementString("title", ps.PortalName);
			if(ps.PortalAlias.HTTPAlias.IndexOf("http://", StringComparison.OrdinalIgnoreCase) == -1)
			{
				wr.WriteElementString("link","http://" + ps.PortalAlias.HTTPAlias);
			}
			else
			{
				wr.WriteElementString("link", ps.PortalAlias.HTTPAlias);
			}
			wr.WriteElementString("description", "RSS Feed for " + ps.PortalName);
			wr.WriteElementString("ttl","120");

            //TODO: look into options for how to display the "Title" of the RSS feed
			DataTable dt = new DataTable();
            dt.Locale = CultureInfo.InvariantCulture;
            if (DisplayType == "ItemListing" || DisplayType == null)
            {
                if (ItemId == -1)
                {
                    dt = DataProvider.Instance().GetMostRecent(ItemTypeId, NumberOfItems, PortalId);
                }
                else
                {
                    dt = DataProvider.Instance().GetMostRecentByCategoryId(ItemId, ItemTypeId, NumberOfItems, PortalId);
                }



            }
            else if (DisplayType == "CategoryFeature")
            {
                DataSet ds = DataProvider.Instance().GetParentItems(ItemId, PortalId, RelationshipTypeId);
                dt = ds.Tables[0];
            }
            else if (DisplayType == "TagFeed")
            {
                if (AllowTags && tagQuery != null && tagQuery.Count > 0)
                {
                    string tagCacheKey = Utility.CacheKeyPublishTag + PortalId.ToString() + ItemTypeId.ToString(CultureInfo.InvariantCulture) + qsTags; // +"PageId";
                    dt = DataCache.GetCache(tagCacheKey) as DataTable;
                    if (dt == null)
                    {
                        dt = Tag.GetItemsFromTags(PortalId, tagQuery);
                        //TODO: should we set a 5 minute cache on RSS? 
                        DataCache.SetCache(tagCacheKey, dt, DateTime.Now.AddMinutes(5));
                        Utility.AddCacheKey(tagCacheKey, PortalId);
                    }
                }
            }
            if (dt != null)
            {
                DataView dv = dt.DefaultView;

                for (int i = 0; i < dv.Count; i++)
                {
                    //DataRow r = dt.Rows[i];
                    DataRow r = dv[i].Row;
                    wr.WriteStartElement("item");

                    //				wr.WriteElementString("slash:comments", objArticle.CommentCount.ToString());
                    //                wr.WriteElementString("wfw:commentRss", "http://" & Request.Url.Host & Me.ResolveUrl("RssComments.aspx?TabID=" & m_tabID & "&ModuleID=" & m_moduleID & "&ArticleID=" & objArticle.ArticleID.ToString()).Replace(" ", "%20"));
                    //                wr.WriteElementString("trackback:ping", "http://" & Request.Url.Host & Me.ResolveUrl("Tracking/Trackback.aspx?ArticleID=" & objArticle.ArticleID.ToString() & "&PortalID=" & _portalSettings.PortalId.ToString() & "&TabID=" & _portalSettings.ActiveTab.TabID.ToString()).Replace(" ", "%20"));

                    string title = String.Empty
                        , description = String.Empty
                        , childItemId = String.Empty
                        , thumbnail = String.Empty
                        , itemVersionId = String.Empty
                        , guid = String.Empty;

                    DateTime lastUpdated = DateTime.MinValue;

                    if (DisplayType == null || string.Equals(DisplayType, "ItemListing", StringComparison.OrdinalIgnoreCase) || string.Equals(DisplayType, "TagFeed", StringComparison.OrdinalIgnoreCase))
                    {
                        title = r["ChildName"].ToString();
                        description = r["ChildDescription"].ToString();
                        childItemId = r["ChilditemId"].ToString();
                        itemVersionId = r["itemVersionID"].ToString();
                        guid = r["itemVersionIdentifier"].ToString();
                        lastUpdated = (DateTime)r["LastUpdated"];
                        thumbnail = r["Thumbnail"].ToString();
                    }
                    else if (string.Equals(DisplayType, "CategoryFeature", StringComparison.OrdinalIgnoreCase))
                    {
                        title = r["Name"].ToString();
                        description = r["Description"].ToString();
                        childItemId = r["itemId"].ToString();
                        itemVersionId = r["itemVersionID"].ToString();
                        guid = r["itemVersionIdentifier"].ToString();
                        lastUpdated = (DateTime)r["LastUpdated"];
                        thumbnail = r["Thumbnail"].ToString();
                    }

                    if (!Uri.IsWellFormedUriString(thumbnail, UriKind.Absolute))
                    {
                        Uri thumnailLink = new Uri(Request.Url, ps.HomeDirectory + thumbnail);
                        thumbnail = thumnailLink.ToString();
                    }

                    wr.WriteElementString("title", title);

                    //if the item isn't disabled add the link
                    if (!Engage.Dnn.Publish.Util.Utility.IsDisabled(Convert.ToInt32(childItemId, CultureInfo.InvariantCulture), PortalId))
                    {
                        wr.WriteElementString("link", "http://" + Request.Url.Authority + ApplicationUrl + ModuleBase.DesktopModuleFolderName + "itemlink.aspx?itemId=" + childItemId);
                    }

                    wr.WriteElementString("description", Util.Utility.StripTags(Server.HtmlDecode(description)));
                    wr.WriteElementString("thumbnail", thumbnail);

                    //TODO: get creator
                    //wr.WriteElementString("dc:creator", r["DisplayName"].ToString());

                    wr.WriteElementString("pubDate", lastUpdated.ToUniversalTime().ToString("r", CultureInfo.InvariantCulture));
                    wr.WriteStartElement("guid");
                    wr.WriteAttributeString("isPermaLink", "false");
                    wr.WriteString(guid);
                    //wr.WriteString(itemVersionId);
                    wr.WriteEndElement();

                    wr.WriteEndElement();

                }
            }

	        wr.WriteEndElement();
            wr.WriteEndElement();
            Response.Write(sw.ToString());
		}
	}
}
