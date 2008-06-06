using System;
using System.Globalization;
using CookComputing.XmlRpc;
using DotNetNuke.Entities.Users;
using DotNetNuke.Security.Membership;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Host;
using Engage.Dnn.Publish.Util;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Common;
using System.Collections;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;

namespace Engage.Dnn.Publish.Services
{
    [XmlRpcService(
    Name = "MetaWeblog ",
    Description = "This is a sample XML-RPC service illustrating method calls with simple parameters and return type.",
    AutoDocumentation = true)]
    [XmlRpcUrl("http://localhost/dotnetnuke_2/desktopmodules/engagepublish/services/MetaWeblog.asmx")]

    public class MetaWeblogOld : XmlRpcService
    {
        //    private static int portalId;// = 0;
        //    //TODO: fix portal id
        //    public static int PortalId
        //    {
        //        get
        //        {
        //            return portalId;
        //        }
        //        set { portalId = value; }
        //    }

        //    /// <summary>
        //    /// Authenticate user
        //    /// </summary>
        //    /// <param name="username">UserName</param>
        //    /// <param name="password">Password</param>
        //    private static UserInfo Authenticate(string username, string password)
        //    {
        //        //Check user credentials using form authentication

        //        UserLoginStatus loginStatus = UserLoginStatus.LOGIN_FAILURE;
        //        UserInfo objUser = UserController.ValidateUser(portalId, username, password, "", "", "", ref loginStatus);

        //        if (loginStatus == UserLoginStatus.LOGIN_FAILURE || loginStatus == UserLoginStatus.LOGIN_USERLOCKEDOUT || loginStatus == UserLoginStatus.LOGIN_USERNOTAPPROVED)
        //        {
        //            throw new System.Security.Authentication.InvalidCredentialException("Invalid credential.Access denied");
        //        }

        //        return objUser;

        //    }

        //    public string LocalResourceFile
        //    {
        //        get { return "~/desktopmodules/engagepublish/services/" + DotNetNuke.Services.Localization.Localization.LocalResourceDirectory + "/MetaWeblog.asmx"; }
        //    }

        //    [XmlRpcMethod("metaWeblog.newPost")]
        //    public string newPost(string blogid, string username, string password, XmlRpcStruct rpcstruct, bool publish)
        //    {
        //        try
        //        {

        //            UserInfo ui = Authenticate(username, password);
        //            if (ui != null)
        //            {
        //                Article a = new Article();
        //                a.StartDate = rpcstruct["pubdate"].ToString();
        //                a.Name = rpcstruct["title"].ToString();
        //                a.ArticleText = rpcstruct["description"].ToString();
        //                a.AuthorUserId = ui.UserID;
        //                a.VersionNumber = "";
        //                a.VersionDescription = Localization.GetString("MetaBlogApi", LocalResourceFile);

        //                Category c = Category.GetCategory(rpcstruct["category"].ToString(), PortalId);

        //                a.ModuleId = c.ModuleId;

        //                ItemRelationship irel = new ItemRelationship();
        //                irel.RelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
        //                a.Relationships.Add(irel);
        //                a.DisplayTabId = c.ChildDisplayTabId;

        //                SaveItemVersionSettings(a);

        //                a.ApprovalStatusId = Util.ApprovalStatus.Approved.GetId();


        //                a.Save(ui.UserID);
        //                //TODO: check if ping enabled
        //                if (Utility.IsPingEnabledForPortal(PortalId))
        //                {
        //                    string s = HostSettings.GetHostSetting(Utility.PublishPingChangedUrl + PortalId.ToString(CultureInfo.InvariantCulture));
        //                    string changedUrl = Utility.HasValue(s) ? s.ToString() : Globals.NavigateURL(c.ChildDisplayTabId);

        //                    Hashtable ht = PortalSettings.GetSiteSettings(PortalId);

        //                    //ping
        //                    Ping.SendPing(ht["PortalName"].ToString(), ht["PortalAlias"].ToString(), changedUrl, PortalId);
        //                }

