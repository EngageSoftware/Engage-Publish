//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.


namespace Engage.Dnn.Publish
{
    using System.Collections.ObjectModel;

	/// <summary>
	/// Summary description for ItemRelationshipCollection.
	/// </summary>
    public class ItemVersionSettingCollection : Collection<ItemVersionSetting>
	{
        //public void Add(ItemVersionSetting r)
        //{
        //    Debug.Assert(r != null, "r cannot be null");
        //    base.InnerList.Add(r);
        //}

        //public void Remove(ItemVersionSetting r)
        //{
        //    base.InnerList.Remove(r);
        //}

        //public ItemVersionSetting this[int index]
        //{
        //    get { return (ItemVersionSetting)base.InnerList[index]; }
        //}

        public ItemVersionSetting this[ItemVersionSetting index]
        {
            get
            {
                int settingIndex = IndexOf(index);
                return this[settingIndex];
            }
        }
	}
}

