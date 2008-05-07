using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.XPath;

using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using Engage.Dnn.Publish.Portability;

namespace Engage.Dnn.Publish.Util
{

    /// <summary>
    /// Features Controller Class supports IPortable currently.
    /// </summary>
    public class FeaturesController : IPortable
    {

        public FeaturesController()
        {

        }

        #region IPortable Members

        /// <summary>
        /// Method is invoked when portal template is imported or user selects Import Content from menu.
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <param name="Content"></param>
        /// <param name="Version"></param>
        /// <param name="UserID"></param>
        public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        {
     
            TransportableXmlValidator validator = new TransportableXmlValidator();
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(Content));

            if (validator.Validate(stream) == false)
            {
                Exception invalidXml = new Exception("Unable to import publish content due to incompatible XML file. Error: " + validator.Errors[0].ToString());
                Exceptions.LogException(invalidXml);
                throw invalidXml;
            }

            //The DNN ValidatorBase closes the stream? Must re-create. hk
            stream = new MemoryStream(Encoding.UTF8.GetBytes(Content));
            XPathDocument doc = new XPathDocument(stream);
            XmlTransporter builder = new XmlTransporter(ModuleID);

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


        /// <summary>
        /// Method is invoked when portal template is created or user selects Export Content from menu.
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <returns></returns>
        public string ExportModule(int ModuleID)
        {
            bool exportAll = false;

            //check query string for a "All" param to signal all rows, not just for a moduleId
            if (HttpContext.Current != null && HttpContext.Current.Request.QueryString["all"] != null)
            {
                exportAll = true;
            }
            XmlTransporter builder = null;
            try
            {
                builder = new XmlTransporter(ModuleID);
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

     
    }
}
