using Engage.Dnn.Publish.Data;

namespace Engage.Dnn.Publish
{
    public class Rating : UserFeedback.Rating
    {
        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="Rating"/> class.
        /// </summary>
        public Rating()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Rating"/> class.
        /// </summary>
        /// <param name="ratingUserId">The user id, or <c>null</c> if the user is unauthenticated.</param>
        /// <param name="ratingItemVersionId">The item version id.</param>
        /// <param name="ratingValue">The rating.</param>
        public Rating(int? ratingUserId, int ratingItemVersionId, int ratingValue)
            : base(ratingUserId, ratingItemVersionId, ratingValue)
        {
        }

        #endregion

        #region Static Data Methods
        /// <summary>
        /// Gets a user's rating for a specific item.
        /// </summary>
        /// <param name="itemId">The item id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="portalId">The portal id.</param>
        /// <param name="itemTypeId">The item type id.</param>
        /// <returns>The rating.</returns>
        public static UserFeedback.Rating GetRating(int itemId, int userId, int portalId, int itemTypeId)
        {
            return GetRating(Item.GetItem(itemId, portalId, itemTypeId, true).ItemVersionId, userId, DataProvider.ModuleQualifier);
        }

        /// <summary>
        /// Updates an existing rating.
        /// </summary>
        /// <param name="itemId">The id of the rated item.</param>
        /// <param name="userId">The user id of the rater.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="portalId">The portal id.</param>
        /// <param name="itemTypeId">The item type id.</param>
        public static void UpdateRating(int itemId, int userId, int rating, int portalId, int itemTypeId)
        {
            UpdateRating(Item.GetItem(itemId, portalId, itemTypeId, true).ItemVersionId, userId, rating, DataProvider.ModuleQualifier);
        }
	#endregion    
    }
}