        //                return a.ItemId.ToString(CultureInfo.InvariantCulture);

        //            }
        //            return "-1";
        //        }
        //        catch (Exception)
        //        {
        //            //TODO: handle exception
        //            return "-1";
        //        }
        //    }

        //    private static void SaveItemVersionSettings(Item av)
        //    {

        //        //Printer Friendly
        //        string hostPrinterFriendlySetting = HostSettings.GetHostSetting(Utility.PublishDefaultPrinterFriendly + PortalId.ToString(CultureInfo.InvariantCulture));
        //        Setting setting = Setting.PrinterFriendly;
        //        setting.PropertyValue = Convert.ToBoolean(hostPrinterFriendlySetting, CultureInfo.InvariantCulture).ToString();
        //        ItemVersionSetting itemVersionSetting = new ItemVersionSetting(setting);
        //        av.VersionSettings.Add(itemVersionSetting);

        //        //Email A Friend
        //        string hostEmailFriendSetting = HostSettings.GetHostSetting(Utility.PublishDefaultEmailAFriend + PortalId.ToString(CultureInfo.InvariantCulture));

        //        setting = Setting.EmailAFriend;
        //        setting.PropertyValue = Convert.ToBoolean(hostEmailFriendSetting, CultureInfo.InvariantCulture).ToString();
        //        itemVersionSetting = new ItemVersionSetting(setting);
        //        av.VersionSettings.Add(itemVersionSetting);

        //        //ratings
        //        string hostRatingSetting = HostSettings.GetHostSetting(Utility.PublishDefaultRatings + PortalId.ToString(CultureInfo.InvariantCulture));
        //        setting = Setting.Rating;
        //        setting.PropertyValue = Convert.ToBoolean(hostRatingSetting, CultureInfo.InvariantCulture).ToString();
        //        itemVersionSetting = new ItemVersionSetting(setting);
        //        av.VersionSettings.Add(itemVersionSetting);

        //        //comments
        //        string hostCommentSetting = HostSettings.GetHostSetting(Utility.PublishDefaultComments + PortalId.ToString(CultureInfo.InvariantCulture));
        //        setting = Setting.Comments;
        //        setting.PropertyValue = Convert.ToBoolean(hostCommentSetting, CultureInfo.InvariantCulture).ToString();
        //        itemVersionSetting = new ItemVersionSetting(setting);
        //        av.VersionSettings.Add(itemVersionSetting);

        //        if (ModuleBase.IsPublishCommentTypeForPortal(portalId))
        //        {
        //            //forum comments
        //            setting = Setting.ForumComments;
        //            setting.PropertyValue = Convert.ToBoolean(hostCommentSetting, CultureInfo.InvariantCulture).ToString();
        //            itemVersionSetting = new ItemVersionSetting(setting);
        //            av.VersionSettings.Add(itemVersionSetting);
        //        }

        //        //include all articles from the parent category
        //        setting = Setting.ArticleSettingIncludeCategories;
        //        setting.PropertyValue = false.ToString();
        //        itemVersionSetting = new ItemVersionSetting(setting);
        //        av.VersionSettings.Add(itemVersionSetting);

        //        //display on current page option
        //        setting = Setting.ArticleSettingCurrentDisplay;
        //        setting.PropertyValue = false.ToString();
        //        itemVersionSetting = new ItemVersionSetting(setting);
        //        av.VersionSettings.Add(itemVersionSetting);

        //        //force display on specific page
        //        setting = Setting.ArticleSettingForceDisplay;
        //        setting.PropertyValue = false.ToString();
        //        itemVersionSetting = new ItemVersionSetting(setting);
        //        av.VersionSettings.Add(itemVersionSetting);

        //        //display return to list
        //        setting = Setting.ArticleSettingReturnToList;
        //        setting.PropertyValue = false.ToString();
        //        itemVersionSetting = new ItemVersionSetting(setting);
        //        av.VersionSettings.Add(itemVersionSetting);

