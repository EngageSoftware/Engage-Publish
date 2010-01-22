//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
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
    using System.Globalization;
    using System.Reflection;
    using DotNetNuke.Common.Utilities;
    using Data;

    /// <summary>
    /// Summary description for RelationshipType.
    /// </summary>
    /// <remarks>This class should remain public, it is used by the Publish TreeView module</remarks>
    public class ItemType
    {
        private readonly string _name = string.Empty;
        private readonly Type _itemType;

        private int _id = -1;

        public static readonly ItemType Category = new ItemType("Category", typeof(Category));
        //public static readonly ItemType Product = new ItemType("Product", typeof(Product));
        public static readonly ItemType Article = new ItemType("Article", typeof(Article));
        public static readonly ItemType TopLevelCategory = new ItemType("TopLevelCategory", typeof(TopLevelCategoryItemType));
        //public static readonly ItemType Document = new ItemType("Document", typeof(Document));
        //public static readonly ItemType Media = new ItemType("Media", typeof(Media));

        private ItemType(string name, Type itemType)
        {
            _itemType = itemType;
            _name = name;
        }

        public Type GetItemType
        {
            get
            {
                return _itemType;
            }
        }

        public static string GetItemName(int itemId)
        {
            //get the item typeId
            return DataProvider.Instance().GetItemName(itemId);
        }


        public static ItemType GetFromId(int id, Type ct)
        {
            //TODO: can we cache this?
            if (ct == null)
                throw new ArgumentNullException("ct");
            if (id < 1)
                //throw new ArgumentOutOfRangeException("_id");
                return null;

            Type type = ct;
            while (type.BaseType != null)
            {
                FieldInfo[] fi = type.GetFields();

                foreach (FieldInfo f in fi)
                {
                    Object o = f.GetValue(type);
                    var cot = o as ItemType;
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

                        catch
                        {
                            //drive on
                        }
                    }
                }

                type = type.BaseType; //check the super type 
            }
            return null;
        }


        public static string GetItemTypeName(int itemTypeId)
        {
            //cache this
            return DataProvider.Instance().GetItemTypeName(itemTypeId);
        }

        public static string GetItemTypeName(int itemTypeId, bool useCache, int portalId, int cacheTime)
        {
           return DataProvider.Instance().GetItemTypeName(itemTypeId, useCache, portalId, cacheTime);
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a simple/cheap operation")]
        public int GetId()
        {
            //cache this
            if (_id == -1)
            {
                string cacheKey = Utility.CacheKeyPublishItemTypeId + _itemType;
                object o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    _id = (int)o;
                }
                else
                {
                    using( IDataReader dr = DataProvider.Instance().GetItemType(_name))
                    {
                        if (dr.Read())
                        {
                            _id = Convert.ToInt32(dr["ItemTypeID"], CultureInfo.InvariantCulture);
                            if (_id > 0)
                            {
                                DataCache.SetCache(cacheKey, _id, DateTime.Now.AddMinutes(15));
                                Utility.AddCacheKey(cacheKey, 0);
                            }
                        }
                    }
                }
            }

            return _id;
        }
    }
}

