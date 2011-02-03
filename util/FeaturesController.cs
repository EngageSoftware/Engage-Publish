// <copyright file="FeaturesController.cs" company="Engage Software">
// Engage: Publish
// Copyright (c) 2004-2011
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
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Xml.XPath;

    using DotNetNuke.Common.Utilities;
    using DotNetNuke.Entities.Modules;
    using DotNetNuke.Services.Exceptions;
    using DotNetNuke.Services.Search;

    using Engage.Dnn.Publish.Data;
    using Engage.Dnn.Publish.Portability;

    /// <summary>
    /// Features Controller implements <see cref="IPortable"/> and <see cref="ISearchable"/> 
    /// as the business controller class for the Engage: Publish and Tag Cloud desktop modules.
    /// </summary>
    public class FeaturesController : IPortable, ISearchable
    {
#if TRIAL

    /// <summary>
    /// The license key for this module
    /// </summary>
        public static readonly Guid ModuleLicenseKey = new Guid("3520E5F9-EDBB-46EA-A377-107306B828C4");
#endif

        #region IPortable Members

        /// <summary>
        /// Method is invoked when portal template is imported or user selects Import content from menu.
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="content"></param>
        /// <param name="version"></param>
        /// <param name="userId"></param>
        public void ImportModule(int moduleId, string content, string version, int userId)
        {
            try
            {
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
                XmlDirector.Deconstruct(builder, doc);
            }
            catch (Exception e)
            {
                Exceptions.LogException(e);
                throw;
            }
        }

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

        #endregion

        #region ISearchable Members

        public SearchItemInfoCollection GetSearchItems(ModuleInfo modInfo)
        {
            return new SearchProvider(modInfo, true).GetSearchItems();
        }

        #endregion
    }
}