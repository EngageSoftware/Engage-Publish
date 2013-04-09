// <copyright file="MetaWeblog.cs" company="Engage Software">
// Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Security.Authentication;
    using System.Web;

    using CookComputing.XmlRpc;

    using DotNetNuke.Common;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Controllers;
    using DotNetNuke.Entities.Portals;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Security.Membership;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    // This code is written based off the article located at http://nayyeri.net/blog/implement-metaweblog-api-in-asp-net/

    // Right now we don't have any support for pulling a list of articles for a particular user, most likely not a big deal.
    // Right now Publish works well as a single blog, not multiple blogs for different users
    // Need to figure out how we're going to do our parsing

    public class MetaWeblog : XmlRpcService, IMetaWeblog
    {
        private static int _portalId; // = 0;

        public static int PortalId
        {
            get { return _portalId; }
            set { _portalId = value; }
        }

        public static string PortalPath { get; set; }

        public string LocalResourceFile
        {
            get { return "~/desktopmodules/engagepublish/services/" + Localization.LocalResourceDirectory + "/MetaWeblog.ashx.resx"; }
        }

        ///<summary>
        /// Add a new blog post
        /// </summary>
        /// <param name="blogid">Blogid</param>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <param name="post">post</param>
        /// <param name="publish">publish</param>
        string IMetaWeblog.AddPost(string blogid, string username, string password, Post post, bool publish)
        {
            return LogExceptions(() =>
            {
                this.LocatePortal(this.Context.Request);

                DotNetNuke.Entities.Users.UserInfo ui = this.Authenticate(username, password);
                if (ui != null)
                {
                    // TODO: we need a default category for users, then we can allow theme detection in WLW
                    var pc = new List<Publish.Category>();
                    foreach (string s in post.categories)
                    {
                        pc.Add(Publish.Category.GetCategory(s, PortalId));
                    }

                    // This only works for the first category, how should we handle other categories?
                    if (pc.Count < 1)
                    {
                        pc.Add(Publish.Category.GetCategory(ModuleBase.DefaultCategoryForPortal(PortalId), PortalId));
                    }

                    if (pc.Count > 0)
                    {
                        // get description
                        // string description = post.description.Substring(0,post.description.IndexOf("
                        // look for <!--pagebreak--> 
                        Article a = Article.Create(
                            post.title,
                            post.description,
                            post.description,
                            ui.UserID,
                            pc[0].ItemId,
                            pc[0].ModuleId,
                            pc[0].PortalId);

                        // TODO: check if dateCreated is a valid date
                        // TODO: date Created is coming in as UTC time
                        // TODO: re-enable Date created
                        // a.StartDate = post.dateCreated.ToString();
                        a.VersionDescription = Localization.GetString("MetaBlogApi", this.LocalResourceFile);

                        if (pc.Count > 1)
                        {
                            for (int i = 1; i < pc.Count; i++)
                            {
                                a.Relationships.Add(new ItemRelationship
                                    {
                                        RelationshipTypeId = RelationshipType.ItemToRelatedCategory.GetId(),
                                        ParentItemId = pc[i].ItemId
                                    });
                            }
                        }

                        // check for tags
                        if (post.mt_keywords != null && post.mt_keywords.Trim() != string.Empty)
                        {
                            // split tags
                            foreach (Tag t in Tag.ParseTags(post.mt_keywords, _portalId))
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
                        if (ModuleBase.UseApprovalsForPortal(_portalId))
                        {
                            var hostController = HostController.Instance;
                            if (ui.IsInRole(hostController.GetString(Utility.PublishAdminRole + _portalId)) || ui.IsSuperUser)
                            {
                                a.ApprovalStatusId = ApprovalStatus.Approved.GetId();
                            }
                            else if (ui.IsInRole(hostController.GetString(Utility.PublishAuthorRole + _portalId)))
                            {
                                a.ApprovalStatusId = ApprovalStatus.Waiting.GetId();
                            }
                        }

                        a.Save(ui.UserID);
                        return a.ItemId.ToString();
                    }

                    throw new XmlRpcFaultException(
                        0, Localization.GetString("PostCategoryFailed.Text", this.LocalResourceFile));
                }

                throw new XmlRpcFaultException(
                    0, Localization.GetString("FailedAuthentication.Text", this.LocalResourceFile));
            });
        }

        bool IMetaWeblog.DeletePost(string key, string postid, string username, string password, bool publish)
        {
            return LogExceptions(() =>
            {
                this.LocatePortal(this.Context.Request);
                DotNetNuke.Entities.Users.UserInfo ui = this.Authenticate(username, password);
                if (ui.UserID > 0)
                {
                    // Item.DeleteItem(Convert.ToInt32(postid));
                    Item.DeleteItem(Convert.ToInt32(postid), _portalId);

                    return true;
                }

                throw new XmlRpcFaultException(
                    0, Localization.GetString("FailedAuthentication.Text", this.LocalResourceFile));
            });
        }

        CategoryInfo[] IMetaWeblog.GetCategories(string blogid, string username, string password)
        {
            return LogExceptions(() =>
            {
                this.LocatePortal(this.Context.Request);
                DotNetNuke.Entities.Users.UserInfo ui = this.Authenticate(username, password);
                if (ui != null)
                {
                    var categoryInfos = new List<CategoryInfo>();

                    DataTable dt = Publish.Category.GetCategoriesByPortalId(PortalId);
                    foreach (DataRow dr in dt.Rows)
                    {
                        var ci = new CategoryInfo
                            {
                                title = dr["Name"].ToString(),
                                categoryid = dr["ItemId"].ToString(),
                                description = dr["Description"].ToString(),
                                htmlUrl = UrlGenerator.GetItemLinkUrl((int)dr["ItemId"], Globals.GetPortalSettings(), (dr["DisplayTabId"] as int?) ?? Null.NullInteger, (int)dr["ModuleId"]),
                                rssUrl = ModuleBase.GetRssLinkUrl(dr["ItemId"].ToString(), 25, Null.NullInteger, PortalId, string.Empty)
                            };
                        categoryInfos.Add(ci);
                    }

                    return categoryInfos.ToArray();
                }

                throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", this.LocalResourceFile));
            });
        }

        Post IMetaWeblog.GetPost(string postid, string username, string password)
        {
            return LogExceptions(() =>
            {
                this.LocatePortal(this.Context.Request);
                DotNetNuke.Entities.Users.UserInfo ui = this.Authenticate(username, password);
                if (ui.UserID > 0)
                {
                    var post = new Post();

                    Article a = Article.GetArticle(Convert.ToInt32(postid), _portalId, true, false);

                    post.description = a.ArticleText;
                    post.title = a.Name;
                    post.postid = a.ItemId.ToString();
                    post.userid = a.AuthorUserId.ToString();
                    post.dateCreated = Convert.ToDateTime(a.StartDate);

                    int i = 0;
                    foreach (ItemRelationship ir in a.Relationships)
                    {
                        var c = new Category
                            {
                                categoryId = ir.ParentItemId.ToString()
                            };
                        Publish.Category pcc = Publish.Category.GetCategory(ir.ParentItemId);
                        c.categoryName = pcc.Name;
                        post.categories[i] = c.ToString();
                        i++;
                    }

                    return post;
                }

                throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", this.LocalResourceFile));
            });
        }

        Post[] IMetaWeblog.GetRecentPosts(string blogid, string username, string password, int numberOfPosts)
        {
            return LogExceptions(() =>
            {
                this.LocatePortal(this.Context.Request);
                DotNetNuke.Entities.Users.UserInfo ui = this.Authenticate(username, password);
                if (ui.UserID > 0)
                {
                    var posts = new List<Post>();

                    // TODO: Implement your own logic to get posts and set the posts
                    // TODO: get a collection of posts for an author...
                    return posts.ToArray();
                }

                throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", this.LocalResourceFile));
            });
        }

        BloggerPost[] IMetaWeblog.GetRecentPosts(string key, string blogid, string username, string password, int numberOfPosts)
        {
            return LogExceptions(() =>
            {
                DotNetNuke.Entities.Users.UserInfo ui = this.Authenticate(username, password);
                var bp = new BloggerPost();
                if (ui.UserID > 0)
                {
                    var posts = new List<BloggerPost>();

                    bp.content = "test post";
                    bp.dateCreated = DateTime.Now;
                    bp.postid = "1";
                    bp.userid = "1";

                    // TODO: Implement your own logic to get posts and set the posts
                    return posts.ToArray();
                }

                throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", this.LocalResourceFile));
            });
        }

        UserInfo IMetaWeblog.GetUserInfo(string key, string username, string password)
        {
            return LogExceptions(() =>
            {
                this.LocatePortal(this.Context.Request);
                DotNetNuke.Entities.Users.UserInfo ui = this.Authenticate(username, password);
                if (ui.UserID > 0)
                {
                    var info = new UserInfo
                        {
                            email = ui.Email,
                            firstname = ui.FirstName,
                            lastname = ui.LastName,
                            nickname = ui.DisplayName,
                            userid = ui.UserID.ToString()
                        };

                    return info;
                }

                throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", this.LocalResourceFile));
            });
        }

        BlogInfo[] IMetaWeblog.GetUsersBlogs(string key, string username, string password)
        {
            return LogExceptions(() =>
            {
                this.LocatePortal(this.Context.Request);
                DotNetNuke.Entities.Users.UserInfo ui = this.Authenticate(username, password);

                if (ui.UserID > 0)
                {
                    // todo: configure blog info for users
                    var infoList = new List<BlogInfo>();
                    var bi = new BlogInfo
                        {
                            blogid = "0"
                        };
                    var pac = new PortalAliasController();
                    foreach (PortalAliasInfo api in pac.GetPortalAliasArrayByPortalID(PortalId))
                    {
                        bi.url = "http://" + api.HTTPAlias;
                        break;
                    }

                    bi.blogName = ui.Username;

                    infoList.Add(bi);

                    return infoList.ToArray();
                }

                throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", this.LocalResourceFile));
            });
        }

        MediaObjectInfo IMetaWeblog.NewMediaObject(string blogid, string username, string password, MediaObject mediaObject)
        {
            return LogExceptions(() =>
            {
                this.LocatePortal(this.Context.Request);

                DotNetNuke.Entities.Users.UserInfo ui = this.Authenticate(username, password);
                if (ui.UserID > 0)
                {
                    var objectInfo = new MediaObjectInfo();

                    string name = mediaObject.name; // object name
                    var media = mediaObject.bits; // object body

                    // Save media object to filesystem. Split name with '/' to extract filename (Windows Live Writer specific)
                    int index = name.LastIndexOf('/');
                    Directory.CreateDirectory(Utility.GetThumbnailLibraryMapPath(PortalId).LocalPath + name.Substring(0, index));
                    FileStream stream = File.Create(Utility.GetThumbnailLibraryMapPath(PortalId).LocalPath + name);
                    stream.Write(media, 0, media.Length);
                    stream.Flush();
                    stream.Close();
                    stream.Dispose();
                    objectInfo.url = Utility.GetThumbnailLibraryPath(PortalId) + name;
                    return objectInfo;
                }

                throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", this.LocalResourceFile));
            });
        }

        bool IMetaWeblog.UpdatePost(string postid, string username, string password, Post post, bool publish)
        {
            return LogExceptions(() =>
            {
                this.LocatePortal(this.Context.Request);
                DotNetNuke.Entities.Users.UserInfo ui = this.Authenticate(username, password);
                if (ui.UserID > 0)
                {
                    Article a = Article.GetArticle(Convert.ToInt32(postid), _portalId, true, true, true);

                    a.Description = post.description;
                    a.ArticleText = post.description;
                    a.Name = post.title;
                    a.VersionDescription = Localization.GetString("MetaBlogApi", this.LocalResourceFile);

                    var pc = new List<Publish.Category>();
                    foreach (string s in post.categories)
                    {
                        pc.Add(Publish.Category.GetCategory(s, PortalId));
                    }

                    // remove all existing categories
                    a.Relationships.Clear();

                    // add the parent category
                    if (pc.Count > 0)
                    {
                        a.Relationships.Add(new ItemRelationship
                            {
                                RelationshipTypeId = RelationshipType.ItemToParentCategory.GetId(),
                                ParentItemId = pc[0].ItemId
                            });
                    }

                    // add any extra categories
                    if (pc.Count > 1)
                    {
                        for (int i = 1; i < pc.Count; i++)
                        {
                            a.Relationships.Add(new ItemRelationship
                                {
                                    RelationshipTypeId = RelationshipType.ItemToRelatedCategory.GetId(),
                                    ParentItemId = pc[i].ItemId
                                });
                        }
                    }

                    // remove existing tags
                    a.Tags.Clear();

                    // check for tags
                    if (post.mt_keywords.Trim() != string.Empty)
                    {
                        // split tags
                        foreach (Tag t in Tag.ParseTags(post.mt_keywords, _portalId))
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
                    if (ModuleBase.UseApprovalsForPortal(_portalId))
                    {
                        var hostController = HostController.Instance;
                        if (ui.IsInRole(hostController.GetString(Utility.PublishAdminRole + _portalId)) || ui.IsSuperUser)
                        {
                            a.ApprovalStatusId = ApprovalStatus.Approved.GetId();
                        }
                        else if (ui.IsInRole(hostController.GetString(Utility.PublishAuthorRole + _portalId)))
                        {
                            a.ApprovalStatusId = ApprovalStatus.Waiting.GetId();
                        }
                    }

                    a.Save(ui.UserID);

                    return true;
                }

                throw new XmlRpcFaultException(0, Localization.GetString("FailedToUpdatePost.Text", this.LocalResourceFile));
            });
        }

        // REMOVED AS WE'RE NOT USING THIS
        // bool IMetaWeblog.SetPostCategories(string postid, string username, string password, MTCategory[] cat)
        // {

        // for (int i = 0; i < cat.Length; i++)
        // {
        // MTCategory mcat;
        // mcat = cat[i];
        // Item iv = Item.GetItem(Convert.ToInt32(postid), _portalId, ItemType.Article.GetId(), false);
        // Tag t = Tag.GetTag(mcat.categoryName, _portalId);

        // //if this item tag relationship already existed for another versionID don't increment the count;
        // if (!ItemTag.CheckItemTag(iv.ItemId, Convert.ToInt32(t.TagId)))
        // {
        // t.TotalItems++;
        // t.Save();
        // }

        // //it.ItemVersionId = i.ItemVersionId;
        // //ad the itemtag relationship
        // ItemTag.AddItemTag(iv.ItemVersionId, Convert.ToInt32(t.TagId));
        // }

        // throw new XmlRpcFaultException(0, Localization.GetString("FailedAuthentication.Text", LocalResourceFile));
        // }

        ///<summary>
        /// Authenticate user
        /// </summary>
        /// <param name="username">UserName</param>
        /// <param name="password">Password</param>
        private DotNetNuke.Entities.Users.UserInfo Authenticate(string username, string password)
        {
            // Check user credentials using form authentication

            // Check the portal alias to verify where the request is coming from and set portalid
            UserLoginStatus loginStatus = UserLoginStatus.LOGIN_FAILURE;
            DotNetNuke.Entities.Users.UserInfo objUser = UserController.ValidateUser(PortalId, username, password, string.Empty, string.Empty, string.Empty, ref loginStatus);

            if (loginStatus == UserLoginStatus.LOGIN_FAILURE || loginStatus == UserLoginStatus.LOGIN_USERLOCKEDOUT ||
                loginStatus == UserLoginStatus.LOGIN_USERNOTAPPROVED)
            {
                throw new InvalidCredentialException(Localization.GetString("FailedAuthentication.Text", this.LocalResourceFile));
            }

            // Check for the author/admin roles in Publish
            var hostController = HostController.Instance;
            if (!objUser.IsInRole(hostController.GetString(Utility.PublishAuthorRole + PortalId)) &&
                !objUser.IsInRole(hostController.GetString(Utility.PublishAdminRole + PortalId)))
            {
                throw new InvalidCredentialException(Localization.GetString("FailedAuthentication.Text", this.LocalResourceFile));
            }

            return objUser;
        }

        ///<summary>
        /// Locate Portal takes the current request and locates which portal is being called based on this request.
        /// </summary>
        /// <param name="request">request</param>
        private void LocatePortal(HttpRequest request)
        {
            string domainName = Globals.GetDomainName(request, true);
            string portalAlias = domainName;
            PortalAliasInfo pai = PortalAliasController.GetPortalAliasInfo(portalAlias);
            if (pai != null)
            {
                PortalId = pai.PortalID;
                PortalSettings ps = Utility.GetPortalSettings(pai.PortalID);
                PortalPath = ps.HomeDirectory;
            }
        }

        private static void LogExceptions(Action action)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                Exceptions.LogException(e);
                throw;
            }
        }

        private static T LogExceptions<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (Exception e)
            {
                Exceptions.LogException(e);
                throw;
            }
        }
    }
}