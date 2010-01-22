using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Xml.Serialization;
using DotNetNuke.Common.Utilities;
using Engage.Dnn.Publish.Data;
using Engage.Dnn.Publish.Util;

namespace Engage.Dnn.Publish
{
    using System.Diagnostics;
    using System.Web;
    using System.Text;

    [XmlRootAttribute(ElementName = "Tag", IsNullable = false)]
    public class Tag
    {

        #region Private Variables
        //attributes hide private members from debugger, so both properties and members aren't shown - BD
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _tagId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _name;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _description;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _totalItems;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DateTime _mostRecentDate;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _languageId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DateTime _createdDate;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _portalId;

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the _tagId of the tag
        /// </summary>
        /// <value>The tag id of the tag.</value>
        public int? TagId
        {
         [DebuggerStepThroughAttribute]
            get
            {
                return _tagId;
            }
        }

         /// <summary>
        /// Gets or sets the tag name for this <see cref="Tag"/> instance.
        /// </summary>
        /// <value>The Name of the Tag.</value>
        public String Name
        {
            [DebuggerStepThrough]
            get
            {
                return _name;
            }
            [DebuggerStepThroughAttribute]
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets the tag description for this <see cref="Tag"/> instance.
        /// </summary>
        /// <value>The description of the Tag.</value>
        public String Description
        {
            [DebuggerStepThroughAttribute]
            get
            {
                return _description;
            }
            [DebuggerStepThroughAttribute]
            set
            {
                _description = value;
            }
        }
        
        
        /// <summary>
        /// Gets or sets the total number of items tagged with this <see cref="Tag"/> instance.
        /// </summary>
        /// <value>The Total Items Tag.</value>
        public int TotalItems
        {
            [DebuggerStepThroughAttribute]
            get
            {
                return _totalItems;
            }
            [DebuggerStepThroughAttribute]
            set
            {
                _totalItems = value;
            }
        }

        /// <summary>
        /// Gets the date an item was last tagged.
        /// </summary>
        /// <value>The creation date of this rating.</value>
        public DateTime MostRecentDate
        {
            [DebuggerStepThroughAttribute]
            get
            {
                return _mostRecentDate;
            }
        }
        

         /// <summary>
        /// Gets or sets the language id for this <see cref="Tag"/> instance.
        /// </summary>
        /// <value>The Language Id</value>
        public int LanguageId
        {
            [DebuggerStepThroughAttribute]
            get
            {
                return _languageId;
            }
            [DebuggerStepThroughAttribute]
            set
            {
                _languageId = value;
            }
        }


        /// <summary>
        /// Gets the date this tag was created.
        /// </summary>
        /// <value>The creation date of this rating.</value>
        public DateTime CreatedDate
        {
            [DebuggerStepThroughAttribute]
            get
            {
                return _createdDate;
            }
        }



        /// <summary>
        /// Gets or sets the PortalId this <see cref="Tag"/> instance.
        /// </summary>
        /// <value>The Language Id</value>
        public int PortalId
        {
            [DebuggerStepThroughAttribute]
            get
            {
                return _portalId;
            }
            [DebuggerStepThroughAttribute]
            set
            {
                _portalId = value;
            }
        }


        #endregion

        #region .ctor
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
            _name = tagName;
            _description = tagDescription;
            _totalItems= tagTotalItems;
            _mostRecentDate = DateTime.Now;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class.
        /// </summary>
        /// <param name="dt">Data table of the tag instance info.</param>
        public Tag(DataTable dt)
        {
            //Make sure that we have at least 1 row returned. If for some reason we have more than one we only use the first one.
            if (dt.Rows.Count > 0)
            {
                _tagId = Convert.ToInt32(dt.Rows[0]["tagId"], CultureInfo.InvariantCulture);
                _name = dt.Rows[0]["name"].ToString();
                _description = dt.Rows[0]["description"].ToString();
                _totalItems = Convert.ToInt32(dt.Rows[0]["totalItems"], CultureInfo.InvariantCulture);
                _mostRecentDate = Convert.ToDateTime(dt.Rows[0]["mostRecentDate"], CultureInfo.InvariantCulture);
                _languageId = Convert.ToInt32(dt.Rows[0]["languageid"], CultureInfo.InvariantCulture);
                _createdDate = Convert.ToDateTime(dt.Rows[0]["datecreated"], CultureInfo.InvariantCulture);
            }
        }
        #endregion

        #region Static methods

        /// <summary>
        /// Gets a specific tag for a specific PortalId
        /// </summary>
        /// <param name="tag">The tag string.</param>
        /// <param name="portalId">The Portal Id.</param>
        public static Tag GetTag(string tag, int portalId)
        {
            //return DataProvider.Instance().GetTag(tag, _portalId);

            string cacheKey = Utility.CacheKeyPublishTag + tag.ToString(CultureInfo.InvariantCulture) + portalId.ToString(CultureInfo.InvariantCulture);
            DataTable dt;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    dt = (DataTable)o;
                }
                else
                {
                    dt = DataProvider.Instance().GetTag(tag, portalId);
                }
                if (dt != null)
                {
                    DataCache.SetCache(cacheKey, dt, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                dt = DataProvider.Instance().GetTag(tag, portalId);
            }
            return new Tag(dt);
        }

        /// <summary>
        /// Gets a specific tag for a specific PortalId
        /// </summary>
        public static Tag GetTag(int tagId)
        {
            return new Tag(DataProvider.Instance().GetTag(tagId));
        }

        public static Tag GetTag(int tagId, int portalId)
        {
            string cacheKey = Utility.CacheKeyPublishTagById + tagId.ToString(CultureInfo.InvariantCulture) + portalId.ToString(CultureInfo.InvariantCulture);
            DataTable dt;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    dt = (DataTable)o;
                }
                else
                {
                    dt = DataProvider.Instance().GetTag(tagId);
                }
                if (dt != null)
                {
                    DataCache.SetCache(cacheKey, dt, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                dt = DataProvider.Instance().GetTag(tagId);
            }
            return new Tag(dt);
        }
        

        /// <summary>
        /// Gets all Tags for a specific PortalId
        /// </summary>
        /// <param name="portalId">The Portal Id.</param>
        public static DataTable GetTags(int portalId)
        {
            //return DataProvider.Instance().GetTags(_portalId);

            string cacheKey = Utility.CacheKeyPublishGetTagsByPortal + portalId.ToString(CultureInfo.InvariantCulture);
            DataTable dt;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    dt = (DataTable)o;
                }
                else
                {
                    dt = DataProvider.Instance().GetTags(portalId);
                }
                if (dt != null)
                {
                    DataCache.SetCache(cacheKey, dt, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                dt = DataProvider.Instance().GetTags(portalId);
            }
            return dt;
        }

        /// <summary>
        /// Gets all tags that match a portion of a particular string for a portal
        /// </summary>
        /// <param name="partialTag">The string to start matching from. </param>
        /// <param name="portalId">The Portal Id.</param>
        public static DataTable GetTagsByString(string partialTag, int portalId)
        {
            //parse through the table and create each tag?

            //return DataProvider.Instance().GetTagsByString(partialTag, _portalId);
            string cacheKey = Utility.CacheKeyPublishGetTagsByString + partialTag.ToString(CultureInfo.InvariantCulture) + "_" + portalId.ToString(CultureInfo.InvariantCulture);
            DataTable dt;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    dt = (DataTable)o;
                }
                else
                {
                    dt = DataProvider.Instance().GetTagsByString(partialTag, portalId);
                }
                if (dt != null)
                {
                    DataCache.SetCache(cacheKey, dt, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                dt = DataProvider.Instance().GetTagsByString(partialTag, portalId);
            }
            return dt;
        }

        public static DataTable GetPopularTags(int portalId, ArrayList tagList, bool selectTop)
        {
            //TODO: change tagList to a <List> of strings
            //string tags = string.Empty;
            var sb = new StringBuilder(50);
            //if (tagList != null) tags = tagList.ToString().Replace(" ", string.Empty);

            if (tagList != null)
            {
                foreach (int tag in tagList)
                {
                    sb.Append(tag.ToString());
                    sb.Append("_");
                }
            }

            string cacheKey = Utility.CacheKeyPublishPopularTags + sb + selectTop.ToString(CultureInfo.InvariantCulture) + "_" + portalId.ToString(CultureInfo.InvariantCulture);
            DataTable dt;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    dt = (DataTable)o;
                }
                else
                {
                    dt = DataProvider.Instance().GetPopularTags(portalId, tagList, selectTop);
                }
                if (dt != null)
                {
                    DataCache.SetCache(cacheKey, dt, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                dt = DataProvider.Instance().GetPopularTags(portalId, tagList, selectTop);
            }
            return dt;
        }

        public static int GetPopularTagsCount(int portalId, ArrayList tagList, bool selectTop)
        {
            //TODO: change tagList to a <List> of strings
            //string tags = string.Empty;
            var sb = new StringBuilder(50);
            //if (tagList != null) tags = tagList.ToString().Replace(" ", string.Empty);

            if (tagList != null)
            {
                foreach (int tag in tagList)
                {
                    sb.Append(tag.ToString());
                    sb.Append("_");
                }
            }

            string cacheKey = Utility.CacheKeyPublishPopularTagsCount + sb + "_" + selectTop.ToString(CultureInfo.InvariantCulture) + "_" + portalId.ToString(CultureInfo.InvariantCulture);
            int tagCount;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    tagCount = (int)o;
                }
                else
                {
                    tagCount = DataProvider.Instance().GetPopularTagsCount(portalId, tagList, selectTop);
                }

                DataCache.SetCache(cacheKey, tagCount, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                Utility.AddCacheKey(cacheKey, portalId);
            }
            else
            {
                tagCount = DataProvider.Instance().GetPopularTagsCount(portalId, tagList, selectTop);
            }

            return tagCount;
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
                    if (t._tagId == 0 && add)
                    {
                        t.Name = HttpUtility.UrlDecode(sTag).Trim();
                        t.PortalId = portalId;
                        //TODO: localize this
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

        public static DataTable GetItemsFromTags(int portalId, ArrayList tagList)
        {   //return DataProvider.Instance().GetItemsFromTags(_portalId, tagList);
            var sb = new StringBuilder(50);
            //if (tagList != null) tags = tagList.ToString().Replace(" ", string.Empty);

            if (tagList != null)
            {
                foreach (int tag in tagList)
                {
                    sb.Append(tag.ToString());
                    sb.Append("_");
                }
            }
            
            string cacheKey = Utility.CacheKeyPublishItemsFromTags + sb + "_" + portalId.ToString(CultureInfo.InvariantCulture);
            DataTable dt;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    dt = (DataTable)o;
                }
                else
                {
                    dt = DataProvider.Instance().GetItemsFromTags(portalId, tagList);
                }
                if (dt != null)
                {
                    DataCache.SetCache(cacheKey, dt, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                dt = DataProvider.Instance().GetItemsFromTags(portalId, tagList);
            }
            return dt;
        }

        public static DataTable GetItemsFromTagsPaging(int portalId, ArrayList tagList, int maxItems, int pageId, string sortOrder)
        {
            //return DataProvider.Instance().GetItemsFromTagsPaging(_portalId, tagList, maxItems, pageId);

            var sb = new StringBuilder(50);
            //if (tagList != null) tags = tagList.ToString().Replace(" ", string.Empty);

            if (tagList != null)
            {
                foreach (int tag in tagList)
                {
                    sb.Append(tag.ToString());
                    sb.Append("_");
                }
            }


            string cacheKey = Utility.CacheKeyPublishItemsFromTagsPage + sb + "_" + pageId + "_" + portalId.ToString(CultureInfo.InvariantCulture);
            DataTable dt;
            if (ModuleBase.UseCachePortal(portalId))
            {
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    dt = (DataTable)o;
                }
                else
                {
                    dt = DataProvider.Instance().GetItemsFromTagsPaging(portalId, tagList, maxItems, pageId, sortOrder);
                }
                if (dt != null)
                {
                    DataCache.SetCache(cacheKey, dt, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                dt = DataProvider.Instance().GetItemsFromTagsPaging(portalId, tagList, maxItems, pageId, sortOrder);
            }
            return dt;
        }

        #endregion

        public void Save()
        {
            if (TagId == 0)
            {
                _tagId = DataProvider.Instance().AddTag(this);
            }
            else
            {
                DataProvider.Instance().UpdateTag(this);
            }
        }

        public void Save(IDbTransaction trans)
        {
            if (TagId == 0)
            {
                _tagId = DataProvider.Instance().AddTag(trans, this);
            }
            else
            {
                DataProvider.Instance().UpdateTag(trans, this);
            }
        }


//        #region Static Data Methods
//        /// <summary>
//        /// Adds an item to a tag.
//        /// </summary>
//        /// <param name="itemVersionId">The item version id.</param>
//        /// <param name="userId">The user id, or <c>null</c> if the user is unauthenticated.</param>
//        /// <param name="rating">The rating.</param>
//        /// <returns></returns>
//        public static int AddItemTag(int itemVersionId, int? _tagId)
//        {
//            return DataProvider.Instance().AddItemTag(itemVersionId, _tagId);
//        }

//        /// <summary>
//        /// Deletes all the ratings for a certain item version.
//        /// </summary>
//        /// <param name="itemVersionId">The item version id.</param>
//        public static void DeleteTags(int itemVersionId)
//        {
//            DataProvider.Instance().DeleteTags(itemVersionId);
//        }


//#endregion    
    }
}