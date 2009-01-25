//Engage: Publish - http://www.engagemodules.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;

namespace Engage.Dnn.Publish.Util
{
	/// <summary>
	/// Summary description for QueryStringParameters.
	/// </summary>
	public class QueryStringParameters
	{
		private StringDictionary values;
		private const string ValueDelimiter = "&";


		public QueryStringParameters()
		{
			this.values = new StringDictionary();
		}

		public void Add(string name, object value)
		{
			if (name == null) throw new ArgumentNullException("name");
			if (value == null) throw new ArgumentNullException("value");
		
			this.values.Add(name, Convert.ToString(value, CultureInfo.InvariantCulture));
		}

		public void ClearKeys()
		{
			this.values.Clear();
		}

		public override string ToString()
		{
			if (this.values.Count < 1) return string.Empty;

			//create full query string
			//pagename?param1=value1&param2=value2,etc...
			StringBuilder sb = new StringBuilder(256);

			//add all the name/value pairs
			IEnumerator ie = this.values.Keys.GetEnumerator();
			while (ie.MoveNext())
			{
				string key = ie.Current.ToString();
				sb.Append(key);
				sb.Append("=");
		            
				//look up the value
				sb.Append(this.values[key]);
				sb.Append(ValueDelimiter);
			}
        
			//trim the trailing '&'
			sb.Length = (sb.Length - 1);

			return sb.ToString();
		}
	}
}

