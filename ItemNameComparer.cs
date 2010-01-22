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

    using System;
    using System.Collections;

	public class ItemNameComparer : IComparer
	{
		private readonly bool _descending;
	    private readonly StringComparison _comparison;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", MessageId = "Engage.Dnn.Publish.ItemNameComparer.#ctor(System.Boolean)", Justification = "delegated to a call that does use a StringComparer")]
        public ItemNameComparer() : this(false)
		{
		}

	    public ItemNameComparer(bool descending) : this(descending, StringComparison.CurrentCulture)
	    {
	    }

	    public ItemNameComparer(bool descending, StringComparison comparison)
	    {
	        _descending = descending;
	        _comparison = comparison;
	    }

	    #region IComparer Members

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)", Justification = "Message is for internal use only")]
        public int Compare(object x, object y)
		{
			if (x == null && y == null) return 0;

			var i1 = x as Item;
			var i2 = y as Item;

			if (i1 == null) throw new ArgumentException("item is not an instance of Item", "x");
			if (i2 == null) throw new ArgumentException("item is not an instance of Item", "y");

			return _descending ? string.Compare(i1.Name, i2.Name, _comparison) : string.Compare(i2.Name, i1.Name, _comparison);
		}

		#endregion
	}

}

