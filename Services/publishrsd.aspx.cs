namespace Engage.Dnn.Publish.Services
{
    using System;
    using System.Globalization;
    using System.Text;
    using System.Web.UI;

    using DotNetNuke.Entities.Portals;

    using Engage.Dnn.Publish.Util;

    public partial class Publishrsd : Page
    {
        public string ApiLink;

        public string EngineName;

        public string EngineUrl;

        public string HomePageUrl;

        public int PortalId
        {
            get
            {
                string i = this.Request.Params["portalId"];
                if (i != null)
                {
                    return Convert.ToInt32(i, CultureInfo.InvariantCulture);
                }

                return -1;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Response.Clear();
            this.Response.ContentType = "text/xml";

            PortalSettings ps = Utility.GetPortalSettings(this.PortalId);

            object o = this.Request.QueryString["HomePageUrl"];
            if (o != null)
            {
                this.EngineName = "EngagePublish";
                this.EngineUrl = "http://www.engagesoftware.com/modules/engagepublish.aspx";
                this.ApiLink = "http://" + ps.PortalAlias.HTTPAlias + ModuleBase.DesktopModuleFolderName + "services/Metaweblog.ashx";

                this.HomePageUrl = o.ToString();
            }

            var responseStream = new StringBuilder(1000);
            responseStream.Append("<?xml version=\"1.0\"  encoding=\"utf-8\" ?><rsd version=\"1.0\" xmlns=\"http://archipelago.phrasewise.com/rsd\">");
            responseStream.Append("<service><engineName>");
            responseStream.Append(this.EngineName);
            responseStream.Append("</engineName><engineLink>");
            responseStream.Append(this.EngineUrl);
            responseStream.Append("</engineLink><homePageLink>");
            responseStream.Append(this.HomePageUrl);
            responseStream.Append("</homePageLink><apis><api name=\"MetaWeblog\" preferred=\"true\" apiLink=\"");
            responseStream.Append(this.ApiLink);
            responseStream.Append("\" blogID=\"0\" /></apis></service></rsd>");
            this.Response.Write(responseStream.ToString());
        }
    }
}