//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System.Collections;
using System.Data;
using System.Diagnostics;
using DotNetNuke.Common.Utilities;
using Engage.Dnn.Publish.Data;

namespace Engage.Dnn.Publish
{
	/// <summary>
	/// Summary description for ItemRelationship.
	/// </summary>
	public class ItemTag
	{
		#region Properties

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int _itemVersionId = -1;
		public int ItemVersionId 
		{
            [DebuggerStepThrough]
            get { return this._itemVersionId; }
            [DebuggerStepThrough]
            set { this._itemVersionId = value; }
		}

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int _tagId = -1;
		public int TagId 
		{
            [DebuggerStepThrough]
			get {return this._tagId;}
            [DebuggerStepThrough]
            set { this._tagId = value; }
		}

		#endregion

		public static ArrayList GetItemTags(int itemVersionId)
		{
            return CBO.FillCollection(DataProvider.Instance().GetItemTags(itemVersionId), typeof(ItemTag));
        }

		public ItemTag()
		{
			//this.startDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
		}

        public static ItemTag Create()
		{
			return new ItemTag();
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
	}
}

