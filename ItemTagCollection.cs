// <copyright file="ItemTagCollection.cs" company="Engage Software">
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
    using System.Collections;
    using System.Diagnostics;

    /// <summary>
    /// Summary description for ItemTagCollection.
    /// </summary>
    public class ItemTagCollection : CollectionBase
    {
        public ItemTag this[int index]
        {
            get { return (ItemTag)this.InnerList[index]; }
        }

        public void Add(ItemTag r)
        {
            Debug.Assert(r != null, "r cannot be null");
            this.InnerList.Add(r);
        }
    }
}