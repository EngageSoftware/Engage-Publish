//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Util
{
    using System;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Reflection;

    using Engage.Dnn.Publish.Data;

    /// <summary>
    /// Summary description for RelationshipType.
    /// </summary>
    public class RelationshipType
    {
        public static readonly RelationshipType CategoryToTopLevelCategory = new RelationshipType("Category To Top Level Category");

        public static readonly RelationshipType ItemToArticleLinks = new RelationshipType("Item To Article Links");

        public static readonly RelationshipType ItemToFeaturedItem = new RelationshipType("Item To Featured Item");

        public static readonly RelationshipType ItemToParentCategory = new RelationshipType("Item To Parent Category");

        public static readonly RelationshipType ItemToRelatedArticle = new RelationshipType("Item To Related Article");

        public static readonly RelationshipType ItemToRelatedCategory = new RelationshipType("Item To Related Category");

        public static readonly RelationshipType ItemToRelatedDocument = new RelationshipType("Item To Related Document");

        public static readonly RelationshipType ItemToRelatedMedia = new RelationshipType("Item To Related Media");

        public static readonly RelationshipType ItemToRelatedProduct = new RelationshipType("Item To Related Product");

        public static readonly RelationshipType ItemToSlideshowImage = new RelationshipType("Item To Slideshow Image");

        public static readonly RelationshipType ItemToSpecialContentArticle = new RelationshipType("Article to Special Content Article");

        public static readonly RelationshipType ItemToVideo = new RelationshipType("Item To Video");

        public static readonly RelationshipType MediaToMediaLargeImage = new RelationshipType("Media to Media Large Image");

        private readonly string _name = string.Empty;

        private int _id = -1;

        // special content displayed in top right corner of an article, links, etc
        private RelationshipType(string name)
        {
            this._name = name;
        }

        public static RelationshipType GetFromId(int id, Type ct)
        {
            if (ct == null)
            {
                throw new ArgumentNullException("ct");
            }

            if (id < 1)
            {
                throw new ArgumentOutOfRangeException("id");
            }

            Type type = ct;
            while (type.BaseType != null)
            {
                FieldInfo[] fi = type.GetFields();

                foreach (FieldInfo f in fi)
                {
                    var cot = f.GetValue(type) as RelationshipType;
                    if (cot != null)
                    {
                        // this prevents old, bogus classes defined in the code from killing the app
                        // client needs to check the return value
                        try
                        {
                            if (id == cot.GetId())
                            {
                                return cot;
                            }
                        }
                        catch
                        {
                            // drive on
                        }
                    }
                }

                type = type.BaseType; // check the super type 
            }

            return null;
        }

        public static RelationshipType GetFromName(string name, Type ct)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            if (ct == null)
            {
                throw new ArgumentNullException("ct");
            }

            Type type = ct;
            while (type.BaseType != null)
            {
                FieldInfo[] fi = type.GetFields();

                foreach (FieldInfo f in fi)
                {
                    var cot = f.GetValue(type) as RelationshipType;
                    if (cot != null)
                    {
                        // this prevents old, bogus classes defined in the code from killing the app
                        // client needs to check the return value
                        try
                        {
                            if (name.Equals(cot._name, StringComparison.OrdinalIgnoreCase))
                            {
                                return cot;
                            }
                        }
                        catch
                        {
                            // drive on
                        }
                    }
                }

                type = type.BaseType; // check the super type 
            }

            return null;
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a simple/cheap operation")]
        public int GetId()
        {
            if (this._id == -1)
            {
                IDataReader dr = null;

                try
                {
                    dr = DataProvider.Instance().GetRelationshipType(this._name);
                    if (dr.Read())
                    {
                        this._id = Convert.ToInt32(dr["RelationshipTypeID"], CultureInfo.InvariantCulture);
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }
            }

            return this._id;
        }
    }
}