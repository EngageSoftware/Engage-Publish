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
using System.IO;
using DotNetNuke.Security;

namespace Engage.Dnn.Publish.Services
{
    //This code is written based off the article located at http://nayyeri.net/blog/implement-metaweblog-api-in-asp-net/

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
                if (pc.Count > 0)
                {

                    //get description
                    //string description = post.description.Substring(0,post.description.IndexOf("
                    //look for <!--pagebreak--> 


                    Article a = Article.Create(post.title.ToString(), post.description.ToString(),
                        post.description.ToString(), ui.UserID, pc[0].ItemId, pc[0].ModuleId, pc[0].PortalId);
                    //TODO: check if dateCreated is a valid date
                    //TODO: date Created is coming in as UTC time
                    //TODO: re-enable Date created
                    //a.StartDate = post.dateCreated.ToString();
                    a.VersionDescription = Localization.GetString("MetaBlogApi", LocalResourceFile);

                    if (pc.Count > 1)
                    {
                        for (int i = 1; i < pc.Count; i++)
                        {
                            ItemRelationship irel = new ItemRelationship();
                            irel.RelationshipTypeId = RelationshipType.ItemToRelatedCategory.GetId();
                            irel.ParentItemId = pc[i].ItemId;
                            a.Relationships.Add(irel);
                        }
                    }

                    //check for tags
                    if(post.mt_keywords.Trim() != string.Empty)
                    {
                        //split tags
                        foreach (Tag t in Tag.ParseTags(post.mt_keywords, portalId))
                        {
                            ItemTag it = ItemTag.Create();
                            it.TagId = Convert.ToInt32(t.TagId, CultureInfo.InvariantCulture);
                            a.Tags.Add(it);
                        } 
                    }
                    if (post.mt_excerpt!=null && post.mt_excerpt.Trim() != string.Empty)
                    {
                        a.Description = post.mt_excerpt;
                    }

                    // handle approval process
                    if (ModuleBase.UseApprovalsForPortal(portalId))
                    {
                        if (ui.IsInRole(HostSettings.GetHostSetting(Utility.PublishAdminRole + portalId)) || ui.IsSuperUser)
                        {
                            a.ApprovalStatusId = ApprovalStatus.Approved.GetId();
                        }
                        else if (ui.IsInRole(HostSettings.GetHostSetting(Utility.PublishAuthorRole + portalId)))
                        {
                            a.ApprovalStatusId = ApprovalStatus.Waiting.GetId();
                        }                        
                    }

                    a.Save(ui.UserID);
                    return a.ItemId.ToString();
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

                Article a = Article.GetArticle(Convert.ToInt32(postid), portalId);

                a.Description = post.description;
                a.ArticleText = post.description;
                a.Name = post.title;
                a.VersionDescription = Localization.GetString("MetaBlogApi", LocalResourceFile);

                List<Publish.Category> pc = new List<Engage.Dnn.Publish.Category>();
                foreach (string s in post.categories)
                {
                    Publish.Category c = Publish.Category.GetCategory(s.ToString(), PortalId);
                    pc.Add(c);
                }
                if (pc.Count > 1)
                {
                    for (int i = 1; i < pc.Count; i++)
                    {
                        ItemRelationship irel = new ItemRelationship();
                        irel.RelationshipTypeId = RelationshipType.ItemToRelatedCategory.GetId();
                        irel.ParentItemId = pc[i].ItemId;
                        a.Relationships.Add(irel);
                    }
                }

                //check for tags
                if (post.mt_keywords.Trim() != string.Empty)
                {
                    //split tags
                    foreach (Tag t in Tag.ParseTags(post.mt_keywords, portalId))
                    {
                        ItemTag it = ItemTag.Create();
                        it.TagId = Convert.ToInt32(t.TagId, CultureInfo.InvariantCulture);
                        a.Tags.Add(it);
                    }
                }

                if (post.mt_excerpt != null && post.mt_excerpt.Trim() != string.Empty)
                {
                    a.Description = post.mt_excerpt;
                }

                // handle approval process
                if (ModuleBase.UseApprovalsForPortal(portalId))
                {
                    if (ui.IsInRole(HostSettings.GetHostSetting(Utility.PublishAdminRole + portalId)) || ui.IsSuperUser)
                    {
                        a.ApprovalStatusId = ApprovalStatus.Approved.GetId();
                    }
                    else if (ui.IsInRole(HostSettings.GetHostSetting(Utility.PublishAuthorRole + portalId)))
                    {
                        a.ApprovalStatusId = ApprovalStatus.Waiting.GetId();
                    }
                }


                a.Save(ui.UserID);
                result = true;
                return result;
            }
            throw new XmlRpcFaultException(0, Localization.GetString("FailedToUpdatePost.Text", LocalResourceFile));
        }

        Post IMetaWeblog.GetPost(string postid, string username, string password)
        {
            LocatePortal(Context.Request);
            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui.UserID > 0)
            {
                Post post = new Post();

                Article a = Article.GetArticle(Convert.ToInt32(postid), portalId, true, false);

                post.description = a.ArticleText;
                post.title = a.Name;
                post.postid = a.ItemId.ToString();
                post.userid = a.AuthorUserId.ToString();
                post.dateCreated = Convert.ToDateTime(a.StartDate);

                int i = 0;
                foreach (ItemRelationship ir in a.Relationships)
                {
                    Category c = new Category();
                    c.categoryId = ir.ParentItemId.ToString();
                    Publish.Category pcc = Publish.Category.GetCategory(ir.ParentItemId);
                    c.categoryName = pcc.Name;
                    post.categories[i] = c.ToString();
                    i++;
                }
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
                    ci.htmlUrl = Utility.GetItemLinkUrl((int)dr["ItemId"], PortalId, (int)dr["DisplayTabId"], (int)dr["ModuleId"]);
                    ci.rssUrl = ModuleBase.GetRssLinkUrl(dr["ItemId"].ToString(), 25, ItemType.Article.GetId(), PortalId, string.Empty);
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


        BloggerPost[] IMetaWeblog.GetRecentPosts(string key, string blogid, string username, string password, int numberOfPosts)
        {
            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui.UserID > 0)
            {
                List<BloggerPost> posts = new List<BloggerPost>();

                BloggerPost bp = new BloggerPost();
                bp.content = "test post";
                bp.dateCreated = DateTime.Now;
                bp.postid = "1";
                bp.userid = "1";
                // TODO: Implement your own logic to get posts and set the posts

                return posts.ToArray();
            }
            throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", LocalResourceFile));
        }


        MediaObjectInfo IMetaWeblog.NewMediaObject(string blogid, string username, string password,
            MediaObject mediaObject)
        {
            LocatePortal(Context.Request);

            DotNetNuke.Entities.Users.UserInfo ui = Authenticate(username, password);
            if (ui.UserID > 0)
            {
                MediaObjectInfo objectInfo = new MediaObjectInfo();

                string name = mediaObject.name; //object name
                string type = mediaObject.type; //object type
                byte[] media = (byte[])mediaObject.bits;   //object body

                //Save media object to filesystem. Split name with '/' to extract filename (Windows Live Writer specific)
                int index = name.LastIndexOf('/');
                Directory.CreateDirectory(Utility.GetThumbnailLibraryMapPath(PortalId).AbsolutePath + name.Substring(0, index));
                FileStream stream = File.Create(Utility.GetThumbnailLibraryMapPath(PortalId).AbsolutePath + name);
                stream.Write(media, 0, media.Length);
                stream.Flush();
                stream.Close();
                stream.Dispose();
                objectInfo.url = Utility.GetThumbnailLibraryPath(PortalId) + name;
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

                //Item.DeleteItem(Convert.ToInt32(postid));
                Item.DeleteItem(Convert.ToInt32(postid), portalId);
                result = true;

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

                infoList.Add(bi);

                return infoList.ToArray();
            }
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

        //REMOVED AS WE'RE NOT USING THIS
        //bool IMetaWeblog.SetPostCategories(string postid, string username, string password, MTCategory[] cat)
        //{
            
        //    for (int i = 0; i < cat.Length; i++)
        //    {
        //        MTCategory mcat;
        //        mcat = cat[i];
        //        Item iv = Item.GetItem(Convert.ToInt32(postid), portalId, ItemType.Article.GetId(), false);
        //        Tag t = Tag.GetTag(mcat.categoryName, portalId);
 
                
        //        //if this item tag relationship already existed for another versionID don't increment the count;
        //        if (!ItemTag.CheckItemTag(iv.ItemId, Convert.ToInt32(t.TagId)))
        //        {
        //            t.TotalItems++;
        //            t.Save();
        //        }

        //        //it.ItemVersionId = i.ItemVersionId;
        //        //ad the itemtag relationship
        //        ItemTag.AddItemTag(iv.ItemVersionId, Convert.ToInt32(t.TagId));
        //    }

        //    throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", LocalResourceFile));
        //}


        #endregion

        #region Private Methods

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


        ///<summary>
        /// Locate Portal takes the current request and locates which portal is being called based on this request.
        /// </summary>
        /// <param name="request">request</param>
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
                PortalSettings ps = Utility.GetPortalSettings(pai.PortalID);
                PortalPath = ps.HomeDirectory;

            }
        }


        #endregion
        
        private static int portalId;// = 0;
        public static int PortalId
        {
            get
            {
                return portalId;
            }
            set { portalId = value; }
        }

        private static string portalPath;// = 0;
        public static string PortalPath
        {
            get
            {
                return portalPath;
            }
            set { portalPath = value; }
        }


        public string LocalResourceFile
        {
            get { return "~/desktopmodules/engagepublish/services/" + DotNetNuke.Services.Localization.Localization.LocalResourceDirectory + "/MetaWeblog.ashx.resx"; }
        }


    }

}

