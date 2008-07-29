using System;
using System.Data;
using System.Configuration;

using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using DotNetNuke.Entities.Users;

using CookComputing.XmlRpc;
using System.Collections.Generic;
using DotNetNuke.Services.Localization;
using Engage.Dnn.Publish.Util;
using DotNetNuke.Security.Membership;
using DotNetNuke.Entities.Host;
using System.Collections;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Common;
using System.Globalization;

namespace Engage.Dnn.Publish.Services
{
    //Most of this code is from

    //http://nayyeri.net/blog/implement-metaweblog-api-in-asp-net/


    //TODO: none of this is localized;

    //TODO: Thoughts on MetaBlogAPI
    //Right now we don't have any support for pulling a list of articles for a particular user, most likely not a big deal.
    //Right now Publish works well as a single blog, not multiple blogs for different users
    //Need to figure out how we're going to do our parsing

    public class MetaWeblog : XmlRpcService, IMetaWeblog
    {
        #region Public Constructors

        public MetaWeblog()
        {
        }

        #endregion


        #region IMetaWeblog Members

        ///<summary>
        /// Add a new blog post
        /// </summary>
        /// <param name="blogid">blogid</param>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <param name="post">post</param>
        /// <param name="publish">publish</param>

        string IMetaWeblog.AddPost(string blogid, string username, string password,
            Post post, bool publish)
        {
            LocatePortal(Context.Request);
            //TODO: something fails in here

            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui != null)
            {
                List<Publish.Category> pc = new List<Engage.Dnn.Publish.Category>();
                foreach (string s in post.categories)
                {
                    Publish.Category c = Publish.Category.GetCategory(s.ToString(), PortalId);
                    pc.Add(c);
                }
                //This only works for the first category, how should we handle other categories?
                if (pc.Count>0)
                {
                    Article a = Article.CreateArticle(post.title.ToString(), post.description.ToString(), 
                        post.description.ToString(), ui.UserID, pc[0].ItemId, pc[0].ModuleId, pc[0].PortalId);
                    a.StartDate = post.dateCreated.ToString();
                    a.VersionDescription = Localization.GetString("MetaBlogApi", LocalResourceFile);

                    //TODO: look to see if there are other categories

                    if (pc.Count > 1)
                    {
                        for (int i =1; i<pc.Count; i++)
                        {
                            ItemRelationship irel = new ItemRelationship();
                            irel.RelationshipTypeId = RelationshipType.ItemToRelatedCategory.GetId();
                            irel.ParentItemId = pc[i].ItemId;
                            a.Relationships.Add(irel);
                        }
                    }

                    a.Save(ui.UserID);
                    //check if ping enabled
                    //if (Utility.IsPingEnabledForPortal(PortalId))
                    //{
                    //    string s = HostSettings.GetHostSetting(Utility.PublishPingChangedUrl + PortalId.ToString(CultureInfo.InvariantCulture));
                    //    string changedUrl = Utility.HasValue(s) ? s.ToString() : Globals.NavigateURL(pc[0].ChildDisplayTabId);

                    //    Hashtable ht = PortalSettings.GetSiteSettings(PortalId);

                    //    //ping
                    //    Ping.SendPing(ht["PortalName"].ToString(), ht["PortalAlias"].ToString(), changedUrl, PortalId);
                    //}
                    Utility.ClearPublishCache(PortalId);
                    return Utility.GetItemLinkUrl(a.ItemId, PortalId, a.DisplayTabId, a.ModuleId, 0, "");    
                    
                }

                throw new XmlRpcFaultException(0, Localization.GetString("PostCategoryFailed.Text", LocalResourceFile));

                
            }
            throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", LocalResourceFile));
        }

