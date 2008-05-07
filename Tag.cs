using System;
using System.Data;
using Engage.Dnn.Publish.Data;
using System.Collections;
using System.Globalization;
using System.Xml.Serialization;

namespace Engage.Dnn.Publish
{
    [XmlRootAttribute(ElementName = "Tag", IsNullable = false)]
    public class Tag
    {
        //TODO: Create SQL and Dataprovider for TAGS

        #region Private Variables
        //attributes hide private members from debugger, so both properties and members aren't shown - BD
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int tagId;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string name;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string description;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int totalItems;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private DateTime mostRecentDate;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int languageId;
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private DateTime createdDate;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private int portalId;

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the tagId of the tag
        /// </summary>
        /// <value>The tag id of the tag.</value>
        public int? TagId
        {
         [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return tagId;
            }
        }

         /// <summary>
        /// Gets or sets the tag name for this <see cref="Tag"/> instance.
        /// </summary>
        /// <value>The Name of the Tag.</value>
        public String Name
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return name;
            }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Gets or sets the tag description for this <see cref="Tag"/> instance.
        /// </summary>
        /// <value>The description of the Tag.</value>
        public String Description
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return description;
            }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set
            {
                description = value;
            }
        }
        
        
        /// <summary>
        /// Gets or sets the total number of items tagged with this <see cref="Tag"/> instance.
        /// </summary>
        /// <value>The Total Items Tag.</value>
        public int TotalItems
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return totalItems;
            }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set
            {
                totalItems = value;
            }
        }

        /// <summary>
        /// Gets the date an item was last tagged.
        /// </summary>
        /// <value>The creation date of this rating.</value>
        public DateTime MostRecentDate
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return mostRecentDate;
            }
        }
        

         /// <summary>
        /// Gets or sets the language id for this <see cref="Tag"/> instance.
        /// </summary>
        /// <value>The Language Id</value>
        public int LanguageId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return languageId;
            }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set
            {
                languageId = value;
            }
        }


        /// <summary>
        /// Gets the date this tag was created.
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



        /// <summary>
        /// Gets or sets the PortalId this <see cref="Tag"/> instance.
        /// </summary>
        /// <value>The Language Id</value>
        public int PortalId
        {
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            get
            {
                return portalId;
            }
            [System.Diagnostics.DebuggerStepThroughAttribute()]
            set
            {
                portalId = value;
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
            name = tagName;
            description = tagDescription;
            totalItems= tagTotalItems;
            mostRecentDate = DateTime.Now;
        }


        //TODO: create add tag



        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class.
        /// </summary>
        /// <param name="dt">Data table of the tag instance info.</param>
        public Tag(DataTable dt)
        {
            //Make sure that we have at least 1 row returned. If for some reason we have more than one we only use the first one.
            if (dt.Rows.Count > 0)
            {
                tagId = Convert.ToInt32(dt.Rows[0]["tagId"], CultureInfo.InvariantCulture);
                name = dt.Rows[0]["name"].ToString();
                description = dt.Rows[0]["description"].ToString();
                totalItems = Convert.ToInt32(dt.Rows[0]["totalItems"], CultureInfo.InvariantCulture);
                mostRecentDate = Convert.ToDateTime(dt.Rows[0]["mostRecentDate"], CultureInfo.InvariantCulture);
                languageId = Convert.ToInt32(dt.Rows[0]["languageid"], CultureInfo.InvariantCulture);
                createdDate = Convert.ToDateTime(dt.Rows[0]["datecreated"], CultureInfo.InvariantCulture);
            }
        }
        #endregion

        #region Static methods

        /// <summary>
        /// Gets a specific tag for a specific PortalId
        /// </summary>
        /// <param name="tag">The tag string.</param>
        /// <param name="portalId">The Portal Id.</param>
        public static DataTable GetTag(string tag, int portalId)
        {
            return DataProvider.Instance().GetTag(tag, portalId);
        }

        /// <summary>
        /// Gets a specific tag for a specific PortalId
        /// </summary>
        public static Tag GetTag(int tagId)
        {
            return new Tag(DataProvider.Instance().GetTag(tagId));
        }

        /// <summary>
        /// Gets all Tags for a specific PortalId
        /// </summary>
        /// <param name="portalId">The Portal Id.</param>
        public static DataTable GetTags(int portalId)
        {
            //parse through the table and create each tag?

            return DataProvider.Instance().GetTags(portalId);
        }

        /// <summary>
        /// Gets all tags that match a portion of a particular string for a portal
        /// </summary>
        /// <param name="partialTag">The string to start matching from. </param>
        /// <param name="portalId">The Portal Id.</param>
        public static DataTable GetTagsByString(string partialTag, int portalId)
        {
            //parse through the table and create each tag?

            return DataProvider.Instance().GetTagsByString(partialTag, portalId);
        }

        public static DataTable GetPopularTags(int portalId, ArrayList tagList, bool selectTop)
        {
            //parse through the table and create each tag?
            return DataProvider.Instance().GetPopularTags(portalId, tagList, selectTop);
        }

        public static int GetPopularTagsCount(int portalId, ArrayList tagList, bool selectTop)
        {
            //parse through the table and create each tag?
            return DataProvider.Instance().GetPopularTagsCount(portalId, tagList, selectTop);
        }

        public static ArrayList ParseTags(string tags, int portalId)
        {
            return ParseTags(tags, portalId, Util.Utility.GetTagSeparators(), true);
        }

        public static ArrayList ParseTags(string tags, int portalId, char[] separators, bool add)
        {
            ArrayList tagList = new ArrayList();
            string[] splitList = tags.Trim().Split(separators);
            foreach (string sTag in splitList)
            {
                if (sTag.Trim().Length > 0)
                {
                    Tag t = new Tag(GetTag(sTag.Trim(), portalId));
                    if (t.tagId == 0 && add)
                    {
                        t.Name = sTag.Trim();
                        t.PortalId = portalId;
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
        {
            return DataProvider.Instance().GetItemsFromTags(portalId, tagList);
        }
       
        #endregion

        public void Save()
        {
            if (this.TagId == 0)
            {
                this.tagId = DataProvider.Instance().AddTag(this);
            }
            else
            {
                DataProvider.Instance().UpdateTag(this);
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
//        public static int AddItemTag(int itemVersionId, int? tagId)
//        {
//            return DataProvider.Instance().AddItemTag(itemVersionId, tagId);
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