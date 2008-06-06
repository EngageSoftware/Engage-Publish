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

namespace Engage.Dnn.Publish.Services
{

    [XmlRpcService(
    Name = "blogger",
    Description = "This is a sample XML-RPC service illustrating method calls with simple parameters and return type.",
    AutoDocumentation = true)]
    [XmlRpcUrl("http://www.gtrifonov.com/MetaBlogApi.ashx")]

    public class blogger : XmlRpcService
    {
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

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="username">UserName</param>
        /// <param name="password">Password</param>
        private static UserInfo Authenticate(string username, string password)
        {
            //Check user credentials using form authentication

            UserLoginStatus loginStatus = UserLoginStatus.LOGIN_FAILURE;
            UserInfo objUser = UserController.ValidateUser(portalId, username, password, "", "", "", ref loginStatus);

            if (loginStatus == UserLoginStatus.LOGIN_FAILURE || loginStatus == UserLoginStatus.LOGIN_USERLOCKEDOUT || loginStatus == UserLoginStatus.LOGIN_USERNOTAPPROVED)
            {
                throw new System.Security.Authentication.InvalidCredentialException("Invalid credential.Access denied");
            }

            return objUser;

        }

        public string LocalResourceFile
        {
            get { return "~/desktopmodules/engagepublish/services/" + DotNetNuke.Services.Localization.Localization.LocalResourceDirectory + "/MetaWeblog.asmx"; }
        }

        [XmlRpcMethod("metaWeblog.newPost")]
        public string newPost(string blogid, string username, string password, XmlRpcStruct rpcstruct, bool publish)
        {
            try
            {

                UserInfo ui = Authenticate(username, password);
                if (ui != null)
                {
                    Article a = new Article();
                    a.StartDate = rpcstruct["pubdate"].ToString();
                    a.Name = rpcstruct["title"].ToString();
                    a.ArticleText = rpcstruct["description"].ToString();
                    a.AuthorUserId = ui.UserID;
                    a.VersionNumber = "";
                    a.VersionDescription = Localization.GetString("MetaBlogApi", LocalResourceFile);

                    Category c = Category.GetCategory(rpcstruct["category"].ToString(), PortalId);

                    a.ModuleId = c.ModuleId;

                    ItemRelationship irel = new ItemRelationship();
                    irel.RelationshipTypeId = RelationshipType.ItemToParentCategory.GetId();
                    a.Relationships.Add(irel);
                    a.DisplayTabId = c.ChildDisplayTabId;

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
                return "-1";
            }
            catch (Exception)
            {
                //TODO: handle exception
                return "-1";
            }
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


        [XmlRpcMethod("metaWeblog.getCategories")]
        public XmlRpcStruct[] getCategories(string blogid, string username, string password)
        {
            XmlRpcStruct rpcstruct = new XmlRpcStruct();

            rpcstruct.Add("description", "description");
            rpcstruct.Add("categoryid", "123");
            rpcstruct.Add("title", "title");

            return new XmlRpcStruct[] { rpcstruct };
        }

        [XmlRpcMethod("metaWeblog.getRecentPosts")]
        public XmlRpcStruct[] getRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            XmlRpcStruct[] posts = new XmlRpcStruct[5];

            return posts;
        }
        [XmlRpcMethod("metaWeblog.getTemplate")]
        public string getTemplate(string appKey, string blogid, string username, string password, string templateType)
        {
            string id = string.Empty;
            return id;
        }
        [XmlRpcMethod("metaWeblog.editPost")]
        public bool editPost(string postid, string username, string password, XmlRpcStruct rpcstruct, bool publish)
        {

            return true;
        }
        [XmlRpcMethod("metaWeblog.getPost")]
        public XmlRpcStruct getPost(string postid, string username, string password)
        {

            XmlRpcStruct rpcstruct = null;
            return rpcstruct;
        }

        [XmlRpcMethod("metaWeblog.deletePost")]
        public bool deletePost(string appKey, string postid, string username, string password, bool publish)
        {
            return false;

        }
        [XmlRpcMethod("metaWeblog.newMediaObject")]
        public XmlRpcStruct newMediaObject(string blogid, string username, string password, XmlRpcStruct rpcstruct)
        {
            XmlRpcStruct rstruct = null;
            bool allowed = System.Web.Security.FormsAuthentication.Authenticate(username, password);

            return rstruct;
        }
    }
}