        //        //show author
        //        string hostAuthorSetting = HostSettings.GetHostSetting(Utility.PublishDefaultShowAuthor + PortalId.ToString(CultureInfo.InvariantCulture));
        //        setting = Setting.Author;
        //        setting.PropertyValue = Convert.ToBoolean(hostAuthorSetting, CultureInfo.InvariantCulture).ToString();
        //        itemVersionSetting = new ItemVersionSetting(setting);
        //        av.VersionSettings.Add(itemVersionSetting);

        //        //show tags
        //        string hostTagsSetting = HostSettings.GetHostSetting(Utility.PublishDefaultShowTags + PortalId.ToString(CultureInfo.InvariantCulture));
        //        setting = Setting.ShowTags;
        //        setting.PropertyValue = Convert.ToBoolean(hostTagsSetting, CultureInfo.InvariantCulture).ToString();
        //        itemVersionSetting = new ItemVersionSetting(setting);
        //        av.VersionSettings.Add(itemVersionSetting);


        //        //use approvals
        //        string hostUseApprovalsSetting = HostSettings.GetHostSetting(Utility.PublishUseApprovals + PortalId.ToString(CultureInfo.InvariantCulture));
        //        setting = Setting.UseApprovals;
        //        setting.PropertyValue = Convert.ToBoolean(hostUseApprovalsSetting, CultureInfo.InvariantCulture).ToString();
        //        itemVersionSetting = new ItemVersionSetting(setting);
        //        av.VersionSettings.Add(itemVersionSetting);

        //    }


        //    [XmlRpcMethod("metaWeblog.getCategories")]
        //    public XmlRpcStruct[] getCategories(string blogid, string username, string password)
        //    {
        //        XmlRpcStruct rpcstruct = new XmlRpcStruct();

        //        rpcstruct.Add("description", "description");
        //        rpcstruct.Add("categoryid", "123");
        //        rpcstruct.Add("title", "title");

        //        return new XmlRpcStruct[] { rpcstruct };
        //    }

        //    [XmlRpcMethod("metaWeblog.getRecentPosts")]
        //    public XmlRpcStruct[] getRecentPosts(string blogid, string username, string password, int numberOfPosts)
        //    {
        //        XmlRpcStruct[] posts = new XmlRpcStruct[5];

        //        return posts;
        //    }
        //    [XmlRpcMethod("metaWeblog.getTemplate")]
        //    public string getTemplate(string appKey, string blogid, string username, string password, string templateType)
        //    {
        //        string id = string.Empty;
        //        return id;
        //    }
        //    [XmlRpcMethod("metaWeblog.editPost")]
        //    public bool editPost(string postid, string username, string password, XmlRpcStruct rpcstruct, bool publish)
        //    {

        //        return true;
        //    }
        //    [XmlRpcMethod("metaWeblog.getPost")]
        //    public XmlRpcStruct getPost(string postid, string username, string password)
        //    {

        //        XmlRpcStruct rpcstruct = null;
        //        return rpcstruct;
        //    }

        //    [XmlRpcMethod("metaWeblog.deletePost")]
        //    public bool deletePost(string appKey, string postid, string username, string password, bool publish)
        //    {
        //        return false;

        //    }
        //    [XmlRpcMethod("metaWeblog.newMediaObject")]
        //    public XmlRpcStruct newMediaObject(string blogid, string username, string password, XmlRpcStruct rpcstruct)
        //    {
        //        XmlRpcStruct rstruct = null;
        //        bool allowed = System.Web.Security.FormsAuthentication.Authenticate(username, password);

        //        return rstruct;
        //    }


        //    /// <summary>
        //    /// Returns user's blogs
        //    /// </summary>
        //    /// <param name="appKey">Application Key</param>
        //    /// <param name="username">UserName</param>
        //    /// <param name="password">Password</param>
        //    /// <returns></returns>
        //    [XmlRpcMethod("blogger.getUsersBlogs")]
        //    public XmlRpcStruct[] getUsersBlogs(string appKey, string username, string password)
        //    {
        //        Authenticate(username, password);

