//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish
{
    using System.Collections;
    using System.Diagnostics;

    /// <summary>
    /// Summary description for ItemRelationshipCollection.
    /// </summary>
    public class ItemRelationshipCollection : CollectionBase
    {
        // public void Remove(ItemRelationship r)
        // {
        // base.InnerList.Remove(r);
        // }
        public ItemRelationship this[int index]
        {
            get { return (ItemRelationship)this.InnerList[index]; }
        }

        public void Add(ItemRelationship r)
        {
            Debug.Assert(r != null, "r cannot be null");
            this.InnerList.Add(r);
        }
    }
}