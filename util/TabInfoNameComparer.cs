// <copyright file="TabInfoNameComparer.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Util
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;

    using DotNetNuke.Entities.Tabs;

    /// <summary>
    /// Summary description for TabInfoNameComparer.
    /// </summary>
    internal class TabInfoNameComparer : IComparer
    {
        private readonly StringComparison _comparison;

        private readonly bool _descending;

        [SuppressMessage("Microsoft.Globalization", "CA1307:SpecifyStringComparison", 
            MessageId = "Engage.Dnn.Publish.Util.TabInfoNameComparer.#ctor(System.Boolean)", 
            Justification = "delegated through a call that uses a StringComparer")]
        public TabInfoNameComparer()
            : this(false)
        {
        }

        public TabInfoNameComparer(bool descending)
            : this(descending, StringComparison.CurrentCulture)
        {
        }

        public TabInfoNameComparer(bool descending, StringComparison provider)
        {
            this._descending = descending;
            this._comparison = provider;
        }

        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", 
            MessageId = "System.ArgumentException.#ctor(System.String)", Justification = "Message is for internal use only")]
        public int Compare(object x, object y)
        {
            if (x == null && y == null)
            {
                return 0;
            }

            var t1 = x as TabInfo;
            var t2 = y as TabInfo;

            if (t1 == null)
            {
                throw new ArgumentException("x is not an instance of TabInfo");
            }

            if (t2 == null)
            {
                throw new ArgumentException("y is not an instance of TabInfo");
            }

            // compare the name of the TabInfo objects
            return this._descending
                       ? string.Compare(t1.TabName, t2.TabName, this._comparison)
                       : string.Compare(t2.TabName, t1.TabName, this._comparison);
        }
    }
}