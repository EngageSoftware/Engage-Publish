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
    using Data;

	/// <summary>
	/// Summary description for RelationshipType.
	/// </summary>
	public class TopLevelCategoryItemType
	{
		private readonly string _name = string.Empty;
		private int _id = -1;


		public static readonly TopLevelCategoryItemType Category = new TopLevelCategoryItemType("Category");

		private TopLevelCategoryItemType(string name)
		{
			_name = name;
		}

		public string Name
		{
			get {return _name;}
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a simple/cheap operation")]
        public int GetId()
		{
			if (_id == -1)
			{
				IDataReader dr = null;

				try
				{
					dr = DataProvider.Instance().GetTopLevelCategoryItem(_name);
					if (dr.Read())
					{
						_id = Convert.ToInt32(dr["ItemId"], CultureInfo.InvariantCulture);
					}
				}
				finally
				{
					if (dr != null) dr.Close();
				}
			}

			return _id;
		}
	}
}