        //        //Create structure for blog list
        //        XmlRpcStruct rpcstruct = new XmlRpcStruct();
        //        rpcstruct.Add("blogid", "123"); // Blog Id
        //        rpcstruct.Add("blogName", "main"); // Blog Name
        //        rpcstruct.Add("url", BaseUrl); // Blog URL
        //        XmlRpcStruct[] datarpcstruct = new XmlRpcStruct[] { rpcstruct };
        //        return datarpcstruct;
        //    }

        //    /// <summary>
        //    /// Set blog post template
        //    /// </summary>
        //    /// <param name="appKey">Application Key</param>
        //    /// <param name="blogid">Blog Identificator</param>
        //    /// <param name="username">UserName</param>
        //    /// <param name="password">Password</param>
        //    /// <param name="template">Template content</param>
        //    /// <param name="templateType">Template Type</param>
        //    /// <returns></returns>
        //    [XmlRpcMethod("metaWeblog.setTemplate")]
        //    public bool setTemplate(string appKey, string blogid, string username, string password, string template, string templateType)
        //    {
        //        Authenticate(username, password);
        //        /*
        //            TODO: Add implementation
        //        */
        //        throw new System.NotImplementedException("SetTemplate is not implemented");
        //    }

        //    /// <summary>
        //    /// Returns rss web host
        //    /// </summary>
        //    private static string BaseUrl
        //    {
        //        get
        //        {
        //            return System.Configuration.ConfigurationManager.AppSettings["RSSWebHost"];
        //        }

        //    }



        //}

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="username">UserName</param>
        /// <param name="password">Password</param>
        private static void Authenticate(string username, string password)
        {
            //Check user credentials using form authentication
            bool allowed = System.Web.Security.FormsAuthentication.Authenticate(username, password);
            if (allowed == false)
            {
                throw new System.Security.Authentication.InvalidCredentialException("Invalid credential.Access denied");
            }
        }

        /// <summary>
        /// Returns user's blogs
        /// </summary>
        /// <param name="appKey">Application Key</param>
        /// <param name="username">UserName</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        [XmlRpcMethod("blogger.getUsersBlogs")]
        public XmlRpcStruct[] getUsersBlogs(string appKey, string username, string password)
        {
            Authenticate(username, password);

            //Create structure for blog list
            XmlRpcStruct rpcstruct = new XmlRpcStruct();
            rpcstruct.Add("blogid", "123"); // Blog Id
            rpcstruct.Add("blogName", "main"); // Blog Name
            rpcstruct.Add("url", BaseUrl); // Blog URL
            XmlRpcStruct[] datarpcstruct = new XmlRpcStruct[] { rpcstruct };
            return datarpcstruct;
        }

        /// <summary>
        /// Set blog post template
        /// </summary>
        /// <param name="appKey">Application Key</param>
        /// <param name="blogid">Blog Identificator</param>
        /// <param name="username">UserName</param>
        /// <param name="password">Password</param>
        /// <param name="template">Template content</param>
        /// <param name="templateType">Template Type</param>
        /// <returns></returns>
        [XmlRpcMethod("metaWeblog.setTemplate")]
        public bool setTemplate(string appKey, string blogid, string username, string password, string template, string templateType)
        {
            Authenticate(username, password);
            /*
                TODO: Add implementation
            */
            throw new System.NotImplementedException("SetTemplate is not implemented");
        }

        /// <summary>
        /// Return list of blog category list
        /// </summary>
        /// <param name="blogid">Blog Identificator</param>
        /// <param name="username">UserName</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        [XmlRpcMethod("metaWeblog.getCategories")]
        public XmlRpcStruct[] getCategories(string blogid, string username, string password)
        {
            Authenticate(username, password);
            /*
               TODO: Add real implementation to read from data source
            */
            XmlRpcStruct rpcstruct = new XmlRpcStruct();
            rpcstruct.Add("categoryid", "1"); // Category ID
            rpcstruct.Add("title", "ASP.NET"); // Category Title
            rpcstruct.Add("description", "description"); // Category Description

            return new XmlRpcStruct[] { rpcstruct };

        }

