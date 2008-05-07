using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using Engage.Dnn.Publish.Data;
using Engage.Dnn.Publish.Util;
using System.Xml.Serialization;

namespace Engage.Dnn.Publish
{
    /// <summary>
    /// A comment on an item, possibly tied to a <see cref="Rating"/>.
    /// </summary>
    [XmlRootAttribute(ElementName = "Comment", IsNullable = false)]
    public class Comment
    {
        /// <summary>
        /// The size of the database fields which hold the first and last names of the commenter.
        /// </summary>
        public const int NameSizeLimit = 50;
        /// <summary>
        /// The size of the database field which holds the email address of the commenter.
        /// </summary>
        public const int EmailAddressSizeLimit = 256;
        public const int UrlSizeLimit = 255;

        #region Private Variables
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _commentId;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _itemVersionId;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int? _userId;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _commentText;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int _approvalStatusId;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private DateTime _createdDate;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private DateTime _lastUpdated;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int? _ratingId;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _firstName;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _lastName;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _emailAddress;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _url;

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the id of the comment.
        /// </summary>
        /// <value>The comment id.</value>
        public int CommentId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return _commentId;
            }
        }
        /// <summary>
        /// Gets or sets the item version id.
        /// </summary>
        /// <value>The item version id.</value>
        public int ItemVersionId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return _itemVersionId;
            }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set
            {
                _itemVersionId = value;
            }
        }
        /// <summary>
        /// Gets the item id.
        /// </summary>
        /// <value>The item id.</value>
        public int ItemId
        {
            get
            {
                return Item.GetItemIdFromVersion(_itemVersionId);
            }
        }
        /// <summary>
        /// Gets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public int? UserId
        {
         [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return _userId;
            }
        }
        /// <summary>
        /// Gets or sets the comment text.
        /// </summary>
        /// <value>The comment text.</value>
        public string CommentText
        {
         [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return _commentText;
            }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set
            {
                _commentText = value;
            }
        }
        /// <summary>
        /// Gets or sets the approval status id.
        /// </summary>
        /// <value>The approval status id.</value>
        public int ApprovalStatusId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return _approvalStatusId;
            }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set
            {
                _approvalStatusId = value;
            }
        }
        /// <summary>
        /// Gets the created date.
        /// </summary>
        /// <value>The created date.</value>
        public DateTime CreatedDate
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return _createdDate;
            }
        }
        /// <summary>
        /// Gets the date this comment was last updated.
        /// </summary>
        /// <value>The date this comment was last updated.</value>
        public DateTime LastUpdated
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return _lastUpdated;
            }
        }
        /// <summary>
        /// This is not currently implemented. Gets the rating id of the <see cref="Rating"/> associated with this Comment.  This is not currently implemented.
        /// </summary>
        /// <value>The rating id.</value>
        [Obsolete("This is not currently implemented, there is no way to associate a rating with a comment")]
        public int? RatingId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return _ratingId;
            }
        }
        /// <summary>
        /// Gets or sets the first name of the commenter.  Truncates the value if it is longer than <see cref="NameSizeLimit"/>.
        /// </summary>
        /// <value>The commenter's first name.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Compiler doesn't see validation.")]
        public string FirstName
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return _firstName;
            }
            set
            {
                if (String.IsNullOrEmpty(value) || value.Length <= NameSizeLimit)
                {
                    _firstName = value;
                }
                else
                {
                    _firstName = value.Substring(0, NameSizeLimit);
                }
            }
        }
        /// <summary>
        /// Gets or sets the last name of the commenter.  Truncates the value if it is longer than <see cref="NameSizeLimit"/>.
        /// </summary>
        /// <value>The commenter's last name.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Compiler doesn't see validation.")]
        public string LastName
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return _lastName;
            }
            set
            {
                if (String.IsNullOrEmpty(value) || value.Length <= NameSizeLimit)
                {
                    _lastName = value;
                }
                else
                {
                    _lastName = value.Substring(0, NameSizeLimit);
                }
            }
        }

        /// <summary>
        /// Gets or sets the email address of the commenter.  Truncates the value if it is longer than <see cref="EmailAddressSizeLimit"/>.
        /// </summary>
        /// <value>The commenter's email address.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Compiler doesn't see validation.")]
        public string EmailAddress
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return _emailAddress;
            }
            set
            {
                if (String.IsNullOrEmpty(value) || value.Length <= EmailAddressSizeLimit)
                {
                    _emailAddress = value;
                }
                else
                {
                    _emailAddress = value.Substring(0, EmailAddressSizeLimit);
                }
            }
        }

        /// <summary>
        /// Gets or sets the URL of the commenter.  Truncates the value if it is longer than <see cref="UrlSizeLimit"/>.
        /// </summary>
        /// <value>The commenter's url.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Compiler doesn't see validation.")]
        public string Url
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return _url;
            }
            set
            {
                if (String.IsNullOrEmpty(value) || value.Length <= UrlSizeLimit)
                {
                    _url = value;
                }
                else
                {
                    _url = value.Substring(0, UrlSizeLimit);
                }
            }
        }
        #endregion

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        /// <param name="commentId">The comment id.</param>
        /// <param name="itemVersionId">The item version id.</param>
        /// <param name="itemId">The item id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="commentText">The comment text.</param>
        /// <param name="approvalStatusId">The approval status id.</param>
        /// <param name="createdDate">The created date.</param>
        /// <param name="lastUpdated">The last updated.</param>
        /// <param name="ratingId">This is not currently implemented. The id of the <see cref="Rating"/> associated with this <see cref="Comment"/>.</param>
        /// <param name="firstName">First name of the commenter.  Will be truncated if longer than <see cref="NameSizeLimit"/>.</param>
        /// <param name="lastName">Last name of the commenter.  Will be truncated if longer than <see cref="NameSizeLimit"/>.</param>
        /// <param name="emailAddress">Email address of the commenter.  Will be truncated if longer than <see cref="EmailAddressSizeLimit"/>.</param>
        private Comment(int commentId, int itemVersionId, int? userId, string commentText, int approvalStatusId, DateTime createdDate, DateTime lastUpdated, int? ratingId, string firstName, string lastName, string emailAddress)
        {
            _commentId = commentId;
            _itemVersionId = itemVersionId;
            _userId = userId;
            _commentText = commentText;
            _approvalStatusId = approvalStatusId;
            _createdDate = createdDate;
            _lastUpdated = lastUpdated;
            _ratingId = ratingId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.EmailAddress = emailAddress;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        /// <param name="itemVersionId">The item version id.</param>
        /// <param name="userId">The user id, or <c>null</c> is the user is unauthenticated.</param>
        /// <param name="commentText">The comment itself.</param>
        /// <param name="approvalStatusId">The approval status id.</param>
        /// <param name="ratingId">This is not currently implemented. The rating id, or <c>null</c> if the <see cref="Comment"/> is not associated with a <see cref="Rating"/>. This is not currently implemented.</param>
        /// <param name="firstName">First name of the commenter.  Will be truncated if longer than <see cref="NameSizeLimit"/>.</param>
        /// <param name="lastName">Last name of the commenter.  Will be truncated if longer than <see cref="NameSizeLimit"/>.</param>
        /// <param name="emailAddress">Email address of the commenter.  Will be truncated if longer than <see cref="EmailAddressSizeLimit"/>.</param>
        public Comment(int itemVersionId, int? userId, string commentText, int approvalStatusId, int? ratingId, string firstName, string lastName, string emailAddress)
        {
            _itemVersionId = itemVersionId;
            _userId = userId;
            _commentText = commentText;
            _approvalStatusId = approvalStatusId;
            _ratingId = ratingId;
            _createdDate = DateTime.Now;
            _lastUpdated = _createdDate;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.EmailAddress = emailAddress;
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
        /// Adds a comment to an <see cref="Item"/>.
        /// </summary>
        /// <param name="itemVersionId">The item version id.</param>
        /// <param name="userId">The user id, or <c>null</c> if the user in unauthenticated.</param>
        /// <param name="commentText">The comment text.</param>
        /// <param name="approvalStatusId">The approval status id.</param>
        /// <param name="ratingId">The id of the associated <see cref="Rating"/>, or <c>null</c> if there is none.</param>
        /// <param name="firstName">First name of the commenter, will be truncated if longer than <see cref="NameSizeLimit"/>.</param>
        /// <param name="lastName">Last name of the commenter, will be truncated if longer than <see cref="NameSizeLimit"/>.</param>
        /// <param name="emailAddress">Email address of the commenter, will be truncated if longer than <see cref="EmailAddressSizeLimit"/>.</param>
        /// <param name="url">URL entered by the commenter, will be truncated if longer than <see cref="UrlSizeLimit"/>.</param>
        /// <returns></returns>
        public static int AddComment(int itemVersionId, int? userId, string commentText, int approvalStatusId, int? ratingId, string firstName, string lastName, string emailAddress, string url)
        {
            if (firstName.Length > NameSizeLimit)
            {
                firstName = firstName.Substring(0, NameSizeLimit);
            }
            if (lastName.Length > NameSizeLimit)
            {
                lastName = lastName.Substring(0, NameSizeLimit);
            }
            if (emailAddress.Length > EmailAddressSizeLimit)
            {
                emailAddress = emailAddress.Substring(0, EmailAddressSizeLimit);
            }
            return DataProvider.Instance().AddComment(itemVersionId, userId, commentText, approvalStatusId, ratingId, firstName, lastName, emailAddress, url);
        }

        /// <summary>
        /// Updates an existing <see cref="Comment"/>.
        /// </summary>
        /// <param name="commentId">The id of the comment to edit.</param>
        /// <param name="commentText">The comment text.</param>
        /// <param name="approvalStatusId">The approval status id.</param>
        /// <param name="firstName">First name of the commenter, will be truncated if longer than <see cref="NameSizeLimit"/>.</param>
        /// <param name="lastName">Last name of the commenter, will be truncated if longer than <see cref="NameSizeLimit"/>.</param>
        /// <param name="emailAddress">Email address of the commenter, will be truncated if longer than <see cref="EmailAddressSizeLimit"/>.</param>
        /// <param name="url">URL entered by the commenter, will be truncated if longer than <see cref="UrlSizeLimit"/>.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Compiler doesn't see validation")]
        public static void UpdateComment(int commentId, string commentText, int approvalStatusId, string firstName, string lastName, string emailAddress, string url)
        {
            if (!String.IsNullOrEmpty(firstName) && firstName.Length > NameSizeLimit)
            {
                firstName = firstName.Substring(0, NameSizeLimit);
            }
            if (!String.IsNullOrEmpty(lastName) && lastName.Length > NameSizeLimit)
            {
                lastName = lastName.Substring(0, NameSizeLimit);
            }
            if (!String.IsNullOrEmpty(emailAddress) && emailAddress.Length > EmailAddressSizeLimit)
            {
                emailAddress = emailAddress.Substring(0, EmailAddressSizeLimit);
            }
            DataProvider.Instance().UpdateComment(commentId, commentText, approvalStatusId, firstName, lastName, emailAddress, url);
        }

        /// <summary>
        /// Gets all comments for an <see cref="Item"/> of a particular <see cref="ApprovalStatus"/>.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <param name="approvalStatusId">The approval status id.</param>
        /// <returns>A <see cref="List{Comment}"/> filled with all Comments of the specified <see cref="ApprovalStatus"/> for the specified <see cref="Item"/></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Not intended to be a reusable library.")]
        public static List<Comment> GetComments(int itemId, int approvalStatusId)
        {
            IDataReader dr = DataProvider.Instance().GetComments(itemId, approvalStatusId);
            List<Comment> comments = new List<Comment>();

            while (dr.Read())
            {
                comments.Add(FillComment(dr));
            }

            return comments;
        }

        /// <summary>
        /// Gets the comment the the specified Id.
        /// </summary>
        /// <param name="commentId">The comment id.</param>
        /// <returns>The requested <see cref="Comment"/>.</returns>
        public static Comment GetComment(int commentId)
        {
            IDataReader dr = DataProvider.Instance().GetComment(commentId);

            if (dr.Read())
            {
                return FillComment(dr);
            }
            return null;
        }

        /// <summary>
        /// Fills a <see cref="Comment"/> from an <see cref="IDataReader"/>.
        /// </summary>
        /// <param name="dr">An <see cref="IDataReader"/> filled with information needed to fill a <see cref="Comment"/>.</param>
        /// <returns>The instantiated <see cref="Comment"/>.</returns>
        private static Comment FillComment(IDataReader dr)
        {
            int? userId = null;
            int? ratingId = null;
            string firstName = null;
            string lastName = null;
            string emailAddress = null;

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

            return new Comment((int)dr["commentId"], (int)dr["itemVersionId"], userId, (string)dr["commentText"], (int)dr["approvalStatusId"], Convert.ToDateTime(dr["createdDate"], CultureInfo.InvariantCulture), Convert.ToDateTime(dr["lastUpdated"], CultureInfo.InvariantCulture), ratingId, firstName, lastName, emailAddress);
        }
	    #endregion

        #region Instance Methods
        /// <summary>
        /// Deletes this instance.
        /// </summary>
		public void Delete()
        {
            DataProvider.Instance().DeleteComment(this.CommentId);
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            Comment.UpdateComment(this.CommentId, this.CommentText, this.ApprovalStatusId, this.FirstName, this.LastName, this.EmailAddress, this.Url);
        }
	    #endregion
    }
}