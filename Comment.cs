//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.


namespace Engage.Dnn.Publish
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Xml.Serialization;
    using DotNetNuke.Common.Utilities;
    using Data;
    using Util;
    /// <summary>
    /// A comment on an item, possibly tied to a <see cref="Rating"/>.
    /// </summary>
    [XmlRootAttribute(ElementName = "Comment", IsNullable = false)]
    public class Comment : UserFeedback.Comment
    {
        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        /// <param name="commentId">The comment id.</param>
        /// <param name="itemVersionId">The item version id.</param>
        /// <param name="itemVersionId">The item version id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="commentText">The comment text.</param>
        /// <param name="approvalStatusId">The approval status id.</param>
        /// <param name="createdDate">The created date.</param>
        /// <param name="lastUpdated">The last updated.</param>
        /// <param name="ratingId">This is not currently implemented. The id of the <see cref="Rating"/> associated with this <see cref="Comment"/>.</param>
        /// <param name="firstName">First name of the commenter.  Will be truncated if longer than <see cref="UserFeedback.Comment.NameSizeLimit"/>.</param>
        /// <param name="lastName">Last name of the commenter.  Will be truncated if longer than <see cref="UserFeedback.Comment.NameSizeLimit"/>.</param>
        /// <param name="emailAddress">Email address of the commenter.  Will be truncated if longer than <see cref="UserFeedback.Comment.EmailAddressSizeLimit"/>.</param>
        /// <param name="url">URL of the commenter</param>
        private Comment(int commentId, int itemVersionId, int? userId, string commentText, int approvalStatusId, DateTime createdDate, DateTime lastUpdated, int? ratingId, string firstName, string lastName, string emailAddress, string url)
            : base(commentId, itemVersionId, userId, commentText, approvalStatusId, createdDate, lastUpdated, ratingId, firstName,  lastName, emailAddress, url)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        /// <param name="itemVersionId">The item version id.</param>
        /// <param name="userId">The user id, or <c>null</c> is the user is unauthenticated.</param>
        /// <param name="commentText">The comment itself.</param>
        /// <param name="approvalStatusId">The approval status id.</param>
        /// <param name="ratingId">This is not currently implemented. The rating id, or <c>null</c> if the <see cref="Comment"/> is not associated with a <see cref="Rating"/>. This is not currently implemented.</param>
        /// <param name="firstName">First name of the commenter.  Will be truncated if longer than <see cref="UserFeedback.Comment.NameSizeLimit"/>.</param>
        /// <param name="lastName">Last name of the commenter.  Will be truncated if longer than <see cref="UserFeedback.Comment.NameSizeLimit"/>.</param>
        /// <param name="emailAddress">Email address of the commenter.  Will be truncated if longer than <see cref="UserFeedback.Comment.EmailAddressSizeLimit"/>.</param>
        /// <param name="url">URL of the commenter</param>
        public Comment(int itemVersionId, int? userId, string commentText, int approvalStatusId, int? ratingId, string firstName, string lastName, string emailAddress, string url)
            : base(itemVersionId, userId, commentText, approvalStatusId, ratingId, firstName, lastName, emailAddress, url)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        public Comment()
        {
        }
        
        #endregion

        #region Static Data Methods
        /// <summary>
        /// Gets all comments for an <see cref="Item"/> of a particular <see cref="ApprovalStatus"/>.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <param name="approvalStatusId">The approval status id.</param>
        /// <returns>A <see cref="List{Comment}"/> filled with all Comments of the specified <see cref="ApprovalStatus"/> for the specified <see cref="Item"/></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Not intended to be a reusable library.")]
        public static List<UserFeedback.Comment> GetCommentsByItemId(int itemId, int approvalStatusId)
        {
            IDataReader dr = DataProvider.Instance().GetComments(itemId, approvalStatusId);
            var comments = new List<UserFeedback.Comment>();

            while (dr.Read())
            {
                comments.Add(FillComment(dr));
            }

            return comments;
        }

        /// <summary>
        /// Fills a <see cref="Comment"/> from an <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="dr">An <see cref="IDataReader"/> filled with information needed to fill a <see cref="Comment"/>.</param>
        /// <returns>The instantiated <see cref="Comment"/>.</returns>
        private static UserFeedback.Comment FillComment(IDataRecord dr)
        {
            int? userId = null;
            int? ratingId = null;
            string firstName = null;
            string lastName = null;
            string emailAddress = null;
            string url = null;

            if (!dr.IsDBNull(dr.GetOrdinal("userId")))
            {
                userId = (int)dr["userId"];
            }
            if (!dr.IsDBNull(dr.GetOrdinal("ratingId")))
            {
                ratingId = (int)dr["ratingId"];
            }
            if (!dr.IsDBNull(dr.GetOrdinal("firstName")))
            {
                firstName = dr["firstName"].ToString();
            }
            if (!dr.IsDBNull(dr.GetOrdinal("lastName")))
            {
                lastName = dr["lastName"].ToString();
            }
            if (!dr.IsDBNull(dr.GetOrdinal("emailAddress")))
            {
                emailAddress = dr["emailAddress"].ToString();
            }

            if (!dr.IsDBNull(dr.GetOrdinal("url")))
            {
                url = dr["url"].ToString();
            }

            return new Comment((int)dr["commentId"], (int)dr["itemVersionId"], userId, (string)dr["commentText"], (int)dr["approvalStatusId"], Convert.ToDateTime(dr["createdDate"], CultureInfo.InvariantCulture), Convert.ToDateTime(dr["lastUpdated"], CultureInfo.InvariantCulture), ratingId, firstName, lastName, emailAddress, url);
        }


        public static int CommentsWaitingForApprovalCount(int portalId, int authorUserId)
        {
            //cache this
            int commentCount;
            string cacheKey = Utility.CacheKeyPublishAuthorCommentCount + authorUserId.ToString(CultureInfo.InvariantCulture) + "_" + portalId;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    commentCount = (int)o;
                }
                else
                {
                    commentCount = DataProvider.Instance().CommentsWaitingForApprovalCount(portalId, authorUserId);
                }

                DataCache.SetCache(cacheKey, commentCount, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                Utility.AddCacheKey(cacheKey, portalId);

            }
            else
            {
                commentCount = DataProvider.Instance().CommentsWaitingForApprovalCount(portalId, authorUserId);
            }

            return commentCount;

        }


	    #endregion

        #region Instance Methods
        /// <summary>
        /// Deletes this instance.
        /// </summary>
		public void Delete()
        {
            Delete(DataProvider.ModuleQualifier);
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
           Save(DataProvider.ModuleQualifier);
        }
	    #endregion
    }
}