        /// <summary>
        /// Returns recent blog posts
        /// </summary>
        /// <param name="blogid">Blog Identificator</param>
        /// <param name="username">UserName</param>
        /// <param name="password">Password</param>
        /// <param name="numberOfPosts">Number of posts to return</param>
        /// <returns></returns>
        [XmlRpcMethod("metaWeblog.getRecentPosts")]
        public XmlRpcStruct[] getRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            Authenticate(username, password);

            //Uses xslt transformation to retrive recent posts
            XslCompiledTransform tr = new XslCompiledTransform();
            XPathDocument xdoc = new XPathDocument(StorageFile);
            XPathNavigator nav = xdoc.CreateNavigator();
            XPathNodeIterator iterator = nav.Select("rss/channel/item[position()<=" + numberOfPosts + "]");
            XmlRpcStruct[] posts = new XmlRpcStruct[iterator.Count];
            int i = 0;

            //Populate structure with post entities
            while (iterator.MoveNext() && i < numberOfPosts)
            {
                XmlRpcStruct rpcstruct = new XmlRpcStruct();
                rpcstruct.Add("title", iterator.Current.SelectSingleNode("title").Value);
                rpcstruct.Add("link", iterator.Current.SelectSingleNode("link").Value);
                rpcstruct.Add("description", iterator.Current.SelectSingleNode("description").Value);
                rpcstruct.Add("pubDate", iterator.Current.SelectSingleNode("pubDate").Value);
                rpcstruct.Add("guid", iterator.Current.SelectSingleNode("guid").Value);
                rpcstruct.Add("postid", iterator.Current.SelectSingleNode("guid").Value);
                rpcstruct.Add("keywords", "123 123");
                rpcstruct.Add("author", iterator.Current.SelectSingleNode("author").Value);
                posts[i] = rpcstruct;
                i++;
            }
            return posts;
        }

        /// <summary>
        /// Return post template
        /// </summary>
        /// <param name="appKey">Application Key</param>
        /// <param name="blogid">Blog Identificator</param>
        /// <param name="username">UserName</param>
        /// <param name="password">Password</param>
        /// <param name="templateType">Template Type</param>
        /// <returns></returns>
        [XmlRpcMethod("metaWeblog.getTemplate")]
        public string getTemplate(string appKey, string blogid, string username, string password, string templateType)
        {
            Authenticate(username, password);

            // TODO: add implementation with datasource
            string template = @"<HTML>
                          <HEAD>
                            <TITLE><$BlogTitle$>: <$BlogDescription$></TITLE>
                          </HEAD>
                          <BODY >
                            <h1><$BlogTitle$></h1>

                              <!-- Blogger code begins here -->

                              <BLOGGER>
                               <BlogDateHeader>
                                   <b><h4><$BlogDateHeaderDate$>:</h4></b>
                              </BlogDateHeader>
                            
                              <a name='<$BlogItemNumber$>'><$BlogItemBody$></a>
                              <br>
                              <small><$BlogItemAuthor$> 
                              <br>
                              <center>______________________</center>
                              <br>
                              </p>
                              </BLOGGER>
                           '
                          </BODY>
                        </HTML>";
            return template;
        }


