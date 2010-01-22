//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
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
    using DotNetNuke.Framework;

    /// <summary>
    /// Defines the relationship between Publish and a forums module used to create discussion threads for articles (in place of comments).
    /// Inheritors of <see cref="ForumProvider"/> must provide a constructor that takes a portalId as an <see cref="int"/> parameter.
    /// </summary>
    public abstract class ForumProvider
    {
        #region  Static Members

        private static readonly Dictionary<int, ForumProvider> Providers = new Dictionary<int, ForumProvider>();

        /// <summary>
        /// Gets the <see cref="ForumProvider"/> instance for this <paramref name="portalId"/>.
        /// </summary>
        /// <param name="portalId">The portal id.</param>
        /// <returns>A concrete representation of the given portal's <see cref="ForumProvider"/></returns>
        public static ForumProvider GetInstance(int portalId)
        {
            if (!Providers.ContainsKey(portalId))
            {
                Type concreteType = Reflection.CreateType(ModuleBase.ForumProviderTypeForPortal(portalId));
                    Providers.Add(portalId, concreteType != null ?
                        (ForumProvider)concreteType.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, new object[] {portalId}, null) :
                        null);
            }
            return Providers[portalId];
        }

        #endregion

        #region Instance Data

        private readonly int _portalId;
        protected int PortalId
        {
            get { return _portalId; }
        }

        #endregion

        #region Constructors

        protected ForumProvider(int portalId)
        {
            _portalId = portalId;
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Adds a comment to the given forum as a new thread with the article description being the first post, and the comment being a reply.
        /// </summary>
        /// <param name="forumId">The ID of the forum in which to post the new thread.</param>
        /// <param name="authorUserId">The user ID of the author of the article.</param>
        /// <param name="title">The title of the article, to be used as the main body of the initial post in the thread.</param>
        /// <param name="description">The description of the article, to be used as the main body of the initial post in the thread.</param>
        /// <param name="linkUrl">A URL pointing to the article.</param>
        /// <param name="commentText">The comment text.</param>
        /// <param name="commentUserId">The user ID of the person creating the comment.</param>
        /// <param name="commentUserIpAddress">The IP address of the user posting a comment.</param>
        /// <returns>The ID of the created forum thread</returns>
        public abstract int AddComment(int forumId, int authorUserId, string title, string description, string linkUrl, string commentText, int commentUserId, string commentUserIpAddress);

        /// <summary>
        /// Gets a URL to the forum thread which holds discussions for an article.
        /// </summary>
        /// <param name="threadId">The ID of the requested forum thread.</param>
        /// <returns>A URL pointing to the forum thread with ID <paramref name="threadId"/>.</returns>
        public abstract string GetThreadUrl(int threadId);

        /// <summary>
        /// Gets a list of the forums for this <see cref="ForumProvider"/>'s portal in a dictionary where the key is the forum ID and the value is the forum name.
        /// </summary>
        /// <returns>A Dictionary with each forum for this portal, relating the forum's ID to its name.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public abstract Dictionary<int, string> GetForums();

        #endregion
    }
}
