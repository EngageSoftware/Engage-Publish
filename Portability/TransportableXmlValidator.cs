using System;
using System.IO;
using DotNetNuke.Common;

namespace Engage.Dnn.Publish.Portability
{

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