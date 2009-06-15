//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2009
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

using System;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace Engage.Dnn.Publish.Portability
{
    public abstract class TransportableElement
    {

        /// <summary>
        /// Method to convert a custom Object to XML string
        /// </summary>
        /// <returns>XML string</returns>
        public string SerializeObjectToXml()
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(this.GetType());
                StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
                xs.Serialize(writer, this);

                return writer.GetStringBuilder().ToString();
            }
            catch (Exception e)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(e);
                throw;
            }
        }
        
        public abstract void Import(int currentModuleId, int portalId);

    }
}