        /// <summary>
        /// Create new Post
        /// </summary>
        /// <param name="blogid">Blog Identificator</param>
        /// <param name="username">UserName</param>
        /// <param name="password">Password</param>
        /// <param name="rpcstruct">Blog post XML-RPC structure</param>
        /// <param name="publish">TRUE to publish post after creation</param>
        /// <returns></returns>
        [XmlRpcMethod("metaWeblog.newPost")]
        public string newPost(string blogid, string username, string password, XmlRpcStruct rpcstruct, bool publish)
        {
            Authenticate(username, password);

            //Create a post node in existing xml storage file
            XmlDocument doc = new XmlDocument();
            doc.Load(StorageFile);
            XmlNode channel = doc.SelectSingleNode("rss/channel");
            XmlNode firstnode = doc.SelectSingleNode("rss/channel/item");
            XmlNode item = doc.CreateElement("item");
            string postpath = String.Format("{0}/blog/{1}/{2}/{3}", BaseUrl, DateTime.Now.Year, DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"));
            XmlNode title = doc.CreateElement("title");
            title.InnerText = rpcstruct["title"].ToString();

            XmlNode link = doc.CreateElement("link");
            link.InnerText = (rpcstruct["link"] != null && rpcstruct["link"].ToString().Trim() != String.Empty) ? rpcstruct["link"].ToString() :
                String.Format("{0}/{1}.aspx", postpath, rpcstruct["title"].ToString());

            XmlNode description = doc.CreateElement("description");
            description.InnerText = rpcstruct["description"] != null ? rpcstruct["description"].ToString() : string.Empty;

            XmlNode pubDate = doc.CreateElement("pubDate");
            pubDate.InnerText = rpcstruct["pubDate"] != null ? rpcstruct["pubDate"].ToString() : DateTime.Now.ToString("ddd, dd MMM yyyy H:mm:ss") + " GMT";

            XmlNode author = doc.CreateElement("author");
            author.InnerText = rpcstruct["author"] != null ? rpcstruct["author"].ToString() : username;

            XmlNode guid = doc.CreateElement("guid");
            string id = Guid.NewGuid().ToString();
            guid.InnerText = id;

            XmlNode comments = doc.CreateElement("comments");
            comments.InnerText = link.InnerText;

            //Append elements to rss item element
            item.AppendChild(title);
            item.AppendChild(link);
            item.AppendChild(description);
            item.AppendChild(pubDate);
            item.AppendChild(author);
            item.AppendChild(guid);
            item.AppendChild(comments);

            if (rpcstruct["category"] != null && rpcstruct["category"].ToString().Trim() != String.Empty)
            {
                XmlNode category = doc.CreateElement("category");
                category.InnerText = rpcstruct["category"].ToString();
                item.AppendChild(category);
            }

            //Update channel lastBuildDate element
            channel.InsertBefore(item, firstnode);
            XmlNode lastupdate = channel.SelectSingleNode("lastBuildDate");
            lastupdate.InnerText = DateTime.Now.ToString("ddd, dd MMM yyyy H:mm:ss") + " GMT";

            doc.Save(StorageFile);

            return id;
        }

        /// <summary>
        /// Edit existing Post
        /// </summary>
        /// <param name="postid">Post Identificator</param>
        /// <param name="username">UserName</param>
        /// <param name="password">Password</param>
        /// <param name="rpcstruct">Blog post XML-RPC structure</param>
        /// <param name="publish">TRUE to publish post after modification</param>
        /// <returns></returns>
        [XmlRpcMethod("metaWeblog.editPost")]
        public bool editPost(string postid, string username, string password, XmlRpcStruct rpcstruct, bool publish)
        {
            Authenticate(username, password);

            XmlDocument doc = new XmlDocument();
            doc.Load(StorageFile);
            XmlNode item = doc.SelectSingleNode("rss/channel/item[guid=\"" + postid + "\"]");

            item.SelectSingleNode("title").InnerText = rpcstruct["title"].ToString();
            item.SelectSingleNode("link").InnerText = (rpcstruct["link"] != null && rpcstruct["link"].ToString().Trim() != String.Empty) ? rpcstruct["link"].ToString() :
              String.Format("{0}/blog/{1}/{2}/{3}/{4}.aspx",
              BaseUrl,
              DateTime.Now.Year,
              DateTime.Now.ToString("MM"),
              DateTime.Now.ToString("dd"),
              rpcstruct["title"].ToString());
            item.SelectSingleNode("description").InnerText = rpcstruct["description"] != null ? rpcstruct["description"].ToString() : string.Empty;
            item.SelectSingleNode("author").InnerText = rpcstruct["author"] != null ? rpcstruct["author"].ToString() : username;
            doc.Save(StorageFile);
            return true;
        }


        /// <summary>
        /// Return existing post
        /// </summary>
        /// <param name="postid">Post Identificator</param>
        /// <param name="username">UserName</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        [XmlRpcMethod("metaWeblog.getPost")]
        public XmlRpcStruct getPost(string postid, string username, string password)
        {
            Authenticate(username, password);

            XmlDocument doc = new XmlDocument();
            doc.Load(StorageFile);
            XmlNode node = doc.SelectSingleNode("rss/channel/item[@guid ='" + postid + "'");

            XmlRpcStruct rpcstruct = new XmlRpcStruct();
            rpcstruct.Add("title", node.SelectSingleNode("title").InnerText);
            rpcstruct.Add("link", node.SelectSingleNode("link").InnerText);
            rpcstruct.Add("description", node.SelectSingleNode("description").InnerText);
            rpcstruct.Add("pubDate", node.SelectSingleNode("pubDate").InnerText);
            rpcstruct.Add("guid", node.SelectSingleNode("guid").InnerText);
            rpcstruct.Add("author", node.SelectSingleNode("author").InnerText);

            return rpcstruct;
        }

        /// <summary>
        /// Delete existing post
        /// </summary>
        /// <param name="appKey">Application key</param>
        /// <param name="postid">Post Identificator</param>
        /// <param name="username">UserName</param>
        /// <param name="password">Password</param>
        /// <param name="publish"></param>
        /// <returns></returns>
        [XmlRpcMethod("blogger.deletePost")]
        public bool deletePost(string appKey, string postid, string username, string password, bool publish)
        {
            Authenticate(username, password);

            XmlDocument doc = new XmlDocument();
            doc.Load(StorageFile);
            XmlNode channel = doc.SelectSingleNode("rss/channel");
            XmlNode node = doc.SelectSingleNode("rss/channel/item[guid =\"" + postid + "\"]");
            if (node != null)
            {
                channel.RemoveChild(node);
                doc.Save(StorageFile);
                return true;
            }
            return false;

        }

        /// <summary>
        /// Create new media object associated with post
        /// </summary>
        /// <param name="blogid">Blog Identificator</param>
        /// <param name="username">UserName</param>
        /// <param name="password">Password</param>
        /// <param name="rpcstruct">XML-RPC struct representing media object</param>
        /// <returns>struct with url location of object</returns>
        [XmlRpcMethod("metaWeblog.newMediaObject")]
        public XmlRpcStruct newMediaObject(string blogid, string username, string password, XmlRpcStruct rpcstruct)
        {
            Authenticate(username, password);

            string name = rpcstruct["name"].ToString(); //object name
            string type = rpcstruct["type"].ToString(); //object type
            byte[] media = (byte[])rpcstruct["bits"];   //object body


            //Save media object to filesystem. Split name with '/' to extract filename (Windows Live Writer specific)
            int index = name.LastIndexOf('/');
            Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + name.Substring(0, index));
            FileStream stream = File.Create(HttpContext.Current.Request.PhysicalApplicationPath + name);
            stream.Write(media, 0, media.Length);
            stream.Flush();
            stream.Close();
            stream.Dispose();
            XmlRpcStruct rstruct = new XmlRpcStruct();
            rstruct.Add("url", BaseUrl + name);
            return rstruct;
        }

        /// <summary>
        /// Returns path of xml storage file settings
        /// </summary>
        private string StorageFile
        {
            get
            {
                return HttpContext.Current.Request.PhysicalApplicationPath + "\\" + System.Configuration.ConfigurationManager.AppSettings["RSSStorage"];
            }
        }
        /// <summary>
        /// Returns rss web host
        /// </summary>
        private static string BaseUrl
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["RSSWebHost"];
            }

        }
    }

}
