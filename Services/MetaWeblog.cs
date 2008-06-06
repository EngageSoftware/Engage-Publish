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

            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui != null)
            {
                Article a = new Article();
                a.StartDate = post.dateCreated.ToString();
                a.Name = post.title.ToString();
                a.ArticleText = post.description.ToString();
                a.AuthorUserId = ui.UserID;
                a.VersionNumber = "";
                a.VersionDescription = Localization.GetString("MetaBlogApi", LocalResourceFile);

                Engage.Dnn.Publish.Category c = Engage.Dnn.Publish.Category.GetCategory(post.categories.ToString(), PortalId);

                a.ModuleId = c.ModuleId;

                ItemRelationship irel = new ItemRelationship();
                irel.RelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();



                a.Relationships.Add(irel);
                a.DisplayTabId = c.ChildDisplayTabId;

                a.NewWindow = false;

                SaveItemVersionSettings(a);

                a.ApprovalStatusId = Util.ApprovalStatus.Approved.GetId();


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
            if (ValidateUser(username, password))
            {
                bool result = false;

                // TODO: Implement your own logic to add the post and set the result

                return result;
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        Post IMetaWeblog.GetPost(string postid, string username, string password)
        {
            if (ValidateUser(username, password))
            {
                Post post = new Post();

                // TODO: Implement your own logic to update the post and set the post

                return post;
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        CategoryInfo[] IMetaWeblog.GetCategories(string blogid, string username, string password)
        {
            if (ValidateUser(username, password))
            {
                List<CategoryInfo> categoryInfos = new List<CategoryInfo>();

                // TODO: Implement your own logic to get category info and set the categoryInfos

                return categoryInfos.ToArray();
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        Post[] IMetaWeblog.GetRecentPosts(string blogid, string username, string password,
            int numberOfPosts)
        {
            if (ValidateUser(username, password))
            {
                List<Post> posts = new List<Post>();

                // TODO: Implement your own logic to get posts and set the posts

                return posts.ToArray();
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
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
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        bool IMetaWeblog.DeletePost(string key, string postid, string username, string password, bool publish)
        {
            if (ValidateUser(username, password))
            {
                bool result = false;

                // TODO: Implement your own logic to delete the post and set the result

                return result;
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        BlogInfo[] IMetaWeblog.GetUsersBlogs(string key, string username, string password)
        {
            if (ValidateUser(username, password))
            {
                List<BlogInfo> infoList = new List<BlogInfo>();

                // TODO: Implement your own logic to get blog info objects and set the infoList

                return infoList.ToArray();
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
        }

        UserInfo IMetaWeblog.GetUserInfo(string key, string username, string password)
        {
            if (ValidateUser(username, password))
            {
                UserInfo info = new UserInfo();

                // TODO: Implement your own logic to get user info objects and set the info

                return info;
            }
            throw new XmlRpcFaultException(0, "User is not valid!");
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
                get { return "~/desktopmodules/engagepublish/services/" + DotNetNuke.Services.Localization.Localization.LocalResourceDirectory + "/MetaWeblog.asmx"; }
            }

            private static void SaveItemVersionSettings(Item av)
            {

                //Printer Friendly
                string hostPrinterFriendlySetting = HostSettings.GetHostSetting(Utility.PublishDefaultPrinterFriendly + PortalId.ToString(CultureInfo.InvariantCulture));
                Setting setting = Setting.PrinterFriendly;
                setting.PropertyValue = Convert.ToBoolean(hostPrinterFriendlySetting, CultureInfo.InvariantCulture).ToString();
                ItemVersionSetting itemVersionSetting = new ItemVersionSetting(setting);
                av.VersionSettings.Add(itemVersionSetting);

                //Email A Friend
                string hostEmailFriendSetting = HostSettings.GetHostSetting(Utility.PublishDefaultEmailAFriend + PortalId.ToString(CultureInfo.InvariantCulture));

                setting = Setting.EmailAFriend;
                setting.PropertyValue = Convert.ToBoolean(hostEmailFriendSetting, CultureInfo.InvariantCulture).ToString();
                itemVersionSetting = new ItemVersionSetting(setting);
                av.VersionSettings.Add(itemVersionSetting);

                //ratings
                string hostRatingSetting = HostSettings.GetHostSetting(Utility.PublishDefaultRatings + PortalId.ToString(CultureInfo.InvariantCulture));
                setting = Setting.Rating;
                setting.PropertyValue = Convert.ToBoolean(hostRatingSetting, CultureInfo.InvariantCulture).ToString();
                itemVersionSetting = new ItemVersionSetting(setting);
                av.VersionSettings.Add(itemVersionSetting);

                //comments
                string hostCommentSetting = HostSettings.GetHostSetting(Utility.PublishDefaultComments + PortalId.ToString(CultureInfo.InvariantCulture));
                setting = Setting.Comments;
                setting.PropertyValue = Convert.ToBoolean(hostCommentSetting, CultureInfo.InvariantCulture).ToString();
                itemVersionSetting = new ItemVersionSetting(setting);
                av.VersionSettings.Add(itemVersionSetting);

                if (ModuleBase.IsPublishCommentTypeForPortal(portalId))
                {
                    //forum comments
                    setting = Setting.ForumComments;
                    setting.PropertyValue = Convert.ToBoolean(hostCommentSetting, CultureInfo.InvariantCulture).ToString();
                    itemVersionSetting = new ItemVersionSetting(setting);
                    av.VersionSettings.Add(itemVersionSetting);
                }

                //include all articles from the parent category
                setting = Setting.ArticleSettingIncludeCategories;
                setting.PropertyValue = false.ToString();
                itemVersionSetting = new ItemVersionSetting(setting);
                av.VersionSettings.Add(itemVersionSetting);

                //display on current page option
                setting = Setting.ArticleSettingCurrentDisplay;
                setting.PropertyValue = false.ToString();
                itemVersionSetting = new ItemVersionSetting(setting);
                av.VersionSettings.Add(itemVersionSetting);

                //force display on specific page
                setting = Setting.ArticleSettingForceDisplay;
                setting.PropertyValue = false.ToString();
                itemVersionSetting = new ItemVersionSetting(setting);
                av.VersionSettings.Add(itemVersionSetting);

                //display return to list
                setting = Setting.ArticleSettingReturnToList;
                setting.PropertyValue = false.ToString();
                itemVersionSetting = new ItemVersionSetting(setting);
                av.VersionSettings.Add(itemVersionSetting);

                //show author
                string hostAuthorSetting = HostSettings.GetHostSetting(Utility.PublishDefaultShowAuthor + PortalId.ToString(CultureInfo.InvariantCulture));
                setting = Setting.Author;
                setting.PropertyValue = Convert.ToBoolean(hostAuthorSetting, CultureInfo.InvariantCulture).ToString();
                itemVersionSetting = new ItemVersionSetting(setting);
                av.VersionSettings.Add(itemVersionSetting);

                //show tags
                string hostTagsSetting = HostSettings.GetHostSetting(Utility.PublishDefaultShowTags + PortalId.ToString(CultureInfo.InvariantCulture));
                setting = Setting.ShowTags;
                setting.PropertyValue = Convert.ToBoolean(hostTagsSetting, CultureInfo.InvariantCulture).ToString();
                itemVersionSetting = new ItemVersionSetting(setting);
                av.VersionSettings.Add(itemVersionSetting);


                //use approvals
                string hostUseApprovalsSetting = HostSettings.GetHostSetting(Utility.PublishUseApprovals + PortalId.ToString(CultureInfo.InvariantCulture));
                setting = Setting.UseApprovals;
                setting.PropertyValue = Convert.ToBoolean(hostUseApprovalsSetting, CultureInfo.InvariantCulture).ToString();
                itemVersionSetting = new ItemVersionSetting(setting);
                av.VersionSettings.Add(itemVersionSetting);

            }



    }

}

