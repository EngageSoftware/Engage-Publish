//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Forum
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using DotNetNuke.Common;
    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Entities.Users;
    using DotNetNuke.Framework.Providers;
    using DotNetNuke.Services.Localization;

    using Engage.Dnn.Publish.Util;

    using Microsoft.ApplicationBlocks.Data;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", 
        Justification = "Created through reflection by ForumProvider")]
    internal class ActiveForumsProvider : ForumProvider
    {
        private const string ProviderType = "data";

        private readonly string _connectionString;

        private readonly string _databaseOwner;

        private readonly string _localResourceFile = "~" + Utility.DesktopModuleFolderName +
                                                     "Providers/ForumProvider/ActiveForumsProvider/App_LocalResources/ActiveForumsProvider.resx";

        /// <summary>
        /// The null representation of a <see cref="DateTime"/> for ActiveForums
        /// </summary>
        /// <remarks>
        /// Found in reflecting through Active.Modules.Forums.40 assembly.
        /// </remarks>
        private readonly DateTime _nullDate = new DateTime(1900, 1, 1);

        private readonly string _objectQualifier;

        private readonly ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);

        public ActiveForumsProvider(int portalId)
            : base(portalId)
        {
            var provider = (Provider)this._providerConfiguration.Providers[this._providerConfiguration.DefaultProvider];

            this._connectionString = Config.GetConnectionString();

            if (string.IsNullOrEmpty(this._connectionString))
            {
                this._connectionString = provider.Attributes["connectionString"];
            }

            this._objectQualifier = provider.Attributes["objectQualifier"];
            if (!string.IsNullOrEmpty(this._objectQualifier) && this._objectQualifier.EndsWith("_", StringComparison.Ordinal) == false)
            {
                this._objectQualifier += "_";
            }

            this._databaseOwner = provider.Attributes["databaseOwner"];
            if (!string.IsNullOrEmpty(this._databaseOwner) && this._databaseOwner.EndsWith(".", StringComparison.Ordinal) == false)
            {
                this._databaseOwner += ".";
            }
        }

        public string NamePrefix
        {
            get { return this._databaseOwner + this._objectQualifier; }
        }

        public override int AddComment(
            int forumId, 
            int authorUserId, 
            string title, 
            string description, 
            string linkUrl, 
            string commentText, 
            int commentUserId, 
            string commentUserIpAddress)
        {
            Debug.Assert(authorUserId != Null.NullInteger);

            int threadId = -1;
            bool topicAlreadyCreated = false;
            var userController = new UserController();
            UserInfo authorInfo = userController.GetUser(this.PortalId, authorUserId);
            UserInfo commenterInfo = userController.GetUser(this.PortalId, commentUserId);
            string authorDisplayName = authorInfo != null ? authorInfo.DisplayName : string.Empty;
            string commenterDisplayName = commenterInfo != null ? commenterInfo.DisplayName : string.Empty;
            bool commenterIsSuperUser = commenterInfo != null ? commenterInfo.IsSuperUser : false;
            string body = string.Format(
                CultureInfo.InvariantCulture, Localization.GetString("PostBody", this._localResourceFile), description, linkUrl, title);

            using (var connection = new SqlConnection(this._connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    int? moduleId = null;
                    using (
                        SqlDataReader forumReader = SqlHelper.ExecuteReader(
                            this._connectionString, 
                            CommandType.StoredProcedure, 
                            this.NamePrefix + "ActiveForums_Forums_Get", 
                            Utility.CreateIntegerParam("@PortalId", this.PortalId), 
                            Utility.CreateIntegerParam("@ModuleId", Null.NullInteger), 
                            // currently, this stored procedure doesn't use moduleId
                            Utility.CreateIntegerParam("@ForumId", forumId)))
                    {
                        if (forumReader.Read())
                        {
                            moduleId = (int)forumReader["ModuleId"];
                        }
                    }

                    if (moduleId.HasValue)
                    {
                        var potentialMatchingTopicIds = new List<int>();
                        using (
                            SqlDataReader searchReader = SqlHelper.ExecuteReader(
                                this._connectionString, 
                                CommandType.StoredProcedure, 
                                this.NamePrefix + "ActiveForums_Search_Standard", 
                                Utility.CreateIntegerParam("@PortalId", this.PortalId), 
                                Utility.CreateIntegerParam("@ModuleId", moduleId), 
                                Utility.CreateIntegerParam("@UserId", authorUserId), 
                                Utility.CreateIntegerParam("@ForumId", forumId), 
                                Utility.CreateBitParam("@IsSuperUser", commenterIsSuperUser), 
                                Utility.CreateNvarcharParam("@SearchString", title, 200), 
                                Utility.CreateIntegerParam("@SearchField", 1), 
                                // 0=Subject&Body, 1= Subject, 2=Body
                                Utility.CreateNvarcharParam("@Author", string.Empty, 200), 
                                Utility.CreateVarcharParam("@Forums", string.Empty, 8000), 
                                Utility.CreateNvarcharParam("@Tags", string.Empty, 400)))
                        {
                            searchReader.NextResult();

                            while (searchReader.Read())
                            {
                                potentialMatchingTopicIds.Add((int)searchReader["TopicId"]);
                            }
                        }

                        foreach (int topicId in potentialMatchingTopicIds)
                        {
                            using (
                                SqlDataReader topicReader = SqlHelper.ExecuteReader(
                                    this._connectionString, 
                                    CommandType.StoredProcedure, 
                                    this.NamePrefix + "ActiveForums_Topics_Get", 
                                    Utility.CreateIntegerParam("@PortalId", this.PortalId), 
                                    Utility.CreateIntegerParam("@ModuleId", moduleId), 
                                    Utility.CreateIntegerParam("@TopicId", topicId), 
                                    Utility.CreateIntegerParam("@ForumId", forumId)))
                            {
                                if (topicReader.Read())
                                {
                                    topicAlreadyCreated = string.Equals(topicReader["Body"].ToString(), body, StringComparison.Ordinal);
                                    if (topicAlreadyCreated)
                                    {
                                        threadId = topicId;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (!topicAlreadyCreated)
                    {
                        // Create thread by author
                        threadId =
                            (int)
                            SqlHelper.ExecuteScalar(
                                transaction, 
                                CommandType.StoredProcedure, 
                                this.NamePrefix + "ActiveForums_Topics_Save", 
                                Utility.CreateIntegerParam("@TopicId", null), 
                                Utility.CreateIntegerParam("@ViewCount", 0), 
                                Utility.CreateIntegerParam("@ReplyCount", 0), 
                                Utility.CreateBitParam("@IsLocked", false), 
                                Utility.CreateBitParam("@IsPinned", false), 
                                Utility.CreateNvarcharParam("@TopicIcon", null, 25), 
                                Utility.CreateIntegerParam("@StatusId", Null.NullInteger), 
                                // 0=informative, 1=not resolved, 3=resolved
                                Utility.CreateBitParam("@IsApproved", true), 
                                Utility.CreateBitParam("@IsDeleted", false), 
                                Utility.CreateBitParam("@IsAnnounce", false), 
                                Utility.CreateBitParam("@IsArchived", false), 
                                Utility.CreateDateTimeParam("@AnnounceStart", this._nullDate), 
                                Utility.CreateDateTimeParam("@AnnounceEnd", this._nullDate), 
                                Utility.CreateNvarcharParam("@Subject", title, 255), 
                                Utility.CreateNtextParam("@Body", body), 
                                Utility.CreateDateTimeParam("@DateCreated", DateTime.Now), 
                                Utility.CreateDateTimeParam("@DateUpdated", DateTime.Now), 
                                Utility.CreateIntegerParam("@AuthorId", authorUserId), 
                                Utility.CreateNvarcharParam("@AuthorName", authorDisplayName, 150), 
                                Utility.CreateNvarcharParam("@IPAddress", string.Empty, 50), 
                                Utility.CreateIntegerParam("@TopicType", 0)); // 0=topic, 1=poll

                        // Add thread as a thread in the forum
                        SqlHelper.ExecuteNonQuery(
                            transaction, 
                            CommandType.StoredProcedure, 
                            this.NamePrefix + "ActiveForums_Topics_SaveToForum", 
                            Utility.CreateIntegerParam("@ForumID", forumId), 
                            Utility.CreateIntegerParam("@TopicID", threadId), 
                            Utility.CreateIntegerParam("@LastReplyID", Null.NullInteger));

                        // TODO: Send update to forum subscribers
                    }

                    Debug.Assert(threadId != -1);

                    // Create comment reply
                    SqlHelper.ExecuteNonQuery(
                        transaction, 
                        CommandType.StoredProcedure, 
                        this.NamePrefix + "ActiveForums_Reply_Save", 
                        Utility.CreateIntegerParam("@TopicId", threadId), 
                        Utility.CreateIntegerParam("@ReplyId", Null.NullInteger), 
                        Utility.CreateIntegerParam("@ReplyToId", threadId), 
                        Utility.CreateIntegerParam("@StatusId", Null.NullInteger), 
                        // 0=informative, 1=not resolved, 3=resolved
                        Utility.CreateBitParam("@IsApproved", true), 
                        Utility.CreateBitParam("@IsDeleted", false), 
                        Utility.CreateNvarcharParam("@Subject", "Re: " + title, 255), 
                        Utility.CreateNtextParam("@Body", commentText), 
                        Utility.CreateDateTimeParam("@DateCreated", DateTime.Now), 
                        Utility.CreateDateTimeParam("@DateUpdated", DateTime.Now), 
                        Utility.CreateIntegerParam("@AuthorId", commentUserId), 
                        Utility.CreateNvarcharParam("@AuthorName", commenterDisplayName, 150), 
                        Utility.CreateNvarcharParam("@IPAddress", commentUserIpAddress, 50));

                    transaction.Commit();
                }

                return threadId;
            }
        }

        public override Dictionary<int, string> GetForums()
        {
            var forums = new Dictionary<int, string>();
            foreach (
                ModuleInfo activeForumsModule in
                    new ModuleController().GetModulesByDefinition(this.PortalId, Utility.ActiveForumsDefinitionModuleName))
            {
                using (
                    IDataReader forumsReader = SqlHelper.ExecuteReader(
                        this._connectionString, 
                        CommandType.StoredProcedure, 
                        this.NamePrefix + "ActiveForums_Forums_List", 
                        Utility.CreateIntegerParam("@ModuleId", activeForumsModule.ModuleID), 
                        Utility.CreateIntegerParam("@ForumGroupId", Null.NullInteger), 
                        Utility.CreateIntegerParam("@ParentForumId", Null.NullInteger), 
                        Utility.CreateBitParam("@FillLastPost", false)))
                {
                    while (forumsReader.Read())
                    {
                        forums.Add((int)forumsReader["ForumId"], forumsReader["ForumName"].ToString());
                    }
                }
            }

            return forums;
        }

        public override string GetThreadUrl(int threadId)
        {
            int? forumId = null;
            int? moduleId = null;
            using (
                SqlDataReader postReader = SqlHelper.ExecuteReader(
                    this._connectionString, 
                    CommandType.Text, 
                    string.Format(
                        CultureInfo.InvariantCulture, 
                        "SELECT ForumId, ModuleId FROM {0}vw_activeforums_ForumTopics WHERE TopicId = @TopicId", 
                        this.NamePrefix), 
                    Utility.CreateIntegerParam("@TopicId", threadId)))
            {
                if (postReader.Read())
                {
                    forumId = (int)postReader["ForumId"];
                    moduleId = (int)postReader["ModuleId"];
                }
            }

            if (forumId.HasValue)
            {
                ModuleInfo module = new ModuleController().GetModule(moduleId.Value, Null.NullInteger);
                return Globals.NavigateURL(
                    module.TabID, 
                    string.Empty, 
                    "aff=" + forumId.Value.ToString(CultureInfo.InvariantCulture), 
                    "aft=" + threadId.ToString(CultureInfo.InvariantCulture), 
                    "afv=topic");
            }

            return string.Empty;
        }
    }
}