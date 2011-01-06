//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2011
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
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Engage.Dnn.Publish.Data;

    /// <summary>
    /// Summary description for RelationshipType.
    /// </summary>
    public class TopLevelCategoryItemType
    {
        public static readonly TopLevelCategoryItemType Category = new TopLevelCategoryItemType("Category");

        private readonly string _name = string.Empty;

        private int _id = -1;

        private TopLevelCategoryItemType(string name)
        {
            this._name = name;
        }

        public string Name
        {
            get { return this._name; }
        }

        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a simple/cheap operation")]
        public int GetId()
        {
            if (this._id == -1)
            {
                IDataReader dr = null;

                try
                {
                    dr = DataProvider.Instance().GetTopLevelCategoryItem(this._name);
                    if (dr.Read())
                    {
                        this._id = Convert.ToInt32(dr["ItemId"], CultureInfo.InvariantCulture);
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }
            }

            return this._id;
        }
    }
}