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

        string IMetaWeblog.AddPost(string blogid, string username, string password,
            Post post, bool publish)
        {
            //TODO: something fails in here

            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui != null)
            {
               
                Engage.Dnn.Publish.Category c = Engage.Dnn.Publish.Category.GetCategory(post.categories.ToString(), PortalId);

                Article a = Article.CreateArticle(post.title.ToString(), post.description.ToString(), post.description.ToString(), ui.UserID, c.ItemId, c.ModuleId, c.PortalId);

                a.VersionDescription = Localization.GetString("MetaBlogApi", LocalResourceFile);

                a.Save(ui.UserID);
                //TODO: check if ping enabled
                if (Utility.IsPingEnabledForPortal(PortalId))
                {
                    string s = HostSettings.GetHostSetting(Utility.PublishPingChangedUrl + PortalId.ToString(CultureInfo.InvariantCulture));
                    string changedUrl = Utility.HasValue(s) ? s.ToString() : Globals.NavigateURL(c.ChildDisplayTabId);

                    Hashtable ht = PortalSettings.GetSiteSettings(PortalId);

                    //ping
                    Ping.SendPing(ht["PortalName"].ToString(), ht["PortalAlias"].ToString(), changedUrl, PortalId);
                }
                return a.ItemId.ToString(CultureInfo.InvariantCulture);
            }

            throw new XmlRpcFaultException(0, "User Did Not Authenticate!");

            //throw new XmlRpcFaultException(0, "User is not valid!");
        

            //if (ValidateUser(username, password))
            //{
            //    string id = string.Empty;

            //    // TODO: Implement your own logic to add the post and set the id

            //    return id;
            //}
            //throw new XmlRpcFaultException(0, "User is not valid!");
        }

        bool IMetaWeblog.UpdatePost(string postid, string username, string password,
            Post post, bool publish)
        {
            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui.UserID > 0)
            {

                bool result = false;

                // TODO: Implement your own logic to add the post and set the result

                return result;
            }
            throw new XmlRpcFaultException(0, "User is not valid! Update post");
        }

        Post IMetaWeblog.GetPost(string postid, string username, string password)
        {
            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui.UserID > 0)
            {
                Post post = new Post();

                // TODO: Implement your own logic to update the post and set the post

                return post;
            }
            throw new XmlRpcFaultException(0, "User is not valid! Get Post");
        }

        CategoryInfo[] IMetaWeblog.GetCategories(string blogid, string username, string password)
        {
            if (ValidateUser(username, password))
            {
                List<CategoryInfo> categoryInfos = new List<CategoryInfo>();

                // TODO: Implement your own logic to get category info and set the categoryInfos

                return categoryInfos.ToArray();
            }
            throw new XmlRpcFaultException(0, "User is not valid! Get Categories");
        }

        Post[] IMetaWeblog.GetRecentPosts(string blogid, string username, string password,
            int numberOfPosts)
        {
            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui.UserID > 0)
            {
                List<Post> posts = new List<Post>();

                // TODO: Implement your own logic to get posts and set the posts

                return posts.ToArray();
            }
            throw new XmlRpcFaultException(0, "User is not valid! Get Recent Posts");
        }

        MediaObjectInfo IMetaWeblog.NewMediaObject(string blogid, string username, string password,
            MediaObject mediaObject)
        {
            if (ValidateUser(username, password))
            {
                MediaObjectInfo objectInfo = new MediaObjectInfo();

                // TODO: Implement your own logic to add media object and set the objectInfo

                return objectInfo;
            }
            throw new XmlRpcFaultException(0, "User is not valid! New Media Object");
        }

        bool IMetaWeblog.DeletePost(string key, string postid, string username, string password, bool publish)
        {
            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui.UserID > 0)
            {
                bool result = false;

                // TODO: Implement your own logic to delete the post and set the result

                return result;
            }
            throw new XmlRpcFaultException(0, "User is not valid! Delete Post");
        }

        BlogInfo[] IMetaWeblog.GetUsersBlogs(string key, string username, string password)
        {
            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);


            if (ui.UserID > 0)
            {
                //todo: configure blog info for users
                List<BlogInfo> infoList = new List<BlogInfo>();
                BlogInfo bi = new BlogInfo();
                bi.blogid = "1";
                bi.blogName = ui.Username;

                bi.url = "http://localhost/dotnetnuke_2/";
                infoList.Add(bi);

                // TODO: Implement your own logic to get blog info objects and set the infoList

                return infoList.ToArray();
            }
            throw new XmlRpcFaultException(0, "User is not valid! Failed getting a list of blogs for a user");
        }

        UserInfo IMetaWeblog.GetUserInfo(string key, string username, string password)
        {
            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui.UserID > 0)
            {
                UserInfo info = new UserInfo();
                info.email = ui.Email;
                info.firstname = ui.FirstName;
                info.lastname = ui.LastName;
                info.nickname = ui.DisplayName;
                info.userid = ui.UserID.ToString();

                // TODO: Implement your own logic to get user info objects and set the info

                return info;
            }
            throw new XmlRpcFaultException(0, "User is not valid! Failed at GetUserInfo");
        }

        #endregion

        #region Private Methods

        private bool ValidateUser(string username, string password)
        {
            bool result = false;

            // TODO: Implement the logic to validate the user

            return result;
        }

            ///<summary>
            /// Authenticate user
            /// </summary>
            /// <param name="username">UserName</param>
            /// <param name="password">Password</param>
            private static DotNetNuke.Entities.Users.UserInfo Authenticate(string username, string password)
            {
                //Check user credentials using form authentication

                UserLoginStatus loginStatus = UserLoginStatus.LOGIN_FAILURE;
                DotNetNuke.Entities.Users.UserInfo objUser = UserController.ValidateUser(portalId, username, password, "", "", "", ref loginStatus);

                if (loginStatus == UserLoginStatus.LOGIN_FAILURE || loginStatus == UserLoginStatus.LOGIN_USERLOCKEDOUT || loginStatus == UserLoginStatus.LOGIN_USERNOTAPPROVED)
                {
                    throw new System.Security.Authentication.InvalidCredentialException("Invalid credential.Access denied");
                }

                return objUser;

            }

        #endregion


            private static int portalId;// = 0;
            //TODO: fix portal id
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
                get { return "~/desktopmodules/engagepublish/services/" + DotNetNuke.Services.Localization.Localization.LocalResourceDirectory + "/MetaWeblog.ashx"; }
            }

    }

}

