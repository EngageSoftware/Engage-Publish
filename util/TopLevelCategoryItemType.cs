//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Data;
using Engage.Dnn.Publish.Data;
using System.Globalization;

namespace Engage.Dnn.Publish.Util
{
	/// <summary>
	/// Summary description for RelationshipType.
	/// </summary>
	public class TopLevelCategoryItemType
	{
		private string name = string.Empty;
		private int id = -1;


		public static readonly TopLevelCategoryItemType Category = new TopLevelCategoryItemType("Category");

		private TopLevelCategoryItemType(string name)
		{
			this.name = name;
		}

		public string Name
		{
			get {return this.name;}
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not a simple/cheap operation")]
        public int GetId()
		{
			if (this.id == -1)
			{
				IDataReader dr = null;

				try
				{
					dr = DataProvider.Instance().GetTopLevelCategoryItem(this.name);
					if (dr.Read())
					{
						this.id = Convert.ToInt32(dr["ItemId"], CultureInfo.InvariantCulture);
					}
				}
				finally
				{
					if (dr != null) dr.Close();
				}
			}

			return this.id;
		}
	}
}

