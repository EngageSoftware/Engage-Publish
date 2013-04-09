// <copyright file="FeaturesController.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2013
// by Engage Software ( http://www.engagesoftware.com )
// </copyright>
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

namespace Engage.Dnn.Publish.TextHTML
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Xml.XPath;

    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Search;

    using Engage.Dnn.Publish.Portability;
    using Engage.Dnn.Publish.Util;

    /// <summary>
    /// Features Controller Class supports <see cref="ISearchable"/> and <see cref="IPortable"/> currently.
    /// </summary>
    public class FeaturesController : ISearchable, IPortable
    {
        /// <summary>
        /// Method is invoked when portal template is created or user selects Export Content from menu.
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public string ExportModule(int moduleId)
        {
            bool exportAll = false;

            // check query string for a "All" param to signal all rows, not just for a moduleId
            if (HttpContext.Current != null && HttpContext.Current.Request.QueryString["all"] != null)
            {
                exportAll = true;
            }

            XmlTransporter builder;
            try
            {
                builder = new XmlTransporter(moduleId);
                XmlDirector.Construct(builder, exportAll);
            }
            catch (Exception e)
            {
                Exceptions.LogException(new Exception(e.ToString()));
                throw;
            }

            return builder.Document.OuterXml;
        }

        /// <summary>
        /// Method is invoked when portal template is imported or user selects Import content from menu.
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="content"></param>
        /// <param name="version"></param>
        /// <param name="userId"></param>
        public void ImportModule(int moduleId, string content, string version, int userId)
        {
            // TODO: implement Import for Text/HTML modules
            var validator = new TransportableXmlValidator();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));

            if (validator.Validate(stream) == false)
            {
                var invalidXml = new Exception("Unable to import publish content due to incompatible XML file. Error: " + validator.Errors[0]);
                Exceptions.LogException(invalidXml);
                throw invalidXml;
            }

            // The DNN ValidatorBase closes the stream? Must re-create. hk
            stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var doc = new XPathDocument(stream);
            var builder = new XmlTransporter(moduleId);

            // TODO: we need to set the ItemID setting to be this recently imported article
            try
            {
                XmlDirector.Deconstruct(builder, doc);
            }
            catch (Exception e)
            {
                Exceptions.LogException(new Exception(e.ToString()));
                throw;
            }
        }

        public SearchItemInfoCollection GetSearchItems(ModuleInfo modInfo)
        {
            return new SearchProvider(modInfo, false).GetSearchItems();
        }
    }
}