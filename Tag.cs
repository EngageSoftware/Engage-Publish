// <copyright file="Tag.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Globalization;
    using System.Text;
    using System.Web;
    using System.Xml.Serialization;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Util;

    [XmlRoot(ElementName = "Tag", IsNullable = false)]
    public class Tag
    {
        // attributes hide private members from debugger, so both properties and members aren't shown - BD

        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class.
        /// </summary>
        public Tag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class.
        /// </summary>
        /// <param name="tagName">The tag name.</param>
        /// <param name="tagDescription">The tag description.</param>
        /// <param name="tagTotalItems">The total items.</param>
        public Tag(string tagName, string tagDescription, int tagTotalItems)
        {
            this.Name = tagName;
            this.Description = tagDescription;
            this.TotalItems = tagTotalItems;
            this.MostRecentDate = DateTime.Now;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class.
        /// </summary>
        /// <param name="dt">Data table of the tag instance info.</param>
        public Tag(DataTable dt)
        {
            // Make sure that we have at least 1 row returned. If for some reason we have more than one we only use the first one.
            if (dt.Rows.Count < 1)
            {
                return;
            }

            var row = dt.Rows[0];
            this.TagId = Convert.ToInt32(row["tagId"], CultureInfo.InvariantCulture);
            this.Name = row["name"].ToString();
            this.Description = row["description"].ToString();
            this.TotalItems = Convert.ToInt32(row["totalItems"], CultureInfo.InvariantCulture);
            this.MostRecentDate = Convert.ToDateTime(row["mostRecentDate"], CultureInfo.InvariantCulture);
            this.LanguageId = Convert.ToInt32(row["languageid"], CultureInfo.InvariantCulture);
            this.CreatedDate = Convert.ToDateTime(row["datecreated"], CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets the date this tag was created.
        /// </summary>
        /// <value>The creation date of this rating.</value>
        public DateTime CreatedDate { get; private set; }

        /// <summary>
        /// Gets or sets the tag description for this <see cref="Tag"/> instance.
        /// </summary>
        /// <value>The description of the Tag.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the language id for this <see cref="Tag"/> instance.
        /// </summary>
        /// <value>The Language Id</value>
        public int LanguageId { get; set; }

        /// <summary>
        /// Gets the date an item was last tagged.
        /// </summary>
        /// <value>The creation date of this rating.</value>
        public DateTime MostRecentDate { get; private set; }

        /// <summary>
        /// Gets or sets the tag name for this <see cref="Tag"/> instance.
        /// </summary>
        /// <value>The Name of the Tag.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the PortalId this <see cref="Tag"/> instance.
        /// </summary>
        /// <value>The Language Id</value>
        public int PortalId { get; set; }

        /// <summary>
        /// Gets the _tagId of the tag
        /// </summary>
        /// <value>The tag id of the tag.</value>
        public int TagId { get; private set; }

        /// <summary>
        /// Gets or sets the total number of items tagged with this <see cref="Tag"/> instance.
        /// </summary>
        /// <value>The Total Items Tag.</value>
        public int TotalItems { get; set; }

        public static DataTable GetItemsFromTags(int portalId, ArrayList tagList)
        {
            var tagIds = new StringBuilder(50);

            if (tagList != null)
            {
                foreach (int tag in tagList)
                {
                    tagIds.Append(tag.ToString());
                    tagIds.Append("_");
                }
            }

            var cacheKey = string.Format(
                CultureInfo.InvariantCulture, 
                "{0}{1}_{2}", 
                Utility.CacheKeyPublishItemsFromTags, 
                tagIds, 
                portalId);

            return Utility.GetValueFromCache(portalId, cacheKey, () => DataProvider.Instance().GetItemsFromTags(portalId, tagList));
        }

        public static DataTable GetItemsFromTagsPaging(int portalId, ArrayList tagList, int maxItems, int pageId, string sortOrder)
        {
            var tagIds = new StringBuilder(50);

            if (tagList != null)
            {
                foreach (int tag in tagList)
                {
                    tagIds.Append(tag.ToString());
                    tagIds.Append("_");
                }
            }

            var cacheKey = string.Format(
                CultureInfo.InvariantCulture, 
                "{0}{1}_{2}_{3}", 
                Utility.CacheKeyPublishItemsFromTagsPage, 
                tagIds, 
                pageId, 
                portalId);

            return Utility.GetValueFromCache(
                portalId, 
                cacheKey, 
                () => DataProvider.Instance().GetItemsFromTagsPaging(portalId, tagList, maxItems, pageId, sortOrder));
        }

        public static DataTable GetPopularTags(int portalId, ArrayList tagList, bool selectTop)
        {
            // TODO: change tagList to a <List> of strings
            var tagIds = new StringBuilder(50);

            if (tagList != null)
            {
                foreach (int tag in tagList)
                {
                    tagIds.Append(tag.ToString());
                    tagIds.Append("_");
                }
            }

            var cacheKey = string.Format(
                CultureInfo.InvariantCulture, 
                "{0}{1}_{2}_{3}", 
                Utility.CacheKeyPublishPopularTags, 
                tagIds, 
                selectTop, 
                portalId);

            return Utility.GetValueFromCache(portalId, cacheKey, () => DataProvider.Instance().GetPopularTags(portalId, tagList, selectTop));
        }

        public static int GetPopularTagsCount(int portalId, ArrayList tagList, bool selectTop)
        {
            // TODO: change tagList to a <List> of strings
            var sb = new StringBuilder(50);

            if (tagList != null)
            {
                foreach (int tag in tagList)
                {
                    sb.Append(tag.ToString());
                    sb.Append("_");
                }
            }

            var cacheKey = string.Format(
                CultureInfo.InvariantCulture, 
                "{0}{1}_{2}_{3}", 
                Utility.CacheKeyPublishPopularTagsCount, 
                sb, 
                selectTop, 
                portalId);


            return Utility.GetValueFromCache(portalId, cacheKey, () => DataProvider.Instance().GetPopularTagsCount(portalId, tagList, selectTop));
        }

        /// <summary>
        /// Gets a specific tag for a specific PortalId
        /// </summary>
        /// <param name="tag">The tag string.</param>
        /// <param name="portalId">The Portal Id.</param>
        public static Tag GetTag(string tag, int portalId)
        {
            var cacheKey = string.Format(CultureInfo.InvariantCulture, "{0}{1}_{2}", Utility.CacheKeyPublishTag, tag, portalId);
            return Utility.GetValueFromCache(portalId, cacheKey, () => new Tag(DataProvider.Instance().GetTag(tag, portalId)));
        }

        /// <summary>
        /// Gets a specific tag for a specific PortalId
        /// </summary>
        [Obsolete("Use GetTag(int, int) so that caching can be used")]
        public static Tag GetTag(int tagId)
        {
            return new Tag(DataProvider.Instance().GetTag(tagId));
        }

        public static Tag GetTag(int tagId, int portalId)
        {
            var cacheKey = string.Format(CultureInfo.InvariantCulture, "{0}{1}_{2}", Utility.CacheKeyPublishTagById, tagId, portalId);
            return Utility.GetValueFromCache(portalId, cacheKey, () => new Tag(DataProvider.Instance().GetTag(tagId)));
        }

        /// <summary>
        /// Gets all Tags for a specific PortalId
        /// </summary>
        /// <param name="portalId">The Portal Id.</param>
        public static DataTable GetTags(int portalId)
        {
            var cacheKey = Utility.CacheKeyPublishGetTagsByPortal + portalId.ToString(CultureInfo.InvariantCulture);
            return Utility.GetValueFromCache(portalId, cacheKey, () => DataProvider.Instance().GetTags(portalId));
        }

        /// <summary>
        /// Gets all tags that match a portion of a particular string for a portal
        /// </summary>
        /// <param name="partialTag">The string to start matching from. </param>
        /// <param name="portalId">The Portal Id.</param>
        public static DataTable GetTagsByString(string partialTag, int portalId)
        {
            var cacheKey = string.Format(CultureInfo.InvariantCulture, "{0}{1}_{2}", Utility.CacheKeyPublishGetTagsByString, partialTag, portalId);
            return Utility.GetValueFromCache(portalId, cacheKey, () => DataProvider.Instance().GetTagsByString(partialTag, portalId));
        }

        public static ArrayList ParseTags(string tags, int portalId)
        {
            return ParseTags(tags, portalId, Utility.GetTagSeparators(), true);
        }

        public static ArrayList ParseTags(string tags, int portalId, char[] separators, bool add)
        {
            var tagList = new ArrayList();
            string[] splitList = tags.Trim().Split(separators);
            foreach (string sTag in splitList)
            {
                if (sTag.Trim().Length > 0)
                {
                    Tag t = GetTag(sTag.Trim(), portalId);
                    if (t.TagId == 0 && add)
                    {
                        t.Name = HttpUtility.UrlDecode(sTag).Trim();
                        t.PortalId = portalId;

                        // TODO: localize this
                        t.Description = "Added by article edit";
                        t.TotalItems = 0;
                        t.Save();
                    }

                    if (t.TagId > 0)
                    {
                        tagList.Add(t);
                    }
                }
            }

            return tagList;
        }

        public void Save()
        {
            if (this.TagId == 0)
            {
                this.TagId = DataProvider.Instance().AddTag(this);
            }
            else
            {
                DataProvider.Instance().UpdateTag(this);
            }
        }

        public void Save(IDbTransaction trans)
        {
            if (this.TagId == 0)
            {
                this.TagId = DataProvider.Instance().AddTag(trans, this);
            }
            else
            {
                DataProvider.Instance().UpdateTag(trans, this);
            }
        }
    }
}