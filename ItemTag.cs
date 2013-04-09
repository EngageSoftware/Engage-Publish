// <copyright file="ItemTag.cs" company="Engage Software">
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
    using System.Diagnostics;
    using System.Globalization;

    using DotNetNuke.Common.Utilities;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Util;

    /// <summary>
    /// Summary description for ItemRelationship.
    /// </summary>
    public class ItemTag
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _itemVersionId = -1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _tagId = -1;

        public int ItemVersionId
        {
            [DebuggerStepThrough]
            get { return this._itemVersionId; }
            [DebuggerStepThrough]
            set { this._itemVersionId = value; }
        }

        public int TagId
        {
            [DebuggerStepThrough]
            get { return this._tagId; }
            [DebuggerStepThrough]
            set { this._tagId = value; }
        }

        public static void AddItemTag(int itemVersionId, int tagId)
        {
            DataProvider.Instance().AddItemTag(itemVersionId, tagId);
        }

        public static void AddItemTag(IDbTransaction trans, int itemVersionId, int tagId)
        {
            DataProvider.Instance().AddItemTag(trans, itemVersionId, tagId);
        }

        public static bool CheckItemTag(IDbTransaction trans, int itemId, int tagId)
        {
            return DataProvider.Instance().CheckItemTag(trans, itemId, tagId) > 0;
        }

        public static bool CheckItemTag(int itemId, int tagId)
        {
            return DataProvider.Instance().CheckItemTag(itemId, tagId) > 0;
        }

        public static ItemTag Create()
        {
            return new ItemTag();
        }

        public static ArrayList GetItemTags(int itemVersionId)
        {
            return CBO.FillCollection(DataProvider.Instance().GetItemTags(itemVersionId), typeof(ItemTag));
        }

        public static ArrayList GetItemTags(int itemVersionId, int portalId)
        {
            string cacheKey = Utility.CacheKeyPublishArticleTags + itemVersionId.ToString(CultureInfo.InvariantCulture) + "_" +
                              portalId.ToString(CultureInfo.InvariantCulture);
            ArrayList al;
            if (ModuleBase.UseCachePortal(portalId))
            {
                var o = DataCache.GetCache(cacheKey);
                if (o != null)
                {
                    al = (ArrayList)o;
                }
                else
                {
                    al = CBO.FillCollection(DataProvider.Instance().GetItemTags(itemVersionId), typeof(ItemTag));
                }

                if (al != null)
                {
                    DataCache.SetCache(cacheKey, al, DateTime.Now.AddMinutes(ModuleBase.CacheTimePortal(portalId)));
                    Utility.AddCacheKey(cacheKey, portalId);
                }
            }
            else
            {
                al = CBO.FillCollection(DataProvider.Instance().GetItemTags(itemVersionId), typeof(ItemTag));
            }

            return al;

            // return CBO.FillCollection(DataProvider.Instance().GetItemTags(_itemVersionId), typeof(ItemTag));
        }
    }
}