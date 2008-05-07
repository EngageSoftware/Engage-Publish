using System;
using System.Data;
using System.Globalization;
using Engage.Dnn.Publish.Data;

namespace Engage.Dnn.Publish
{
    public class Rating
    {
        public const int DefaultMaximumRating = 5;

        #region Private Variables
        //attributes hide private members from debugger, so both properties and members aren't shown - BD
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int? userId;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int rating;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int itemVersionId;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private DateTime lastUpdated;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private DateTime createdDate;
        
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the user id of the rater.
        /// </summary>
        /// <value>The user id of the rater, or <c>null</c> if the rater is unauthenticated.</value>
        public int? UserId
        {
         [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return userId;
            }
        }
        /// <summary>
        /// Gets or sets the numerical rating contained in this <see cref="Rating"/> instance.
        /// </summary>
        /// <value>The rating.</value>
        public int RatingValue
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return rating;
            }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set
            {
                rating = value;
            }
        }
        /// <summary>
        /// Gets the id of rated item.
        /// </summary>
        /// <value>The rated item's id.</value>
        public int ItemId
        {
            get
            {
                return Item.GetItemIdFromVersion(itemVersionId);
            }
        }
        /// <summary>
        /// Gets or sets the itemVersionId of the rated item.
        /// </summary>
        /// <value>The itemVersionId.</value>
        public int ItemVersionId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return itemVersionId;
            }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set
            {
                itemVersionId = value;
            }
        }
        /// <summary>
        /// Gets the date that this rating was last updated.
        /// </summary>
        /// <value>The date that this rating was last updated.</value>
        public DateTime LastUpdated
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return lastUpdated;
            }
        }
        /// <summary>
        /// Gets the date this rating was created.
        /// </summary>
        /// <value>The creation date of this rating.</value>
        public DateTime CreatedDate
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return createdDate;
            }
        }
        
        #endregion

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="Rating"/> class.
        /// </summary>
        public Rating()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rating"/> class.
        /// </summary>
        /// <param name="ratingUserId">The user id, or <c>null</c> if the user is unauthenticated.</param>
        /// <param name="ratingItemVersionId">The item version id.</param>
        /// <param name="ratingValue">The rating.</param>
        public Rating(int? ratingUserId, int ratingItemVersionId, int ratingValue)
        {
            userId = ratingUserId;
            itemVersionId = ratingItemVersionId;
            rating = ratingValue;
            createdDate = DateTime.Now;
            lastUpdated = createdDate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rating"/> class.
        /// </summary>
        /// <param name="ratingUserId">The user id, or <c>null</c> if the user is unauthenticated.</param>
        /// <param name="ratingItemVersionId">The item version id.</param>
        /// <param name="ratingValue">The rating.</param>
        /// <param name="ratingCreatedDate">The created date.</param>
        /// <param name="ratingLastUpdated">The last updated date.</param>
        private Rating(int? ratingUserId, int ratingItemVersionId, int ratingValue, DateTime ratingCreatedDate, DateTime ratingLastUpdated)
        {
            userId = ratingUserId;
            itemVersionId = ratingItemVersionId;
            rating = ratingValue;
            createdDate = ratingCreatedDate;
            lastUpdated = ratingLastUpdated;
        }
        
        #endregion

        #region Static Data Methods
		/// <summary>
        /// Adds a rating to an article.
        /// </summary>
        /// <param name="itemVersionId">The item version id.</param>
        /// <param name="userId">The user id, or <c>null</c> if the user is unauthenticated.</param>
        /// <param name="rating">The rating.</param>
        /// <returns></returns>
        public static int AddRating(int itemVersionId, int? userId, int rating)
        {
            return DataProvider.Instance().AddRating(itemVersionId, userId, rating);
        }

        /// <summary>
        /// Deletes all the ratings for a certain item version.
        /// </summary>
        /// <param name="itemVersionId">The item version id.</param>
        public static void DeleteRatings(int itemVersionId)
        {
            DataProvider.Instance().DeleteRatings(itemVersionId);
        }

        /// <summary>
        /// Gets a user's rating for a specific item.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>The rating.</returns>
        public static Rating GetRating(int itemId, int userId)
        {
	        IDataReader dr = DataProvider.Instance().GetRating(itemId, userId);

            try 
	        {	        
                if (dr.Read())
                {
                    return new Rating(userId, (int)dr["itemVersionId"], (int)dr["rating"], Convert.ToDateTime(dr["createdDate"], CultureInfo.InvariantCulture), Convert.ToDateTime(dr["lastUpdated"], CultureInfo.InvariantCulture));
                }
	        }
	        finally
	        {
                dr.Close();
	        }   
            return null;
        }

        /// <summary>
        /// Updates an existing rating.
        /// </summary>
        /// <param name="itemId">The id of the rated item.</param>
        /// <param name="userId">The user id of the rater.</param>
        /// <param name="rating">The rating.</param>
        public static void UpdateRating(int itemId, int userId, int rating)
        {
            DataProvider.Instance().UpdateRating(itemId, userId, rating);
        }

        /// <summary>
        /// Updates an existing rating.
        /// </summary>
        /// <param name="ratingId">The rating id.</param>
        /// <param name="rating">The rating.</param>
        public static void UpdateRating(int ratingId, int rating)
        {
            DataProvider.Instance().UpdateRating(ratingId, rating);
        }
	#endregion    
    }
}