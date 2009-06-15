//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.
using System;
using System.Data;
using System.Globalization;
using System.Reflection;
using Engage.Dnn.Publish.Data;

namespace Engage.Dnn.Publish.Util
{
	/// <summary>
	/// Summary description for RelationshipType.
	/// </summary>
	public class RelationshipType
	{
		private string name = string.Empty;
		private int id = -1;

		public static readonly RelationshipType ItemToParentCategory = new RelationshipType("Item To Parent Category");
		public static readonly RelationshipType CategoryToTopLevelCategory = new RelationshipType("Category To Top Level Category");
		public static readonly RelationshipType ItemToSpecialContentArticle = new RelationshipType("Article to Special Content Article");
		public static readonly RelationshipType ItemToRelatedCategory = new RelationshipType("Item To Related Category");
		public static readonly RelationshipType ItemToRelatedArticle = new RelationshipType("Item To Related Article");
		public static readonly RelationshipType ItemToRelatedDocument = new RelationshipType("Item To Related Document");
		public static readonly RelationshipType ItemToRelatedProduct = new RelationshipType("Item To Related Product");
		public static readonly RelationshipType ItemToRelatedMedia = new RelationshipType("Item To Related Media");
		public static readonly RelationshipType ItemToVideo = new RelationshipType("Item To Video");
		public static readonly RelationshipType ItemToArticleLinks = new RelationshipType("Item To Article Links");
		public static readonly RelationshipType ItemToFeaturedItem = new RelationshipType("Item To Featured Item");
    	public static readonly RelationshipType MediaToMediaLargeImage = new RelationshipType("Media to Media Large Image");
        
		//special content displayed in top right corner of an article, links, etc
		public static readonly RelationshipType ItemToSlideshowImage = new RelationshipType("Item To Slideshow Image");

		private RelationshipType(string name)
		{
			this.name = name;
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a simple/cheap operation")]
        public int GetId()
		{
			if (this.id == -1)
			{
				IDataReader dr = null;

				try
				{
					dr = DataProvider.Instance().GetRelationshipType(this.name);
					if (dr.Read())
					{
						this.id = Convert.ToInt32(dr["RelationshipTypeID"], CultureInfo.InvariantCulture);
					}
				}
				finally
				{
					if (dr != null) dr.Close();
				}
			}

			return this.id;
		}

        public static RelationshipType GetFromId(int id, Type ct)
        {
            if (ct == null) throw new ArgumentNullException("ct");
            if (id < 1) throw new ArgumentOutOfRangeException("id");

            Type type = ct;
            while (type.BaseType != null)
            {
                FieldInfo[] fi = type.GetFields();

                foreach (FieldInfo f in fi)
                {
                    RelationshipType cot = f.GetValue(type) as RelationshipType;
                    if (cot != null)
                    {
                        //this prevents old, bogus classes defined in the code from killing the app
                        //client needs to check the return value
                        try
                        {
                            if (id == cot.GetId())
                            {
                                return cot;
                            }
                        }

                        catch (Exception)
                        {
                            //drive on
                        }
                    }
                }

                type = type.BaseType; //check the super type 
            }

            return null;
        }

        public static RelationshipType GetFromName(string name, Type ct)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            if (ct == null) throw new ArgumentNullException("ct");

            Type type = ct;
            while (type.BaseType != null)
            {
                FieldInfo[] fi = type.GetFields();

                foreach (FieldInfo f in fi)
                {
                    RelationshipType cot = f.GetValue(type) as RelationshipType;
                    if (cot != null)
                    {
                        //this prevents old, bogus classes defined in the code from killing the app
                        //client needs to check the return value
                        try
                        {
                            if (name.Equals(cot.name, StringComparison.OrdinalIgnoreCase))
                            {
                                return cot;
                            }
                        }
                        catch (Exception)
                        {
                            //drive on
                        }
                    }
                }

                type = type.BaseType; //check the super type 
            }

            return null;
        }
	}
}

