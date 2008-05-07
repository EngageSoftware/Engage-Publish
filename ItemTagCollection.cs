//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2008
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System.Collections;
using System.Diagnostics;

namespace Engage.Dnn.Publish
{
	/// <summary>
    /// Summary description for ItemTagCollection.
	/// </summary>
	internal class ItemTagCollection : CollectionBase
	{
		public void Add(ItemTag r)
		{
			Debug.Assert(r != null, "r cannot be null");
			base.InnerList.Add(r);
		}

        public ItemTag this[int index]
		{
			get {return (ItemTag) base.InnerList[index];}
		}
	}
}

