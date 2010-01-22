//Engage: Publish - http://www.engagesoftware.com
//Copyright (c) 2004-2010
//by Engage Software ( http://www.engagesoftware.com )

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
//TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
//THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
//CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
//DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.Portability
{
    using System;
    using System.IO;
    using DotNetNuke.Common;

    public class TransportableXmlValidator : XmlValidatorBase
    {

        public override bool Validate(Stream xmlStream)
        {
            bool valid = false;

            try
            {
                string path = Globals.ApplicationMapPath;
                string desktopFolder = Util.Utility.DesktopModuleFolderName.Replace("/", @"\");
                string fullPath  = path + desktopFolder + "Content.Publish.xsd";         
                SchemaSet.Add("", fullPath);
                valid = base.Validate(xmlStream);
            }
            catch(Exception ex)
            {
                Errors.Add(ex.ToString());
            }

            return valid;
        }

        public bool Validate(Stream xmlStream, string schemaFile)
        {
            SchemaSet.Add("", schemaFile);
            return base.Validate(xmlStream);
        }
    }
}