        bool IMetaWeblog.UpdatePost(string postid, string username, string password,
            Post post, bool publish)
        {
            LocatePortal(Context.Request);
            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui.UserID > 0)
            {

                bool result = false;

                //TODO: should we add a new version of a post?
                

                return result;
            }
            throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", LocalResourceFile));
        }

        Post IMetaWeblog.GetPost(string postid, string username, string password)
        {
            LocatePortal(Context.Request);
            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui.UserID > 0)
            {
                Post post = new Post();

                // TODO: Implement your own logic to update the post and set the post

                return post;
            }
            throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", LocalResourceFile));
        }

        CategoryInfo[] IMetaWeblog.GetCategories(string blogid, string username, string password)
        {
            LocatePortal(Context.Request);
            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui != null)
            {
                List<CategoryInfo> categoryInfos = new List<CategoryInfo>();

                DataTable dt = Publish.Category.GetCategoriesByPortalId(PortalId);
                foreach (DataRow dr in dt.Rows)
                {
                    CategoryInfo ci = new CategoryInfo();
                    ci.title = dr["Name"].ToString();
                    ci.categoryid = dr["ItemId"].ToString();
                    ci.description = dr["Description"].ToString();
                    ci.htmlUrl = Utility.GetItemLinkUrl(dr["ItemId"].ToString(), PortalId, Convert.ToInt32(dr["DisplayTabId"].ToString()), Convert.ToInt32(dr["ModuleId"].ToString()), 0, "");
                    ci.rssUrl = ModuleBase.GetRssLinkUrl(dr["ItemId"].ToString(), 25, ItemType.Article.GetId(), PortalId, "");
                    categoryInfos.Add(ci);
                }

                return categoryInfos.ToArray();
            }
            throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", LocalResourceFile));
        }

        Post[] IMetaWeblog.GetRecentPosts(string blogid, string username, string password,
            int numberOfPosts)
        {
            LocatePortal(Context.Request);
            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui.UserID > 0)
            {
                List<Post> posts = new List<Post>();

                // TODO: Implement your own logic to get posts and set the posts

                return posts.ToArray();
            }
            throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", LocalResourceFile));
        }

        MediaObjectInfo IMetaWeblog.NewMediaObject(string blogid, string username, string password,
            MediaObject mediaObject)
        {
            LocatePortal(Context.Request);
            if (ValidateUser(username, password))
            {
                MediaObjectInfo objectInfo = new MediaObjectInfo();

                // TODO: Implement your own logic to add media object and set the objectInfo

                return objectInfo;
            }
            throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", LocalResourceFile));
        }

        bool IMetaWeblog.DeletePost(string key, string postid, string username, string password, bool publish)
        {
            LocatePortal(Context.Request);
            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui.UserID > 0)
            {
                bool result = false;

                // TODO: Implement your own logic to delete the post and set the result

                return result;
            }
            throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", LocalResourceFile));
        }

        BlogInfo[] IMetaWeblog.GetUsersBlogs(string key, string username, string password)
        {
            LocatePortal(Context.Request);
            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            
            if (ui.UserID > 0)
            {
                //todo: configure blog info for users
                List<BlogInfo> infoList = new List<BlogInfo>();
                BlogInfo bi = new BlogInfo();
                bi.blogid = "0";
                PortalAliasController pac = new PortalAliasController();
                foreach (PortalAliasInfo api in pac.GetPortalAliasArrayByPortalID(PortalId))
                {
                    bi.url = "http://" + api.HTTPAlias.ToString();
                    break;
                }

                bi.blogName = ui.Username;

                //bi.url = pacc[0].HTTPAlias.ToString();
                infoList.Add(bi);

                // TODO: Implement your own logic to get blog info objects and set the infoList

                return infoList.ToArray();
            }
            //TODO: localize this 
            throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", LocalResourceFile));
        }

        UserInfo IMetaWeblog.GetUserInfo(string key, string username, string password)
        {
            LocatePortal(Context.Request);
            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui.UserID > 0)
            {
                UserInfo info = new UserInfo();
                info.email = ui.Email;
                info.firstname = ui.FirstName;
                info.lastname = ui.LastName;
                info.nickname = ui.DisplayName;
                info.userid = ui.UserID.ToString();

                return info;
            }
            throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", LocalResourceFile));
        }

        #endregion

        #region Private Methods

        private bool ValidateUser(string username, string password)
        {
            LocatePortal(Context.Request);
            bool result = false;

            // TODO: Implement the logic to validate the user

            return result;
        }

        ///<summary>
        /// Authenticate user
        /// </summary>
        /// <param name="username">UserName</param>
        /// <param name="password">Password</param>
        private DotNetNuke.Entities.Users.UserInfo Authenticate(string username, string password)
        {
            //Check user credentials using form authentication

            //Check the portal alias to verify where the request is coming from and set portalid

            UserLoginStatus loginStatus = UserLoginStatus.LOGIN_FAILURE;
            DotNetNuke.Entities.Users.UserInfo objUser = UserController.ValidateUser(PortalId, username, password, "", "", "", ref loginStatus);

            if (loginStatus == UserLoginStatus.LOGIN_FAILURE || loginStatus == UserLoginStatus.LOGIN_USERLOCKEDOUT || loginStatus == UserLoginStatus.LOGIN_USERNOTAPPROVED)
            {
                throw new System.Security.Authentication.InvalidCredentialException(Localization.GetString("FailedAuthentication.Text", LocalResourceFile));
            }

            //Check for the author/admin roles in Publish
            if (!objUser.IsInRole(HostSettings.GetHostSetting(Utility.PublishAuthorRole + PortalId)) && !objUser.IsInRole(HostSettings.GetHostSetting(Utility.PublishAdminRole + PortalId)))
            {
                throw new System.Security.Authentication.InvalidCredentialException(Localization.GetString("FailedAuthentication.Text", LocalResourceFile));
            }
            return objUser;
        }

        #endregion

        private void LocatePortal(HttpRequest request)
        {            
            string requestedPath = request.Url.AbsoluteUri;
            string domainName = string.Empty;
            string portalAlias = string.Empty;

            domainName = DotNetNuke.Common.Globals.GetDomainName(request, true);

            portalAlias = domainName;
            PortalAliasInfo pai = new PortalAliasInfo();
            pai = PortalSettings.GetPortalAliasInfo(portalAlias);
            if (pai != null)
            {
                PortalId = pai.PortalID;
            }
        }


        private static int portalId;// = 0;
        public static int PortalId
        {
            get
            {
                return portalId;
            }
            set { portalId = value; }
        }
        
        public string LocalResourceFile
        {
            get { return "~/desktopmodules/engagepublish/services/" + DotNetNuke.Services.Localization.Localization.LocalResourceDirectory + "/MetaWeblog.ashx.resx"; }
        }


    }

}

