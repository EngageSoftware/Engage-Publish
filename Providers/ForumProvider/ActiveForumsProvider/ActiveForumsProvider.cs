//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.Providers;
using DotNetNuke.Services.Localization;
using Microsoft.ApplicationBlocks.Data;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish.Forum
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification="Created through reflection by ForumProvider")]
    class ActiveForumsProvider : ForumProvider
    {
        private const string ProviderType = "data";
        private readonly string LocalResourceFile = "~" + Utility.DesktopModuleFolderName + "Providers/ForumProvider/ActiveForumsProvider/App_LocalResources/ActiveForumsProvider.resx";
        private readonly ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
        private readonly string _connectionString;
        private readonly string _objectQualifier;
        private readonly string _databaseOwner;

        public string NamePrefix
        {
            get { return this._databaseOwner + this._objectQualifier; }
        }

        public ActiveForumsProvider(int portalId) : base(portalId)
        {
            Provider provider = (Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider];

            this._connectionString = Config.GetConnectionString();

            if (String.IsNullOrEmpty(this._connectionString))
            {
                this._connectionString = provider.Attributes["connectionString"];
            }

            this._objectQualifier = provider.Attributes["objectQualifier"];
            if (!String.IsNullOrEmpty(this._objectQualifier) && this._objectQualifier.EndsWith("_", StringComparison.Ordinal) == false)
            {
                this._objectQualifier += "_";
            }

            this._databaseOwner = provider.Attributes["databaseOwner"];
            if (!String.IsNullOrEmpty(this._databaseOwner) && this._databaseOwner.EndsWith(".", StringComparison.Ordinal) == false)
            {
                this._databaseOwner += ".";
            }
        }

        public override int AddComment(int forumId, int authorUserId, string title, string description, string linkUrl, string commentText, int commentUserId, string commentUserIPAddress)
        {
            Debug.Assert(authorUserId != Null.NullInteger);

            int threadId = -1;
            bool topicAlreadyCreated = false;
            UserInfo authorInfo = UserController.GetUser(PortalId, authorUserId, false);
            UserInfo commenterInfo = UserController.GetUser(PortalId, commentUserId, false);
            string authorDisplayName = authorInfo != null ?  authorInfo.DisplayName : string.Empty;
            string commenterDisplayName = commenterInfo != null ? commenterInfo.DisplayName : string.Empty;
            string body = string.Format(CultureInfo.InvariantCulture, Localization.GetString("PostBody", LocalResourceFile), description, linkUrl, title);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (SqlDataReader searchReader = SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, NamePrefix + "ActiveForums_StandardSearch",
                        Utility.CreateVarcharParam("@ForumIDs", "<fs><f fid=\"" + forumId.ToString(CultureInfo.InvariantCulture) + "\" /></fs>", 8000),
                        Utility.CreateIntegerParam("@PageIndex", 0), Utility.CreateIntegerParam("@PageSize", int.MaxValue - 1),
                        Utility.CreateNvarcharParam("@Query", "%" + title + "%", 100), Utility.CreateNvarcharParam("@User", authorDisplayName, 200),
                        Utility.CreateIntegerParam("@Timespan", 0)))
                    {
                        if (searchReader.NextResult())
                        {
                            while (searchReader.Read())
                            {
                                int bodyColumnIndex = searchReader.GetOrdinal("Body");
                                if (!searchReader.IsDBNull(bodyColumnIndex))
                                {
                                    topicAlreadyCreated = string.Equals((string)searchReader[bodyColumnIndex], body, StringComparison.Ordinal);
                                    if (topicAlreadyCreated)
                                    {
                                        threadId = (int)searchReader["PostID"];
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (!topicAlreadyCreated)
                    {
                        //Create thread by author
                        threadId = (int)(decimal)SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, NamePrefix + "NTForums_AddTopic",
                            Utility.CreateIntegerParam("@ForumID", forumId), Utility.CreateNvarcharParam("@Subject", title, 150),
                            Utility.CreateNtextParam("@Body", body), Utility.CreateIntegerParam("@UserID", authorUserId),
                            Utility.CreateNvarcharParam("@UserName", authorDisplayName, 50), Utility.CreateBitParam("@Pinned", false),
                            Utility.CreateNvarcharParam("@Icon", null, 25), Utility.CreateBitParam("@Locked", false), Utility.CreateBitParam("@Approved", true),
                            Utility.CreateNvarcharParam("@EmailAddress", "", 200), Utility.CreateNvarcharParam("@IPAddress", "", 50),
                            Utility.CreateBitParam("@IsAnnounce", false), Utility.CreateDateTimeParam("@StartDate", DateTime.Now),
                            Utility.CreateDateTimeParam("@EndDate", (DateTime?)null), Utility.CreateNvarcharParam("@PostType", "POST", 10));

                        //Add thread as a thread in the forum
                        SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, NamePrefix + "NTForums_AddTopicReply",
                            Utility.CreateIntegerParam("@NewPostID", threadId), Utility.CreateIntegerParam("@ForumID", forumId));

                        //Increment author's post count
                        SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, NamePrefix + "NTForums_UpdateUserPosts",
                            Utility.CreateIntegerParam("@UserID", authorUserId), Utility.CreateIntegerParam("@Increment", 1));
                    }
                    Debug.Assert(threadId != -1);

                    //Create comment reply
                    int replyThreadId =
                        (int)(decimal)SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, NamePrefix + "NTForums_AddReply",
                            Utility.CreateIntegerParam("@ParentPostID", threadId), Utility.CreateIntegerParam("@ForumID", forumId),
                            Utility.CreateNtextParam("@Body", commentText), Utility.CreateIntegerParam("@UserID", commentUserId),
                            Utility.CreateNvarcharParam("@UserName", commenterDisplayName, 50), Utility.CreateNvarcharParam("@Icon", null, 25),
                            Utility.CreateBitParam("@Locked", false), Utility.CreateNvarcharParam("@Subject", "Re: " + title, 150),
                            Utility.CreateBitParam("@Approved", true), Utility.CreateNvarcharParam("@EmailAddress", "", 200),
                            Utility.CreateNvarcharParam("@IPAddress", commentUserIPAddress, 50));

                    if (!Null.IsNull(commentUserId))
                    {
                        //Increment author's post count
                        SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, NamePrefix + "NTForums_UpdateUserPosts",
                            Utility.CreateIntegerParam("@UserID", commentUserId), Utility.CreateIntegerParam("@Increment", 1));
                    }

                    //Add reply as a post in the thread
                    SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, NamePrefix + "NTForums_AddPostReply",
                        Utility.CreateIntegerParam("@NewPostID", replyThreadId), Utility.CreateDateTimeParam("@PostDate", DateTime.Now),
                        Utility.CreateIntegerParam("@ParentPostID", threadId));

                    //Add reply as a post in the thread
                    SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, NamePrefix + "NTForums_UpdateForumReplies",
                        Utility.CreateIntegerParam("@ForumID", forumId), Utility.CreateIntegerParam("@PostID", replyThreadId),
                        Utility.CreateIntegerParam("@Increment", 1));

                    transaction.Commit();
                }
                return threadId;
            }
        }

        public override Dictionary<int, string> GetForums()
        {
            using (IDataReader forumsReader = SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, NamePrefix + "ActiveForums_GetGroupAndForum", Utility.CreateIntegerParam("@PortalID", PortalId)))
            {
                Dictionary<int, string> forums = new Dictionary<int, string>();

                while (forumsReader.Read())
                {
                    int nameColumnIndex = forumsReader.GetOrdinal("Name");
                    forums.Add((int)forumsReader["ForumID"], forumsReader.IsDBNull(nameColumnIndex) ? string.Empty : (string)forumsReader[nameColumnIndex]);
                }

                return forums;
            }
        }

        public override string GetThreadUrl(int threadId)
        {
            int? forumId = null;
            using (SqlDataReader postReader = SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure, NamePrefix + "NTForums_GetPostbyID",
                Utility.CreateIntegerParam("@PostID", threadId)))
            {
                if (postReader.Read())
                {
                    forumId = (int)postReader["ForumID"];
                }
            }

            if (forumId.HasValue)
            {
                using (SqlDataReader forumReader = SqlHelper.ExecuteReader(_connectionString, CommandType.StoredProcedure,
                    NamePrefix + "ActiveForums_GetForumTabID", Utility.CreateIntegerParam("@ForumID", forumId)))
                {
                    if (forumReader.Read())
                    {
                        int tabId = (int)forumReader["TabID"];

                        return Globals.NavigateURL(tabId, string.Empty, "forumid=" + forumId.Value.ToString(CultureInfo.InvariantCulture), "postid=" + threadId.ToString(CultureInfo.InvariantCulture), "view=topic");
                    }
                }
            }
            return string.Empty;
        }
    }
}